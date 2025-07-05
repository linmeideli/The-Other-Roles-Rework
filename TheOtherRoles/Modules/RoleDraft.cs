using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BepInEx.Unity.IL2CPP.Utils.Collections;
using HarmonyLib;
using Hazel;
using Reactor.Utilities.Extensions;
using TheOtherRoles.Patches;
using TheOtherRoles.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static TheOtherRoles.TheOtherRoles;
using Object = UnityEngine.Object;

namespace TheOtherRoles.Modules;

[HarmonyPatch]
internal class RoleDraft
{
    public static bool isRunning;

    public static List<byte> pickOrder = new();
    private static bool picked;
    private static float timer;
    private static readonly List<ActionButton> buttons = new();
    private static TextMeshPro feedText;
    public static List<byte> alreadyPicked = new();
    public static TextMeshPro buttonText;

    public static bool isEnabled => CustomOptionHolder.isDraftMode.getBool() &&
                                    (TORMapOptions.gameMode == CustomGamemodes.Classic ||
                                     TORMapOptions.gameMode == CustomGamemodes.Guesser);

    public static IEnumerator CoSelectRoles(IntroCutscene __instance)
    {
        if (CustomOptionHolder.draftModeCanChat.getBool())
            FastDestroyableSingleton<HudManager>.Instance.Chat.SetVisible(true);

        isRunning = true;
        SoundEffectsManager.play("draft", 1f, true, true);
        alreadyPicked.Clear();
        var playedAlert = false;
        feedText = Object.Instantiate(__instance.TeamTitle, __instance.transform);
        var aspectPosition = feedText.gameObject.AddComponent<AspectPosition>();
        aspectPosition.Alignment = AspectPosition.EdgeAlignments.LeftTop;
        aspectPosition.DistanceFromEdge = new Vector2(1.62f, 1.2f);
        aspectPosition.AdjustPosition();
        feedText.transform.localScale = new Vector3(0.6f, 0.6f, 1);
        feedText.text = $"<size=190%>{"playerPicks".Translate()}</size>\n\n";
        feedText.alignment = TextAlignmentOptions.TopLeft;
        feedText.autoSizeTextContainer = true;
        feedText.fontSize = 3f;
        feedText.enableAutoSizing = false;

        var scroller = feedText.gameObject.AddComponent<Scroller>();
        scroller.allowX = false;
        scroller.allowY = true;
        scroller.active = true;
        scroller.velocity = new Vector2(0, 0);
        scroller.ContentYBounds = new FloatRange(0, (feedText.text.Count() - 12) * (0.25f));
        scroller.enabled = true;

        __instance.TeamTitle.transform.localPosition =
            __instance.TeamTitle.transform.localPosition + new Vector3(1f, 0f);
        __instance.TeamTitle.text = "currentlyPicking".Translate();
        __instance.BackgroundBar.enabled = false;
        __instance.TeamTitle.transform.localScale = new Vector3(0.25f, 0.25f, 1f);
        __instance.TeamTitle.autoSizeTextContainer = true;
        __instance.TeamTitle.enableAutoSizing = false;
        __instance.TeamTitle.fontSize = 5;
        __instance.TeamTitle.alignment = TextAlignmentOptions.Top;
        __instance.ImpostorText.gameObject.SetActive(false);
        GameObject.Find("BackgroundLayer")?.SetActive(false);
        foreach (var player in Object.FindObjectsOfType<PoolablePlayer>())
            if (player.name.Contains("Dummy"))
                player.gameObject.SetActive(false);

        __instance.FrontMost.gameObject.SetActive(false);

        if (AmongUsClient.Instance.AmHost) sendPickOrder();

        while (pickOrder.Count == 0) yield return null;

        while (pickOrder.Count > 0)
        {
            picked = false;
            timer = 0;
            var maxTimer = CustomOptionHolder.draftModeTimeToChoose.getFloat();
            var playerText = "";
            while (timer < maxTimer || !picked)
            {
                if (pickOrder.Count == 0)
                    break;
                // wait for pick
                timer += Time.deltaTime;
                if (PlayerControl.LocalPlayer.PlayerId == pickOrder[0])
                {
                    if (!playedAlert)
                    {
                        playedAlert = true;
                        SoundManager.Instance.PlaySound(ShipStatus.Instance.SabotageSound, false);
                    }

                    // Animate beginning of choice, by changing background color
                    var min = 50 / 255f;
                    var backGroundColor = new Color(min, min, min, 1);
                    if (timer < 1)
                    {
                        var max = 230 / 255f;
                        if (timer < 0.5f)
                        {
                            // White flash                              
                            var p = timer / 0.5f;
                            var value = (float)Math.Pow(p, 2f) * max;
                            backGroundColor = new Color(value, value, value, 1);
                        }
                        else
                        {
                            var p = (1 - timer) / 0.5f;
                            var value = (float)Math.Pow(p, 2f) * max + (1 - (float)Math.Pow(p, 2f)) * min;
                            backGroundColor = new Color(value, value, value, 1);
                        }
                    }

                    HudManager.Instance.FullScreen.color = backGroundColor;
                    GameObject.Find("BackgroundLayer")?.SetActive(false);

                    // enable pick, wait for pick
                    var youColor = timer - (int)timer > 0.5 ? Color.red : Color.yellow;
                    playerText = Helpers.cs(youColor, "youText".Translate() + "!");
                    // Available Roles:
                    List<RoleInfo> availableRoles = new();
                    foreach (var roleInfo in RoleInfo.allRoleInfos)
                    {
                        var impostorCount = PlayerControl.AllPlayerControls.ToArray().ToList()
                            .Where(x => x.Data.Role.IsImpostor).Count();
                        if (roleInfo.isModifier) continue;
                        // Remove Impostor Roles
                        if (PlayerControl.LocalPlayer.Data.Role.IsImpostor && !roleInfo.isImpostor) continue;
                        if (!PlayerControl.LocalPlayer.Data.Role.IsImpostor && roleInfo.isImpostor) continue;

                        var roleData = RoleManagerSelectRolesPatch.getRoleAssignmentData();
                        roleData.crewSettings.Add((byte)RoleId.Sheriff,
                            CustomOptionHolder.sheriffSpawnRate.getSelection());
                        if (CustomOptionHolder.sheriffSpawnRate.getSelection() > 0)
                            roleData.crewSettings.Add((byte)RoleId.Deputy,
                                CustomOptionHolder.deputySpawnRate.getSelection());
                        if (roleData.neutralSettings.ContainsKey((byte)roleInfo.roleId) &&
                            roleData.neutralSettings[(byte)roleInfo.roleId] == 0) continue;
                        if (roleData.impSettings.ContainsKey((byte)roleInfo.roleId) &&
                            roleData.impSettings[(byte)roleInfo.roleId] == 0) continue;
                        if (roleData.crewSettings.ContainsKey((byte)roleInfo.roleId) &&
                            roleData.crewSettings[(byte)roleInfo.roleId] == 0) continue;
                        if (new List<RoleId> { RoleId.Janitor, RoleId.Godfather, RoleId.Mafioso }.Contains(
                                roleInfo.roleId) && (CustomOptionHolder.mafiaSpawnRate.getSelection() == 0 ||
                                                     GameOptionsManager.Instance.currentGameOptions.NumImpostors < 3))
                            continue;
                        if (roleInfo.roleId == RoleId.Sidekick) continue;
                        if (roleInfo.roleId == RoleId.Deputy && Sheriff.sheriff == null) continue;
                        if (roleInfo.roleId == RoleId.Pursuer) continue;
                        if (roleInfo.roleId == RoleId.Spy && impostorCount < 2) continue;
                        if (roleInfo.roleId == RoleId.Prosecutor &&
                            (CustomOptionHolder.lawyerIsProsecutorChance.getSelection() == 0 ||
                             CustomOptionHolder.lawyerSpawnRate.getSelection() == 0)) continue;
                        if (roleInfo.roleId == RoleId.Lawyer &&
                            (CustomOptionHolder.lawyerIsProsecutorChance.getSelection() == 10 ||
                             CustomOptionHolder.lawyerSpawnRate.getSelection() == 0)) continue;
                        if (TORMapOptions.gameMode == CustomGamemodes.Guesser &&
                            (roleInfo.roleId == RoleId.EvilGuesser || roleInfo.roleId == RoleId.NiceGuesser)) continue;
                        if (alreadyPicked.Contains((byte)roleInfo.roleId) && roleInfo.roleId != RoleId.Crewmate)
                            continue;
                        if (CustomOptionHolder.crewmateRolesFill.getBool() && roleInfo.roleId == RoleId.Crewmate)
                            continue;

                        var impsPicked = alreadyPicked.Where(x => RoleInfo.roleInfoById[(RoleId)x].isImpostor).Count();

                        // Hanlde forcing of 100% roles for impostors
                        if (PlayerControl.LocalPlayer.Data.Role.IsImpostor)
                        {
                            var impsMax = CustomOptionHolder.impostorRolesCountMax.getSelection();
                            var impsMin = CustomOptionHolder.impostorRolesCountMin.getSelection();
                            if (impsMin > impsMax) impsMin = impsMax;
                            var impsLeft = pickOrder.Where(x => Helpers.playerById(x).Data.Role.IsImpostor).Count();
                            var imps100 = roleData.impSettings.Where(x => x.Value == 10).Count();
                            if (imps100 > impsMax) imps100 = impsMax;
                            var imps100Picked = alreadyPicked.Where(x => roleData.impSettings.GetValueSafe(x) == 10)
                                .Count();
                            if (imps100 - imps100Picked >= impsLeft && !(roleData.impSettings
                                    .Where(x => x.Value == 10 && x.Key == (byte)roleInfo.roleId).Count() > 0)) continue;
                            if (impsMin - impsPicked >= impsLeft && roleInfo.roleId == RoleId.Impostor) continue;
                            if (impsPicked >= impsMax && roleInfo.roleId != RoleId.Impostor) continue;
                        }

                        // Player is no impostor! Handle forcing of 100% roles for crew and neutral
                        else
                        {
                            // No more neutrals possible!
                            var neutralsPicked = alreadyPicked.Where(x => RoleInfo.roleInfoById[(RoleId)x].isNeutral)
                                .Count();
                            var crewPicked = alreadyPicked.Count - impsPicked - neutralsPicked;
                            var neutralsMax = CustomOptionHolder.neutralRolesCountMax.getSelection();
                            var neutralsMin = CustomOptionHolder.neutralRolesCountMin.getSelection();
                            var neutrals100 = roleData.neutralSettings.Where(x => x.Value == 10).Count();
                            if (neutrals100 > neutralsMin) neutralsMin = neutrals100;
                            if (neutralsMin > neutralsMax) neutralsMin = neutralsMax;

                            // If crewmate fill disabled and crew picked the amount of allowed crewmates alreay: no more crewmate except vanilla crewmate allowed!
                            var crewLimit = PlayerControl.AllPlayerControls.Count - impostorCount -
                                            (neutralsMin > neutrals100 ? neutralsMin :
                                                neutrals100 > neutralsMax ? neutralsMax : neutrals100);
                            var maxCrew = CustomOptionHolder.crewmateRolesFill.getBool()
                                ? CustomOptionHolder.crewmateRolesCountMax.getSelection()
                                : crewLimit;
                            if (maxCrew > crewLimit)
                                maxCrew = crewLimit;
                            if (crewPicked >= crewLimit && !roleInfo.isNeutral && roleInfo.roleId != RoleId.Crewmate)
                                continue;
                            // Fill roles means no crewmates allowed!
                            if (CustomOptionHolder.crewmateRolesFill.getBool() && roleInfo.roleId == RoleId.Crewmate)
                                continue;

                            var allowAnyNeutral = false;
                            if (neutralsPicked >= neutralsMax && roleInfo.isNeutral) continue;
                            // More neutrals needed? Then no more crewmates! This takes precedence over crew roles set to 100%!
                            var crewmatesLeft = pickOrder.Count -
                                                pickOrder.Where(x => Helpers.playerById(x).Data.Role.IsImpostor)
                                                    .Count();

                            if (crewmatesLeft <= neutralsMin - neutralsPicked && !roleInfo.isNeutral) continue;

                            if (neutralsMin - neutrals100 > neutralsPicked)
                                allowAnyNeutral = true;
                            // Handle 100% Roles PER Faction.

                            var neutrals100Picked = alreadyPicked
                                .Where(x => roleData.neutralSettings.GetValueSafe(x) == 10).Count();
                            if (neutrals100 > neutralsMax) neutrals100 = neutralsMax;

                            var crew100 = roleData.crewSettings.Where(x => x.Value == 10).Count();
                            var crew100Picked = alreadyPicked.Where(x => roleData.crewSettings.GetValueSafe(x) == 10)
                                .Count();
                            if (neutrals100 > neutralsMax) neutrals100 = neutralsMax;

                            if (crew100 > maxCrew) crew100 = maxCrew;
                            if ((neutrals100 - neutrals100Picked >= crewmatesLeft || (roleInfo.isNeutral &&
                                    neutrals100 - neutrals100Picked >= neutralsMax - neutralsPicked)) &&
                                !(neutrals100Picked >= neutralsMax) &&
                                !(roleData.neutralSettings.Where(x => x.Value == 10 && x.Key == (byte)roleInfo.roleId)
                                    .Count() > 0)) continue;
                            if (!(allowAnyNeutral && roleInfo.isNeutral) && crew100 - crew100Picked >= crewmatesLeft &&
                                !(roleData.crewSettings.Where(x => x.Value == 10 && x.Key == (byte)roleInfo.roleId)
                                    .Count() > 0)) continue;

                            if (!(allowAnyNeutral && roleInfo.isNeutral) &&
                                neutrals100 + crew100 - neutrals100Picked - crew100Picked >= crewmatesLeft &&
                                !(roleData.crewSettings.Where(x => x.Value == 10 && x.Key == (byte)roleInfo.roleId)
                                      .Count() > 0 ||
                                  roleData.neutralSettings.Where(x => x.Value == 10 && x.Key == (byte)roleInfo.roleId)
                                      .Count() > 0)) continue;
                        }

                        // Handle role pairings that are blocked, e.g. Vampire Warlock, Cleaner Vulture etc.
                        var blocked = false;
                        foreach (var blockedRoleId in CustomOptionHolder.blockedRolePairings)
                            if (alreadyPicked.Contains(blockedRoleId.Key) &&
                                blockedRoleId.Value.ToList().Contains((byte)roleInfo.roleId))
                            {
                                blocked = true;
                                break;
                            }

                        if (blocked) continue;


                        availableRoles.Add(roleInfo);
                    }

                    // Fallback for if all roles are somehow removed. (This is only the case if there is a bug, hence print a warning
                    if (availableRoles.Count == 0)
                    {
                        if (PlayerControl.LocalPlayer.Data.Role.IsImpostor)
                            availableRoles.Add(RoleInfo.impostor);
                        else
                            availableRoles.Add(RoleInfo.crewmate);
                        TheOtherRolesPlugin.Logger.LogWarning(
                            "Draft Mode: Fallback triggered, because no roles were left. Forced addition of basegame Imp/Crewmate");
                    }

                    List<RoleInfo> originalAvailable = new(availableRoles);

                    // remove some roles, so that you can't always get the same roles:
                    if (availableRoles.Count > CustomOptionHolder.draftModeAmountOfChoices.getFloat())
                    {
                        var countToRemove = availableRoles.Count -
                                            (int)CustomOptionHolder.draftModeAmountOfChoices.getFloat();
                        while (countToRemove-- > 0)
                        {
                            var toRemove = availableRoles.OrderBy(_ => Guid.NewGuid()).First();
                            availableRoles.Remove(toRemove);
                        }
                    }

                    if (timer >= maxTimer)
                        sendPick((byte)originalAvailable.OrderBy(_ => Guid.NewGuid()).First().roleId);


                    if (GameObject.Find("RoleButton") == null)
                    {
                        SoundEffectsManager.play("timemasterShield");
                        var i = 0;
                        var buttonsPerRow = 4;
                        var lastRow = availableRoles.Count / buttonsPerRow;
                        var buttonsInLastRow = availableRoles.Count % buttonsPerRow;

                        foreach (var roleInfo in availableRoles)
                        {
                            float row = i / buttonsPerRow;
                            float col = i % buttonsPerRow;
                            if (buttonsInLastRow != 0 && row == lastRow) col += (buttonsPerRow - buttonsInLastRow) / 2f;
                            // planned rows: maximum of 4, hence the following calculation for rows as well:
                            row += (4 - lastRow - 1) / 2f;

                            ActionButton actionButton = Object.Instantiate(HudManager.Instance.KillButton,
                                __instance.TeamTitle.transform);
                            actionButton.gameObject.SetActive(true);
                            actionButton.gameObject.name = "RoleButton";
                            actionButton.transform.localPosition = new Vector3(-8.4f + col * 5.5f, -10 - row * 3f);
                            actionButton.transform.localScale = new Vector3(2f, 2f);
                            actionButton.SetCoolDown(0, 0);
                            var textHolder = new GameObject("textHolder");
                            buttonText = textHolder.AddComponent<TextMeshPro>();
                            buttonText.text = "<b>" + roleInfo.name.Replace(" ", "\n") + "</b>";
                            buttonText.horizontalAlignment = HorizontalAlignmentOptions.Center;
                            buttonText.fontSize = 5;
                            textHolder.layer = actionButton.gameObject.layer;
                            buttonText.color = roleInfo.color;
                            textHolder.transform.SetParent(actionButton.transform, false);
                            textHolder.transform.localPosition =
                                new Vector3(0, buttonText.text.Contains("\n") ? -1.975f : -2.2f, -1);
                            var actionButtonGameObject = actionButton.gameObject;
                            var actionButtonRenderer = actionButton.graphic;
                            var actionButtonMat = actionButtonRenderer.material;

                            var button = actionButton.GetComponent<PassiveButton>();
                            button.OnClick = new Button.ButtonClickedEvent();
                            button.OnClick.AddListener((Action)(() => { sendPick((byte)roleInfo.roleId); }));
                            HudManager.Instance.StartCoroutine(Effects.Lerp(0.5f,
                                new Action<float>(p => { actionButton.OverrideText(""); })));
                            buttons.Add(actionButton);
                            i++;
                        }

                        if (GameObject.Find("RandomButton") == null && availableRoles.Count > 1)
                        {
                            float row = (availableRoles.Count + 1) / buttonsPerRow;
                            float col = (availableRoles.Count + 1) % buttonsPerRow;
                            if (buttonsInLastRow != 0 && row == lastRow) col += (buttonsPerRow - buttonsInLastRow) / 2f;
                            row += (4 - lastRow - 1) / 2f;

                            ActionButton randomButton = Object.Instantiate(HudManager.Instance.KillButton,
                                __instance.TeamTitle.transform);
                            randomButton.gameObject.SetActive(true);
                            randomButton.gameObject.name = "RandomButton";
                            randomButton.transform.localPosition = new Vector3(-8.4f + col * 5.5f, -10 - row * 3f);
                            randomButton.transform.localScale = new Vector3(2f, 2f);
                            randomButton.SetCoolDown(0, 0);
                            randomButton.buttonLabelText.gameObject.SetActive(false);

                            var randomTextHolder = new GameObject("randomTextHolder");
                            var randomText = randomTextHolder.AddComponent<TextMeshPro>();
                            randomText.text = $"<b>{"randomButtonText".Translate()}</b>";
                            randomText.horizontalAlignment = HorizontalAlignmentOptions.Center;
                            randomText.fontSize = 5;
                            randomTextHolder.layer = randomButton.gameObject.layer;
                            randomText.color = Color.green;
                            randomTextHolder.transform.SetParent(randomButton.transform, false);
                            randomTextHolder.transform.localPosition = new Vector3(0, buttonText.text.Contains("\n") ? -1.975f : -2.2f, -1);

                            var randomPassiveButton = randomButton.GetComponent<PassiveButton>();
                            randomPassiveButton.OnClick = new Button.ButtonClickedEvent();
                            randomPassiveButton.OnClick.AddListener((Action)(() => {
                                var randomRole = availableRoles.OrderBy(_ => Guid.NewGuid()).First();
                                sendPick((byte)randomRole.roleId);
                            }));

                            HudManager.Instance.StartCoroutine(Effects.Lerp(0.5f,
                                new Action<float>(p => {
                                    randomButton.graphic.material.SetFloat("_Desat", p % 0.5f * 2f);
                                })));

                            buttons.Add(randomButton);
                        }
                    }
                }
                else
                {
                    var currentPick = PlayerControl.AllPlayerControls.Count - pickOrder.Count + 1;
                    playerText = string.Format("anonymousPlayer".Translate(), currentPick);
                    HudManager.Instance.FullScreen.color = Color.black;
                }

                __instance.TeamTitle.text =
                    $"{Helpers.cs(Color.red, $"<size=300%>{"welcomeText".Translate()}</size>")}\n\n\n<size=200%> {"currentlyPicking".Translate()}</size>\n\n\n<size=250%>{playerText}</size>";
                var waitMore = pickOrder.IndexOf(PlayerControl.LocalPlayer.PlayerId);
                var waitMoreText = "";
                if (waitMore > 0) waitMoreText = string.Format("untilYourTurnText".Translate(), waitMore);
                __instance.TeamTitle.text +=
                    $"\n\n{waitMoreText}\n" + string.Format("randomSelectionText".Translate(), (int)(maxTimer + 1 - timer)) + $"\n {(SoundManager.MusicVolume > -80 ? "roleDraftMusic".Translate() : "")}";
                yield return null;
            }
        }

        HudManager.Instance.FullScreen.color = Color.black;
        __instance.FrontMost.gameObject.SetActive(true);
        GameObject.Find("BackgroundLayer")?.SetActive(true);
        if (AmongUsClient.Instance.AmHost)
        {
            RoleManagerSelectRolesPatch.assignRoleTargets(null); // Assign targets for Lawyer & Prosecutor
            if (RoleManagerSelectRolesPatch.isGuesserGamemode) RoleManagerSelectRolesPatch.assignGuesserGamemode();
            RoleManagerSelectRolesPatch.assignModifiers(); // Assign modifier
        }

        var myTimer = 0f;
        while (myTimer < 3f)
        {
            myTimer += Time.deltaTime;
            var c = new Color(0, 0, 0, myTimer / 3.0f);
            __instance.FrontMost.color = c;
            yield return null;
        }

        SoundEffectsManager.stop("draft");
        if (CustomOptionHolder.draftModeCanChat.getBool())
            FastDestroyableSingleton<HudManager>.Instance.Chat.SetVisible(false);
        isRunning = false;
    }

