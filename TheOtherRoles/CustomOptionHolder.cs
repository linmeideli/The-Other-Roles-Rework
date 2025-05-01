using System;
using System.Collections.Generic;
using TheOtherRoles.Modules;
using TheOtherRoles.Utilities;
using UnityEngine;

namespace TheOtherRoles
{
	// Token: 0x02000009 RID: 9
	public class CustomOptionHolder
	{
		// Token: 0x06000010 RID: 16 RVA: 0x00005C84 File Offset: 0x00003E84
		public static string cs(Color c, string s)
		{
			return string.Format("<color=#{0:X2}{1:X2}{2:X2}{3:X2}>{4}</color>", new object[]
			{
				CustomOptionHolder.ToByte(c.r),
				CustomOptionHolder.ToByte(c.g),
				CustomOptionHolder.ToByte(c.b),
				CustomOptionHolder.ToByte(c.a),
				s
			});
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00005CF8 File Offset: 0x00003EF8
		private static byte ToByte(float f)
		{
			f = Mathf.Clamp01(f);
			return (byte)(f * 255f);
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00005D1C File Offset: 0x00003F1C
		public static bool isMapSelectionOption(CustomOption option)
		{
			return option == CustomOptionHolder.propHuntMap && option == CustomOptionHolder.hideNSeekMap;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00005D44 File Offset: 0x00003F44
		public static void Load()
		{
            CustomOption.vanillaSettings = TheOtherRolesPlugin.Instance.Config.Bind("Preset0", "VanillaOptions", "");
            CustomOptionHolder.presetSelection = CustomOption.Create(0, CustomOption.CustomOptionType.General, CustomOptionHolder.cs(new Color(0.8f, 0.8f, 0f, 1f), "presetSelection".Translate()), CustomOptionHolder.presets, null, true, null, "");
			bool canBeEnabled = EventUtility.canBeEnabled;
			if (canBeEnabled)
			{
				CustomOptionHolder.enableEventMode = CustomOption.Create(10423, CustomOption.CustomOptionType.General, CustomOptionHolder.cs(Color.green, ModTranslation.GetString("enableEventMode", null)), true, null, true, null, "");
			}
			CustomOptionHolder.crewmateRolesCountMin = CustomOption.Create(300, CustomOption.CustomOptionType.General, CustomOptionHolder.cs(new Color(0.8f, 0.8f, 0f, 1f), ModTranslation.GetString("crewmateRolesCountMin", null)), 15f, 0f, 15f, 1f, null, true, null, "Min/Max Roles");
			CustomOptionHolder.crewmateRolesCountMax = CustomOption.Create(301, CustomOption.CustomOptionType.General, CustomOptionHolder.cs(new Color(0.8f, 0.8f, 0f, 1f), ModTranslation.GetString("crewmateRolesCountMax", null)), 15f, 0f, 15f, 1f, null, false, null, "");
			CustomOptionHolder.neutralRolesCountMin = CustomOption.Create(302, CustomOption.CustomOptionType.General, CustomOptionHolder.cs(new Color(0.8f, 0.8f, 0f, 1f), ModTranslation.GetString("neutralRolesCountMin", null)), 15f, 0f, 15f, 1f, null, false, null, "");
			CustomOptionHolder.neutralRolesCountMax = CustomOption.Create(303, CustomOption.CustomOptionType.General, CustomOptionHolder.cs(new Color(0.8f, 0.8f, 0f, 1f), ModTranslation.GetString("neutralRolesCountMax", null)), 15f, 0f, 15f, 1f, null, false, null, "");
			CustomOptionHolder.impostorRolesCountMin = CustomOption.Create(304, CustomOption.CustomOptionType.General, CustomOptionHolder.cs(new Color(0.8f, 0.8f, 0f, 1f), ModTranslation.GetString("impostorRolesCountMin", null)), 15f, 0f, 15f, 1f, null, false, null, "");
			CustomOptionHolder.impostorRolesCountMax = CustomOption.Create(305, CustomOption.CustomOptionType.General, CustomOptionHolder.cs(new Color(0.8f, 0.8f, 0f, 1f), ModTranslation.GetString("impostorRolesCountMax", null)), 15f, 0f, 15f, 1f, null, false, null, "");
			CustomOptionHolder.modifiersCountMin = CustomOption.Create(306, CustomOption.CustomOptionType.General, CustomOptionHolder.cs(new Color(0.8f, 0.8f, 0f, 1f), ModTranslation.GetString("modifiersCountMin", null)), 15f, 0f, 15f, 1f, null, false, null, "");
			CustomOptionHolder.modifiersCountMax = CustomOption.Create(307, CustomOption.CustomOptionType.General, CustomOptionHolder.cs(new Color(0.8f, 0.8f, 0f, 1f), ModTranslation.GetString("modifiersCountMax", null)), 15f, 0f, 15f, 1f, null, false, null, "");
			CustomOptionHolder.crewmateRolesFill = CustomOption.Create(308, CustomOption.CustomOptionType.General, CustomOptionHolder.cs(new Color(0.8f, 0.8f, 0f, 1f), ModTranslation.GetString("crewmateRolesFill", null)), false, null, false, null, "");
			CustomOptionHolder.mafiaSpawnRate = CustomOption.Create(18, CustomOption.CustomOptionType.Impostor, CustomOptionHolder.cs(TheOtherRoles.Janitor.color, ModTranslation.GetString("mafiaSpawnRate", null)), CustomOptionHolder.rates, null, true, null, "");
			CustomOptionHolder.janitorCooldown = CustomOption.Create(19, CustomOption.CustomOptionType.Impostor, ModTranslation.GetString("janitorCooldown", null), 30f, 10f, 60f, 2.5f, CustomOptionHolder.mafiaSpawnRate, false, null, "");
			CustomOptionHolder.morphlingSpawnRate = CustomOption.Create(20, CustomOption.CustomOptionType.Impostor, CustomOptionHolder.cs(Morphling.color, ModTranslation.GetString("Morphling", null)), CustomOptionHolder.rates, null, true, null, "");
			CustomOptionHolder.morphlingCooldown = CustomOption.Create(21, CustomOption.CustomOptionType.Impostor, ModTranslation.GetString("morphlingCooldown", null), 30f, 10f, 60f, 2.5f, CustomOptionHolder.morphlingSpawnRate, false, null, "");
			CustomOptionHolder.morphlingDuration = CustomOption.Create(22, CustomOption.CustomOptionType.Impostor, ModTranslation.GetString("morphlingDuration", null), 10f, 1f, 20f, 0.5f, CustomOptionHolder.morphlingSpawnRate, false, null, "");
			CustomOptionHolder.camouflagerSpawnRate = CustomOption.Create(30, CustomOption.CustomOptionType.Impostor, CustomOptionHolder.cs(Camouflager.color, ModTranslation.GetString("Camouflager", null)), CustomOptionHolder.rates, null, true, null, "");
			CustomOptionHolder.camouflagerCooldown = CustomOption.Create(31, CustomOption.CustomOptionType.Impostor, ModTranslation.GetString("camouflagerCooldown", null), 30f, 10f, 60f, 2.5f, CustomOptionHolder.camouflagerSpawnRate, false, null, "");
			CustomOptionHolder.camouflagerDuration = CustomOption.Create(32, CustomOption.CustomOptionType.Impostor, ModTranslation.GetString("camouflagerDuration", null), 10f, 1f, 20f, 0.5f, CustomOptionHolder.camouflagerSpawnRate, false, null, "");
			CustomOptionHolder.vampireSpawnRate = CustomOption.Create(40, CustomOption.CustomOptionType.Impostor, CustomOptionHolder.cs(Vampire.color, ModTranslation.GetString("Vampire", null)), CustomOptionHolder.rates, null, true, null, "");
			CustomOptionHolder.vampireKillDelay = CustomOption.Create(41, CustomOption.CustomOptionType.Impostor, ModTranslation.GetString("vampireKillDelay", null), 10f, 1f, 20f, 1f, CustomOptionHolder.vampireSpawnRate, false, null, "");
			CustomOptionHolder.vampireCooldown = CustomOption.Create(42, CustomOption.CustomOptionType.Impostor, ModTranslation.GetString("vampireCooldown", null), 30f, 10f, 60f, 2.5f, CustomOptionHolder.vampireSpawnRate, false, null, "");
			CustomOptionHolder.vampireCanKillNearGarlics = CustomOption.Create(43, CustomOption.CustomOptionType.Impostor, ModTranslation.GetString("vampireCanKillNearGarlics", null), true, CustomOptionHolder.vampireSpawnRate, false, null, "");
			CustomOptionHolder.eraserSpawnRate = CustomOption.Create(230, CustomOption.CustomOptionType.Impostor, CustomOptionHolder.cs(Eraser.color, ModTranslation.GetString("Eraser", null)), CustomOptionHolder.rates, null, true, null, "");
			CustomOptionHolder.eraserCooldown = CustomOption.Create(231, CustomOption.CustomOptionType.Impostor, ModTranslation.GetString(ModTranslation.GetString("eraserCooldown", null), null), 30f, 10f, 120f, 5f, CustomOptionHolder.eraserSpawnRate, false, null, "");
			CustomOptionHolder.eraserCanEraseAnyone = CustomOption.Create(232, CustomOption.CustomOptionType.Impostor, ModTranslation.GetString(ModTranslation.GetString("eraserCanEraseAnyone", null), null), false, CustomOptionHolder.eraserSpawnRate, false, null, "");
			CustomOptionHolder.tricksterSpawnRate = CustomOption.Create(250, CustomOption.CustomOptionType.Impostor, CustomOptionHolder.cs(Trickster.color, ModTranslation.GetString("Trickster", null)), CustomOptionHolder.rates, null, true, null, "");
			CustomOptionHolder.tricksterPlaceBoxCooldown = CustomOption.Create(251, CustomOption.CustomOptionType.Impostor, ModTranslation.GetString("tricksterPlaceBoxCooldown", null), 10f, 2.5f, 30f, 2.5f, CustomOptionHolder.tricksterSpawnRate, false, null, "");
			CustomOptionHolder.tricksterLightsOutCooldown = CustomOption.Create(252, CustomOption.CustomOptionType.Impostor, ModTranslation.GetString("tricksterLightsOutCooldown", null), 30f, 10f, 60f, 5f, CustomOptionHolder.tricksterSpawnRate, false, null, "");
			CustomOptionHolder.tricksterLightsOutDuration = CustomOption.Create(253, CustomOption.CustomOptionType.Impostor, ModTranslation.GetString("tricksterLightsOutDuration", null), 15f, 5f, 60f, 2.5f, CustomOptionHolder.tricksterSpawnRate, false, null, "");
			CustomOptionHolder.cleanerSpawnRate = CustomOption.Create(260, CustomOption.CustomOptionType.Impostor, CustomOptionHolder.cs(Cleaner.color, ModTranslation.GetString("Cleaner", null)), CustomOptionHolder.rates, null, true, null, "");
			CustomOptionHolder.cleanerCooldown = CustomOption.Create(261, CustomOption.CustomOptionType.Impostor, ModTranslation.GetString("cleanerCooldown", null), 30f, 10f, 60f, 2.5f, CustomOptionHolder.cleanerSpawnRate, false, null, "");
			CustomOptionHolder.warlockSpawnRate = CustomOption.Create(270, CustomOption.CustomOptionType.Impostor, CustomOptionHolder.cs(Cleaner.color, ModTranslation.GetString("Warlock", null)), CustomOptionHolder.rates, null, true, null, "");
			CustomOptionHolder.warlockCooldown = CustomOption.Create(271, CustomOption.CustomOptionType.Impostor, ModTranslation.GetString("warlockCooldown", null), 30f, 10f, 60f, 2.5f, CustomOptionHolder.warlockSpawnRate, false, null, "");
			CustomOptionHolder.warlockRootTime = CustomOption.Create(272, CustomOption.CustomOptionType.Impostor, ModTranslation.GetString("warlockRootTime", null), 5f, 0f, 15f, 1f, CustomOptionHolder.warlockSpawnRate, false, null, "");
			CustomOptionHolder.bountyHunterSpawnRate = CustomOption.Create(320, CustomOption.CustomOptionType.Impostor, CustomOptionHolder.cs(BountyHunter.color, ModTranslation.GetString("BountyHunter", null)), CustomOptionHolder.rates, null, true, null, "");
			CustomOptionHolder.bountyHunterBountyDuration = CustomOption.Create(321, CustomOption.CustomOptionType.Impostor, ModTranslation.GetString("bountyHunterBountyDuration", null), 60f, 10f, 180f, 10f, CustomOptionHolder.bountyHunterSpawnRate, false, null, "");
			CustomOptionHolder.bountyHunterReducedCooldown = CustomOption.Create(322, CustomOption.CustomOptionType.Impostor, ModTranslation.GetString("bountyHunterReducedCooldown", null), 2.5f, 0f, 30f, 2.5f, CustomOptionHolder.bountyHunterSpawnRate, false, null, "");
			CustomOptionHolder.bountyHunterPunishmentTime = CustomOption.Create(323, CustomOption.CustomOptionType.Impostor, ModTranslation.GetString("bountyHunterPunishmentTime", null), 20f, 0f, 60f, 2.5f, CustomOptionHolder.bountyHunterSpawnRate, false, null, "");
			CustomOptionHolder.bountyHunterShowArrow = CustomOption.Create(324, CustomOption.CustomOptionType.Impostor, ModTranslation.GetString("bountyHunterShowArrow", null), true, CustomOptionHolder.bountyHunterSpawnRate, false, null, "");
			CustomOptionHolder.bountyHunterArrowUpdateIntervall = CustomOption.Create(325, CustomOption.CustomOptionType.Impostor, ModTranslation.GetString("bountyHunterArrowUpdateIntervall", null), 15f, 2.5f, 60f, 2.5f, CustomOptionHolder.bountyHunterShowArrow, false, null, "");
			CustomOptionHolder.bountyHunterShowCooldownForGhosts = CustomOption.Create(4399, CustomOption.CustomOptionType.Impostor, ModTranslation.GetString("bountyHunterShowCooldownForGhosts", null), true, CustomOptionHolder.bountyHunterSpawnRate, false, null, "");
			CustomOptionHolder.witchSpawnRate = CustomOption.Create(370, CustomOption.CustomOptionType.Impostor, CustomOptionHolder.cs(Witch.color, ModTranslation.GetString("Witch", null)), CustomOptionHolder.rates, null, true, null, "");
			CustomOptionHolder.witchCooldown = CustomOption.Create(371, CustomOption.CustomOptionType.Impostor, ModTranslation.GetString("witchCooldown", null), 30f, 10f, 120f, 5f, CustomOptionHolder.witchSpawnRate, false, null, "");
			CustomOptionHolder.witchAdditionalCooldown = CustomOption.Create(372, CustomOption.CustomOptionType.Impostor, ModTranslation.GetString("witchAdditionalCooldown", null), 10f, 0f, 60f, 5f, CustomOptionHolder.witchSpawnRate, false, null, "");
			CustomOptionHolder.witchCanSpellAnyone = CustomOption.Create(373, CustomOption.CustomOptionType.Impostor, ModTranslation.GetString("witchCanSpellAnyone", null), false, CustomOptionHolder.witchSpawnRate, false, null, "");
			CustomOptionHolder.witchSpellCastingDuration = CustomOption.Create(374, CustomOption.CustomOptionType.Impostor, ModTranslation.GetString("witchSpellCastingDuration", null), 1f, 0f, 10f, 1f, CustomOptionHolder.witchSpawnRate, false, null, "");
			CustomOptionHolder.witchTriggerBothCooldowns = CustomOption.Create(375, CustomOption.CustomOptionType.Impostor, ModTranslation.GetString("witchTriggerBothCooldowns", null), true, CustomOptionHolder.witchSpawnRate, false, null, "");
			CustomOptionHolder.witchVoteSavesTargets = CustomOption.Create(376, CustomOption.CustomOptionType.Impostor, ModTranslation.GetString("witchVoteSavesTargets", null), true, CustomOptionHolder.witchSpawnRate, false, null, "");
			CustomOptionHolder.ninjaSpawnRate = CustomOption.Create(380, CustomOption.CustomOptionType.Impostor, CustomOptionHolder.cs(Ninja.color, ModTranslation.GetString("Ninja", null)), CustomOptionHolder.rates, null, true, null, "");
			CustomOptionHolder.ninjaCooldown = CustomOption.Create(381, CustomOption.CustomOptionType.Impostor, ModTranslation.GetString("ninjaCooldown", null), 30f, 10f, 120f, 5f, CustomOptionHolder.ninjaSpawnRate, false, null, "");
			CustomOptionHolder.ninjaKnowsTargetLocation = CustomOption.Create(382, CustomOption.CustomOptionType.Impostor, ModTranslation.GetString("ninjaKnowsTargetLocation", null), true, CustomOptionHolder.ninjaSpawnRate, false, null, "");
			CustomOptionHolder.ninjaTraceTime = CustomOption.Create(383, CustomOption.CustomOptionType.Impostor, ModTranslation.GetString("ninjaTraceTime", null), 5f, 1f, 20f, 0.5f, CustomOptionHolder.ninjaSpawnRate, false, null, "");
			CustomOptionHolder.ninjaTraceColorTime = CustomOption.Create(384, CustomOption.CustomOptionType.Impostor, ModTranslation.GetString("ninjaTraceColorTime", null), 2f, 0f, 20f, 0.5f, CustomOptionHolder.ninjaSpawnRate, false, null, "");
			CustomOptionHolder.ninjaInvisibleDuration = CustomOption.Create(385, CustomOption.CustomOptionType.Impostor, ModTranslation.GetString("ninjaInvisibleDuration", null), 3f, 0f, 20f, 1f, CustomOptionHolder.ninjaSpawnRate, false, null, "");
			CustomOptionHolder.bomberSpawnRate = CustomOption.Create(460, CustomOption.CustomOptionType.Impostor, CustomOptionHolder.cs(Bomber.color, ModTranslation.GetString("Bomber", null)), CustomOptionHolder.rates, null, true, null, "");
			CustomOptionHolder.bomberBombDestructionTime = CustomOption.Create(461, CustomOption.CustomOptionType.Impostor, ModTranslation.GetString("bomberBombDestructionTime", null), 20f, 2.5f, 120f, 2.5f, CustomOptionHolder.bomberSpawnRate, false, null, "");
			CustomOptionHolder.bomberBombDestructionRange = CustomOption.Create(462, CustomOption.CustomOptionType.Impostor, ModTranslation.GetString("bomberBombDestructionRange", null), 50f, 5f, 150f, 5f, CustomOptionHolder.bomberSpawnRate, false, null, "");
			CustomOptionHolder.bomberBombHearRange = CustomOption.Create(463, CustomOption.CustomOptionType.Impostor, ModTranslation.GetString("bomberBombHearRange", null), 60f, 5f, 150f, 5f, CustomOptionHolder.bomberSpawnRate, false, null, "");
			CustomOptionHolder.bomberDefuseDuration = CustomOption.Create(464, CustomOption.CustomOptionType.Impostor, ModTranslation.GetString("bomberDefuseDuration", null), 3f, 0.5f, 30f, 0.5f, CustomOptionHolder.bomberSpawnRate, false, null, "");
			CustomOptionHolder.bomberBombCooldown = CustomOption.Create(465, CustomOption.CustomOptionType.Impostor, ModTranslation.GetString("bomberBombCooldown", null), 15f, 2.5f, 30f, 2.5f, CustomOptionHolder.bomberSpawnRate, false, null, "");
			CustomOptionHolder.bomberBombActiveAfter = CustomOption.Create(466, CustomOption.CustomOptionType.Impostor, ModTranslation.GetString("bomberBombActiveAfter", null), 3f, 0.5f, 15f, 0.5f, CustomOptionHolder.bomberSpawnRate, false, null, "");
			CustomOptionHolder.yoyoSpawnRate = CustomOption.Create(470, CustomOption.CustomOptionType.Impostor, CustomOptionHolder.cs(Yoyo.color, ModTranslation.GetString("Yoyo", null)), CustomOptionHolder.rates, null, true, null, "");
			CustomOptionHolder.yoyoBlinkDuration = CustomOption.Create(471, CustomOption.CustomOptionType.Impostor, ModTranslation.GetString("yoyoBlinkDuration", null), 20f, 2.5f, 120f, 2.5f, CustomOptionHolder.yoyoSpawnRate, false, null, "");
			CustomOptionHolder.yoyoMarkCooldown = CustomOption.Create(472, CustomOption.CustomOptionType.Impostor, ModTranslation.GetString("yoyoMarkCooldown", null), 20f, 2.5f, 120f, 2.5f, CustomOptionHolder.yoyoSpawnRate, false, null, "");
			CustomOptionHolder.yoyoMarkStaysOverMeeting = CustomOption.Create(473, CustomOption.CustomOptionType.Impostor, ModTranslation.GetString("yoyoMarkStaysOverMeeting", null), true, CustomOptionHolder.yoyoSpawnRate, false, null, "");
			CustomOptionHolder.yoyoHasAdminTable = CustomOption.Create(474, CustomOption.CustomOptionType.Impostor, ModTranslation.GetString("yoyoHasAdminTable", null), true, CustomOptionHolder.yoyoSpawnRate, false, null, "");
			CustomOptionHolder.yoyoAdminTableCooldown = CustomOption.Create(475, CustomOption.CustomOptionType.Impostor, ModTranslation.GetString("yoyoAdminTableCooldown", null), 20f, 2.5f, 120f, 2.5f, CustomOptionHolder.yoyoHasAdminTable, false, null, "");
			CustomOptionHolder.yoyoSilhouetteVisibility = CustomOption.Create(476, CustomOption.CustomOptionType.Impostor, ModTranslation.GetString("yoyoSilhouetteVisibility", null), new string[]
			{
				"0%",
				"10%",
				"20%",
				"30%",
				"40%",
				"50%"
			}, CustomOptionHolder.yoyoSpawnRate, false, null, "");
			CustomOptionHolder.fraudsterSpawnRate = CustomOption.Create(480, CustomOption.CustomOptionType.Impostor, CustomOptionHolder.cs(Fraudster.color, ModTranslation.GetString("Fraudster", null)), CustomOptionHolder.rates, null, true, null, "");
			CustomOptionHolder.fraudstercooldown = CustomOption.Create(481, CustomOption.CustomOptionType.Impostor, ModTranslation.GetString("fraudstercooldown", null), 20f, 2.5f, 120f, 2.5f, CustomOptionHolder.fraudsterSpawnRate, false, null, "");
            CustomOptionHolder.fraudsterSpawnRate = CustomOption.Create(987, CustomOption.CustomOptionType.Impostor, CustomOptionHolder.cs(Devil.color, ModTranslation.GetString("Devil", null)), CustomOptionHolder.rates, null, true, null, "");
            CustomOptionHolder.fraudstercooldown = CustomOption.Create(988, CustomOption.CustomOptionType.Impostor, ModTranslation.GetString("DevilColldown", null), 20f, 2.5f, 120f, 2.5f, CustomOptionHolder.devilSpawnRate, false, null, "");
           // CustomOptionHolder.LasterSpawnRate = CustomOption.Create(65899, CustomOption.CustomOptionType.Neutral, CustomOptionHolder.cs(Guesser.color, ModTranslation.GetString("modifierAssassin", null)), CustomOptionHolder.rates, null, true, null, "");
            CustomOptionHolder.guesserSpawnRate = CustomOption.Create(310, CustomOption.CustomOptionType.Neutral, CustomOptionHolder.cs(Guesser.color, ModTranslation.GetString("modifierAssassin", null)), CustomOptionHolder.rates, null, true, null, "");
			CustomOptionHolder.guesserIsImpGuesserRate = CustomOption.Create(311, CustomOption.CustomOptionType.Neutral, ModTranslation.GetString("guesserIsImpGuesserRate", null), CustomOptionHolder.rates, CustomOptionHolder.guesserSpawnRate, false, null, "");
			CustomOptionHolder.guesserNumberOfShots = CustomOption.Create(312, CustomOption.CustomOptionType.Neutral, ModTranslation.GetString("modifierAssassinNumberOfShots", null), 2f, 1f, 15f, 1f, CustomOptionHolder.guesserSpawnRate, false, null, "");
			CustomOptionHolder.guesserHasMultipleShotsPerMeeting = CustomOption.Create(313, CustomOption.CustomOptionType.Neutral, ModTranslation.GetString("modifierAssassinMultipleShotsPerMeeting", null), false, CustomOptionHolder.guesserSpawnRate, false, null, "");
			CustomOptionHolder.guesserKillsThroughShield = CustomOption.Create(315, CustomOption.CustomOptionType.Neutral, ModTranslation.GetString("modifierAssassinKillsThroughShield", null), true, CustomOptionHolder.guesserSpawnRate, false, null, "");
			CustomOptionHolder.guesserEvilCanKillSpy = CustomOption.Create(316, CustomOption.CustomOptionType.Neutral, ModTranslation.GetString("guesserEvilCanKillSpy", null), true, CustomOptionHolder.guesserSpawnRate, false, null, "");
			CustomOptionHolder.guesserSpawnBothRate = CustomOption.Create(317, CustomOption.CustomOptionType.Neutral, ModTranslation.GetString("guesserSpawnBothRate", null), CustomOptionHolder.rates, CustomOptionHolder.guesserSpawnRate, false, null, "");
			CustomOptionHolder.guesserCantGuessSnitchIfTaksDone = CustomOption.Create(318, CustomOption.CustomOptionType.Neutral, ModTranslation.GetString("guesserCantGuessSnitchIfTaksDone", null), true, CustomOptionHolder.guesserSpawnRate, false, null, "");
			CustomOptionHolder.jesterSpawnRate = CustomOption.Create(60, CustomOption.CustomOptionType.Neutral, CustomOptionHolder.cs(TheOtherRoles.Jester.color, ModTranslation.GetString("Jester", null)), CustomOptionHolder.rates, null, true, null, "");
			CustomOptionHolder.jesterCanCallEmergency = CustomOption.Create(61, CustomOption.CustomOptionType.Neutral, ModTranslation.GetString("jesterCanCallEmergency", null), true, CustomOptionHolder.jesterSpawnRate, false, null, "");
			CustomOptionHolder.jesterHasImpostorVision = CustomOption.Create(62, CustomOption.CustomOptionType.Neutral, ModTranslation.GetString("jesterHasImpostorVision", null), false, CustomOptionHolder.jesterSpawnRate, false, null, "");
			CustomOptionHolder.arsonistSpawnRate = CustomOption.Create(290, CustomOption.CustomOptionType.Neutral, CustomOptionHolder.cs(Arsonist.color, ModTranslation.GetString("Arsonist", null)), CustomOptionHolder.rates, null, true, null, "");
			CustomOptionHolder.arsonistCooldown = CustomOption.Create(291, CustomOption.CustomOptionType.Neutral, ModTranslation.GetString("arsonistCooldown", null), 12.5f, 2.5f, 60f, 2.5f, CustomOptionHolder.arsonistSpawnRate, false, null, "");
			CustomOptionHolder.arsonistDuration = CustomOption.Create(292, CustomOption.CustomOptionType.Neutral, ModTranslation.GetString("arsonistDuration", null), 3f, 1f, 10f, 1f, CustomOptionHolder.arsonistSpawnRate, false, null, "");
			CustomOptionHolder.jackalSpawnRate = CustomOption.Create(220, CustomOption.CustomOptionType.Neutral, CustomOptionHolder.cs(Jackal.color, ModTranslation.GetString("Jackal", null)), CustomOptionHolder.rates, null, true, null, "");
			CustomOptionHolder.jackalKillCooldown = CustomOption.Create(221, CustomOption.CustomOptionType.Neutral, ModTranslation.GetString("jackalKillCooldown", null), 30f, 10f, 60f, 2.5f, CustomOptionHolder.jackalSpawnRate, false, null, "");
			CustomOptionHolder.jackalCreateSidekickCooldown = CustomOption.Create(222, CustomOption.CustomOptionType.Neutral, ModTranslation.GetString("jackalCreateSidekickCooldown", null), 30f, 10f, 60f, 2.5f, CustomOptionHolder.jackalSpawnRate, false, null, "");
			CustomOptionHolder.jackalCanUseVents = CustomOption.Create(223, CustomOption.CustomOptionType.Neutral, ModTranslation.GetString("jackalCanUseVents", null), true, CustomOptionHolder.jackalSpawnRate, false, null, "");
			CustomOptionHolder.jackalCanSabotageLights = CustomOption.Create(431, CustomOption.CustomOptionType.Neutral, ModTranslation.GetString("jackalCanSabotageLights", null), true, CustomOptionHolder.jackalSpawnRate, false, null, "");
			CustomOptionHolder.jackalCanCreateSidekick = CustomOption.Create(224, CustomOption.CustomOptionType.Neutral, ModTranslation.GetString("jackalCanCreateSidekick", null), false, CustomOptionHolder.jackalSpawnRate, false, null, "");
			CustomOptionHolder.sidekickPromotesToJackal = CustomOption.Create(225, CustomOption.CustomOptionType.Neutral, ModTranslation.GetString("sidekickPromotesToJackal", null), false, CustomOptionHolder.jackalCanCreateSidekick, false, null, "");
			CustomOptionHolder.sidekickCanKill = CustomOption.Create(226, CustomOption.CustomOptionType.Neutral, ModTranslation.GetString("sidekickCanKill", null), false, CustomOptionHolder.jackalCanCreateSidekick, false, null, "");
			CustomOptionHolder.sidekickCanUseVents = CustomOption.Create(227, CustomOption.CustomOptionType.Neutral, ModTranslation.GetString("sidekickCanUseVents", null), true, CustomOptionHolder.jackalCanCreateSidekick, false, null, "");
			CustomOptionHolder.sidekickCanSabotageLights = CustomOption.Create(432, CustomOption.CustomOptionType.Neutral, ModTranslation.GetString("sidekickCanSabotageLights", null), true, CustomOptionHolder.jackalCanCreateSidekick, false, null, "");
			CustomOptionHolder.jackalPromotedFromSidekickCanCreateSidekick = CustomOption.Create(228, CustomOption.CustomOptionType.Neutral, ModTranslation.GetString("jackalPromotedFromSidekickCanCreateSidekick", null), true, CustomOptionHolder.sidekickPromotesToJackal, false, null, "");
			CustomOptionHolder.jackalCanCreateSidekickFromImpostor = CustomOption.Create(229, CustomOption.CustomOptionType.Neutral, ModTranslation.GetString("jackalCanCreateSidekickFromImpostor", null), true, CustomOptionHolder.jackalCanCreateSidekick, false, null, "");
			CustomOptionHolder.jackalAndSidekickHaveImpostorVision = CustomOption.Create(430, CustomOption.CustomOptionType.Neutral, ModTranslation.GetString("jackalAndSidekickHaveImpostorVision", null), false, CustomOptionHolder.jackalSpawnRate, false, null, "");
			CustomOptionHolder.vultureSpawnRate = CustomOption.Create(340, CustomOption.CustomOptionType.Neutral, CustomOptionHolder.cs(Vulture.color, ModTranslation.GetString("Vulture", null)), CustomOptionHolder.rates, null, true, null, "");
			CustomOptionHolder.vultureCooldown = CustomOption.Create(341, CustomOption.CustomOptionType.Neutral, ModTranslation.GetString("vultureCooldown", null), 15f, 10f, 60f, 2.5f, CustomOptionHolder.vultureSpawnRate, false, null, "");
			CustomOptionHolder.vultureNumberToWin = CustomOption.Create(342, CustomOption.CustomOptionType.Neutral, ModTranslation.GetString("vultureNumberToWin", null), 4f, 1f, 10f, 1f, CustomOptionHolder.vultureSpawnRate, false, null, "");
			CustomOptionHolder.vultureCanUseVents = CustomOption.Create(343, CustomOption.CustomOptionType.Neutral, ModTranslation.GetString("vultureCanUseVents", null), true, CustomOptionHolder.vultureSpawnRate, false, null, "");
			CustomOptionHolder.vultureShowArrows = CustomOption.Create(344, CustomOption.CustomOptionType.Neutral, ModTranslation.GetString("vultureShowArrows", null), true, CustomOptionHolder.vultureSpawnRate, false, null, "");
			CustomOptionHolder.lawyerSpawnRate = CustomOption.Create(350, CustomOption.CustomOptionType.Neutral, CustomOptionHolder.cs(Lawyer.color, ModTranslation.GetString("Lawyer", null)), CustomOptionHolder.rates, null, true, null, "");
			CustomOptionHolder.lawyerIsProsecutorChance = CustomOption.Create(358, CustomOption.CustomOptionType.Neutral, ModTranslation.GetString("lawyerIsProsecutorChance", null), CustomOptionHolder.rates, CustomOptionHolder.lawyerSpawnRate, false, null, "");
			CustomOptionHolder.lawyerVision = CustomOption.Create(354, CustomOption.CustomOptionType.Neutral, ModTranslation.GetString("lawyerVision", null), 1f, 0.25f, 3f, 0.25f, CustomOptionHolder.lawyerSpawnRate, false, null, "");
			CustomOptionHolder.lawyerKnowsRole = CustomOption.Create(355, CustomOption.CustomOptionType.Neutral, ModTranslation.GetString("lawyerKnowsRole", null), false, CustomOptionHolder.lawyerSpawnRate, false, null, "");
			CustomOptionHolder.lawyerCanCallEmergency = CustomOption.Create(352, CustomOption.CustomOptionType.Neutral, ModTranslation.GetString("lawyerCanCallEmergency", null), true, CustomOptionHolder.lawyerSpawnRate, false, null, "");
			CustomOptionHolder.lawyerTargetCanBeJester = CustomOption.Create(351, CustomOption.CustomOptionType.Neutral, ModTranslation.GetString("lawyerTargetCanBeJester", null), false, CustomOptionHolder.lawyerSpawnRate, false, null, "");
			CustomOptionHolder.pursuerCooldown = CustomOption.Create(356, CustomOption.CustomOptionType.Neutral, ModTranslation.GetString("pursuerCooldown", null), 30f, 5f, 60f, 2.5f, CustomOptionHolder.lawyerSpawnRate, false, null, "");
			CustomOptionHolder.pursuerBlanksNumber = CustomOption.Create(357, CustomOption.CustomOptionType.Neutral, ModTranslation.GetString("pursuerBlanksNumber", null), 5f, 1f, 20f, 1f, CustomOptionHolder.lawyerSpawnRate, false, null, "");
			CustomOptionHolder.mayorSpawnRate = CustomOption.Create(80, CustomOption.CustomOptionType.Crewmate, CustomOptionHolder.cs(TheOtherRoles.Mayor.color, ModTranslation.GetString("Mayor", null)), CustomOptionHolder.rates, null, true, null, "");
			CustomOptionHolder.mayorCanSeeVoteColors = CustomOption.Create(81, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("mayorCanSeeVoteColors", null), false, CustomOptionHolder.mayorSpawnRate, false, null, "");
			CustomOptionHolder.mayorTasksNeededToSeeVoteColors = CustomOption.Create(82, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("mayorTasksNeededToSeeVoteColors", null), 5f, 0f, 20f, 1f, CustomOptionHolder.mayorCanSeeVoteColors, false, null, "");
			CustomOptionHolder.mayorMeetingButton = CustomOption.Create(83, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("mayorMeetingButton", null), true, CustomOptionHolder.mayorSpawnRate, false, null, "");
			CustomOptionHolder.mayorMaxRemoteMeetings = CustomOption.Create(84, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("mayorMaxRemoteMeetings", null), 1f, 1f, 5f, 1f, CustomOptionHolder.mayorMeetingButton, false, null, "");
			CustomOptionHolder.mayorChooseSingleVote = CustomOption.Create(85, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("mayorChooseSingleVote", null), new string[]
			{
				ModTranslation.GetString("optionOff", null),
				ModTranslation.GetString("OnBoforeVoting", null),
				ModTranslation.GetString("OnUntilMeetingEnds", null)
			}, CustomOptionHolder.mayorSpawnRate, false, null, "");
			CustomOptionHolder.engineerSpawnRate = CustomOption.Create(90, CustomOption.CustomOptionType.Crewmate, CustomOptionHolder.cs(TheOtherRoles.Engineer.color, ModTranslation.GetString("Engineer", null)), CustomOptionHolder.rates, null, true, null, "");
			CustomOptionHolder.engineerNumberOfFixes = CustomOption.Create(91, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("engineerNumberOfFixes", null), 1f, 1f, 3f, 1f, CustomOptionHolder.engineerSpawnRate, false, null, "");
			CustomOptionHolder.engineerHighlightForImpostors = CustomOption.Create(92, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("engineerHighlightForImpostors", null), true, CustomOptionHolder.engineerSpawnRate, false, null, "");
			CustomOptionHolder.engineerHighlightForTeamJackal = CustomOption.Create(93, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("engineerHighlightForTeamJackal", null), true, CustomOptionHolder.engineerSpawnRate, false, null, "");
			CustomOptionHolder.sheriffSpawnRate = CustomOption.Create(100, CustomOption.CustomOptionType.Crewmate, CustomOptionHolder.cs(TheOtherRoles.Sheriff.color, ModTranslation.GetString("Sheriff", null)), CustomOptionHolder.rates, null, true, null, "");
			CustomOptionHolder.sheriffCooldown = CustomOption.Create(101, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("sheriffCooldown", null), 30f, 10f, 60f, 2.5f, CustomOptionHolder.sheriffSpawnRate, false, null, "");
			CustomOptionHolder.sheriffCanKillNeutrals = CustomOption.Create(102, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("sheriffCanKillNeutrals", null), false, CustomOptionHolder.sheriffSpawnRate, false, null, "");
			CustomOptionHolder.deputySpawnRate = CustomOption.Create(103, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("Deputy", null), CustomOptionHolder.rates, CustomOptionHolder.sheriffSpawnRate, false, null, "");
			CustomOptionHolder.deputyNumberOfHandcuffs = CustomOption.Create(104, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("deputyNumberOfHandcuffs", null), 3f, 1f, 10f, 1f, CustomOptionHolder.deputySpawnRate, false, null, "");
			CustomOptionHolder.deputyHandcuffCooldown = CustomOption.Create(105, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("deputyHandcuffCooldown", null), 30f, 10f, 60f, 2.5f, CustomOptionHolder.deputySpawnRate, false, null, "");
			CustomOptionHolder.deputyHandcuffDuration = CustomOption.Create(106, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("deputyHandcuffDuration", null), 15f, 5f, 60f, 2.5f, CustomOptionHolder.deputySpawnRate, false, null, "");
			CustomOptionHolder.deputyKnowsSheriff = CustomOption.Create(107, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("deputyKnowsSheriff", null), true, CustomOptionHolder.deputySpawnRate, false, null, "");
			CustomOptionHolder.deputyGetsPromoted = CustomOption.Create(108, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("deputyGetsPromoted", null), new string[]
			{
				ModTranslation.GetString("optionOff", null),
				ModTranslation.GetString("OnImmediately", null),
				ModTranslation.GetString("OnAfterMeeting", null)
			}, CustomOptionHolder.deputySpawnRate, false, null, "");
			CustomOptionHolder.deputyKeepsHandcuffs = CustomOption.Create(109, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("deputyKeepsHandcuffs", null), true, CustomOptionHolder.deputyGetsPromoted, false, null, "");
			CustomOptionHolder.lighterSpawnRate = CustomOption.Create(110, CustomOption.CustomOptionType.Crewmate, CustomOptionHolder.cs(TheOtherRoles.Lighter.color, ModTranslation.GetString("Lighter", null)), CustomOptionHolder.rates, null, true, null, "");
			CustomOptionHolder.lighterModeLightsOnVision = CustomOption.Create(111, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("lighterModeLightsOnVision", null), 1.5f, 0.25f, 5f, 0.25f, CustomOptionHolder.lighterSpawnRate, false, null, "");
			CustomOptionHolder.lighterModeLightsOffVision = CustomOption.Create(112, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("lighterModeLightsOffVision", null), 0.5f, 0.25f, 5f, 0.25f, CustomOptionHolder.lighterSpawnRate, false, null, "");
			CustomOptionHolder.lighterFlashlightWidth = CustomOption.Create(113, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("lighterFlashlightWidth", null), 0.3f, 0.1f, 1f, 0.1f, CustomOptionHolder.lighterSpawnRate, false, null, "");
			CustomOptionHolder.detectiveSpawnRate = CustomOption.Create(120, CustomOption.CustomOptionType.Crewmate, CustomOptionHolder.cs(TheOtherRoles.Detective.color, ModTranslation.GetString("Detective", null)), CustomOptionHolder.rates, null, true, null, "");
			CustomOptionHolder.detectiveAnonymousFootprints = CustomOption.Create(121, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("detectiveAnonymousFootprints", null), false, CustomOptionHolder.detectiveSpawnRate, false, null, "");
			CustomOptionHolder.detectiveFootprintIntervall = CustomOption.Create(122, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("detectiveFootprintIntervall", null), 0.5f, 0.25f, 10f, 0.25f, CustomOptionHolder.detectiveSpawnRate, false, null, "");
			CustomOptionHolder.detectiveFootprintDuration = CustomOption.Create(123, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("detectiveFootprintDuration", null), 5f, 0.25f, 10f, 0.25f, CustomOptionHolder.detectiveSpawnRate, false, null, "");
			CustomOptionHolder.detectiveReportNameDuration = CustomOption.Create(124, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("detectiveReportNameDuration", null), 0f, 0f, 60f, 2.5f, CustomOptionHolder.detectiveSpawnRate, false, null, "");
			CustomOptionHolder.detectiveReportColorDuration = CustomOption.Create(125, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("detectiveReportColorDuration", null), 20f, 0f, 120f, 2.5f, CustomOptionHolder.detectiveSpawnRate, false, null, "");
			CustomOptionHolder.timeMasterSpawnRate = CustomOption.Create(130, CustomOption.CustomOptionType.Crewmate, CustomOptionHolder.cs(TimeMaster.color, ModTranslation.GetString("TimeMaster", null)), CustomOptionHolder.rates, null, true, null, "");
			CustomOptionHolder.timeMasterCooldown = CustomOption.Create(131, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("timeMasterCooldown", null), 30f, 10f, 120f, 2.5f, CustomOptionHolder.timeMasterSpawnRate, false, null, "");
			CustomOptionHolder.timeMasterRewindTime = CustomOption.Create(132, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("timeMasterRewindTime", null), 3f, 1f, 10f, 1f, CustomOptionHolder.timeMasterSpawnRate, false, null, "");
			CustomOptionHolder.timeMasterShieldDuration = CustomOption.Create(133, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("timeMasterShieldDuration", null), 3f, 1f, 20f, 1f, CustomOptionHolder.timeMasterSpawnRate, false, null, "");
			CustomOptionHolder.medicSpawnRate = CustomOption.Create(140, CustomOption.CustomOptionType.Crewmate, CustomOptionHolder.cs(Medic.color, ModTranslation.GetString("Medic", null)), CustomOptionHolder.rates, null, true, null, "");
			CustomOptionHolder.medicShowShielded = CustomOption.Create(143, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("medicShowShielded", null), new string[]
			{
				ModTranslation.GetString("evone", null),
				ModTranslation.GetString("evtwo", null),
				ModTranslation.GetString("jstone", null)
			}, CustomOptionHolder.medicSpawnRate, false, null, "");
			CustomOptionHolder.medicShowAttemptToShielded = CustomOption.Create(144, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("medicShowAttemptToShielded", null), false, CustomOptionHolder.medicSpawnRate, false, null, "");
			CustomOptionHolder.medicSetOrShowShieldAfterMeeting = CustomOption.Create(145, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("medicSetOrShowShieldAfterMeeting", null), new string[]
			{
				ModTranslation.GetString("OnImmediately", null),
				ModTranslation.GetString("OnImmediatelyOnAfterMeeting", null),
				ModTranslation.GetString("OnAfterMeeting", null)
			}, CustomOptionHolder.medicSpawnRate, false, null, "");
			CustomOptionHolder.medicShowAttemptToMedic = CustomOption.Create(146, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("medicShowAttemptToMedic", null), false, CustomOptionHolder.medicSpawnRate, false, null, "");
			CustomOptionHolder.swapperSpawnRate = CustomOption.Create(150, CustomOption.CustomOptionType.Crewmate, CustomOptionHolder.cs(Swapper.color, ModTranslation.GetString("Swapper", null)), CustomOptionHolder.rates, null, true, null, "");
			CustomOptionHolder.swapperCanCallEmergency = CustomOption.Create(151, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("swapperCanCallEmergency", null), false, CustomOptionHolder.swapperSpawnRate, false, null, "");
			CustomOptionHolder.swapperCanOnlySwapOthers = CustomOption.Create(152, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("swapperCanOnlySwapOthers", null), false, CustomOptionHolder.swapperSpawnRate, false, null, "");
			CustomOptionHolder.swapperSwapsNumber = CustomOption.Create(153, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("swapperSwapsNumber", null), 1f, 0f, 5f, 1f, CustomOptionHolder.swapperSpawnRate, false, null, "");
			CustomOptionHolder.swapperRechargeTasksNumber = CustomOption.Create(154, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("swapperRechargeTasksNumber", null), 2f, 1f, 10f, 1f, CustomOptionHolder.swapperSpawnRate, false, null, "");
			CustomOptionHolder.seerSpawnRate = CustomOption.Create(160, CustomOption.CustomOptionType.Crewmate, CustomOptionHolder.cs(Seer.color, ModTranslation.GetString("Seer", null)), CustomOptionHolder.rates, null, true, null, "");
			CustomOptionHolder.seerMode = CustomOption.Create(161, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("seerMode", null), new string[]
			{
				ModTranslation.GetString("FlashSoul", null),
				ModTranslation.GetString("Flash", null),
				ModTranslation.GetString("Soul", null)
			}, CustomOptionHolder.seerSpawnRate, false, null, "");
			CustomOptionHolder.seerLimitSoulDuration = CustomOption.Create(163, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("seerLimitSoulDuration", null), false, CustomOptionHolder.seerSpawnRate, false, null, "");
			CustomOptionHolder.seerSoulDuration = CustomOption.Create(162, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("seerSoulDuration", null), 15f, 0f, 120f, 5f, CustomOptionHolder.seerLimitSoulDuration, false, null, "");
			CustomOptionHolder.hackerSpawnRate = CustomOption.Create(170, CustomOption.CustomOptionType.Crewmate, CustomOptionHolder.cs(Hacker.color, ModTranslation.GetString("Hacker", null)), CustomOptionHolder.rates, null, true, null, "");
			CustomOptionHolder.hackerCooldown = CustomOption.Create(171, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("hackerCooldown", null), 30f, 5f, 60f, 5f, CustomOptionHolder.hackerSpawnRate, false, null, "");
			CustomOptionHolder.hackerHackeringDuration = CustomOption.Create(172, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("hackerHackeringDuration", null), 10f, 2.5f, 60f, 2.5f, CustomOptionHolder.hackerSpawnRate, false, null, "");
			CustomOptionHolder.hackerOnlyColorType = CustomOption.Create(173, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("hackerOnlyColorType", null), false, CustomOptionHolder.hackerSpawnRate, false, null, "");
			CustomOptionHolder.hackerToolsNumber = CustomOption.Create(174, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("hackerToolsNumber", null), 5f, 1f, 30f, 1f, CustomOptionHolder.hackerSpawnRate, false, null, "");
			CustomOptionHolder.hackerRechargeTasksNumber = CustomOption.Create(175, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("hackerRechargeTasksNumber", null), 2f, 1f, 5f, 1f, CustomOptionHolder.hackerSpawnRate, false, null, "");
			CustomOptionHolder.hackerNoMove = CustomOption.Create(176, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("hackerNoMove", null), true, CustomOptionHolder.hackerSpawnRate, false, null, "");
			CustomOptionHolder.trackerSpawnRate = CustomOption.Create(200, CustomOption.CustomOptionType.Crewmate, CustomOptionHolder.cs(Tracker.color, ModTranslation.GetString("Tracker", null)), CustomOptionHolder.rates, null, true, null, "");
			CustomOptionHolder.trackerUpdateIntervall = CustomOption.Create(201, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("trackerUpdateIntervall", null), 5f, 1f, 30f, 1f, CustomOptionHolder.trackerSpawnRate, false, null, "");
			CustomOptionHolder.trackerResetTargetAfterMeeting = CustomOption.Create(202, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("trackerResetTargetAfterMeeting", null), false, CustomOptionHolder.trackerSpawnRate, false, null, "");
			CustomOptionHolder.trackerCanTrackCorpses = CustomOption.Create(203, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("trackerCanTrackCorpses", null), true, CustomOptionHolder.trackerSpawnRate, false, null, "");
			CustomOptionHolder.trackerCorpsesTrackingCooldown = CustomOption.Create(204, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("trackerCorpsesTrackingCooldown", null), 30f, 5f, 120f, 5f, CustomOptionHolder.trackerCanTrackCorpses, false, null, "");
			CustomOptionHolder.trackerCorpsesTrackingDuration = CustomOption.Create(205, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("trackerCorpsesTrackingDuration", null), 5f, 2.5f, 30f, 2.5f, CustomOptionHolder.trackerCanTrackCorpses, false, null, "");
			CustomOptionHolder.trackerTrackingMethod = CustomOption.Create(206, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("trackerTrackingMethod", null), new string[]
			{
				ModTranslation.GetString("Arrow", null),
				ModTranslation.GetString("Checker", null),
				ModTranslation.GetString("ArrowCheck", null)
			}, CustomOptionHolder.trackerSpawnRate, false, null, "");
			CustomOptionHolder.snitchSpawnRate = CustomOption.Create(210, CustomOption.CustomOptionType.Crewmate, CustomOptionHolder.cs(Snitch.color, ModTranslation.GetString("Snitch", null)), CustomOptionHolder.rates, null, true, null, "");
			CustomOptionHolder.snitchLeftTasksForReveal = CustomOption.Create(219, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("snitchLeftTasksForReveal", null), 5f, 0f, 25f, 1f, CustomOptionHolder.snitchSpawnRate, false, null, "");
			CustomOptionHolder.snitchMode = CustomOption.Create(211, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("snitchMode", null), new string[]
			{
				ModTranslation.GetString("Chat", null),
				ModTranslation.GetString("Map", null),
				ModTranslation.GetString("ChatMap", null)
			}, CustomOptionHolder.snitchSpawnRate, false, null, "");
			CustomOptionHolder.snitchTargets = CustomOption.Create(212, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("snitchTargets", null), new string[]
			{
				ModTranslation.GetString("evilPlayer", null),
				ModTranslation.GetString("killerPlayer", null)
			}, CustomOptionHolder.snitchSpawnRate, false, null, "");
			CustomOptionHolder.spySpawnRate = CustomOption.Create(240, CustomOption.CustomOptionType.Crewmate, CustomOptionHolder.cs(Spy.color, ModTranslation.GetString("Spy", null)), CustomOptionHolder.rates, null, true, null, "");
			CustomOptionHolder.spyCanDieToSheriff = CustomOption.Create(241, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("spyCanDieToSheriff", null), false, CustomOptionHolder.spySpawnRate, false, null, "");
			CustomOptionHolder.spyImpostorsCanKillAnyone = CustomOption.Create(242, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("spyImpostorsCanKillAnyone", null), true, CustomOptionHolder.spySpawnRate, false, null, "");
			CustomOptionHolder.spyCanEnterVents = CustomOption.Create(243, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("spyCanEnterVents", null), false, CustomOptionHolder.spySpawnRate, false, null, "");
			CustomOptionHolder.spyHasImpostorVision = CustomOption.Create(244, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("spyHasImpostorVision", null), false, CustomOptionHolder.spySpawnRate, false, null, "");
			CustomOptionHolder.portalmakerSpawnRate = CustomOption.Create(390, CustomOption.CustomOptionType.Crewmate, CustomOptionHolder.cs(TheOtherRoles.Portalmaker.color, ModTranslation.GetString("Portalmaker", null)), CustomOptionHolder.rates, null, true, null, "");
			CustomOptionHolder.portalmakerCooldown = CustomOption.Create(391, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("portalmakerCooldown", null), 30f, 10f, 60f, 2.5f, CustomOptionHolder.portalmakerSpawnRate, false, null, "");
			CustomOptionHolder.portalmakerUsePortalCooldown = CustomOption.Create(392, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("portalmakerUsePortalCooldown", null), 30f, 10f, 60f, 2.5f, CustomOptionHolder.portalmakerSpawnRate, false, null, "");
			CustomOptionHolder.portalmakerLogOnlyColorType = CustomOption.Create(393, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("portalmakerLogOnlyColorType", null), true, CustomOptionHolder.portalmakerSpawnRate, false, null, "");
			CustomOptionHolder.portalmakerLogHasTime = CustomOption.Create(394, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("portalmakerLogHasTime", null), true, CustomOptionHolder.portalmakerSpawnRate, false, null, "");
			CustomOptionHolder.portalmakerCanPortalFromAnywhere = CustomOption.Create(395, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("portalmakerCanPortalFromAnywhere", null), true, CustomOptionHolder.portalmakerSpawnRate, false, null, "");
			CustomOptionHolder.securityGuardSpawnRate = CustomOption.Create(280, CustomOption.CustomOptionType.Crewmate, CustomOptionHolder.cs(SecurityGuard.color, ModTranslation.GetString("SecurityGuard", null)), CustomOptionHolder.rates, null, true, null, "");
			CustomOptionHolder.securityGuardCooldown = CustomOption.Create(281, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("securityGuardCooldown", null), 30f, 10f, 60f, 2.5f, CustomOptionHolder.securityGuardSpawnRate, false, null, "");
			CustomOptionHolder.securityGuardTotalScrews = CustomOption.Create(282, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("securityGuardTotalScrews", null), 7f, 1f, 15f, 1f, CustomOptionHolder.securityGuardSpawnRate, false, null, "");
			CustomOptionHolder.securityGuardCamPrice = CustomOption.Create(283, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("securityGuardCamPrice", null), 2f, 1f, 15f, 1f, CustomOptionHolder.securityGuardSpawnRate, false, null, "");
			CustomOptionHolder.securityGuardVentPrice = CustomOption.Create(284, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("securityGuardVentPrice", null), 1f, 1f, 15f, 1f, CustomOptionHolder.securityGuardSpawnRate, false, null, "");
			CustomOptionHolder.securityGuardCamDuration = CustomOption.Create(285, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("securityGuardCamDuration", null), 10f, 2.5f, 60f, 2.5f, CustomOptionHolder.securityGuardSpawnRate, false, null, "");
			CustomOptionHolder.securityGuardCamMaxCharges = CustomOption.Create(286, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("securityGuardCamMaxCharges", null), 5f, 1f, 30f, 1f, CustomOptionHolder.securityGuardSpawnRate, false, null, "");
			CustomOptionHolder.securityGuardCamRechargeTasksNumber = CustomOption.Create(287, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("securityGuardCamRechargeTasksNumber", null), 3f, 1f, 10f, 1f, CustomOptionHolder.securityGuardSpawnRate, false, null, "");
			CustomOptionHolder.securityGuardNoMove = CustomOption.Create(288, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("securityGuardNoMove", null), true, CustomOptionHolder.securityGuardSpawnRate, false, null, "");
			CustomOptionHolder.mediumSpawnRate = CustomOption.Create(360, CustomOption.CustomOptionType.Crewmate, CustomOptionHolder.cs(Medium.color, ModTranslation.GetString("Medium", null)), CustomOptionHolder.rates, null, true, null, "");
			CustomOptionHolder.mediumCooldown = CustomOption.Create(361, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("mediumCooldown", null), 30f, 5f, 120f, 5f, CustomOptionHolder.mediumSpawnRate, false, null, "");
			CustomOptionHolder.mediumDuration = CustomOption.Create(362, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("mediumDuration", null), 3f, 0f, 15f, 1f, CustomOptionHolder.mediumSpawnRate, false, null, "");
			CustomOptionHolder.mediumOneTimeUse = CustomOption.Create(363, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("mediumOneTimeUse", null), false, CustomOptionHolder.mediumSpawnRate, false, null, "");
			CustomOptionHolder.mediumChanceAdditionalInfo = CustomOption.Create(364, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("mediumChanceAdditionalInfo", null), CustomOptionHolder.rates, CustomOptionHolder.mediumSpawnRate, false, null, "");
			CustomOptionHolder.prophetSpawnRate = CustomOption.Create(9005, CustomOption.CustomOptionType.Crewmate, CustomOptionHolder.cs(Prophet.color, "Prophet".Translate()), CustomOptionHolder.rates, null, true, null, "");
			CustomOptionHolder.prophetCooldown = CustomOption.Create(9011, CustomOption.CustomOptionType.Crewmate, "prophetCooldown".Translate(), 30f, 5f, 60f, 1f, CustomOptionHolder.prophetSpawnRate, false, null, "");
			CustomOptionHolder.prophetNumExamines = CustomOption.Create(9006, CustomOption.CustomOptionType.Crewmate, "prophetNumExamines".Translate(), 4f, 1f, 10f, 1f, CustomOptionHolder.prophetSpawnRate, false, null, "");
			CustomOptionHolder.prophetCanCallEmergency = CustomOption.Create(9007, CustomOption.CustomOptionType.Crewmate, "prophetCanCallEmergency".Translate(), false, CustomOptionHolder.prophetSpawnRate, false, null, "");
			CustomOptionHolder.prophetIsRevealed = CustomOption.Create(9012, CustomOption.CustomOptionType.Crewmate, "prophetIsRevealed".Translate(), true, CustomOptionHolder.prophetSpawnRate, false, null, "");
			CustomOptionHolder.prophetExaminesToBeRevealed = CustomOption.Create(9008, CustomOption.CustomOptionType.Crewmate, "prophetExaminesToBeRevealed".Translate(), 3f, 1f, 10f, 1f, CustomOptionHolder.prophetIsRevealed, false, null, "");
			CustomOptionHolder.prophetAccuracy = CustomOption.Create(9009, CustomOption.CustomOptionType.Crewmate, "prophetAccuracy".Translate(), 30f, 0f, 100f, 10f, CustomOptionHolder.prophetSpawnRate, false, null, "");
			CustomOptionHolder.thiefSpawnRate = CustomOption.Create(400, CustomOption.CustomOptionType.Neutral, CustomOptionHolder.cs(Thief.color, ModTranslation.GetString("Thief", null)), CustomOptionHolder.rates, null, true, null, "");
			CustomOptionHolder.thiefCooldown = CustomOption.Create(401, CustomOption.CustomOptionType.Neutral, ModTranslation.GetString("thiefCooldown", null), 30f, 5f, 120f, 5f, CustomOptionHolder.thiefSpawnRate, false, null, "");
			CustomOptionHolder.thiefCanKillSheriff = CustomOption.Create(402, CustomOption.CustomOptionType.Neutral, ModTranslation.GetString("thiefCanKillSheriff", null), true, CustomOptionHolder.thiefSpawnRate, false, null, "");
			CustomOptionHolder.thiefHasImpVision = CustomOption.Create(403, CustomOption.CustomOptionType.Neutral, ModTranslation.GetString("thiefHasImpVision", null), true, CustomOptionHolder.thiefSpawnRate, false, null, "");
			CustomOptionHolder.thiefCanUseVents = CustomOption.Create(404, CustomOption.CustomOptionType.Neutral, ModTranslation.GetString("thiefCanUseVents", null), true, CustomOptionHolder.thiefSpawnRate, false, null, "");
			CustomOptionHolder.thiefCanStealWithGuess = CustomOption.Create(405, CustomOption.CustomOptionType.Neutral, ModTranslation.GetString("thiefCanStealWithGuess", null), false, CustomOptionHolder.thiefSpawnRate, false, null, "");
			CustomOptionHolder.trapperSpawnRate = CustomOption.Create(410, CustomOption.CustomOptionType.Crewmate, CustomOptionHolder.cs(Trapper.color, ModTranslation.GetString("Trapper", null)), CustomOptionHolder.rates, null, true, null, "");
			CustomOptionHolder.trapperCooldown = CustomOption.Create(420, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("trapperCooldown", null), 30f, 5f, 120f, 5f, CustomOptionHolder.trapperSpawnRate, false, null, "");
			CustomOptionHolder.trapperMaxCharges = CustomOption.Create(440, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("trapperMaxCharges", null), 5f, 1f, 15f, 1f, CustomOptionHolder.trapperSpawnRate, false, null, "");
			CustomOptionHolder.trapperRechargeTasksNumber = CustomOption.Create(450, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("trapperRechargeTasksNumber", null), 2f, 1f, 15f, 1f, CustomOptionHolder.trapperSpawnRate, false, null, "");
			CustomOptionHolder.trapperTrapNeededTriggerToReveal = CustomOption.Create(451, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("trapperTrapNeededTriggerToReveal", null), 3f, 2f, 10f, 1f, CustomOptionHolder.trapperSpawnRate, false, null, "");
			CustomOptionHolder.trapperAnonymousMap = CustomOption.Create(452, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("trapperAnonymousMap", null), false, CustomOptionHolder.trapperSpawnRate, false, null, "");
			CustomOptionHolder.trapperInfoType = CustomOption.Create(453, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("trapperInfoType", null), new string[]
			{
				ModTranslation.GetString("roles", null),
				ModTranslation.GetString("CIRoles", null),
				ModTranslation.GetString("Names", null)
			}, CustomOptionHolder.trapperSpawnRate, false, null, "");
			CustomOptionHolder.trapperTrapDuration = CustomOption.Create(454, CustomOption.CustomOptionType.Crewmate, ModTranslation.GetString("trapperTrapDuration", null), 5f, 1f, 15f, 1f, CustomOptionHolder.trapperSpawnRate, false, null, "");
			CustomOptionHolder.modifiersAreHidden = CustomOption.Create(1009, CustomOption.CustomOptionType.Modifier, CustomOptionHolder.cs(Color.yellow, ModTranslation.GetString("modifiersAreHidden", null)), true, null, true, null, CustomOptionHolder.cs(Color.yellow, ModTranslation.GetString("modifierSettings", null)));
			CustomOptionHolder.modifierBloody = CustomOption.Create(1000, CustomOption.CustomOptionType.Modifier, CustomOptionHolder.cs(Color.yellow, ModTranslation.GetString("Bloody", null)), CustomOptionHolder.rates, null, true, null, "");
			CustomOptionHolder.modifierBloodyQuantity = CustomOption.Create(1001, CustomOption.CustomOptionType.Modifier, ModTranslation.GetString("modifierBloodyQuantity", null), CustomOptionHolder.ratesModifier, CustomOptionHolder.modifierBloody, false, null, "");
			CustomOptionHolder.modifierBloodyDuration = CustomOption.Create(1002, CustomOption.CustomOptionType.Modifier, ModTranslation.GetString("modifierBloodyDuration", null), 10f, 3f, 60f, 1f, CustomOptionHolder.modifierBloody, false, null, "");
			CustomOptionHolder.modifierAntiTeleport = CustomOption.Create(1010, CustomOption.CustomOptionType.Modifier, CustomOptionHolder.cs(Color.yellow, ModTranslation.GetString("AntiTeleport", null)), CustomOptionHolder.rates, null, true, null, "");
			CustomOptionHolder.modifierAntiTeleportQuantity = CustomOption.Create(1011, CustomOption.CustomOptionType.Modifier, ModTranslation.GetString("modifierAntiTeleportQuantity", null), CustomOptionHolder.ratesModifier, CustomOptionHolder.modifierAntiTeleport, false, null, "");
			CustomOptionHolder.modifierTieBreaker = CustomOption.Create(1020, CustomOption.CustomOptionType.Modifier, CustomOptionHolder.cs(Color.yellow, ModTranslation.GetString("TieBreaker", null)), CustomOptionHolder.rates, null, true, null, "");
			CustomOptionHolder.modifierBait = CustomOption.Create(1030, CustomOption.CustomOptionType.Modifier, CustomOptionHolder.cs(Color.yellow, ModTranslation.GetString("Bait", null)), CustomOptionHolder.rates, null, true, null, "");
			CustomOptionHolder.modifierBaitQuantity = CustomOption.Create(1031, CustomOption.CustomOptionType.Modifier, ModTranslation.GetString("modifierBaitQuantity", null), CustomOptionHolder.ratesModifier, CustomOptionHolder.modifierBait, false, null, "");
			CustomOptionHolder.modifierBaitReportDelayMin = CustomOption.Create(1032, CustomOption.CustomOptionType.Modifier, ModTranslation.GetString("modifierBaitReportDelayMin", null), 0f, 0f, 10f, 1f, CustomOptionHolder.modifierBait, false, null, "");
			CustomOptionHolder.modifierBaitReportDelayMax = CustomOption.Create(1033, CustomOption.CustomOptionType.Modifier, ModTranslation.GetString("modifierBaitReportDelayMax", null), 0f, 0f, 10f, 1f, CustomOptionHolder.modifierBait, false, null, "");
			CustomOptionHolder.modifierBaitShowKillFlash = CustomOption.Create(1034, CustomOption.CustomOptionType.Modifier, ModTranslation.GetString("modifierBaitShowKillFlash", null), true, CustomOptionHolder.modifierBait, false, null, "");
			CustomOptionHolder.modifierLover = CustomOption.Create(1040, CustomOption.CustomOptionType.Modifier, CustomOptionHolder.cs(Color.yellow, ModTranslation.GetString("Lover", null)), CustomOptionHolder.rates, null, true, null, "");
			CustomOptionHolder.modifierLoverImpLoverRate = CustomOption.Create(1041, CustomOption.CustomOptionType.Modifier, ModTranslation.GetString("modifierLoverImpLoverRate", null), CustomOptionHolder.rates, CustomOptionHolder.modifierLover, false, null, "");
			CustomOptionHolder.modifierLoverBothDie = CustomOption.Create(1042, CustomOption.CustomOptionType.Modifier, ModTranslation.GetString("modifierLoverBothDie", null), true, CustomOptionHolder.modifierLover, false, null, "");
			CustomOptionHolder.modifierLoverEnableChat = CustomOption.Create(1043, CustomOption.CustomOptionType.Modifier, ModTranslation.GetString("modifierLoverEnableChat", null), true, CustomOptionHolder.modifierLover, false, null, "");
            CustomOptionHolder.modifierLastImpostor = CustomOption.Create(1053, CustomOption.CustomOptionType.Modifier, CustomOptionHolder.cs(Color.yellow, ModTranslation.GetString("LastImpostor", null)), CustomOptionHolder.rates, null, true, null, "");
            CustomOptionHolder.modifierSunglasses = CustomOption.Create(1050, CustomOption.CustomOptionType.Modifier, CustomOptionHolder.cs(Color.yellow, ModTranslation.GetString("Sunglasses", null)), CustomOptionHolder.rates, null, true, null, "");
			CustomOptionHolder.modifierSunglassesQuantity = CustomOption.Create(1051, CustomOption.CustomOptionType.Modifier, ModTranslation.GetString("modifierSunglassesQuantity", null), CustomOptionHolder.ratesModifier, CustomOptionHolder.modifierSunglasses, false, null, "");
			CustomOptionHolder.modifierSunglassesVision = CustomOption.Create(1052, CustomOption.CustomOptionType.Modifier, ModTranslation.GetString("modifierSunglassesVision", null), new string[]

			{
				"-10%",
				"-20%",
				"-30%",
				"-40%",
				"-50%"
			}, CustomOptionHolder.modifierSunglasses, false, null, "");
			CustomOptionHolder.modifierLighterln = CustomOption.Create(40180, CustomOption.CustomOptionType.Modifier, CustomOptionHolder.cs(Color.yellow, ModTranslation.GetString("Lighterln", null)), CustomOptionHolder.rates, null, true, null, "");
			CustomOptionHolder.modifierMini = CustomOption.Create(1061, CustomOption.CustomOptionType.Modifier, CustomOptionHolder.cs(Color.yellow, ModTranslation.GetString("Mini", null)), CustomOptionHolder.rates, null, true, null, "");
			CustomOptionHolder.modifierMiniGrowingUpDuration = CustomOption.Create(1062, CustomOption.CustomOptionType.Modifier, ModTranslation.GetString("modifierMiniGrowingUpDuration", null), 400f, 100f, 1500f, 100f, CustomOptionHolder.modifierMini, false, null, "");
			CustomOptionHolder.modifierMiniGrowingUpInMeeting = CustomOption.Create(1063, CustomOption.CustomOptionType.Modifier, ModTranslation.GetString("modifierMiniGrowingUpInMeeting", null), true, CustomOptionHolder.modifierMini, false, null, "");
			CustomOptionHolder.modifierVip = CustomOption.Create(1070, CustomOption.CustomOptionType.Modifier, CustomOptionHolder.cs(Color.yellow, ModTranslation.GetString("Vip", null)), CustomOptionHolder.rates, null, true, null, "");
			CustomOptionHolder.modifierVipQuantity = CustomOption.Create(1071, CustomOption.CustomOptionType.Modifier, ModTranslation.GetString("modifierVipQuantity", null), CustomOptionHolder.ratesModifier, CustomOptionHolder.modifierVip, false, null, "");
			CustomOptionHolder.modifierVipShowColor = CustomOption.Create(1072, CustomOption.CustomOptionType.Modifier, ModTranslation.GetString("modifierVipShowColor", null), true, CustomOptionHolder.modifierVip, false, null, "");
			CustomOptionHolder.modifierInvert = CustomOption.Create(1080, CustomOption.CustomOptionType.Modifier, CustomOptionHolder.cs(Color.yellow, ModTranslation.GetString("Invert", null)), CustomOptionHolder.rates, null, true, null, "");
			CustomOptionHolder.modifierInvertQuantity = CustomOption.Create(1081, CustomOption.CustomOptionType.Modifier, ModTranslation.GetString("modifierInvertQuantity", null), CustomOptionHolder.ratesModifier, CustomOptionHolder.modifierInvert, false, null, "");
			CustomOptionHolder.modifierInvertDuration = CustomOption.Create(1082, CustomOption.CustomOptionType.Modifier, ModTranslation.GetString("modifierInvertDuration", null), 3f, 1f, 15f, 1f, CustomOptionHolder.modifierInvert, false, null, "");
			CustomOptionHolder.modifierChameleon = CustomOption.Create(1090, CustomOption.CustomOptionType.Modifier, CustomOptionHolder.cs(Color.yellow, ModTranslation.GetString("Chameleon", null)), CustomOptionHolder.rates, null, true, null, "");
			CustomOptionHolder.modifierChameleonQuantity = CustomOption.Create(1091, CustomOption.CustomOptionType.Modifier, ModTranslation.GetString("modifierChameleonQuantity", null), CustomOptionHolder.ratesModifier, CustomOptionHolder.modifierChameleon, false, null, "");
			CustomOptionHolder.modifierChameleonHoldDuration = CustomOption.Create(1092, CustomOption.CustomOptionType.Modifier, ModTranslation.GetString("modifierChameleonHoldDuration", null), 3f, 1f, 10f, 0.5f, CustomOptionHolder.modifierChameleon, false, null, "");
			CustomOptionHolder.modifierChameleonFadeDuration = CustomOption.Create(1093, CustomOption.CustomOptionType.Modifier, ModTranslation.GetString("modifierChameleonFadeDuration", null), 1f, 0.25f, 10f, 0.25f, CustomOptionHolder.modifierChameleon, false, null, "");
			CustomOptionHolder.modifierChameleonMinVisibility = CustomOption.Create(1094, CustomOption.CustomOptionType.Modifier, ModTranslation.GetString("modifierChameleonMinVisibility", null), new string[]
			{
				"0%",
				"10%",
				"20%",
				"30%",
				"40%",
				"50%"
			}, CustomOptionHolder.modifierChameleon, false, null, "");
			CustomOptionHolder.modifierShifter = CustomOption.Create(1100, CustomOption.CustomOptionType.Modifier, CustomOptionHolder.cs(Color.yellow, ModTranslation.GetString("Shifter", null)), CustomOptionHolder.rates, null, true, null, "");
			CustomOptionHolder.guesserGamemodeCrewNumber = CustomOption.Create(2001, CustomOption.CustomOptionType.Guesser, CustomOptionHolder.cs(Guesser.color, "guesserGamemodeCrewNumber".Translate()), 15f, 1f, 15f, 1f, null, true, null, "guesserGamemodeCrewNumber".Translate());
			CustomOptionHolder.guesserGamemodeNeutralNumber = CustomOption.Create(2002, CustomOption.CustomOptionType.Guesser, CustomOptionHolder.cs(Guesser.color, "guesserGamemodeNeutralNumber".Translate()), 15f, 1f, 15f, 1f, null, false, null, "");
			CustomOptionHolder.guesserGamemodeImpNumber = CustomOption.Create(2003, CustomOption.CustomOptionType.Guesser, CustomOptionHolder.cs(Guesser.color, "guesserGamemodeImpNumber".Translate()), 15f, 1f, 15f, 1f, null, false, null, "");
			CustomOptionHolder.guesserForceJackalGuesser = CustomOption.Create(2007, CustomOption.CustomOptionType.Guesser, "guesserForceJackalGuesser".Translate(), false, null, true, null, "guesserForceJackalGuesser".Translate());
			CustomOptionHolder.guesserGamemodeSidekickIsAlwaysGuesser = CustomOption.Create(2012, CustomOption.CustomOptionType.Guesser, "guesserGamemodeSidekickIsAlwaysGuesser".Translate(), false, null, false, null, "");
			CustomOptionHolder.guesserForceThiefGuesser = CustomOption.Create(2011, CustomOption.CustomOptionType.Guesser, "guesserForceThiefGuesser".Translate(), false, null, false, null, "");
			CustomOptionHolder.guesserGamemodeHaveModifier = CustomOption.Create(2004, CustomOption.CustomOptionType.Guesser, "guesserGamemodeHaveModifier".Translate(), true, null, true, null, "GuesserModeClassicSettings".Translate());
			CustomOptionHolder.guesserGamemodeNumberOfShots = CustomOption.Create(2005, CustomOption.CustomOptionType.Guesser, "guesserGamemodeNumberOfShots".Translate(), 3f, 1f, 15f, 1f, null, false, null, "");
			CustomOptionHolder.guesserGamemodeHasMultipleShotsPerMeeting = CustomOption.Create(2006, CustomOption.CustomOptionType.Guesser, "guesserGamemodeHasMultipleShotsPerMeeting".Translate(), false, null, false, null, "");
			CustomOptionHolder.guesserGamemodeKillsThroughShield = CustomOption.Create(2008, CustomOption.CustomOptionType.Guesser, "guesserGamemodeKillsThroughShield".Translate(), true, null, false, null, "");
			CustomOptionHolder.guesserGamemodeEvilCanKillSpy = CustomOption.Create(2009, CustomOption.CustomOptionType.Guesser, "guesserGamemodeEvilCanKillSpy".Translate(), true, null, false, null, "");
			CustomOptionHolder.guesserGamemodeCantGuessSnitchIfTaksDone = CustomOption.Create(2010, CustomOption.CustomOptionType.Guesser, "guesserGamemodeCantGuessSnitchIfTaksDone".Translate(), true, null, false, null, "");
			CustomOptionHolder.hideNSeekMap = CustomOption.Create(3020, CustomOption.CustomOptionType.HideNSeekMain, CustomOptionHolder.cs(Color.yellow, ModTranslation.GetString("hideNSeekMap", null)), new string[]
			{
				"The Skeld",
				"Mira",
				"Polus",
				"Airship",
				"Fungle",
				"Submerged",
				"LI Map"
			}, null, true, delegate()
			{
				int num = CustomOptionHolder.hideNSeekMap.selection;
				bool flag = num >= 3;
				if (flag)
				{
					num++;
				}
				GameOptionsManager.Instance.currentNormalGameOptions.MapId = (byte)num;
			}, "");
			CustomOptionHolder.hideNSeekHunterCount = CustomOption.Create(3000, CustomOption.CustomOptionType.HideNSeekMain, CustomOptionHolder.cs(Color.yellow, ModTranslation.GetString("hideNSeekHunterCount", null)), 1f, 1f, 3f, 1f, null, false, null, "");
			CustomOptionHolder.hideNSeekKillCooldown = CustomOption.Create(3021, CustomOption.CustomOptionType.HideNSeekMain, CustomOptionHolder.cs(Color.yellow, ModTranslation.GetString("hideNSeekKillCooldown", null)), 10f, 2.5f, 60f, 2.5f, null, false, null, "");
			CustomOptionHolder.hideNSeekHunterVision = CustomOption.Create(3001, CustomOption.CustomOptionType.HideNSeekMain, CustomOptionHolder.cs(Color.yellow, ModTranslation.GetString("hideNSeekHunterVision", null)), 0.5f, 0.25f, 2f, 0.25f, null, false, null, "");
			CustomOptionHolder.hideNSeekHuntedVision = CustomOption.Create(3002, CustomOption.CustomOptionType.HideNSeekMain, CustomOptionHolder.cs(Color.yellow, ModTranslation.GetString("hideNSeekHuntedVision", null)), 2f, 0.25f, 5f, 0.25f, null, false, null, "");
			CustomOptionHolder.hideNSeekCommonTasks = CustomOption.Create(3023, CustomOption.CustomOptionType.HideNSeekMain, CustomOptionHolder.cs(Color.yellow, ModTranslation.GetString("hideNSeekCommonTasks", null)), 1f, 0f, 4f, 1f, null, false, null, "");
			CustomOptionHolder.hideNSeekShortTasks = CustomOption.Create(3024, CustomOption.CustomOptionType.HideNSeekMain, CustomOptionHolder.cs(Color.yellow, ModTranslation.GetString("hideNSeekShortTasks", null)), 3f, 1f, 23f, 1f, null, false, null, "");
			CustomOptionHolder.hideNSeekLongTasks = CustomOption.Create(3025, CustomOption.CustomOptionType.HideNSeekMain, CustomOptionHolder.cs(Color.yellow, ModTranslation.GetString("hideNSeekLongTasks", null)), 3f, 0f, 15f, 1f, null, false, null, "");
			CustomOptionHolder.hideNSeekTimer = CustomOption.Create(3003, CustomOption.CustomOptionType.HideNSeekMain, CustomOptionHolder.cs(Color.yellow, ModTranslation.GetString("hideNSeekTimer", null)), 5f, 1f, 30f, 0.5f, null, false, null, "");
			CustomOptionHolder.hideNSeekTaskWin = CustomOption.Create(3004, CustomOption.CustomOptionType.HideNSeekMain, CustomOptionHolder.cs(Color.yellow, ModTranslation.GetString("hideNSeekTaskWin", null)), false, null, false, null, "");
			CustomOptionHolder.hideNSeekTaskPunish = CustomOption.Create(3017, CustomOption.CustomOptionType.HideNSeekMain, CustomOptionHolder.cs(Color.yellow, ModTranslation.GetString("hideNSeekTaskPunish", null)), 10f, 0f, 30f, 1f, null, false, null, "");
			CustomOptionHolder.hideNSeekCanSabotage = CustomOption.Create(3019, CustomOption.CustomOptionType.HideNSeekMain, CustomOptionHolder.cs(Color.yellow, ModTranslation.GetString("hideNSeekCanSabotage", null)), false, null, false, null, "");
			CustomOptionHolder.hideNSeekHunterWaiting = CustomOption.Create(3022, CustomOption.CustomOptionType.HideNSeekMain, CustomOptionHolder.cs(Color.yellow, ModTranslation.GetString("hideNSeekHunterWaiting", null)), 15f, 2.5f, 60f, 2.5f, null, false, null, "");
			CustomOptionHolder.hunterLightCooldown = CustomOption.Create(3005, CustomOption.CustomOptionType.HideNSeekRoles, CustomOptionHolder.cs(Color.red, ModTranslation.GetString("hunterLightCooldown", null)), 30f, 5f, 60f, 1f, null, true, null, ModTranslation.GetString("hunterSettings", null));
			CustomOptionHolder.hunterLightDuration = CustomOption.Create(3006, CustomOption.CustomOptionType.HideNSeekRoles, CustomOptionHolder.cs(Color.red, ModTranslation.GetString("hunterLightDuration", null)), 5f, 1f, 60f, 1f, null, false, null, "");
			CustomOptionHolder.hunterLightVision = CustomOption.Create(3007, CustomOption.CustomOptionType.HideNSeekRoles, CustomOptionHolder.cs(Color.red, ModTranslation.GetString("hunterLightVision", null)), 3f, 1f, 5f, 0.25f, null, false, null, "");
			CustomOptionHolder.hunterLightPunish = CustomOption.Create(3008, CustomOption.CustomOptionType.HideNSeekRoles, CustomOptionHolder.cs(Color.red, ModTranslation.GetString("hunterLightPunish", null)), 5f, 0f, 30f, 1f, null, false, null, "");
			CustomOptionHolder.hunterAdminCooldown = CustomOption.Create(3009, CustomOption.CustomOptionType.HideNSeekRoles, CustomOptionHolder.cs(Color.red, ModTranslation.GetString("hunterAdminCooldown", null)), 30f, 5f, 60f, 1f, null, false, null, "");
			CustomOptionHolder.hunterAdminDuration = CustomOption.Create(3010, CustomOption.CustomOptionType.HideNSeekRoles, CustomOptionHolder.cs(Color.red, ModTranslation.GetString("hunterAdminDuration", null)), 5f, 1f, 60f, 1f, null, false, null, "");
			CustomOptionHolder.hunterAdminPunish = CustomOption.Create(3011, CustomOption.CustomOptionType.HideNSeekRoles, CustomOptionHolder.cs(Color.red, ModTranslation.GetString("hunterAdminPunish", null)), 5f, 0f, 30f, 1f, null, false, null, "");
			CustomOptionHolder.hunterArrowCooldown = CustomOption.Create(3012, CustomOption.CustomOptionType.HideNSeekRoles, CustomOptionHolder.cs(Color.red, ModTranslation.GetString("hunterArrowCooldown", null)), 30f, 5f, 60f, 1f, null, false, null, "");
			CustomOptionHolder.hunterArrowDuration = CustomOption.Create(3013, CustomOption.CustomOptionType.HideNSeekRoles, CustomOptionHolder.cs(Color.red, ModTranslation.GetString("hunterArrowDuration", null)), 5f, 0f, 60f, 1f, null, false, null, "");
			CustomOptionHolder.hunterArrowPunish = CustomOption.Create(3014, CustomOption.CustomOptionType.HideNSeekRoles, CustomOptionHolder.cs(Color.red, ModTranslation.GetString("hunterArrowPunish", null)), 5f, 0f, 30f, 1f, null, false, null, "");
			CustomOptionHolder.huntedShieldCooldown = CustomOption.Create(3015, CustomOption.CustomOptionType.HideNSeekRoles, CustomOptionHolder.cs(Color.gray, ModTranslation.GetString("huntedShieldCooldown", null)), 30f, 5f, 60f, 1f, null, true, null, ModTranslation.GetString("huntedSettings", null));
			CustomOptionHolder.huntedShieldDuration = CustomOption.Create(3016, CustomOption.CustomOptionType.HideNSeekRoles, CustomOptionHolder.cs(Color.gray, ModTranslation.GetString("huntedShieldDuration", null)), 5f, 1f, 60f, 1f, null, false, null, "");
			CustomOptionHolder.huntedShieldRewindTime = CustomOption.Create(3018, CustomOption.CustomOptionType.HideNSeekRoles, CustomOptionHolder.cs(Color.gray, ModTranslation.GetString("huntedShieldRewindTime", null)), 3f, 1f, 10f, 1f, null, false, null, "");
			CustomOptionHolder.huntedShieldNumber = CustomOption.Create(3026, CustomOption.CustomOptionType.HideNSeekRoles, CustomOptionHolder.cs(Color.gray, ModTranslation.GetString("huntedShieldNumber", null)), 3f, 1f, 15f, 1f, null, false, null, "");
			CustomOptionHolder.propHuntMap = CustomOption.Create(4020, CustomOption.CustomOptionType.PropHunt, CustomOptionHolder.cs(Color.yellow, ModTranslation.GetString("propHuntMap", null)), new string[]
			{
				"The Skeld",
				"Mira",
				"Polus",
				"Airship",
				"Fungle",
				"Submerged",
				"LI Map"
			}, null, true, delegate()
			{
				int num = CustomOptionHolder.propHuntMap.selection;
				bool flag = num >= 3;
				if (flag)
				{
					num++;
				}
				GameOptionsManager.Instance.currentNormalGameOptions.MapId = (byte)num;
			}, "");
			CustomOptionHolder.propHuntTimer = CustomOption.Create(4021, CustomOption.CustomOptionType.PropHunt, CustomOptionHolder.cs(Color.yellow, ModTranslation.GetString("propHuntTimer", null)), 5f, 1f, 30f, 0.5f, null, true, null, ModTranslation.GetString("propHuntSettings", null));
			CustomOptionHolder.propHuntUnstuckCooldown = CustomOption.Create(4011, CustomOption.CustomOptionType.PropHunt, CustomOptionHolder.cs(Color.yellow, ModTranslation.GetString("propHuntUnstuckCooldown", null)), 30f, 2.5f, 60f, 2.5f, null, false, null, "");
			CustomOptionHolder.propHuntUnstuckDuration = CustomOption.Create(4012, CustomOption.CustomOptionType.PropHunt, CustomOptionHolder.cs(Color.yellow, ModTranslation.GetString("propHuntUnstuckDuration", null)), 2f, 1f, 60f, 1f, null, false, null, "");
			CustomOptionHolder.propHunterVision = CustomOption.Create(4006, CustomOption.CustomOptionType.PropHunt, CustomOptionHolder.cs(Color.yellow, ModTranslation.GetString("propHunterVision", null)), 0.5f, 0.25f, 2f, 0.25f, null, false, null, "");
			CustomOptionHolder.propVision = CustomOption.Create(4007, CustomOption.CustomOptionType.PropHunt, CustomOptionHolder.cs(Color.yellow, ModTranslation.GetString("propVision", null)), 2f, 0.25f, 5f, 0.25f, null, false, null, "");
			CustomOptionHolder.propHuntNumberOfHunters = CustomOption.Create(4000, CustomOption.CustomOptionType.PropHunt, CustomOptionHolder.cs(Color.red, ModTranslation.GetString("propHuntNumberOfHunters", null)), 1f, 1f, 5f, 1f, null, true, null, ModTranslation.GetString("hunterSettings", null));
			CustomOptionHolder.hunterInitialBlackoutTime = CustomOption.Create(4001, CustomOption.CustomOptionType.PropHunt, CustomOptionHolder.cs(Color.red, ModTranslation.GetString("hunterInitialBlackoutTime", null)), 10f, 5f, 20f, 1f, null, false, null, "");
			CustomOptionHolder.hunterMissCooldown = CustomOption.Create(4004, CustomOption.CustomOptionType.PropHunt, CustomOptionHolder.cs(Color.red, ModTranslation.GetString("hunterMissCooldown", null)), 10f, 2.5f, 60f, 2.5f, null, false, null, "");
			CustomOptionHolder.hunterHitCooldown = CustomOption.Create(4005, CustomOption.CustomOptionType.PropHunt, CustomOptionHolder.cs(Color.red, ModTranslation.GetString("hunterHitCooldown", null)), 10f, 2.5f, 60f, 2.5f, null, false, null, "");
			CustomOptionHolder.propHuntRevealCooldown = CustomOption.Create(4008, CustomOption.CustomOptionType.PropHunt, CustomOptionHolder.cs(Color.red, ModTranslation.GetString("propHuntRevealCooldown", null)), 30f, 10f, 90f, 2.5f, null, false, null, "");
			CustomOptionHolder.propHuntRevealDuration = CustomOption.Create(4009, CustomOption.CustomOptionType.PropHunt, CustomOptionHolder.cs(Color.red, ModTranslation.GetString("propHuntRevealDuration", null)), 5f, 1f, 60f, 1f, null, false, null, "");
			CustomOptionHolder.propHuntRevealPunish = CustomOption.Create(4010, CustomOption.CustomOptionType.PropHunt, CustomOptionHolder.cs(Color.red, ModTranslation.GetString("propHuntRevealPunish", null)), 10f, 0f, 1800f, 5f, null, false, null, "");
			CustomOptionHolder.propHuntAdminCooldown = CustomOption.Create(4022, CustomOption.CustomOptionType.PropHunt, CustomOptionHolder.cs(Color.red, ModTranslation.GetString("propHuntAdminCooldown", null)), 30f, 2.5f, 1800f, 2.5f, null, false, null, "");
			CustomOptionHolder.propHuntFindCooldown = CustomOption.Create(4023, CustomOption.CustomOptionType.PropHunt, CustomOptionHolder.cs(Color.red, ModTranslation.GetString("propHuntFindCooldown", null)), 60f, 2.5f, 1800f, 2.5f, null, false, null, "");
			CustomOptionHolder.propHuntFindDuration = CustomOption.Create(4024, CustomOption.CustomOptionType.PropHunt, CustomOptionHolder.cs(Color.red, ModTranslation.GetString("propHuntFindDuration", null)), 5f, 1f, 15f, 1f, null, false, null, "");
			CustomOptionHolder.propBecomesHunterWhenFound = CustomOption.Create(4003, CustomOption.CustomOptionType.PropHunt, CustomOptionHolder.cs(Palette.CrewmateBlue, ModTranslation.GetString("propBecomesHunterWhenFound", null)), false, null, true, null, ModTranslation.GetString("propSettings", null));
			CustomOptionHolder.propHuntInvisEnabled = CustomOption.Create(4013, CustomOption.CustomOptionType.PropHunt, CustomOptionHolder.cs(Palette.CrewmateBlue, ModTranslation.GetString("propHuntInvisEnabled", null)), true, null, true, null, "");
			CustomOptionHolder.propHuntInvisCooldown = CustomOption.Create(4014, CustomOption.CustomOptionType.PropHunt, CustomOptionHolder.cs(Palette.CrewmateBlue, ModTranslation.GetString("propHuntInvisCooldown", null)), 120f, 10f, 1800f, 2.5f, CustomOptionHolder.propHuntInvisEnabled, false, null, "");
			CustomOptionHolder.propHuntInvisDuration = CustomOption.Create(4015, CustomOption.CustomOptionType.PropHunt, CustomOptionHolder.cs(Palette.CrewmateBlue, ModTranslation.GetString("propHuntInvisDuration", null)), 5f, 1f, 30f, 1f, CustomOptionHolder.propHuntInvisEnabled, false, null, "");
			CustomOptionHolder.propHuntSpeedboostEnabled = CustomOption.Create(4016, CustomOption.CustomOptionType.PropHunt, CustomOptionHolder.cs(Palette.CrewmateBlue, ModTranslation.GetString("propHuntSpeedboostEnabled", null)), true, null, true, null, "");
			CustomOptionHolder.propHuntSpeedboostCooldown = CustomOption.Create(4017, CustomOption.CustomOptionType.PropHunt, CustomOptionHolder.cs(Palette.CrewmateBlue, ModTranslation.GetString("propHuntSpeedboostCooldown", null)), 60f, 2.5f, 1800f, 2.5f, CustomOptionHolder.propHuntSpeedboostEnabled, false, null, "");
			CustomOptionHolder.propHuntSpeedboostDuration = CustomOption.Create(4018, CustomOption.CustomOptionType.PropHunt, CustomOptionHolder.cs(Palette.CrewmateBlue, ModTranslation.GetString("propHuntSpeedboostDuration", null)), 5f, 1f, 15f, 1f, CustomOptionHolder.propHuntSpeedboostEnabled, false, null, "");
			CustomOptionHolder.propHuntSpeedboostSpeed = CustomOption.Create(4019, CustomOption.CustomOptionType.PropHunt, CustomOptionHolder.cs(Palette.CrewmateBlue, ModTranslation.GetString("propHuntSpeedboostSpeed", null)), 2f, 1.25f, 5f, 0.25f, CustomOptionHolder.propHuntSpeedboostEnabled, false, null, "");
			CustomOptionHolder.maxNumberOfMeetings = CustomOption.Create(3, CustomOption.CustomOptionType.General, ModTranslation.GetString("maxNumberOfMeetings", null), 10f, 0f, 15f, 1f, null, true, null, ModTranslation.GetString("inGameSettings", null));
			CustomOptionHolder.anyPlayerCanStopStart = CustomOption.Create(2, CustomOption.CustomOptionType.General, CustomOptionHolder.cs(new Color(0.8f, 0.8f, 0f, 1f), ModTranslation.GetString("anyPlayerCanStopStart", null)), false, null, false, null, "");
			CustomOptionHolder.blockSkippingInEmergencyMeetings = CustomOption.Create(4, CustomOption.CustomOptionType.General, ModTranslation.GetString("blockSkippingInEmergencyMeetings", null), false, null, false, null, "");
			CustomOptionHolder.noVoteIsSelfVote = CustomOption.Create(5, CustomOption.CustomOptionType.General, ModTranslation.GetString("noVoteIsSelfVote", null), false, CustomOptionHolder.blockSkippingInEmergencyMeetings, false, null, "");
			CustomOptionHolder.hidePlayerNames = CustomOption.Create(6, CustomOption.CustomOptionType.General, ModTranslation.GetString("hidePlayerNames", null), false, null, false, null, "");
			CustomOptionHolder.allowParallelMedBayScans = CustomOption.Create(7, CustomOption.CustomOptionType.General, ModTranslation.GetString("allowParallelMedBayScans", null), false, null, false, null, "");
			CustomOptionHolder.shieldFirstKill = CustomOption.Create(8, CustomOption.CustomOptionType.General, ModTranslation.GetString("shieldFirstKill", null), false, null, false, null, "");
			CustomOptionHolder.finishTasksBeforeHauntingOrZoomingOut = CustomOption.Create(9, CustomOption.CustomOptionType.General, ModTranslation.GetString("finishTasksBeforeHauntingOrZoomingOut", null), true, null, false, null, "");
			CustomOptionHolder.deadImpsBlockSabotage = CustomOption.Create(13, CustomOption.CustomOptionType.General, ModTranslation.GetString("deadImpsBlockSabotage", null), false, null, false, null, "");
			CustomOptionHolder.camsNightVision = CustomOption.Create(11, CustomOption.CustomOptionType.General, ModTranslation.GetString("camsNightVision", null), false, null, true, null, ModTranslation.GetString("camsNightVisionSettings", null));
			CustomOptionHolder.dynamicMap = CustomOption.Create(500, CustomOption.CustomOptionType.General, "dynamicMap".Translate(), false, null, true, null, "dynamicMap".Translate());
			CustomOptionHolder.dynamicMapEnableSkeld = CustomOption.Create(501, CustomOption.CustomOptionType.General, "Skeld", CustomOptionHolder.rates, CustomOptionHolder.dynamicMap, false, null, "");
			CustomOptionHolder.dynamicMapEnableMira = CustomOption.Create(502, CustomOption.CustomOptionType.General, "Mira", CustomOptionHolder.rates, CustomOptionHolder.dynamicMap, false, null, "");
			CustomOptionHolder.dynamicMapEnablePolus = CustomOption.Create(503, CustomOption.CustomOptionType.General, "Polus", CustomOptionHolder.rates, CustomOptionHolder.dynamicMap, false, null, "");
			CustomOptionHolder.dynamicMapEnableAirShip = CustomOption.Create(504, CustomOption.CustomOptionType.General, "Airship", CustomOptionHolder.rates, CustomOptionHolder.dynamicMap, false, null, "");
			CustomOptionHolder.dynamicMapEnableFungle = CustomOption.Create(506, CustomOption.CustomOptionType.General, "Fungle", CustomOptionHolder.rates, CustomOptionHolder.dynamicMap, false, null, "");
			CustomOptionHolder.dynamicMapEnableSubmerged = CustomOption.Create(505, CustomOption.CustomOptionType.General, "Submerged", CustomOptionHolder.rates, CustomOptionHolder.dynamicMap, false, null, "");
			CustomOptionHolder.dynamicMapSeparateSettings = CustomOption.Create(509, CustomOption.CustomOptionType.General, "dynamicMapSeparateSettings".Translate(), false, CustomOptionHolder.dynamicMap, true, null, "");
			CustomOptionHolder.camsNoNightVisionIfImpVision = CustomOption.Create(12, CustomOption.CustomOptionType.General, "camsNoNightVisionIfImpVision".Translate(), false, CustomOptionHolder.camsNightVision, false, null, "");
			CustomOptionHolder.AddVents = CustomOption.Create(114513, CustomOption.CustomOptionType.General, "AddVents".Translate(), false, null, true, null, "AddVents".Translate());
			CustomOptionHolder.addPolusVents = CustomOption.Create(114514, CustomOption.CustomOptionType.General, "addPolusVents".Translate(), false, CustomOptionHolder.enableBetterPolus, false, null, "");
			CustomOptionHolder.addAirShipVents = CustomOption.Create(114515, CustomOption.CustomOptionType.General, "addAirShipVents".Translate(), false, CustomOptionHolder.enableAirShipModify, false, null, "");
			CustomOptionHolder.enableAirShipModify = CustomOption.Create(114516, CustomOption.CustomOptionType.General, CustomOptionHolder.cs(Color.yellow, "enableAirShipModify".Translate()), false, null, false, null, "");
			CustomOptionHolder.enableBetterPolus = CustomOption.Create(114517, CustomOption.CustomOptionType.General, "enableBetterPolus".Translate(), false, null, false, null, "");
			CustomOptionHolder.blockedRolePairings.Add(19, new byte[]
			{
				28
			});
			CustomOptionHolder.blockedRolePairings.Add(28, new byte[]
			{
				19
			});
			CustomOptionHolder.blockedRolePairings.Add(25, new byte[]
			{
				55
			});
			CustomOptionHolder.blockedRolePairings.Add(55, new byte[]
			{
				25
			});
			CustomOptionHolder.blockedRolePairings.Add(34, new byte[]
			{
				27
			});
			CustomOptionHolder.blockedRolePairings.Add(27, new byte[]
			{
				34
			});
		}

		// Token: 0x040000B4 RID: 180
		public static string[] rates = new string[]
		{
			"0%",
			"10%",
			"20%",
			"30%",
			"40%",
			"50%",
			"60%",
			"70%",
			"80%",
			"90%",
			"100%"
		};

		// Token: 0x040000B5 RID: 181
		public static string[] ratesModifier = new string[]
		{
			"1",
			"2",
			"3",
			"4",
			"5",
			"6",
			"7",
			"8",
			"9",
			"10",
			"11",
			"12",
			"13",
			"14",
			"15"
		};

		// Token: 0x040000B6 RID: 182
		public static string[] presets = new string[]
		{
			"preset1".Translate(),
			"preset2".Translate(),
			"RandomPresetSkeld".Translate(),
			"RandomPresetMira".Translate(),
			"RandomPresetPolus".Translate(),
			"RandomPresetAirship".Translate(),
			"RandomPresetSubmerged".Translate()
		};

		// Token: 0x040000B7 RID: 183
		public static CustomOption presetSelection;

		// Token: 0x040000B8 RID: 184
		public static CustomOption activateRoles;

		// Token: 0x040000B9 RID: 185
		public static CustomOption crewmateRolesCountMin;

		// Token: 0x040000BA RID: 186
		public static CustomOption crewmateRolesCountMax;

		// Token: 0x040000BB RID: 187
		public static CustomOption crewmateRolesFill;

		// Token: 0x040000BC RID: 188
		public static CustomOption neutralRolesCountMin;

		// Token: 0x040000BD RID: 189
		public static CustomOption neutralRolesCountMax;

		// Token: 0x040000BE RID: 190
		public static CustomOption impostorRolesCountMin;

		// Token: 0x040000BF RID: 191
		public static CustomOption impostorRolesCountMax;

		// Token: 0x040000C0 RID: 192
		public static CustomOption modifiersCountMin;

		// Token: 0x040000C1 RID: 193
		public static CustomOption modifiersCountMax;

		// Token: 0x040000C2 RID: 194
		public static CustomOption anyPlayerCanStopStart;

		// Token: 0x040000C3 RID: 195
		public static CustomOption enableEventMode;

		// Token: 0x040000C4 RID: 196
		public static CustomOption deadImpsBlockSabotage;

		// Token: 0x040000C5 RID: 197
		public static CustomOption mafiaSpawnRate;

		// Token: 0x040000C6 RID: 198
		public static CustomOption janitorCooldown;

		// Token: 0x040000C7 RID: 199
		public static CustomOption morphlingSpawnRate;

		// Token: 0x040000C8 RID: 200
		public static CustomOption morphlingCooldown;

		// Token: 0x040000C9 RID: 201
		public static CustomOption morphlingDuration;

		// Token: 0x040000CA RID: 202
		public static CustomOption camouflagerSpawnRate;

		// Token: 0x040000CB RID: 203
		public static CustomOption camouflagerCooldown;

		// Token: 0x040000CC RID: 204
		public static CustomOption camouflagerDuration;

		// Token: 0x040000CD RID: 205
		public static CustomOption vampireSpawnRate;

		// Token: 0x040000CE RID: 206
		public static CustomOption vampireKillDelay;

		// Token: 0x040000CF RID: 207
		public static CustomOption vampireCooldown;

		// Token: 0x040000D0 RID: 208
		public static CustomOption vampireCanKillNearGarlics;

		// Token: 0x040000D1 RID: 209
		public static CustomOption eraserSpawnRate;

		// Token: 0x040000D2 RID: 210
		public static CustomOption eraserCooldown;

		// Token: 0x040000D3 RID: 211
		public static CustomOption eraserCanEraseAnyone;

		// Token: 0x040000D4 RID: 212
		public static CustomOption guesserSpawnRate;

		// Token: 0x040000D5 RID: 213
		public static CustomOption guesserIsImpGuesserRate;

		// Token: 0x040000D6 RID: 214
		public static CustomOption guesserNumberOfShots;

		// Token: 0x040000D7 RID: 215
		public static CustomOption guesserHasMultipleShotsPerMeeting;

		// Token: 0x040000D8 RID: 216
		public static CustomOption guesserKillsThroughShield;

		// Token: 0x040000D9 RID: 217
		public static CustomOption guesserEvilCanKillSpy;

		// Token: 0x040000DA RID: 218
		public static CustomOption guesserSpawnBothRate;

		// Token: 0x040000DB RID: 219
		public static CustomOption guesserCantGuessSnitchIfTaksDone;

		// Token: 0x040000DC RID: 220
		public static CustomOption jesterSpawnRate;

		// Token: 0x040000DD RID: 221
		public static CustomOption jesterCanCallEmergency;

		// Token: 0x040000DE RID: 222
		public static CustomOption jesterHasImpostorVision;

		// Token: 0x040000DF RID: 223
		public static CustomOption arsonistSpawnRate;

		// Token: 0x040000E0 RID: 224
		public static CustomOption arsonistCooldown;

		// Token: 0x040000E1 RID: 225
		public static CustomOption arsonistDuration;

		// Token: 0x040000E2 RID: 226
		public static CustomOption jackalSpawnRate;

		// Token: 0x040000E3 RID: 227
		public static CustomOption jackalKillCooldown;

		// Token: 0x040000E4 RID: 228
		public static CustomOption jackalCreateSidekickCooldown;

		// Token: 0x040000E5 RID: 229
		public static CustomOption jackalCanSabotageLights;

		// Token: 0x040000E6 RID: 230
		public static CustomOption jackalCanUseVents;

		// Token: 0x040000E7 RID: 231
		public static CustomOption jackalCanCreateSidekick;

		// Token: 0x040000E8 RID: 232
		public static CustomOption sidekickPromotesToJackal;

		// Token: 0x040000E9 RID: 233
		public static CustomOption sidekickCanKill;

		// Token: 0x040000EA RID: 234
		public static CustomOption sidekickCanUseVents;

		// Token: 0x040000EB RID: 235
		public static CustomOption sidekickCanSabotageLights;

		// Token: 0x040000EC RID: 236
		public static CustomOption jackalPromotedFromSidekickCanCreateSidekick;

		// Token: 0x040000ED RID: 237
		public static CustomOption jackalCanCreateSidekickFromImpostor;

		// Token: 0x040000EE RID: 238
		public static CustomOption jackalAndSidekickHaveImpostorVision;

		// Token: 0x040000EF RID: 239
		public static CustomOption bountyHunterSpawnRate;

		// Token: 0x040000F0 RID: 240
		public static CustomOption bountyHunterBountyDuration;

		// Token: 0x040000F1 RID: 241
		public static CustomOption bountyHunterReducedCooldown;

		// Token: 0x040000F2 RID: 242
		public static CustomOption bountyHunterPunishmentTime;

		// Token: 0x040000F3 RID: 243
		public static CustomOption bountyHunterShowArrow;

		// Token: 0x040000F4 RID: 244
		public static CustomOption bountyHunterArrowUpdateIntervall;

		// Token: 0x040000F5 RID: 245
		public static CustomOption bountyHunterShowCooldownForGhosts;

		// Token: 0x040000F6 RID: 246
		public static CustomOption witchSpawnRate;

		// Token: 0x040000F7 RID: 247
		public static CustomOption witchCooldown;

		// Token: 0x040000F8 RID: 248
		public static CustomOption witchAdditionalCooldown;

		// Token: 0x040000F9 RID: 249
		public static CustomOption witchCanSpellAnyone;

		// Token: 0x040000FA RID: 250
		public static CustomOption witchSpellCastingDuration;

		// Token: 0x040000FB RID: 251
		public static CustomOption witchTriggerBothCooldowns;

		// Token: 0x040000FC RID: 252
		public static CustomOption witchVoteSavesTargets;

		// Token: 0x040000FD RID: 253
		public static CustomOption ninjaSpawnRate;

		// Token: 0x040000FE RID: 254
		public static CustomOption ninjaCooldown;

		// Token: 0x040000FF RID: 255
		public static CustomOption ninjaKnowsTargetLocation;

		// Token: 0x04000100 RID: 256
		public static CustomOption ninjaTraceTime;

		// Token: 0x04000101 RID: 257
		public static CustomOption ninjaTraceColorTime;

		// Token: 0x04000102 RID: 258
		public static CustomOption ninjaInvisibleDuration;

		// Token: 0x04000103 RID: 259
		public static CustomOption mayorSpawnRate;

		// Token: 0x04000104 RID: 260
		public static CustomOption mayorCanSeeVoteColors;

		// Token: 0x04000105 RID: 261
		public static CustomOption mayorTasksNeededToSeeVoteColors;

		// Token: 0x04000106 RID: 262
		public static CustomOption mayorMeetingButton;

		// Token: 0x04000107 RID: 263
		public static CustomOption mayorMaxRemoteMeetings;

		// Token: 0x04000108 RID: 264
		public static CustomOption mayorChooseSingleVote;

		// Token: 0x04000109 RID: 265
		public static CustomOption portalmakerSpawnRate;

		// Token: 0x0400010A RID: 266
		public static CustomOption portalmakerCooldown;

		// Token: 0x0400010B RID: 267
		public static CustomOption portalmakerUsePortalCooldown;

		// Token: 0x0400010C RID: 268
		public static CustomOption portalmakerLogOnlyColorType;

		// Token: 0x0400010D RID: 269
		public static CustomOption portalmakerLogHasTime;

		// Token: 0x0400010E RID: 270
		public static CustomOption portalmakerCanPortalFromAnywhere;

		// Token: 0x0400010F RID: 271
		public static CustomOption engineerSpawnRate;

		// Token: 0x04000110 RID: 272
		public static CustomOption engineerNumberOfFixes;

		// Token: 0x04000111 RID: 273
		public static CustomOption engineerHighlightForImpostors;

		// Token: 0x04000112 RID: 274
		public static CustomOption engineerHighlightForTeamJackal;

		// Token: 0x04000113 RID: 275
		public static CustomOption sheriffSpawnRate;

		// Token: 0x04000114 RID: 276
		public static CustomOption sheriffCooldown;

		// Token: 0x04000115 RID: 277
		public static CustomOption sheriffCanKillNeutrals;

		// Token: 0x04000116 RID: 278
		public static CustomOption deputySpawnRate;

		// Token: 0x04000117 RID: 279
		public static CustomOption deputyNumberOfHandcuffs;

		// Token: 0x04000118 RID: 280
		public static CustomOption deputyHandcuffCooldown;

		// Token: 0x04000119 RID: 281
		public static CustomOption deputyGetsPromoted;

		// Token: 0x0400011A RID: 282
		public static CustomOption deputyKeepsHandcuffs;

		// Token: 0x0400011B RID: 283
		public static CustomOption deputyHandcuffDuration;

		// Token: 0x0400011C RID: 284
		public static CustomOption deputyKnowsSheriff;

		// Token: 0x0400011D RID: 285
		public static CustomOption lighterSpawnRate;

		// Token: 0x0400011E RID: 286
		public static CustomOption lighterModeLightsOnVision;

		// Token: 0x0400011F RID: 287
		public static CustomOption lighterModeLightsOffVision;

		// Token: 0x04000120 RID: 288
		public static CustomOption lighterFlashlightWidth;

		// Token: 0x04000121 RID: 289
		public static CustomOption detectiveSpawnRate;

		// Token: 0x04000122 RID: 290
		public static CustomOption detectiveAnonymousFootprints;

		// Token: 0x04000123 RID: 291
		public static CustomOption detectiveFootprintIntervall;

		// Token: 0x04000124 RID: 292
		public static CustomOption detectiveFootprintDuration;

		// Token: 0x04000125 RID: 293
		public static CustomOption detectiveReportNameDuration;

		// Token: 0x04000126 RID: 294
		public static CustomOption detectiveReportColorDuration;

		// Token: 0x04000127 RID: 295
		public static CustomOption timeMasterSpawnRate;

		// Token: 0x04000128 RID: 296
		public static CustomOption timeMasterCooldown;

		// Token: 0x04000129 RID: 297
		public static CustomOption timeMasterRewindTime;

		// Token: 0x0400012A RID: 298
		public static CustomOption timeMasterShieldDuration;

		// Token: 0x0400012B RID: 299
		public static CustomOption medicSpawnRate;

		// Token: 0x0400012C RID: 300
		public static CustomOption medicShowShielded;

		// Token: 0x0400012D RID: 301
		public static CustomOption medicShowAttemptToShielded;

		// Token: 0x0400012E RID: 302
		public static CustomOption medicSetOrShowShieldAfterMeeting;

		// Token: 0x0400012F RID: 303
		public static CustomOption medicShowAttemptToMedic;

		// Token: 0x04000130 RID: 304
		public static CustomOption medicSetShieldAfterMeeting;

		// Token: 0x04000131 RID: 305
		public static CustomOption swapperSpawnRate;

		// Token: 0x04000132 RID: 306
		public static CustomOption swapperCanCallEmergency;

		// Token: 0x04000133 RID: 307
		public static CustomOption swapperCanOnlySwapOthers;

		// Token: 0x04000134 RID: 308
		public static CustomOption swapperSwapsNumber;

		// Token: 0x04000135 RID: 309
		public static CustomOption swapperRechargeTasksNumber;

		// Token: 0x04000136 RID: 310
		public static CustomOption seerSpawnRate;

		// Token: 0x04000137 RID: 311
		public static CustomOption seerMode;

		// Token: 0x04000138 RID: 312
		public static CustomOption seerSoulDuration;

		// Token: 0x04000139 RID: 313
		public static CustomOption seerLimitSoulDuration;

		// Token: 0x0400013A RID: 314
		public static CustomOption hackerSpawnRate;

		// Token: 0x0400013B RID: 315
		public static CustomOption hackerCooldown;

		// Token: 0x0400013C RID: 316
		public static CustomOption hackerHackeringDuration;

		// Token: 0x0400013D RID: 317
		public static CustomOption hackerOnlyColorType;

		// Token: 0x0400013E RID: 318
		public static CustomOption hackerToolsNumber;

		// Token: 0x0400013F RID: 319
		public static CustomOption hackerRechargeTasksNumber;

		// Token: 0x04000140 RID: 320
		public static CustomOption hackerNoMove;

		// Token: 0x04000141 RID: 321
		public static CustomOption trackerSpawnRate;

		// Token: 0x04000142 RID: 322
		public static CustomOption trackerUpdateIntervall;

		// Token: 0x04000143 RID: 323
		public static CustomOption trackerResetTargetAfterMeeting;

		// Token: 0x04000144 RID: 324
		public static CustomOption trackerCanTrackCorpses;

		// Token: 0x04000145 RID: 325
		public static CustomOption trackerCorpsesTrackingCooldown;

		// Token: 0x04000146 RID: 326
		public static CustomOption trackerCorpsesTrackingDuration;

		// Token: 0x04000147 RID: 327
		public static CustomOption trackerTrackingMethod;

		// Token: 0x04000148 RID: 328
		public static CustomOption snitchSpawnRate;

		// Token: 0x04000149 RID: 329
		public static CustomOption snitchLeftTasksForReveal;

		// Token: 0x0400014A RID: 330
		public static CustomOption snitchMode;

		// Token: 0x0400014B RID: 331
		public static CustomOption snitchTargets;

		// Token: 0x0400014C RID: 332
		public static CustomOption spySpawnRate;

		// Token: 0x0400014D RID: 333
		public static CustomOption spyCanDieToSheriff;

		// Token: 0x0400014E RID: 334
		public static CustomOption spyImpostorsCanKillAnyone;

		// Token: 0x0400014F RID: 335
		public static CustomOption spyCanEnterVents;

		// Token: 0x04000150 RID: 336
		public static CustomOption spyHasImpostorVision;

		// Token: 0x04000151 RID: 337
		public static CustomOption tricksterSpawnRate;

		// Token: 0x04000152 RID: 338
		public static CustomOption tricksterPlaceBoxCooldown;

		// Token: 0x04000153 RID: 339
		public static CustomOption tricksterLightsOutCooldown;

		// Token: 0x04000154 RID: 340
		public static CustomOption tricksterLightsOutDuration;

		// Token: 0x04000155 RID: 341
		public static CustomOption cleanerSpawnRate;

		// Token: 0x04000156 RID: 342
		public static CustomOption cleanerCooldown;

		// Token: 0x04000157 RID: 343
		public static CustomOption warlockSpawnRate;

		// Token: 0x04000158 RID: 344
		public static CustomOption warlockCooldown;

		// Token: 0x04000159 RID: 345
		public static CustomOption warlockRootTime;

		// Token: 0x0400015A RID: 346
		public static CustomOption securityGuardSpawnRate;

		// Token: 0x0400015B RID: 347
		public static CustomOption securityGuardCooldown;

		// Token: 0x0400015C RID: 348
		public static CustomOption securityGuardTotalScrews;

		// Token: 0x0400015D RID: 349
		public static CustomOption securityGuardCamPrice;

		// Token: 0x0400015E RID: 350
		public static CustomOption securityGuardVentPrice;

		// Token: 0x0400015F RID: 351
		public static CustomOption securityGuardCamDuration;

		// Token: 0x04000160 RID: 352
		public static CustomOption securityGuardCamMaxCharges;

		// Token: 0x04000161 RID: 353
		public static CustomOption securityGuardCamRechargeTasksNumber;

		// Token: 0x04000162 RID: 354
		public static CustomOption securityGuardNoMove;

		// Token: 0x04000163 RID: 355
		public static CustomOption vultureSpawnRate;

		// Token: 0x04000164 RID: 356
		public static CustomOption vultureCooldown;

		// Token: 0x04000165 RID: 357
		public static CustomOption vultureNumberToWin;

		// Token: 0x04000166 RID: 358
		public static CustomOption vultureCanUseVents;

		// Token: 0x04000167 RID: 359
		public static CustomOption vultureShowArrows;

		// Token: 0x04000168 RID: 360
		public static CustomOption mediumSpawnRate;

		// Token: 0x04000169 RID: 361
		public static CustomOption mediumCooldown;

		// Token: 0x0400016A RID: 362
		public static CustomOption mediumDuration;

		// Token: 0x0400016B RID: 363
		public static CustomOption mediumOneTimeUse;

		// Token: 0x0400016C RID: 364
		public static CustomOption mediumChanceAdditionalInfo;

		// Token: 0x0400016D RID: 365
		public static CustomOption lawyerSpawnRate;

		// Token: 0x0400016E RID: 366
		public static CustomOption lawyerIsProsecutorChance;

		// Token: 0x0400016F RID: 367
		public static CustomOption lawyerTargetCanBeJester;

		// Token: 0x04000170 RID: 368
		public static CustomOption lawyerVision;

		// Token: 0x04000171 RID: 369
		public static CustomOption lawyerKnowsRole;

		// Token: 0x04000172 RID: 370
		public static CustomOption lawyerCanCallEmergency;

		// Token: 0x04000173 RID: 371
		public static CustomOption pursuerCooldown;

		// Token: 0x04000174 RID: 372
		public static CustomOption pursuerBlanksNumber;

		// Token: 0x04000175 RID: 373
		public static CustomOption thiefSpawnRate;

		// Token: 0x04000176 RID: 374
		public static CustomOption thiefCooldown;

		// Token: 0x04000177 RID: 375
		public static CustomOption thiefHasImpVision;

		// Token: 0x04000178 RID: 376
		public static CustomOption thiefCanUseVents;

		// Token: 0x04000179 RID: 377
		public static CustomOption thiefCanKillSheriff;

		// Token: 0x0400017A RID: 378
		public static CustomOption thiefCanStealWithGuess;

		// Token: 0x0400017B RID: 379
		public static CustomOption trapperSpawnRate;

		// Token: 0x0400017C RID: 380
		public static CustomOption trapperCooldown;

		// Token: 0x0400017D RID: 381
		public static CustomOption trapperMaxCharges;

		// Token: 0x0400017E RID: 382
		public static CustomOption trapperRechargeTasksNumber;

		// Token: 0x0400017F RID: 383
		public static CustomOption trapperTrapNeededTriggerToReveal;

		// Token: 0x04000180 RID: 384
		public static CustomOption trapperAnonymousMap;

		// Token: 0x04000181 RID: 385
		public static CustomOption trapperInfoType;

		// Token: 0x04000182 RID: 386
		public static CustomOption trapperTrapDuration;

		// Token: 0x04000183 RID: 387
		public static CustomOption bomberSpawnRate;

		// Token: 0x04000184 RID: 388
		public static CustomOption bomberBombDestructionTime;

		// Token: 0x04000185 RID: 389
		public static CustomOption bomberBombDestructionRange;

		// Token: 0x04000186 RID: 390
		public static CustomOption bomberBombHearRange;

		// Token: 0x04000187 RID: 391
		public static CustomOption bomberDefuseDuration;

		// Token: 0x04000188 RID: 392
		public static CustomOption bomberBombCooldown;

		// Token: 0x04000189 RID: 393
		public static CustomOption bomberBombActiveAfter;

		// Token: 0x0400018A RID: 394
		public static CustomOption yoyoSpawnRate;

		// Token: 0x0400018B RID: 395
		public static CustomOption yoyoBlinkDuration;

		// Token: 0x0400018C RID: 396
		public static CustomOption yoyoMarkCooldown;

		// Token: 0x0400018D RID: 397
		public static CustomOption yoyoMarkStaysOverMeeting;

		// Token: 0x0400018E RID: 398
		public static CustomOption yoyoHasAdminTable;

		// Token: 0x0400018F RID: 399
		public static CustomOption yoyoAdminTableCooldown;

		// Token: 0x04000190 RID: 400
		public static CustomOption yoyoSilhouetteVisibility;

		// Token: 0x04000191 RID: 401
		public static CustomOption prophetSpawnRate;

		// Token: 0x04000192 RID: 402
		public static CustomOption prophetCooldown;

		// Token: 0x04000193 RID: 403
		public static CustomOption prophetNumExamines;

		// Token: 0x04000194 RID: 404
		public static CustomOption prophetAccuracy;

		// Token: 0x04000195 RID: 405
		public static CustomOption prophetCanCallEmergency;

		// Token: 0x04000196 RID: 406
		public static CustomOption prophetIsRevealed;

		// Token: 0x04000197 RID: 407
		public static CustomOption prophetExaminesToBeRevealed;

		// Token: 0x04000198 RID: 408
		public static CustomOption fraudsterSpawnRate;

		// Token: 0x04000199 RID: 409
		public static CustomOption fraudstercooldown;

        // Token: 0x04000198 RID: 408
        public static CustomOption devilSpawnRate;

        // Token: 0x04000199 RID: 409
        public static CustomOption devilcooldown;

        // Token: 0x0400019A RID: 410
        public static CustomOption modifiersAreHidden;

		// Token: 0x0400019B RID: 411
		public static CustomOption modifierBait;

		// Token: 0x0400019C RID: 412
		public static CustomOption modifierBaitQuantity;

		// Token: 0x0400019D RID: 413
		public static CustomOption modifierBaitReportDelayMin;

		// Token: 0x0400019E RID: 414
		public static CustomOption modifierBaitReportDelayMax;

		// Token: 0x0400019F RID: 415
		public static CustomOption modifierBaitShowKillFlash;

		// Token: 0x040001A0 RID: 416
		public static CustomOption modifierLover;

		// Token: 0x040001A1 RID: 417
		public static CustomOption modifierLoverImpLoverRate;

		// Token: 0x040001A2 RID: 418
		public static CustomOption modifierLoverBothDie;

		// Token: 0x040001A3 RID: 419
		public static CustomOption modifierLoverEnableChat;

		// Token: 0x040001A4 RID: 420
		public static CustomOption modifierBloody;

		// Token: 0x040001A5 RID: 421
		public static CustomOption modifierBloodyQuantity;

		// Token: 0x040001A6 RID: 422
		public static CustomOption modifierBloodyDuration;

		// Token: 0x040001A7 RID: 423
		public static CustomOption modifierAntiTeleport;

		// Token: 0x040001A8 RID: 424
		public static CustomOption modifierAntiTeleportQuantity;

		// Token: 0x040001A9 RID: 425
		public static CustomOption modifierTieBreaker;

		// Token: 0x040001AA RID: 426
		public static CustomOption modifierSunglasses;

		// Token: 0x040001AB RID: 427
		public static CustomOption modifierSunglassesQuantity;

		// Token: 0x040001AC RID: 428
		public static CustomOption modifierSunglassesVision;

		// Token: 0x040001AD RID: 429
		public static CustomOption modifierLighterln;

		// Token: 0x040001AE RID: 430
		public static CustomOption modifierMini;

		// Token: 0x040001AF RID: 431
		public static CustomOption modifierMiniGrowingUpDuration;

		// Token: 0x040001B0 RID: 432
		public static CustomOption modifierMiniGrowingUpInMeeting;

		// Token: 0x040001B1 RID: 433
		public static CustomOption modifierVip;

		// Token: 0x040001B2 RID: 434
		public static CustomOption modifierVipQuantity;

		// Token: 0x040001B3 RID: 435
		public static CustomOption modifierVipShowColor;

		// Token: 0x040001B4 RID: 436
		public static CustomOption modifierInvert;

		// Token: 0x040001B5 RID: 437
		public static CustomOption modifierInvertQuantity;

		// Token: 0x040001B6 RID: 438
		public static CustomOption modifierInvertDuration;

		// Token: 0x040001B7 RID: 439
		public static CustomOption modifierChameleon;

		// Token: 0x040001B8 RID: 440
		public static CustomOption modifierChameleonQuantity;

		// Token: 0x040001B9 RID: 441
		public static CustomOption modifierChameleonHoldDuration;

		// Token: 0x040001BA RID: 442
		public static CustomOption modifierChameleonFadeDuration;

		// Token: 0x040001BB RID: 443
		public static CustomOption modifierChameleonMinVisibility;

		// Token: 0x040001BC RID: 444
		public static CustomOption modifierShifter;

		// Token: 0x040001BD RID: 445
		public static CustomOption maxNumberOfMeetings;

		// Token: 0x040001BE RID: 446
		public static CustomOption blockSkippingInEmergencyMeetings;

		// Token: 0x040001BF RID: 447
		public static CustomOption noVoteIsSelfVote;

		// Token: 0x040001C0 RID: 448
		public static CustomOption hidePlayerNames;

		// Token: 0x040001C1 RID: 449
		public static CustomOption allowParallelMedBayScans;

		// Token: 0x040001C2 RID: 450
		public static CustomOption shieldFirstKill;

		// Token: 0x040001C3 RID: 451
		public static CustomOption finishTasksBeforeHauntingOrZoomingOut;

		// Token: 0x040001C4 RID: 452
		public static CustomOption camsNightVision;

		// Token: 0x040001C5 RID: 453
		public static CustomOption camsNoNightVisionIfImpVision;

		// Token: 0x040001C6 RID: 454
		public static CustomOption dynamicMap;

		// Token: 0x040001C7 RID: 455
		public static CustomOption dynamicMapEnableSkeld;

		// Token: 0x040001C8 RID: 456
		public static CustomOption dynamicMapEnableMira;

		// Token: 0x040001C9 RID: 457
		public static CustomOption dynamicMapEnablePolus;

		// Token: 0x040001CA RID: 458
		public static CustomOption dynamicMapEnableAirShip;

		// Token: 0x040001CB RID: 459
		public static CustomOption dynamicMapEnableFungle;

		// Token: 0x040001CC RID: 460
		public static CustomOption dynamicMapEnableSubmerged;

		// Token: 0x040001CD RID: 461
		public static CustomOption dynamicMapSeparateSettings;

		// Token: 0x040001CE RID: 462
		public static CustomOption guesserGamemodeCrewNumber;

		// Token: 0x040001CF RID: 463
		public static CustomOption guesserGamemodeNeutralNumber;

		// Token: 0x040001D0 RID: 464
		public static CustomOption guesserGamemodeImpNumber;

		// Token: 0x040001D1 RID: 465
		public static CustomOption guesserForceJackalGuesser;

		// Token: 0x040001D2 RID: 466
		public static CustomOption guesserForceThiefGuesser;

		// Token: 0x040001D3 RID: 467
		public static CustomOption guesserGamemodeHaveModifier;

		// Token: 0x040001D4 RID: 468
		public static CustomOption guesserGamemodeNumberOfShots;

		// Token: 0x040001D5 RID: 469
		public static CustomOption guesserGamemodeHasMultipleShotsPerMeeting;

		// Token: 0x040001D6 RID: 470
		public static CustomOption guesserGamemodeKillsThroughShield;

		// Token: 0x040001D7 RID: 471
		public static CustomOption guesserGamemodeEvilCanKillSpy;

		// Token: 0x040001D8 RID: 472
		public static CustomOption guesserGamemodeCantGuessSnitchIfTaksDone;

		// Token: 0x040001D9 RID: 473
		public static CustomOption guesserGamemodeSidekickIsAlwaysGuesser;

		// Token: 0x040001DA RID: 474
		public static CustomOption hideNSeekHunterCount;

		// Token: 0x040001DB RID: 475
		public static CustomOption hideNSeekKillCooldown;

		// Token: 0x040001DC RID: 476
		public static CustomOption hideNSeekHunterVision;

		// Token: 0x040001DD RID: 477
		public static CustomOption hideNSeekHuntedVision;

		// Token: 0x040001DE RID: 478
		public static CustomOption hideNSeekTimer;

		// Token: 0x040001DF RID: 479
		public static CustomOption hideNSeekCommonTasks;

		// Token: 0x040001E0 RID: 480
		public static CustomOption hideNSeekShortTasks;

		// Token: 0x040001E1 RID: 481
		public static CustomOption hideNSeekLongTasks;

		// Token: 0x040001E2 RID: 482
		public static CustomOption hideNSeekTaskWin;

		// Token: 0x040001E3 RID: 483
		public static CustomOption hideNSeekTaskPunish;

		// Token: 0x040001E4 RID: 484
		public static CustomOption hideNSeekCanSabotage;

		// Token: 0x040001E5 RID: 485
		public static CustomOption hideNSeekMap;

		// Token: 0x040001E6 RID: 486
		public static CustomOption hideNSeekHunterWaiting;

		// Token: 0x040001E7 RID: 487
		public static CustomOption hunterLightCooldown;

		// Token: 0x040001E8 RID: 488
		public static CustomOption hunterLightDuration;

		// Token: 0x040001E9 RID: 489
		public static CustomOption hunterLightVision;

		// Token: 0x040001EA RID: 490
		public static CustomOption hunterLightPunish;

		// Token: 0x040001EB RID: 491
		public static CustomOption hunterAdminCooldown;

		// Token: 0x040001EC RID: 492
		public static CustomOption hunterAdminDuration;

		// Token: 0x040001ED RID: 493
		public static CustomOption hunterAdminPunish;

		// Token: 0x040001EE RID: 494
		public static CustomOption hunterArrowCooldown;

		// Token: 0x040001EF RID: 495
		public static CustomOption hunterArrowDuration;

		// Token: 0x040001F0 RID: 496
		public static CustomOption hunterArrowPunish;

		// Token: 0x040001F1 RID: 497
		public static CustomOption huntedShieldCooldown;

		// Token: 0x040001F2 RID: 498
		public static CustomOption huntedShieldDuration;

		// Token: 0x040001F3 RID: 499
		public static CustomOption huntedShieldRewindTime;

		// Token: 0x040001F4 RID: 500
		public static CustomOption huntedShieldNumber;

		// Token: 0x040001F5 RID: 501
		public static CustomOption propHuntMap;

		// Token: 0x040001F6 RID: 502
		public static CustomOption propHuntTimer;

		// Token: 0x040001F7 RID: 503
		public static CustomOption propHuntNumberOfHunters;

		// Token: 0x040001F8 RID: 504
		public static CustomOption hunterInitialBlackoutTime;

		// Token: 0x040001F9 RID: 505
		public static CustomOption hunterMissCooldown;

		// Token: 0x040001FA RID: 506
		public static CustomOption hunterHitCooldown;

		// Token: 0x040001FB RID: 507
		public static CustomOption hunterMaxMissesBeforeDeath;

		// Token: 0x040001FC RID: 508
		public static CustomOption propBecomesHunterWhenFound;

		// Token: 0x040001FD RID: 509
		public static CustomOption propHunterVision;

		// Token: 0x040001FE RID: 510
		public static CustomOption propVision;

		// Token: 0x040001FF RID: 511
		public static CustomOption propHuntRevealCooldown;

		// Token: 0x04000200 RID: 512
		public static CustomOption propHuntRevealDuration;

		// Token: 0x04000201 RID: 513
		public static CustomOption propHuntRevealPunish;

		// Token: 0x04000202 RID: 514
		public static CustomOption propHuntUnstuckCooldown;

		// Token: 0x04000203 RID: 515
		public static CustomOption propHuntUnstuckDuration;

		// Token: 0x04000204 RID: 516
		public static CustomOption propHuntInvisCooldown;

		// Token: 0x04000205 RID: 517
		public static CustomOption propHuntInvisDuration;

		// Token: 0x04000206 RID: 518
		public static CustomOption propHuntSpeedboostCooldown;

		// Token: 0x04000207 RID: 519
		public static CustomOption propHuntSpeedboostDuration;

		// Token: 0x04000208 RID: 520
		public static CustomOption propHuntSpeedboostSpeed;

		// Token: 0x04000209 RID: 521
		public static CustomOption propHuntSpeedboostEnabled;

		// Token: 0x0400020A RID: 522
		public static CustomOption propHuntInvisEnabled;

		// Token: 0x0400020B RID: 523
		public static CustomOption propHuntAdminCooldown;

		// Token: 0x0400020C RID: 524
		public static CustomOption propHuntFindCooldown;

		// Token: 0x0400020D RID: 525
		public static CustomOption propHuntFindDuration;

		// Token: 0x0400020E RID: 526
		public static CustomOption AddVents;

		// Token: 0x0400020F RID: 527
		public static CustomOption addPolusVents;

		// Token: 0x04000210 RID: 528
		public static CustomOption addAirShipVents;

		// Token: 0x04000211 RID: 529
		public static CustomOption enableAirShipModify;

		// Token: 0x04000212 RID: 530
		public static CustomOption enableBetterPolus;

		public static CustomOption modifierLastImpostor;

        // Token: 0x04000213 RID: 531
        internal static Dictionary<byte, byte[]> blockedRolePairings = new Dictionary<byte, byte[]>();
	}
}
