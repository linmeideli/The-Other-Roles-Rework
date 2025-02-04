using System.Collections.Generic;
using UnityEngine;
using static TheOtherRoles.TheOtherRoles;
using Types = TheOtherRoles.CustomOption.CustomOptionType;
using TheOtherRoles.Modules;
using System;
using Il2CppSystem;

namespace TheOtherRoles {
    public class CustomOptionHolder {
        public static string[] rates = new string[]{"0%", "10%", "20%", "30%", "40%", "50%", "60%", "70%", "80%", "90%", "100%"};
        public static string[] ratesModifier = new string[]{"1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15" };
        public static string[] presets = new string[]{ "preset1".Translate(), "preset2".Translate(), "RandomPresetSkeld".Translate(), "RandomPresetMira".Translate(), "RandomPresetPolus".Translate(), "RandomPresetAirship".Translate(), "RandomPresetSubmerged".Translate() };

        public static CustomOption presetSelection;
        public static CustomOption activateRoles;
        public static CustomOption crewmateRolesCountMin;
        public static CustomOption crewmateRolesCountMax;
        public static CustomOption crewmateRolesFill;
        public static CustomOption neutralRolesCountMin;
        public static CustomOption neutralRolesCountMax;
        public static CustomOption impostorRolesCountMin;
        public static CustomOption impostorRolesCountMax;
        public static CustomOption modifiersCountMin;
        public static CustomOption modifiersCountMax;

        public static CustomOption anyPlayerCanStopStart;
        public static CustomOption enableEventMode;
        public static CustomOption deadImpsBlockSabotage;

        public static CustomOption mafiaSpawnRate;
        public static CustomOption janitorCooldown;

        public static CustomOption morphlingSpawnRate;
        public static CustomOption morphlingCooldown;
        public static CustomOption morphlingDuration;

        public static CustomOption camouflagerSpawnRate;
        public static CustomOption camouflagerCooldown;
        public static CustomOption camouflagerDuration;

        public static CustomOption vampireSpawnRate;
        public static CustomOption vampireKillDelay;
        public static CustomOption vampireCooldown;
        public static CustomOption vampireCanKillNearGarlics;

        public static CustomOption eraserSpawnRate;
        public static CustomOption eraserCooldown;
        public static CustomOption eraserCanEraseAnyone;
        public static CustomOption guesserSpawnRate;
        public static CustomOption guesserIsImpGuesserRate;
        public static CustomOption guesserNumberOfShots;
        public static CustomOption guesserHasMultipleShotsPerMeeting;
        public static CustomOption guesserKillsThroughShield;
        public static CustomOption guesserEvilCanKillSpy;
        public static CustomOption guesserSpawnBothRate;
        public static CustomOption guesserCantGuessSnitchIfTaksDone;

        public static CustomOption jesterSpawnRate;
        public static CustomOption jesterCanCallEmergency;
        public static CustomOption jesterHasImpostorVision;

        public static CustomOption arsonistSpawnRate;
        public static CustomOption arsonistCooldown;
        public static CustomOption arsonistDuration;

        public static CustomOption jackalSpawnRate;
        public static CustomOption jackalKillCooldown;
        public static CustomOption jackalCreateSidekickCooldown;
        public static CustomOption jackalCanSabotageLights;
        public static CustomOption jackalCanUseVents;
        public static CustomOption jackalCanCreateSidekick;
        public static CustomOption sidekickPromotesToJackal;
        public static CustomOption sidekickCanKill;
        public static CustomOption sidekickCanUseVents;
        public static CustomOption sidekickCanSabotageLights;
        public static CustomOption jackalPromotedFromSidekickCanCreateSidekick;
        public static CustomOption jackalCanCreateSidekickFromImpostor;
        public static CustomOption jackalAndSidekickHaveImpostorVision;

        public static CustomOption bountyHunterSpawnRate;
        public static CustomOption bountyHunterBountyDuration;
        public static CustomOption bountyHunterReducedCooldown;
        public static CustomOption bountyHunterPunishmentTime;
        public static CustomOption bountyHunterShowArrow;
        public static CustomOption bountyHunterArrowUpdateIntervall;
        public static CustomOption bountyHunterShowCooldownForGhosts;

        public static CustomOption witchSpawnRate;
        public static CustomOption witchCooldown;
        public static CustomOption witchAdditionalCooldown;
        public static CustomOption witchCanSpellAnyone;
        public static CustomOption witchSpellCastingDuration;
        public static CustomOption witchTriggerBothCooldowns;
        public static CustomOption witchVoteSavesTargets;

        public static CustomOption ninjaSpawnRate;
        public static CustomOption ninjaCooldown;
        public static CustomOption ninjaKnowsTargetLocation;
        public static CustomOption ninjaTraceTime;
        public static CustomOption ninjaTraceColorTime;
        public static CustomOption ninjaInvisibleDuration;

        public static CustomOption mayorSpawnRate;
        public static CustomOption mayorCanSeeVoteColors;
        public static CustomOption mayorTasksNeededToSeeVoteColors;
        public static CustomOption mayorMeetingButton;
        public static CustomOption mayorMaxRemoteMeetings;
        public static CustomOption mayorChooseSingleVote;

        public static CustomOption portalmakerSpawnRate;
        public static CustomOption portalmakerCooldown;
        public static CustomOption portalmakerUsePortalCooldown;
        public static CustomOption portalmakerLogOnlyColorType;
        public static CustomOption portalmakerLogHasTime;
        public static CustomOption portalmakerCanPortalFromAnywhere;

        public static CustomOption engineerSpawnRate;
        public static CustomOption engineerNumberOfFixes;
        public static CustomOption engineerHighlightForImpostors;
        public static CustomOption engineerHighlightForTeamJackal;

        public static CustomOption sheriffSpawnRate;
        public static CustomOption sheriffCooldown;
        public static CustomOption sheriffCanKillNeutrals;
        public static CustomOption deputySpawnRate;

        public static CustomOption deputyNumberOfHandcuffs;
        public static CustomOption deputyHandcuffCooldown;
        public static CustomOption deputyGetsPromoted;
        public static CustomOption deputyKeepsHandcuffs;
        public static CustomOption deputyHandcuffDuration;
        public static CustomOption deputyKnowsSheriff;

        public static CustomOption lighterSpawnRate;
        public static CustomOption lighterModeLightsOnVision;
        public static CustomOption lighterModeLightsOffVision;
        public static CustomOption lighterFlashlightWidth;

        public static CustomOption detectiveSpawnRate;
        public static CustomOption detectiveAnonymousFootprints;
        public static CustomOption detectiveFootprintIntervall;
        public static CustomOption detectiveFootprintDuration;
        public static CustomOption detectiveReportNameDuration;
        public static CustomOption detectiveReportColorDuration;

        public static CustomOption timeMasterSpawnRate;
        public static CustomOption timeMasterCooldown;
        public static CustomOption timeMasterRewindTime;
        public static CustomOption timeMasterShieldDuration;

        public static CustomOption medicSpawnRate;
        public static CustomOption medicShowShielded;
        public static CustomOption medicShowAttemptToShielded;
        public static CustomOption medicSetOrShowShieldAfterMeeting;
        public static CustomOption medicShowAttemptToMedic;
        public static CustomOption medicSetShieldAfterMeeting;

        public static CustomOption swapperSpawnRate;
        public static CustomOption swapperCanCallEmergency;
        public static CustomOption swapperCanOnlySwapOthers;
        public static CustomOption swapperSwapsNumber;
        public static CustomOption swapperRechargeTasksNumber;

        public static CustomOption seerSpawnRate;
        public static CustomOption seerMode;
        public static CustomOption seerSoulDuration;
        public static CustomOption seerLimitSoulDuration;

        public static CustomOption hackerSpawnRate;
        public static CustomOption hackerCooldown;
        public static CustomOption hackerHackeringDuration;
        public static CustomOption hackerOnlyColorType;
        public static CustomOption hackerToolsNumber;
        public static CustomOption hackerRechargeTasksNumber;
        public static CustomOption hackerNoMove;

        public static CustomOption trackerSpawnRate;
        public static CustomOption trackerUpdateIntervall;
        public static CustomOption trackerResetTargetAfterMeeting;
        public static CustomOption trackerCanTrackCorpses;
        public static CustomOption trackerCorpsesTrackingCooldown;
        public static CustomOption trackerCorpsesTrackingDuration;
        public static CustomOption trackerTrackingMethod;

        public static CustomOption snitchSpawnRate;
        public static CustomOption snitchLeftTasksForReveal;
        public static CustomOption snitchMode;
        public static CustomOption snitchTargets;

        public static CustomOption spySpawnRate;
        public static CustomOption spyCanDieToSheriff;
        public static CustomOption spyImpostorsCanKillAnyone;
        public static CustomOption spyCanEnterVents;
        public static CustomOption spyHasImpostorVision;

        public static CustomOption tricksterSpawnRate;
        public static CustomOption tricksterPlaceBoxCooldown;
        public static CustomOption tricksterLightsOutCooldown;
        public static CustomOption tricksterLightsOutDuration;

        public static CustomOption cleanerSpawnRate;
        public static CustomOption cleanerCooldown;
        
        public static CustomOption warlockSpawnRate;
        public static CustomOption warlockCooldown;
        public static CustomOption warlockRootTime;

        public static CustomOption securityGuardSpawnRate;
        public static CustomOption securityGuardCooldown;
        public static CustomOption securityGuardTotalScrews;
        public static CustomOption securityGuardCamPrice;
        public static CustomOption securityGuardVentPrice;
        public static CustomOption securityGuardCamDuration;
        public static CustomOption securityGuardCamMaxCharges;
        public static CustomOption securityGuardCamRechargeTasksNumber;
        public static CustomOption securityGuardNoMove;

        public static CustomOption vultureSpawnRate;
        public static CustomOption vultureCooldown;
        public static CustomOption vultureNumberToWin;
        public static CustomOption vultureCanUseVents;
        public static CustomOption vultureShowArrows;

        public static CustomOption mediumSpawnRate;
        public static CustomOption mediumCooldown;
        public static CustomOption mediumDuration;
        public static CustomOption mediumOneTimeUse;
        public static CustomOption mediumChanceAdditionalInfo;

        public static CustomOption lawyerSpawnRate;
        public static CustomOption lawyerIsProsecutorChance;
        public static CustomOption lawyerTargetCanBeJester;
        public static CustomOption lawyerVision;
        public static CustomOption lawyerKnowsRole;
        public static CustomOption lawyerCanCallEmergency;
        public static CustomOption pursuerCooldown;
        public static CustomOption pursuerBlanksNumber;

        public static CustomOption thiefSpawnRate;
        public static CustomOption thiefCooldown;
        public static CustomOption thiefHasImpVision;
        public static CustomOption thiefCanUseVents;
        public static CustomOption thiefCanKillSheriff;
        public static CustomOption thiefCanStealWithGuess;


