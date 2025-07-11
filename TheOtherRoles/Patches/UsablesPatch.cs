using System;
using System.Collections.Generic;
using System.Linq;
using AmongUs.GameOptions;
using HarmonyLib;
using Reactor.Utilities.Extensions;
using TheOtherRoles.CustomGameModes;
using TheOtherRoles.Utilities;
using TMPro;
using UnityEngine;
using static TheOtherRoles.TheOtherRoles;
using static TheOtherRoles.GameHistory;
using static TheOtherRoles.TORMapOptions;
using Object = UnityEngine.Object;
using TheOtherRoles.Modules;

namespace TheOtherRoles.Patches;

[HarmonyPatch(typeof(Vent), nameof(Vent.CanUse))]
public static class VentCanUsePatch
{
    public static bool Prefix(Vent __instance, ref float __result, [HarmonyArgument(0)] NetworkedPlayerInfo pc,
        [HarmonyArgument(1)] ref bool canUse, [HarmonyArgument(2)] ref bool couldUse)
    {
        if (GameOptionsManager.Instance.currentGameOptions.GameMode == GameModes.HideNSeek) return true;
        var num = float.MaxValue;
        var @object = pc.Object;

        var roleCouldUse = @object.roleCanUseVents();

        if (__instance.name.StartsWith("SealedVent_"))
        {
            canUse = couldUse = false;
            __result = num;
            return false;
        }

        // Submerged Compatability if needed:
        if (SubmergedCompatibility.IsSubmerged)
        {
            // as submerged does, only change stuff for vents 9 and 14 of submerged. Code partially provided by AlexejheroYTB
            if (SubmergedCompatibility.getInTransition())
            {
                __result = float.MaxValue;
                return canUse = couldUse = false;
            }

            switch (__instance.Id)
            {
                case 9: // Cannot enter vent 9 (Engine Room Exit Only Vent)!
                    if (PlayerControl.LocalPlayer.inVent) break;
                    __result = float.MaxValue;
                    return canUse = couldUse = false;
                case 14: // Lower Central
                    __result = float.MaxValue;
                    couldUse = roleCouldUse && !pc.IsDead && (@object.CanMove || @object.inVent);
                    canUse = couldUse;
                    if (canUse)
                    {
                        var center = @object.Collider.bounds.center;
                        var position = __instance.transform.position;
                        __result = Vector2.Distance(center, position);
                        canUse &= __result <= __instance.UsableDistance;
                    }

                    return false;
            }
        }

        var usableDistance = __instance.UsableDistance;
        if (__instance.name.StartsWith("JackInTheBoxVent_"))
        {
            if (Trickster.trickster != PlayerControl.LocalPlayer)
            {
                // Only the Trickster can use the Jack-In-The-Boxes!
                canUse = false;
                couldUse = false;
                __result = num;
                return false;
            }

            // Reduce the usable distance to reduce the risk of gettings stuck while trying to jump into the box if it's placed near objects
            usableDistance = 0.4f;
        }

        couldUse = (@object.inVent || roleCouldUse) && !pc.IsDead && (@object.CanMove || @object.inVent);
        canUse = couldUse;
        if (canUse)
        {
            var center = @object.Collider.bounds.center;
            var position = __instance.transform.position;
            num = Vector2.Distance(center, position);
            canUse &= num <= usableDistance &&
                      (!PhysicsHelpers.AnythingBetween(@object.Collider, center, position, Constants.ShipOnlyMask,
                          false) || __instance.name.StartsWith("JackInTheBoxVent_"));
        }

        __result = num;
        return false;
    }
}

[HarmonyPatch(typeof(VentButton), nameof(VentButton.DoClick))]
internal class VentButtonDoClickPatch
{
    private static bool Prefix(VentButton __instance)
    {
        // Manually modifying the VentButton to use Vent.Use again in order to trigger the Vent.Use prefix patch
        if (__instance.currentTarget != null && !Deputy.handcuffedKnows.ContainsKey(PlayerControl.LocalPlayer.PlayerId))
            __instance.currentTarget.Use();
        return false;
    }
}

