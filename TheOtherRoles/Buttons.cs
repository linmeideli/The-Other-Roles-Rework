using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Hazel;
using TheOtherRoles.CustomGameModes;
using TheOtherRoles.Modules;
using TheOtherRoles.Objects;
using TheOtherRoles.Patches;
using TheOtherRoles.Utilities;
using TMPro;
using UnityEngine;
using static TheOtherRoles.TheOtherRoles;
using Object = UnityEngine.Object;
using Random = System.Random;

namespace TheOtherRoles;

[HarmonyPatch(typeof(HudManager), nameof(HudManager.Start))]
internal static class HudManagerStartPatch
{
    private static bool initialized;

    private static CustomButton engineerRepairButton;
    public static CustomButton janitorCleanButton;//
    public static CustomButton sheriffKillButton;
    private static CustomButton deputyHandcuffButton;
    private static CustomButton timeMasterShieldButton;
    private static CustomButton medicShieldButton;
    private static CustomButton shifterShiftButton;
    public static CustomButton morphlingButton;//
    public static CustomButton camouflagerButton;//
    private static CustomButton portalmakerPlacePortalButton;
    private static CustomButton usePortalButton;
    private static CustomButton portalmakerMoveToPortalButton;
    private static CustomButton hackerButton;
    public static CustomButton hackerVitalsButton;
    public static CustomButton hackerAdminTableButton;
    private static CustomButton trackerTrackPlayerButton;
    private static CustomButton trackerTrackCorpsesButton;
    public static CustomButton vampireKillButton;//
    public static CustomButton garlicButton;
    public static CustomButton jackalKillButton;//
    public static CustomButton sidekickKillButton;//
    public static CustomButton jackalSidekickButton;//
    public static CustomButton jackalAndSidekickSabotageLightsButton;//
    public static CustomButton eraserButton;//
    public static CustomButton placeJackInTheBoxButton;//
    public static CustomButton lightsOutButton;//
    public static CustomButton cleanerCleanButton;//
    public static CustomButton warlockCurseButton;//
    public static CustomButton securityGuardButton;
    public static CustomButton securityGuardCamButton;
    public static CustomButton arsonistButton;
    public static CustomButton vultureEatButton;
    public static CustomButton mediumButton;
    public static CustomButton pursuerButton;
    public static CustomButton witchSpellButton;
    public static CustomButton ninjaButton;
    public static CustomButton mayorMeetingButton;
    public static CustomButton thiefKillButton;
    public static CustomButton trapperButton;
    public static CustomButton bomberButton;//
    public static CustomButton yoyoButton;//
    public static CustomButton yoyoAdminTableButton;
    public static CustomButton fraudsterButton;//
    public static CustomButton devilButton;//
    public static CustomButton prophetButton;
    public static CustomButton peacedoveButton;
    public static CustomButton defuseButton;
    public static CustomButton zoomOutButton;
    private static CustomButton hunterLighterButton;
    private static CustomButton hunterAdminTableButton;
    private static CustomButton hunterArrowButton;
    private static CustomButton huntedShieldButton;
    public static CustomButton propDisguiseButton;
    private static CustomButton propHuntUnstuckButton;
    public static CustomButton propHuntRevealButton;
    private static CustomButton propHuntInvisButton;
    private static CustomButton propHuntSpeedboostButton;
    public static CustomButton propHuntAdminButton;
    public static CustomButton propHuntFindButton;
    public static CustomButton eventKickButton;

    public static Dictionary<byte, List<CustomButton>> deputyHandcuffedButtons;
    public static PoolablePlayer targetDisplay;
    public static GameObject propSpriteHolder;
    public static SpriteRenderer propSpriteRenderer;

    public static TMP_Text securityGuardButtonScrewsText;
    public static TMP_Text securityGuardChargesText;
    public static TMP_Text deputyButtonHandcuffsText;
    public static TMP_Text pursuerButtonBlanksText;
    public static TMP_Text hackerAdminTableChargesText;
    public static TMP_Text hackerVitalsChargesText;
    public static TMP_Text trapperChargesText;
    public static TMP_Text portalmakerButtonText1;
    public static TMP_Text portalmakerButtonText2;
    public static TMP_Text huntedShieldCountText;

    public static void setCustomButtonCooldowns()
    {
        if (!initialized)
            try
            {
                createButtonsPostfix(HudManager.Instance);
            }
            catch
            {
                TheOtherRolesPlugin.Logger.LogWarning(
                    "Button cooldowns not set, either the gamemode does not require them or there's something wrong.");
                return;
            }

        engineerRepairButton.MaxTimer = 0f;
        janitorCleanButton.MaxTimer = Janitor.cooldown;
        sheriffKillButton.MaxTimer = Sheriff.cooldown;
        deputyHandcuffButton.MaxTimer = Deputy.handcuffCooldown;
        timeMasterShieldButton.MaxTimer = TimeMaster.cooldown;
        medicShieldButton.MaxTimer = 0f;
        shifterShiftButton.MaxTimer = 0f;
        morphlingButton.MaxTimer = Morphling.cooldown;
        camouflagerButton.MaxTimer = Camouflager.cooldown;
        portalmakerPlacePortalButton.MaxTimer = Portalmaker.cooldown;
        usePortalButton.MaxTimer = Portalmaker.usePortalCooldown;
        portalmakerMoveToPortalButton.MaxTimer = Portalmaker.usePortalCooldown;
        hackerButton.MaxTimer = Hacker.cooldown;
        hackerVitalsButton.MaxTimer = Hacker.cooldown;
        hackerAdminTableButton.MaxTimer = Hacker.cooldown;
        vampireKillButton.MaxTimer = Vampire.cooldown;
        trackerTrackPlayerButton.MaxTimer = 0f;
        garlicButton.MaxTimer = 0f;
        jackalKillButton.MaxTimer = Jackal.cooldown;
        sidekickKillButton.MaxTimer = Sidekick.cooldown;
        jackalSidekickButton.MaxTimer = Jackal.createSidekickCooldown;
        eraserButton.MaxTimer = Eraser.cooldown;
        placeJackInTheBoxButton.MaxTimer = Trickster.placeBoxCooldown;
        lightsOutButton.MaxTimer = Trickster.lightsOutCooldown;
        cleanerCleanButton.MaxTimer = Cleaner.cooldown;
        warlockCurseButton.MaxTimer = Warlock.cooldown;
        securityGuardButton.MaxTimer = SecurityGuard.cooldown;
        securityGuardCamButton.MaxTimer = SecurityGuard.cooldown;
        arsonistButton.MaxTimer = Arsonist.cooldown;
        vultureEatButton.MaxTimer = Vulture.cooldown;
        mediumButton.MaxTimer = Medium.cooldown;
        pursuerButton.MaxTimer = Pursuer.cooldown;
        trackerTrackCorpsesButton.MaxTimer = Tracker.corpsesTrackingCooldown;
        witchSpellButton.MaxTimer = Witch.cooldown;
        ninjaButton.MaxTimer = Ninja.cooldown;
        thiefKillButton.MaxTimer = Thief.cooldown;
        mayorMeetingButton.MaxTimer = GameManager.Instance.LogicOptions.GetEmergencyCooldown();
        trapperButton.MaxTimer = Trapper.cooldown;
        bomberButton.MaxTimer = Bomber.bombCooldown;
        yoyoButton.MaxTimer = Yoyo.markCooldown;
        yoyoAdminTableButton.MaxTimer = Yoyo.adminCooldown;
        yoyoAdminTableButton.EffectDuration = 10f;
        fraudsterButton.MaxTimer = Fraudster.cooldown;
        devilButton.MaxTimer = Devil.blindCooldown;
        prophetButton.MaxTimer= Prophet.cooldown;
        hunterLighterButton.MaxTimer = Hunter.lightCooldown;
        hunterAdminTableButton.MaxTimer = Hunter.AdminCooldown;
        hunterArrowButton.MaxTimer = Hunter.ArrowCooldown;
        huntedShieldButton.MaxTimer = Hunted.shieldCooldown;
        defuseButton.MaxTimer = 0f;
        defuseButton.Timer = 0f;
        propDisguiseButton.MaxTimer = 1f;
        propHuntUnstuckButton.MaxTimer = PropHunt.unstuckCooldown;
        propHuntRevealButton.MaxTimer = PropHunt.revealCooldown;
        propHuntInvisButton.MaxTimer = PropHunt.invisCooldown;
        propHuntSpeedboostButton.MaxTimer = PropHunt.speedboostCooldown;
        propHuntAdminButton.MaxTimer = PropHunt.adminCooldown;
        propHuntFindButton.MaxTimer = PropHunt.findCooldown;

        timeMasterShieldButton.EffectDuration = TimeMaster.shieldDuration;
        hackerButton.EffectDuration = Hacker.duration;
        hackerVitalsButton.EffectDuration = Hacker.duration;
        hackerAdminTableButton.EffectDuration = Hacker.duration;
        vampireKillButton.EffectDuration = Vampire.delay;
        camouflagerButton.EffectDuration = Camouflager.duration;
        morphlingButton.EffectDuration = Morphling.duration;
        lightsOutButton.EffectDuration = Trickster.lightsOutDuration;
        arsonistButton.EffectDuration = Arsonist.duration;
        mediumButton.EffectDuration = Medium.duration;
        trackerTrackCorpsesButton.EffectDuration = Tracker.corpsesTrackingDuration;
        witchSpellButton.EffectDuration = Witch.spellCastingDuration;
        securityGuardCamButton.EffectDuration = SecurityGuard.duration;
        hunterLighterButton.EffectDuration = Hunter.lightDuration;
        hunterArrowButton.EffectDuration = Hunter.ArrowDuration;
        huntedShieldButton.EffectDuration = Hunted.shieldDuration;
        defuseButton.EffectDuration = Bomber.defuseDuration;
        bomberButton.EffectDuration = Bomber.destructionTime + Bomber.bombActiveAfter;
        propHuntUnstuckButton.EffectDuration = PropHunt.unstuckDuration;
        propHuntRevealButton.EffectDuration = PropHunt.revealDuration;
        propHuntInvisButton.EffectDuration = PropHunt.invisDuration;
        propHuntSpeedboostButton.EffectDuration = PropHunt.speedboostDuration;
        propHuntAdminButton.EffectDuration = PropHunt.adminDuration;
        propHuntFindButton.EffectDuration = PropHunt.findDuration;
        // Already set the timer to the max, as the button is enabled during the game and not available at the start
        lightsOutButton.Timer = lightsOutButton.MaxTimer;
        zoomOutButton.MaxTimer = 0f;
    }

    public static void resetTimeMasterButton()
    {
        timeMasterShieldButton.Timer = timeMasterShieldButton.MaxTimer;
        timeMasterShieldButton.isEffectActive = false;
        timeMasterShieldButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
        SoundEffectsManager.stop("timemasterShield");
    }

    public static void resetHuntedRewindButton()
    {
        huntedShieldButton.Timer = huntedShieldButton.MaxTimer;
        huntedShieldButton.isEffectActive = false;
        huntedShieldButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
        SoundEffectsManager.stop("timemasterShield");
    }

    private static void addReplacementHandcuffedButton(CustomButton button, Vector3? positionOffset = null,
        Func<bool> couldUse = null)
    {
        var positionOffsetValue =
            positionOffset ?? button.PositionOffset; // For non custom buttons, we can set these manually.
        positionOffsetValue.z = -0.1f;
        couldUse = couldUse ?? button.CouldUse;
        var replacementHandcuffedButton = new CustomButton(() => { }, () => { return true; }, couldUse, () => { },
            Deputy.getHandcuffedButtonSprite(), positionOffsetValue, button.hudManager, button.hotkey,
            true, Deputy.handcuffDuration, () => { }, button.mirror);
        replacementHandcuffedButton.Timer = replacementHandcuffedButton.EffectDuration;
        replacementHandcuffedButton.actionButton.cooldownTimerText.color = new Color(0F, 0.8F, 0F);
        replacementHandcuffedButton.isEffectActive = true;
        if (deputyHandcuffedButtons.ContainsKey(PlayerControl.LocalPlayer.PlayerId))
            deputyHandcuffedButtons[PlayerControl.LocalPlayer.PlayerId].Add(replacementHandcuffedButton);
        else
            deputyHandcuffedButtons.Add(PlayerControl.LocalPlayer.PlayerId,
                new List<CustomButton> { replacementHandcuffedButton });
    }

    // Disables / Enables all Buttons (except the ones disabled in the Deputy class), and replaces them with new buttons.
    public static void setAllButtonsHandcuffedStatus(bool handcuffed, bool reset = false)
    {
        if (reset)
        {
            deputyHandcuffedButtons = new Dictionary<byte, List<CustomButton>>();
            return;
        }

        if (handcuffed && !deputyHandcuffedButtons.ContainsKey(PlayerControl.LocalPlayer.PlayerId))
        {
            var maxI = CustomButton.buttons.Count;
            for (var i = 0; i < maxI; i++)
                try
                {
                    if (CustomButton.buttons[i].HasButton()) // For each custombutton the player has
                        addReplacementHandcuffedButton(CustomButton
                            .buttons[i]); // The new buttons are the only non-handcuffed buttons now!
                    CustomButton.buttons[i].isHandcuffed = true;
                }
                catch (NullReferenceException)
                {
                    System.Console.WriteLine(
                        "[WARNING] NullReferenceException from MeetingEndedUpdate().HasButton(), if theres only one warning its fine"); // Note: idk what this is good for, but i copied it from above /gendelo
                }

            // Non Custom (Vanilla) Buttons. The Originals are disabled / hidden in UpdatePatch.cs already, just need to replace them. Can use any button, as we replace onclick etc anyways.
            // Kill Button if enabled for the Role
            if (FastDestroyableSingleton<HudManager>.Instance.KillButton.isActiveAndEnabled)
                addReplacementHandcuffedButton(arsonistButton, CustomButton.ButtonPositions.upperRowRight,
                    () => { return FastDestroyableSingleton<HudManager>.Instance.KillButton.currentTarget != null; });
            // Vent Button if enabled
            if (PlayerControl.LocalPlayer.roleCanUseVents())
                addReplacementHandcuffedButton(arsonistButton, CustomButton.ButtonPositions.upperRowCenter,
                    () =>
                    {
                        return FastDestroyableSingleton<HudManager>.Instance.ImpostorVentButton.currentTarget != null;
                    });
            // Report Button
            addReplacementHandcuffedButton(arsonistButton,
                !PlayerControl.LocalPlayer.Data.Role.IsImpostor
                    ? new Vector3(-1f, -0.06f, 0)
                    : CustomButton.ButtonPositions.lowerRowRight,
                () =>
                {
                    return FastDestroyableSingleton<HudManager>.Instance.ReportButton.graphic.color ==
                           Palette.EnabledColor;
                });
        }
        else if (!handcuffed &&
                 deputyHandcuffedButtons.ContainsKey(PlayerControl.LocalPlayer
                     .PlayerId)) // Reset to original. Disables the replacements, enables the original buttons.
        {
            foreach (var replacementButton in deputyHandcuffedButtons[PlayerControl.LocalPlayer.PlayerId])
            {
                replacementButton.HasButton = () => { return false; };
                replacementButton.Update(); // To make it disappear properly.
                CustomButton.buttons.Remove(replacementButton);
            }

            deputyHandcuffedButtons.Remove(PlayerControl.LocalPlayer.PlayerId);

            foreach (var button in CustomButton.buttons) button.isHandcuffed = false;
        }
    }

    private static void setButtonTargetDisplay(PlayerControl target, CustomButton button = null, Vector3? offset = null)
    {
        if (target == null || button == null)
        {
            if (targetDisplay != null)
            {
                // Reset the poolable player
                targetDisplay.gameObject.SetActive(false);
                GameObject.Destroy(targetDisplay.gameObject);
                targetDisplay = null;
            }

            return;
        }

        // Add poolable player to the button so that the target outfit is shown
        button.actionButton.cooldownTimerText.transform.localPosition =
            new Vector3(0, 0, -1f); // Before the poolable player
        targetDisplay = Object.Instantiate(IntroCutsceneOnDestroyPatch.playerPrefab, button.actionButton.transform);
        var data = target.Data;
        target.SetPlayerMaterialColors(targetDisplay.cosmetics.currentBodySprite.BodySprite);
        targetDisplay.SetSkin(data.DefaultOutfit.SkinId, data.DefaultOutfit.ColorId);
        targetDisplay.SetHat(data.DefaultOutfit.HatId, data.DefaultOutfit.ColorId);
        targetDisplay.cosmetics.nameText.text = ""; // Hide the name!
        targetDisplay.transform.localPosition = new Vector3(0f, 0.22f, -0.01f);
        if (offset != null) targetDisplay.transform.localPosition += (Vector3)offset;
        targetDisplay.transform.localScale = Vector3.one * 0.33f;
        targetDisplay.setSemiTransparent(false);
        targetDisplay.gameObject.SetActive(true);
    }