        public static CustomOption trapperSpawnRate;
        public static CustomOption trapperCooldown;
        public static CustomOption trapperMaxCharges;
        public static CustomOption trapperRechargeTasksNumber;
        public static CustomOption trapperTrapNeededTriggerToReveal;
        public static CustomOption trapperAnonymousMap;
        public static CustomOption trapperInfoType;
        public static CustomOption trapperTrapDuration;

        public static CustomOption bomberSpawnRate;
        public static CustomOption bomberBombDestructionTime;
        public static CustomOption bomberBombDestructionRange;
        public static CustomOption bomberBombHearRange;
        public static CustomOption bomberDefuseDuration;
        public static CustomOption bomberBombCooldown;
        public static CustomOption bomberBombActiveAfter;

        public static CustomOption yoyoSpawnRate;
        public static CustomOption yoyoBlinkDuration;
        public static CustomOption yoyoMarkCooldown;
        public static CustomOption yoyoMarkStaysOverMeeting;
        public static CustomOption yoyoHasAdminTable;
        public static CustomOption yoyoAdminTableCooldown;
        public static CustomOption yoyoSilhouetteVisibility;

        public static CustomOption prophetSpawnRate;
        public static CustomOption prophetCooldown;
        public static CustomOption prophetNumExamines;
        public static CustomOption prophetKillCrewAsRed;
        public static CustomOption NeutralAsRed;
        public static CustomOption prophetCanCallEmergency;
        

        public static CustomOption modifiersAreHidden;

        public static CustomOption modifierBait;
        public static CustomOption modifierBaitQuantity;
        public static CustomOption modifierBaitReportDelayMin;
        public static CustomOption modifierBaitReportDelayMax;
        public static CustomOption modifierBaitShowKillFlash;

        public static CustomOption modifierLover;
        public static CustomOption modifierLoverImpLoverRate;
        public static CustomOption modifierLoverBothDie;
        public static CustomOption modifierLoverEnableChat;

        public static CustomOption modifierBloody;
        public static CustomOption modifierBloodyQuantity;
        public static CustomOption modifierBloodyDuration;

        public static CustomOption modifierAntiTeleport;
        public static CustomOption modifierAntiTeleportQuantity;

        public static CustomOption modifierTieBreaker;

        public static CustomOption modifierSunglasses;
        public static CustomOption modifierSunglassesQuantity;
        public static CustomOption modifierSunglassesVision;

        public static CustomOption modifierLighterln;
       // public static CustomOption modifierLighterlnQuantity;
        //public static CustomOption modifierLighterlnVision;

        public static CustomOption modifierMini;
        public static CustomOption modifierMiniGrowingUpDuration;
        public static CustomOption modifierMiniGrowingUpInMeeting;

        public static CustomOption modifierVip;
        public static CustomOption modifierVipQuantity;
        public static CustomOption modifierVipShowColor;

        public static CustomOption modifierInvert;
        public static CustomOption modifierInvertQuantity;
        public static CustomOption modifierInvertDuration;

        public static CustomOption modifierChameleon;
        public static CustomOption modifierChameleonQuantity;
        public static CustomOption modifierChameleonHoldDuration;
        public static CustomOption modifierChameleonFadeDuration;
        public static CustomOption modifierChameleonMinVisibility;

        public static CustomOption modifierShifter;

        public static CustomOption maxNumberOfMeetings;
        public static CustomOption blockSkippingInEmergencyMeetings;
        public static CustomOption noVoteIsSelfVote;
        public static CustomOption hidePlayerNames;
        public static CustomOption allowParallelMedBayScans;
        public static CustomOption shieldFirstKill;
        public static CustomOption finishTasksBeforeHauntingOrZoomingOut;
        public static CustomOption camsNightVision;
        public static CustomOption camsNoNightVisionIfImpVision;

        public static CustomOption dynamicMap;
        public static CustomOption dynamicMapEnableSkeld;
        public static CustomOption dynamicMapEnableMira;
        public static CustomOption dynamicMapEnablePolus;
        public static CustomOption dynamicMapEnableAirShip;
        public static CustomOption dynamicMapEnableFungle;
        public static CustomOption dynamicMapEnableSubmerged;
        public static CustomOption dynamicMapSeparateSettings;

        //Guesser Gamemode
        public static CustomOption guesserGamemodeCrewNumber;
        public static CustomOption guesserGamemodeNeutralNumber;
        public static CustomOption guesserGamemodeImpNumber;
        public static CustomOption guesserForceJackalGuesser;
        public static CustomOption guesserForceThiefGuesser;
        public static CustomOption guesserGamemodeHaveModifier;
        public static CustomOption guesserGamemodeNumberOfShots;
        public static CustomOption guesserGamemodeHasMultipleShotsPerMeeting;
        public static CustomOption guesserGamemodeKillsThroughShield;
        public static CustomOption guesserGamemodeEvilCanKillSpy;
        public static CustomOption guesserGamemodeCantGuessSnitchIfTaksDone;
        public static CustomOption guesserGamemodeSidekickIsAlwaysGuesser;

        // Hide N Seek Gamemode
        public static CustomOption hideNSeekHunterCount;
        public static CustomOption hideNSeekKillCooldown;
        public static CustomOption hideNSeekHunterVision;
        public static CustomOption hideNSeekHuntedVision;
        public static CustomOption hideNSeekTimer;
        public static CustomOption hideNSeekCommonTasks;
        public static CustomOption hideNSeekShortTasks;
        public static CustomOption hideNSeekLongTasks;
        public static CustomOption hideNSeekTaskWin;
        public static CustomOption hideNSeekTaskPunish;
        public static CustomOption hideNSeekCanSabotage;
        public static CustomOption hideNSeekMap;
        public static CustomOption hideNSeekHunterWaiting;

        public static CustomOption hunterLightCooldown;
        public static CustomOption hunterLightDuration;
        public static CustomOption hunterLightVision;
        public static CustomOption hunterLightPunish;
        public static CustomOption hunterAdminCooldown;
        public static CustomOption hunterAdminDuration;
        public static CustomOption hunterAdminPunish;
        public static CustomOption hunterArrowCooldown;
        public static CustomOption hunterArrowDuration;
        public static CustomOption hunterArrowPunish;

        public static CustomOption huntedShieldCooldown;
        public static CustomOption huntedShieldDuration;
        public static CustomOption huntedShieldRewindTime;
        public static CustomOption huntedShieldNumber;

        // Prop Hunt Settings
        public static CustomOption propHuntMap;
        public static CustomOption propHuntTimer;
        public static CustomOption propHuntNumberOfHunters;
        public static CustomOption hunterInitialBlackoutTime;
        public static CustomOption hunterMissCooldown;
        public static CustomOption hunterHitCooldown;
        public static CustomOption hunterMaxMissesBeforeDeath;
        public static CustomOption propBecomesHunterWhenFound;
        public static CustomOption propHunterVision;
        public static CustomOption propVision;
        public static CustomOption propHuntRevealCooldown;
        public static CustomOption propHuntRevealDuration;
        public static CustomOption propHuntRevealPunish;
        public static CustomOption propHuntUnstuckCooldown;
        public static CustomOption propHuntUnstuckDuration;
        public static CustomOption propHuntInvisCooldown;
        public static CustomOption propHuntInvisDuration;
        public static CustomOption propHuntSpeedboostCooldown;
        public static CustomOption propHuntSpeedboostDuration;
        public static CustomOption propHuntSpeedboostSpeed;
        public static CustomOption propHuntSpeedboostEnabled;
        public static CustomOption propHuntInvisEnabled;
        public static CustomOption propHuntAdminCooldown;
        public static CustomOption propHuntFindCooldown;
        public static CustomOption propHuntFindDuration;

        //Add Settings
        public static CustomOption AddVents;
        public static CustomOption addPolusVents;
        public static CustomOption addAirShipVents;
        public static CustomOption enableAirShipModify;
        public static CustomOption enableBetterPolus;



        internal static Dictionary<byte, byte[]> blockedRolePairings = new Dictionary<byte, byte[]>();

        public static string cs(Color c, string s) {
            return string.Format("<color=#{0:X2}{1:X2}{2:X2}{3:X2}>{4}</color>", ToByte(c.r), ToByte(c.g), ToByte(c.b), ToByte(c.a), s);
        }
 
        private static byte ToByte(float f) {
            f = Mathf.Clamp01(f);
            return (byte)(f * 255);
        }

        public static bool isMapSelectionOption(CustomOption option) {
            return option == CustomOptionHolder.propHuntMap && option == CustomOptionHolder.hideNSeekMap;
        }