[HarmonyPatch(typeof(Vent), nameof(Vent.Use))]
public static class VentUsePatch
{
    public static bool Prefix(Vent __instance)
    {
        if (GameOptionsManager.Instance.currentGameOptions.GameMode == GameModes.HideNSeek) return true;
        // Deputy handcuff disables the vents
        if (Deputy.handcuffedPlayers.Contains(PlayerControl.LocalPlayer.PlayerId))
        {
            Deputy.setHandcuffedKnows();
            return false;
        }

        if (Trapper.playersOnMap.Contains(PlayerControl.LocalPlayer.PlayerId)) return false;

        bool canUse;
        bool couldUse;
        __instance.CanUse(PlayerControl.LocalPlayer.Data, out canUse, out couldUse);
        var canMoveInVents = PlayerControl.LocalPlayer != Spy.spy &&
                             !Trapper.playersOnMap.Contains(PlayerControl.LocalPlayer.PlayerId);
        if (!canUse) return false; // No need to execute the native method as using is disallowed anyways

        var isEnter = !PlayerControl.LocalPlayer.inVent;

        if (__instance.name.StartsWith("JackInTheBoxVent_"))
        {
            __instance.SetButtons(isEnter && canMoveInVents);
            var writer =
                AmongUsClient.Instance.StartRpc(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.UseUncheckedVent);
            writer.WritePacked(__instance.Id);
            writer.Write(PlayerControl.LocalPlayer.PlayerId);
            writer.Write(isEnter ? byte.MaxValue : (byte)0);
            writer.EndMessage();
            RPCProcedure.useUncheckedVent(__instance.Id, PlayerControl.LocalPlayer.PlayerId,
                isEnter ? byte.MaxValue : (byte)0);
            SoundEffectsManager.play("tricksterUseBoxVent");
            return false;
        }

        if (isEnter)
            PlayerControl.LocalPlayer.MyPhysics.RpcEnterVent(__instance.Id);
        else
            PlayerControl.LocalPlayer.MyPhysics.RpcExitVent(__instance.Id);
        __instance.SetButtons(isEnter && canMoveInVents);
        return false;
    }
}

[HarmonyPatch(typeof(Vent), nameof(Vent.TryMoveToVent))]
public static class MoveToVentPatch
{
    public static bool Prefix(Vent otherVent)
    {
        return !Trapper.playersOnMap.Contains(PlayerControl.LocalPlayer.PlayerId);
    }
}

[HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.FixedUpdate))]
internal class VentButtonVisibilityPatch
{
    private static void Postfix(PlayerControl __instance)
    {
        if (__instance.AmOwner && __instance.roleCanUseVents() &&
            FastDestroyableSingleton<HudManager>.Instance.ReportButton.isActiveAndEnabled)
            FastDestroyableSingleton<HudManager>.Instance.ImpostorVentButton.Show();
    }
}

[HarmonyPatch(typeof(VentButton), nameof(VentButton.SetTarget))]
internal class VentButtonSetTargetPatch
{
    private static Sprite defaultVentSprite;

    private static void Postfix(VentButton __instance)
    {
        // Trickster render special vent button
        if (Trickster.trickster != null && Trickster.trickster == PlayerControl.LocalPlayer)
        {
            if (defaultVentSprite == null) defaultVentSprite = __instance.graphic.sprite;
            var isSpecialVent = __instance.currentTarget != null && __instance.currentTarget.gameObject != null &&
                                __instance.currentTarget.gameObject.name.StartsWith("JackInTheBoxVent_");
            __instance.graphic.sprite = isSpecialVent ? Trickster.getTricksterVentButtonSprite() : defaultVentSprite;
            __instance.buttonLabelText.enabled = !isSpecialVent;
        }
    }
}

[HarmonyPatch(typeof(KillButton), nameof(KillButton.DoClick))]
internal class KillButtonDoClickPatch
{
    public static KillButton instance { get;  set; }
    public static bool Prefix(KillButton __instance)
    {
        if (PropHunt.isPropHuntGM)
        {
            KillAnimationCoPerformKillPatch.hideNextAnimation = true; // dont jump out of bounds!
            return false;
        }

        if (__instance.isActiveAndEnabled && __instance.currentTarget && !__instance.isCoolingDown &&
            !PlayerControl.LocalPlayer.Data.IsDead && PlayerControl.LocalPlayer.CanMove)
        {
            // Deputy handcuff update.
            if (Deputy.handcuffedPlayers.Contains(PlayerControl.LocalPlayer.PlayerId))
            {
                Deputy.setHandcuffedKnows();
                return false;
            }
            //if(PlayerControl.LocalPlayer == Devil.devil && ShipStatus.Instance.Systems.ContainsKey(SystemTypes.Electrical)){
            //    return false;
            //}

            // Use an unchecked kill command, to allow shorter kill cooldowns etc. without getting kicked
            var res = Helpers.checkMurderAttemptAndKill(PlayerControl.LocalPlayer, __instance.currentTarget);
            // Handle blank kill
            if (res == MurderAttemptResult.BlankKill)
            {
                PlayerControl.LocalPlayer.killTimer = GameOptionsManager.Instance.currentNormalGameOptions.KillCooldown;
                if (PlayerControl.LocalPlayer == Cleaner.cleaner)
                    Cleaner.cleaner.killTimer = HudManagerStartPatch.cleanerCleanButton.Timer =
                        HudManagerStartPatch.cleanerCleanButton.MaxTimer;
                else if (PlayerControl.LocalPlayer == Warlock.warlock)
                    Warlock.warlock.killTimer = HudManagerStartPatch.warlockCurseButton.Timer =
                        HudManagerStartPatch.warlockCurseButton.MaxTimer;
                else if (PlayerControl.LocalPlayer == Mini.mini && Mini.mini.Data.Role.IsImpostor)
                    Mini.mini.SetKillTimer(GameOptionsManager.Instance.currentNormalGameOptions.KillCooldown *
                                           (Mini.isGrownUp() ? 0.66f : 2f));
                else if (PlayerControl.LocalPlayer == Witch.witch)
                    Witch.witch.killTimer = HudManagerStartPatch.witchSpellButton.Timer =
                        HudManagerStartPatch.witchSpellButton.MaxTimer;
                else if (PlayerControl.LocalPlayer == Ninja.ninja)
                    Ninja.ninja.killTimer = HudManagerStartPatch.ninjaButton.Timer =
                        HudManagerStartPatch.ninjaButton.MaxTimer;
            }

            __instance.SetTarget(null);
        }

        return false;
    }
}

