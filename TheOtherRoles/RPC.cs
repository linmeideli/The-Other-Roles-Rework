using System;
using System.Collections.Generic;
using System.Linq;
using AmongUs.Data;
using AmongUs.GameOptions;
using Assets.CoreScripts;
using HarmonyLib;
using Hazel;
using InnerNet;
using PowerTools;
using Reactor.Utilities.Extensions;
using TheOtherRoles.CustomGameModes;
using TheOtherRoles.Modules;
using TheOtherRoles.Objects;
using TheOtherRoles.Patches;
using TheOtherRoles.Utilities;
using TMPro;
using UnityEngine;
using static TheOtherRoles.TheOtherRoles;
using static TheOtherRoles.HudManagerStartPatch;
using static TheOtherRoles.GameHistory;
using static TheOtherRoles.TORMapOptions;
using Object = UnityEngine.Object;
using Il2CppSystem.Collections.Generic;
using static UnityEngine.GraphicsBuffer;
using TheOtherRoles;

namespace TheOtherRoles;

public enum RoleId
{
    Jester,
    Mayor,
    Portalmaker,
    Engineer,
    Sheriff,
    Deputy,
    Lighter,
    Godfather,
    Mafioso,
    Janitor,
    Detective,
    TimeMaster,
    Medic,
    Swapper,
    Seer,
    Morphling,
    Camouflager,
    Hacker,
    Tracker,
    Vampire,
    Snitch,
    Jackal,
    Sidekick,
    Eraser,
    Spy,
    Trickster,
    Cleaner,
    Warlock,
    SecurityGuard,
    Arsonist,
    EvilGuesser,
    NiceGuesser,
    BountyHunter,
    Vulture,
    Medium,
    Trapper,
    Lawyer,
    Prosecutor,
    Pursuer,
    Witch,
    Ninja,
    Thief,
    Bomber,
    Yoyo,
    Fraudster,
    Devil,
    Prophet,
    PeaceDove,
    Crewmate,
    Impostor,

    // Modifier ---
    Lover,
    Bait,
    Bloody,
    AntiTeleport,
    Tiebreaker,
    Sunglasses,
    Mini,
    Vip,
    Invert,
    Chameleon,
    Armored,
    Shifter
}

internal enum CustomRPC
{
    // Main Controls

    ResetVaribles = 100,
    ShareOptions,
    ForceEnd,
    WorkaroundSetRoles,
    SetRole,
    SetModifier,
    VersionHandshake,
    UseUncheckedVent,
    UncheckedMurderPlayer,
    UncheckedCmdReportDeadBody,
    UncheckedExilePlayer,
    DynamicMapOption,
    SetGameStarting,
    ShareGamemode,
    StopStart,

    // Role functionality

    EngineerFixLights = 120,
    EngineerFixSubmergedOxygen,
    EngineerUsedRepair,
    CleanBody,
    MedicSetShielded,
    ShieldedMurderAttempt,
    TimeMasterShield,
    TimeMasterRewindTime,
    ShifterShift,
    SwapperSwap,
    MorphlingMorph,
    CamouflagerCamouflage,
    TrackerUsedTracker,
    VampireSetBitten,
    PlaceGarlic,
    DeputyUsedHandcuffs,
    DeputyPromotes,
    JackalCreatesSidekick,
    SidekickPromotes,
    ErasePlayerRoles,
    BlindPlayerVision,
    ShowBlindedReport,
    SetFutureErased,
    SetFutureShifted,
    SetFutureShielded,
    SetFutureSpelled,
    SetFutureBlinded,
    PlaceNinjaTrace,
    PlacePortal,
    UsePortal,
    PlaceJackInTheBox,
    LightsOut,
    PlaceCamera,
    SealVent,
    ArsonistWin,
    GuesserShoot,
    LawyerSetTarget,
    LawyerPromotesToPursuer,
    SetBlanked,
    Bloody,
    SetFirstKill,
    Invert,
    SetTiebreak,
    SetInvisible,
    ThiefStealsRole,
    SetTrap,
    TriggerTrap,
    MayorSetVoteTwice,
    PlaceBomb,
    DefuseBomb,
    ShareRoom,
    YoyoMarkLocation,
    YoyoBlink,
    BreakArmor,
    Suicide,
    SuicideMeeting,
    ProphetExamine,
    ReloadCooldowns,

    // Gamemode
    SetGuesserGm,
    HuntedShield,
    HuntedRewindTime,
    SetProp,
    SetRevealed,
    PropHuntStartTimer,
    PropHuntSetInvis,
    PropHuntSetSpeedboost,
    DraftModePickOrder,
    DraftModePick,

    // Other functionality
    ShareTimer,
    ShareGhostInfo,
    EventKick
}

public static class RPCProcedure
{
    public enum GhostInfoTypes
    {
        HandcuffNoticed,
        HandcuffOver,
        ArsonistDouse,
        BountyTarget,
        NinjaMarked,
        WarlockTarget,
        MediumInfo,
        BlankUsed,
        DetectiveOrMedicInfo,
        VampireTimer,
        DeathReasonAndKiller
    }

    // Main Controls

    public static void resetVariables()
    {
        Garlic.clearGarlics();
        JackInTheBox.clearJackInTheBoxes();
        NinjaTrace.clearTraces();
        Silhouette.clearSilhouettes();
        Portal.clearPortals();
        Bloodytrail.resetSprites();
        Trap.clearTraps();
        clearAndReloadMapOptions();
        clearAndReloadRoles();
        clearGameHistory();
        setCustomButtonCooldowns();
        CustomButton.ReloadHotkeys();
        reloadPluginOptions();
        Helpers.toggleZoom(true);
        GameStartManagerPatch.GameStartManagerUpdatePatch.startingTimer = 0;
        SurveillanceMinigamePatch.nightVisionOverlays = null;
        EventUtility.clearAndReload();
        MapBehaviourPatch.clearAndReload();
        HudManagerUpdate.CloseSummary();
    }

    public static void HandleShareOptions(byte numberOfOptions, MessageReader reader)
    {
        try
        {
            for (var i = 0; i < numberOfOptions; i++)
            {
                var optionId = reader.ReadPackedUInt32();
                var selection = reader.ReadPackedUInt32();
                var option = CustomOption.options.First(option => option.id == (int)optionId);
                option.updateSelection((int)selection, i == numberOfOptions - 1);
            }
        }
        catch (Exception e)
        {
            TheOtherRolesPlugin.Logger.LogError("Error while deserializing options: " + e.Message);
        }
    }

    public static void forceEnd()
    {
        if (AmongUsClient.Instance.GameState != InnerNetClient.GameStates.Started) return;
        foreach (var player in PlayerControl.AllPlayerControls)
            if (!player.Data.Role.IsImpostor)
            {
                GameData.Instance
                    .GetPlayerById(player
                        .PlayerId); // player.RemoveInfected(); (was removed in 2022.12.08, no idea if we ever need that part again, replaced by these 2 lines.) 
                player.CoSetRole(RoleTypes.Crewmate, true);

                player.MurderPlayer(player);
                player.Data.IsDead = true;
            }
    }

    public static void shareGamemode(byte gm)
    {
        gameMode = (CustomGamemodes)gm;
        LobbyViewSettingsPatch.currentButtons?.ForEach(x => x.gameObject?.Destroy());
        LobbyViewSettingsPatch.currentButtons?.Clear();
        LobbyViewSettingsPatch.currentButtonTypes?.Clear();
    }

    public static void stopStart(byte playerId)
    {
        if (!CustomOptionHolder.anyPlayerCanStopStart.getBool())
            return;
        SoundManager.Instance.StopSound(GameStartManager.Instance.gameStartSound);
        if (AmongUsClient.Instance.AmHost)
        {
            GameStartManager.Instance.ResetStartState();
            PlayerControl.LocalPlayer.RpcSendChat(
                string.Format("stopStartChatText".Translate(), Helpers.playerById(playerId).Data.PlayerName));
        }
    }

    public static void workaroundSetRoles(byte numberOfRoles, MessageReader reader)
    {
        for (var i = 0; i < numberOfRoles; i++)
        {
            var playerId = (byte)reader.ReadPackedUInt32();
            var roleId = (byte)reader.ReadPackedUInt32();
            try
            {
                setRole(roleId, playerId);
            }
            catch (Exception e)
            {
                TheOtherRolesPlugin.Logger.LogError("Error while deserializing roles: " + e.Message);
            }
        }
    }