    public static void Postfix(HudManager __instance)
    {
        initialized = false;

        try
        {
            createButtonsPostfix(__instance);
        }
        catch
        {
        }
    }

    public static void createButtonsPostfix(HudManager __instance)
    {
        // get map id, or raise error to wait...
        var mapId = GameOptionsManager.Instance.currentNormalGameOptions.MapId;

        // Engineer Repair
        engineerRepairButton = new CustomButton(
            () =>
            {
                engineerRepairButton.Timer = 0f;
                var usedRepairWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                    (byte)CustomRPC.EngineerUsedRepair, SendOption.Reliable);
                AmongUsClient.Instance.FinishRpcImmediately(usedRepairWriter);
                RPCProcedure.engineerUsedRepair();
                SoundEffectsManager.play("engineerRepair");
                foreach (var task in PlayerControl.LocalPlayer.myTasks.GetFastEnumerator())
                    if (task.TaskType == TaskTypes.FixLights)
                    {
                        var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                            (byte)CustomRPC.EngineerFixLights, SendOption.Reliable);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                        RPCProcedure.engineerFixLights();
                    }
                    else if (task.TaskType == TaskTypes.RestoreOxy)
                    {
                        MapUtilities.CachedShipStatus.RpcRepairSystem(SystemTypes.LifeSupp, 0 | 64);
                        MapUtilities.CachedShipStatus.RpcRepairSystem(SystemTypes.LifeSupp, 1 | 64);
                    }
                    else if (task.TaskType == TaskTypes.ResetReactor)
                    {
                        MapUtilities.CachedShipStatus.RpcRepairSystem(SystemTypes.Reactor, 16);
                    }
                    else if (task.TaskType == TaskTypes.ResetSeismic)
                    {
                        MapUtilities.CachedShipStatus.RpcRepairSystem(SystemTypes.Laboratory, 16);
                    }
                    else if (task.TaskType == TaskTypes.FixComms)
                    {
                        MapUtilities.CachedShipStatus.RpcRepairSystem(SystemTypes.Comms, 16 | 0);
                        MapUtilities.CachedShipStatus.RpcRepairSystem(SystemTypes.Comms, 16 | 1);
                    }
                    else if (task.TaskType == TaskTypes.StopCharles)
                    {
                        MapUtilities.CachedShipStatus.RpcRepairSystem(SystemTypes.Reactor, 0 | 16);
                        MapUtilities.CachedShipStatus.RpcRepairSystem(SystemTypes.Reactor, 1 | 16);
                    }
                    else if (SubmergedCompatibility.IsSubmerged &&
                             task.TaskType == SubmergedCompatibility.RetrieveOxygenMask)
                    {
                        var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                            (byte)CustomRPC.EngineerFixSubmergedOxygen, SendOption.Reliable);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                        RPCProcedure.engineerFixSubmergedOxygen();
                    }
            },
            () =>
            {
                return Engineer.engineer != null && Engineer.engineer == PlayerControl.LocalPlayer &&
                       Engineer.remainingFixes > 0 && !PlayerControl.LocalPlayer.Data.IsDead;
            },
            () =>
            {
                var sabotageActive = false;
                foreach (var task in PlayerControl.LocalPlayer.myTasks.GetFastEnumerator())
                    if (task.TaskType == TaskTypes.FixLights || task.TaskType == TaskTypes.RestoreOxy ||
                        task.TaskType == TaskTypes.ResetReactor || task.TaskType == TaskTypes.ResetSeismic ||
                        task.TaskType == TaskTypes.FixComms || task.TaskType == TaskTypes.StopCharles
                        || (SubmergedCompatibility.IsSubmerged &&
                            task.TaskType == SubmergedCompatibility.RetrieveOxygenMask))
                        sabotageActive = true;
                return sabotageActive && Engineer.remainingFixes > 0 && PlayerControl.LocalPlayer.CanMove;
            },
            () => { },
            Engineer.getButtonSprite(),
            CustomButton.ButtonPositions.upperRowRight,
            __instance,
            KeyCode.F,
            buttonText: "engineerRepair"
        );

        // Janitor Clean
        janitorCleanButton = new CustomButton(
            () =>
            {
                foreach (var collider2D in Physics2D.OverlapCircleAll(PlayerControl.LocalPlayer.GetTruePosition(),
                             PlayerControl.LocalPlayer.MaxReportDistance, Constants.PlayersOnlyMask))
                    if (collider2D.tag == "DeadBody")
                    {
                        var component = collider2D.GetComponent<DeadBody>();
                        if (component && !component.Reported)
                        {
                            var truePosition = PlayerControl.LocalPlayer.GetTruePosition();
                            var truePosition2 = component.TruePosition;
                            if (Vector2.Distance(truePosition2, truePosition) <=
                                PlayerControl.LocalPlayer.MaxReportDistance && PlayerControl.LocalPlayer.CanMove &&
                                !PhysicsHelpers.AnythingBetween(truePosition, truePosition2,
                                    Constants.ShipAndObjectsMask, false))
                            {
                                var playerInfo = GameData.Instance.GetPlayerById(component.ParentId);

                                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                                    (byte)CustomRPC.CleanBody, SendOption.Reliable);
                                writer.Write(playerInfo.PlayerId);
                                writer.Write(Janitor.janitor.PlayerId);
                                AmongUsClient.Instance.FinishRpcImmediately(writer);
                                RPCProcedure.cleanBody(playerInfo.PlayerId, Janitor.janitor.PlayerId);
                                janitorCleanButton.Timer = janitorCleanButton.MaxTimer;
                                SoundEffectsManager.play("cleanerClean");

                                break;
                            }
                        }
                    }
            },
            () =>
            {
                return Janitor.janitor != null && Janitor.janitor == PlayerControl.LocalPlayer &&
                       !PlayerControl.LocalPlayer.Data.IsDead;
            },
            () =>
            {
                return __instance.ReportButton.graphic.color == Palette.EnabledColor &&
                       PlayerControl.LocalPlayer.CanMove;
            },
            () => { janitorCleanButton.Timer = janitorCleanButton.MaxTimer; },
            Janitor.getButtonSprite(),
            CustomButton.ButtonPositions.upperRowLeft,
            __instance,
            KeyCode.F,
            buttonText: "cleanerClean"
        );

        // Sheriff Kill
        sheriffKillButton = new CustomButton(
            () =>
            {
                var murderAttemptResult = Helpers.checkMuderAttempt(Sheriff.sheriff, Sheriff.currentTarget);
                if (murderAttemptResult == MurderAttemptResult.SuppressKill) return;

                if (murderAttemptResult == MurderAttemptResult.PerformKill)
                {
                    byte targetId = 0;
                    if ((Sheriff.currentTarget.Data.Role.IsImpostor &&
                         (Sheriff.currentTarget != Mini.mini || Mini.isGrownUp())) ||
                        (Sheriff.spyCanDieToSheriff && Spy.spy == Sheriff.currentTarget) ||
                        (Sheriff.canKillNeutrals && Helpers.isNeutral(Sheriff.currentTarget)) ||
                        Jackal.jackal == Sheriff.currentTarget || Sidekick.sidekick == Sheriff.currentTarget)
                        targetId = Sheriff.currentTarget.PlayerId;
                    else
                        targetId = PlayerControl.LocalPlayer.PlayerId;

                    // Armored sheriff shot doesnt kill if backfired
                    if (targetId == Sheriff.sheriff.PlayerId && Helpers.checkArmored(Sheriff.sheriff, true, true))
                        return;
                    var killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                        (byte)CustomRPC.UncheckedMurderPlayer, SendOption.Reliable);
                    killWriter.Write(Sheriff.sheriff.Data.PlayerId);
                    killWriter.Write(targetId);
                    killWriter.Write(byte.MaxValue);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.uncheckedMurderPlayer(Sheriff.sheriff.Data.PlayerId, targetId, byte.MaxValue);
                }

                sheriffKillButton.Timer = sheriffKillButton.MaxTimer;
                Sheriff.currentTarget = null;
            },
            () =>
            {
                return Sheriff.sheriff != null && Sheriff.sheriff == PlayerControl.LocalPlayer &&
                       !PlayerControl.LocalPlayer.Data.IsDead;
            },
            () => { return Sheriff.currentTarget && PlayerControl.LocalPlayer.CanMove; },
            () => { sheriffKillButton.Timer = sheriffKillButton.MaxTimer; },
            Helpers.loadSpriteFromResources("SheriffKillButton.png", 115f),
            CustomButton.ButtonPositions.lowerRowRight,
            __instance,
            KeyCode.Q,
            buttonText: "Kill"
        );

        // Deputy Handcuff
        deputyHandcuffButton = new CustomButton(
            () =>
            {
                byte targetId = 0;
                targetId = Sheriff.sheriff == PlayerControl.LocalPlayer
                    ? Sheriff.currentTarget.PlayerId
                    : Deputy.currentTarget
                        .PlayerId; // If the deputy is now the sheriff, sheriffs target, else deputies target

                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                    (byte)CustomRPC.DeputyUsedHandcuffs, SendOption.Reliable);
                writer.Write(targetId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.deputyUsedHandcuffs(targetId);
                Deputy.currentTarget = null;
                deputyHandcuffButton.Timer = deputyHandcuffButton.MaxTimer;

                SoundEffectsManager.play("deputyHandcuff");
            },
            () =>
            {
                return ((Deputy.deputy != null && Deputy.deputy == PlayerControl.LocalPlayer) ||
                        (Sheriff.sheriff != null && Sheriff.sheriff == PlayerControl.LocalPlayer &&
                         Sheriff.sheriff == Sheriff.formerDeputy && Deputy.keepsHandcuffsOnPromotion)) &&
                       !PlayerControl.LocalPlayer.Data.IsDead;
            },
            () =>
            {
                if (deputyButtonHandcuffsText != null) deputyButtonHandcuffsText.text = $"{Deputy.remainingHandcuffs}";
                return ((Deputy.deputy != null && Deputy.deputy == PlayerControl.LocalPlayer && Deputy.currentTarget) ||
                        (Sheriff.sheriff != null && Sheriff.sheriff == PlayerControl.LocalPlayer &&
                         Sheriff.sheriff == Sheriff.formerDeputy && Sheriff.currentTarget)) &&
                       Deputy.remainingHandcuffs > 0 && PlayerControl.LocalPlayer.CanMove;
            },
            () => { deputyHandcuffButton.Timer = deputyHandcuffButton.MaxTimer; },
            Deputy.getButtonSprite(),
            CustomButton.ButtonPositions.lowerRowRight,
            __instance,
            KeyCode.F,
            buttonText: "deputyHandcuff"
        );
        // Deputy Handcuff button handcuff counter
        deputyButtonHandcuffsText = GameObject.Instantiate(deputyHandcuffButton.actionButton.cooldownTimerText,
            deputyHandcuffButton.actionButton.cooldownTimerText.transform.parent);
        deputyButtonHandcuffsText.text = "";
        deputyButtonHandcuffsText.enableWordWrapping = false;
        deputyButtonHandcuffsText.transform.localScale = Vector3.one * 0.5f;
        deputyButtonHandcuffsText.transform.localPosition += new Vector3(-0.05f, 0.7f, 0);

        // Time Master Rewind Time
        timeMasterShieldButton = new CustomButton(
            () =>
            {
                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                    (byte)CustomRPC.TimeMasterShield, SendOption.Reliable);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.timeMasterShield();
                SoundEffectsManager.play("timemasterShield");
            },
            () =>
            {
                return TimeMaster.timeMaster != null && TimeMaster.timeMaster == PlayerControl.LocalPlayer &&
                       !PlayerControl.LocalPlayer.Data.IsDead;
            },
            () => { return PlayerControl.LocalPlayer.CanMove; },
            () =>
            {
                timeMasterShieldButton.Timer = timeMasterShieldButton.MaxTimer;
                timeMasterShieldButton.isEffectActive = false;
                timeMasterShieldButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
            },
            TimeMaster.getButtonSprite(),
            CustomButton.ButtonPositions.lowerRowRight,
            __instance,
            KeyCode.F,
            true,
            TimeMaster.shieldDuration,
            () =>
            {
                timeMasterShieldButton.Timer = timeMasterShieldButton.MaxTimer;
                SoundEffectsManager.stop("timemasterShield");
            },
            buttonText: "timemasterShield"
        );

        // Medic Shield
        medicShieldButton = new CustomButton(
            () =>
            {
                medicShieldButton.Timer = 0f;

                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                    Medic.setShieldAfterMeeting ? (byte)CustomRPC.SetFutureShielded : (byte)CustomRPC.MedicSetShielded,
                    SendOption.Reliable);
                writer.Write(Medic.currentTarget.PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                if (Medic.setShieldAfterMeeting)
                    RPCProcedure.setFutureShielded(Medic.currentTarget.PlayerId);
                else
                    RPCProcedure.medicSetShielded(Medic.currentTarget.PlayerId);
                Medic.meetingAfterShielding = false;

                SoundEffectsManager.play("medicShield");
            },
            () =>
            {
                return Medic.medic != null && Medic.medic == PlayerControl.LocalPlayer &&
                       !PlayerControl.LocalPlayer.Data.IsDead;
            },
            () => { return !Medic.usedShield && Medic.currentTarget && PlayerControl.LocalPlayer.CanMove; },
            () => { },
            Medic.getButtonSprite(),
            CustomButton.ButtonPositions.lowerRowRight,
            __instance,
            KeyCode.F,
            buttonText: "medicShield"
        );


        // Shifter shift
        shifterShiftButton = new CustomButton(
            () =>
            {
                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                    (byte)CustomRPC.SetFutureShifted, SendOption.Reliable);
                writer.Write(Shifter.currentTarget.PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.setFutureShifted(Shifter.currentTarget.PlayerId);
                SoundEffectsManager.play("shifterShift");
            },
            () =>
            {
                return Shifter.shifter != null && Shifter.shifter == PlayerControl.LocalPlayer &&
                       Shifter.futureShift == null && !PlayerControl.LocalPlayer.Data.IsDead;
            },
            () => { return Shifter.currentTarget && Shifter.futureShift == null && PlayerControl.LocalPlayer.CanMove; },
            () => { },
            Shifter.getButtonSprite(),
            new Vector3(0, 1f, 0),
            __instance,
            null,
            true,
            "shifterShift"
        );

        // Morphling morph

        morphlingButton = new CustomButton(
            () =>
            {
                if (Morphling.sampledTarget != null)
                {
                    var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                        (byte)CustomRPC.MorphlingMorph, SendOption.Reliable);
                    writer.Write(Morphling.sampledTarget.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    RPCProcedure.morphlingMorph(Morphling.sampledTarget.PlayerId);
                    Morphling.sampledTarget = null;
                    morphlingButton.EffectDuration = Morphling.duration;
                    SoundEffectsManager.play("morphlingMorph");
                    morphlingButton.buttonText = "morphlingSample".Translate();
                }
                else if (Morphling.currentTarget != null)
                {
                    Morphling.sampledTarget = Morphling.currentTarget;
                    morphlingButton.Sprite = Morphling.getMorphSprite();
                    morphlingButton.EffectDuration = 1f;
                    SoundEffectsManager.play("morphlingSample");
                    morphlingButton.buttonText = "morphlingMorph".Translate();

                    // Add poolable player to the button so that the target outfit is shown
                    setButtonTargetDisplay(Morphling.sampledTarget, morphlingButton);
                }
            },
            () =>
            {
                return Morphling.morphling != null && Morphling.morphling == PlayerControl.LocalPlayer &&
                       !PlayerControl.LocalPlayer.Data.IsDead;
            },
            () =>
            {
                return (Morphling.currentTarget || Morphling.sampledTarget) && PlayerControl.LocalPlayer.CanMove &&
                       !Helpers.MushroomSabotageActive();
            },
            () =>
            {
                morphlingButton.Timer = morphlingButton.MaxTimer;
                morphlingButton.Sprite = Morphling.getSampleSprite();
                morphlingButton.isEffectActive = false;
                morphlingButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
                Morphling.sampledTarget = null;
                setButtonTargetDisplay(null);
            },
            Morphling.getSampleSprite(),
            CustomButton.ButtonPositions.upperRowLeft,
            __instance,
            KeyCode.F,
            true,
            Morphling.duration,
            () =>
            {
                if (Morphling.sampledTarget == null)
                {
                    morphlingButton.Timer = morphlingButton.MaxTimer;
                    morphlingButton.Sprite = Morphling.getSampleSprite();
                    SoundEffectsManager.play("morphlingMorph");

                    // Reset the poolable player
                    setButtonTargetDisplay(null);
                }
            },
            buttonText: "morphlingSample"
        );

        // Camouflager camouflage
        camouflagerButton = new CustomButton(
            () =>
            {
                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                    (byte)CustomRPC.CamouflagerCamouflage, SendOption.Reliable);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.camouflagerCamouflage();
                SoundEffectsManager.play("morphlingMorph");
            },
            () =>
            {
                return Camouflager.camouflager != null && Camouflager.camouflager == PlayerControl.LocalPlayer &&
                       !PlayerControl.LocalPlayer.Data.IsDead;
            },
            () => { return PlayerControl.LocalPlayer.CanMove; },
            () =>
            {
                camouflagerButton.Timer = camouflagerButton.MaxTimer;
                camouflagerButton.isEffectActive = false;
                camouflagerButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
            },
            Camouflager.getButtonSprite(),
            CustomButton.ButtonPositions.upperRowLeft,
            __instance,
            KeyCode.F,
            true,
            Camouflager.duration,
            () =>
            {
                camouflagerButton.Timer = camouflagerButton.MaxTimer;
                SoundEffectsManager.play("morphlingMorph");
            },
            buttonText: "camouflagerCamo"
        );

        // Hacker button
        hackerButton = new CustomButton(
            () =>
            {
                Hacker.hackerTimer = Hacker.duration;
                SoundEffectsManager.play("hackerHack");
            },
            () =>
            {
                return Hacker.hacker != null && Hacker.hacker == PlayerControl.LocalPlayer &&
                       !PlayerControl.LocalPlayer.Data.IsDead;
            },
            () => { return true; },
            () =>
            {
                hackerButton.Timer = hackerButton.MaxTimer;
                hackerButton.isEffectActive = false;
                hackerButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
            },
            Hacker.getButtonSprite(),
            CustomButton.ButtonPositions.upperRowRight,
            __instance,
            KeyCode.F,
            true,
            0f,
            () => { hackerButton.Timer = hackerButton.MaxTimer; },
            buttonText: "hackerHack"
        );

        hackerAdminTableButton = new CustomButton(
            () =>
            {
                if (!MapBehaviour.Instance || !MapBehaviour.Instance.isActiveAndEnabled)
                {
                    var __instance = FastDestroyableSingleton<HudManager>.Instance;
                    __instance.InitMap();
                    MapBehaviour.Instance.ShowCountOverlay(true, true, true);
                }

                if (Hacker.cantMove) PlayerControl.LocalPlayer.moveable = false;
                PlayerControl.LocalPlayer.NetTransform.Halt(); // Stop current movement 
                Hacker.chargesAdminTable--;
            },
            () =>
            {
                return Hacker.hacker != null && Hacker.hacker == PlayerControl.LocalPlayer &&
                       !PlayerControl.LocalPlayer.Data.IsDead;
            },
            () =>
            {
                if (hackerAdminTableChargesText != null)
                    hackerAdminTableChargesText.text = $"{Hacker.chargesAdminTable} / {Hacker.toolsNumber}";
                return Hacker.chargesAdminTable > 0;
            },
            () =>
            {
                hackerAdminTableButton.Timer = hackerAdminTableButton.MaxTimer;
                hackerAdminTableButton.isEffectActive = false;
                hackerAdminTableButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
            },
            Hacker.getAdminSprite(),
            CustomButton.ButtonPositions.lowerRowRight,
            __instance,
            KeyCode.G,
            true,
            0f,
            () =>
            {
                hackerAdminTableButton.Timer = hackerAdminTableButton.MaxTimer;
                if (!hackerVitalsButton.isEffectActive) PlayerControl.LocalPlayer.moveable = true;
                if (MapBehaviour.Instance && MapBehaviour.Instance.isActiveAndEnabled) MapBehaviour.Instance.Close();
            },
            GameOptionsManager.Instance.currentNormalGameOptions.MapId == 3,
            FastDestroyableSingleton<TranslationController>.Instance.GetString(StringNames.Admin)
        );

        // Hacker Admin Table Charges
        hackerAdminTableChargesText = GameObject.Instantiate(hackerAdminTableButton.actionButton.cooldownTimerText,
            hackerAdminTableButton.actionButton.cooldownTimerText.transform.parent);
        hackerAdminTableChargesText.text = "";
        hackerAdminTableChargesText.enableWordWrapping = false;
        hackerAdminTableChargesText.transform.localScale = Vector3.one * 0.5f;
        hackerAdminTableChargesText.transform.localPosition += new Vector3(-0.05f, 0.7f, 0);

        hackerVitalsButton = new CustomButton(
            () =>
            {
                if (GameOptionsManager.Instance.currentNormalGameOptions.MapId != 1)
                {
                    if (Hacker.vitals == null)
                    {
                        var e = Object.FindObjectsOfType<SystemConsole>().FirstOrDefault(x =>
                            x.gameObject.name.Contains("panel_vitals") || x.gameObject.name.Contains("Vitals"));
                        if (e == null || Camera.main == null) return;
                        Hacker.vitals = Object.Instantiate(e.MinigamePrefab, Camera.main.transform, false);
                    }

                    Hacker.vitals.transform.SetParent(Camera.main.transform, false);
                    Hacker.vitals.transform.localPosition = new Vector3(0.0f, 0.0f, -50f);
                    Hacker.vitals.Begin(null);
                }
                else
                {
                    if (Hacker.doorLog == null)
                    {
                        var e = Object.FindObjectsOfType<SystemConsole>()
                            .FirstOrDefault(x => x.gameObject.name.Contains("SurvLogConsole"));
                        if (e == null || Camera.main == null) return;
                        Hacker.doorLog = Object.Instantiate(e.MinigamePrefab, Camera.main.transform, false);
                    }

                    Hacker.doorLog.transform.SetParent(Camera.main.transform, false);
                    Hacker.doorLog.transform.localPosition = new Vector3(0.0f, 0.0f, -50f);
                    Hacker.doorLog.Begin(null);
                }

                if (Hacker.cantMove) PlayerControl.LocalPlayer.moveable = false;
                PlayerControl.LocalPlayer.NetTransform.Halt(); // Stop current movement 

                Hacker.chargesVitals--;
            },
            () =>
            {
                return Hacker.hacker != null && Hacker.hacker == PlayerControl.LocalPlayer &&
                       !PlayerControl.LocalPlayer.Data.IsDead &&
                       GameOptionsManager.Instance.currentGameOptions.MapId != 0 &&
                       GameOptionsManager.Instance.currentNormalGameOptions.MapId != 3;
            },
            () =>
            {
                if (hackerVitalsChargesText != null)
                    hackerVitalsChargesText.text = $"{Hacker.chargesVitals} / {Hacker.toolsNumber}";
                hackerVitalsButton.actionButton.graphic.sprite =
                    Helpers.isMira() ? Hacker.getLogSprite() : Hacker.getVitalsSprite();
                hackerVitalsButton.actionButton.OverrideText(Helpers.isMira()
                    ? FastDestroyableSingleton<TranslationController>.Instance.GetString(StringNames.DoorlogLabel)
                    : FastDestroyableSingleton<TranslationController>.Instance.GetString(StringNames.VitalsAbility));
                return Hacker.chargesVitals > 0;
            },
            () =>
            {
                hackerVitalsButton.Timer = hackerVitalsButton.MaxTimer;
                hackerVitalsButton.isEffectActive = false;
                hackerVitalsButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
            },
            Hacker.getVitalsSprite(),
            CustomButton.ButtonPositions.lowerRowCenter,
            __instance,
            KeyCode.H,
            true,
            0f,
            () =>
            {
                hackerVitalsButton.Timer = hackerVitalsButton.MaxTimer;
                if (!hackerAdminTableButton.isEffectActive) PlayerControl.LocalPlayer.moveable = true;
                if (Minigame.Instance)
                {
                    if (Helpers.isMira()) Hacker.doorLog.ForceClose();
                    else Hacker.vitals.ForceClose();
                }
            },
            false,
            Helpers.isMira()
                ? FastDestroyableSingleton<TranslationController>.Instance.GetString(StringNames.DoorlogLabel)
                : FastDestroyableSingleton<TranslationController>.Instance.GetString(StringNames.VitalsAbility)
        );

        // Hacker Vitals Charges
        hackerVitalsChargesText = GameObject.Instantiate(hackerVitalsButton.actionButton.cooldownTimerText,
            hackerVitalsButton.actionButton.cooldownTimerText.transform.parent);
        hackerVitalsChargesText.text = "";
        hackerVitalsChargesText.enableWordWrapping = false;
        hackerVitalsChargesText.transform.localScale = Vector3.one * 0.5f;
        hackerVitalsChargesText.transform.localPosition += new Vector3(-0.05f, 0.7f, 0);

        // Tracker button
        trackerTrackPlayerButton = new CustomButton(
            () =>
            {
                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                    (byte)CustomRPC.TrackerUsedTracker, SendOption.Reliable);
                writer.Write(Tracker.currentTarget.PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.trackerUsedTracker(Tracker.currentTarget.PlayerId);
                SoundEffectsManager.play("trackerTrackPlayer");
            },
            () =>
            {
                return Tracker.tracker != null && Tracker.tracker == PlayerControl.LocalPlayer &&
                       !PlayerControl.LocalPlayer.Data.IsDead;
            },
            () =>
            {
                return PlayerControl.LocalPlayer.CanMove && Tracker.currentTarget != null && !Tracker.usedTracker;
            },
            () =>
            {
                if (Tracker.resetTargetAfterMeeting) Tracker.resetTracked();
                else if (Tracker.currentTarget != null && Tracker.currentTarget.Data.IsDead)
                    Tracker.currentTarget = null;
            },
            Tracker.getButtonSprite(),
            CustomButton.ButtonPositions.lowerRowRight,
            __instance,
            KeyCode.F,
            buttonText: "trackerTrackPlayer"
        );

        trackerTrackCorpsesButton = new CustomButton(
            () =>
            {
                Tracker.corpsesTrackingTimer = Tracker.corpsesTrackingDuration;
                SoundEffectsManager.play("trackerTrackCorpses");
            },
            () =>
            {
                return Tracker.tracker != null && Tracker.tracker == PlayerControl.LocalPlayer &&
                       !PlayerControl.LocalPlayer.Data.IsDead && Tracker.canTrackCorpses;
            },
            () => { return PlayerControl.LocalPlayer.CanMove; },
            () =>
            {
                trackerTrackCorpsesButton.Timer = trackerTrackCorpsesButton.MaxTimer;
                trackerTrackCorpsesButton.isEffectActive = false;
                trackerTrackCorpsesButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
            },
            Tracker.getTrackCorpsesButtonSprite(),
            CustomButton.ButtonPositions.lowerRowCenter,
            __instance,
            KeyCode.G,
            true,
            Tracker.corpsesTrackingDuration,
            () => { trackerTrackCorpsesButton.Timer = trackerTrackCorpsesButton.MaxTimer; },
            buttonText: "trackerTrackCorpses"
        );

        vampireKillButton = new CustomButton(
            () =>
            {
                var murder = Helpers.checkMuderAttempt(Vampire.vampire, Vampire.currentTarget);
                if (murder == MurderAttemptResult.PerformKill)
                {
                    if (Vampire.targetNearGarlic)
                    {
                        var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                            (byte)CustomRPC.UncheckedMurderPlayer, SendOption.Reliable);
                        writer.Write(Vampire.vampire.PlayerId);
                        writer.Write(Vampire.currentTarget.PlayerId);
                        writer.Write(byte.MaxValue);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                        RPCProcedure.uncheckedMurderPlayer(Vampire.vampire.PlayerId, Vampire.currentTarget.PlayerId,
                            byte.MaxValue);

                        vampireKillButton.HasEffect = false; // Block effect on this click
                        vampireKillButton.Timer = vampireKillButton.MaxTimer;
                    }
                    else
                    {
                        Vampire.bitten = Vampire.currentTarget;
                        // Notify players about bitten
                        var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                            (byte)CustomRPC.VampireSetBitten, SendOption.Reliable);
                        writer.Write(Vampire.bitten.PlayerId);
                        writer.Write((byte)0);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                        RPCProcedure.vampireSetBitten(Vampire.bitten.PlayerId, 0);

                        var lastTimer = (byte)Vampire.delay;
                        FastDestroyableSingleton<HudManager>.Instance.StartCoroutine(Effects.Lerp(Vampire.delay,
                            new Action<float>(p =>
                            {
                                // Delayed action
                                if (p <= 1f)
                                {
                                    var timer = (byte)vampireKillButton.Timer;
                                    if (timer != lastTimer)
                                    {
                                        lastTimer = timer;
                                        var writer = AmongUsClient.Instance.StartRpcImmediately(
                                            PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ShareGhostInfo,
                                            SendOption.Reliable);
                                        writer.Write(PlayerControl.LocalPlayer.PlayerId);
                                        writer.Write((byte)RPCProcedure.GhostInfoTypes.VampireTimer);
                                        writer.Write(timer);
                                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                                    }
                                }

                                if (p == 1f)
                                {
                                    // Perform kill if possible and reset bitten (regardless whether the kill was successful or not)
                                    var res = Helpers.checkMurderAttemptAndKill(Vampire.vampire, Vampire.bitten,
                                        showAnimation: false);
                                    if (res == MurderAttemptResult.PerformKill)
                                    {
                                        var writer = AmongUsClient.Instance.StartRpcImmediately(
                                            PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.VampireSetBitten,
                                            SendOption.Reliable);
                                        writer.Write(byte.MaxValue);
                                        writer.Write(byte.MaxValue);
                                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                                        RPCProcedure.vampireSetBitten(byte.MaxValue, byte.MaxValue);
                                    }
                                }
                            })));
                        SoundEffectsManager.play("vampireBite");

                        vampireKillButton.HasEffect = true; // Trigger effect on this click
                    }
                }
                else if (murder == MurderAttemptResult.BlankKill)
                {
                    vampireKillButton.Timer = vampireKillButton.MaxTimer;
                    vampireKillButton.HasEffect = false;
                }
                else
                {
                    vampireKillButton.HasEffect = false;
                }
            },
            () =>
            {
                return Vampire.vampire != null && Vampire.vampire == PlayerControl.LocalPlayer &&
                       !PlayerControl.LocalPlayer.Data.IsDead;
            },
            () =>
            {
                if (Vampire.targetNearGarlic && Vampire.canKillNearGarlics)
                {
                    vampireKillButton.actionButton.graphic.sprite = __instance.KillButton.graphic.sprite;
                    vampireKillButton.showButtonText = true;
                }
                else
                {
                    vampireKillButton.actionButton.graphic.sprite = Vampire.getButtonSprite();
                    vampireKillButton.showButtonText = false;
                }

                return Vampire.currentTarget != null && PlayerControl.LocalPlayer.CanMove &&
                       (!Vampire.targetNearGarlic || Vampire.canKillNearGarlics);
            },
            () =>
            {
                vampireKillButton.Timer = vampireKillButton.MaxTimer;
                vampireKillButton.isEffectActive = false;
                vampireKillButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
            },
            Vampire.getButtonSprite(),
            CustomButton.ButtonPositions.upperRowLeft,
            __instance,
            KeyCode.Q,
            false,
            0f,
            () => { vampireKillButton.Timer = vampireKillButton.MaxTimer; },
            buttonText: "vampireBite"
        );

        fraudsterButton = new CustomButton(
            () =>
            {

                byte targetId = PlayerControl.LocalPlayer.PlayerId;
                MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.Suicide, Hazel.SendOption.Reliable, -1); killWriter.Write(targetId);
                AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                RPCProcedure.serialKillerSuicide(targetId);
            },
            () => { return PlayerControl.LocalPlayer == Fraudster.fraudster && !PlayerControl.LocalPlayer.Data.IsDead; },
            () => { return true; },
            () =>
            {
                fraudsterButton.Timer = fraudsterButton.MaxTimer = 20f;
            },
            Fraudster.getButtonSprite(),
            CustomButton.ButtonPositions.upperRowLeft,
            __instance,
            KeyCode.F,
            buttonText: "fraudsterButtonSuicide"
        );

        garlicButton = new CustomButton(
            () =>
            {
                Vampire.localPlacedGarlic = true;
                var pos = PlayerControl.LocalPlayer.transform.position;
                var buff = new byte[sizeof(float) * 2];
                Buffer.BlockCopy(BitConverter.GetBytes(pos.x), 0, buff, 0 * sizeof(float), sizeof(float));
                Buffer.BlockCopy(BitConverter.GetBytes(pos.y), 0, buff, 1 * sizeof(float), sizeof(float));

                var writer =
                    AmongUsClient.Instance.StartRpc(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.PlaceGarlic);
                writer.WriteBytesAndSize(buff);
                writer.EndMessage();
                RPCProcedure.placeGarlic(buff);
                SoundEffectsManager.play("garlic");
            },
            () =>
            {
                return !Vampire.localPlacedGarlic && !PlayerControl.LocalPlayer.Data.IsDead && Vampire.garlicsActive &&
                       !HideNSeek.isHideNSeekGM && !PropHunt.isPropHuntGM;
            },
            () => { return PlayerControl.LocalPlayer.CanMove && !Vampire.localPlacedGarlic; },
            () => { },
            Vampire.getGarlicButtonSprite(),
            new Vector3(0, -0.06f, 0),
            __instance,
            null,
            true,
            "planGarlic"
        );

        portalmakerPlacePortalButton = new CustomButton(
            () =>
            {
                portalmakerPlacePortalButton.Timer = portalmakerPlacePortalButton.MaxTimer;

                var pos = PlayerControl.LocalPlayer.transform.position;
                var buff = new byte[sizeof(float) * 2];
                Buffer.BlockCopy(BitConverter.GetBytes(pos.x), 0, buff, 0 * sizeof(float), sizeof(float));
                Buffer.BlockCopy(BitConverter.GetBytes(pos.y), 0, buff, 1 * sizeof(float), sizeof(float));

                var writer =
                    AmongUsClient.Instance.StartRpc(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.PlacePortal);
                writer.WriteBytesAndSize(buff);
                writer.EndMessage();
                RPCProcedure.placePortal(buff);
                SoundEffectsManager.play("tricksterPlaceBox");
            },
            () =>
            {
                return Portalmaker.portalmaker != null && Portalmaker.portalmaker == PlayerControl.LocalPlayer &&
                       !PlayerControl.LocalPlayer.Data.IsDead && Portal.secondPortal == null;
            },
            () => { return PlayerControl.LocalPlayer.CanMove && Portal.secondPortal == null; },
            () => { portalmakerPlacePortalButton.Timer = portalmakerPlacePortalButton.MaxTimer; },
            Portalmaker.getPlacePortalButtonSprite(),
            CustomButton.ButtonPositions.lowerRowRight,
            __instance,
            KeyCode.F,
            buttonText: "tricksterPlaceBox"
        );

        usePortalButton = new CustomButton(
            () =>
            {
                var didTeleport = false;
                Vector3 exit = Portal.findExit(PlayerControl.LocalPlayer.transform.position);
                Vector3 entry = Portal.findEntry(PlayerControl.LocalPlayer.transform.position);

                var portalMakerSoloTeleport = !Portal.locationNearEntry(PlayerControl.LocalPlayer.transform.position);
                if (portalMakerSoloTeleport)
                {
                    exit = Portal.firstPortal.portalGameObject.transform.position;
                    entry = PlayerControl.LocalPlayer.transform.position;
                }

                PlayerControl.LocalPlayer.NetTransform.RpcSnapTo(entry);

                if (!PlayerControl.LocalPlayer.Data.IsDead)
                {
                    // Ghosts can portal too, but non-blocking and only with a local animation
                    var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                        (byte)CustomRPC.UsePortal, SendOption.Reliable);
                    writer.Write(PlayerControl.LocalPlayer.PlayerId);
                    writer.Write(portalMakerSoloTeleport ? (byte)1 : (byte)0);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                }

                RPCProcedure.usePortal(PlayerControl.LocalPlayer.PlayerId, portalMakerSoloTeleport ? (byte)1 : (byte)0);
                usePortalButton.Timer = usePortalButton.MaxTimer;
                portalmakerMoveToPortalButton.Timer = usePortalButton.MaxTimer;
                SoundEffectsManager.play("portalUse");
                FastDestroyableSingleton<HudManager>.Instance.StartCoroutine(Effects.Lerp(Portal.teleportDuration,
                    new Action<float>(p =>
                    {
                        // Delayed action
                        PlayerControl.LocalPlayer.moveable = false;
                        PlayerControl.LocalPlayer.NetTransform.Halt();
                        if (p >= 0.5f && p <= 0.53f && !didTeleport && !MeetingHud.Instance)
                        {
                            if (SubmergedCompatibility.IsSubmerged) SubmergedCompatibility.ChangeFloor(exit.y > -7);
                            PlayerControl.LocalPlayer.NetTransform.RpcSnapTo(exit);
                            didTeleport = true;
                        }

                        if (p == 1f) PlayerControl.LocalPlayer.moveable = true;
                    })));
            },
            () =>
            {
                if (PlayerControl.LocalPlayer == Portalmaker.portalmaker && Portal.bothPlacedAndEnabled)
                    portalmakerButtonText1.text =
                        Portal.locationNearEntry(PlayerControl.LocalPlayer.transform.position) ||
                        !Portalmaker.canPortalFromAnywhere
                            ? ""
                            : "1. " + Portal.firstPortal.room;
                return Portal.bothPlacedAndEnabled;
            },
            () =>
            {
                return PlayerControl.LocalPlayer.CanMove &&
                       (Portal.locationNearEntry(PlayerControl.LocalPlayer.transform.position) ||
                        (Portalmaker.canPortalFromAnywhere && PlayerControl.LocalPlayer == Portalmaker.portalmaker)) &&
                       !Portal.isTeleporting;
            },
            () => { usePortalButton.Timer = usePortalButton.MaxTimer; },
            Portalmaker.getUsePortalButtonSprite(),
            new Vector3(0.9f, -0.06f, 0),
            __instance,
            KeyCode.J,
            true,
            FastDestroyableSingleton<TranslationController>.Instance.GetString(StringNames.UseLabel)
        );

        portalmakerMoveToPortalButton = new CustomButton(
            () =>
            {
                var didTeleport = false;
                var exit = Portal.secondPortal.portalGameObject.transform.position;

                if (!PlayerControl.LocalPlayer.Data.IsDead)
                {
                    // Ghosts can portal too, but non-blocking and only with a local animation
                    var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                        (byte)CustomRPC.UsePortal, SendOption.Reliable);
                    writer.Write(PlayerControl.LocalPlayer.PlayerId);
                    writer.Write((byte)2);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                }

                RPCProcedure.usePortal(PlayerControl.LocalPlayer.PlayerId, 2);
                usePortalButton.Timer = usePortalButton.MaxTimer;
                portalmakerMoveToPortalButton.Timer = usePortalButton.MaxTimer;
                SoundEffectsManager.play("portalUse");
                FastDestroyableSingleton<HudManager>.Instance.StartCoroutine(Effects.Lerp(Portal.teleportDuration,
                    new Action<float>(p =>
                    {
                        // Delayed action
                        PlayerControl.LocalPlayer.moveable = false;
                        PlayerControl.LocalPlayer.NetTransform.Halt();
                        if (p >= 0.5f && p <= 0.53f && !didTeleport && !MeetingHud.Instance)
                        {
                            if (SubmergedCompatibility.IsSubmerged) SubmergedCompatibility.ChangeFloor(exit.y > -7);
                            PlayerControl.LocalPlayer.NetTransform.RpcSnapTo(exit);
                            didTeleport = true;
                        }

                        if (p == 1f) PlayerControl.LocalPlayer.moveable = true;
                    })));
            },
            () =>
            {
                return Portalmaker.canPortalFromAnywhere && Portal.bothPlacedAndEnabled &&
                       PlayerControl.LocalPlayer == Portalmaker.portalmaker;
            },
            () =>
            {
                return PlayerControl.LocalPlayer.CanMove &&
                       !Portal.locationNearEntry(PlayerControl.LocalPlayer.transform.position) && !Portal.isTeleporting;
            },
            () => { portalmakerMoveToPortalButton.Timer = usePortalButton.MaxTimer; },
            Portalmaker.getUsePortalButtonSprite(),
            new Vector3(0.9f, 1f, 0),
            __instance,
            KeyCode.G,
            true,
            FastDestroyableSingleton<TranslationController>.Instance.GetString(StringNames.UseLabel)
        );


        portalmakerButtonText1 = GameObject.Instantiate(usePortalButton.actionButton.cooldownTimerText,
            usePortalButton.actionButton.cooldownTimerText.transform.parent);
        portalmakerButtonText1.text = "";
        portalmakerButtonText1.enableWordWrapping = false;
        portalmakerButtonText1.transform.localScale = Vector3.one * 0.5f;
        portalmakerButtonText1.transform.localPosition += new Vector3(-0.05f, 0.55f, -1f);

        portalmakerButtonText2 = GameObject.Instantiate(portalmakerMoveToPortalButton.actionButton.cooldownTimerText,
            portalmakerMoveToPortalButton.actionButton.cooldownTimerText.transform.parent);
        portalmakerButtonText2.text = "";
        portalmakerButtonText2.enableWordWrapping = false;
        portalmakerButtonText2.transform.localScale = Vector3.one * 0.5f;
        portalmakerButtonText2.transform.localPosition += new Vector3(-0.05f, 0.55f, -1f);


        // Jackal Sidekick Button
        jackalSidekickButton = new CustomButton(
            () =>
            {
                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                    (byte)CustomRPC.JackalCreatesSidekick, SendOption.Reliable);
                writer.Write(Jackal.currentTarget.PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.jackalCreatesSidekick(Jackal.currentTarget.PlayerId);
                SoundEffectsManager.play("jackalSidekick");
            },
            () =>
            {
                return Jackal.canCreateSidekick && Jackal.jackal != null &&
                       Jackal.jackal == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead;
            },
            () =>
            {
                return Jackal.canCreateSidekick && Jackal.currentTarget != null && PlayerControl.LocalPlayer.CanMove;
            },
            () => { jackalSidekickButton.Timer = jackalSidekickButton.MaxTimer; },
            Jackal.getSidekickButtonSprite(),
            CustomButton.ButtonPositions.lowerRowCenter,
            __instance,
            KeyCode.F,
            buttonText: "jackalSidekick"
        );

        // Jackal Kill
        jackalKillButton = new CustomButton(
            () =>
            {
                if (Helpers.checkMurderAttemptAndKill(Jackal.jackal, Jackal.currentTarget) ==
                    MurderAttemptResult.SuppressKill) return;

                jackalKillButton.Timer = jackalKillButton.MaxTimer;
                Jackal.currentTarget = null;
            },
            () =>
            {
                return Jackal.jackal != null && Jackal.jackal == PlayerControl.LocalPlayer &&
                       !PlayerControl.LocalPlayer.Data.IsDead;
            },
            () => { return Jackal.currentTarget && PlayerControl.LocalPlayer.CanMove; },
            () => { jackalKillButton.Timer = jackalKillButton.MaxTimer; },
            __instance.KillButton.graphic.sprite,
            CustomButton.ButtonPositions.upperRowRight,
            __instance,
            KeyCode.Q
        );

        // Sidekick Kill
        sidekickKillButton = new CustomButton(
            () =>
            {
                if (Helpers.checkMurderAttemptAndKill(Sidekick.sidekick, Sidekick.currentTarget) ==
                    MurderAttemptResult.SuppressKill) return;
                sidekickKillButton.Timer = sidekickKillButton.MaxTimer;
                Sidekick.currentTarget = null;
            },
            () =>
            {
                return Sidekick.canKill && Sidekick.sidekick != null &&
                       Sidekick.sidekick == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead;
            },
            () => { return Sidekick.currentTarget && PlayerControl.LocalPlayer.CanMove; },
            () => { sidekickKillButton.Timer = sidekickKillButton.MaxTimer; },
            __instance.KillButton.graphic.sprite,
            CustomButton.ButtonPositions.upperRowRight,
            __instance,
            KeyCode.Q
        );

        jackalAndSidekickSabotageLightsButton = new CustomButton(
            () =>
            {
                ShipStatus.Instance.RpcUpdateSystem(SystemTypes.Sabotage, (byte)SystemTypes.Electrical);
                SoundEffectsManager.play("ligherLight");
            },
            () =>
            {
                return ((Jackal.jackal != null && Jackal.jackal == PlayerControl.LocalPlayer &&
                         Jackal.canSabotageLights) ||
                        (Sidekick.sidekick != null && Sidekick.sidekick == PlayerControl.LocalPlayer &&
                         Sidekick.canSabotageLights)) && !PlayerControl.LocalPlayer.Data.IsDead;
            },
            () =>
            {
                if (Helpers.sabotageTimer() > jackalAndSidekickSabotageLightsButton.Timer || Helpers.sabotageActive())
                    jackalAndSidekickSabotageLightsButton.Timer =
                        Helpers.sabotageTimer() + 5f; // this will give imps time to do another sabotage.
                return Helpers.canUseSabotage();
            },
            () => { jackalAndSidekickSabotageLightsButton.Timer = Helpers.sabotageTimer() + 5f; },
            Trickster.getLightsOutButtonSprite(),
            CustomButton.ButtonPositions.upperRowCenter,
            __instance,
            KeyCode.G,
            buttonText: "lightOutButton"
        );

        // Eraser erase button
        eraserButton = new CustomButton(
            () =>
            {
                eraserButton.MaxTimer += 10;
                eraserButton.Timer = eraserButton.MaxTimer;

                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                    (byte)CustomRPC.SetFutureErased, SendOption.Reliable);
                writer.Write(Eraser.currentTarget.PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.setFutureErased(Eraser.currentTarget.PlayerId);
                SoundEffectsManager.play("eraserErase");
            },
            () =>
            {
                return Eraser.eraser != null && Eraser.eraser == PlayerControl.LocalPlayer &&
                       !PlayerControl.LocalPlayer.Data.IsDead;
            },
            () => { return PlayerControl.LocalPlayer.CanMove && Eraser.currentTarget != null; },
            () => { eraserButton.Timer = eraserButton.MaxTimer; },
            Eraser.getButtonSprite(),
            CustomButton.ButtonPositions.upperRowLeft,
            __instance,
            KeyCode.F,
            buttonText: "eraserErase"
        );

        devilButton = new CustomButton(
            () =>
            {
                devilButton.Timer = devilButton.MaxTimer;

                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                    (byte)CustomRPC.SetFutureBlinded, SendOption.Reliable);
                writer.Write(Devil.currentTarget.PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.setFutureBlinded(Devil.currentTarget.PlayerId);
                SoundEffectsManager.play("ligherLight");
            },
            () =>
            {
                return Devil.devil != null && Devil.devil == PlayerControl.LocalPlayer &&
                       !PlayerControl.LocalPlayer.Data.IsDead;
            },
            () => { return PlayerControl.LocalPlayer.CanMove && Devil.currentTarget != null; },
            () => { devilButton.Timer = devilButton.MaxTimer; },
            Devil.getButtonSprite(),
            CustomButton.ButtonPositions.upperRowLeft,
            __instance,
            KeyCode.F,
            buttonText: "devilButtonBlind"
        );

        peacedoveButton = new CustomButton(
            () =>
            {
                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                    (byte)CustomRPC.ReloadCooldowns, SendOption.Reliable);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.reloadCooldowns();
            },
            () =>
            {
                return PeaceDove.peacedove != null && PeaceDove.peacedove == PlayerControl.LocalPlayer &&
                       PeaceDove.reloadMaxNum > 0 && !PlayerControl.LocalPlayer.Data.IsDead;
            },
            () =>
            {
                return PlayerControl.LocalPlayer.CanMove;
            },
            () => {peacedoveButton.Timer = peacedoveButton.MaxTimer; },
            PeaceDove.getButtonSprite(),
            CustomButton.ButtonPositions.upperRowRight,
            __instance,
            KeyCode.F,
            buttonText: "peacedoveButtonReload"
        );


        prophetButton = new CustomButton(

            () =>
            {
                if (Prophet.currentTarget != null)
                {
                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ProphetExamine, Hazel.SendOption.Reliable, -1);
                    writer.Write(Prophet.currentTarget.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    RPCProcedure.prophetExamine(Prophet.currentTarget.PlayerId);

                    prophetButton.Timer = prophetButton.MaxTimer;
                }
            },
               () => { return Prophet.prophet != null && PlayerControl.LocalPlayer == Prophet.prophet && !PlayerControl.LocalPlayer.Data.IsDead && Prophet.examinesLeft > 0; },
               () =>
               {
                   return Prophet.currentTarget != null && PlayerControl.LocalPlayer.CanMove;
               },
               () => { prophetButton.Timer = prophetButton.MaxTimer; },
               Prophet.getButtonSprite(),
               CustomButton.ButtonPositions.lowerRowRight,
               __instance,
               KeyCode.F,
               buttonText: "ProphetText"
        );

        placeJackInTheBoxButton = new CustomButton(
            () =>
            {
                placeJackInTheBoxButton.Timer = placeJackInTheBoxButton.MaxTimer;

                var pos = PlayerControl.LocalPlayer.transform.position;
                var buff = new byte[sizeof(float) * 2];
                Buffer.BlockCopy(BitConverter.GetBytes(pos.x), 0, buff, 0 * sizeof(float), sizeof(float));
                Buffer.BlockCopy(BitConverter.GetBytes(pos.y), 0, buff, 1 * sizeof(float), sizeof(float));

                var writer = AmongUsClient.Instance.StartRpc(PlayerControl.LocalPlayer.NetId,
                    (byte)CustomRPC.PlaceJackInTheBox);
                writer.WriteBytesAndSize(buff);
                writer.EndMessage();
                RPCProcedure.placeJackInTheBox(buff);
                SoundEffectsManager.play("tricksterPlaceBox");
            },
            () =>
            {
                return Trickster.trickster != null && Trickster.trickster == PlayerControl.LocalPlayer &&
                       !PlayerControl.LocalPlayer.Data.IsDead && !JackInTheBox.hasJackInTheBoxLimitReached();
            },
            () => { return PlayerControl.LocalPlayer.CanMove && !JackInTheBox.hasJackInTheBoxLimitReached(); },
            () => { placeJackInTheBoxButton.Timer = placeJackInTheBoxButton.MaxTimer; },
            Trickster.getPlaceBoxButtonSprite(),
            CustomButton.ButtonPositions.upperRowLeft,
            __instance,
            KeyCode.F,
            buttonText: "tricksterPlaceBox"
        );

        lightsOutButton = new CustomButton(
            () =>
            {
                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                    (byte)CustomRPC.LightsOut, SendOption.Reliable);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.lightsOut();
                SoundEffectsManager.play("lighterLight");
            },
            () =>
            {
                return Trickster.trickster != null && Trickster.trickster == PlayerControl.LocalPlayer &&
                       !PlayerControl.LocalPlayer.Data.IsDead
                       && JackInTheBox.hasJackInTheBoxLimitReached() && JackInTheBox.boxesConvertedToVents;
            },
            () =>
            {
                return PlayerControl.LocalPlayer.CanMove && JackInTheBox.hasJackInTheBoxLimitReached() &&
                       JackInTheBox.boxesConvertedToVents;
            },
            () =>
            {
                lightsOutButton.Timer = lightsOutButton.MaxTimer;
                lightsOutButton.isEffectActive = false;
                lightsOutButton.actionButton.graphic.color = Palette.EnabledColor;
            },
            Trickster.getLightsOutButtonSprite(),
            CustomButton.ButtonPositions.upperRowLeft,
            __instance,
            KeyCode.F,
            true,
            Trickster.lightsOutDuration,
            () =>
            {
                lightsOutButton.Timer = lightsOutButton.MaxTimer;
                SoundEffectsManager.play("lighterLight");
            },
            buttonText: "lightOutButton"
        );

        // Cleaner Clean
        cleanerCleanButton = new CustomButton(
            () =>
            {
                foreach (var collider2D in Physics2D.OverlapCircleAll(PlayerControl.LocalPlayer.GetTruePosition(),
                             PlayerControl.LocalPlayer.MaxReportDistance, Constants.PlayersOnlyMask))
                    if (collider2D.tag == "DeadBody")
                    {
                        var component = collider2D.GetComponent<DeadBody>();
                        if (component && !component.Reported)
                        {
                            var truePosition = PlayerControl.LocalPlayer.GetTruePosition();
                            var truePosition2 = component.TruePosition;
                            if (Vector2.Distance(truePosition2, truePosition) <=
                                PlayerControl.LocalPlayer.MaxReportDistance && PlayerControl.LocalPlayer.CanMove &&
                                !PhysicsHelpers.AnythingBetween(truePosition, truePosition2,
                                    Constants.ShipAndObjectsMask, false))
                            {
                                var playerInfo = GameData.Instance.GetPlayerById(component.ParentId);

                                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                                    (byte)CustomRPC.CleanBody, SendOption.Reliable);
                                writer.Write(playerInfo.PlayerId);
                                writer.Write(Cleaner.cleaner.PlayerId);
                                AmongUsClient.Instance.FinishRpcImmediately(writer);
                                RPCProcedure.cleanBody(playerInfo.PlayerId, Cleaner.cleaner.PlayerId);

                                Cleaner.cleaner.killTimer = cleanerCleanButton.Timer = cleanerCleanButton.MaxTimer;
                                SoundEffectsManager.play("cleanerClean");
                                break;
                            }
                        }
                    }
            },
            () =>
            {
                return Cleaner.cleaner != null && Cleaner.cleaner == PlayerControl.LocalPlayer &&
                       !PlayerControl.LocalPlayer.Data.IsDead;
            },
            () =>
            {
                return __instance.ReportButton.graphic.color == Palette.EnabledColor &&
                       PlayerControl.LocalPlayer.CanMove;
            },
            () => { cleanerCleanButton.Timer = cleanerCleanButton.MaxTimer; },
            Cleaner.getButtonSprite(),
            CustomButton.ButtonPositions.upperRowLeft,
            __instance,
            KeyCode.F,
            buttonText: "cleanerClean"
        );

        // Warlock curse
        warlockCurseButton = new CustomButton(
            () =>
            {
                if (Warlock.curseVictim == null)
                {
                    // Apply Curse
                    Warlock.curseVictim = Warlock.currentTarget;
                    warlockCurseButton.Sprite = Warlock.getCurseKillButtonSprite();
                    warlockCurseButton.Timer = 1f;
                    SoundEffectsManager.play("warlockCurse");
                    warlockCurseButton.buttonText =
                        FastDestroyableSingleton<TranslationController>.Instance.GetString(StringNames.KillLabel);

                    // Ghost Info
                    var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                        (byte)CustomRPC.ShareGhostInfo, SendOption.Reliable);
                    writer.Write(PlayerControl.LocalPlayer.PlayerId);
                    writer.Write((byte)RPCProcedure.GhostInfoTypes.WarlockTarget);
                    writer.Write(Warlock.curseVictim.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                }
                else if (Warlock.curseVictim != null && Warlock.curseVictimTarget != null)
                {
                    var murder = Helpers.checkMurderAttemptAndKill(Warlock.warlock, Warlock.curseVictimTarget,
                        showAnimation: false);
                    if (murder == MurderAttemptResult.SuppressKill) return;

                    // If blanked or killed
                    if (Warlock.rootTime > 0)
                    {
                        AntiTeleport.position = PlayerControl.LocalPlayer.transform.position;
                        PlayerControl.LocalPlayer.moveable = false;
                        PlayerControl.LocalPlayer.NetTransform
                            .Halt(); // Stop current movement so the warlock is not just running straight into the next object
                        FastDestroyableSingleton<HudManager>.Instance.StartCoroutine(Effects.Lerp(Warlock.rootTime,
                            new Action<float>(p =>
                            {
                                // Delayed action
                                if (p == 1f) PlayerControl.LocalPlayer.moveable = true;
                            })));
                    }

                    Warlock.curseVictim = null;
                    Warlock.curseVictimTarget = null;
                    warlockCurseButton.Sprite = Warlock.getCurseButtonSprite();
                    Warlock.warlock.killTimer = warlockCurseButton.Timer = warlockCurseButton.MaxTimer;
                    warlockCurseButton.buttonText = "warlockCurse".Translate();

                    var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                        (byte)CustomRPC.ShareGhostInfo, SendOption.Reliable);
                    writer.Write(PlayerControl.LocalPlayer.PlayerId);
                    writer.Write((byte)RPCProcedure.GhostInfoTypes.WarlockTarget);
                    writer.Write(byte.MaxValue); // This will set it to null!
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                }
            },
            () =>
            {
                return Warlock.warlock != null && Warlock.warlock == PlayerControl.LocalPlayer &&
                       !PlayerControl.LocalPlayer.Data.IsDead;
            },
            () =>
            {
                return ((Warlock.curseVictim == null && Warlock.currentTarget != null) ||
                        (Warlock.curseVictim != null && Warlock.curseVictimTarget != null)) &&
                       PlayerControl.LocalPlayer.CanMove;
            },
            () =>
            {
                warlockCurseButton.Timer = warlockCurseButton.MaxTimer;
                warlockCurseButton.Sprite = Warlock.getCurseButtonSprite();
                Warlock.curseVictim = null;
                Warlock.curseVictimTarget = null;
            },
            Warlock.getCurseButtonSprite(),
            CustomButton.ButtonPositions.upperRowLeft,
            __instance,
            KeyCode.F,
            buttonText: "warlockCurse"
        );

        // Security Guard button
        securityGuardButton = new CustomButton(
            () =>
            {
                if (SecurityGuard.ventTarget != null)
                {
                    // Seal vent
                    var writer =
                        AmongUsClient.Instance.StartRpc(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.SealVent);
                    writer.WritePacked(SecurityGuard.ventTarget.Id);
                    writer.EndMessage();
                    RPCProcedure.sealVent(SecurityGuard.ventTarget.Id);
                    SecurityGuard.ventTarget = null;
                }
                else if (!Helpers.isMira() && !Helpers.isFungle() && !SubmergedCompatibility.IsSubmerged)
                {
                    // Place camera if there's no vent and it's not MiraHQ or Submerged
                    var pos = PlayerControl.LocalPlayer.transform.position;
                    var buff = new byte[sizeof(float) * 2];
                    Buffer.BlockCopy(BitConverter.GetBytes(pos.x), 0, buff, 0 * sizeof(float), sizeof(float));
                    Buffer.BlockCopy(BitConverter.GetBytes(pos.y), 0, buff, 1 * sizeof(float), sizeof(float));

                    var writer =
                        AmongUsClient.Instance.StartRpc(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.PlaceCamera);
                    writer.WriteBytesAndSize(buff);
                    writer.EndMessage();
                    RPCProcedure.placeCamera(buff);
                }

                SoundEffectsManager.play("securityGuardPlaceCam"); // Same sound used for both types (cam or vent)!
                securityGuardButton.Timer = securityGuardButton.MaxTimer;
            },
            () =>
            {
                return SecurityGuard.securityGuard != null &&
                       SecurityGuard.securityGuard == PlayerControl.LocalPlayer &&
                       !PlayerControl.LocalPlayer.Data.IsDead && SecurityGuard.remainingScrews >=
                       Mathf.Min(SecurityGuard.ventPrice, SecurityGuard.camPrice);
            },
            () =>
            {
                securityGuardButton.actionButton.graphic.sprite =
                    SecurityGuard.ventTarget == null && !Helpers.isMira() && !Helpers.isFungle() &&
                    !SubmergedCompatibility.IsSubmerged
                        ? SecurityGuard.getPlaceCameraButtonSprite()
                        : SecurityGuard.getCloseVentButtonSprite();
                if (SecurityGuard.ventTarget == null && !Helpers.isMira() && !Helpers.isFungle() &&
                    GameOptionsManager.Instance.currentNormalGameOptions.MapId != 1 &&
                    !SubmergedCompatibility.IsSubmerged)
                    securityGuardButton.buttonText = "securityGuardPlaceCam".Translate();
                else
                    securityGuardButton.buttonText = "securityGuardSealVent".Translate();
                if (securityGuardButtonScrewsText != null)
                    securityGuardButtonScrewsText.text = $"{SecurityGuard.remainingScrews}/{SecurityGuard.totalScrews}";

                if (SecurityGuard.ventTarget != null)
                    return SecurityGuard.remainingScrews >= SecurityGuard.ventPrice &&
                           PlayerControl.LocalPlayer.CanMove;
                return !Helpers.isMira() && !Helpers.isFungle() && !SubmergedCompatibility.IsSubmerged &&
                       SecurityGuard.remainingScrews >= SecurityGuard.camPrice && PlayerControl.LocalPlayer.CanMove;
            },
            () => { securityGuardButton.Timer = securityGuardButton.MaxTimer; },
            SecurityGuard.getPlaceCameraButtonSprite(),
            CustomButton.ButtonPositions.lowerRowRight,
            __instance,
            KeyCode.F,
            buttonText: "securityGuardSealVent"
        );

        // Security Guard button screws counter
        securityGuardButtonScrewsText = GameObject.Instantiate(securityGuardButton.actionButton.cooldownTimerText,
            securityGuardButton.actionButton.cooldownTimerText.transform.parent);
        securityGuardButtonScrewsText.text = "";
        securityGuardButtonScrewsText.enableWordWrapping = false;
        securityGuardButtonScrewsText.transform.localScale = Vector3.one * 0.5f;
        securityGuardButtonScrewsText.transform.localPosition += new Vector3(-0.05f, 0.7f, 0);

        securityGuardCamButton = new CustomButton(
            () =>
            {
                if (!Helpers.isMira())
                {
                    if (SecurityGuard.minigame == null)
                    {
                        var mapId = GameOptionsManager.Instance.currentNormalGameOptions.MapId;
                        var e = Object.FindObjectsOfType<SystemConsole>().FirstOrDefault(x =>
                            x.gameObject.name.Contains("Surv_Panel") || x.name.Contains("Cam") ||
                            x.name.Contains("BinocularsSecurityConsole"));
                        if (Helpers.isSkeld() || mapId == 3)
                            e = Object.FindObjectsOfType<SystemConsole>()
                                .FirstOrDefault(x => x.gameObject.name.Contains("SurvConsole"));
                        else if (Helpers.isAirship())
                            e = Object.FindObjectsOfType<SystemConsole>()
                                .FirstOrDefault(x => x.gameObject.name.Contains("task_cams"));
                        if (e == null || Camera.main == null) return;
                        SecurityGuard.minigame = Object.Instantiate(e.MinigamePrefab, Camera.main.transform, false);
                    }

                    SecurityGuard.minigame.transform.SetParent(Camera.main.transform, false);
                    SecurityGuard.minigame.transform.localPosition = new Vector3(0.0f, 0.0f, -50f);
                    SecurityGuard.minigame.Begin(null);
                }
                else
                {
                    if (SecurityGuard.minigame == null)
                    {
                        var e = Object.FindObjectsOfType<SystemConsole>()
                            .FirstOrDefault(x => x.gameObject.name.Contains("SurvLogConsole"));
                        if (e == null || Camera.main == null) return;
                        SecurityGuard.minigame = Object.Instantiate(e.MinigamePrefab, Camera.main.transform, false);
                    }

                    SecurityGuard.minigame.transform.SetParent(Camera.main.transform, false);
                    SecurityGuard.minigame.transform.localPosition = new Vector3(0.0f, 0.0f, -50f);
                    SecurityGuard.minigame.Begin(null);
                }

                SecurityGuard.charges--;

                if (SecurityGuard.cantMove) PlayerControl.LocalPlayer.moveable = false;
                PlayerControl.LocalPlayer.NetTransform.Halt(); // Stop current movement 
            },
            () =>
            {
                return SecurityGuard.securityGuard != null &&
                       SecurityGuard.securityGuard == PlayerControl.LocalPlayer &&
                       !PlayerControl.LocalPlayer.Data.IsDead && SecurityGuard.remainingScrews <
                       Mathf.Min(SecurityGuard.ventPrice, SecurityGuard.camPrice)
                       && !SubmergedCompatibility.IsSubmerged;
            },
            () =>
            {
                if (securityGuardChargesText != null)
                    securityGuardChargesText.text = $"{SecurityGuard.charges} / {SecurityGuard.maxCharges}";
                securityGuardCamButton.actionButton.graphic.sprite =
                    Helpers.isMira() ? SecurityGuard.getLogSprite() : SecurityGuard.getCamSprite();
                securityGuardCamButton.actionButton.OverrideText(Helpers.isMira()
                    ? FastDestroyableSingleton<TranslationController>.Instance.GetString(StringNames.DoorlogLabel)
                    : FastDestroyableSingleton<TranslationController>.Instance.GetString(StringNames.Security));
                return PlayerControl.LocalPlayer.CanMove && SecurityGuard.charges > 0;
            },
            () =>
            {
                securityGuardCamButton.Timer = securityGuardCamButton.MaxTimer;
                securityGuardCamButton.isEffectActive = false;
                securityGuardCamButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
            },
            SecurityGuard.getCamSprite(),
            CustomButton.ButtonPositions.lowerRowRight,
            __instance,
            KeyCode.G,
            true,
            0f,
            () =>
            {
                securityGuardCamButton.Timer = securityGuardCamButton.MaxTimer;
                if (Minigame.Instance) SecurityGuard.minigame.ForceClose();
                PlayerControl.LocalPlayer.moveable = true;
            },
            false,
            Helpers.isMira()
                ? FastDestroyableSingleton<TranslationController>.Instance.GetString(StringNames.DoorlogLabel)
                : FastDestroyableSingleton<TranslationController>.Instance.GetString(StringNames.Security)
        );

        // Security Guard cam button charges
        securityGuardChargesText = GameObject.Instantiate(securityGuardCamButton.actionButton.cooldownTimerText,
            securityGuardCamButton.actionButton.cooldownTimerText.transform.parent);
        securityGuardChargesText.text = "";
        securityGuardChargesText.enableWordWrapping = false;
        securityGuardChargesText.transform.localScale = Vector3.one * 0.5f;
        securityGuardChargesText.transform.localPosition += new Vector3(-0.05f, 0.7f, 0);

        // Arsonist button
        arsonistButton = new CustomButton(
            () =>
            {
                var dousedEveryoneAlive = Arsonist.dousedEveryoneAlive();
                if (dousedEveryoneAlive)
                {
                    var winWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                        (byte)CustomRPC.ArsonistWin, SendOption.Reliable);
                    AmongUsClient.Instance.FinishRpcImmediately(winWriter);
                    RPCProcedure.arsonistWin();
                    arsonistButton.HasEffect = false;
                }
                else if (Arsonist.currentTarget != null)
                {
                    Arsonist.douseTarget = Arsonist.currentTarget;
                    arsonistButton.HasEffect = true;
                    SoundEffectsManager.play("arsonistDouse");
                }
            },
            () =>
            {
                return Arsonist.arsonist != null && Arsonist.arsonist == PlayerControl.LocalPlayer &&
                       !PlayerControl.LocalPlayer.Data.IsDead;
            },
            () =>
            {
                var dousedEveryoneAlive = Arsonist.dousedEveryoneAlive();
                if (dousedEveryoneAlive)
                {
                    arsonistButton.actionButton.graphic.sprite = Arsonist.getIgniteSprite();
                    arsonistButton.buttonText = "arsonistIgnite".Translate();
                }

                if (arsonistButton.isEffectActive && Arsonist.douseTarget != Arsonist.currentTarget)
                {
                    Arsonist.douseTarget = null;
                    arsonistButton.Timer = 0f;
                    arsonistButton.isEffectActive = false;
                }

                return PlayerControl.LocalPlayer.CanMove && (dousedEveryoneAlive || Arsonist.currentTarget != null);
            },
            () =>
            {
                arsonistButton.Timer = arsonistButton.MaxTimer;
                arsonistButton.isEffectActive = false;
                Arsonist.douseTarget = null;
            },
            Arsonist.getDouseSprite(),
            CustomButton.ButtonPositions.lowerRowRight,
            __instance,
            KeyCode.F,
            true,
            Arsonist.duration,
            () =>
            {
                if (Arsonist.douseTarget != null) Arsonist.dousedPlayers.Add(Arsonist.douseTarget);

                arsonistButton.Timer = Arsonist.dousedEveryoneAlive() ? 0 : arsonistButton.MaxTimer;

                foreach (var p in Arsonist.dousedPlayers)
                    if (TORMapOptions.playerIcons.ContainsKey(p.PlayerId))
                        TORMapOptions.playerIcons[p.PlayerId].setSemiTransparent(false);

                // Ghost Info
                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                    (byte)CustomRPC.ShareGhostInfo, SendOption.Reliable);
                writer.Write(PlayerControl.LocalPlayer.PlayerId);
                writer.Write((byte)RPCProcedure.GhostInfoTypes.ArsonistDouse);
                writer.Write(Arsonist.douseTarget.PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);

                Arsonist.douseTarget = null;
            },
            buttonText: "arsonistDouse"
        );

        // Vulture Eat
        vultureEatButton = new CustomButton(
            () =>
            {
                foreach (var collider2D in Physics2D.OverlapCircleAll(PlayerControl.LocalPlayer.GetTruePosition(),
                             PlayerControl.LocalPlayer.MaxReportDistance, Constants.PlayersOnlyMask))
                    if (collider2D.tag == "DeadBody")
                    {
                        var component = collider2D.GetComponent<DeadBody>();
                        if (component && !component.Reported)
                        {
                            var truePosition = PlayerControl.LocalPlayer.GetTruePosition();
                            var truePosition2 = component.TruePosition;
                            if (Vector2.Distance(truePosition2, truePosition) <=
                                PlayerControl.LocalPlayer.MaxReportDistance && PlayerControl.LocalPlayer.CanMove &&
                                !PhysicsHelpers.AnythingBetween(truePosition, truePosition2,
                                    Constants.ShipAndObjectsMask, false))
                            {
                                var playerInfo = GameData.Instance.GetPlayerById(component.ParentId);

                                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                                    (byte)CustomRPC.CleanBody, SendOption.Reliable);
                                writer.Write(playerInfo.PlayerId);
                                writer.Write(Vulture.vulture.PlayerId);
                                AmongUsClient.Instance.FinishRpcImmediately(writer);
                                RPCProcedure.cleanBody(playerInfo.PlayerId, Vulture.vulture.PlayerId);

                                Vulture.cooldown = vultureEatButton.Timer = vultureEatButton.MaxTimer;
                                SoundEffectsManager.play("vultureEat");
                                break;
                            }
                        }
                    }
            },
            () =>
            {
                return Vulture.vulture != null && Vulture.vulture == PlayerControl.LocalPlayer &&
                       !PlayerControl.LocalPlayer.Data.IsDead;
            },
            () =>
            {
                return __instance.ReportButton.graphic.color == Palette.EnabledColor &&
                       PlayerControl.LocalPlayer.CanMove;
            },
            () => { vultureEatButton.Timer = vultureEatButton.MaxTimer; },
            Vulture.getButtonSprite(),
            CustomButton.ButtonPositions.lowerRowCenter,
            __instance,
            KeyCode.F,
            buttonText: "vultureEat"
        );

        // Medium button
        mediumButton = new CustomButton(
            () =>
            {
                if (Medium.target != null)
                {
                    Medium.soulTarget = Medium.target;
                    mediumButton.HasEffect = true;
                    SoundEffectsManager.play("mediumAsk");
                }
            },
            () =>
            {
                return Medium.medium != null && Medium.medium == PlayerControl.LocalPlayer &&
                       !PlayerControl.LocalPlayer.Data.IsDead;
            },
            () =>
            {
                if (mediumButton.isEffectActive && Medium.target != Medium.soulTarget)
                {
                    Medium.soulTarget = null;
                    mediumButton.Timer = 0f;
                    mediumButton.isEffectActive = false;
                }

                return Medium.target != null && PlayerControl.LocalPlayer.CanMove;
            },
            () =>
            {
                mediumButton.Timer = mediumButton.MaxTimer;
                mediumButton.isEffectActive = false;
                Medium.soulTarget = null;
            },
            Medium.getQuestionSprite(),
            CustomButton.ButtonPositions.lowerRowRight,
            __instance,
            KeyCode.F,
            true,
            Medium.duration,
            () =>
            {
                mediumButton.Timer = mediumButton.MaxTimer;
                if (Medium.target == null || Medium.target.player == null) return;
                var msg = Medium.getInfo(Medium.target.player, Medium.target.killerIfExisting,
                    Medium.target.deathReason);
                FastDestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, msg);

                // Ghost Info
                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                    (byte)CustomRPC.ShareGhostInfo, SendOption.Reliable);
                writer.Write(Medium.target.player.PlayerId);
                writer.Write((byte)RPCProcedure.GhostInfoTypes.MediumInfo);
                writer.Write(msg);
                AmongUsClient.Instance.FinishRpcImmediately(writer);

                // Remove soul
                if (Medium.oneTimeUse)
                {
                    var closestDistance = float.MaxValue;
                    SpriteRenderer target = null;

                    foreach (var (db, ps) in Medium.deadBodies)
                        if (db == Medium.target)
                        {
                            var deadBody = Tuple.Create(db, ps);
                            Medium.deadBodies.Remove(deadBody);
                            break;
                        }

                    foreach (var rend in Medium.souls)
                    {
                        var distance = Vector2.Distance(rend.transform.position,
                            PlayerControl.LocalPlayer.GetTruePosition());
                        if (distance < closestDistance)
                        {
                            closestDistance = distance;
                            target = rend;
                        }
                    }

                    FastDestroyableSingleton<HudManager>.Instance.StartCoroutine(Effects.Lerp(5f, new Action<float>(p =>
                    {
                        if (target != null)
                        {
                            var tmp = target.color;
                            tmp.a = Mathf.Clamp01(1 - p);
                            target.color = tmp;
                        }

                        if (p == 1f && target != null && target.gameObject != null) Object.Destroy(target.gameObject);
                    })));

                    Medium.souls.Remove(target);
                }

                SoundEffectsManager.stop("mediumAsk");
            },
            buttonText: "mediumAsk"
        );

        // Pursuer button
        pursuerButton = new CustomButton(
            () =>
            {
                if (Pursuer.target != null)
                {
                    var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                        (byte)CustomRPC.SetBlanked, SendOption.Reliable);
                    writer.Write(Pursuer.target.PlayerId);
                    writer.Write(byte.MaxValue);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    RPCProcedure.setBlanked(Pursuer.target.PlayerId, byte.MaxValue);

                    Pursuer.target = null;

                    Pursuer.blanks++;
                    pursuerButton.Timer = pursuerButton.MaxTimer;
                    SoundEffectsManager.play("pursuerBlank");
                }
            },
            () =>
            {
                return Pursuer.pursuer != null && Pursuer.pursuer == PlayerControl.LocalPlayer &&
                       !PlayerControl.LocalPlayer.Data.IsDead && Pursuer.blanks < Pursuer.blanksNumber;
            },
            () =>
            {
                if (pursuerButtonBlanksText != null)
                    pursuerButtonBlanksText.text = $"{Pursuer.blanksNumber - Pursuer.blanks}";

                return Pursuer.blanksNumber > Pursuer.blanks && PlayerControl.LocalPlayer.CanMove &&
                       Pursuer.target != null;
            },
            () => { pursuerButton.Timer = pursuerButton.MaxTimer; },
            Pursuer.getTargetSprite(),
            CustomButton.ButtonPositions.lowerRowRight,
            __instance,
            KeyCode.F,
            buttonText: "pursuerBlank"
        );

        // Pursuer button blanks left
        pursuerButtonBlanksText = GameObject.Instantiate(pursuerButton.actionButton.cooldownTimerText,
            pursuerButton.actionButton.cooldownTimerText.transform.parent);
        pursuerButtonBlanksText.text = "";
        pursuerButtonBlanksText.enableWordWrapping = false;
        pursuerButtonBlanksText.transform.localScale = Vector3.one * 0.5f;
        pursuerButtonBlanksText.transform.localPosition += new Vector3(-0.05f, 0.7f, 0);


        // Witch Spell button
        witchSpellButton = new CustomButton(
            () =>
            {
                if (Witch.currentTarget != null)
                {
                    Witch.spellCastingTarget = Witch.currentTarget;
                    SoundEffectsManager.play("witchSpell");
                }
            },
            () =>
            {
                return Witch.witch != null && Witch.witch == PlayerControl.LocalPlayer &&
                       !PlayerControl.LocalPlayer.Data.IsDead;
            },
            () =>
            {
                if (witchSpellButton.isEffectActive && Witch.spellCastingTarget != Witch.currentTarget)
                {
                    Witch.spellCastingTarget = null;
                    witchSpellButton.Timer = 0f;
                    witchSpellButton.isEffectActive = false;
                }

                return PlayerControl.LocalPlayer.CanMove && Witch.currentTarget != null;
            },
            () =>
            {
                witchSpellButton.Timer = witchSpellButton.MaxTimer;
                witchSpellButton.isEffectActive = false;
                Witch.spellCastingTarget = null;
            },
            Witch.getButtonSprite(),
            CustomButton.ButtonPositions.upperRowLeft,
            __instance,
            KeyCode.F,
            true,
            Witch.spellCastingDuration,
            () =>
            {
                if (Witch.spellCastingTarget == null) return;
                var attempt = Helpers.checkMuderAttempt(Witch.witch, Witch.spellCastingTarget);
                if (attempt == MurderAttemptResult.PerformKill)
                {
                    var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                        (byte)CustomRPC.SetFutureSpelled, SendOption.Reliable);
                    writer.Write(Witch.currentTarget.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    RPCProcedure.setFutureSpelled(Witch.currentTarget.PlayerId);
                }

                if (attempt == MurderAttemptResult.BlankKill || attempt == MurderAttemptResult.PerformKill)
                {
                    Witch.currentCooldownAddition += Witch.cooldownAddition;
                    witchSpellButton.MaxTimer = Witch.cooldown + Witch.currentCooldownAddition;
                    PlayerControlFixedUpdatePatch
                        .miniCooldownUpdate(); // Modifies the MaxTimer if the witch is the mini
                    witchSpellButton.Timer = witchSpellButton.MaxTimer;
                    if (Witch.triggerBothCooldowns)
                    {
                        var multiplier = Mini.mini != null && PlayerControl.LocalPlayer == Mini.mini
                            ? Mini.isGrownUp() ? 0.66f : 2f
                            : 1f;
                        Witch.witch.killTimer = GameOptionsManager.Instance.currentNormalGameOptions.KillCooldown *
                                                multiplier;
                    }
                }
                else
                {
                    witchSpellButton.Timer = 0f;
                }

                Witch.spellCastingTarget = null;
            },
            buttonText: "witchSpell"
        );

        // Ninja mark and assassinate button 
        ninjaButton = new CustomButton(
            () =>
            {
                MessageWriter writer;
                if (Ninja.ninjaMarked != null)
                {
                    ninjaButton.buttonText = "ninjaMarkButton".Translate();
                    // Murder attempt with teleport
                    var attempt = Helpers.checkMuderAttempt(Ninja.ninja, Ninja.ninjaMarked);
                    if (attempt == MurderAttemptResult.PerformKill)
                    {
                        // Create first trace before killing
                        var pos = PlayerControl.LocalPlayer.transform.position;
                        var buff = new byte[sizeof(float) * 2];
                        Buffer.BlockCopy(BitConverter.GetBytes(pos.x), 0, buff, 0 * sizeof(float), sizeof(float));
                        Buffer.BlockCopy(BitConverter.GetBytes(pos.y), 0, buff, 1 * sizeof(float), sizeof(float));

                        writer = AmongUsClient.Instance.StartRpc(PlayerControl.LocalPlayer.NetId,
                            (byte)CustomRPC.PlaceNinjaTrace);
                        writer.WriteBytesAndSize(buff);
                        writer.EndMessage();
                        RPCProcedure.placeNinjaTrace(buff);

                        var invisibleWriter = AmongUsClient.Instance.StartRpcImmediately(
                            PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.SetInvisible, SendOption.Reliable);
                        invisibleWriter.Write(Ninja.ninja.PlayerId);
                        invisibleWriter.Write(byte.MinValue);
                        AmongUsClient.Instance.FinishRpcImmediately(invisibleWriter);
                        RPCProcedure.setInvisible(Ninja.ninja.PlayerId, byte.MinValue);

                        // Perform Kill
                        var writer2 = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                            (byte)CustomRPC.UncheckedMurderPlayer, SendOption.Reliable);
                        writer2.Write(PlayerControl.LocalPlayer.PlayerId);
                        writer2.Write(Ninja.ninjaMarked.PlayerId);
                        writer2.Write(byte.MaxValue);
                        AmongUsClient.Instance.FinishRpcImmediately(writer2);
                        if (SubmergedCompatibility.IsSubmerged)
                            SubmergedCompatibility.ChangeFloor(Ninja.ninjaMarked.transform.localPosition.y > -7);
                        RPCProcedure.uncheckedMurderPlayer(PlayerControl.LocalPlayer.PlayerId,
                            Ninja.ninjaMarked.PlayerId, byte.MaxValue);

                        // Create Second trace after killing
                        pos = Ninja.ninjaMarked.transform.position;
                        buff = new byte[sizeof(float) * 2];
                        Buffer.BlockCopy(BitConverter.GetBytes(pos.x), 0, buff, 0 * sizeof(float), sizeof(float));
                        Buffer.BlockCopy(BitConverter.GetBytes(pos.y), 0, buff, 1 * sizeof(float), sizeof(float));

                        var writer3 = AmongUsClient.Instance.StartRpc(PlayerControl.LocalPlayer.NetId,
                            (byte)CustomRPC.PlaceNinjaTrace);
                        writer3.WriteBytesAndSize(buff);
                        writer3.EndMessage();
                        RPCProcedure.placeNinjaTrace(buff);
                    }

                    if (attempt == MurderAttemptResult.BlankKill || attempt == MurderAttemptResult.PerformKill)
                    {
                        ninjaButton.Timer = ninjaButton.MaxTimer;
                        Ninja.ninja.killTimer = GameOptionsManager.Instance.currentNormalGameOptions.KillCooldown;
                    }
                    else if (attempt == MurderAttemptResult.SuppressKill)
                    {
                        ninjaButton.Timer = 0f;
                    }

                    Ninja.ninjaMarked = null;
                    return;
                }

                if (Ninja.currentTarget != null)
                {
                    ninjaButton.buttonText = "ninjaAssassinateButton".Translate();
                    Ninja.ninjaMarked = Ninja.currentTarget;
                    ninjaButton.Timer = 5f;
                    SoundEffectsManager.play("warlockCurse");

                    // Ghost Info
                    writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                        (byte)CustomRPC.ShareGhostInfo, SendOption.Reliable);
                    writer.Write(PlayerControl.LocalPlayer.PlayerId);
                    writer.Write((byte)RPCProcedure.GhostInfoTypes.NinjaMarked);
                    writer.Write(Ninja.ninjaMarked.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                }
            },
            () =>
            {
                return Ninja.ninja != null && Ninja.ninja == PlayerControl.LocalPlayer &&
                       !PlayerControl.LocalPlayer.Data.IsDead;
            },
            () =>
            {
                // CouldUse
                ninjaButton.Sprite = Ninja.ninjaMarked != null
                    ? Ninja.getKillButtonSprite()
                    : Ninja.getMarkButtonSprite();
                return (Ninja.currentTarget != null || (Ninja.ninjaMarked != null &&
                                                        !TransportationToolPatches.isUsingTransportation(
                                                            Ninja.ninjaMarked))) && PlayerControl.LocalPlayer.CanMove;
            },
            () =>
            {
                // on meeting ends
                ninjaButton.Timer = ninjaButton.MaxTimer;
                Ninja.ninjaMarked = null;
            },
            Ninja.getMarkButtonSprite(),
            CustomButton.ButtonPositions.upperRowLeft,
            __instance,
            KeyCode.F,
            buttonText: "ninjaMarkButton"
        );

        mayorMeetingButton = new CustomButton(
            () =>
            {
                PlayerControl.LocalPlayer.NetTransform.Halt(); // Stop current movement 
                Mayor.remoteMeetingsLeft--;
                Helpers
                    .handleVampireBiteOnBodyReport(); // Manually call Vampire handling, since the CmdReportDeadBody Prefix won't be called
                RPCProcedure.uncheckedCmdReportDeadBody(PlayerControl.LocalPlayer.PlayerId, byte.MaxValue);

                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                    (byte)CustomRPC.UncheckedCmdReportDeadBody, SendOption.Reliable);
                writer.Write(PlayerControl.LocalPlayer.PlayerId);
                writer.Write(byte.MaxValue);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                mayorMeetingButton.Timer = 1f;
            },
            () =>
            {
                return Mayor.mayor != null && Mayor.mayor == PlayerControl.LocalPlayer &&
                       !PlayerControl.LocalPlayer.Data.IsDead && Mayor.meetingButton;
            },
            () =>
            {
                mayorMeetingButton.actionButton.OverrideText("mayorMeetingButton".Translate() + " (" +
                                                             Mayor.remoteMeetingsLeft + ")");
                var sabotageActive = false;
                foreach (var task in PlayerControl.LocalPlayer.myTasks.GetFastEnumerator())
                    if (task.TaskType == TaskTypes.FixLights || task.TaskType == TaskTypes.RestoreOxy ||
                        task.TaskType == TaskTypes.ResetReactor || task.TaskType == TaskTypes.ResetSeismic ||
                        task.TaskType == TaskTypes.FixComms || task.TaskType == TaskTypes.StopCharles
                        || (SubmergedCompatibility.IsSubmerged &&
                            task.TaskType == SubmergedCompatibility.RetrieveOxygenMask))
                        sabotageActive = true;
                return !sabotageActive && PlayerControl.LocalPlayer.CanMove && Mayor.remoteMeetingsLeft > 0;
            },
            () => { mayorMeetingButton.Timer = mayorMeetingButton.MaxTimer; },
            Mayor.getMeetingSprite(),
            CustomButton.ButtonPositions.lowerRowRight,
            __instance,
            KeyCode.F,
            true,
            0f,
            () => { },
            false,
            "mayorMeetingButton"
        );

        // Trapper button
        trapperButton = new CustomButton(
            () =>
            {
                var pos = PlayerControl.LocalPlayer.transform.position;
                var buff = new byte[sizeof(float) * 2];
                Buffer.BlockCopy(BitConverter.GetBytes(pos.x), 0, buff, 0 * sizeof(float), sizeof(float));
                Buffer.BlockCopy(BitConverter.GetBytes(pos.y), 0, buff, 1 * sizeof(float), sizeof(float));

                var writer = AmongUsClient.Instance.StartRpc(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.SetTrap);
                writer.WriteBytesAndSize(buff);
                writer.EndMessage();
                RPCProcedure.setTrap(buff);

                SoundEffectsManager.play("trapperTrap");
                trapperButton.Timer = trapperButton.MaxTimer;
            },
            () =>
            {
                return Trapper.trapper != null && Trapper.trapper == PlayerControl.LocalPlayer &&
                       !PlayerControl.LocalPlayer.Data.IsDead;
            },
            () =>
            {
                if (trapperChargesText != null) trapperChargesText.text = $"{Trapper.charges} / {Trapper.maxCharges}";
                return PlayerControl.LocalPlayer.CanMove && Trapper.charges > 0;
            },
            () => { trapperButton.Timer = trapperButton.MaxTimer; },
            Trapper.getButtonSprite(),
            CustomButton.ButtonPositions.lowerRowRight,
            __instance,
            KeyCode.F,
            buttonText: "trapperTrap"
        );

        // Bomber button
        bomberButton = new CustomButton(
            () =>
            {
                if (Helpers.checkMuderAttempt(Bomber.bomber, Bomber.bomber, ignoreMedic: true) !=
                    MurderAttemptResult.BlankKill)
                {
                    var pos = PlayerControl.LocalPlayer.transform.position;
                    var buff = new byte[sizeof(float) * 2];
                    Buffer.BlockCopy(BitConverter.GetBytes(pos.x), 0, buff, 0 * sizeof(float), sizeof(float));
                    Buffer.BlockCopy(BitConverter.GetBytes(pos.y), 0, buff, 1 * sizeof(float), sizeof(float));

                    var writer =
                        AmongUsClient.Instance.StartRpc(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.PlaceBomb);
                    writer.WriteBytesAndSize(buff);
                    writer.EndMessage();
                    RPCProcedure.placeBomb(buff);

                    SoundEffectsManager.play("trapperTrap");
                }

                bomberButton.Timer = bomberButton.MaxTimer;
                Bomber.isPlanted = true;
            },
            () =>
            {
                return Bomber.bomber != null && Bomber.bomber == PlayerControl.LocalPlayer &&
                       !PlayerControl.LocalPlayer.Data.IsDead;
            },
            () => { return PlayerControl.LocalPlayer.CanMove && !Bomber.isPlanted; },
            () => { bomberButton.Timer = bomberButton.MaxTimer; },
            Bomber.getButtonSprite(),
            CustomButton.ButtonPositions.upperRowLeft,
            __instance,
            KeyCode.F,
            true,
            Bomber.destructionTime,
            () =>
            {
                bomberButton.Timer = bomberButton.MaxTimer;
                bomberButton.isEffectActive = false;
                bomberButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
            },
            buttonText: "bomberPlantButton"
        );

        defuseButton = new CustomButton(
            () => { defuseButton.HasEffect = true; },
            () =>
            {
                if (shifterShiftButton.HasButton())
                    defuseButton.PositionOffset = new Vector3(0f, 2f, 0f);
                else
                    defuseButton.PositionOffset = new Vector3(0f, 1f, 0f);
                return Bomber.bomb != null && Bomb.canDefuse && !PlayerControl.LocalPlayer.Data.IsDead;
            },
            () =>
            {
                if (defuseButton.isEffectActive && !Bomb.canDefuse)
                {
                    defuseButton.Timer = 0f;
                    defuseButton.isEffectActive = false;
                }

                return PlayerControl.LocalPlayer.CanMove;
            },
            () =>
            {
                defuseButton.Timer = 0f;
                defuseButton.isEffectActive = false;
            },
            Bomb.getDefuseSprite(),
            new Vector3(0f, 1f, 0),
            __instance,
            null,
            true,
            Bomber.defuseDuration,
            () =>
            {
                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                    (byte)CustomRPC.DefuseBomb, SendOption.Reliable);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.defuseBomb();

                defuseButton.Timer = 0f;
                Bomb.canDefuse = false;
            },
            true,
            "defuseButtonText"
        );

        thiefKillButton = new CustomButton(
            () =>
            {
                var thief = Thief.thief;
                var target = Thief.currentTarget;
                var result = Helpers.checkMuderAttempt(thief, target);
                if (result == MurderAttemptResult.BlankKill)
                {
                    thiefKillButton.Timer = thiefKillButton.MaxTimer;
                    return;
                }

                if (Thief.suicideFlag)
                {
                    // Suicide
                    var writer2 = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                        (byte)CustomRPC.UncheckedMurderPlayer, SendOption.Reliable);
                    writer2.Write(thief.PlayerId);
                    writer2.Write(thief.PlayerId);
                    writer2.Write(0);
                    RPCProcedure.uncheckedMurderPlayer(thief.PlayerId, thief.PlayerId, 0);
                    AmongUsClient.Instance.FinishRpcImmediately(writer2);
                    Thief.thief.clearAllTasks();
                }

                // Steal role if survived.
                if (!Thief.thief.Data.IsDead && result == MurderAttemptResult.PerformKill)
                {
                    var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                        (byte)CustomRPC.ThiefStealsRole, SendOption.Reliable);
                    writer.Write(target.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    RPCProcedure.thiefStealsRole(target.PlayerId);
                }

                // Kill the victim (after becoming their role - so that no win is triggered for other teams)
                if (result == MurderAttemptResult.PerformKill)
                {
                    var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                        (byte)CustomRPC.UncheckedMurderPlayer, SendOption.Reliable);
                    writer.Write(thief.PlayerId);
                    writer.Write(target.PlayerId);
                    writer.Write(byte.MaxValue);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    RPCProcedure.uncheckedMurderPlayer(thief.PlayerId, target.PlayerId, byte.MaxValue);
                }
            },
            () =>
            {
                return Thief.thief != null && PlayerControl.LocalPlayer == Thief.thief &&
                       !PlayerControl.LocalPlayer.Data.IsDead;
            },
            () => { return Thief.currentTarget != null && PlayerControl.LocalPlayer.CanMove; },
            () => { thiefKillButton.Timer = thiefKillButton.MaxTimer; },
            __instance.KillButton.graphic.sprite,
            CustomButton.ButtonPositions.upperRowRight,
            __instance,
            KeyCode.Q
        );

        // Trapper Charges
        trapperChargesText = GameObject.Instantiate(trapperButton.actionButton.cooldownTimerText,
            trapperButton.actionButton.cooldownTimerText.transform.parent);
        trapperChargesText.text = "";
        trapperChargesText.enableWordWrapping = false;
        trapperChargesText.transform.localScale = Vector3.one * 0.5f;
        trapperChargesText.transform.localPosition += new Vector3(-0.05f, 0.7f, 0);


        // Yoyo button
        yoyoButton = new CustomButton(
            () =>
            {
                var pos = PlayerControl.LocalPlayer.transform.position;
                var buff = new byte[sizeof(float) * 2];
                Buffer.BlockCopy(BitConverter.GetBytes(pos.x), 0, buff, 0 * sizeof(float), sizeof(float));
                Buffer.BlockCopy(BitConverter.GetBytes(pos.y), 0, buff, 1 * sizeof(float), sizeof(float));

                if (Yoyo.markedLocation == null)
                {
                    var writer = AmongUsClient.Instance.StartRpc(PlayerControl.LocalPlayer.NetId,
                        (byte)CustomRPC.YoyoMarkLocation);
                    writer.WriteBytesAndSize(buff);
                    writer.EndMessage();
                    RPCProcedure.yoyoMarkLocation(buff);
                    SoundEffectsManager.play("tricksterPlaceBox");
                    yoyoButton.Sprite = Yoyo.getBlinkButtonSprite();
                    yoyoButton.Timer = 10f;
                    yoyoButton.HasEffect = false;
                    yoyoButton.buttonText = "yoyoButtonBlink".Translate();
                }
                else
                {
                    // Jump to location
                    var exit = (Vector3)Yoyo.markedLocation;
                    if (SubmergedCompatibility.IsSubmerged) SubmergedCompatibility.ChangeFloor(exit.y > -7);
                    var writer =
                        AmongUsClient.Instance.StartRpc(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.YoyoBlink);
                    writer.Write(byte.MaxValue);
                    writer.WriteBytesAndSize(buff);
                    writer.EndMessage();
                    RPCProcedure.yoyoBlink(true, buff);
                    yoyoButton.EffectDuration = Yoyo.blinkDuration;
                    yoyoButton.Timer = 10f;
                    yoyoButton.HasEffect = true;
                    yoyoButton.buttonText = "yoyoButtonBlinkReturning".Translate();
                    SoundEffectsManager.play("morphlingMorph");
                }
            },
            () =>
            {
                return Yoyo.yoyo != null && Yoyo.yoyo == PlayerControl.LocalPlayer &&
                       !PlayerControl.LocalPlayer.Data.IsDead;
            },
            () => { return PlayerControl.LocalPlayer.CanMove; },
            () =>
            {
                if (Yoyo.markStaysOverMeeting)
                {
                    yoyoButton.Timer = 10f;
                }
                else
                {
                    Yoyo.markedLocation = null;
                    yoyoButton.Timer = yoyoButton.MaxTimer;
                    yoyoButton.Sprite = Yoyo.getMarkButtonSprite();
                    yoyoButton.buttonText = "yoyoButtonMark".Translate();
                }
            },
            Yoyo.getMarkButtonSprite(),
            CustomButton.ButtonPositions.upperRowLeft,
            __instance,
            KeyCode.F,
            false,
            Yoyo.blinkDuration,
            () =>
            {
                if (TransportationToolPatches.isUsingTransportation(Yoyo.yoyo))
                {
                    yoyoButton.Timer = 0.5f;
                    yoyoButton.DeputyTimer = 0.5f;
                    yoyoButton.isEffectActive = true;
                    yoyoButton.actionButton.cooldownTimerText.color = new Color(0F, 0.8F, 0F);
                    return;
                }

                if (Yoyo.yoyo.inVent) __instance.ImpostorVentButton.DoClick();

                // jump back!
                var pos = PlayerControl.LocalPlayer.transform.position;
                var buff = new byte[sizeof(float) * 2];
                Buffer.BlockCopy(BitConverter.GetBytes(pos.x), 0, buff, 0 * sizeof(float), sizeof(float));
                Buffer.BlockCopy(BitConverter.GetBytes(pos.y), 0, buff, 1 * sizeof(float), sizeof(float));
                var exit = (Vector3)Yoyo.markedLocation;
                if (SubmergedCompatibility.IsSubmerged) SubmergedCompatibility.ChangeFloor(exit.y > -7);
                var writer =
                    AmongUsClient.Instance.StartRpc(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.YoyoBlink);
                writer.Write((byte)0);
                writer.WriteBytesAndSize(buff);
                writer.EndMessage();
                RPCProcedure.yoyoBlink(false, buff);

                yoyoButton.Timer = yoyoButton.MaxTimer;
                yoyoButton.isEffectActive = false;
                yoyoButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
                yoyoButton.HasEffect = false;
                yoyoButton.Sprite = Yoyo.getMarkButtonSprite();
                yoyoButton.buttonText = "yoyoButtonMark".Translate();
                SoundEffectsManager.play("morphlingMorph");
                if (Minigame.Instance) Minigame.Instance.Close();
            },
            buttonText: "yoyoButtonMark"
        );

        yoyoAdminTableButton = new CustomButton(
            () =>
            {
                if (!MapBehaviour.Instance || !MapBehaviour.Instance.isActiveAndEnabled)
                {
                    var __instance = FastDestroyableSingleton<HudManager>.Instance;
                    __instance.InitMap();
                    MapBehaviour.Instance.ShowCountOverlay(true, true, true);
                }
            },
            () =>
            {
                return Yoyo.yoyo != null && Yoyo.yoyo == PlayerControl.LocalPlayer && Yoyo.hasAdminTable &&
                       !PlayerControl.LocalPlayer.Data.IsDead;
            },
            () => { return true; },
            () =>
            {
                yoyoAdminTableButton.Timer = yoyoAdminTableButton.MaxTimer;
                yoyoAdminTableButton.isEffectActive = false;
                yoyoAdminTableButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
            },
            Hacker.getAdminSprite(),
            CustomButton.ButtonPositions.lowerRowCenter,
            __instance,
            KeyCode.G,
            true,
            0f,
            () =>
            {
                yoyoAdminTableButton.Timer = yoyoAdminTableButton.MaxTimer;
                if (MapBehaviour.Instance && MapBehaviour.Instance.isActiveAndEnabled) MapBehaviour.Instance.Close();
            },
            GameOptionsManager.Instance.currentNormalGameOptions.MapId == 3,
            FastDestroyableSingleton<TranslationController>.Instance.GetString(StringNames.Admin)
        );


        zoomOutButton = new CustomButton(
            () => { Helpers.toggleZoom(); },
            () =>
            {
                if (PlayerControl.LocalPlayer == null || !PlayerControl.LocalPlayer.Data.IsDead ||
                    (PlayerControl.LocalPlayer.Data.Role.IsImpostor &&
                     !CustomOptionHolder.deadImpsBlockSabotage.getBool())) return false;
                var (playerCompleted, playerTotal) = TasksHandler.taskInfo(PlayerControl.LocalPlayer.Data);
                var numberOfLeftTasks = playerTotal - playerCompleted;
                return numberOfLeftTasks <= 0 || !CustomOptionHolder.finishTasksBeforeHauntingOrZoomingOut.getBool();
            },
            () => { return true; },
            () => { },
            null, // Invisible button!
            new Vector3(0.4f, 2.8f, 0),
            __instance,
            KeyCode.KeypadPlus
        );
        zoomOutButton.Timer = 0f;


        hunterLighterButton = new CustomButton(
            () =>
            {
                Hunter.lightActive.Add(PlayerControl.LocalPlayer.PlayerId);
                SoundEffectsManager.play("lighterLight");

                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                    (byte)CustomRPC.ShareTimer, SendOption.Reliable);
                writer.Write(Hunter.lightPunish);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.shareTimer(Hunter.lightPunish);
            },
            () => { return HideNSeek.isHunter() && !PlayerControl.LocalPlayer.Data.IsDead; },
            () => { return true; },
            () =>
            {
                hunterLighterButton.Timer = 30f;
                hunterLighterButton.isEffectActive = false;
                hunterLighterButton.actionButton.graphic.color = Palette.EnabledColor;
            },
            Hunter.getLightSprite(),
            CustomButton.ButtonPositions.upperRowFarLeft,
            __instance,
            KeyCode.F,
            true,
            Hunter.lightDuration,
            () =>
            {
                Hunter.lightActive.Remove(PlayerControl.LocalPlayer.PlayerId);
                hunterLighterButton.Timer = hunterLighterButton.MaxTimer;
                SoundEffectsManager.play("lighterLight");
            },
            buttonText: "lighterLightOn"
        );

        hunterAdminTableButton = new CustomButton(
            () =>
            {
                if (!MapBehaviour.Instance || !MapBehaviour.Instance.isActiveAndEnabled)
                {
                    var __instance = FastDestroyableSingleton<HudManager>.Instance;
                    __instance.InitMap();
                    MapBehaviour.Instance.ShowCountOverlay(true, true, false);
                }

                PlayerControl.LocalPlayer.NetTransform.Halt(); // Stop current movement 

                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                    (byte)CustomRPC.ShareTimer, SendOption.Reliable);
                writer.Write(Hunter.AdminPunish);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.shareTimer(Hunter.AdminPunish);
            },
            () => { return HideNSeek.isHunter() && !PlayerControl.LocalPlayer.Data.IsDead; },
            () => { return true; },
            () =>
            {
                hunterAdminTableButton.Timer = hunterAdminTableButton.MaxTimer;
                hunterAdminTableButton.isEffectActive = false;
                hunterAdminTableButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
            },
            Hacker.getAdminSprite(),
            CustomButton.ButtonPositions.lowerRowCenter,
            __instance,
            KeyCode.G,
            true,
            Hunter.AdminDuration,
            () =>
            {
                hunterAdminTableButton.Timer = hunterAdminTableButton.MaxTimer;
                if (MapBehaviour.Instance && MapBehaviour.Instance.isActiveAndEnabled) MapBehaviour.Instance.Close();
            },
            false,
            FastDestroyableSingleton<TranslationController>.Instance.GetString(StringNames.Admin)
        );

        hunterArrowButton = new CustomButton(
            () =>
            {
                Hunter.arrowActive = true;
                SoundEffectsManager.play("trackerTrackPlayer");

                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                    (byte)CustomRPC.ShareTimer, SendOption.Reliable);
                writer.Write(Hunter.ArrowPunish);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.shareTimer(Hunter.ArrowPunish);
            },
            () => { return HideNSeek.isHunter() && !PlayerControl.LocalPlayer.Data.IsDead; },
            () => { return true; },
            () =>
            {
                hunterArrowButton.Timer = 30f;
                hunterArrowButton.isEffectActive = false;
                hunterArrowButton.actionButton.graphic.color = Palette.EnabledColor;
            },
            Hunter.getArrowSprite(),
            CustomButton.ButtonPositions.upperRowLeft,
            __instance,
            KeyCode.R,
            true,
            Hunter.ArrowDuration,
            () =>
            {
                Hunter.arrowActive = false;
                hunterArrowButton.Timer = hunterArrowButton.MaxTimer;
                SoundEffectsManager.play("trackerTrackPlayer");
            }
        );

        huntedShieldButton = new CustomButton(
            () =>
            {
                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                    (byte)CustomRPC.HuntedShield, SendOption.Reliable);
                writer.Write(PlayerControl.LocalPlayer.PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.huntedShield(PlayerControl.LocalPlayer.PlayerId);
                SoundEffectsManager.play("timemasterShield");

                Hunted.shieldCount--;
            },
            () => { return HideNSeek.isHunted() && !PlayerControl.LocalPlayer.Data.IsDead; },
            () =>
            {
                if (huntedShieldCountText != null) huntedShieldCountText.text = $"{Hunted.shieldCount}";
                return PlayerControl.LocalPlayer.CanMove && Hunted.shieldCount > 0;
            },
            () =>
            {
                huntedShieldButton.Timer = huntedShieldButton.MaxTimer;
                huntedShieldButton.isEffectActive = false;
                huntedShieldButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
            },
            TimeMaster.getButtonSprite(),
            CustomButton.ButtonPositions.lowerRowRight,
            __instance,
            KeyCode.F,
            true,
            Hunted.shieldDuration,
            () =>
            {
                huntedShieldButton.Timer = huntedShieldButton.MaxTimer;
                SoundEffectsManager.stop("timemasterShield");
            },
            buttonText: "medicShield"
        );

        huntedShieldCountText = GameObject.Instantiate(huntedShieldButton.actionButton.cooldownTimerText,
            huntedShieldButton.actionButton.cooldownTimerText.transform.parent);
        huntedShieldCountText.text = "";
        huntedShieldCountText.enableWordWrapping = false;
        huntedShieldCountText.transform.localScale = Vector3.one * 0.5f;
        huntedShieldCountText.transform.localPosition += new Vector3(-0.05f, 0.7f, 0);


        propDisguiseButton = new CustomButton(
            () =>
            {
                // Prop stuff
                var player = PlayerControl.LocalPlayer;
                var disguiseTarget = PropHunt.currentTarget;
                if (disguiseTarget != null)
                {
                    player.transform.localScale = disguiseTarget.transform.lossyScale;
                    var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                        (byte)CustomRPC.SetProp, SendOption.Reliable);
                    writer.Write(PlayerControl.LocalPlayer.PlayerId);
                    writer.Write(disguiseTarget.gameObject.name);
                    writer.Write(disguiseTarget.gameObject.transform.position.x);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    RPCProcedure.propHuntSetProp(PlayerControl.LocalPlayer.PlayerId, disguiseTarget.gameObject.name,
                        disguiseTarget.gameObject.transform.position.x);
                    SoundEffectsManager.play("morphlingMorph");
                    propDisguiseButton.Timer = 1f;
                }
            },
            () =>
            {
                return PropHunt.isPropHuntGM && !PlayerControl.LocalPlayer.Data.Role.IsImpostor &&
                       !PlayerControl.LocalPlayer.Data.IsDead;
            },
            () =>
            {
                propSpriteRenderer.sprite = PropHunt.currentTarget?.GetComponent<SpriteRenderer>()?.sprite;
                if (propSpriteRenderer.sprite == null)
                    propSpriteRenderer.sprite = PropHunt.currentTarget?.transform
                        .GetComponentInChildren<SpriteRenderer>()?.sprite;
                if (propSpriteRenderer.sprite != null)
                    propSpriteHolder.transform.localScale *= 1 / propSpriteRenderer.bounds.size.magnitude;
                return PropHunt.currentTarget != null &&
                       PropHunt.currentTarget?.GetComponent<SpriteRenderer>()?.sprite != null;
            },
            () => { },
            null,
            CustomButton.ButtonPositions.lowerRowRight,
            __instance,
            KeyCode.F,
            buttonText: "propDisguiseButtonText"
        );
        propSpriteHolder = new GameObject("TORPropButtonPropSpritePreview");
        propSpriteRenderer = propSpriteHolder.AddComponent<SpriteRenderer>();
        propSpriteHolder.transform.SetParent(propDisguiseButton.actionButtonGameObject.transform, false);
        propSpriteHolder.transform.localPosition = new Vector3(0, 0, -2f);

        propHuntUnstuckButton = new CustomButton(
            () => { PlayerControl.LocalPlayer.Collider.enabled = false; },
            () => { return PropHunt.isPropHuntGM && !PlayerControl.LocalPlayer.Data.IsDead; },
            () => { return true; },
            () => { },
            PropHunt.getUnstuckButtonSprite(),
            CustomButton.ButtonPositions.upperRowLeft,
            __instance,
            KeyCode.LeftShift,
            true,
            1f,
            () =>
            {
                PlayerControl.LocalPlayer.Collider.enabled = true;
                propHuntUnstuckButton.Timer = propHuntUnstuckButton.MaxTimer;
            },
            buttonText: "propHuntUnstuckButtonText"
        );

        propHuntRevealButton = new CustomButton(
            () =>
            {
                // select a random crewplayer to reveal.
                var candidates = PlayerControl.AllPlayerControls.ToArray().Where(x =>
                        !x.Data.Role.IsImpostor && !x.Data.IsDead &&
                        !PropHunt.isCurrentlyRevealed.ContainsKey(x.PlayerId))
                    .ToList();
                var rng = new Random();
                var selectedPlayer = candidates[rng.Next(candidates.Count)];
                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                    (byte)CustomRPC.SetRevealed, SendOption.Reliable);
                writer.Write(selectedPlayer.PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.propHuntSetRevealed(selectedPlayer.PlayerId);
            },
            () =>
            {
                return PropHunt.isPropHuntGM && !PlayerControl.LocalPlayer.Data.IsDead &&
                       PlayerControl.LocalPlayer.Data.Role.IsImpostor;
            },
            () => { return PropHunt.timer - PropHunt.revealPunish > 0; },
            () => { },
            PropHunt.getRevealButtonSprite(),
            CustomButton.ButtonPositions.upperRowFarLeft,
            __instance,
            KeyCode.R,
            true,
            5f,
            () => { propHuntRevealButton.Timer = propHuntRevealButton.MaxTimer; },
            buttonText: "propHuntRevealButtonText"
        );

        propHuntInvisButton = new CustomButton(
            () =>
            {
                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                    (byte)CustomRPC.PropHuntSetInvis, SendOption.Reliable);
                writer.Write(PlayerControl.LocalPlayer.PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.propHuntSetInvis(PlayerControl.LocalPlayer.PlayerId);
                SoundEffectsManager.play("morphlingMorph");
            },
            () =>
            {
                return PropHunt.isPropHuntGM && !PlayerControl.LocalPlayer.Data.IsDead &&
                       !PlayerControl.LocalPlayer.Data.Role.IsImpostor && PropHunt.enableInvis;
            },
            () => { return PropHunt.currentObject.ContainsKey(PlayerControl.LocalPlayer.PlayerId); },
            () => { },
            PropHunt.getInvisButtonSprite(),
            CustomButton.ButtonPositions.upperRowFarLeft,
            __instance,
            KeyCode.I,
            true,
            5f,
            () =>
            {
                SoundEffectsManager.play("morphlingMorph");
                propHuntInvisButton.Timer = propHuntInvisButton.MaxTimer;
            },
            buttonText: "propHuntInvisButtonText"
        );

        propHuntSpeedboostButton = new CustomButton(
            () =>
            {
                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                    (byte)CustomRPC.PropHuntSetSpeedboost, SendOption.Reliable);
                writer.Write(PlayerControl.LocalPlayer.PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.propHuntSetSpeedboost(PlayerControl.LocalPlayer.PlayerId);
                SoundEffectsManager.play("timemasterShield");
            },
            () =>
            {
                return PropHunt.isPropHuntGM && !PlayerControl.LocalPlayer.Data.IsDead &&
                       !PlayerControl.LocalPlayer.Data.Role.IsImpostor && PropHunt.enableSpeedboost;
            },
            () => { return true; },
            () => { },
            PropHunt.getSpeedboostButtonSprite(),
            CustomButton.ButtonPositions.lowerRowCenter,
            __instance,
            KeyCode.G,
            true,
            5f,
            () =>
            {
                SoundEffectsManager.stop("timemasterShield");
                propHuntSpeedboostButton.Timer = propHuntSpeedboostButton.MaxTimer;
            },
            buttonText: "propHuntSpeedboostButtonText"
        );

        propHuntAdminButton = new CustomButton(
            () =>
            {
                if (!MapBehaviour.Instance || !MapBehaviour.Instance.isActiveAndEnabled)
                {
                    var __instance = FastDestroyableSingleton<HudManager>.Instance;
                    __instance.InitMap();
                    MapBehaviour.Instance.ShowCountOverlay(true, true, false);
                }

                PlayerControl.LocalPlayer.NetTransform.Halt(); // Stop current movement
            },
            () =>
            {
                return PropHunt.isPropHuntGM && !PlayerControl.LocalPlayer.Data.IsDead &&
                       PlayerControl.LocalPlayer.Data.Role.IsImpostor;
            },
            () =>
            {
                propHuntAdminButton.PositionOffset = PlayerControl.LocalPlayer.inVent
                    ? CustomButton.ButtonPositions.farUpperRowLeft
                    : CustomButton.ButtonPositions.upperRowCenter;
                return !PlayerControl.LocalPlayer.inVent;
            },
            () =>
            {
                propHuntAdminButton.Timer = hunterAdminTableButton.MaxTimer;
                propHuntAdminButton.isEffectActive = false;
                propHuntAdminButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
            },
            Hacker.getAdminSprite(),
            CustomButton.ButtonPositions.farUpperRowLeft,
            __instance,
            KeyCode.G,
            true,
            PropHunt.adminDuration,
            () =>
            {
                propHuntAdminButton.Timer = propHuntAdminButton.MaxTimer;
                if (MapBehaviour.Instance && MapBehaviour.Instance.isActiveAndEnabled) MapBehaviour.Instance.Close();
            },
            false,
            FastDestroyableSingleton<TranslationController>.Instance.GetString(StringNames.Admin)
        );
        propHuntFindButton = new CustomButton(
            () => { SoundEffectsManager.play("timemasterShield"); },
            () =>
            {
                return PropHunt.isPropHuntGM && !PlayerControl.LocalPlayer.Data.IsDead &&
                       PlayerControl.LocalPlayer.Data.Role.IsImpostor;
            },
            () => { return true; },
            () => { },
            PropHunt.getFindButtonSprite(),
            CustomButton.ButtonPositions.lowerRowCenter,
            __instance,
            KeyCode.F,
            true,
            5f,
            () =>
            {
                SoundEffectsManager.stop("timemasterShield");
                propHuntFindButton.Timer = propHuntFindButton.MaxTimer;
                propHuntFindButton.isEffectActive = false;
            },
            buttonText: "trackerTrackCorpses"
        );

        eventKickButton = new CustomButton(
            () => { EventUtility.kickTarget(); },
            () =>
            {
                return EventUtility.isEnabled && Mini.mini != null && !Mini.mini.Data.IsDead &&
                       PlayerControl.LocalPlayer != Mini.mini;
            },
            () => { return EventUtility.currentTarget != null; },
            () => { },
            EventUtility.getKickButtonSprite(),
            CustomButton.ButtonPositions.highRowRight,
            __instance,
            KeyCode.K,
            true,
            3f,
            () =>
            {
                // onEffectEnds
                eventKickButton.Timer = 69;
            },
            buttonText: "eventKickButtonText"
        );

        // Set the default (or settings from the previous game) timers / durations when spawning the buttons
        initialized = true;
        setCustomButtonCooldowns();
        deputyHandcuffedButtons = new Dictionary<byte, List<CustomButton>>();
    }
}