[HarmonyPatch(typeof(SabotageButton), nameof(SabotageButton.Refresh))]
internal class SabotageButtonRefreshPatch
{
    private static void Postfix()
    {
        // Mafia disable sabotage button for Janitor and sometimes for Mafioso
        var blockSabotageJanitor = Janitor.janitor != null && Janitor.janitor == PlayerControl.LocalPlayer;
        var blockSabotageMafioso = Mafioso.mafioso != null && Mafioso.mafioso == PlayerControl.LocalPlayer &&
                                   Godfather.godfather != null && !Godfather.godfather.Data.IsDead;
        if (blockSabotageJanitor || blockSabotageMafioso)
            FastDestroyableSingleton<HudManager>.Instance.SabotageButton.SetDisabled();
    }
}

[HarmonyPatch(typeof(ReportButton), nameof(ReportButton.DoClick))]
internal class ReportButtonDoClickPatch
{
    public static bool Prefix(ReportButton __instance)
    {
        if (__instance.isActiveAndEnabled && Deputy.handcuffedPlayers.Contains(PlayerControl.LocalPlayer.PlayerId) &&
            __instance.graphic.color == Palette.EnabledColor) Deputy.setHandcuffedKnows();
        return !Deputy.handcuffedKnows.ContainsKey(PlayerControl.LocalPlayer.PlayerId);
    }
}

[HarmonyPatch(typeof(EmergencyMinigame), nameof(EmergencyMinigame.Update))]
internal class EmergencyMinigameUpdatePatch
{
    private static void Postfix(EmergencyMinigame __instance)
    {
        var roleCanCallEmergency = true;
        var statusText = "";

        // Deactivate emergency button for Swapper
        if (Swapper.swapper != null && Swapper.swapper == PlayerControl.LocalPlayer && !Swapper.canCallEmergency)
        {
            roleCanCallEmergency = false;
            statusText = "swapperCallMeetingText";
        }

        // Potentially deactivate emergency button for Jester
        if (Jester.jester != null && Jester.jester == PlayerControl.LocalPlayer && !Jester.canCallEmergency)
        {
            roleCanCallEmergency = false;
            statusText = "jesterCallMeetingText";
        }

        // Potentially deactivate emergency button for Lawyer/Prosecutor
        if (Lawyer.lawyer != null && Lawyer.lawyer == PlayerControl.LocalPlayer && !Lawyer.canCallEmergency)
        {
            roleCanCallEmergency = false;
            statusText = "lawyerCallMeetingText";
            if (Lawyer.isProsecutor) statusText = "prosecutorCallMeetingText";
        }
        if (Prophet.prophet != null && Prophet.prophet == PlayerControl.LocalPlayer && !Prophet.canCallEmergency)
        {
            roleCanCallEmergency = false;
            statusText = "prophetMeetingText";
        }

        if (!roleCanCallEmergency)
        {
            __instance.StatusText.text = statusText.Translate();
            __instance.NumberText.text = string.Empty;
            __instance.ClosedLid.gameObject.SetActive(true);
            __instance.OpenLid.gameObject.SetActive(false);
            __instance.ButtonActive = false;
            return;
        }

        // Handle max number of meetings
        if (__instance.state == 1)
        {
            var localRemaining = PlayerControl.LocalPlayer.RemainingEmergencies;
            var teamRemaining = Mathf.Max(0, maxNumberOfMeetings - meetingsCount);
            var remaining = Mathf.Min(localRemaining,
                Mayor.mayor != null && Mayor.mayor == PlayerControl.LocalPlayer ? 1 : teamRemaining);
            __instance.NumberText.text = string.Format("modNumberText".Translate(), localRemaining.ToString(), teamRemaining.ToString());
            __instance.ButtonActive = remaining > 0;
            __instance.ClosedLid.gameObject.SetActive(!__instance.ButtonActive);
            __instance.OpenLid.gameObject.SetActive(__instance.ButtonActive);
        }
    }
}