    public static void setRole(byte roleId, byte playerId)
    {
        foreach (var player in PlayerControl.AllPlayerControls)
            if (player.PlayerId == playerId)
            {
                switch ((RoleId)roleId)
                {
                    case RoleId.Jester:
                        Jester.jester = player;
                        break;
                    case RoleId.Mayor:
                        Mayor.mayor = player;
                        break;
                    case RoleId.Portalmaker:
                        Portalmaker.portalmaker = player;
                        break;
                    case RoleId.Engineer:
                        Engineer.engineer = player;
                        break;
                    case RoleId.Sheriff:
                        Sheriff.sheriff = player;
                        break;
                    case RoleId.Deputy:
                        Deputy.deputy = player;
                        break;
                    case RoleId.Lighter:
                        Lighter.lighter = player;
                        break;
                    case RoleId.Godfather:
                        Godfather.godfather = player;
                        break;
                    case RoleId.Mafioso:
                        Mafioso.mafioso = player;
                        break;
                    case RoleId.Janitor:
                        Janitor.janitor = player;
                        break;
                    case RoleId.Detective:
                        Detective.detective = player;
                        break;
                    case RoleId.TimeMaster:
                        TimeMaster.timeMaster = player;
                        break;
                    case RoleId.Medic:
                        Medic.medic = player;
                        break;
                    case RoleId.Shifter:
                        Shifter.shifter = player;
                        break;
                    case RoleId.Swapper:
                        Swapper.swapper = player;
                        break;
                    case RoleId.Seer:
                        Seer.seer = player;
                        break;
                    case RoleId.Morphling:
                        Morphling.morphling = player;
                        break;
                    case RoleId.Camouflager:
                        Camouflager.camouflager = player;
                        break;
                    case RoleId.Hacker:
                        Hacker.hacker = player;
                        break;
                    case RoleId.Tracker:
                        Tracker.tracker = player;
                        break;
                    case RoleId.Vampire:
                        Vampire.vampire = player;
                        break;
                    case RoleId.Snitch:
                        Snitch.snitch = player;
                        break;
                    case RoleId.Jackal:
                        Jackal.jackal = player;
                        break;
                    case RoleId.Sidekick:
                        Sidekick.sidekick = player;
                        break;
                    case RoleId.Eraser:
                        Eraser.eraser = player;
                        break;
                    case RoleId.Spy:
                        Spy.spy = player;
                        break;
                    case RoleId.Trickster:
                        Trickster.trickster = player;
                        break;
                    case RoleId.Cleaner:
                        Cleaner.cleaner = player;
                        break;
                    case RoleId.Warlock:
                        Warlock.warlock = player;
                        break;
                    case RoleId.SecurityGuard:
                        SecurityGuard.securityGuard = player;
                        break;
                    case RoleId.Arsonist:
                        Arsonist.arsonist = player;
                        break;
                    case RoleId.EvilGuesser:
                        Guesser.evilGuesser = player;
                        break;
                    case RoleId.NiceGuesser:
                        Guesser.niceGuesser = player;
                        break;
                    case RoleId.BountyHunter:
                        BountyHunter.bountyHunter = player;
                        break;
                    case RoleId.Vulture:
                        Vulture.vulture = player;
                        break;
                    case RoleId.Medium:
                        Medium.medium = player;
                        break;
                    case RoleId.Trapper:
                        Trapper.trapper = player;
                        break;
                    case RoleId.Lawyer:
                        Lawyer.lawyer = player;
                        break;
                    case RoleId.Prosecutor:
                        Lawyer.lawyer = player;
                        Lawyer.isProsecutor = true;
                        break;
                    case RoleId.Pursuer:
                        Pursuer.pursuer = player;
                        break;
                    case RoleId.Witch:
                        Witch.witch = player;
                        break;
                    case RoleId.Ninja:
                        Ninja.ninja = player;
                        break;
                    case RoleId.Thief:
                        Thief.thief = player;
                        break;
                    case RoleId.Bomber:
                        Bomber.bomber = player;
                        break;
                    case RoleId.Yoyo:
                        Yoyo.yoyo = player;
                        break;
                    case RoleId.Fraudster:
                        Fraudster.fraudster = player;
                        break;
                    case RoleId.Devil:
                        Devil.devil = player;
                        break;
                    case RoleId.Prophet:
                        Prophet.prophet = player;
                        break;
                    case RoleId.PeaceDove:
                        PeaceDove.peacedove = player;
                        break;
                }

                if (AmongUsClient.Instance.AmHost && player.roleCanUseVents() && !player.Data.Role.IsImpostor)
                {
                    player.RpcSetRole(RoleTypes.Engineer);
                    player.CoSetRole(RoleTypes.Engineer, true);
                }
            }
    }

    public static void setModifier(byte modifierId, byte playerId, byte flag)
    {
        var player = Helpers.playerById(playerId);
        switch ((RoleId)modifierId)
        {
            case RoleId.Bait:
                Bait.bait.Add(player);
                break;
            case RoleId.Lover:
                if (flag == 0) Lovers.lover1 = player;
                else Lovers.lover2 = player;
                break;
            case RoleId.Bloody:
                Bloody.bloody.Add(player);
                break;
            case RoleId.AntiTeleport:
                AntiTeleport.antiTeleport.Add(player);
                break;
            case RoleId.Tiebreaker:
                Tiebreaker.tiebreaker = player;
                break;
            case RoleId.Sunglasses:
                Sunglasses.sunglasses.Add(player);
                break;
            case RoleId.Mini:
                Mini.mini = player;
                break;
            case RoleId.Vip:
                Vip.vip.Add(player);
                break;
            case RoleId.Invert:
                Invert.invert.Add(player);
                break;
            case RoleId.Chameleon:
                Chameleon.chameleon.Add(player);
                break;
            case RoleId.Armored:
                Armored.armored = player;
                break;
            case RoleId.Shifter:
                Shifter.shifter = player;
                break;
        }
    }

    public static void versionHandshake(int major, int minor, int build, int revision, Guid guid, int clientId)
    {
        Version ver;
        if (revision < 0)
            ver = new Version(major, minor, build);
        else
            ver = new Version(major, minor, build, revision);
        GameStartManagerPatch.playerVersions[clientId] = new GameStartManagerPatch.PlayerVersion(ver, guid);
    }

    public static void useUncheckedVent(int ventId, byte playerId, byte isEnter)
    {
        var player = Helpers.playerById(playerId);
        if (player == null) return;
        // Fill dummy MessageReader and call MyPhysics.HandleRpc as the corountines cannot be accessed
        var reader = new MessageReader();
        var bytes = BitConverter.GetBytes(ventId);
        if (!BitConverter.IsLittleEndian)
            Array.Reverse(bytes);
        reader.Buffer = bytes;
        reader.Length = bytes.Length;

        JackInTheBox.startAnimation(ventId);
        player.MyPhysics.HandleRpc(isEnter != 0 ? (byte)19 : (byte)20, reader);
    }

    public static void uncheckedMurderPlayer(byte sourceId, byte targetId, byte showAnimation)
    {
        if (AmongUsClient.Instance.GameState != InnerNetClient.GameStates.Started) return;
        var source = Helpers.playerById(sourceId);
        var target = Helpers.playerById(targetId);
        if (source != null && target != null)
        {
            if (showAnimation == 0) KillAnimationCoPerformKillPatch.hideNextAnimation = true;
            source.MurderPlayer(target);
        }
    }

    public static void uncheckedCmdReportDeadBody(byte sourceId, byte targetId)
    {
        var source = Helpers.playerById(sourceId);
        var t = targetId == byte.MaxValue ? null : Helpers.playerById(targetId).Data;
        if (source != null) source.ReportDeadBody(t);
    }

    public static void uncheckedExilePlayer(byte targetId)
    {
        var target = Helpers.playerById(targetId);
        if (target != null) target.Exiled();
    }

    public static void dynamicMapOption(byte mapId)
    {
        GameOptionsManager.Instance.currentNormalGameOptions.MapId = mapId;
    }

    public static void setGameStarting()
    {
        GameStartManagerPatch.GameStartManagerUpdatePatch.startingTimer = 5f;
    }

    // Role functionality

    public static void engineerFixLights()
    {
        var switchSystem = MapUtilities.Systems[SystemTypes.Electrical].CastFast<SwitchSystem>();
        switchSystem.ActualSwitches = switchSystem.ExpectedSwitches;
    }

    public static void engineerFixSubmergedOxygen()
    {
        SubmergedCompatibility.RepairOxygen();
    }

    public static void engineerUsedRepair()
    {
        Engineer.remainingFixes--;
        if (Helpers.shouldShowGhostInfo())
        {
            Helpers.showFlash(Engineer.color, 0.5f, "engineerUsedRepairText".Translate());
            ;
        }
    }

    public static void cleanBody(byte playerId, byte cleaningPlayerId)
    {
        if (Medium.futureDeadBodies != null)
        {
            var deadBody = Medium.futureDeadBodies.Find(x => x.Item1.player.PlayerId == playerId).Item1;
            if (deadBody != null) deadBody.wasCleaned = true;
        }

        DeadBody[] array = Object.FindObjectsOfType<DeadBody>();
        for (var i = 0; i < array.Length; i++)
            if (GameData.Instance.GetPlayerById(array[i].ParentId).PlayerId == playerId)
                Object.Destroy(array[i].gameObject);

        if (Vulture.vulture != null && cleaningPlayerId == Vulture.vulture.PlayerId)
        {
            Vulture.eatenBodies++;
            if (Vulture.eatenBodies == Vulture.vultureNumberToWin) Vulture.triggerVultureWin = true;
        }
    }

    public static void timeMasterRewindTime()
    {
        TimeMaster.shieldActive = false; // Shield is no longer active when rewinding
        SoundEffectsManager.stop("timemasterShield"); // Shield sound stopped when rewinding
        if (TimeMaster.timeMaster != null && TimeMaster.timeMaster == PlayerControl.LocalPlayer)
            resetTimeMasterButton();
        FastDestroyableSingleton<HudManager>.Instance.FullScreen.color = new Color(0f, 0.5f, 0.8f, 0.3f);
        FastDestroyableSingleton<HudManager>.Instance.FullScreen.enabled = true;
        FastDestroyableSingleton<HudManager>.Instance.FullScreen.gameObject.SetActive(true);
        FastDestroyableSingleton<HudManager>.Instance.StartCoroutine(Effects.Lerp(TimeMaster.rewindTime / 2,
            new Action<float>(p =>
            {
                if (p == 1f) FastDestroyableSingleton<HudManager>.Instance.FullScreen.enabled = false;
            })));

        if (TimeMaster.timeMaster == null || PlayerControl.LocalPlayer == TimeMaster.timeMaster)
            return; // Time Master himself does not rewind

        TimeMaster.isRewinding = true;

        if (MapBehaviour.Instance)
            MapBehaviour.Instance.Close();
        if (Minigame.Instance)
            Minigame.Instance.ForceClose();
        PlayerControl.LocalPlayer.moveable = false;
    }

