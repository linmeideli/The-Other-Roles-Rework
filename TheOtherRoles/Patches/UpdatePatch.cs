using System.Collections.Generic;
using System.Linq;
using AmongUs.GameOptions;
using HarmonyLib;
using InnerNet;
using Rewired;
using RewiredConsts;
using TheOtherRoles.CustomGameModes;
using TheOtherRoles.Modules;
using TheOtherRoles.Objects;
using TheOtherRoles.Utilities;
using UnityEngine;
using static TheOtherRoles.TheOtherRoles;

namespace TheOtherRoles.Patches;

[HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
internal class HudManagerUpdatePatch
{
    private static readonly Dictionary<byte, (string name, Color color)> TagColorDict = new();

    private static void resetNameTagsAndColors()
    {
        var localPlayer = PlayerControl.LocalPlayer;
        var myData = PlayerControl.LocalPlayer.Data;
        var amImpostor = myData.Role.IsImpostor;
        var morphTimerNotUp = Morphling.morphTimer > 0f;
        var morphTargetNotNull = Morphling.morphTarget != null;

        var dict = TagColorDict;
        dict.Clear();

        foreach (var data in GameData.Instance.AllPlayers.GetFastEnumerator())
        {
            var player = data.Object;
            var text = data.PlayerName;
            Color color;
            if (player)
            {
                var playerName = text;
                if (morphTimerNotUp && morphTargetNotNull && Morphling.morphling == player)
                    playerName = Morphling.morphTarget.Data.PlayerName;
                var nameText = player.cosmetics.nameText;

                nameText.text = Helpers.hidePlayerName(localPlayer, player) ? "" : playerName;
                nameText.color = color = amImpostor && data.Role.IsImpostor ? Palette.ImpostorRed : Color.white;
                nameText.color = nameText.color.SetAlpha(Chameleon.visibility(player.PlayerId));
            }
            else
            {
                color = Color.white;
            }


            dict.Add(data.PlayerId, (text, color));
        }

        if (MeetingHud.Instance != null)
            foreach (var playerVoteArea in MeetingHud.Instance.playerStates)
            {
                var data = dict[playerVoteArea.TargetPlayerId];
                var text = playerVoteArea.NameText;
                text.text = data.name;
                text.color = data.color;
            }
    }

    private static void setPlayerNameColor(PlayerControl p, Color color)
    {
        p.cosmetics.nameText.color = color.SetAlpha(Chameleon.visibility(p.PlayerId));
        if (MeetingHud.Instance != null)
            foreach (var player in MeetingHud.Instance.playerStates)
                if (player.NameText != null && p.PlayerId == player.TargetPlayerId)
                    player.NameText.color = color;
    }

    private static void setNameColors()
    {
        var localPlayer = PlayerControl.LocalPlayer;
        var localRole = RoleInfo.getRoleInfoForPlayer(localPlayer, false).FirstOrDefault();
        setPlayerNameColor(localPlayer, localRole.color);

        /*if (Jester.jester != null && Jester.jester == localPlayer)
            setPlayerNameColor(Jester.jester, Jester.color);
        else if (Mayor.mayor != null && Mayor.mayor == localPlayer)
            setPlayerNameColor(Mayor.mayor, Mayor.color);
        else if (Engineer.engineer != null && Engineer.engineer == localPlayer)
            setPlayerNameColor(Engineer.engineer, Engineer.color);
        else if (Sheriff.sheriff != null && Sheriff.sheriff == localPlayer) {
            setPlayerNameColor(Sheriff.sheriff, Sheriff.color);
            if (Deputy.deputy != null && Deputy.knowsSheriff) {
                setPlayerNameColor(Deputy.deputy, Deputy.color);
            }
        } else*/
        if (Deputy.deputy != null && Deputy.deputy == localPlayer)
        {
            setPlayerNameColor(Deputy.deputy, Deputy.color);
            if (Sheriff.sheriff != null && Deputy.knowsSheriff) setPlayerNameColor(Sheriff.sheriff, Sheriff.color);
        } /*else if (Portalmaker.portalmaker != null && Portalmaker.portalmaker == localPlayer)
            setPlayerNameColor(Portalmaker.portalmaker, Portalmaker.color);
        else if (Lighter.lighter != null && Lighter.lighter == localPlayer)
            setPlayerNameColor(Lighter.lighter, Lighter.color);
        else if (Detective.detective != null && Detective.detective == localPlayer)
            setPlayerNameColor(Detective.detective, Detective.color);
        else if (TimeMaster.timeMaster != null && TimeMaster.timeMaster == localPlayer)
            setPlayerNameColor(TimeMaster.timeMaster, TimeMaster.color);
        else if (Medic.medic != null && Medic.medic == localPlayer)
            setPlayerNameColor(Medic.medic, Medic.color);
        else if (Shifter.shifter != null && Shifter.shifter == localPlayer)
            setPlayerNameColor(Shifter.shifter, Shifter.color);
        else if (Swapper.swapper != null && Swapper.swapper == localPlayer)
            setPlayerNameColor(Swapper.swapper, Swapper.color);
        else if (Seer.seer != null && Seer.seer == localPlayer)
            setPlayerNameColor(Seer.seer, Seer.color);
        else if (Hacker.hacker != null && Hacker.hacker == localPlayer)
            setPlayerNameColor(Hacker.hacker, Hacker.color);
        else if (Tracker.tracker != null && Tracker.tracker == localPlayer)
            setPlayerNameColor(Tracker.tracker, Tracker.color);
        else if (Snitch.snitch != null && Snitch.snitch == localPlayer)
            setPlayerNameColor(Snitch.snitch, Snitch.color);*/
        else if (Jackal.jackal != null && Jackal.jackal == localPlayer)
        {
            // Jackal can see his sidekick
            setPlayerNameColor(Jackal.jackal, Jackal.color);
            if (Sidekick.sidekick != null) setPlayerNameColor(Sidekick.sidekick, Jackal.color);
            if (Jackal.fakeSidekick != null) setPlayerNameColor(Jackal.fakeSidekick, Jackal.color);
        }
        /*else if (Spy.spy != null && Spy.spy == localPlayer) {
            setPlayerNameColor(Spy.spy, Spy.color);
        } else if (SecurityGuard.securityGuard != null && SecurityGuard.securityGuard == localPlayer) {
            setPlayerNameColor(SecurityGuard.securityGuard, SecurityGuard.color);
        } else if (Arsonist.arsonist != null && Arsonist.arsonist == localPlayer) {
            setPlayerNameColor(Arsonist.arsonist, Arsonist.color);
        } else if (Guesser.niceGuesser != null && Guesser.niceGuesser == localPlayer) {
            setPlayerNameColor(Guesser.niceGuesser, Guesser.color);
        } else if (Guesser.evilGuesser != null && Guesser.evilGuesser == localPlayer) {
            setPlayerNameColor(Guesser.evilGuesser, Palette.ImpostorRed);
        } else if (Vulture.vulture != null && Vulture.vulture == localPlayer) {
            setPlayerNameColor(Vulture.vulture, Vulture.color);
        } else if (Medium.medium != null && Medium.medium == localPlayer) {
            setPlayerNameColor(Medium.medium, Medium.color);
        } else if (Trapper.trapper != null && Trapper.trapper == localPlayer) {
            setPlayerNameColor(Trapper.trapper, Trapper.color);
        } else if (Lawyer.lawyer != null && Lawyer.lawyer == localPlayer) {
            setPlayerNameColor(Lawyer.lawyer, Lawyer.color);
        } else if (Pursuer.pursuer != null && Pursuer.pursuer == localPlayer) {
            setPlayerNameColor(Pursuer.pursuer, Pursuer.color);
        }*/

        // No else if here, as a Lover of team Jackal needs the colors
        if (Sidekick.sidekick != null && Sidekick.sidekick == localPlayer)
        {
            // Sidekick can see the jackal
            setPlayerNameColor(Sidekick.sidekick, Sidekick.color);
            if (Jackal.jackal != null) setPlayerNameColor(Jackal.jackal, Jackal.color);
        }

        // No else if here, as the Impostors need the Spy name to be colored
        if (Spy.spy != null && localPlayer.Data.Role.IsImpostor) setPlayerNameColor(Spy.spy, Spy.color);
        if (Sidekick.sidekick != null && Sidekick.wasTeamRed && localPlayer.Data.Role.IsImpostor)
            setPlayerNameColor(Sidekick.sidekick, Spy.color);
        if (Jackal.jackal != null && Jackal.wasTeamRed && localPlayer.Data.Role.IsImpostor)
            setPlayerNameColor(Jackal.jackal, Spy.color);

        // Crewmate roles with no changes: Mini
        // Impostor roles with no changes: Morphling, Camouflager, Vampire, Godfather, Eraser, Janitor, Cleaner, Warlock, BountyHunter,  Witch and Mafioso
    }

    private static void setNameTags()
    {
        // Mafia
        if (PlayerControl.LocalPlayer != null && PlayerControl.LocalPlayer.Data.Role.IsImpostor)
        {
            foreach (var player in PlayerControl.AllPlayerControls)
                if (Godfather.godfather != null && Godfather.godfather == player)
                    player.cosmetics.nameText.text = player.Data.PlayerName + "setNameTagsG".Translate();
                else if (Mafioso.mafioso != null && Mafioso.mafioso == player)
                    player.cosmetics.nameText.text = player.Data.PlayerName + "setNameTagsM".Translate();
                else if (Janitor.janitor != null && Janitor.janitor == player)
                    player.cosmetics.nameText.text = player.Data.PlayerName + "setNameTagsJ".Translate();
            if (MeetingHud.Instance != null)
                foreach (var player in MeetingHud.Instance.playerStates)
                    if (Godfather.godfather != null && Godfather.godfather.PlayerId == player.TargetPlayerId)
                        player.NameText.text = Godfather.godfather.Data.PlayerName + "setNameTagsG".Translate();
                    else if (Mafioso.mafioso != null && Mafioso.mafioso.PlayerId == player.TargetPlayerId)
                        player.NameText.text = Mafioso.mafioso.Data.PlayerName + "setNameTagsM".Translate();
                    else if (Janitor.janitor != null && Janitor.janitor.PlayerId == player.TargetPlayerId)
                        player.NameText.text = Janitor.janitor.Data.PlayerName + "setNameTagsJ".Translate();
        }
        //Fraudster
        if (PlayerControl.LocalPlayer != null && PlayerControl.LocalPlayer.Data.Role.IsImpostor && Fraudster.fraudster != null && Fraudster.fraudster.Data.IsDead && Sheriff.sheriff != null )
        {
            foreach (PlayerControl player in PlayerControl.AllPlayerControls.ToArray())
                if (Sheriff.sheriff != null && Sheriff.sheriff == player)
                    player.cosmetics.nameText.text += $" ({"sheriff".Translate()})";
                

            if (MeetingHud.Instance != null)
                foreach (PlayerVoteArea player in MeetingHud.Instance.playerStates)
                    if (Sheriff.sheriff != null && Sheriff.sheriff.PlayerId == player.TargetPlayerId)
                        player.NameText.text +=  $" ({"sheriff".Translate()})";
                    
        }
        if (PlayerControl.LocalPlayer != null && PlayerControl.LocalPlayer.Data.Role.IsImpostor && Fraudster.fraudster != null && Fraudster.fraudster.Data.IsDead && Deputy.deputy != null)
        {
            foreach (PlayerControl player in PlayerControl.AllPlayerControls.ToArray())
                if (Deputy.deputy != null && Deputy.deputy == player)
                    player.cosmetics.nameText.text += $" ({"deputy".Translate()})";

            if (MeetingHud.Instance != null)
                foreach (PlayerVoteArea player in MeetingHud.Instance.playerStates)
                    if (Deputy.deputy != null && Deputy.deputy.PlayerId == player.TargetPlayerId)
                        player.NameText.text += $" ({"deputy".Translate()})";
        }

        // Lovers
        if (Lovers.lover1 != null && Lovers.lover2 != null && (Lovers.lover1 == PlayerControl.LocalPlayer ||
                                                               Lovers.lover2 == PlayerControl.LocalPlayer))
        {
            var suffix = Helpers.cs(Lovers.color, " ♥");
            Lovers.lover1.cosmetics.nameText.text += suffix;
            Lovers.lover2.cosmetics.nameText.text += suffix;

            if (MeetingHud.Instance != null)
                foreach (var player in MeetingHud.Instance.playerStates)
                    if (Lovers.lover1.PlayerId == player.TargetPlayerId ||
                        Lovers.lover2.PlayerId == player.TargetPlayerId)
                        player.NameText.text += suffix;
        }

        // Lawyer or Prosecutor
        if (Lawyer.lawyer != null && Lawyer.target != null && Lawyer.lawyer == PlayerControl.LocalPlayer)
        {
            var color = Lawyer.color;
            var target = Lawyer.target;
            var suffix = Helpers.cs(color, " §");
            target.cosmetics.nameText.text += suffix;

            if (MeetingHud.Instance != null)
                foreach (var player in MeetingHud.Instance.playerStates)
                    if (player.TargetPlayerId == target.PlayerId)
                        player.NameText.text += suffix;
        }

        // Former Thief
        if (Thief.formerThief != null &&
            (Thief.formerThief == PlayerControl.LocalPlayer || PlayerControl.LocalPlayer.Data.IsDead))
        {
            var suffix = Helpers.cs(Thief.color, " $");
            Thief.formerThief.cosmetics.nameText.text += suffix;
            if (MeetingHud.Instance != null)
                foreach (var player in MeetingHud.Instance.playerStates)
                    if (player.TargetPlayerId == Thief.formerThief.PlayerId)
                        player.NameText.text += suffix;
        }

        // Display lighter / darker color for all alive players
        if (PlayerControl.LocalPlayer != null && MeetingHud.Instance != null && TORMapOptions.showLighterDarker)
            foreach (var player in MeetingHud.Instance.playerStates)
            {
                var target = Helpers.playerById(player.TargetPlayerId);
                if (target != null) player.NameText.text += $" ({(Helpers.isLighterColor(target) ? "colorLightMax".Translate() : "colorDarkMax".Translate())})";
            }

        // Add medic shield info:
        if (MeetingHud.Instance != null && Medic.medic != null && Medic.shielded != null &&
            Medic.shieldVisible(Medic.shielded))
            foreach (var player in MeetingHud.Instance.playerStates)
                if (player.TargetPlayerId == Medic.shielded.PlayerId)
                    player.NameText.text = Helpers.cs(Medic.color, "[") + player.NameText.text +
                                           Helpers.cs(Medic.color, "]");
        // player.HighlightedFX.color = Medic.color;
        // player.HighlightedFX.enabled = true;
    }

    private static void updateShielded()
    {
        if (Medic.shielded == null) return;

        if (Medic.shielded.Data.IsDead || Medic.medic == null || Medic.medic.Data.IsDead) Medic.shielded = null;
    }

    private static void timerUpdate()
    {
        var dt = Time.deltaTime;
        Hacker.hackerTimer -= dt;
        Trickster.lightsOutTimer -= dt;
        Tracker.corpsesTrackingTimer -= dt;
        Ninja.invisibleTimer -= dt;
        HideNSeek.timer -= dt;
        foreach (var key in Deputy.handcuffedKnows.Keys)
            Deputy.handcuffedKnows[key] -= dt;
    }

    public static void miniUpdate()
    {
        if (Mini.mini == null || Camouflager.camouflageTimer > 0f || Helpers.MushroomSabotageActive() ||
            (Mini.mini == Morphling.morphling && Morphling.morphTimer > 0f) ||
            (Mini.mini == Ninja.ninja && Ninja.isInvisble) || SurveillanceMinigamePatch.nightVisionIsActive) return;

        var growingProgress = Mini.growingProgress();
        var scale = growingProgress * 0.35f + 0.35f;
        var suffix = "";
        if (growingProgress != 1f)
            suffix = " <color=#FAD934FF>(" + Mathf.FloorToInt(growingProgress * 18) + ")</color>";
        if (!Mini.isGrowingUpInMeeting && MeetingHud.Instance != null && Mini.ageOnMeetingStart != 0 &&
            !(Mini.ageOnMeetingStart >= 18))
            suffix = " <color=#FAD934FF>(" + Mini.ageOnMeetingStart + ")</color>";

        Mini.mini.cosmetics.nameText.text += suffix;
        if (MeetingHud.Instance != null)
            foreach (var player in MeetingHud.Instance.playerStates)
                if (player.NameText != null && Mini.mini.PlayerId == player.TargetPlayerId)
                    player.NameText.text += suffix;

        if (Morphling.morphling != null && Morphling.morphTarget == Mini.mini && Morphling.morphTimer > 0f)
            Morphling.morphling.cosmetics.nameText.text += suffix;
    }

    private static void updateImpostorKillButton(HudManager __instance)
    {
        if (!PlayerControl.LocalPlayer.Data.Role.IsImpostor) return;
        if (MeetingHud.Instance)
        {
            __instance.KillButton.Hide();
            return;
        }

        var enabled = true;
        if (Vampire.vampire != null && Vampire.vampire == PlayerControl.LocalPlayer)
            enabled = false;
        else if (Mafioso.mafioso != null && Mafioso.mafioso == PlayerControl.LocalPlayer &&
                 Godfather.godfather != null && !Godfather.godfather.Data.IsDead)
            enabled = false;
        else if (Janitor.janitor != null && Janitor.janitor == PlayerControl.LocalPlayer)
            enabled = false;

        if (enabled) __instance.KillButton.Show();
        else __instance.KillButton.Hide();

        if (Deputy.handcuffedKnows.ContainsKey(PlayerControl.LocalPlayer.PlayerId) &&
            Deputy.handcuffedKnows[PlayerControl.LocalPlayer.PlayerId] > 0) __instance.KillButton.Hide();
    }

    private static void updateReportButton(HudManager __instance)
    {
        if (GameOptionsManager.Instance.currentGameOptions.GameMode == GameModes.HideNSeek) return;
        if ((Deputy.handcuffedKnows.ContainsKey(PlayerControl.LocalPlayer.PlayerId) &&
             Deputy.handcuffedKnows[PlayerControl.LocalPlayer.PlayerId] > 0) ||
            MeetingHud.Instance) __instance.ReportButton.Hide();
        else if (!__instance.ReportButton.isActiveAndEnabled) __instance.ReportButton.Show();
    }

    private static void updateVentButton(HudManager __instance)
    {
        if (GameOptionsManager.Instance.currentGameOptions.GameMode == GameModes.HideNSeek) return;
        if ((Deputy.handcuffedKnows.ContainsKey(PlayerControl.LocalPlayer.PlayerId) &&
             Deputy.handcuffedKnows[PlayerControl.LocalPlayer.PlayerId] > 0) ||
            MeetingHud.Instance) __instance.ImpostorVentButton.Hide();
        else if (PlayerControl.LocalPlayer.roleCanUseVents() && !__instance.ImpostorVentButton.isActiveAndEnabled)
            __instance.ImpostorVentButton.Show();
        if (ReInput.players.GetPlayer(0).GetButtonDown(Action.UseVent) &&
            !PlayerControl.LocalPlayer.Data.Role.IsImpostor &&
            PlayerControl.LocalPlayer.roleCanUseVents()) __instance.ImpostorVentButton.DoClick();
    }

    private static void updateUseButton(HudManager __instance)
    {
        if (MeetingHud.Instance) __instance.UseButton.Hide();
    }

    private static void updateSabotageButton(HudManager __instance)
    {
        if (MeetingHud.Instance || TORMapOptions.gameMode == CustomGamemodes.HideNSeek ||
            TORMapOptions.gameMode == CustomGamemodes.PropHunt) __instance.SabotageButton.Hide();
        if (PlayerControl.LocalPlayer.Data.IsDead && CustomOptionHolder.deadImpsBlockSabotage.getBool())
            __instance.SabotageButton.Hide();
    }

    private static void updateMapButton(HudManager __instance)
    {
        if (Trapper.trapper == null || !(PlayerControl.LocalPlayer.PlayerId == Trapper.trapper.PlayerId) ||
            __instance == null || __instance.MapButton.HeldButtonSprite == null) return;
        __instance.MapButton.HeldButtonSprite.color = Trapper.playersOnMap.Any() ? Trapper.color : Color.white;
    }

    private static void Postfix(HudManager __instance)
    {
        if (AmongUsClient.Instance.GameState != InnerNetClient.GameStates.Started ||
            GameOptionsManager.Instance.currentGameOptions.GameMode == GameModes.HideNSeek) return;

        EventUtility.Update();

        CustomButton.HudUpdate();
        resetNameTagsAndColors();
        setNameColors();
        updateShielded();
        setNameTags();

        // Impostors
        updateImpostorKillButton(__instance);
        // Timer updates
        timerUpdate();
        // Mini
        miniUpdate();

        // Deputy Sabotage, Use and Vent Button Disabling
        updateReportButton(__instance);
        updateVentButton(__instance);
        // Meeting hide buttons if needed (used for the map usage, because closing the map would show buttons)
        updateSabotageButton(__instance);
        updateUseButton(__instance);
        updateMapButton(__instance);
        if (!MeetingHud.Instance) __instance.AbilityButton?.Update();

        // Fix dead player's pets being visible by just always updating whether the pet should be visible at all.
        foreach (var target in PlayerControl.AllPlayerControls)
        {
            var pet = target.GetPet();
            if (pet != null)
                pet.Visible = ((PlayerControl.LocalPlayer.Data.IsDead && target.Data.IsDead) || !target.Data.IsDead) &&
                              !target.inVent;
        }
    }
}