[HarmonyPatch(typeof(Console), nameof(Console.CanUse))]
public static class ConsoleCanUsePatch
{
    public static bool Prefix(ref float __result, Console __instance, [HarmonyArgument(0)] NetworkedPlayerInfo pc,
        [HarmonyArgument(1)] out bool canUse, [HarmonyArgument(2)] out bool couldUse)
    {
        canUse = couldUse = false;
        if (Swapper.swapper != null && Swapper.swapper == PlayerControl.LocalPlayer)
            return !__instance.TaskTypes.Any(x => x == TaskTypes.FixLights || x == TaskTypes.FixComms);
        if (__instance.AllowImpostor) return true;
        if (!pc.Object.hasFakeTasks()) return true;
        __result = float.MaxValue;
        return false;
    }
}

[HarmonyPatch(typeof(TuneRadioMinigame), nameof(TuneRadioMinigame.Begin))]
internal class CommsMinigameBeginPatch
{
    private static void Postfix(TuneRadioMinigame __instance)
    {
        // Block Swapper from fixing comms. Still looking for a better way to do this, but deleting the task doesn't seem like a viable option since then the camera, admin table, ... work while comms are out
        if (Swapper.swapper != null && Swapper.swapper == PlayerControl.LocalPlayer) __instance.Close();
    }
}

[HarmonyPatch(typeof(SwitchMinigame), nameof(SwitchMinigame.Begin))]
internal class LightsMinigameBeginPatch
{
    private static void Postfix(SwitchMinigame __instance)
    {
        // Block Swapper from fixing lights. One could also just delete the PlayerTask, but I wanted to do it the same way as with coms for now.
        if (Swapper.swapper != null && Swapper.swapper == PlayerControl.LocalPlayer) __instance.Close();
    }
}

[HarmonyPatch]
internal class VitalsMinigamePatch
{
    private static List<TextMeshPro> hackerTexts = new();

    [HarmonyPatch(typeof(VitalsMinigame), nameof(VitalsMinigame.Begin))]
    private class VitalsMinigameStartPatch
    {
        private static void Postfix(VitalsMinigame __instance)
        {
            if (Hacker.hacker != null && PlayerControl.LocalPlayer == Hacker.hacker)
            {
                hackerTexts = new List<TextMeshPro>();
                foreach (var panel in __instance.vitals)
                {
                    var text = Object.Instantiate(__instance.SabText, panel.transform);
                    hackerTexts.Add(text);
                    Object.DestroyImmediate(text.GetComponent<AlphaBlink>());
                    text.gameObject.SetActive(false);
                    text.transform.localScale = Vector3.one * 0.75f;
                    text.transform.localPosition = new Vector3(-0.75f, -0.23f, 0f);
                }
            }

            //Fix Visor in Vitals
            foreach (var panel in __instance.vitals)
                if (panel.PlayerIcon != null && panel.PlayerIcon.cosmetics.skin != null)
                    panel.PlayerIcon.cosmetics.skin.transform.position = new Vector3(0, 0, 0f);
        }
    }

    [HarmonyPatch(typeof(VitalsMinigame), nameof(VitalsMinigame.Update))]
    private class VitalsMinigameUpdatePatch
    {
        private static void Postfix(VitalsMinigame __instance)
        {
            // Hacker show time since death

            if (Hacker.hacker != null && Hacker.hacker == PlayerControl.LocalPlayer && Hacker.hackerTimer > 0)
                for (var k = 0; k < __instance.vitals.Length; k++)
                {
                    var vitalsPanel = __instance.vitals[k];
                    var player = vitalsPanel.PlayerInfo;

                    // Hacker update
                    if (vitalsPanel.IsDead)
                    {
                        var deadPlayer = deadPlayers?.Where(x => x.player?.PlayerId == player?.PlayerId)
                            ?.FirstOrDefault();
                        if (deadPlayer != null && k < hackerTexts.Count && hackerTexts[k] != null)
                        {
                            var timeSinceDeath = (float)(DateTime.UtcNow - deadPlayer.timeOfDeath).TotalMilliseconds;
                            hackerTexts[k].gameObject.SetActive(true);
                            hackerTexts[k].text = Math.Round(timeSinceDeath / 1000) + "s";
                        }
                    }
                }
            else
                foreach (var text in hackerTexts)
                    if (text != null && text.gameObject != null)
                        text.gameObject.SetActive(false);
        }
    }
}