    public static void receivePick(byte playerId, byte roleId)
    {
        if (!isEnabled) return;
        RPCProcedure.setRole(roleId, playerId);
        alreadyPicked.Add(roleId);
        try
        {
            pickOrder.Remove(playerId);
            timer = 0;
            picked = true;
            var roleInfo = RoleInfo.allRoleInfos.First(x => (byte)x.roleId == roleId);
            var roleString = Helpers.cs(roleInfo.color, roleInfo.name);
            var roleLength =
                roleInfo.name.Length; // Not used for now, but stores the amount of charactes of the roleString.
            if (!CustomOptionHolder.draftModeShowRoles.getBool() && !(playerId == PlayerControl.LocalPlayer.PlayerId))
            {
                roleString = "roleDraftUnknowRole".Translate();
                roleLength = roleString.Length;
            }
            else if (CustomOptionHolder.draftModeHideImpRoles.getBool() && roleInfo.isImpostor &&
                     !(playerId == PlayerControl.LocalPlayer.PlayerId))
            {
                roleString = Helpers.cs(Palette.ImpostorRed, "roleDraftImpRole".Translate());
                roleLength = "Impostor Role".Length;
            }
            else if (CustomOptionHolder.draftModeHideNeutralRoles.getBool() && roleInfo.isNeutral &&
                     !(playerId == PlayerControl.LocalPlayer.PlayerId))
            {
                roleString = Helpers.cs(Palette.Blue, "roleDraftNeutRole".Translate());
                roleLength = "Neutral Role".Length;
            }
            else if (CustomOptionHolder.draftModeHideCrewRoles.getBool() && !roleInfo.isImpostor && !roleInfo.isNeutral &&
                     !(playerId == PlayerControl.LocalPlayer.PlayerId))
            {
                roleString = Helpers.cs(Palette.Blue, "roleDraftCrewRole".Translate());
                roleLength = "Crewmate Role".Length;
            }

            var line = $"{(playerId == PlayerControl.LocalPlayer.PlayerId ? "youText2".Translate() : alreadyPicked.Count)}:";
            int spaceCount = Math.Max(0, 6 - line.Length);
            line += string.Concat(Enumerable.Repeat(" ", spaceCount)) + roleString;
            feedText.text += line + "\n";
            SoundEffectsManager.play("select");
        }
        catch (Exception e)
        {
            TheOtherRolesPlugin.Logger.LogError(e);
        }
    }