    public static void timeMasterShield()
    {
        TimeMaster.shieldActive = true;
        FastDestroyableSingleton<HudManager>.Instance.StartCoroutine(Effects.Lerp(TimeMaster.shieldDuration,
            new Action<float>(p =>
            {
                if (p == 1f) TimeMaster.shieldActive = false;
            })));
    }

    public static void medicSetShielded(byte shieldedId)
    {
        Medic.usedShield = true;
        Medic.shielded = Helpers.playerById(shieldedId);
        Medic.futureShielded = null;
    }

    public static void shieldedMurderAttempt()
    {
        if (Medic.shielded == null || Medic.medic == null) return;

        var isShieldedAndShow = Medic.shielded == PlayerControl.LocalPlayer && Medic.showAttemptToShielded;
        isShieldedAndShow =
            isShieldedAndShow &&
            (Medic.meetingAfterShielding ||
             !Medic.showShieldAfterMeeting); // Dont show attempt, if shield is not shown yet
        var isMedicAndShow = Medic.medic == PlayerControl.LocalPlayer && Medic.showAttemptToMedic;

        if (isShieldedAndShow || isMedicAndShow || Helpers.shouldShowGhostInfo())
            Helpers.showFlash(Palette.ImpostorRed, 0.5f, "shieldedMurderAttemptText".Translate());
    }

    public static void shifterShift(byte targetId)
    {
        var oldShifter = Shifter.shifter;
        var player = Helpers.playerById(targetId);
        if (player == null || oldShifter == null) return;

        Shifter.futureShift = null;
        Shifter.clearAndReload();

        // Suicide (exile) when impostor or impostor variants
        if ((player.Data.Role.IsImpostor || Helpers.isNeutral(player)) && !oldShifter.Data.IsDead)
        {
            oldShifter.Exiled();
            overrideDeathReasonAndKiller(oldShifter, DeadPlayer.CustomDeathReason.Shift, player);
            if (oldShifter == Lawyer.target && AmongUsClient.Instance.AmHost && Lawyer.lawyer != null)
            {
                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                    (byte)CustomRPC.LawyerPromotesToPursuer, SendOption.Reliable);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                lawyerPromotesToPursuer();
            }

            return;
        }

        Shifter.shiftRole(oldShifter, player);