        public static void Load() {

            CustomOption.vanillaSettings = TheOtherRolesPlugin.Instance.Config.Bind("Preset0", "VanillaOptions", "");

            // Role Options
            presetSelection = CustomOption.Create(0, Types.General, cs(new Color(204f / 255f, 204f / 255f, 0, 1f), "presetSelection".Translate()), presets, null, true);

            
            if (Utilities.EventUtility.canBeEnabled) enableEventMode = CustomOption.Create(10423, Types.General, cs(Color.green, ModTranslation.GetString("enableEventMode")), true, null, true);

            // Using new id's for the options to not break compatibilty with older versions
            crewmateRolesCountMin = CustomOption.Create(300, Types.General, cs(new Color(204f / 255f, 204f / 255f, 0, 1f), ModTranslation.GetString("crewmateRolesCountMin")), 15f, 0f, 15f, 1f, null, true, heading: "Min/Max Roles");
            crewmateRolesCountMax = CustomOption.Create(301, Types.General, cs(new Color(204f / 255f, 204f / 255f, 0, 1f), ModTranslation.GetString("crewmateRolesCountMax")), 15f, 0f, 15f, 1f);            
            neutralRolesCountMin = CustomOption.Create(302, Types.General, cs(new Color(204f / 255f, 204f / 255f, 0, 1f), ModTranslation.GetString("neutralRolesCountMin")), 15f, 0f, 15f, 1f);
            neutralRolesCountMax = CustomOption.Create(303, Types.General, cs(new Color(204f / 255f, 204f / 255f, 0, 1f), ModTranslation.GetString("neutralRolesCountMax")), 15f, 0f, 15f, 1f);
            impostorRolesCountMin = CustomOption.Create(304, Types.General, cs(new Color(204f / 255f, 204f / 255f, 0, 1f), ModTranslation.GetString("impostorRolesCountMin")), 15f, 0f, 15f, 1f);
            impostorRolesCountMax = CustomOption.Create(305, Types.General, cs(new Color(204f / 255f, 204f / 255f, 0, 1f), ModTranslation.GetString("impostorRolesCountMax")), 15f, 0f, 15f, 1f);
            modifiersCountMin = CustomOption.Create(306, Types.General, cs(new Color(204f / 255f, 204f / 255f, 0, 1f), ModTranslation.GetString("modifiersCountMin")), 15f, 0f, 15f, 1f);
            modifiersCountMax = CustomOption.Create(307, Types.General, cs(new Color(204f / 255f, 204f / 255f, 0, 1f), ModTranslation.GetString("modifiersCountMax")), 15f, 0f, 15f, 1f);
            crewmateRolesFill = CustomOption.Create(308, Types.General, cs(new Color(204f / 255f, 204f / 255f, 0, 1f), ModTranslation.GetString("crewmateRolesFill")), false);

            mafiaSpawnRate = CustomOption.Create(18, Types.Impostor, cs(Janitor.color, ModTranslation.GetString("mafiaSpawnRate")), rates, null, true);
            janitorCooldown = CustomOption.Create(19, Types.Impostor, ModTranslation.GetString("janitorCooldown"), 30f, 10f, 60f, 2.5f, mafiaSpawnRate);

            morphlingSpawnRate = CustomOption.Create(20, Types.Impostor, cs(Morphling.color, ModTranslation.GetString("Morphling")), rates, null, true);
            morphlingCooldown = CustomOption.Create(21, Types.Impostor, ModTranslation.GetString("morphlingCooldown"), 30f, 10f, 60f, 2.5f, morphlingSpawnRate);
            morphlingDuration = CustomOption.Create(22, Types.Impostor, ModTranslation.GetString("morphlingDuration"), 10f, 1f, 20f, 0.5f, morphlingSpawnRate);

            camouflagerSpawnRate = CustomOption.Create(30, Types.Impostor, cs(Camouflager.color, ModTranslation.GetString("Camouflager")), rates, null, true);
            camouflagerCooldown = CustomOption.Create(31, Types.Impostor, ModTranslation.GetString("camouflagerCooldown"), 30f, 10f, 60f, 2.5f, camouflagerSpawnRate);
            camouflagerDuration = CustomOption.Create(32, Types.Impostor, ModTranslation.GetString("camouflagerDuration"), 10f, 1f, 20f, 0.5f, camouflagerSpawnRate);

            vampireSpawnRate = CustomOption.Create(40, Types.Impostor, cs(Vampire.color, ModTranslation.GetString("Vampire")), rates, null, true);
            vampireKillDelay = CustomOption.Create(41, Types.Impostor, ModTranslation.GetString("vampireKillDelay"), 10f, 1f, 20f, 1f, vampireSpawnRate);
            vampireCooldown = CustomOption.Create(42, Types.Impostor, ModTranslation.GetString("vampireCooldown"), 30f, 10f, 60f, 2.5f, vampireSpawnRate);
            vampireCanKillNearGarlics = CustomOption.Create(43, Types.Impostor, ModTranslation.GetString("vampireCanKillNearGarlics"), true, vampireSpawnRate);

            eraserSpawnRate = CustomOption.Create(230, Types.Impostor, cs(Eraser.color, ModTranslation.GetString("Eraser")), rates, null, true);
            eraserCooldown = CustomOption.Create(231, Types.Impostor, ModTranslation.GetString(ModTranslation.GetString("eraserCooldown")), 30f, 10f, 120f, 5f, eraserSpawnRate);
            eraserCanEraseAnyone = CustomOption.Create(232, Types.Impostor, ModTranslation.GetString(ModTranslation.GetString("eraserCanEraseAnyone")), false, eraserSpawnRate);

            tricksterSpawnRate = CustomOption.Create(250, Types.Impostor, cs(Trickster.color, ModTranslation.GetString("Trickster")), rates, null, true);
            tricksterPlaceBoxCooldown = CustomOption.Create(251, Types.Impostor, ModTranslation.GetString("tricksterPlaceBoxCooldown"), 10f, 2.5f, 30f, 2.5f, tricksterSpawnRate);
            tricksterLightsOutCooldown = CustomOption.Create(252, Types.Impostor, ModTranslation.GetString("tricksterLightsOutCooldown"), 30f, 10f, 60f, 5f, tricksterSpawnRate);
            tricksterLightsOutDuration = CustomOption.Create(253, Types.Impostor, ModTranslation.GetString("tricksterLightsOutDuration"), 15f, 5f, 60f, 2.5f, tricksterSpawnRate);

            cleanerSpawnRate = CustomOption.Create(260, Types.Impostor, cs(Cleaner.color, ModTranslation.GetString("Cleaner")), rates, null, true);
            cleanerCooldown = CustomOption.Create(261, Types.Impostor, ModTranslation.GetString("cleanerCooldown"), 30f, 10f, 60f, 2.5f, cleanerSpawnRate);

            warlockSpawnRate = CustomOption.Create(270, Types.Impostor, cs(Cleaner.color, ModTranslation.GetString("Warlock")), rates, null, true);
            warlockCooldown = CustomOption.Create(271, Types.Impostor, ModTranslation.GetString("warlockCooldown"), 30f, 10f, 60f, 2.5f, warlockSpawnRate);
            warlockRootTime = CustomOption.Create(272, Types.Impostor, ModTranslation.GetString("warlockRootTime"), 5f, 0f, 15f, 1f, warlockSpawnRate);

            bountyHunterSpawnRate = CustomOption.Create(320, Types.Impostor, cs(BountyHunter.color, ModTranslation.GetString("BountyHunter")), rates, null, true);
            bountyHunterBountyDuration = CustomOption.Create(321, Types.Impostor, ModTranslation.GetString("bountyHunterBountyDuration"),  60f, 10f, 180f, 10f, bountyHunterSpawnRate);
            bountyHunterReducedCooldown = CustomOption.Create(322, Types.Impostor, ModTranslation.GetString("bountyHunterReducedCooldown"), 2.5f, 0f, 30f, 2.5f, bountyHunterSpawnRate);
            bountyHunterPunishmentTime = CustomOption.Create(323, Types.Impostor, ModTranslation.GetString("bountyHunterPunishmentTime"), 20f, 0f, 60f, 2.5f, bountyHunterSpawnRate);
            bountyHunterShowArrow = CustomOption.Create(324, Types.Impostor, ModTranslation.GetString("bountyHunterShowArrow"), true, bountyHunterSpawnRate);
            bountyHunterArrowUpdateIntervall = CustomOption.Create(325, Types.Impostor, ModTranslation.GetString("bountyHunterArrowUpdateIntervall"), 15f, 2.5f, 60f, 2.5f, bountyHunterShowArrow);
            bountyHunterShowCooldownForGhosts = CustomOption.Create(4399, Types.Impostor, ModTranslation.GetString("bountyHunterShowCooldownForGhosts"), true, bountyHunterSpawnRate);

                witchSpawnRate = CustomOption.Create(370, Types.Impostor, cs(Witch.color, ModTranslation.GetString("Witch")), rates, null, true);
            witchCooldown = CustomOption.Create(371, Types.Impostor, ModTranslation.GetString("witchCooldown"), 30f, 10f, 120f, 5f, witchSpawnRate);
            witchAdditionalCooldown = CustomOption.Create(372, Types.Impostor, ModTranslation.GetString("witchAdditionalCooldown"), 10f, 0f, 60f, 5f, witchSpawnRate);
            witchCanSpellAnyone = CustomOption.Create(373, Types.Impostor, ModTranslation.GetString("witchCanSpellAnyone"), false, witchSpawnRate);
            witchSpellCastingDuration = CustomOption.Create(374, Types.Impostor, ModTranslation.GetString("witchSpellCastingDuration"), 1f, 0f, 10f, 1f, witchSpawnRate);
            witchTriggerBothCooldowns = CustomOption.Create(375, Types.Impostor, ModTranslation.GetString("witchTriggerBothCooldowns"), true, witchSpawnRate);
            witchVoteSavesTargets = CustomOption.Create(376, Types.Impostor, ModTranslation.GetString("witchVoteSavesTargets"), true, witchSpawnRate);

            ninjaSpawnRate = CustomOption.Create(380, Types.Impostor, cs(Ninja.color, ModTranslation.GetString("Ninja")), rates, null, true);
            ninjaCooldown = CustomOption.Create(381, Types.Impostor, ModTranslation.GetString("ninjaCooldown"), 30f, 10f, 120f, 5f, ninjaSpawnRate);
            ninjaKnowsTargetLocation = CustomOption.Create(382, Types.Impostor, ModTranslation.GetString("ninjaKnowsTargetLocation"), true, ninjaSpawnRate);
            ninjaTraceTime = CustomOption.Create(383, Types.Impostor, ModTranslation.GetString("ninjaTraceTime"), 5f, 1f, 20f, 0.5f, ninjaSpawnRate);
            ninjaTraceColorTime = CustomOption.Create(384, Types.Impostor, ModTranslation.GetString("ninjaTraceColorTime"), 2f, 0f, 20f, 0.5f, ninjaSpawnRate);
            ninjaInvisibleDuration = CustomOption.Create(385, Types.Impostor, ModTranslation.GetString("ninjaInvisibleDuration"), 3f, 0f, 20f, 1f, ninjaSpawnRate);

            bomberSpawnRate = CustomOption.Create(460, Types.Impostor, cs(Bomber.color, ModTranslation.GetString("Bomber")), rates, null, true);
            bomberBombDestructionTime = CustomOption.Create(461, Types.Impostor, ModTranslation.GetString("bomberBombDestructionTime"), 20f, 2.5f, 120f, 2.5f, bomberSpawnRate);
            bomberBombDestructionRange = CustomOption.Create(462, Types.Impostor, ModTranslation.GetString("bomberBombDestructionRange"), 50f, 5f, 150f, 5f, bomberSpawnRate);
            bomberBombHearRange = CustomOption.Create(463, Types.Impostor, ModTranslation.GetString("bomberBombHearRange"), 60f, 5f, 150f, 5f, bomberSpawnRate);
            bomberDefuseDuration = CustomOption.Create(464, Types.Impostor, ModTranslation.GetString("bomberDefuseDuration"), 3f, 0.5f, 30f, 0.5f, bomberSpawnRate);
            bomberBombCooldown = CustomOption.Create(465, Types.Impostor, ModTranslation.GetString("bomberBombCooldown"), 15f, 2.5f, 30f, 2.5f, bomberSpawnRate);
            bomberBombActiveAfter = CustomOption.Create(466, Types.Impostor, ModTranslation.GetString("bomberBombActiveAfter"), 3f, 0.5f, 15f, 0.5f, bomberSpawnRate);


            yoyoSpawnRate = CustomOption.Create(470, Types.Impostor, cs(Yoyo.color, ModTranslation.GetString("Yoyo")), rates, null, true);
            yoyoBlinkDuration = CustomOption.Create(471, Types.Impostor, ModTranslation.GetString("yoyoBlinkDuration"), 20f, 2.5f, 120f, 2.5f, yoyoSpawnRate);
            yoyoMarkCooldown = CustomOption.Create(472, Types.Impostor, ModTranslation.GetString("yoyoMarkCooldown"), 20f, 2.5f, 120f, 2.5f, yoyoSpawnRate);
            yoyoMarkStaysOverMeeting = CustomOption.Create(473, Types.Impostor, ModTranslation.GetString("yoyoMarkStaysOverMeeting"), true, yoyoSpawnRate);
            yoyoHasAdminTable = CustomOption.Create(474, Types.Impostor, ModTranslation.GetString("yoyoHasAdminTable"), true, yoyoSpawnRate);
            yoyoAdminTableCooldown = CustomOption.Create(475, Types.Impostor, ModTranslation.GetString("yoyoAdminTableCooldown"), 20f, 2.5f, 120f, 2.5f, yoyoHasAdminTable);
            yoyoSilhouetteVisibility = CustomOption.Create(476, Types.Impostor, ModTranslation.GetString("yoyoSilhouetteVisibility"), new string[] { "0%", "10%", "20%", "30%", "40%", "50%" }, yoyoSpawnRate);


            guesserSpawnRate = CustomOption.Create(310, Types.Neutral, cs(Guesser.color, ModTranslation.GetString("modifierAssassin")), rates, null, true);
            guesserIsImpGuesserRate = CustomOption.Create(311, Types.Neutral, ModTranslation.GetString("guesserIsImpGuesserRate"), rates, guesserSpawnRate);
            guesserNumberOfShots = CustomOption.Create(312, Types.Neutral, ModTranslation.GetString("modifierAssassinNumberOfShots"), 2f, 1f, 15f, 1f, guesserSpawnRate);
            guesserHasMultipleShotsPerMeeting = CustomOption.Create(313, Types.Neutral, ModTranslation.GetString("modifierAssassinMultipleShotsPerMeeting"), false, guesserSpawnRate);
            guesserKillsThroughShield  = CustomOption.Create(315, Types.Neutral, ModTranslation.GetString("modifierAssassinKillsThroughShield"), true, guesserSpawnRate);
            guesserEvilCanKillSpy  = CustomOption.Create(316, Types.Neutral, ModTranslation.GetString("guesserEvilCanKillSpy"), true, guesserSpawnRate);
            guesserSpawnBothRate = CustomOption.Create(317, Types.Neutral, ModTranslation.GetString("guesserSpawnBothRate"), rates, guesserSpawnRate);
            guesserCantGuessSnitchIfTaksDone = CustomOption.Create(318, Types.Neutral, ModTranslation.GetString("guesserCantGuessSnitchIfTaksDone"), true, guesserSpawnRate);

            jesterSpawnRate = CustomOption.Create(60, Types.Neutral, cs(Jester.color, ModTranslation.GetString("Jester")), rates, null, true);
            jesterCanCallEmergency = CustomOption.Create(61, Types.Neutral, ModTranslation.GetString("jesterCanCallEmergency"), true, jesterSpawnRate);
            jesterHasImpostorVision = CustomOption.Create(62, Types.Neutral, ModTranslation.GetString("jesterHasImpostorVision"), false, jesterSpawnRate);

            arsonistSpawnRate = CustomOption.Create(290, Types.Neutral, cs(Arsonist.color, ModTranslation.GetString("Arsonist")), rates, null, true);
            arsonistCooldown = CustomOption.Create(291, Types.Neutral, ModTranslation.GetString("arsonistCooldown"), 12.5f, 2.5f, 60f, 2.5f, arsonistSpawnRate);
            arsonistDuration = CustomOption.Create(292, Types.Neutral, ModTranslation.GetString("arsonistDuration"), 3f, 1f, 10f, 1f, arsonistSpawnRate);

            jackalSpawnRate = CustomOption.Create(220, Types.Neutral, cs(Jackal.color, ModTranslation.GetString("Jackal")), rates, null, true);
            jackalKillCooldown = CustomOption.Create(221, Types.Neutral, ModTranslation.GetString("jackalKillCooldown"), 30f, 10f, 60f, 2.5f, jackalSpawnRate);
            jackalCreateSidekickCooldown = CustomOption.Create(222, Types.Neutral, ModTranslation.GetString("jackalCreateSidekickCooldown"), 30f, 10f, 60f, 2.5f, jackalSpawnRate);
            jackalCanUseVents = CustomOption.Create(223, Types.Neutral, ModTranslation.GetString("jackalCanUseVents"), true, jackalSpawnRate);
            jackalCanSabotageLights = CustomOption.Create(431, Types.Neutral, ModTranslation.GetString("jackalCanSabotageLights"), true, jackalSpawnRate);
            jackalCanCreateSidekick = CustomOption.Create(224, Types.Neutral, ModTranslation.GetString("jackalCanCreateSidekick"), false, jackalSpawnRate);
            sidekickPromotesToJackal = CustomOption.Create(225, Types.Neutral, ModTranslation.GetString("sidekickPromotesToJackal"), false, jackalCanCreateSidekick);
            sidekickCanKill = CustomOption.Create(226, Types.Neutral, ModTranslation.GetString("sidekickCanKill"), false, jackalCanCreateSidekick);
            sidekickCanUseVents = CustomOption.Create(227, Types.Neutral, ModTranslation.GetString("sidekickCanUseVents"), true, jackalCanCreateSidekick);
            sidekickCanSabotageLights = CustomOption.Create(432, Types.Neutral, ModTranslation.GetString("sidekickCanSabotageLights"), true, jackalCanCreateSidekick);
            jackalPromotedFromSidekickCanCreateSidekick = CustomOption.Create(228, Types.Neutral, ModTranslation.GetString("jackalPromotedFromSidekickCanCreateSidekick"), true, sidekickPromotesToJackal);
            jackalCanCreateSidekickFromImpostor = CustomOption.Create(229, Types.Neutral, ModTranslation.GetString("jackalCanCreateSidekickFromImpostor"), true, jackalCanCreateSidekick);
            jackalAndSidekickHaveImpostorVision = CustomOption.Create(430, Types.Neutral, ModTranslation.GetString("jackalAndSidekickHaveImpostorVision"), false, jackalSpawnRate);

            vultureSpawnRate = CustomOption.Create(340, Types.Neutral, cs(Vulture.color, ModTranslation.GetString("Vulture")), rates, null, true);
            vultureCooldown = CustomOption.Create(341, Types.Neutral, ModTranslation.GetString("vultureCooldown"), 15f, 10f, 60f, 2.5f, vultureSpawnRate);
            vultureNumberToWin = CustomOption.Create(342, Types.Neutral, ModTranslation.GetString("vultureNumberToWin"), 4f, 1f, 10f, 1f, vultureSpawnRate);
            vultureCanUseVents = CustomOption.Create(343, Types.Neutral, ModTranslation.GetString("vultureCanUseVents"), true, vultureSpawnRate);
            vultureShowArrows = CustomOption.Create(344, Types.Neutral, ModTranslation.GetString("vultureShowArrows"), true, vultureSpawnRate);

            lawyerSpawnRate = CustomOption.Create(350, Types.Neutral, cs(Lawyer.color, ModTranslation.GetString("Lawyer")), rates, null, true);
            lawyerIsProsecutorChance = CustomOption.Create(358, Types.Neutral, ModTranslation.GetString("lawyerIsProsecutorChance"), rates, lawyerSpawnRate);
            lawyerVision = CustomOption.Create(354, Types.Neutral, ModTranslation.GetString("lawyerVision"), 1f, 0.25f, 3f, 0.25f, lawyerSpawnRate);
            lawyerKnowsRole = CustomOption.Create(355, Types.Neutral, ModTranslation.GetString("lawyerKnowsRole"), false, lawyerSpawnRate);
            lawyerCanCallEmergency = CustomOption.Create(352, Types.Neutral, ModTranslation.GetString("lawyerCanCallEmergency"), true, lawyerSpawnRate);
            lawyerTargetCanBeJester = CustomOption.Create(351, Types.Neutral, ModTranslation.GetString("lawyerTargetCanBeJester"), false, lawyerSpawnRate);
            pursuerCooldown = CustomOption.Create(356, Types.Neutral, ModTranslation.GetString("pursuerCooldown"), 30f, 5f, 60f, 2.5f, lawyerSpawnRate);
            pursuerBlanksNumber = CustomOption.Create(357, Types.Neutral, ModTranslation.GetString("pursuerBlanksNumber"), 5f, 1f, 20f, 1f, lawyerSpawnRate);

            mayorSpawnRate = CustomOption.Create(80, Types.Crewmate, cs(Mayor.color, ModTranslation.GetString("Mayor")), rates, null, true);
            mayorCanSeeVoteColors = CustomOption.Create(81, Types.Crewmate, ModTranslation.GetString("mayorCanSeeVoteColors"), false, mayorSpawnRate);
            mayorTasksNeededToSeeVoteColors = CustomOption.Create(82, Types.Crewmate, ModTranslation.GetString("mayorTasksNeededToSeeVoteColors"), 5f, 0f, 20f, 1f, mayorCanSeeVoteColors);
            mayorMeetingButton = CustomOption.Create(83, Types.Crewmate, ModTranslation.GetString("mayorMeetingButton"), true, mayorSpawnRate);
            mayorMaxRemoteMeetings = CustomOption.Create(84, Types.Crewmate, ModTranslation.GetString("mayorMaxRemoteMeetings"), 1f, 1f, 5f, 1f, mayorMeetingButton);
            mayorChooseSingleVote = CustomOption.Create(85, Types.Crewmate, ModTranslation.GetString("mayorChooseSingleVote"), new string[] { ModTranslation.GetString("optionOff"), ModTranslation.GetString("OnBoforeVoting"), ModTranslation.GetString("OnUntilMeetingEnds") }, mayorSpawnRate);

            engineerSpawnRate = CustomOption.Create(90, Types.Crewmate, cs(Engineer.color, ModTranslation.GetString("Engineer")), rates, null, true);
            engineerNumberOfFixes = CustomOption.Create(91, Types.Crewmate, ModTranslation.GetString("engineerNumberOfFixes"), 1f, 1f, 3f, 1f, engineerSpawnRate);
            engineerHighlightForImpostors = CustomOption.Create(92, Types.Crewmate, ModTranslation.GetString("engineerHighlightForImpostors"), true, engineerSpawnRate);
            engineerHighlightForTeamJackal = CustomOption.Create(93, Types.Crewmate, ModTranslation.GetString("engineerHighlightForTeamJackal"), true, engineerSpawnRate);

            sheriffSpawnRate = CustomOption.Create(100, Types.Crewmate, cs(Sheriff.color, ModTranslation.GetString("Sheriff")), rates, null, true);
            sheriffCooldown = CustomOption.Create(101, Types.Crewmate, ModTranslation.GetString("sheriffCooldown"), 30f, 10f, 60f, 2.5f, sheriffSpawnRate);
            sheriffCanKillNeutrals = CustomOption.Create(102, Types.Crewmate, ModTranslation.GetString("sheriffCanKillNeutrals"), false, sheriffSpawnRate);
            deputySpawnRate = CustomOption.Create(103, Types.Crewmate, ModTranslation.GetString("Deputy"), rates, sheriffSpawnRate);
          
            deputyNumberOfHandcuffs = CustomOption.Create(104, Types.Crewmate, ModTranslation.GetString("deputyNumberOfHandcuffs"), 3f, 1f, 10f, 1f, deputySpawnRate);
            deputyHandcuffCooldown = CustomOption.Create(105, Types.Crewmate, ModTranslation.GetString("deputyHandcuffCooldown"), 30f, 10f, 60f, 2.5f, deputySpawnRate);
            deputyHandcuffDuration = CustomOption.Create(106, Types.Crewmate, ModTranslation.GetString("deputyHandcuffDuration"), 15f, 5f, 60f, 2.5f, deputySpawnRate);
            deputyKnowsSheriff = CustomOption.Create(107, Types.Crewmate, ModTranslation.GetString("deputyKnowsSheriff"), true, deputySpawnRate);
            deputyGetsPromoted = CustomOption.Create(108, Types.Crewmate, ModTranslation.GetString("deputyGetsPromoted"), new string[] { ModTranslation.GetString("optionOff"), ModTranslation.GetString("OnImmediately"), ModTranslation.GetString("OnAfterMeeting") }, deputySpawnRate);
            deputyKeepsHandcuffs = CustomOption.Create(109, Types.Crewmate, ModTranslation.GetString("deputyKeepsHandcuffs"), true, deputyGetsPromoted);

            lighterSpawnRate = CustomOption.Create(110, Types.Crewmate, cs(Lighter.color, ModTranslation.GetString("Lighter")), rates, null, true);
            lighterModeLightsOnVision = CustomOption.Create(111, Types.Crewmate, ModTranslation.GetString("lighterModeLightsOnVision"), 1.5f, 0.25f, 5f, 0.25f, lighterSpawnRate);
            lighterModeLightsOffVision = CustomOption.Create(112, Types.Crewmate, ModTranslation.GetString("lighterModeLightsOffVision"), 0.5f, 0.25f, 5f, 0.25f, lighterSpawnRate);
            lighterFlashlightWidth = CustomOption.Create(113, Types.Crewmate, ModTranslation.GetString("lighterFlashlightWidth"), 0.3f, 0.1f, 1f, 0.1f, lighterSpawnRate);

            detectiveSpawnRate = CustomOption.Create(120, Types.Crewmate, cs(Detective.color, ModTranslation.GetString("Detective")), rates, null, true);
            detectiveAnonymousFootprints = CustomOption.Create(121, Types.Crewmate, ModTranslation.GetString("detectiveAnonymousFootprints"), false, detectiveSpawnRate);
            detectiveFootprintIntervall = CustomOption.Create(122, Types.Crewmate, ModTranslation.GetString("detectiveFootprintIntervall"), 0.5f, 0.25f, 10f, 0.25f, detectiveSpawnRate);
            detectiveFootprintDuration = CustomOption.Create(123, Types.Crewmate, ModTranslation.GetString("detectiveFootprintDuration"), 5f, 0.25f, 10f, 0.25f, detectiveSpawnRate);
            detectiveReportNameDuration = CustomOption.Create(124, Types.Crewmate, ModTranslation.GetString("detectiveReportNameDuration"), 0, 0, 60, 2.5f, detectiveSpawnRate);
            detectiveReportColorDuration = CustomOption.Create(125, Types.Crewmate, ModTranslation.GetString("detectiveReportColorDuration"), 20, 0, 120, 2.5f, detectiveSpawnRate);

            timeMasterSpawnRate = CustomOption.Create(130, Types.Crewmate, cs(TimeMaster.color, ModTranslation.GetString("TimeMaster")), rates, null, true);
            timeMasterCooldown = CustomOption.Create(131, Types.Crewmate, ModTranslation.GetString("timeMasterCooldown"), 30f, 10f, 120f, 2.5f, timeMasterSpawnRate);
            timeMasterRewindTime = CustomOption.Create(132, Types.Crewmate, ModTranslation.GetString("timeMasterRewindTime"), 3f, 1f, 10f, 1f, timeMasterSpawnRate);
            timeMasterShieldDuration = CustomOption.Create(133, Types.Crewmate, ModTranslation.GetString("timeMasterShieldDuration"), 3f, 1f, 20f, 1f, timeMasterSpawnRate);

            medicSpawnRate = CustomOption.Create(140, Types.Crewmate, cs(Medic.color, ModTranslation.GetString("Medic")), rates, null, true);
            medicShowShielded = CustomOption.Create(143, Types.Crewmate, ModTranslation.GetString("medicShowShielded"), new string[] { ModTranslation.GetString("evone"), ModTranslation.GetString("evtwo"), ModTranslation.GetString("jstone") }, medicSpawnRate);
            medicShowAttemptToShielded = CustomOption.Create(144, Types.Crewmate, ModTranslation.GetString("medicShowAttemptToShielded"), false, medicSpawnRate);
            medicSetOrShowShieldAfterMeeting = CustomOption.Create(145, Types.Crewmate, ModTranslation.GetString("medicSetOrShowShieldAfterMeeting"), new string[] { ModTranslation.GetString("OnImmediately"), ModTranslation.GetString("OnImmediately" + "OnAfterMeeting") , ModTranslation.GetString("OnAfterMeeting") }, medicSpawnRate);
            medicShowAttemptToMedic = CustomOption.Create(146, Types.Crewmate, ModTranslation.GetString("medicShowAttemptToMedic"), false, medicSpawnRate);

            swapperSpawnRate = CustomOption.Create(150, Types.Crewmate, cs(Swapper.color, ModTranslation.GetString("Swapper")), rates, null, true);
            swapperCanCallEmergency = CustomOption.Create(151, Types.Crewmate, ModTranslation.GetString("swapperCanCallEmergency"), false, swapperSpawnRate);
            swapperCanOnlySwapOthers = CustomOption.Create(152, Types.Crewmate, ModTranslation.GetString("swapperCanOnlySwapOthers"), false, swapperSpawnRate);
            swapperSwapsNumber = CustomOption.Create(153, Types.Crewmate, ModTranslation.GetString("swapperSwapsNumber"), 1f, 0f, 5f, 1f, swapperSpawnRate);
            swapperRechargeTasksNumber = CustomOption.Create(154, Types.Crewmate, ModTranslation.GetString("swapperRechargeTasksNumber"), 2f, 1f, 10f, 1f, swapperSpawnRate);

            seerSpawnRate = CustomOption.Create(160, Types.Crewmate, cs(Seer.color, ModTranslation.GetString("Seer")), rates, null, true);
            seerMode = CustomOption.Create(161, Types.Crewmate, ModTranslation.GetString("seerMode"), new string[] { ModTranslation.GetString("Mode1"), ModTranslation.GetString("Mode2"), ModTranslation.GetString("Mode3") }, seerSpawnRate);
            seerLimitSoulDuration = CustomOption.Create(163, Types.Crewmate, ModTranslation.GetString("seerLimitSoulDuration"), false, seerSpawnRate);
            seerSoulDuration = CustomOption.Create(162, Types.Crewmate, ModTranslation.GetString("seerSoulDuration"), 15f, 0f, 120f, 5f, seerLimitSoulDuration);

            hackerSpawnRate = CustomOption.Create(170, Types.Crewmate, cs(Hacker.color, ModTranslation.GetString("Hacker")), rates, null, true);
            hackerCooldown = CustomOption.Create(171, Types.Crewmate, ModTranslation.GetString("hackerCooldown"), 30f, 5f, 60f, 5f, hackerSpawnRate);
            hackerHackeringDuration = CustomOption.Create(172, Types.Crewmate, ModTranslation.GetString("hackerHackeringDuration"), 10f, 2.5f, 60f, 2.5f, hackerSpawnRate);
            hackerOnlyColorType = CustomOption.Create(173, Types.Crewmate, ModTranslation.GetString("hackerOnlyColorType"), false, hackerSpawnRate);
            hackerToolsNumber = CustomOption.Create(174, Types.Crewmate, ModTranslation.GetString("hackerToolsNumber"), 5f, 1f, 30f, 1f, hackerSpawnRate);
            hackerRechargeTasksNumber = CustomOption.Create(175, Types.Crewmate, ModTranslation.GetString("hackerRechargeTasksNumber"), 2f, 1f, 5f, 1f, hackerSpawnRate);
            hackerNoMove = CustomOption.Create(176, Types.Crewmate, ModTranslation.GetString("hackerNoMove"), true, hackerSpawnRate);

            trackerSpawnRate = CustomOption.Create(200, Types.Crewmate, cs(Tracker.color, ModTranslation.GetString("Tracker")), rates, null, true);

            trackerUpdateIntervall = CustomOption.Create(201, Types.Crewmate, ModTranslation.GetString("trackerUpdateIntervall"), 5f, 1f, 30f, 1f, trackerSpawnRate);
            trackerResetTargetAfterMeeting = CustomOption.Create(202, Types.Crewmate, ModTranslation.GetString("trackerResetTargetAfterMeeting"), false, trackerSpawnRate);
            trackerCanTrackCorpses = CustomOption.Create(203, Types.Crewmate, ModTranslation.GetString("trackerCanTrackCorpses"), true, trackerSpawnRate);
            trackerCorpsesTrackingCooldown = CustomOption.Create(204, Types.Crewmate, ModTranslation.GetString("trackerCorpsesTrackingCooldown"), 30f, 5f, 120f, 5f, trackerCanTrackCorpses);
            trackerCorpsesTrackingDuration = CustomOption.Create(205, Types.Crewmate, ModTranslation.GetString("trackerCorpsesTrackingDuration"), 5f, 2.5f, 30f, 2.5f, trackerCanTrackCorpses);
            trackerTrackingMethod = CustomOption.Create(206, Types.Crewmate, ModTranslation.GetString("trackerTrackingMethod"), new string[] { ModTranslation.GetString("Method1"), ModTranslation.GetString("Method2"), ModTranslation.GetString("Method3") }, trackerSpawnRate);

            snitchSpawnRate = CustomOption.Create(210, Types.Crewmate, cs(Snitch.color, ModTranslation.GetString("Snitch")), rates, null, true);
            snitchLeftTasksForReveal = CustomOption.Create(219, Types.Crewmate, ModTranslation.GetString("snitchLeftTasksForReveal"), 5f, 0f, 25f, 1f, snitchSpawnRate);
            snitchMode = CustomOption.Create(211, Types.Crewmate, ModTranslation.GetString("snitchMode"), new string[] { ModTranslation.GetString("Mode1"), ModTranslation.GetString("Mode2"), ModTranslation.GetString("Mode3") }, snitchSpawnRate);
            snitchTargets = CustomOption.Create(212, Types.Crewmate, ModTranslation.GetString("snitchTargets"), new string[] { ModTranslation.GetString("Target1"), ModTranslation.GetString("Target2") }, snitchSpawnRate);

            spySpawnRate = CustomOption.Create(240, Types.Crewmate, cs(Spy.color, ModTranslation.GetString("Spy")), rates, null, true);
            spyCanDieToSheriff = CustomOption.Create(241, Types.Crewmate, ModTranslation.GetString("spyCanDieToSheriff"), false, spySpawnRate);
            spyImpostorsCanKillAnyone = CustomOption.Create(242, Types.Crewmate, ModTranslation.GetString("spyImpostorsCanKillAnyone"), true, spySpawnRate);
            spyCanEnterVents = CustomOption.Create(243, Types.Crewmate, ModTranslation.GetString("spyCanEnterVents"), false, spySpawnRate);
            spyHasImpostorVision = CustomOption.Create(244, Types.Crewmate, ModTranslation.GetString("spyHasImpostorVision"), false, spySpawnRate);

            portalmakerSpawnRate = CustomOption.Create(390, Types.Crewmate, cs(Portalmaker.color, ModTranslation.GetString("Portalmaker")), rates, null, true);
            portalmakerCooldown = CustomOption.Create(391, Types.Crewmate, ModTranslation.GetString("portalmakerCooldown"), 30f, 10f, 60f, 2.5f, portalmakerSpawnRate);
            portalmakerUsePortalCooldown = CustomOption.Create(392, Types.Crewmate, ModTranslation.GetString("portalmakerUsePortalCooldown"), 30f, 10f, 60f, 2.5f, portalmakerSpawnRate);
            portalmakerLogOnlyColorType = CustomOption.Create(393, Types.Crewmate, ModTranslation.GetString("portalmakerLogOnlyColorType"), true, portalmakerSpawnRate);
            portalmakerLogHasTime = CustomOption.Create(394, Types.Crewmate, ModTranslation.GetString("portalmakerLogHasTime"), true, portalmakerSpawnRate);
            portalmakerCanPortalFromAnywhere = CustomOption.Create(395, Types.Crewmate, ModTranslation.GetString("portalmakerCanPortalFromAnywhere"), true, portalmakerSpawnRate);

            securityGuardSpawnRate = CustomOption.Create(280, Types.Crewmate, cs(SecurityGuard.color, ModTranslation.GetString("SecurityGuard")), rates, null, true);
            securityGuardCooldown = CustomOption.Create(281, Types.Crewmate, ModTranslation.GetString("securityGuardCooldown"), 30f, 10f, 60f, 2.5f, securityGuardSpawnRate);
            securityGuardTotalScrews = CustomOption.Create(282, Types.Crewmate, ModTranslation.GetString("securityGuardTotalScrews"), 7f, 1f, 15f, 1f, securityGuardSpawnRate);
            securityGuardCamPrice = CustomOption.Create(283, Types.Crewmate, ModTranslation.GetString("securityGuardCamPrice"), 2f, 1f, 15f, 1f, securityGuardSpawnRate);
            securityGuardVentPrice = CustomOption.Create(284, Types.Crewmate, ModTranslation.GetString("securityGuardVentPrice"), 1f, 1f, 15f, 1f, securityGuardSpawnRate);
            securityGuardCamDuration = CustomOption.Create(285, Types.Crewmate, ModTranslation.GetString("securityGuardCamDuration"), 10f, 2.5f, 60f, 2.5f, securityGuardSpawnRate);
            securityGuardCamMaxCharges = CustomOption.Create(286, Types.Crewmate, ModTranslation.GetString("securityGuardCamMaxCharges"), 5f, 1f, 30f, 1f, securityGuardSpawnRate);
            securityGuardCamRechargeTasksNumber = CustomOption.Create(287, Types.Crewmate, ModTranslation.GetString("securityGuardCamRechargeTasksNumber"), 3f, 1f, 10f, 1f, securityGuardSpawnRate);
            securityGuardNoMove = CustomOption.Create(288, Types.Crewmate, ModTranslation.GetString("securityGuardNoMove"), true, securityGuardSpawnRate);

            mediumSpawnRate = CustomOption.Create(360, Types.Crewmate, cs(Medium.color, ModTranslation.GetString("Medium")), rates, null, true);
            mediumCooldown = CustomOption.Create(361, Types.Crewmate, ModTranslation.GetString("mediumCooldown"), 30f, 5f, 120f, 5f, mediumSpawnRate);
            mediumDuration = CustomOption.Create(362, Types.Crewmate, ModTranslation.GetString("mediumDuration"), 3f, 0f, 15f, 1f, mediumSpawnRate);
            mediumOneTimeUse = CustomOption.Create(363, Types.Crewmate, ModTranslation.GetString("mediumOneTimeUse"), false, mediumSpawnRate);
            mediumChanceAdditionalInfo = CustomOption.Create(364, Types.Crewmate, ModTranslation.GetString("mediumChanceAdditionalInfo"), rates, mediumSpawnRate);

            prophetSpawnRate = CustomOption.Create(111222333, Types.Crewmate, cs(Prophet.color, ModTranslation.GetString("Prophet")), rates, null, true);
            prophetCooldown = CustomOption.Create(222333444, Types.Crewmate, ModTranslation.GetString("prophetCooldown"), 30f, 5f, 120f, 5f, prophetSpawnRate);
            prophetNumExamines = CustomOption.Create(333444555, Types.Crewmate, ModTranslation.GetString("prophetNumExamines"), 7f, 1f, 15f, 1f, prophetSpawnRate);
            prophetKillCrewAsRed = CustomOption.Create(444555666, Types.Crewmate, ModTranslation.GetString("prophetKillCrewAsRed"), false, prophetSpawnRate);
            NeutralAsRed = CustomOption.Create(555666777, Types.Crewmate, ModTranslation.GetString("NeutralAsRed"), false, prophetSpawnRate);
            prophetCanCallEmergency = CustomOption.Create(666777888, Types.Crewmate, ModTranslation.GetString("prophetCanCallEmergency"), true, prophetSpawnRate);

            thiefSpawnRate = CustomOption.Create(400, Types.Neutral, cs(Thief.color, ModTranslation.GetString("Thief")), rates, null, true);
            thiefCooldown = CustomOption.Create(401, Types.Neutral, ModTranslation.GetString("thiefCooldown"), 30f, 5f, 120f, 5f, thiefSpawnRate);
            thiefCanKillSheriff = CustomOption.Create(402, Types.Neutral, ModTranslation.GetString("thiefCanKillSheriff"), true, thiefSpawnRate);
            thiefHasImpVision = CustomOption.Create(403, Types.Neutral, ModTranslation.GetString("thiefHasImpVision"), true, thiefSpawnRate);
            thiefCanUseVents = CustomOption.Create(404, Types.Neutral, ModTranslation.GetString("thiefCanUseVents"), true, thiefSpawnRate);
            thiefCanStealWithGuess = CustomOption.Create(405, Types.Neutral, ModTranslation.GetString("thiefCanStealWithGuess"), false, thiefSpawnRate);

            trapperSpawnRate = CustomOption.Create(410, Types.Crewmate, cs(Trapper.color, ModTranslation.GetString("Trapper")), rates, null, true);
            trapperCooldown = CustomOption.Create(420, Types.Crewmate, ModTranslation.GetString("trapperCooldown"), 30f, 5f, 120f, 5f, trapperSpawnRate);
            trapperMaxCharges = CustomOption.Create(440, Types.Crewmate, ModTranslation.GetString("trapperMaxCharges"), 5f, 1f, 15f, 1f, trapperSpawnRate);
            trapperRechargeTasksNumber = CustomOption.Create(450, Types.Crewmate, ModTranslation.GetString("trapperRechargeTasksNumber"), 2f, 1f, 15f, 1f, trapperSpawnRate);
            trapperTrapNeededTriggerToReveal = CustomOption.Create(451, Types.Crewmate, ModTranslation.GetString("trapperTrapNeededTriggerToReveal"), 3f, 2f, 10f, 1f, trapperSpawnRate);
            trapperAnonymousMap = CustomOption.Create(452, Types.Crewmate, ModTranslation.GetString("trapperAnonymousMap"), false, trapperSpawnRate);
            trapperInfoType = CustomOption.Create(453, Types.Crewmate, ModTranslation.GetString("trapperInfoType"), new string[] { ModTranslation.GetString("Type1"), ModTranslation.GetString("Type2"), ModTranslation.GetString("Type3") }, trapperSpawnRate);
            trapperTrapDuration = CustomOption.Create(454, Types.Crewmate, ModTranslation.GetString("trapperTrapDuration"), 5f, 1f, 15f, 1f, trapperSpawnRate);

            modifiersAreHidden = CustomOption.Create(1009, Types.Modifier, cs(Color.yellow, ModTranslation.GetString("modifiersAreHidden")), true, null, true, heading: cs(Color.yellow, ModTranslation.GetString("ModifiersSettings")));

            modifierBloody = CustomOption.Create(1000, Types.Modifier, cs(Color.yellow, ModTranslation.GetString("Bloody")), rates, null, true);
            modifierBloodyQuantity = CustomOption.Create(1001, Types.Modifier, ModTranslation.GetString("modifierBloodyQuantity"), ratesModifier, modifierBloody);
            modifierBloodyDuration = CustomOption.Create(1002, Types.Modifier, ModTranslation.GetString("modifierBloodyDuration"), 10f, 3f, 60f, 1f, modifierBloody);

            modifierAntiTeleport = CustomOption.Create(1010, Types.Modifier, cs(Color.yellow, ModTranslation.GetString("AntiTeleport")), rates, null, true);
            modifierAntiTeleportQuantity = CustomOption.Create(1011, Types.Modifier, ModTranslation.GetString("modifierAntiTeleportQuantity"), ratesModifier, modifierAntiTeleport);

            modifierTieBreaker = CustomOption.Create(1020, Types.Modifier, cs(Color.yellow, ModTranslation.GetString("TieBreaker")), rates, null, true);

            modifierBait = CustomOption.Create(1030, Types.Modifier, cs(Color.yellow, ModTranslation.GetString("Bait")), rates, null, true);
            modifierBaitQuantity = CustomOption.Create(1031, Types.Modifier, ModTranslation.GetString("modifierBaitQuantity"), ratesModifier, modifierBait);
            modifierBaitReportDelayMin = CustomOption.Create(1032, Types.Modifier, ModTranslation.GetString("modifierBaitReportDelayMin"), 0f, 0f, 10f, 1f, modifierBait);
            modifierBaitReportDelayMax = CustomOption.Create(1033, Types.Modifier, ModTranslation.GetString("modifierBaitReportDelayMax"), 0f, 0f, 10f, 1f, modifierBait);
            modifierBaitShowKillFlash = CustomOption.Create(1034, Types.Modifier, ModTranslation.GetString("modifierBaitShowKillFlash"), true, modifierBait);

            modifierLover = CustomOption.Create(1040, Types.Modifier, cs(Color.yellow, ModTranslation.GetString("Lover")), rates, null, true);
            modifierLoverImpLoverRate = CustomOption.Create(1041, Types.Modifier, ModTranslation.GetString("modifierLoverImpLoverRate"), rates, modifierLover);
            modifierLoverBothDie = CustomOption.Create(1042, Types.Modifier, ModTranslation.GetString("modifierLoverBothDie"), true, modifierLover);
            modifierLoverEnableChat = CustomOption.Create(1043, Types.Modifier, ModTranslation.GetString("modifierLoverEnableChat"), true, modifierLover);

            modifierSunglasses = CustomOption.Create(1050, Types.Modifier, cs(Color.yellow, ModTranslation.GetString("Sunglasses")), rates, null, true);
            modifierSunglassesQuantity = CustomOption.Create(1051, Types.Modifier, ModTranslation.GetString("modifierSunglassesQuantity"), ratesModifier, modifierSunglasses);
            modifierSunglassesVision = CustomOption.Create(1052, Types.Modifier, ModTranslation.GetString("modifierSunglassesVision"), new string[] { "-10%", "-20%", "-30%", "-40%", "-50%" }, modifierSunglasses);

            modifierLighterln = CustomOption.Create(40180, Types.Modifier, cs(Color.yellow, ModTranslation.GetString("Lighterln")), rates, null, true);

            modifierMini = CustomOption.Create(1061, Types.Modifier, cs(Color.yellow, ModTranslation.GetString("Mini")), rates, null, true);
            modifierMiniGrowingUpDuration = CustomOption.Create(1062, Types.Modifier, ModTranslation.GetString("modifierMiniGrowingUpDuration"), 400f, 100f, 1500f, 100f, modifierMini);
            modifierMiniGrowingUpInMeeting = CustomOption.Create(1063, Types.Modifier, ModTranslation.GetString("modifierMiniGrowingUpInMeeting"), true, modifierMini);

            modifierVip = CustomOption.Create(1070, Types.Modifier, cs(Color.yellow, ModTranslation.GetString("Vip")), rates, null, true);
            modifierVipQuantity = CustomOption.Create(1071, Types.Modifier, ModTranslation.GetString("modifierVipQuantity"), ratesModifier, modifierVip);
            modifierVipShowColor = CustomOption.Create(1072, Types.Modifier, ModTranslation.GetString("modifierVipShowColor"), true, modifierVip);

            modifierInvert = CustomOption.Create(1080, Types.Modifier, cs(Color.yellow, ModTranslation.GetString("Invert")), rates, null, true);
            modifierInvertQuantity = CustomOption.Create(1081, Types.Modifier, ModTranslation.GetString("modifierInvertQuantity"), ratesModifier, modifierInvert);
            modifierInvertDuration = CustomOption.Create(1082, Types.Modifier, ModTranslation.GetString("modifierInvertDuration"), 3f, 1f, 15f, 1f, modifierInvert);

            modifierChameleon = CustomOption.Create(1090, Types.Modifier, cs(Color.yellow, ModTranslation.GetString("Chameleon")), rates, null, true);
            modifierChameleonQuantity = CustomOption.Create(1091, Types.Modifier, ModTranslation.GetString("modifierChameleonQuantity"), ratesModifier, modifierChameleon);
            modifierChameleonHoldDuration = CustomOption.Create(1092, Types.Modifier, ModTranslation.GetString("modifierChameleonHoldDuration"), 3f, 1f, 10f, 0.5f, modifierChameleon);
            modifierChameleonFadeDuration = CustomOption.Create(1093, Types.Modifier, ModTranslation.GetString("modifierChameleonFadeDuration"), 1f, 0.25f, 10f, 0.25f, modifierChameleon);
            modifierChameleonMinVisibility = CustomOption.Create(1094, Types.Modifier, ModTranslation.GetString("modifierChameleonMinVisibility"), new string[] { "0%", "10%", "20%", "30%", "40%", "50%" }, modifierChameleon);

            modifierShifter = CustomOption.Create(1100, Types.Modifier, cs(Color.yellow, ModTranslation.GetString("Shifter")), rates, null, true);


            // Guesser Gamemode (2000 - 2999)
            guesserGamemodeCrewNumber = CustomOption.Create(2001, Types.Guesser, cs(Guesser.color, "guesserGamemodeCrewNumber".Translate()), 15f, 1f, 15f, 1f, null, true, heading: "guesserGamemodeCrewNumber".Translate());
            guesserGamemodeNeutralNumber = CustomOption.Create(2002, Types.Guesser, cs(Guesser.color, "guesserGamemodeNeutralNumber".Translate()), 15f, 1f, 15f, 1f, null);
            guesserGamemodeImpNumber = CustomOption.Create(2003, Types.Guesser, cs(Guesser.color, "guesserGamemodeImpNumber".Translate()), 15f, 1f, 15f, 1f, null);
            guesserForceJackalGuesser = CustomOption.Create(2007, Types.Guesser, "guesserForceJackalGuesser".Translate(), false, null, true, heading: "guesserForceJackalGuesser".Translate());
            guesserGamemodeSidekickIsAlwaysGuesser = CustomOption.Create(2012, Types.Guesser, "guesserGamemodeSidekickIsAlwaysGuesser".Translate(), false, null);
            guesserForceThiefGuesser = CustomOption.Create(2011, Types.Guesser, "guesserForceThiefGuesser".Translate(), false, null);
            guesserGamemodeHaveModifier = CustomOption.Create(2004, Types.Guesser, "guesserGamemodeHaveModifier".Translate(), true, null, true, heading: "GuesserModeClassicSettings".Translate());
            guesserGamemodeNumberOfShots = CustomOption.Create(2005, Types.Guesser, "guesserGamemodeNumberOfShots".Translate(), 3f, 1f, 15f, 1f, null);
            guesserGamemodeHasMultipleShotsPerMeeting = CustomOption.Create(2006, Types.Guesser, "guesserGamemodeHasMultipleShotsPerMeeting".Translate(), false, null);
            //guesserGamemodeCrewGuesserNumberOfTasks = CustomOption.Create(2013, Types.Guesser, "��Ա�Ĺֽ����²⼼���������������", 0f, 0f, 15f, 1f, null);
            guesserGamemodeKillsThroughShield = CustomOption.Create(2008, Types.Guesser, "guesserGamemodeKillsThroughShield".Translate(), true, null);
            guesserGamemodeEvilCanKillSpy = CustomOption.Create(2009, Types.Guesser, "guesserGamemodeEvilCanKillSpy".Translate(), true, null);
            guesserGamemodeCantGuessSnitchIfTaksDone = CustomOption.Create(2010, Types.Guesser, "guesserGamemodeCantGuessSnitchIfTaksDone".Translate(), true, null);


            // Hide N Seek Gamemode
            hideNSeekMap = CustomOption.Create(3020, Types.HideNSeekMain, cs(Color.yellow, ModTranslation.GetString("hideNSeekMap")), new string[] { "The Skeld", "Mira", "Polus", "Airship", "Fungle", "Submerged", "LI Map" }, null, true, onChange: () => { int map = hideNSeekMap.selection; if (map >= 3) map++; GameOptionsManager.Instance.currentNormalGameOptions.MapId = (byte)map; });
            hideNSeekHunterCount = CustomOption.Create(3000, Types.HideNSeekMain, cs(Color.yellow, ModTranslation.GetString("hideNSeekHunterCount")), 1f, 1f, 3f, 1f);
            hideNSeekKillCooldown = CustomOption.Create(3021, Types.HideNSeekMain, cs(Color.yellow, ModTranslation.GetString("hideNSeekKillCooldown")), 10f, 2.5f, 60f, 2.5f);
            hideNSeekHunterVision = CustomOption.Create(3001, Types.HideNSeekMain, cs(Color.yellow, ModTranslation.GetString("hideNSeekHunterVision")), 0.5f, 0.25f, 2f, 0.25f);
            hideNSeekHuntedVision = CustomOption.Create(3002, Types.HideNSeekMain, cs(Color.yellow, ModTranslation.GetString("hideNSeekHuntedVision")), 2f, 0.25f, 5f, 0.25f);
            hideNSeekCommonTasks = CustomOption.Create(3023, Types.HideNSeekMain, cs(Color.yellow, ModTranslation.GetString("hideNSeekCommonTasks")), 1f, 0f, 4f, 1f);
            hideNSeekShortTasks = CustomOption.Create(3024, Types.HideNSeekMain, cs(Color.yellow, ModTranslation.GetString("hideNSeekShortTasks")), 3f, 1f, 23f, 1f);
            hideNSeekLongTasks = CustomOption.Create(3025, Types.HideNSeekMain, cs(Color.yellow, ModTranslation.GetString("hideNSeekLongTasks")), 3f, 0f, 15f, 1f);
            hideNSeekTimer = CustomOption.Create(3003, Types.HideNSeekMain, cs(Color.yellow, ModTranslation.GetString("hideNSeekTimer")), 5f, 1f, 30f, 0.5f);
            hideNSeekTaskWin = CustomOption.Create(3004, Types.HideNSeekMain, cs(Color.yellow, ModTranslation.GetString("hideNSeekTaskWin")), false);
            hideNSeekTaskPunish = CustomOption.Create(3017, Types.HideNSeekMain, cs(Color.yellow, ModTranslation.GetString("hideNSeekTaskPunish")), 10f, 0f, 30f, 1f);
            hideNSeekCanSabotage = CustomOption.Create(3019, Types.HideNSeekMain, cs(Color.yellow, ModTranslation.GetString("hideNSeekCanSabotage")), false);
            hideNSeekHunterWaiting = CustomOption.Create(3022, Types.HideNSeekMain, cs(Color.yellow, ModTranslation.GetString("hideNSeekHunterWaiting")), 15f, 2.5f, 60f, 2.5f);

            hunterLightCooldown = CustomOption.Create(3005, Types.HideNSeekRoles, cs(Color.red, ModTranslation.GetString("hunterLightCooldown")), 30f, 5f, 60f, 1f, null, true, heading: ModTranslation.GetString("hunterSettings"));
            hunterLightDuration = CustomOption.Create(3006, Types.HideNSeekRoles, cs(Color.red, ModTranslation.GetString("hunterLightDuration")), 5f, 1f, 60f, 1f);
            hunterLightVision = CustomOption.Create(3007, Types.HideNSeekRoles, cs(Color.red, ModTranslation.GetString("hunterLightVision")), 3f, 1f, 5f, 0.25f);
            hunterLightPunish = CustomOption.Create(3008, Types.HideNSeekRoles, cs(Color.red, ModTranslation.GetString("hunterLightPunish")), 5f, 0f, 30f, 1f);
            hunterAdminCooldown = CustomOption.Create(3009, Types.HideNSeekRoles, cs(Color.red, ModTranslation.GetString("hunterAdminCooldown")), 30f, 5f, 60f, 1f);
            hunterAdminDuration = CustomOption.Create(3010, Types.HideNSeekRoles, cs(Color.red, ModTranslation.GetString("hunterAdminDuration")), 5f, 1f, 60f, 1f);
            hunterAdminPunish = CustomOption.Create(3011, Types.HideNSeekRoles, cs(Color.red, ModTranslation.GetString("hunterAdminPunish")), 5f, 0f, 30f, 1f);
            hunterArrowCooldown = CustomOption.Create(3012, Types.HideNSeekRoles, cs(Color.red, ModTranslation.GetString("hunterArrowCooldown")), 30f, 5f, 60f, 1f);
            hunterArrowDuration = CustomOption.Create(3013, Types.HideNSeekRoles, cs(Color.red, ModTranslation.GetString("hunterArrowDuration")), 5f, 0f, 60f, 1f);
            hunterArrowPunish = CustomOption.Create(3014, Types.HideNSeekRoles, cs(Color.red, ModTranslation.GetString("hunterArrowPunish")), 5f, 0f, 30f, 1f);

            huntedShieldCooldown = CustomOption.Create(3015, Types.HideNSeekRoles, cs(Color.gray, ModTranslation.GetString("huntedShieldCooldown")), 30f, 5f, 60f, 1f, null, true, heading: ModTranslation.GetString("huntedSettings"));
            huntedShieldDuration = CustomOption.Create(3016, Types.HideNSeekRoles, cs(Color.gray, ModTranslation.GetString("huntedShieldDuration")), 5f, 1f, 60f, 1f);
            huntedShieldRewindTime = CustomOption.Create(3018, Types.HideNSeekRoles, cs(Color.gray, ModTranslation.GetString("huntedShieldRewindTime")), 3f, 1f, 10f, 1f);
            huntedShieldNumber = CustomOption.Create(3026, Types.HideNSeekRoles, cs(Color.gray, ModTranslation.GetString("huntedShieldNumber")), 3f, 1f, 15f, 1f);

            // Prop Hunt General Options
            propHuntMap = CustomOption.Create(4020, Types.PropHunt, cs(Color.yellow, ModTranslation.GetString("propHuntMap")), new string[] { "The Skeld", "Mira", "Polus", "Airship", "Fungle", "Submerged", "LI Map" }, null, true, onChange: () => { int map = propHuntMap.selection; if (map >= 3) map++; GameOptionsManager.Instance.currentNormalGameOptions.MapId = (byte)map; });
            propHuntTimer = CustomOption.Create(4021, Types.PropHunt, cs(Color.yellow, ModTranslation.GetString("propHuntTimer")), 5f, 1f, 30f, 0.5f, null, true, heading: ModTranslation.GetString("propHuntSettings"));
            propHuntUnstuckCooldown = CustomOption.Create(4011, Types.PropHunt, cs(Color.yellow, ModTranslation.GetString("propHuntUnstuckCooldown")), 30f, 2.5f, 60f, 2.5f);
            propHuntUnstuckDuration = CustomOption.Create(4012, Types.PropHunt, cs(Color.yellow, ModTranslation.GetString("propHuntUnstuckDuration")), 2f, 1f, 60f, 1f);
            propHunterVision = CustomOption.Create(4006, Types.PropHunt, cs(Color.yellow, ModTranslation.GetString("propHunterVision")), 0.5f, 0.25f, 2f, 0.25f);
            propVision = CustomOption.Create(4007, Types.PropHunt, cs(Color.yellow, ModTranslation.GetString("propVision")), 2f, 0.25f, 5f, 0.25f);

            // Hunter Options
            propHuntNumberOfHunters = CustomOption.Create(4000, Types.PropHunt, cs(Color.red, ModTranslation.GetString("propHuntNumberOfHunters")), 1f, 1f, 5f, 1f, null, true, heading: ModTranslation.GetString("hunterSettings"));
            hunterInitialBlackoutTime = CustomOption.Create(4001, Types.PropHunt, cs(Color.red, ModTranslation.GetString("hunterInitialBlackoutTime")), 10f, 5f, 20f, 1f);
            hunterMissCooldown = CustomOption.Create(4004, Types.PropHunt, cs(Color.red, ModTranslation.GetString("hunterMissCooldown")), 10f, 2.5f, 60f, 2.5f);
            hunterHitCooldown = CustomOption.Create(4005, Types.PropHunt, cs(Color.red, ModTranslation.GetString("hunterHitCooldown")), 10f, 2.5f, 60f, 2.5f);
            propHuntRevealCooldown = CustomOption.Create(4008, Types.PropHunt, cs(Color.red, ModTranslation.GetString("propHuntRevealCooldown")), 30f, 10f, 90f, 2.5f);
            propHuntRevealDuration = CustomOption.Create(4009, Types.PropHunt, cs(Color.red, ModTranslation.GetString("propHuntRevealDuration")), 5f, 1f, 60f, 1f);
            propHuntRevealPunish = CustomOption.Create(4010, Types.PropHunt, cs(Color.red, ModTranslation.GetString("propHuntRevealPunish")), 10f, 0f, 1800f, 5f);
            propHuntAdminCooldown = CustomOption.Create(4022, Types.PropHunt, cs(Color.red, ModTranslation.GetString("propHuntAdminCooldown")), 30f, 2.5f, 1800f, 2.5f);
            propHuntFindCooldown = CustomOption.Create(4023, Types.PropHunt, cs(Color.red, ModTranslation.GetString("propHuntFindCooldown")), 60f, 2.5f, 1800f, 2.5f);
            propHuntFindDuration = CustomOption.Create(4024, Types.PropHunt, cs(Color.red, ModTranslation.GetString("propHuntFindDuration")), 5f, 1f, 15f, 1f);

            // Prop Options
            propBecomesHunterWhenFound = CustomOption.Create(4003, Types.PropHunt, cs(Palette.CrewmateBlue, ModTranslation.GetString("propBecomesHunterWhenFound")), false, null, true, heading: ModTranslation.GetString("propSettings"));
            propHuntInvisEnabled = CustomOption.Create(4013, Types.PropHunt, cs(Palette.CrewmateBlue, ModTranslation.GetString("propHuntInvisEnabled")), true, null, true);
            propHuntInvisCooldown = CustomOption.Create(4014, Types.PropHunt, cs(Palette.CrewmateBlue, ModTranslation.GetString("propHuntInvisCooldown")), 120f, 10f, 1800f, 2.5f, propHuntInvisEnabled);
            propHuntInvisDuration = CustomOption.Create(4015, Types.PropHunt, cs(Palette.CrewmateBlue, ModTranslation.GetString("propHuntInvisDuration")), 5f, 1f, 30f, 1f, propHuntInvisEnabled);
            propHuntSpeedboostEnabled = CustomOption.Create(4016, Types.PropHunt, cs(Palette.CrewmateBlue, ModTranslation.GetString("propHuntSpeedboostEnabled")), true, null, true);
            propHuntSpeedboostCooldown = CustomOption.Create(4017, Types.PropHunt, cs(Palette.CrewmateBlue, ModTranslation.GetString("propHuntSpeedboostCooldown")), 60f, 2.5f, 1800f, 2.5f, propHuntSpeedboostEnabled);
            propHuntSpeedboostDuration = CustomOption.Create(4018, Types.PropHunt, cs(Palette.CrewmateBlue, ModTranslation.GetString("propHuntSpeedboostDuration")), 5f, 1f, 15f, 1f, propHuntSpeedboostEnabled);
            propHuntSpeedboostSpeed = CustomOption.Create(4019, Types.PropHunt, cs(Palette.CrewmateBlue, ModTranslation.GetString("propHuntSpeedboostSpeed")), 2f, 1.25f, 5f, 0.25f, propHuntSpeedboostEnabled);




            // Other options
            maxNumberOfMeetings = CustomOption.Create(3, Types.General, ModTranslation.GetString("maxNumberOfMeetings"), 10, 0, 15, 1, null, true, heading: ModTranslation.GetString("inGameSettings"));
            anyPlayerCanStopStart = CustomOption.Create(2, Types.General, cs(new Color(204f / 255f, 204f / 255f, 0, 1f), ModTranslation.GetString("anyPlayerCanStopStart")), false, null, false);
            blockSkippingInEmergencyMeetings = CustomOption.Create(4, Types.General, ModTranslation.GetString("blockSkippingInEmergencyMeetings"), false);
            noVoteIsSelfVote = CustomOption.Create(5, Types.General, ModTranslation.GetString("noVoteIsSelfVote"), false, blockSkippingInEmergencyMeetings);
            hidePlayerNames = CustomOption.Create(6, Types.General, ModTranslation.GetString("hidePlayerNames"), false);
            allowParallelMedBayScans = CustomOption.Create(7, Types.General, ModTranslation.GetString("allowParallelMedBayScans"), false);
            shieldFirstKill = CustomOption.Create(8, Types.General, ModTranslation.GetString("shieldFirstKill"), false);
            finishTasksBeforeHauntingOrZoomingOut = CustomOption.Create(9, Types.General, ModTranslation.GetString("finishTasksBeforeHauntingOrZoomingOut"), true);
            deadImpsBlockSabotage = CustomOption.Create(13, Types.General, ModTranslation.GetString("deadImpsBlockSabotage"), false, null, false);
            camsNightVision = CustomOption.Create(11, Types.General, ModTranslation.GetString("camsNightVision"), false, null, true, heading: ModTranslation.GetString("camsNightVisionSettings"));




            dynamicMap = CustomOption.Create(500, Types.General, "dynamicMap".Translate(), false, null, true, heading: "dynamicMap".Translate());
            dynamicMapEnableSkeld = CustomOption.Create(501, Types.General, "Skeld", rates, dynamicMap, false);
            dynamicMapEnableMira = CustomOption.Create(502, Types.General, "Mira", rates, dynamicMap, false);
            dynamicMapEnablePolus = CustomOption.Create(503, Types.General, "Polus", rates, dynamicMap, false);
            dynamicMapEnableAirShip = CustomOption.Create(504, Types.General, "Airship", rates, dynamicMap, false);
            dynamicMapEnableFungle = CustomOption.Create(506, Types.General, "Fungle", rates, dynamicMap, false);
            dynamicMapEnableSubmerged = CustomOption.Create(505, Types.General, "Submerged", rates, dynamicMap, false);
            dynamicMapSeparateSettings = CustomOption.Create(509, Types.General, "dynamicMapSeparateSettings".Translate(), false, dynamicMap, true);
            camsNoNightVisionIfImpVision = CustomOption.Create(12, Types.General, "camsNoNightVisionIfImpVision".Translate(), false, camsNightVision, false);

            AddVents = CustomOption.Create(114513, Types.General, "AddVents".Translate(), false, null, true, heading: "AddVents".Translate());
            addPolusVents = CustomOption.Create(114514, Types.General, "addPolusVents".Translate(), false, enableBetterPolus);
            addAirShipVents = CustomOption.Create(114515, Types.General, "addAirShipVents".Translate(), false, enableAirShipModify);
            enableAirShipModify = CustomOption.Create(114516, Types.General, cs(Color.yellow, "enableAirShipModify".Translate()), false, null);
            enableBetterPolus = CustomOption.Create(114517, Types.General, "enableBetterPolus".Translate(), false, null);




            blockedRolePairings.Add((byte)RoleId.Vampire, new [] { (byte)RoleId.Warlock});
            blockedRolePairings.Add((byte)RoleId.Warlock, new [] { (byte)RoleId.Vampire});
            blockedRolePairings.Add((byte)RoleId.Spy, new [] { (byte)RoleId.Mini});
            blockedRolePairings.Add((byte)RoleId.Mini, new [] { (byte)RoleId.Spy});
            blockedRolePairings.Add((byte)RoleId.Vulture, new [] { (byte)RoleId.Cleaner});
            blockedRolePairings.Add((byte)RoleId.Cleaner, new [] { (byte)RoleId.Vulture});
            
        }
    }
}