    public static void sendPick(byte RoleId)
    {
        SoundEffectsManager.stop("timeMasterShield");
        var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
            (byte)CustomRPC.DraftModePick, SendOption.Reliable);
        writer.Write(PlayerControl.LocalPlayer.PlayerId);
        writer.Write(RoleId);
        AmongUsClient.Instance.FinishRpcImmediately(writer);
        receivePick(PlayerControl.LocalPlayer.PlayerId, RoleId);

        // destroy all the buttons:
        foreach (var button in buttons) button?.gameObject?.Destroy();
        buttons.Clear();
    }


    public static void sendPickOrder()
    {
        pickOrder = PlayerControl.AllPlayerControls.ToArray().Select(x => x.PlayerId).OrderBy(_ => Guid.NewGuid())
            .ToList().ToList();
        var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
            (byte)CustomRPC.DraftModePickOrder, SendOption.Reliable);
        writer.Write((byte)pickOrder.Count);
        foreach (var item in pickOrder) writer.Write(item);
        AmongUsClient.Instance.FinishRpcImmediately(writer);
    }


    public static void receivePickOrder(int amount, MessageReader reader)
    {
        pickOrder.Clear();
        for (var i = 0; i < amount; i++) pickOrder.Add(reader.ReadByte());
    }

    private class PatchedEnumerator : IEnumerable
    {
        public IEnumerator enumerator;
        public IEnumerator Postfix;

        public IEnumerator GetEnumerator()
        {
            while (enumerator.MoveNext()) yield return enumerator.Current;
            while (Postfix.MoveNext())
                yield return Postfix.Current;
        }
    }


    [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.ShowTeam))]
    private class ShowRolePatch
    {
        [HarmonyPostfix]
        public static void Postfix(IntroCutscene __instance, ref Il2CppSystem.Collections.IEnumerator __result)
        {
            if (!isEnabled) return;
            var newEnumerator = new PatchedEnumerator
            {
                enumerator = __result.WrapToManaged(),
                Postfix = CoSelectRoles(__instance)
            };
            __result = newEnumerator.GetEnumerator().WrapToIl2Cpp();
        }
    }
}