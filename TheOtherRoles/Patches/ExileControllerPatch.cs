using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Hazel;
using PowerTools;
using TheOtherRoles.Objects;
using TheOtherRoles.Utilities;
using UnityEngine;
using static TheOtherRoles.TheOtherRoles;
using Object = UnityEngine.Object;

namespace TheOtherRoles.Patches;

[HarmonyPatch(typeof(ExileController), nameof(ExileController.BeginForGameplay))]
[HarmonyPriority(Priority.First)]
internal class ExileControllerBeginPatch
{
    public static void Prefix(ExileController __instance, [HarmonyArgument(0)] ref NetworkedPlayerInfo exiled)
    {
        // Medic shield
        if (Medic.medic != null && AmongUsClient.Instance.AmHost && Medic.futureShielded != null &&
            !Medic.medic.Data.IsDead)
        {
            // We need to send the RPC from the host here, to make sure that the order of shifting and setting the shield is correct(for that reason the futureShifted and futureShielded are being synced)
            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                (byte)CustomRPC.MedicSetShielded, SendOption.Reliable);
            writer.Write(Medic.futureShielded.PlayerId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            RPCProcedure.medicSetShielded(Medic.futureShielded.PlayerId);
        }

        if (Medic.usedShield) Medic.meetingAfterShielding = true; // Has to be after the setting of the shield

        // Shifter shift
        if (Shifter.shifter != null && AmongUsClient.Instance.AmHost && Shifter.futureShift != null)
        {
            // We need to send the RPC from the host here, to make sure that the order of shifting and erasing is correct (for that reason the futureShifted and futureErased are being synced)
            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                (byte)CustomRPC.ShifterShift, SendOption.Reliable);
            writer.Write(Shifter.futureShift.PlayerId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            RPCProcedure.shifterShift(Shifter.futureShift.PlayerId);
        }

        Shifter.futureShift = null;

        // Eraser erase
        if (Eraser.eraser != null && AmongUsClient.Instance.AmHost &&
            Eraser.futureErased !=
            null) // We need to send the RPC from the host here, to make sure that the order of shifting and erasing is correct (for that reason the futureShifted and futureErased are being synced)
            foreach (var target in Eraser.futureErased)
            {
                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                    (byte)CustomRPC.ErasePlayerRoles, SendOption.Reliable);
                writer.Write(target.PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.erasePlayerRoles(target.PlayerId);
                Eraser.alreadyErased.Add(target.PlayerId);
            }

        Eraser.futureErased = new List<PlayerControl>();

        if (Devil.devil != null && AmongUsClient.Instance.AmHost &&
           Devil.futureBlinded !=
           null) // We need to send the RPC from the host here, to make sure that the order of shifting and erasing is correct (for that reason the futureShifted and futureErased are being synced)
            foreach (var target in Eraser.futureErased)
            {
                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                    (byte)CustomRPC.ShowBlindedReportAndSetLookName, SendOption.Reliable);
                writer.Write(target.PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.ShowBlindedReportAndSetLookName(target.PlayerId);
            }

        Devil.futureBlinded = new List<PlayerControl>();

        // Trickster boxes
        if (Trickster.trickster != null && JackInTheBox.hasJackInTheBoxLimitReached()) JackInTheBox.convertToVents();

        // Activate portals.
        Portal.meetingEndsUpdate();

        // Witch execute casted spells
        if (Witch.witch != null && Witch.futureSpelled != null && AmongUsClient.Instance.AmHost)
        {
            var exiledIsWitch = exiled != null && exiled.PlayerId == Witch.witch.PlayerId;
            var witchDiesWithExiledLover = exiled != null && Lovers.existing() && Lovers.bothDie &&
                                           (Lovers.lover1.PlayerId == Witch.witch.PlayerId ||
                                            Lovers.lover2.PlayerId == Witch.witch.PlayerId) &&
                                           (exiled.PlayerId == Lovers.lover1.PlayerId ||
                                            exiled.PlayerId == Lovers.lover2.PlayerId);

            if ((witchDiesWithExiledLover || exiledIsWitch) && Witch.witchVoteSavesTargets)
                Witch.futureSpelled = new List<PlayerControl>();
            foreach (var target in Witch.futureSpelled)
                if (target != null && !target.Data.IsDead && Helpers.checkMuderAttempt(Witch.witch, target, true) ==
                    MurderAttemptResult.PerformKill)
                {
                    if (exiled != null && Lawyer.lawyer != null &&
                        (target == Lawyer.lawyer || target == Lovers.otherLover(Lawyer.lawyer)) &&
                        Lawyer.target != null && Lawyer.isProsecutor && Lawyer.target.PlayerId == exiled.PlayerId)
                        continue;
                    if (target == Lawyer.target && Lawyer.lawyer != null)
                    {
                        var writer2 = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                            (byte)CustomRPC.LawyerPromotesToPursuer, SendOption.Reliable);
                        AmongUsClient.Instance.FinishRpcImmediately(writer2);
                        RPCProcedure.lawyerPromotesToPursuer();
                    }

                    var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                        (byte)CustomRPC.UncheckedExilePlayer, SendOption.Reliable);
                    writer.Write(target.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    RPCProcedure.uncheckedExilePlayer(target.PlayerId);

                    var writer3 = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                        (byte)CustomRPC.ShareGhostInfo, SendOption.Reliable);
                    writer3.Write(PlayerControl.LocalPlayer.PlayerId);
                    writer3.Write((byte)RPCProcedure.GhostInfoTypes.DeathReasonAndKiller);
                    writer3.Write(target.PlayerId);
                    writer3.Write((byte)DeadPlayer.CustomDeathReason.WitchExile);
                    writer3.Write(Witch.witch.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer3);

                    GameHistory.overrideDeathReasonAndKiller(target, DeadPlayer.CustomDeathReason.WitchExile,
                        Witch.witch);
                }
        }

        Witch.futureSpelled = new List<PlayerControl>();

        // SecurityGuard vents and cameras
        var allCameras = MapUtilities.CachedShipStatus.AllCameras.ToList();
        TORMapOptions.camerasToAdd.ForEach(camera =>
        {
            camera.gameObject.SetActive(true);
            camera.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            allCameras.Add(camera);
        });
        MapUtilities.CachedShipStatus.AllCameras = allCameras.ToArray();
        TORMapOptions.camerasToAdd = new List<SurvCamera>();

        foreach (var vent in TORMapOptions.ventsToSeal)
        {
            var animator = vent.GetComponent<SpriteAnim>();
            vent.EnterVentAnim = vent.ExitVentAnim = null;
            var newSprite = animator == null
                ? SecurityGuard.getStaticVentSealedSprite()
                : SecurityGuard.getAnimatedVentSealedSprite();
            var rend = vent.myRend;
            if (Helpers.isFungle())
            {
                newSprite = SecurityGuard.getFungleVentSealedSprite();
                rend = vent.transform.GetChild(3).GetComponent<SpriteRenderer>();
                animator = vent.transform.GetChild(3).GetComponent<SpriteAnim>();
            }

            animator?.Stop();
            rend.sprite = newSprite;
            if (SubmergedCompatibility.IsSubmerged && vent.Id == 0)
                vent.myRend.sprite = SecurityGuard.getSubmergedCentralUpperSealedSprite();
            if (SubmergedCompatibility.IsSubmerged && vent.Id == 14)
                vent.myRend.sprite = SecurityGuard.getSubmergedCentralLowerSealedSprite();
            rend.color = Color.white;
            vent.name = "SealedVent_" + vent.name;
        }

        TORMapOptions.ventsToSeal = new List<Vent>();

        EventUtility.meetingEndsUpdate();
    }
}