[HarmonyPatch]
internal class AdminPanelPatch
{
    private static Dictionary<SystemTypes, List<Color>> players = new();

    [HarmonyPatch(typeof(MapCountOverlay), nameof(MapCountOverlay.Update))]
    private class MapCountOverlayUpdatePatch
    {
        private static bool Prefix(MapCountOverlay __instance)
        {
            // Save colors for the Hacker
            __instance.timer += Time.deltaTime;
            if (__instance.timer < 0.1f) return false;
            __instance.timer = 0f;
            players = new Dictionary<SystemTypes, List<Color>>();
            var commsActive = false;
            foreach (var task in PlayerControl.LocalPlayer.myTasks.GetFastEnumerator())
                if (task.TaskType == TaskTypes.FixComms)
                    commsActive = true;


            if (!__instance.isSab && commsActive)
            {
                __instance.isSab = true;
                __instance.BackgroundColor.SetColor(Palette.DisabledGrey);
                __instance.SabotageText.gameObject.SetActive(true);
                return false;
            }

            if (__instance.isSab && !commsActive)
            {
                __instance.isSab = false;
                __instance.BackgroundColor.SetColor(Color.green);
                __instance.SabotageText.gameObject.SetActive(false);
            }

            for (var i = 0; i < __instance.CountAreas.Length; i++)
            {
                var counterArea = __instance.CountAreas[i];
                var roomColors = new List<Color>();
                players.Add(counterArea.RoomType, roomColors);

                if (!commsActive)
                {
                    var plainShipRoom = MapUtilities.CachedShipStatus.FastRooms[counterArea.RoomType];

                    if (plainShipRoom != null && plainShipRoom.roomArea)
                    {
                        var hashSet = new HashSet<int>();
                        var num = plainShipRoom.roomArea.OverlapCollider(__instance.filter, __instance.buffer);
                        var num2 = 0;
                        for (var j = 0; j < num; j++)
                        {
                            var collider2D = __instance.buffer[j];
                            if (collider2D.CompareTag("DeadBody") && __instance.includeDeadBodies)
                            {
                                num2++;
                                var bodyComponent = collider2D.GetComponent<DeadBody>();
                                if (bodyComponent)
                                {
                                    var playerInfo = GameData.Instance.GetPlayerById(bodyComponent.ParentId);
                                    if (playerInfo != null)
                                    {
                                        var color = Palette.PlayerColors[playerInfo.DefaultOutfit.ColorId];
                                        if (Hacker.onlyColorType)
                                            color = Helpers.isD(playerInfo.PlayerId)
                                                ? Palette.PlayerColors[7]
                                                : Palette.PlayerColors[6];
                                        roomColors.Add(color);
                                    }
                                }
                            }
                            else if (!collider2D.isTrigger)
                            {
                                var component = collider2D.GetComponent<PlayerControl>();
                                if (component && component.Data != null && !component.Data.Disconnected &&
                                    !component.Data.IsDead &&
                                    (__instance.showLivePlayerPosition || !component.AmOwner) &&
                                    hashSet.Add(component.PlayerId))
                                {
                                    num2++;
                                    if (component?.cosmetics?.currentBodySprite?.BodySprite?.material != null)
                                    {
                                        var color =
                                            component.cosmetics.currentBodySprite.BodySprite.material.GetColor(
                                                "_BodyColor");
                                        if (Hacker.onlyColorType)
                                            color = Helpers.isLighterColor(component)
                                                ? Palette.PlayerColors[7]
                                                : Palette.PlayerColors[6];
                                        roomColors.Add(color);
                                    }
                                }
                            }
                        }

                        counterArea.UpdateCount(num2);
                    }
                    else
                    {
                        Debug.LogWarning("Couldn't find counter for:" + counterArea.RoomType);
                    }
                }
                else
                {
                    counterArea.UpdateCount(0);
                }
            }

            return false;
        }
    }

    [HarmonyPatch(typeof(CounterArea), nameof(CounterArea.UpdateCount))]
    private class CounterAreaUpdateCountPatch
    {
        private static Material defaultMat;
        private static Material newMat;