        // Set cooldowns to max for both players
        if (PlayerControl.LocalPlayer == oldShifter || PlayerControl.LocalPlayer == player)
            CustomButton.ResetAllCooldowns();
    }

    public static void swapperSwap(byte playerId1, byte playerId2)
    {
        if (MeetingHud.Instance)
        {
            Swapper.playerId1 = playerId1;
            Swapper.playerId2 = playerId2;
        }
    }

    public static void morphlingMorph(byte playerId)
    {
        var target = Helpers.playerById(playerId);
        if (Morphling.morphling == null || target == null) return;

        Morphling.morphTimer = Morphling.duration;
        Morphling.morphTarget = target;
        if (Camouflager.camouflageTimer <= 0f)
            Morphling.morphling.setLook(target.Data.PlayerName, target.Data.DefaultOutfit.ColorId,
                target.Data.DefaultOutfit.HatId, target.Data.DefaultOutfit.VisorId, target.Data.DefaultOutfit.SkinId,
                target.Data.DefaultOutfit.PetId);
    }

    public static void camouflagerCamouflage()
    {
        if (Camouflager.camouflager == null) return;

        Camouflager.camouflageTimer = Camouflager.duration;
        if (Helpers.MushroomSabotageActive()) return; // Dont overwrite the fungle "camo"
        foreach (var player in PlayerControl.AllPlayerControls)
            player.setLook("", 6, "", "", "", "");
    }

    public static void vampireSetBitten(byte targetId, byte performReset)
    {
        if (performReset != 0)
        {
            Vampire.bitten = null;
            return;
        }

        if (Vampire.vampire == null) return;
        foreach (var player in PlayerControl.AllPlayerControls)
            if (player.PlayerId == targetId && !player.Data.IsDead)
                Vampire.bitten = player;
    }
    public static void serialKillerSuicide(byte PlayerId)
    {
        PlayerControl playerid = Helpers.playerById(PlayerId);
        if (playerid == null) return;
        playerid.MurderPlayer(playerid, MurderResultFlags.Succeeded);
        GameHistory.overrideDeathReasonAndKiller(playerid, DeadPlayer.CustomDeathReason.Kill);
    }
    public static void serialKillerSuicideMeeting(byte PlayerId)
    {
        PlayerControl playerid = Helpers.playerById(PlayerId);
        if (playerid == null) return;
        playerid.MurderPlayer(playerid, MurderResultFlags.Succeeded);
        GameHistory.overrideDeathReasonAndKiller(playerid, DeadPlayer.CustomDeathReason.Guess);
    }
    public static void placeGarlic(byte[] buff)
    {
        var position = Vector3.zero;
        position.x = BitConverter.ToSingle(buff, 0 * sizeof(float));
        position.y = BitConverter.ToSingle(buff, 1 * sizeof(float));
        new Garlic(position);
    }

    public static void trackerUsedTracker(byte targetId)
    {
        Tracker.usedTracker = true;
        foreach (var player in PlayerControl.AllPlayerControls)
            if (player.PlayerId == targetId)
                Tracker.tracked = player;
    }
    public static void prophetExamine(byte targetId)
    {
        var target = Helpers.playerById(targetId);
        if (target == null) return;
        if (Prophet.examined.ContainsKey(target)) Prophet.examined.Remove(target);
        Prophet.examined.Add(target, Prophet.isKiller(target));
        Prophet.examinesLeft--;
        if ((Prophet.examineNum - Prophet.examinesLeft >= Prophet.examinesToBeRevealed) && Prophet.revealProphet) Prophet.isRevealed = true;
    }

    public static void reloadCooldowns()
    {
        PeaceDove.reloadMaxNum--;
        Cleaner.cleaner.SetKillTimer(114514f);
        /*
        foreach(PlayerControl p in PlayerControl.AllPlayerControls)
        {
            if (p.Data.Role.IsImpostor)
            {
                if (Mini.mini.Data.Role.IsImpostor && !Mini.isGrownUp()) Mini.mini.SetKillTimer(GameOptionsManager.Instance.currentNormalGameOptions.KillCooldown * 2 + PeaceDove.reloadCooldown);
                p.SetKillTimer(GameOptionsManager.Instance.currentNormalGameOptions.KillCooldown + PeaceDove.reloadCooldown);
            }
            else if (Jackal.jackal.PlayerId == p.PlayerId || Sidekick.sidekick.PlayerId == p.PlayerId)
            {
                Jackal.cooldown += PeaceDove.reloadCooldown;
                Sidekick.cooldown += PeaceDove.reloadCooldown;
            }
             if(PeaceDove.reloadSkills == true)
            {
                float pc = PeaceDove.reloadCooldown;
                Janitor.cooldown  += pc;
                Morphling.cooldown += pc;
                Camouflager.cooldown += pc;
                Vampire.cooldown += pc;
                Jackal.createSidekickCooldown += pc;
                Sidekick.cooldown += pc;
                Eraser.cooldown += pc;
                Trickster.placeBoxCooldown += pc;
                Trickster.lightsOutCooldown += pc;
                Cleaner.cooldown += pc;
                Warlock.cooldown += pc;
                Bomber.bombCooldown += pc;
                Yoyo.markCooldown += pc;
                Yoyo.adminCooldown += pc;
                Fraudster.cooldown += pc;
                Devil.blindCooldown += pc;
            }
            if (Helpers.shouldShowGhostInfo() &&(p.Data.Role.IsImpostor || p.PlayerId == Jackal.jackal.PlayerId || p.PlayerId == Sidekick.sidekick.PlayerId))
            {
                Helpers.showFlash(PeaceDove.color, 0.5f, "peacedoveReloadText".Translate());
            }
        }*/
    }
    public static void deputyUsedHandcuffs(byte targetId)
    {
        Deputy.remainingHandcuffs--;
        Deputy.handcuffedPlayers.Add(targetId);
    }

    public static void deputyPromotes()
    {
        if (Deputy.deputy != null)
        {
            // Deputy should never be null here, but there appeared to be a race condition during testing, which was removed.
            Sheriff.replaceCurrentSheriff(Deputy.deputy);
            Sheriff.formerDeputy = Deputy.deputy;
            Deputy.deputy = null;
            // No clear and reload, as we need to keep the number of handcuffs left etc
        }
    }

    public static void jackalCreatesSidekick(byte targetId)
    {
        var player = Helpers.playerById(targetId);
        if (player == null) return;
        if (Lawyer.target == player && Lawyer.isProsecutor && Lawyer.lawyer != null && !Lawyer.lawyer.Data.IsDead)
            Lawyer.isProsecutor = false;

        if (!Jackal.canCreateSidekickFromImpostor && player.Data.Role.IsImpostor)
        {
            Jackal.fakeSidekick = player;
        }
        else
        {
            var wasSpy = Spy.spy != null && player == Spy.spy;
            var wasImpostor = player.Data.Role.IsImpostor; // This can only be reached if impostors can be sidekicked.
            FastDestroyableSingleton<RoleManager>.Instance.SetRole(player, RoleTypes.Crewmate);
            if (player == Lawyer.lawyer && Lawyer.target != null)
            {
                var playerInfoTransform = Lawyer.target.cosmetics.nameText.transform.parent.FindChild("Info");
                var playerInfo = playerInfoTransform != null ? playerInfoTransform.GetComponent<TextMeshPro>() : null;
                if (playerInfo != null) playerInfo.text = "";
            }

            erasePlayerRoles(player.PlayerId);
            Sidekick.sidekick = player;
            if (player.PlayerId == PlayerControl.LocalPlayer.PlayerId) PlayerControl.LocalPlayer.moveable = true;
            if (wasSpy || wasImpostor) Sidekick.wasTeamRed = true;
            Sidekick.wasSpy = wasSpy;
            Sidekick.wasImpostor = wasImpostor;
            if (player == PlayerControl.LocalPlayer) SoundEffectsManager.play("jackalSidekick");
            if (HandleGuesser.isGuesserGm && CustomOptionHolder.guesserGamemodeSidekickIsAlwaysGuesser.getBool() &&
                !HandleGuesser.isGuesser(targetId))
                setGuesserGm(targetId);
        }

        Jackal.canCreateSidekick = false;
    }

    public static void sidekickPromotes()
    {
        Jackal.removeCurrentJackal();
        Jackal.jackal = Sidekick.sidekick;
        Jackal.canCreateSidekick = Jackal.jackalPromotedFromSidekickCanCreateSidekick;
        Jackal.wasTeamRed = Sidekick.wasTeamRed;
        Jackal.wasSpy = Sidekick.wasSpy;
        Jackal.wasImpostor = Sidekick.wasImpostor;
        Sidekick.clearAndReload();
    }

    public static void erasePlayerRoles(byte playerId, bool ignoreModifier = true)
    {
        var player = Helpers.playerById(playerId);
        if (player == null || !player.canBeErased()) return;

        // Crewmate roles
        if (player == Mayor.mayor) Mayor.clearAndReload();
        if (player == Portalmaker.portalmaker) Portalmaker.clearAndReload();
        if (player == Engineer.engineer) Engineer.clearAndReload();
        if (player == Sheriff.sheriff) Sheriff.clearAndReload();
        if (player == Deputy.deputy) Deputy.clearAndReload();
        if (player == Lighter.lighter) Lighter.clearAndReload();
        if (player == Detective.detective) Detective.clearAndReload();
        if (player == TimeMaster.timeMaster) TimeMaster.clearAndReload();
        if (player == Medic.medic) Medic.clearAndReload();
        if (player == Shifter.shifter) Shifter.clearAndReload();
        if (player == Seer.seer) Seer.clearAndReload();
        if (player == Hacker.hacker) Hacker.clearAndReload();
        if (player == Tracker.tracker) Tracker.clearAndReload();
        if (player == Snitch.snitch) Snitch.clearAndReload();
        if (player == Swapper.swapper) Swapper.clearAndReload();
        if (player == Spy.spy) Spy.clearAndReload();
        if (player == SecurityGuard.securityGuard) SecurityGuard.clearAndReload();
        if (player == Medium.medium) Medium.clearAndReload();
        if (player == Trapper.trapper) Trapper.clearAndReload();
        if(player == Prophet.prophet) Prophet.clearAndReload();
        if (player == PeaceDove.peacedove) PeaceDove.clearAndReload();

        // Impostor roles
        if (player == Morphling.morphling) Morphling.clearAndReload();
        if (player == Camouflager.camouflager) Camouflager.clearAndReload();
        if (player == Godfather.godfather) Godfather.clearAndReload();
        if (player == Mafioso.mafioso) Mafioso.clearAndReload();
        if (player == Janitor.janitor) Janitor.clearAndReload();
        if (player == Vampire.vampire) Vampire.clearAndReload();
        if (player == Eraser.eraser) Eraser.clearAndReload();
        if (player == Trickster.trickster) Trickster.clearAndReload();
        if (player == Cleaner.cleaner) Cleaner.clearAndReload();
        if (player == Warlock.warlock) Warlock.clearAndReload();
        if (player == Witch.witch) Witch.clearAndReload();
        if (player == Ninja.ninja) Ninja.clearAndReload();
        if (player == Bomber.bomber) Bomber.clearAndReload();
        if (player == Yoyo.yoyo) Yoyo.clearAndReload();
        if (player == Fraudster.fraudster) Fraudster.clearAndReload();
        if (player == Devil.devil) Devil.clearAndReload();

        // Other roles
        if (player == Jester.jester) Jester.clearAndReload();
        if (player == Arsonist.arsonist) Arsonist.clearAndReload();
        if (Guesser.isGuesser(player.PlayerId)) Guesser.clear(player.PlayerId);
        if (player == Jackal.jackal)
        {
            // Promote Sidekick and hence override the the Jackal or erase Jackal
            if (Sidekick.promotesToJackal && Sidekick.sidekick != null && !Sidekick.sidekick.Data.IsDead)
                sidekickPromotes();
            else
                Jackal.clearAndReload();
        }

        if (player == Sidekick.sidekick) Sidekick.clearAndReload();
        if (player == BountyHunter.bountyHunter) BountyHunter.clearAndReload();
        if (player == Vulture.vulture) Vulture.clearAndReload();
        if (player == Lawyer.lawyer) Lawyer.clearAndReload();
        if (player == Pursuer.pursuer) Pursuer.clearAndReload();
        if (player == Thief.thief) Thief.clearAndReload();

        // Modifier
        if (!ignoreModifier)
        {
            if (player == Lovers.lover1 || player == Lovers.lover2)
                Lovers.clearAndReload(); // The whole Lover couple is being erased
            if (Bait.bait.Any(x => x.PlayerId == player.PlayerId))
                Bait.bait.RemoveAll(x => x.PlayerId == player.PlayerId);
            if (Bloody.bloody.Any(x => x.PlayerId == player.PlayerId))
                Bloody.bloody.RemoveAll(x => x.PlayerId == player.PlayerId);
            if (AntiTeleport.antiTeleport.Any(x => x.PlayerId == player.PlayerId))
                AntiTeleport.antiTeleport.RemoveAll(x => x.PlayerId == player.PlayerId);
            if (Sunglasses.sunglasses.Any(x => x.PlayerId == player.PlayerId))
                Sunglasses.sunglasses.RemoveAll(x => x.PlayerId == player.PlayerId);
            if (player == Tiebreaker.tiebreaker) Tiebreaker.clearAndReload();
            if (player == Mini.mini) Mini.clearAndReload();
            if (Vip.vip.Any(x => x.PlayerId == player.PlayerId)) Vip.vip.RemoveAll(x => x.PlayerId == player.PlayerId);
            if (Invert.invert.Any(x => x.PlayerId == player.PlayerId))
                Invert.invert.RemoveAll(x => x.PlayerId == player.PlayerId);
            if (Chameleon.chameleon.Any(x => x.PlayerId == player.PlayerId))
                Chameleon.chameleon.RemoveAll(x => x.PlayerId == player.PlayerId);
            if (player == Armored.armored) Armored.clearAndReload();
        }
    }
    

    public static void setFutureErased(byte playerId)
    {
        var player = Helpers.playerById(playerId);
        if (Eraser.futureErased == null)
            Eraser.futureErased = new System.Collections.Generic.List<PlayerControl>();
        if (player != null) Eraser.futureErased.Add(player);
    }

    public static void setFutureShifted(byte playerId)
    {
        Shifter.futureShift = Helpers.playerById(playerId);
    }

    public static void setFutureShielded(byte playerId)
    {
        Medic.futureShielded = Helpers.playerById(playerId);
        Medic.usedShield = true;
    }

    public static void setFutureSpelled(byte playerId)
    {
        var player = Helpers.playerById(playerId);
        if (Witch.futureSpelled == null)
            Witch.futureSpelled = new System.Collections.Generic.List<PlayerControl>();
        if (player != null) Witch.futureSpelled.Add(player);
    }
    public static void setFutureBlinded(byte playerId)
    {
        var player = Helpers.playerById(playerId);

        if (Devil.futureBlinded == null)
            Devil.futureBlinded = new System.Collections.Generic.List<PlayerControl>();
        if(Devil.visionOfPlayersShouldBeChanged == null)
            Devil.visionOfPlayersShouldBeChanged = new System.Collections.Generic.List<PlayerControl>();

        if (player != null) 
        {
            Devil.futureBlinded.Add(player);
            Devil.visionOfPlayersShouldBeChanged.Add(player);
        }
    }
    public static void placeNinjaTrace(byte[] buff)
    {
        var position = Vector3.zero;
        position.x = BitConverter.ToSingle(buff, 0 * sizeof(float));
        position.y = BitConverter.ToSingle(buff, 1 * sizeof(float));
        new NinjaTrace(position, Ninja.traceTime);
        if (PlayerControl.LocalPlayer != Ninja.ninja)
            Ninja.ninjaMarked = null;
    }

    public static void setInvisible(byte playerId, byte flag)
    {
        var target = Helpers.playerById(playerId);
        if (target == null) return;
        if (flag == byte.MaxValue)
        {
            target.cosmetics.currentBodySprite.BodySprite.color = Color.white;
            target.cosmetics.colorBlindText.gameObject.SetActive(DataManager.Settings.Accessibility.ColorBlindMode);
            target.cosmetics.colorBlindText.color = target.cosmetics.colorBlindText.color.SetAlpha(1f);

            if (Camouflager.camouflageTimer <= 0 && !Helpers.MushroomSabotageActive()) target.setDefaultLook();
            Ninja.isInvisble = false;
            return;
        }

        target.setLook("", 6, "", "", "", "");
        var color = Color.clear;
        var canSee = PlayerControl.LocalPlayer.Data.Role.IsImpostor || PlayerControl.LocalPlayer.Data.IsDead;
        if (canSee) color.a = 0.1f;
        target.cosmetics.currentBodySprite.BodySprite.color = color;
        target.cosmetics.colorBlindText.gameObject.SetActive(false);
        target.cosmetics.colorBlindText.color = target.cosmetics.colorBlindText.color.SetAlpha(canSee ? 0.1f : 0f);
        Ninja.invisibleTimer = Ninja.invisibleDuration;
        Ninja.isInvisble = true;
    }

    public static void placePortal(byte[] buff)
    {
        Vector3 position = Vector2.zero;
        position.x = BitConverter.ToSingle(buff, 0 * sizeof(float));
        position.y = BitConverter.ToSingle(buff, 1 * sizeof(float));
        new Portal(position);
    }

    public static void usePortal(byte playerId, byte exit)
    {
        Portal.startTeleport(playerId, exit);
    }

    public static void placeJackInTheBox(byte[] buff)
    {
        var position = Vector3.zero;
        position.x = BitConverter.ToSingle(buff, 0 * sizeof(float));
        position.y = BitConverter.ToSingle(buff, 1 * sizeof(float));
        new JackInTheBox(position);
    }

    public static void lightsOut()
    {
        Trickster.lightsOutTimer = Trickster.lightsOutDuration;
        // If the local player is impostor indicate lights out
        if (Helpers.hasImpVision(GameData.Instance.GetPlayerById(PlayerControl.LocalPlayer.PlayerId)))
            new CustomMessage("lightsOutText".Translate(), Trickster.lightsOutDuration);
    }

    public static void placeCamera(byte[] buff)
    {
        var referenceCamera = Object.FindObjectOfType<SurvCamera>();
        if (referenceCamera == null) return; // Mira HQ

        SecurityGuard.remainingScrews -= SecurityGuard.camPrice;
        SecurityGuard.placedCameras++;

        var position = Vector3.zero;
        position.x = BitConverter.ToSingle(buff, 0 * sizeof(float));
        position.y = BitConverter.ToSingle(buff, 1 * sizeof(float));

        var camera = Object.Instantiate(referenceCamera);
        camera.transform.position = new Vector3(position.x, position.y, referenceCamera.transform.position.z - 1f);
        camera.CamName = $"Security Camera {SecurityGuard.placedCameras}";
        camera.Offset = new Vector3(0f, 0f, camera.Offset.z);
        if (GameOptionsManager.Instance.currentNormalGameOptions.MapId == 2 ||
            GameOptionsManager.Instance.currentNormalGameOptions.MapId == 4)
            camera.transform.localRotation = new Quaternion(0, 0, 1, 1); // Polus and Airship 

        if (SubmergedCompatibility.IsSubmerged)
        {
            // remove 2d box collider of console, so that no barrier can be created. (irrelevant for now, but who knows... maybe we need it later)
            var fixConsole = camera.transform.FindChild("FixConsole");
            if (fixConsole != null)
            {
                var boxCollider = fixConsole.GetComponent<BoxCollider2D>();
                if (boxCollider != null) Object.Destroy(boxCollider);
            }
        }


        if (PlayerControl.LocalPlayer == SecurityGuard.securityGuard)
        {
            camera.gameObject.SetActive(true);
            camera.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
        }
        else
        {
            camera.gameObject.SetActive(false);
        }

        camerasToAdd.Add(camera);
    }

    public static void sealVent(int ventId)
    {
        var vent = MapUtilities.CachedShipStatus.AllVents.FirstOrDefault(x => x != null && x.Id == ventId);
        if (vent == null) return;

        SecurityGuard.remainingScrews -= SecurityGuard.ventPrice;
        if (PlayerControl.LocalPlayer == SecurityGuard.securityGuard)
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
            rend.color = new Color(1f, 1f, 1f, 0.5f);
            vent.name = "FutureSealedVent_" + vent.name;
        }

        ventsToSeal.Add(vent);
    }

    public static void arsonistWin()
    {
        Arsonist.triggerArsonistWin = true;
        foreach (var p in PlayerControl.AllPlayerControls)
            if (p != Arsonist.arsonist && !p.Data.IsDead)
            {
                p.Exiled();
                overrideDeathReasonAndKiller(p, DeadPlayer.CustomDeathReason.Arson, Arsonist.arsonist);
            }
    }

    public static void lawyerSetTarget(byte playerId)
    {
        Lawyer.target = Helpers.playerById(playerId);
    }

    public static void lawyerPromotesToPursuer()
    {
        var player = Lawyer.lawyer;
        var client = Lawyer.target;
        Lawyer.clearAndReload(false);

        Pursuer.pursuer = player;

        if (player.PlayerId == PlayerControl.LocalPlayer.PlayerId && client != null)
        {
            var playerInfoTransform = client.cosmetics.nameText.transform.parent.FindChild("Info");
            var playerInfo = playerInfoTransform != null ? playerInfoTransform.GetComponent<TextMeshPro>() : null;
            if (playerInfo != null) playerInfo.text = "";
        }
    }

    public static void guesserShoot(byte killerId, byte dyingTargetId, byte guessedTargetId, byte guessedRoleId)
    {
        var dyingTarget = Helpers.playerById(dyingTargetId);
        if (dyingTarget == null) return;
        if (Lawyer.target != null && dyingTarget == Lawyer.target)
            Lawyer.targetWasGuessed = true; // Lawyer shouldn't be exiled with the client for guesses
        var dyingLoverPartner = Lovers.bothDie ? dyingTarget.getPartner() : null; // Lover check
        if (Lawyer.target != null && dyingLoverPartner == Lawyer.target)
            Lawyer.targetWasGuessed = true; // Lawyer shouldn't be exiled with the client for guesses

        var guesser = Helpers.playerById(killerId);
        if (Thief.thief != null && Thief.thief.PlayerId == killerId && Thief.canStealWithGuess)
        {
            var roleInfo = RoleInfo.allRoleInfos.FirstOrDefault(x => (byte)x.roleId == guessedRoleId);
            if (!Thief.thief.Data.IsDead && !Thief.isFailedThiefKill(dyingTarget, guesser, roleInfo))
                thiefStealsRole(dyingTarget.PlayerId);
        }

        var lawyerDiedAdditionally = false;
        if (Lawyer.lawyer != null && !Lawyer.isProsecutor && Lawyer.lawyer.PlayerId == killerId &&
            Lawyer.target != null && Lawyer.target.PlayerId == dyingTargetId)
        {
            // Lawyer guessed client.
            if (PlayerControl.LocalPlayer == Lawyer.lawyer)
            {
                FastDestroyableSingleton<HudManager>.Instance.KillOverlay.ShowKillAnimation(Lawyer.lawyer.Data,
                    Lawyer.lawyer.Data);
                if (MeetingHudPatch.guesserUI != null) MeetingHudPatch.guesserUIExitButton.OnClick.Invoke();
            }

            Lawyer.lawyer.Exiled();
            lawyerDiedAdditionally = true;
            overrideDeathReasonAndKiller(Lawyer.lawyer, DeadPlayer.CustomDeathReason.LawyerSuicide, guesser);
        }

        dyingTarget.Exiled();
        overrideDeathReasonAndKiller(dyingTarget, DeadPlayer.CustomDeathReason.Guess, guesser);
        var partnerId = dyingLoverPartner != null ? dyingLoverPartner.PlayerId : dyingTargetId;

        HandleGuesser.remainingShots(killerId, true);
        if (Constants.ShouldPlaySfx()) SoundManager.Instance.PlaySound(dyingTarget.KillSfx, false, 0.8f);
        if (MeetingHud.Instance)
        {
            foreach (var pva in MeetingHud.Instance.playerStates)
            {
                if (pva.TargetPlayerId == dyingTargetId || pva.TargetPlayerId == partnerId ||
                    (lawyerDiedAdditionally && Lawyer.lawyer.PlayerId == pva.TargetPlayerId))
                {
                    pva.SetDead(pva.DidReport, true);
                    pva.Overlay.gameObject.SetActive(true);
                    MeetingHudPatch.swapperCheckAndReturnSwap(MeetingHud.Instance, pva.TargetPlayerId);
                }

                //Give players back their vote if target is shot dead
                if (pva.VotedFor != dyingTargetId && pva.VotedFor != partnerId &&
                    (!lawyerDiedAdditionally || Lawyer.lawyer.PlayerId != pva.VotedFor)) continue;
                pva.UnsetVote();
                var voteAreaPlayer = Helpers.playerById(pva.TargetPlayerId);
                if (!voteAreaPlayer.AmOwner) continue;
                MeetingHud.Instance.ClearVote();
            }

            if (AmongUsClient.Instance.AmHost)
                MeetingHud.Instance.CheckForEndVoting();
        }

        if (FastDestroyableSingleton<HudManager>.Instance != null && guesser != null)
            if (PlayerControl.LocalPlayer == dyingTarget)
            {
                FastDestroyableSingleton<HudManager>.Instance.KillOverlay.ShowKillAnimation(guesser.Data,
                    dyingTarget.Data);
                if (MeetingHudPatch.guesserUI != null) MeetingHudPatch.guesserUIExitButton.OnClick.Invoke();
            }
            else if (dyingLoverPartner != null && PlayerControl.LocalPlayer == dyingLoverPartner)
            {
                FastDestroyableSingleton<HudManager>.Instance.KillOverlay.ShowKillAnimation(dyingLoverPartner.Data,
                    dyingLoverPartner.Data);
                if (MeetingHudPatch.guesserUI != null) MeetingHudPatch.guesserUIExitButton.OnClick.Invoke();
            }

        // remove shoot button from targets for all guessers and close their guesserUI
        if (GuesserGM.isGuesser(PlayerControl.LocalPlayer.PlayerId) && PlayerControl.LocalPlayer != guesser &&
            !PlayerControl.LocalPlayer.Data.IsDead &&
            GuesserGM.remainingShots(PlayerControl.LocalPlayer.PlayerId) > 0 && MeetingHud.Instance)
        {
            MeetingHud.Instance.playerStates.ToList().ForEach(x =>
            {
                if (x.TargetPlayerId == dyingTarget.PlayerId && x.transform.FindChild("ShootButton") != null)
                    Object.Destroy(x.transform.FindChild("ShootButton").gameObject);
            });
            if (dyingLoverPartner != null)
                MeetingHud.Instance.playerStates.ToList().ForEach(x =>
                {
                    if (x.TargetPlayerId == dyingLoverPartner.PlayerId && x.transform.FindChild("ShootButton") != null)
                        Object.Destroy(x.transform.FindChild("ShootButton").gameObject);
                });

            if (MeetingHudPatch.guesserUI != null && MeetingHudPatch.guesserUIExitButton != null)
            {
                if (MeetingHudPatch.guesserCurrentTarget == dyingTarget.PlayerId)
                    MeetingHudPatch.guesserUIExitButton.OnClick.Invoke();
                else if (dyingLoverPartner != null &&
                         MeetingHudPatch.guesserCurrentTarget == dyingLoverPartner.PlayerId)
                    MeetingHudPatch.guesserUIExitButton.OnClick.Invoke();
            }
        }


        var guessedTarget = Helpers.playerById(guessedTargetId);
        if (PlayerControl.LocalPlayer.Data.IsDead && guessedTarget != null && guesser != null)
        {
            var roleInfo = RoleInfo.allRoleInfos.FirstOrDefault(x => (byte)x.roleId == guessedRoleId);
            var msg =
                string.Format("guessedChatText".Translate(), guesser.Data.PlayerName, roleInfo?.name ?? "", guessedTarget.Data.PlayerName);
            if (AmongUsClient.Instance.AmClient && FastDestroyableSingleton<HudManager>.Instance)
                FastDestroyableSingleton<HudManager>.Instance.Chat.AddChat(guesser, msg);
            if (msg.IndexOf("who", StringComparison.OrdinalIgnoreCase) >= 0)
                FastDestroyableSingleton<UnityTelemetry>.Instance.SendWho();
        }
    }

    public static void setBlanked(byte playerId, byte value)
    {
        var target = Helpers.playerById(playerId);
        if (target == null) return;
        Pursuer.blankedList.RemoveAll(x => x.PlayerId == playerId);
        if (value > 0) Pursuer.blankedList.Add(target);
    }

    public static void bloody(byte killerPlayerId, byte bloodyPlayerId)
    {
        if (Bloody.active.ContainsKey(killerPlayerId)) return;
        Bloody.active.Add(killerPlayerId, Bloody.duration);
        Bloody.bloodyKillerMap.Add(killerPlayerId, bloodyPlayerId);
    }

    public static void setFirstKill(byte playerId)
    {
        var target = Helpers.playerById(playerId);
        if (target == null) return;
        firstKillPlayer = target;
    }

    public static void setTiebreak()
    {
        Tiebreaker.isTiebreak = true;
    }

    public static void thiefStealsRole(byte playerId)
    {
        var target = Helpers.playerById(playerId);
        var thief = Thief.thief;
        if (target == null) return;
        if (target == Sheriff.sheriff) Sheriff.sheriff = thief;
        if (target == Jackal.jackal)
        {
            Jackal.jackal = thief;
            Jackal.formerJackals.Add(target);
        }

        if (target == Sidekick.sidekick)
        {
            Sidekick.sidekick = thief;
            Jackal.formerJackals.Add(target);
            if (HandleGuesser.isGuesserGm && CustomOptionHolder.guesserGamemodeSidekickIsAlwaysGuesser.getBool() &&
                !HandleGuesser.isGuesser(thief.PlayerId))
                setGuesserGm(thief.PlayerId);
        }

        if (target == Guesser.evilGuesser) Guesser.evilGuesser = thief;
        if (target == Godfather.godfather) Godfather.godfather = thief;
        if (target == Mafioso.mafioso) Mafioso.mafioso = thief;
        if (target == Janitor.janitor) Janitor.janitor = thief;
        if (target == Morphling.morphling) Morphling.morphling = thief;
        if (target == Camouflager.camouflager) Camouflager.camouflager = thief;
        if (target == Vampire.vampire) Vampire.vampire = thief;
        if (target == Eraser.eraser) Eraser.eraser = thief;
        if (target == Trickster.trickster) Trickster.trickster = thief;
        if (target == Cleaner.cleaner) Cleaner.cleaner = thief;
        if (target == Warlock.warlock) Warlock.warlock = thief;
        if (target == BountyHunter.bountyHunter) BountyHunter.bountyHunter = thief;
        if (target == Witch.witch)
        {
            Witch.witch = thief;
            if (MeetingHud.Instance)
                if (Witch.witchVoteSavesTargets) // In a meeting, if the thief guesses the witch, all targets are saved or no target is saved.
                    Witch.futureSpelled = new System.Collections.Generic.List<PlayerControl>();
                else // If thief kills witch during the round, remove the thief from the list of spelled people, keep the rest
                    Witch.futureSpelled.RemoveAll(x => x.PlayerId == thief.PlayerId);
        }

        if (target == Ninja.ninja) Ninja.ninja = thief;
        if (target == Bomber.bomber) Bomber.bomber = thief;
        if (target == Yoyo.yoyo)
        {
            Yoyo.yoyo = thief;
            Yoyo.markedLocation = null;
        }
        if (target == Fraudster.fraudster) Fraudster.fraudster = thief;
        if (target == Devil.devil) Devil.devil = thief;

        if (target.Data.Role.IsImpostor)
        {
            RoleManager.Instance.SetRole(Thief.thief, RoleTypes.Impostor);
            FastDestroyableSingleton<HudManager>.Instance.KillButton.SetCoolDown(Thief.thief.killTimer,
                GameOptionsManager.Instance.currentNormalGameOptions.KillCooldown);
        }

        if (Lawyer.lawyer != null && target == Lawyer.target)
            Lawyer.target = thief;
        if (Thief.thief == PlayerControl.LocalPlayer) CustomButton.ResetAllCooldowns();
        Thief.clearAndReload();
        Thief.formerThief = thief; // After clearAndReload, else it would get reset...
    }

    public static void setTrap(byte[] buff)
    {
        if (Trapper.trapper == null) return;
        Trapper.charges -= 1;
        var position = Vector3.zero;
        position.x = BitConverter.ToSingle(buff, 0 * sizeof(float));
        position.y = BitConverter.ToSingle(buff, 1 * sizeof(float));
        new Trap(position);
    }

    public static void triggerTrap(byte playerId, byte trapId)
    {
        Trap.triggerTrap(playerId, trapId);
    }

    public static void setGuesserGm(byte playerId)
    {
        var target = Helpers.playerById(playerId);
        if (target == null) return;
        new GuesserGM(target);
    }

    public static void shareTimer(float punish)
    {
        HideNSeek.timer -= punish;
    }

    public static void huntedShield(byte playerId)
    {
        if (!Hunted.timeshieldActive.Contains(playerId)) Hunted.timeshieldActive.Add(playerId);
        FastDestroyableSingleton<HudManager>.Instance.StartCoroutine(Effects.Lerp(Hunted.shieldDuration,
            new Action<float>(p =>
            {
                if (p == 1f) Hunted.timeshieldActive.Remove(playerId);
            })));
    }

    public static void huntedRewindTime(byte playerId)
    {
        Hunted.timeshieldActive.Remove(playerId); // Shield is no longer active when rewinding
        SoundEffectsManager.stop("timemasterShield"); // Shield sound stopped when rewinding
        if (playerId == PlayerControl.LocalPlayer.PlayerId) resetHuntedRewindButton();
        FastDestroyableSingleton<HudManager>.Instance.FullScreen.color = new Color(0f, 0.5f, 0.8f, 0.3f);
        FastDestroyableSingleton<HudManager>.Instance.FullScreen.enabled = true;
        FastDestroyableSingleton<HudManager>.Instance.FullScreen.gameObject.SetActive(true);
        FastDestroyableSingleton<HudManager>.Instance.StartCoroutine(Effects.Lerp(Hunted.shieldRewindTime,
            new Action<float>(p =>
            {
                if (p == 1f) FastDestroyableSingleton<HudManager>.Instance.FullScreen.enabled = false;
            })));

        if (!PlayerControl.LocalPlayer.Data.Role.IsImpostor) return; // only rewind hunter

        TimeMaster.isRewinding = true;

        if (MapBehaviour.Instance)
            MapBehaviour.Instance.Close();
        if (Minigame.Instance)
            Minigame.Instance.ForceClose();
        PlayerControl.LocalPlayer.moveable = false;
    }

    public static void propHuntStartTimer(bool blackout = false)
    {
        if (blackout)
        {
            PropHunt.blackOutTimer = PropHunt.initialBlackoutTime;
            PropHunt.transformLayers();
        }
        else
        {
            PropHunt.timerRunning = true;
            PropHunt.blackOutTimer = 0f;
        }

        PropHunt.startTime = DateTime.UtcNow;
        foreach (var pc in PlayerControl.AllPlayerControls.ToArray().Where(x => x.Data.Role.IsImpostor))
            pc.MyPhysics.SetBodyType(PlayerBodyTypes.Seeker);
    }

    public static void propHuntSetProp(byte playerId, string propName, float posX)
    {
        var player = Helpers.playerById(playerId);
        var prop = PropHunt.FindPropByNameAndPos(propName, posX);
        if (prop == null) return;
        try
        {
            player.GetComponent<SpriteRenderer>().sprite = prop.GetComponent<SpriteRenderer>().sprite;
        }
        catch
        {
            player.GetComponent<SpriteRenderer>().sprite =
                prop.transform.GetComponentInChildren<SpriteRenderer>().sprite;
        }

        player.transform.localScale = prop.transform.lossyScale;
        player.Visible = false;
        PropHunt.currentObject[player.PlayerId] = new Tuple<string, float>(propName, posX);
    }

    public static void propHuntSetRevealed(byte playerId)
    {
        SoundEffectsManager.play("morphlingMorph");
        PropHunt.isCurrentlyRevealed.Add(playerId, PropHunt.revealDuration);
        PropHunt.timer -= PropHunt.revealPunish;
    }

    public static void propHuntSetInvis(byte playerId)
    {
        PropHunt.invisPlayers.Add(playerId, PropHunt.invisDuration);
    }

    public static void propHuntSetSpeedboost(byte playerId)
    {
        PropHunt.speedboostActive.Add(playerId, PropHunt.speedboostDuration);
    }

    public static void receiveGhostInfo(byte senderId, MessageReader reader)
    {
        var sender = Helpers.playerById(senderId);

        var infoType = (GhostInfoTypes)reader.ReadByte();
        switch (infoType)
        {
            case GhostInfoTypes.HandcuffNoticed:
                Deputy.setHandcuffedKnows(true, senderId);
                break;
            case GhostInfoTypes.HandcuffOver:
                _ = Deputy.handcuffedKnows.Remove(senderId);
                break;
            case GhostInfoTypes.ArsonistDouse:
                Arsonist.dousedPlayers.Add(Helpers.playerById(reader.ReadByte()));
                break;
            case GhostInfoTypes.BountyTarget:
                BountyHunter.bounty = Helpers.playerById(reader.ReadByte());
                break;
            case GhostInfoTypes.NinjaMarked:
                Ninja.ninjaMarked = Helpers.playerById(reader.ReadByte());
                break;
            case GhostInfoTypes.WarlockTarget:
                Warlock.curseVictim = Helpers.playerById(reader.ReadByte());
                break;
            case GhostInfoTypes.MediumInfo:
                var mediumInfo = reader.ReadString();
                if (Helpers.shouldShowGhostInfo())
                    FastDestroyableSingleton<HudManager>.Instance.Chat.AddChat(sender, mediumInfo);
                break;
            case GhostInfoTypes.DetectiveOrMedicInfo:
                var detectiveInfo = reader.ReadString();
                if (Helpers.shouldShowGhostInfo())
                    FastDestroyableSingleton<HudManager>.Instance.Chat.AddChat(sender, detectiveInfo);
                break;
            case GhostInfoTypes.BlankUsed:
                Pursuer.blankedList.Remove(sender);
                break;
            case GhostInfoTypes.VampireTimer:
                vampireKillButton.Timer = reader.ReadByte();
                break;
            case GhostInfoTypes.DeathReasonAndKiller:
                overrideDeathReasonAndKiller(Helpers.playerById(reader.ReadByte()),
                    (DeadPlayer.CustomDeathReason)reader.ReadByte(), Helpers.playerById(reader.ReadByte()));
                break;
        }
    }

    public static void placeBomb(byte[] buff)
    {
        if (Bomber.bomber == null) return;
        var position = Vector3.zero;
        position.x = BitConverter.ToSingle(buff, 0 * sizeof(float));
        position.y = BitConverter.ToSingle(buff, 1 * sizeof(float));
        new Bomb(position);
    }

    public static void defuseBomb()
    {
        try
        {
            SoundEffectsManager.playAtPosition("bombDefused", Bomber.bomb.bomb.transform.position,
                range: Bomber.hearRange);
        }
        catch
        {
        }

        Bomber.clearBomb();
        bomberButton.Timer = bomberButton.MaxTimer;
        bomberButton.isEffectActive = false;
        bomberButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
    }

    public static void shareRoom(byte playerId, byte roomId)
    {
        if (Snitch.playerRoomMap.ContainsKey(playerId)) Snitch.playerRoomMap[playerId] = roomId;
        else Snitch.playerRoomMap.Add(playerId, roomId);
    }

    public static void yoyoMarkLocation(byte[] buff)
    {
        if (Yoyo.yoyo == null) return;
        var position = Vector3.zero;
        position.x = BitConverter.ToSingle(buff, 0 * sizeof(float));
        position.y = BitConverter.ToSingle(buff, 1 * sizeof(float));
        Yoyo.markLocation(position);
        new Silhouette(position, -1, false);
    }

    public static void yoyoBlink(bool isFirstJump, byte[] buff)
    {
        if (Yoyo.yoyo == null || Yoyo.markedLocation == null) return;
        var markedPos = (Vector3)Yoyo.markedLocation;
        Yoyo.yoyo.NetTransform.SnapTo(markedPos);

        var markedSilhouette = Silhouette.silhouettes.FirstOrDefault(s =>
            s.gameObject.transform.position.x == markedPos.x && s.gameObject.transform.position.y == markedPos.y);
        if (markedSilhouette != null)
            markedSilhouette.permanent = false;

        var position = Vector3.zero;
        position.x = BitConverter.ToSingle(buff, 0 * sizeof(float));
        position.y = BitConverter.ToSingle(buff, 1 * sizeof(float));
        // Create Silhoutte At Start Position:
        if (isFirstJump)
        {
            Yoyo.markLocation(position);
            new Silhouette(position, Yoyo.blinkDuration);
        }
        else
        {
            new Silhouette(position, 5);
            Yoyo.markedLocation = null;
        }

        if (Chameleon.chameleon.Any(x => x.PlayerId == Yoyo.yoyo.PlayerId)) // Make the Yoyo visible if chameleon!
            Chameleon.lastMoved[Yoyo.yoyo.PlayerId] = Time.time;
    }

    public static void breakArmor()
    {
        if (Armored.armored == null || Armored.isBrokenArmor) return;
        Armored.isBrokenArmor = true;
        if (PlayerControl.LocalPlayer.Data.IsDead) Armored.armored.ShowFailedMurder();
    }
}

[HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.HandleRpc))]
internal class RPCHandlerPatch
{
    private static void Postfix([HarmonyArgument(0)] byte callId, [HarmonyArgument(1)] MessageReader reader)
    {
        var packetId = callId;
        switch (packetId)
        {
            // Main Controls

            case (byte)CustomRPC.ResetVaribles:
                RPCProcedure.resetVariables();
                break;
            case (byte)CustomRPC.ShareOptions:
                RPCProcedure.HandleShareOptions(reader.ReadByte(), reader);
                break;
            case (byte)CustomRPC.ForceEnd:
                RPCProcedure.forceEnd();
                break;
            case (byte)CustomRPC.WorkaroundSetRoles:
                RPCProcedure.workaroundSetRoles(reader.ReadByte(), reader);
                break;
            case (byte)CustomRPC.SetRole:
                var roleId = reader.ReadByte();
                var playerId = reader.ReadByte();
                RPCProcedure.setRole(roleId, playerId);
                break;
            case (byte)CustomRPC.SetModifier:
                var modifierId = reader.ReadByte();
                var pId = reader.ReadByte();
                var flag = reader.ReadByte();
                RPCProcedure.setModifier(modifierId, pId, flag);
                break;
            case (byte)CustomRPC.VersionHandshake:
                var major = reader.ReadByte();
                var minor = reader.ReadByte();
                var patch = reader.ReadByte();
                var timer = reader.ReadSingle();
                if (!AmongUsClient.Instance.AmHost && timer >= 0f) GameStartManagerPatch.timer = timer;
                var versionOwnerId = reader.ReadPackedInt32();
                byte revision = 0xFF;
                Guid guid;
                if (reader.Length - reader.Position >= 17)
                {
                    // enough bytes left to read
                    revision = reader.ReadByte();
                    // GUID
                    byte[] gbytes = reader.ReadBytes(16);
                    guid = new Guid(gbytes);
                }
                else
                {
                    guid = new Guid(new byte[16]);
                }

                RPCProcedure.versionHandshake(major, minor, patch, revision == 0xFF ? -1 : revision, guid,
                    versionOwnerId);
                break;
            case (byte)CustomRPC.UseUncheckedVent:
                var ventId = reader.ReadPackedInt32();
                var ventingPlayer = reader.ReadByte();
                var isEnter = reader.ReadByte();
                RPCProcedure.useUncheckedVent(ventId, ventingPlayer, isEnter);
                break;
            case (byte)CustomRPC.UncheckedMurderPlayer:
                var source = reader.ReadByte();
                var target = reader.ReadByte();
                var showAnimation = reader.ReadByte();
                RPCProcedure.uncheckedMurderPlayer(source, target, showAnimation);
                break;
            case (byte)CustomRPC.UncheckedExilePlayer:
                var exileTarget = reader.ReadByte();
                RPCProcedure.uncheckedExilePlayer(exileTarget);
                break;
            case (byte)CustomRPC.UncheckedCmdReportDeadBody:
                var reportSource = reader.ReadByte();
                var reportTarget = reader.ReadByte();
                RPCProcedure.uncheckedCmdReportDeadBody(reportSource, reportTarget);
                break;
            case (byte)CustomRPC.DynamicMapOption:
                var mapId = reader.ReadByte();
                RPCProcedure.dynamicMapOption(mapId);
                break;
            case (byte)CustomRPC.SetGameStarting:
                RPCProcedure.setGameStarting();
                break;

            // Role functionality

            case (byte)CustomRPC.EngineerFixLights:
                RPCProcedure.engineerFixLights();
                break;
            case (byte)CustomRPC.EngineerFixSubmergedOxygen:
                RPCProcedure.engineerFixSubmergedOxygen();
                break;
            case (byte)CustomRPC.EngineerUsedRepair:
                RPCProcedure.engineerUsedRepair();
                break;
            case (byte)CustomRPC.CleanBody:
                RPCProcedure.cleanBody(reader.ReadByte(), reader.ReadByte());
                break;
            case (byte)CustomRPC.TimeMasterRewindTime:
                RPCProcedure.timeMasterRewindTime();
                break;
            case (byte)CustomRPC.TimeMasterShield:
                RPCProcedure.timeMasterShield();
                break;
            case (byte)CustomRPC.MedicSetShielded:
                RPCProcedure.medicSetShielded(reader.ReadByte());
                break;
            case (byte)CustomRPC.ShieldedMurderAttempt:
                RPCProcedure.shieldedMurderAttempt();
                break;
            case (byte)CustomRPC.ShifterShift:
                RPCProcedure.shifterShift(reader.ReadByte());
                break;
            case (byte)CustomRPC.SwapperSwap:
                var playerId1 = reader.ReadByte();
                var playerId2 = reader.ReadByte();
                RPCProcedure.swapperSwap(playerId1, playerId2);
                break;
            case (byte)CustomRPC.MayorSetVoteTwice:
                Mayor.voteTwice = reader.ReadBoolean();
                break;
            case (byte)CustomRPC.MorphlingMorph:
                RPCProcedure.morphlingMorph(reader.ReadByte());
                break;
            case (byte)CustomRPC.CamouflagerCamouflage:
                RPCProcedure.camouflagerCamouflage();
                break;
            case (byte)CustomRPC.VampireSetBitten:
                var bittenId = reader.ReadByte();
                var reset = reader.ReadByte();
                RPCProcedure.vampireSetBitten(bittenId, reset);
                break;
            case (byte)CustomRPC.PlaceGarlic:
                RPCProcedure.placeGarlic(reader.ReadBytesAndSize());
                break;
            case (byte)CustomRPC.TrackerUsedTracker:
                RPCProcedure.trackerUsedTracker(reader.ReadByte());
                break;
            case (byte)CustomRPC.DeputyUsedHandcuffs:
                RPCProcedure.deputyUsedHandcuffs(reader.ReadByte());
                break;
            case (byte)CustomRPC.DeputyPromotes:
                RPCProcedure.deputyPromotes();
                break;
            case (byte)CustomRPC.JackalCreatesSidekick:
                RPCProcedure.jackalCreatesSidekick(reader.ReadByte());
                break;
            case (byte)CustomRPC.SidekickPromotes:
                RPCProcedure.sidekickPromotes();
                break;
            case (byte)CustomRPC.ErasePlayerRoles:
                var eraseTarget = reader.ReadByte();
                RPCProcedure.erasePlayerRoles(eraseTarget);
                Eraser.alreadyErased.Add(eraseTarget);
                break;
            case (byte)CustomRPC.SetFutureErased:
                RPCProcedure.setFutureErased(reader.ReadByte());
                break;
            case (byte)CustomRPC.SetFutureShifted:
                RPCProcedure.setFutureShifted(reader.ReadByte());
                break;
            case (byte)CustomRPC.SetFutureShielded:
                RPCProcedure.setFutureShielded(reader.ReadByte());
                break;
            case (byte)CustomRPC.PlaceNinjaTrace:
                RPCProcedure.placeNinjaTrace(reader.ReadBytesAndSize());
                break;
            case (byte)CustomRPC.PlacePortal:
                RPCProcedure.placePortal(reader.ReadBytesAndSize());
                break;
            case (byte)CustomRPC.UsePortal:
                RPCProcedure.usePortal(reader.ReadByte(), reader.ReadByte());
                break;
            case (byte)CustomRPC.PlaceJackInTheBox:
                RPCProcedure.placeJackInTheBox(reader.ReadBytesAndSize());
                break;
            case (byte)CustomRPC.LightsOut:
                RPCProcedure.lightsOut();
                break;
            case (byte)CustomRPC.PlaceCamera:
                RPCProcedure.placeCamera(reader.ReadBytesAndSize());
                break;
            case (byte)CustomRPC.SealVent:
                RPCProcedure.sealVent(reader.ReadPackedInt32());
                break;
            case (byte)CustomRPC.ArsonistWin:
                RPCProcedure.arsonistWin();
                break;
            case (byte)CustomRPC.GuesserShoot:
                var killerId = reader.ReadByte();
                var dyingTarget = reader.ReadByte();
                var guessedTarget = reader.ReadByte();
                var guessedRoleId = reader.ReadByte();
                RPCProcedure.guesserShoot(killerId, dyingTarget, guessedTarget, guessedRoleId);
                break;
            case (byte)CustomRPC.LawyerSetTarget:
                RPCProcedure.lawyerSetTarget(reader.ReadByte());
                break;
            case (byte)CustomRPC.LawyerPromotesToPursuer:
                RPCProcedure.lawyerPromotesToPursuer();
                break;
            case (byte)CustomRPC.SetBlanked:
                var pid = reader.ReadByte();
                var blankedValue = reader.ReadByte();
                RPCProcedure.setBlanked(pid, blankedValue);
                break;
            case (byte)CustomRPC.SetFutureSpelled:
                RPCProcedure.setFutureSpelled(reader.ReadByte());
                break;
            case (byte)CustomRPC.SetFutureBlinded:
                RPCProcedure.setFutureBlinded(reader.ReadByte());
                break;
            case (byte)CustomRPC.Bloody:
                var bloodyKiller = reader.ReadByte();
                var bloodyDead = reader.ReadByte();
                RPCProcedure.bloody(bloodyKiller, bloodyDead);
                break;
            case (byte)CustomRPC.SetFirstKill:
                var firstKill = reader.ReadByte();
                RPCProcedure.setFirstKill(firstKill);
                break;
            case (byte)CustomRPC.SetTiebreak:
                RPCProcedure.setTiebreak();
                break;
            case (byte)CustomRPC.SetInvisible:
                var invisiblePlayer = reader.ReadByte();
                var invisibleFlag = reader.ReadByte();
                RPCProcedure.setInvisible(invisiblePlayer, invisibleFlag);
                break;
            case (byte)CustomRPC.ThiefStealsRole:
                var thiefTargetId = reader.ReadByte();
                RPCProcedure.thiefStealsRole(thiefTargetId);
                break;
            case (byte)CustomRPC.SetTrap:
                RPCProcedure.setTrap(reader.ReadBytesAndSize());
                break;
            case (byte)CustomRPC.TriggerTrap:
                var trappedPlayer = reader.ReadByte();
                var trapId = reader.ReadByte();
                RPCProcedure.triggerTrap(trappedPlayer, trapId);
                break;
            case (byte)CustomRPC.PlaceBomb:
                RPCProcedure.placeBomb(reader.ReadBytesAndSize());
                break;
            case (byte)CustomRPC.DefuseBomb:
                RPCProcedure.defuseBomb();
                break;
            case (byte)CustomRPC.ShareGamemode:
                var gm = reader.ReadByte();
                RPCProcedure.shareGamemode(gm);
                break;
            case (byte)CustomRPC.StopStart:
                RPCProcedure.stopStart(reader.ReadByte());
                break;
            case (byte)CustomRPC.YoyoMarkLocation:
                RPCProcedure.yoyoMarkLocation(reader.ReadBytesAndSize());
                break;
            case (byte)CustomRPC.YoyoBlink:
                RPCProcedure.yoyoBlink(reader.ReadByte() == byte.MaxValue, reader.ReadBytesAndSize());
                break;
            case (byte)CustomRPC.BreakArmor:
                RPCProcedure.breakArmor();
                break;
            case (byte)CustomRPC.SuicideMeeting:
                RPCProcedure.serialKillerSuicideMeeting(reader.ReadByte());
                break;
            case (byte)CustomRPC.Suicide:
                RPCProcedure.serialKillerSuicide(reader.ReadByte());
                break;
            case (byte)CustomRPC.ProphetExamine:
                RPCProcedure.prophetExamine(reader.ReadByte());
                break;
            case (byte)CustomRPC.ReloadCooldowns:
                RPCProcedure.reloadCooldowns();
                break;
            // Game mode
            case (byte)CustomRPC.SetGuesserGm:
                var guesserGm = reader.ReadByte();
                RPCProcedure.setGuesserGm(guesserGm);
                break;
            case (byte)CustomRPC.ShareTimer:
                var punish = reader.ReadSingle();
                RPCProcedure.shareTimer(punish);
                break;
            case (byte)CustomRPC.HuntedShield:
                var huntedPlayer = reader.ReadByte();
                RPCProcedure.huntedShield(huntedPlayer);
                break;
            case (byte)CustomRPC.HuntedRewindTime:
                var rewindPlayer = reader.ReadByte();
                RPCProcedure.huntedRewindTime(rewindPlayer);
                break;
            case (byte)CustomRPC.PropHuntStartTimer:
                RPCProcedure.propHuntStartTimer(reader.ReadBoolean());
                break;
            case (byte)CustomRPC.SetProp:
                var targetPlayer = reader.ReadByte();
                var propName = reader.ReadString();
                var posX = reader.ReadSingle();
                RPCProcedure.propHuntSetProp(targetPlayer, propName, posX);
                break;
            case (byte)CustomRPC.SetRevealed:
                RPCProcedure.propHuntSetRevealed(reader.ReadByte());
                break;
            case (byte)CustomRPC.PropHuntSetInvis:
                RPCProcedure.propHuntSetInvis(reader.ReadByte());
                break;
            case (byte)CustomRPC.PropHuntSetSpeedboost:
                RPCProcedure.propHuntSetSpeedboost(reader.ReadByte());
                break;
            case (byte)CustomRPC.DraftModePickOrder:
                RoleDraft.receivePickOrder(reader.ReadByte(), reader);
                break;
            case (byte)CustomRPC.DraftModePick:
                RoleDraft.receivePick(reader.ReadByte(), reader.ReadByte());
                break;
            case (byte)CustomRPC.ShareGhostInfo:
                RPCProcedure.receiveGhostInfo(reader.ReadByte(), reader);
                break;


            case (byte)CustomRPC.ShareRoom:
                var roomPlayer = reader.ReadByte();
                var roomId = reader.ReadByte();
                RPCProcedure.shareRoom(roomPlayer, roomId);
                break;
            case (byte)CustomRPC.EventKick:
                var kickSource = reader.ReadByte();
                var kickTarget = reader.ReadByte();
                EventUtility.handleKick(Helpers.playerById(kickSource), Helpers.playerById(kickTarget),
                    reader.ReadSingle());
                break;
        }
    }
}