[HarmonyPatch]
internal class ExileControllerWrapUpPatch
{
    // Workaround to add a "postfix" to the destroying of the exile controller (i.e. cutscene) and SpwanInMinigame of submerged
    [HarmonyPatch(typeof(Object), nameof(Object.Destroy), typeof(GameObject))]
    public static void Prefix(GameObject obj)
    {
        // Nightvision:
        if (obj != null && obj.name != null && obj.name.Contains("FungleSecurity"))
        {
            SurveillanceMinigamePatch.resetNightVision();
            return;
        }

        // submerged
        if (!SubmergedCompatibility.IsSubmerged) return;
        if (obj.name.Contains("ExileCutscene"))
        {
            WrapUpPostfix(obj.GetComponent<ExileController>().initData.networkedPlayer?.Object);
        }
        else if (obj.name.Contains("SpawnInMinigame"))
        {
            AntiTeleport.setPosition();
            Chameleon.lastMoved.Clear();
        }
    }

    private static void WrapUpPostfix(PlayerControl exiled)
    {
        // Prosecutor win condition
        if (exiled != null && Lawyer.lawyer != null && Lawyer.target != null && Lawyer.isProsecutor &&
            Lawyer.target.PlayerId == exiled.PlayerId && !Lawyer.lawyer.Data.IsDead)
            Lawyer.triggerProsecutorWin = true;

        // Mini exile lose condition
        else if (exiled != null && Mini.mini != null && Mini.mini.PlayerId == exiled.PlayerId && !Mini.isGrownUp() &&
                 !Mini.mini.Data.Role.IsImpostor && !RoleInfo.getRoleInfoForPlayer(Mini.mini).Any(x => x.isNeutral))
            Mini.triggerMiniLose = true;
        // Jester win condition
        else if (exiled != null && Jester.jester != null && Jester.jester.PlayerId == exiled.PlayerId)
            Jester.triggerJesterWin = true;


        // Reset custom button timers where necessary
        CustomButton.MeetingEndedUpdate();

        // Mini set adapted cooldown
        if (Mini.mini != null && PlayerControl.LocalPlayer == Mini.mini && Mini.mini.Data.Role.IsImpostor)
        {
            var multiplier = Mini.isGrownUp() ? 0.66f : 2f;
            Mini.mini.SetKillTimer(GameOptionsManager.Instance.currentNormalGameOptions.KillCooldown * multiplier);
        }

        // Seer spawn souls
        if (Seer.deadBodyPositions != null && Seer.seer != null && PlayerControl.LocalPlayer == Seer.seer &&
            (Seer.mode == 0 || Seer.mode == 2))
        {
            foreach (var pos in Seer.deadBodyPositions)
            {
                var soul = new GameObject();
                //soul.transform.position = pos;
                soul.transform.position = new Vector3(pos.x, pos.y, pos.y / 1000 - 1f);
                soul.layer = 5;
                var rend = soul.AddComponent<SpriteRenderer>();
                soul.AddSubmergedComponent(SubmergedCompatibility.Classes.ElevatorMover);
                rend.sprite = Seer.getSoulSprite();

                if (Seer.limitSoulDuration)
                    FastDestroyableSingleton<HudManager>.Instance.StartCoroutine(Effects.Lerp(Seer.soulDuration,
                        new Action<float>(p =>
                        {
                            if (rend != null)
                            {
                                var tmp = rend.color;
                                tmp.a = Mathf.Clamp01(1 - p);
                                rend.color = tmp;
                            }

                            if (p == 1f && rend != null && rend.gameObject != null) Object.Destroy(rend.gameObject);
                        })));
            }

            Seer.deadBodyPositions = new List<Vector3>();
        }

        // Tracker reset deadBodyPositions
        Tracker.deadBodyPositions = new List<Vector3>();

        // Arsonist deactivate dead poolable players
        if (Arsonist.arsonist != null && Arsonist.arsonist == PlayerControl.LocalPlayer)
        {
            var visibleCounter = 0;
            var newBottomLeft = IntroCutsceneOnDestroyPatch.bottomLeft;
            var BottomLeft = newBottomLeft + new Vector3(-0.25f, -0.25f, 0);
            foreach (var p in PlayerControl.AllPlayerControls)
            {
                if (!TORMapOptions.playerIcons.ContainsKey(p.PlayerId)) continue;
                if (p.Data.IsDead || p.Data.Disconnected)
                {
                    TORMapOptions.playerIcons[p.PlayerId].gameObject.SetActive(false);
                }
                else
                {
                    TORMapOptions.playerIcons[p.PlayerId].transform.localPosition =
                        newBottomLeft + Vector3.right * visibleCounter * 0.35f;
                    visibleCounter++;
                }
            }
        }

        // Deputy check Promotion, see if the sheriff still exists. The promotion will be after the meeting.
        if (Deputy.deputy != null) PlayerControlFixedUpdatePatch.deputyCheckPromotion(true);

        // Force Bounty Hunter Bounty Update
        if (BountyHunter.bountyHunter != null && BountyHunter.bountyHunter == PlayerControl.LocalPlayer)
            BountyHunter.bountyUpdateTimer = 0f;

        // Medium spawn souls
        if (Medium.medium != null && PlayerControl.LocalPlayer == Medium.medium)
        {
            if (Medium.souls != null)
            {
                foreach (var sr in Medium.souls) Object.Destroy(sr.gameObject);
                Medium.souls = new List<SpriteRenderer>();
            }

            if (Medium.futureDeadBodies != null)
            {
                foreach (var (db, ps) in Medium.futureDeadBodies)
                {
                    var s = new GameObject();
                    //s.transform.position = ps;
                    s.transform.position = new Vector3(ps.x, ps.y, ps.y / 1000 - 1f);
                    s.layer = 5;
                    var rend = s.AddComponent<SpriteRenderer>();
                    s.AddSubmergedComponent(SubmergedCompatibility.Classes.ElevatorMover);
                    rend.sprite = Medium.getSoulSprite();
                    Medium.souls.Add(rend);
                }

                Medium.deadBodies = Medium.futureDeadBodies;
                Medium.futureDeadBodies = new List<Tuple<DeadPlayer, Vector3>>();
            }
        }

        // AntiTeleport set position
        AntiTeleport.setPosition();

        // Invert add meeting
        if (Invert.meetings > 0) Invert.meetings--;

        Chameleon.lastMoved.Clear();

        foreach (var trap in Trap.traps) trap.triggerable = false;
        FastDestroyableSingleton<HudManager>.Instance.StartCoroutine(Effects.Lerp(
            GameOptionsManager.Instance.currentNormalGameOptions.KillCooldown / 2 + 2, new Action<float>(p =>
            {
                if (p == 1f)
                    foreach (var trap in Trap.traps)
                        trap.triggerable = true;
            })));

        if (!Yoyo.markStaysOverMeeting)
            Silhouette.clearSilhouettes();
    }