        private static void Postfix(CounterArea __instance)
        {
            // Hacker display saved colors on the admin panel
            var showHackerInfo = Hacker.hacker != null && Hacker.hacker == PlayerControl.LocalPlayer &&
                                 Hacker.hackerTimer > 0;
            if (players.ContainsKey(__instance.RoomType))
            {
                var colors = players[__instance.RoomType];
                var i = -1;
                foreach (var icon in __instance.myIcons.GetFastEnumerator())
                {
                    i += 1;
                    var renderer = icon.GetComponent<SpriteRenderer>();

                    if (renderer != null)
                    {
                        if (defaultMat == null) defaultMat = renderer.material;
                        if (newMat == null) newMat = Object.Instantiate(defaultMat);
                        if (showHackerInfo && colors.Count > i)
                        {
                            renderer.material = newMat;
                            var color = colors[i];
                            renderer.material.SetColor("_BodyColor", color);
                            var id = Palette.PlayerColors.IndexOf(color);
                            if (id < 0)
                                renderer.material.SetColor("_BackColor", color);
                            else
                                renderer.material.SetColor("_BackColor", Palette.ShadowColors[id]);
                            renderer.material.SetColor("_VisorColor", Palette.VisorColor);
                        }
                        else
                        {
                            renderer.material = defaultMat;
                        }
                    }
                }
            }
        }
    }
}

[HarmonyPatch]
internal class SurveillanceMinigamePatch
{
    private static int page;
    private static float timer;

    public static List<GameObject> nightVisionOverlays;

    private static readonly Sprite overlaySprite =
        Helpers.loadSpriteFromResources("NightVisionOverlay.png", 350f);

    public static bool nightVisionIsActive;
    private static bool isLightsOut;

    private static void nightVisionUpdate(SurveillanceMinigame SkeldCamsMinigame = null,
        PlanetSurveillanceMinigame SwitchCamsMinigame = null, FungleSurveillanceMinigame FungleCamMinigame = null)
    {
        GameObject closeButton = null;
        if (nightVisionOverlays == null)
        {
            List<MeshRenderer> viewPorts = new();
            Transform viewablesTransform = null;
            if (SkeldCamsMinigame != null)
            {
                closeButton = SkeldCamsMinigame.Viewables.transform.Find("CloseButton").gameObject;
                foreach (var rend in SkeldCamsMinigame.ViewPorts) viewPorts.Add(rend);
                viewablesTransform = SkeldCamsMinigame.Viewables.transform;
            }
            else if (SwitchCamsMinigame != null)
            {
                closeButton = SwitchCamsMinigame.Viewables.transform.Find("CloseButton").gameObject;
                viewPorts.Add(SwitchCamsMinigame.ViewPort);
                viewablesTransform = SwitchCamsMinigame.Viewables.transform;
            }
            else if (FungleCamMinigame != null)
            {
                closeButton = FungleCamMinigame.transform.Find("CloseButton").gameObject;
                viewPorts.Add(FungleCamMinigame.viewport);
                viewablesTransform = FungleCamMinigame.viewport.transform;
            }
            else
            {
                return;
            }

            nightVisionOverlays = new List<GameObject>();

            foreach (var renderer in viewPorts)
            {
                GameObject overlayObject;
                float zPosition;
                if (FungleCamMinigame != null)
                {
                    overlayObject = GameObject.Instantiate(closeButton, renderer.transform);
                    overlayObject.layer = renderer.gameObject.layer;
                    zPosition = -0.5f;
                    overlayObject.transform.localPosition = new Vector3(0, 0, zPosition);
                }
                else
                {
                    overlayObject = GameObject.Instantiate(closeButton, viewablesTransform);
                    zPosition = overlayObject.transform.position.z;
                    overlayObject.layer = closeButton.layer;
                    overlayObject.transform.position = new Vector3(renderer.transform.position.x,
                        renderer.transform.position.y, zPosition);
                }

                var localScale = SkeldCamsMinigame != null
                    ? new Vector3(0.91f, 0.612f, 1f)
                    : new Vector3(2.124f, 1.356f, 1f);
                localScale = FungleCamMinigame != null ? new Vector3(10f, 10f, 1f) : localScale;
                overlayObject.transform.localScale = localScale;
                var overlayRenderer = overlayObject.GetComponent<SpriteRenderer>();
                overlayRenderer.sprite = overlaySprite;
                overlayObject.SetActive(false);
                GameObject.Destroy(overlayObject.GetComponent<CircleCollider2D>());
                nightVisionOverlays.Add(overlayObject);
            }
        }

        isLightsOut = PlayerControl.LocalPlayer.myTasks.ToArray().Any(x => x.name.Contains("FixLightsTask")) ||
                      Trickster.lightsOutTimer > 0;
        var ignoreNightVision =
            (CustomOptionHolder.camsNoNightVisionIfImpVision.getBool() &&
             Helpers.hasImpVision(GameData.Instance.GetPlayerById(PlayerControl.LocalPlayer.PlayerId))) ||
            PlayerControl.LocalPlayer.Data.IsDead;
        var nightVisionEnabled = CustomOptionHolder.camsNightVision.getBool();

        if (isLightsOut && !nightVisionIsActive && nightVisionEnabled && !ignoreNightVision)
        {
            // only update when something changed!
            foreach (var pc in PlayerControl.AllPlayerControls)
            {
                if (pc == Ninja.ninja && Ninja.invisibleTimer > 0f) continue;
                pc.setLook("", 11, "", "", "", "", false);
            }

            foreach (var overlayObject in nightVisionOverlays) overlayObject.SetActive(true);
            // Dead Bodies
            foreach (var deadBody in GameObject.FindObjectsOfType<DeadBody>())
            {
                var component = deadBody.bodyRenderers.FirstOrDefault();
                component.material.SetColor("_BackColor", Palette.ShadowColors[11]);
                component.material.SetColor("_BodyColor", Palette.PlayerColors[11]);
            }

            nightVisionIsActive = true;
        }
        else if (!isLightsOut && nightVisionIsActive)
        {
            resetNightVision();
        }
    }

    public static void resetNightVision()
    {
        foreach (var go in nightVisionOverlays) go.Destroy();
        nightVisionOverlays = null;

        if (nightVisionIsActive)
        {
            nightVisionIsActive = false;
            foreach (var pc in PlayerControl.AllPlayerControls)
            {
                if (Camouflager.camouflageTimer > 0)
                {
                    pc.setLook("", 6, "", "", "", "", false);
                }
                else if (pc == Morphling.morphling && Morphling.morphTimer > 0)
                {
                    var target = Morphling.morphTarget;
                    Morphling.morphling.setLook(target.Data.PlayerName, target.Data.DefaultOutfit.ColorId,
                        target.Data.DefaultOutfit.HatId, target.Data.DefaultOutfit.VisorId,
                        target.Data.DefaultOutfit.SkinId, target.Data.DefaultOutfit.PetId, false);
                }
                else if (pc == Ninja.ninja && Ninja.invisibleTimer > 0f)
                {
                    continue;
                }
                else
                {
                    pc.setDefaultLook(false);
                }

                // Dead Bodies
                foreach (var deadBody in GameObject.FindObjectsOfType<DeadBody>())
                {
                    var colorId = GameData.Instance.GetPlayerById(deadBody.ParentId).Object.Data.DefaultOutfit.ColorId;
                    var component = deadBody.bodyRenderers.FirstOrDefault();
                    component.material.SetColor("_BackColor", Palette.ShadowColors[colorId]);
                    component.material.SetColor("_BodyColor", Palette.PlayerColors[colorId]);
                }
            }
        }
    }