    [HarmonyPatch(typeof(ExileController), nameof(ExileController.WrapUp))]
    private class BaseExileControllerPatch
    {
        public static void Postfix(ExileController __instance)
        {
            var networkedPlayer = __instance.initData.networkedPlayer;
            WrapUpPostfix(networkedPlayer != null ? networkedPlayer.Object : null);
        }
    }

    [HarmonyPatch(typeof(AirshipExileController), nameof(AirshipExileController.WrapUpAndSpawn))]
    private class AirshipExileControllerPatch
    {
        public static void Postfix(AirshipExileController __instance)
        {
            var networkedPlayer = __instance.initData.networkedPlayer;
            WrapUpPostfix(networkedPlayer != null ? networkedPlayer.Object : null);
        }
    }
}

[HarmonyPatch(typeof(SpawnInMinigame),
    nameof(SpawnInMinigame.Close))] // Set position of AntiTp players AFTER they have selected a spawn.
internal class AirshipSpawnInPatch
{
    private static void Postfix()
    {
        AntiTeleport.setPosition();
        Chameleon.lastMoved.Clear();
    }
}

[HarmonyPatch(typeof(TranslationController), nameof(TranslationController.GetString), typeof(StringNames),
    typeof(Il2CppReferenceArray<Il2CppSystem.Object>))]
internal class ExileControllerMessagePatch
{
    private static void Postfix(ref string __result, [HarmonyArgument(0)] StringNames id)
    {
        try
        {
            if (ExileController.Instance != null && ExileController.Instance.initData != null)
            {
                var player = ExileController.Instance.initData.networkedPlayer.Object;
                if (player == null) return;
                // Exile role text
                if (id == StringNames.ExileTextPN || id == StringNames.ExileTextSN || id == StringNames.ExileTextPP ||
                    id == StringNames.ExileTextSP)
                    __result = player.Data.PlayerName + " was The " + string.Join(" ",
                        RoleInfo.getRoleInfoForPlayer(player, false).Select(x => x.name).ToArray());
                // Hide number of remaining impostors on Jester win
                if (id == StringNames.ImpostorsRemainP || id == StringNames.ImpostorsRemainS)
                    if (Jester.jester != null && player.PlayerId == Jester.jester.PlayerId)
                        __result = "";
                if (Tiebreaker.isTiebreak) __result += " (Tiebreaker)";
                Tiebreaker.isTiebreak = false;
            }
        }
        catch
        {
            // pass - Hopefully prevent leaving while exiling to softlock game
        }
    }
}