    public static void enforceNightVision(PlayerControl player)
    {
        if (isLightsOut && nightVisionOverlays != null && nightVisionIsActive)
            player.setLook("", 11, "", "", "", "", false);
    }

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.SetPlayerMaterialColors))]
    public static void Postfix(PlayerControl __instance, SpriteRenderer rend)
    {
        if (!nightVisionIsActive) return;
        foreach (var deadBody in GameObject.FindObjectsOfType<DeadBody>())
        foreach (var component in new SpriteRenderer[2]
                     { deadBody.bodyRenderers.FirstOrDefault(), deadBody.bloodSplatter })
        {
            component.material.SetColor("_BackColor", Palette.ShadowColors[11]);
            component.material.SetColor("_BodyColor", Palette.PlayerColors[11]);
        }
    }

    [HarmonyPatch(typeof(SurveillanceMinigame), nameof(SurveillanceMinigame.Begin))]
    private class SurveillanceMinigameBeginPatch
    {
        public static void Postfix(SurveillanceMinigame __instance)
        {
            // Add securityGuard cameras
            page = 0;
            timer = 0;
            if (MapUtilities.CachedShipStatus.AllCameras.Length > 4 && __instance.FilteredRooms.Length > 0)
            {
                __instance.textures = __instance.textures.ToList()
                    .Concat(new RenderTexture[MapUtilities.CachedShipStatus.AllCameras.Length - 4]).ToArray();
                for (var i = 4; i < MapUtilities.CachedShipStatus.AllCameras.Length; i++)
                {
                    var surv = MapUtilities.CachedShipStatus.AllCameras[i];
                    var camera = Object.Instantiate(__instance.CameraPrefab);
                    camera.transform.SetParent(__instance.transform);
                    camera.transform.position = new Vector3(surv.transform.position.x, surv.transform.position.y, 8f);
                    camera.orthographicSize = 2.35f;
                    var temporary = RenderTexture.GetTemporary(256, 256, 16, (RenderTextureFormat)0);
                    __instance.textures[i] = temporary;
                    camera.targetTexture = temporary;
                }
            }
        }
    }

    [HarmonyPatch(typeof(SurveillanceMinigame), nameof(SurveillanceMinigame.Update))]
    private class SurveillanceMinigameUpdatePatch
    {
        public static bool Prefix(SurveillanceMinigame __instance)
        {
            // Update normal and securityGuard cameras
            timer += Time.deltaTime;
            var numberOfPages = Mathf.CeilToInt(MapUtilities.CachedShipStatus.AllCameras.Length / 4f);

            var update = false;

            if (timer > 3f || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                update = true;
                timer = 0f;
                page = (page + 1) % numberOfPages;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                page = (page + numberOfPages - 1) % numberOfPages;
                update = true;
                timer = 0f;
            }

            if ((__instance.isStatic || update) &&
                !PlayerTask.PlayerHasTaskOfType<IHudOverrideTask>(PlayerControl.LocalPlayer))
            {
                __instance.isStatic = false;
                for (var i = 0; i < __instance.ViewPorts.Length; i++)
                {
                    __instance.ViewPorts[i].sharedMaterial = __instance.DefaultMaterial;
                    __instance.SabText[i].gameObject.SetActive(false);
                    if (page * 4 + i < __instance.textures.Length)
                        __instance.ViewPorts[i].material.SetTexture("_MainTex", __instance.textures[page * 4 + i]);
                    else
                        __instance.ViewPorts[i].sharedMaterial = __instance.StaticMaterial;
                }
            }
            else if (!__instance.isStatic && PlayerTask.PlayerHasTaskOfType<HudOverrideTask>(PlayerControl.LocalPlayer))
            {
                __instance.isStatic = true;
                for (var j = 0; j < __instance.ViewPorts.Length; j++)
                {
                    __instance.ViewPorts[j].sharedMaterial = __instance.StaticMaterial;
                    __instance.SabText[j].gameObject.SetActive(true);
                }
            }

            nightVisionUpdate(__instance);
            return false;
        }
    }

    [HarmonyPatch(typeof(PlanetSurveillanceMinigame), nameof(PlanetSurveillanceMinigame.Update))]
    private class PlanetSurveillanceMinigameUpdatePatch
    {
        public static void Postfix(PlanetSurveillanceMinigame __instance)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
                __instance.NextCamera(1);
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
                __instance.NextCamera(-1);

            nightVisionUpdate(SwitchCamsMinigame: __instance);
        }
    }

    [HarmonyPatch(typeof(FungleSurveillanceMinigame), nameof(FungleSurveillanceMinigame.Update))]
    private class FungleSurveillanceMinigameUpdatePatch
    {
        public static void Postfix(FungleSurveillanceMinigame __instance)
        {
            nightVisionUpdate(FungleCamMinigame: __instance);
        }
    }


    [HarmonyPatch(typeof(SurveillanceMinigame), nameof(SurveillanceMinigame.OnDestroy))]
    private class SurveillanceMinigameDestroyPatch
    {
        public static void Prefix()
        {
            resetNightVision();
        }
    }

    [HarmonyPatch(typeof(PlanetSurveillanceMinigame), nameof(PlanetSurveillanceMinigame.OnDestroy))]
    private class PlanetSurveillanceMinigameDestroyPatch
    {
        public static void Prefix()
        {
            resetNightVision();
        }
    }
}

[HarmonyPatch(typeof(MedScanMinigame), nameof(MedScanMinigame.FixedUpdate))]
internal class MedScanMinigameFixedUpdatePatch
{
    private static void Prefix(MedScanMinigame __instance)
    {
        if (allowParallelMedBayScans)
        {
            __instance.medscan.CurrentUser = PlayerControl.LocalPlayer.PlayerId;
            __instance.medscan.UsersList.Clear();
        }
    }
}

[HarmonyPatch(typeof(MapBehaviour), nameof(MapBehaviour.ShowSabotageMap))]
internal class ShowSabotageMapPatch
{
    private static bool Prefix(MapBehaviour __instance)
    {
        if (HideNSeek.isHideNSeekGM)
            return HideNSeek.canSabotage;
        if (PropHunt.isPropHuntGM) return false;
        if (PlayerControl.LocalPlayer.Data.IsDead && CustomOptionHolder.deadImpsBlockSabotage.getBool())
        {
            __instance.ShowNormalMap();
            return false;
        }

        return true;
    }
}