using System.Linq;
using System;
using System.Collections.Generic;
using TheOtherRoles.Players;
using static TheOtherRoles.TheOtherRoles;
using UnityEngine;
using TheOtherRoles.Utilities;
using TheOtherRoles.CustomGameModes;
using TheOtherRoles;
using System.Threading.Tasks;
using System.Net.Http;
using TheOtherRoles.Modules;

namespace TheOtherRoles
{
    public class RoleInfo
    {
        public Color color;
        public string name;
        public string introDescription;
        public string shortDescription;
        public RoleId roleId;
        public bool isNeutral;
        public bool isModifier;

        public RoleInfo(string name, Color color, string introDescription, string shortDescription, RoleId roleId, bool isNeutral = false, bool isModifier = false)
        {
            this.color = color;
            this.name = name.Translate();
            this.introDescription = introDescription.Translate();
            this.shortDescription = shortDescription.Translate();
            this.roleId = roleId;
            this.isNeutral = isNeutral;
            this.isModifier = isModifier;
        }

        public static RoleInfo jester = new RoleInfo("Jester", Jester.color, "JesterIntroDesc", "JesterShortDesc", RoleId.Jester, true);
        public static RoleInfo mayor = new RoleInfo("Mayor", Mayor.color, "MayorIntroDesc", "MayorShortDesc", RoleId.Mayor);
        public static RoleInfo portalmaker = new RoleInfo("Portalmaker", Portalmaker.color, "PortalmakerIntroDesc", "PortalmakerShortDesc", RoleId.Portalmaker);
        public static RoleInfo engineer = new RoleInfo("Engineer", Engineer.color, "EngineerIntroDesc", "EngineerShortDesc", RoleId.Engineer);
        public static RoleInfo sheriff = new RoleInfo("Sheriff", Sheriff.color, "SheriffIntroDesc", "SheriffShortDesc", RoleId.Sheriff);
        public static RoleInfo deputy = new RoleInfo("Deputy", Sheriff.color, "DeputyIntroDesc", "DeputyShortDesc", RoleId.Deputy);
        public static RoleInfo lighter = new RoleInfo("Lighter", Lighter.color, "LighterIntroDesc", "LighterShortDesc", RoleId.Lighter);
        public static RoleInfo godfather = new RoleInfo("godfather", Godfather.color, "godfatherntroDesc", "godfatherShortDesc", RoleId.Godfather);
        public static RoleInfo mafioso = new RoleInfo("mafioso", Mafioso.color, "mafiosoIntroDesc", "mafiosoShortDesc", RoleId.Mafioso);
        public static RoleInfo janitor = new RoleInfo("janitor", Janitor.color, "janitorIntroDesc", "janitorShortDesc", RoleId.Janitor);
        public static RoleInfo morphling = new RoleInfo("Morphling", Morphling.color, "MorphlingIntroDesc", "MorphlingShortDesc", RoleId.Morphling);
        public static RoleInfo camouflager = new RoleInfo("Camouflager", Camouflager.color, "CamouflagerIntroDesc", "CamouflagerShortDesc", RoleId.Camouflager);
        public static RoleInfo vampire = new RoleInfo("Vampire", Vampire.color, "VampireIntroDesc", "VampireShortDesc", RoleId.Vampire);
        public static RoleInfo eraser = new RoleInfo("Eraser", Eraser.color, "EraserIntroDesc", "EraserShortDesc", RoleId.Eraser);
        public static RoleInfo trickster = new RoleInfo("Trickster", Trickster.color, "TricksterIntroDesc", "TricksterShortDesc", RoleId.Trickster);
        public static RoleInfo cleaner = new RoleInfo("Cleaner", Cleaner.color, "CleanerIntroDesc", "CleanerShortDesc", RoleId.Cleaner);
        public static RoleInfo warlock = new RoleInfo("Warlock", Warlock.color, "WarlockIntroDesc", "WarlockShortDesc", RoleId.Warlock);
        public static RoleInfo bountyHunter = new RoleInfo("BountyHunter", BountyHunter.color, "BountyHunterIntroDesc", "BountyHunterShortDesc", RoleId.BountyHunter);
        public static RoleInfo detective = new RoleInfo("Detective", Detective.color, "DetectiveIntroDesc", "DetectiveShortDesc", RoleId.Detective);
        public static RoleInfo timeMaster = new RoleInfo("TimeMaster", TimeMaster.color, "TimeMasterIntroDesc", "TimeMasterShortDesc", RoleId.TimeMaster);
        public static RoleInfo medic = new RoleInfo("Medic", Medic.color, "MedicIntroDesc", "MedicShortDesc", RoleId.Medic);
        public static RoleInfo swapper = new RoleInfo("Swapper", Swapper.color, "SwapperIntroDesc", "SwapperShortDesc", RoleId.Swapper);
        public static RoleInfo seer = new RoleInfo("Seer", Seer.color, "SeerIntroDesc", "SeerShortDesc", RoleId.Seer);
        public static RoleInfo hacker = new RoleInfo("Hacker", Hacker.color, "HackerIntroDesc", "HackerShortDesc", RoleId.Hacker);
        public static RoleInfo tracker = new RoleInfo("Tracker", Tracker.color, "TrackerIntroDesc", "TrackerShortDesc", RoleId.Tracker);
        public static RoleInfo snitch = new RoleInfo("Snitch", Snitch.color, "SnitchIntroDesc", "SnitchShortDesc", RoleId.Snitch);
        public static RoleInfo investigator = new("Prophet", Prophet.color, "ProphetIntroDesc" , "ProphetShortDesc", RoleId.Investigator);
        public static RoleInfo jackal = new RoleInfo("Jackal", Jackal.color, "JackalIntroDesc", "JackalShortDesc", RoleId.Jackal, true);
        public static RoleInfo sidekick = new RoleInfo("Sidekick", Sidekick.color, "SidekickIntroDesc", "SidekickShortDesc", RoleId.Sidekick, true);
        public static RoleInfo spy = new RoleInfo("Spy", Spy.color, "SpyIntroDesc", "SpyShortDesc", RoleId.Spy);
        public static RoleInfo securityGuard = new RoleInfo("SecurityGuard", SecurityGuard.color, "SecurityGuardIntroDesc", "SecurityGuardShortDesc", RoleId.SecurityGuard);
        public static RoleInfo arsonist = new RoleInfo("Arsonist", Arsonist.color, "ArsonistIntroDesc", "ArsonistShortDesc", RoleId.Arsonist, true);
        public static RoleInfo goodGuesser = new RoleInfo("Vigilante", Guesser.color, "VigilanteIntroDesc", "VigilanteShortDesc", RoleId.NiceGuesser);
        public static RoleInfo badGuesser = new RoleInfo("Assassin", Palette.ImpostorRed, "AssassinIntroDesc", "AssassinShortDesc", RoleId.EvilGuesser);
        public static RoleInfo vulture = new RoleInfo("Vulture", Vulture.color, "VultureIntroDesc", "VultureShortDesc", RoleId.Vulture, true);
        public static RoleInfo medium = new RoleInfo("Medium", Medium.color, "MediumIntroDesc", "MediumShortDesc", RoleId.Medium);
        public static RoleInfo trapper = new RoleInfo("Trapper", Trapper.color, "TrapperIntroDesc", "TrapperShortDesc", RoleId.Trapper);
        public static RoleInfo lawyer = new RoleInfo("Lawyer", Lawyer.color, "LawyerIntroDesc", "LawyerShortDesc", RoleId.Lawyer, true);
        public static RoleInfo prosecutor = new RoleInfo("Prosecutor", Lawyer.color, "ProsecutorIntroDesc", "ProsecutorShortDesc", RoleId.Prosecutor, true);
        public static RoleInfo pursuer = new RoleInfo("Pursuer", Pursuer.color, "PursuerIntroDesc", "PursuerShortDesc", RoleId.Pursuer);
        public static RoleInfo impostor = new RoleInfo("Impostor", Palette.ImpostorRed, Helpers.cs(Palette.ImpostorRed, "ImpostorIntroDesc"), "ImpostorShortDesc", RoleId.Impostor);
        public static RoleInfo crewmate = new RoleInfo("Crewmate", Color.white, "CrewmateIntroDesc", "CrewmateShortDesc", RoleId.Crewmate);
        public static RoleInfo witch = new RoleInfo("Witch", Witch.color, "WitchIntroDesc", "WitchShortDesc", RoleId.Witch);
        public static RoleInfo ninja = new RoleInfo("Ninja", Ninja.color, "NinjaIntroDesc", "NinjaShortDesc", RoleId.Ninja);
        public static RoleInfo thief = new RoleInfo("Thief", Thief.color, "ThiefIntroDesc", "ThiefShortDesc", RoleId.Thief, true);
        public static RoleInfo bomber = new RoleInfo("Bomber", Bomber.color, "BomberIntroDesc", "BomberShortDesc", RoleId.Bomber);
        public static RoleInfo yoyo = new RoleInfo("Yoyo", Yoyo.color, "YoyoIntroDesc", "YoyoShortDesc", RoleId.Yoyo);

        public static RoleInfo hunter = new RoleInfo("Hunter", Palette.ImpostorRed, Helpers.cs(Palette.ImpostorRed, "HunterIntroDesc"), "HunterShortDesc", RoleId.Impostor);
        public static RoleInfo hunted = new RoleInfo("Hunted", Color.white, "HuntedIntroDesc", "HuntedShortDesc", RoleId.Crewmate);

        public static RoleInfo prop = new RoleInfo("Prop", Color.white, "PropIntroDesc", "PropShortDesc", RoleId.Crewmate);



        // Modifier
        public static RoleInfo bloody = new RoleInfo("Bloody", Color.yellow, "BloodyIntroDesc", "BloodyShortDesc", RoleId.Bloody, false, true);
        public static RoleInfo antiTeleport = new RoleInfo("AntiTeleport", Color.yellow, "AntiTeleportIntroDesc", "AntiTeleportShortDesc", RoleId.AntiTeleport, false, true);
        public static RoleInfo tiebreaker = new RoleInfo("TieBreaker", Color.yellow, "TieBreakerIntroDesc", "TieBreakerShortDesc", RoleId.Tiebreaker, false, true);
        public static RoleInfo bait = new RoleInfo("Bait", Color.yellow, "BaitIntroDesc", "BaitShortDesc", RoleId.Bait, false, true);
        public static RoleInfo sunglasses = new RoleInfo("Sunglasses", Color.yellow, "SunglassesIntroDesc", "SunglassesShortDesc", RoleId.Sunglasses, false, true);
        public static RoleInfo lighterln = new RoleInfo("Torch", Color.yellow, "TorchIntroDesc", "TorchShortDesc", RoleId.Lighterln, false, true);
        public static RoleInfo lover = new RoleInfo("Lover", Lovers.color, "LoverIntroDesc", "LoverShortDesc", RoleId.Lover, false, true);
        public static RoleInfo mini = new RoleInfo("Mini", Color.yellow, "MiniIntroDesc", "MiniShortDesc", RoleId.Mini, false, true);
        public static RoleInfo vip = new RoleInfo("Vip", Color.yellow, "VipIntroDesc", "VipShortDesc", RoleId.Vip, false, true);
        public static RoleInfo invert = new RoleInfo("醉鬼", Color.yellow, "InvertIntroDesc", "InvertShortDesc", RoleId.Invert, false, true);
        public static RoleInfo chameleon = new RoleInfo("Chameleon", Color.yellow, "ChameleonIntroDesc", "ChameleonShortDesc", RoleId.Chameleon, false, true);
        public static RoleInfo shifter = new RoleInfo("Shifter", Color.yellow, "ShifterIntroDesc", "ShifterShortDesc", RoleId.Shifter, false, true);


        public static List<RoleInfo> allRoleInfos = new List<RoleInfo>() {
            impostor,
            godfather,
            mafioso,
            janitor,
            morphling,
            camouflager,
            vampire,
            eraser,
            trickster,
            cleaner,
            warlock,
            bountyHunter,
            witch,
            ninja,
            bomber,
            yoyo,
            goodGuesser,
            badGuesser,
            lover,
            jester,
            arsonist,
            jackal,
            sidekick,
            vulture,
            pursuer,
            lawyer,
            thief,
            prosecutor,
            crewmate,
            mayor,
            portalmaker,
            engineer,
            sheriff,
            deputy,
            lighter,
            detective,
            timeMaster,
            medic,
            swapper,
            seer,
            hacker,
            tracker,
            snitch,
            spy,
            securityGuard,
            bait,
            medium,
            trapper,
            bloody,
            antiTeleport,
            tiebreaker,
            sunglasses,
            lighterln,
            mini,
            vip,
            invert,
            chameleon,
            shifter
        };

        public static List<RoleInfo> getRoleInfoForPlayer(PlayerControl p, bool showModifier = true)
        {
            List<RoleInfo> infos = new List<RoleInfo>();
            if (p == null) return infos;

            // Modifier
            if (showModifier)
            {
                // after dead modifier
                if (!CustomOptionHolder.modifiersAreHidden.getBool() || PlayerControl.LocalPlayer.Data.IsDead || AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Ended)
                {
                    if (Bait.bait.Any(x => x.PlayerId == p.PlayerId)) infos.Add(bait);
                    if (Bloody.bloody.Any(x => x.PlayerId == p.PlayerId)) infos.Add(bloody);
                    if (Vip.vip.Any(x => x.PlayerId == p.PlayerId)) infos.Add(vip);
                }
                if (p == Lovers.lover1 || p == Lovers.lover2) infos.Add(lover);
                if (p == Tiebreaker.tiebreaker) infos.Add(tiebreaker);
                if (AntiTeleport.antiTeleport.Any(x => x.PlayerId == p.PlayerId)) infos.Add(antiTeleport);
                if (Sunglasses.sunglasses.Any(x => x.PlayerId == p.PlayerId)) infos.Add(sunglasses);
                if (Lighterln.lighterln.Any(x => x.PlayerId == p.PlayerId)) infos.Add(lighterln);
                if (p == Mini.mini) infos.Add(mini);
                if (Invert.invert.Any(x => x.PlayerId == p.PlayerId)) infos.Add(invert);
                if (Chameleon.chameleon.Any(x => x.PlayerId == p.PlayerId)) infos.Add(chameleon);
                if (p == Shifter.shifter) infos.Add(shifter);
            }

            int count = infos.Count;  // Save count after modifiers are added so that the role count can be checked

            // Special roles
            if (p == Jester.jester) infos.Add(jester);
            if (p == Mayor.mayor) infos.Add(mayor);
            if (p == Portalmaker.portalmaker) infos.Add(portalmaker);
            if (p == Engineer.engineer) infos.Add(engineer);
            if (p == Sheriff.sheriff || p == Sheriff.formerSheriff) infos.Add(sheriff);
            if (p == Deputy.deputy) infos.Add(deputy);
            if (p == Lighter.lighter) infos.Add(lighter);
            if (p == Godfather.godfather) infos.Add(godfather);
            if (p == Mafioso.mafioso) infos.Add(mafioso);
            if (p == Janitor.janitor) infos.Add(janitor);
            if (p == Morphling.morphling) infos.Add(morphling);
            if (p == Camouflager.camouflager) infos.Add(camouflager);
            if (p == Vampire.vampire) infos.Add(vampire);
            if (p == Eraser.eraser) infos.Add(eraser);
            if (p == Trickster.trickster) infos.Add(trickster);
            if (p == Cleaner.cleaner) infos.Add(cleaner);
            if (p == Warlock.warlock) infos.Add(warlock);
            if (p == Witch.witch) infos.Add(witch);
            if (p == Ninja.ninja) infos.Add(ninja);
            if (p == Bomber.bomber) infos.Add(bomber);
            if (p == Yoyo.yoyo) infos.Add(yoyo);
            if (p == Detective.detective) infos.Add(detective);
            if (p == TimeMaster.timeMaster) infos.Add(timeMaster);
            if (p == Medic.medic) infos.Add(medic);
            if (p == Swapper.swapper) infos.Add(swapper);
            if (p == Seer.seer) infos.Add(seer);
            if (p == Hacker.hacker) infos.Add(hacker);
            if (p == Tracker.tracker) infos.Add(tracker);
            if (p == Snitch.snitch) infos.Add(snitch);
            if (p == Prophet.prophet) infos.Add(investigator);
            if (p == Jackal.jackal || (Jackal.formerJackals != null && Jackal.formerJackals.Any(x => x.PlayerId == p.PlayerId))) infos.Add(jackal);
            if (p == Sidekick.sidekick) infos.Add(sidekick);
            if (p == Spy.spy) infos.Add(spy);
            if (p == SecurityGuard.securityGuard) infos.Add(securityGuard);
            if (p == Arsonist.arsonist) infos.Add(arsonist);
            if (p == Guesser.niceGuesser) infos.Add(goodGuesser);
            if (p == Guesser.evilGuesser) infos.Add(badGuesser);
            if (p == BountyHunter.bountyHunter) infos.Add(bountyHunter);
            if (p == Vulture.vulture) infos.Add(vulture);
            if (p == Medium.medium) infos.Add(medium);
            if (p == Lawyer.lawyer && !Lawyer.isProsecutor) infos.Add(lawyer);
            if (p == Lawyer.lawyer && Lawyer.isProsecutor) infos.Add(prosecutor);
            if (p == Trapper.trapper) infos.Add(trapper);
            if (p == Pursuer.pursuer) infos.Add(pursuer);
            if (p == Thief.thief) infos.Add(thief);

            // Default roles (just impostor, just crewmate, or hunter / hunted for hide n seek, prop hunt prop ...
            if (infos.Count == count)
            {
                if (p.Data.Role.IsImpostor)
                    infos.Add(TORMapOptions.gameMode == CustomGamemodes.HideNSeek || TORMapOptions.gameMode == CustomGamemodes.PropHunt ? RoleInfo.hunter : RoleInfo.impostor);
                else
                    infos.Add(TORMapOptions.gameMode == CustomGamemodes.HideNSeek ? RoleInfo.hunted : TORMapOptions.gameMode == CustomGamemodes.PropHunt ? RoleInfo.prop : RoleInfo.crewmate);
            }

            return infos;
        }

        public static String GetRolesString(PlayerControl p, bool useColors, bool showModifier = true, bool suppressGhostInfo = false)
        {
            string roleName;
            roleName = String.Join(" ", getRoleInfoForPlayer(p, showModifier).Select(x => useColors ? Helpers.cs(x.color, x.name) : x.name).ToArray());
            if (Lawyer.target != null && p.PlayerId == Lawyer.target.PlayerId && CachedPlayer.LocalPlayer.PlayerControl != Lawyer.target)
                roleName += (useColors ? Helpers.cs(Pursuer.color, " §") : " §");
            if (HandleGuesser.isGuesserGm && HandleGuesser.isGuesser(p.PlayerId)) roleName += " (赌怪)";

            if (!suppressGhostInfo && p != null)
            {
                if (p == Shifter.shifter && (CachedPlayer.LocalPlayer.PlayerControl == Shifter.shifter || Helpers.shouldShowGhostInfo()) && Shifter.futureShift != null)
                    roleName += Helpers.cs(Color.yellow, " ← " + Shifter.futureShift.Data.PlayerName);
                if (p == Vulture.vulture && (CachedPlayer.LocalPlayer.PlayerControl == Vulture.vulture || Helpers.shouldShowGhostInfo()))
                    roleName = roleName + Helpers.cs(Vulture.color, $" (剩余{Vulture.vultureNumberToWin - Vulture.eatenBodies} 个尸体)");
                if (CustomOptionHolder.bountyHunterShowCooldownForGhosts.getBool())
                {
                    if (p == BountyHunter.bountyHunter && (CachedPlayer.LocalPlayer.PlayerControl == BountyHunter.bountyHunter || Helpers.shouldShowGhostInfo()))
                        roleName = roleName + Helpers.cs(BountyHunter.color, $" (正确击杀：{BountyHunter.bountyKillCooldown}秒 错误击杀：{BountyHunter.punishmentTime}秒)");
                }
                if (Helpers.shouldShowGhostInfo())
                {
                    if (Eraser.futureErased.Contains(p))
                        roleName = Helpers.cs(Color.gray, "(被抹除) ") + roleName;
                    if (Vampire.vampire != null && !Vampire.vampire.Data.IsDead && Vampire.bitten == p && !p.Data.IsDead)
                        roleName = Helpers.cs(Vampire.color, $"(吸血: {(int)HudManagerStartPatch.vampireKillButton.Timer + 1}) ") + roleName;
                    if (Deputy.handcuffedPlayers.Contains(p.PlayerId))
                        roleName = Helpers.cs(Color.gray, "(拷住) ") + roleName;
                    if (Deputy.handcuffedKnows.ContainsKey(p.PlayerId))  // Active cuff
                        roleName = Helpers.cs(Deputy.color, "(拷住) ") + roleName;
                    if (p == Warlock.curseVictim)
                        roleName = Helpers.cs(Warlock.color, "(咒术) ") + roleName;
                    if (p == Ninja.ninjaMarked)
                        roleName = Helpers.cs(Ninja.color, "(标记) ") + roleName;
                    if (Pursuer.blankedList.Contains(p) && !p.Data.IsDead)
                        roleName = Helpers.cs(Pursuer.color, "(空包弹) ") + roleName;
                    if (Witch.futureSpelled.Contains(p) && !MeetingHud.Instance) // This is already displayed in meetings!
                        roleName = Helpers.cs(Witch.color, "☆ ") + roleName;
                    if (BountyHunter.bounty == p)
                        roleName = Helpers.cs(BountyHunter.color, "(悬赏) ") + roleName;
                    if (Arsonist.dousedPlayers.Contains(p))
                        roleName = Helpers.cs(Arsonist.color, "♨ ") + roleName;
                    if (p == Arsonist.arsonist)
                        roleName = roleName + Helpers.cs(Arsonist.color, $" (剩余{CachedPlayer.AllPlayers.Count(x => { return x.PlayerControl != Arsonist.arsonist && !x.Data.IsDead && !x.Data.Disconnected && !Arsonist.dousedPlayers.Any(y => y.PlayerId == x.PlayerId); })} 个)");
                    if (p == Jackal.fakeSidekick)
                        roleName = Helpers.cs(Sidekick.color, $" (假跟班)") + roleName;

                    // Death Reason on Ghosts
                    if (p.Data.IsDead)
                    {
                        string deathReasonString = "";
                        var deadPlayer = GameHistory.deadPlayers.FirstOrDefault(x => x.player.PlayerId == p.PlayerId);

                        Color killerColor = new();
                        if (deadPlayer != null && deadPlayer.killerIfExisting != null)
                        {
                            killerColor = RoleInfo.getRoleInfoForPlayer(deadPlayer.killerIfExisting, false).FirstOrDefault().color;
                        }

                        if (deadPlayer != null)
                        {
                            switch (deadPlayer.deathReason)
                            {
                                case DeadPlayer.CustomDeathReason.Disconnect:
                                    deathReasonString = " - 断连";
                                    break;
                                case DeadPlayer.CustomDeathReason.Exile:
                                    deathReasonString = " - 票出";
                                    break;
                                case DeadPlayer.CustomDeathReason.Kill:
                                    deathReasonString = $" - 被杀于 {Helpers.cs(killerColor, deadPlayer.killerIfExisting.Data.PlayerName)}";
                                    break;
                                case DeadPlayer.CustomDeathReason.Guess:
                                    if (deadPlayer.killerIfExisting.Data.PlayerName == p.Data.PlayerName)
                                        deathReasonString = $" - 错误猜测";
                                    else
                                        deathReasonString = $" - 被 {Helpers.cs(killerColor, deadPlayer.killerIfExisting.Data.PlayerName)} 猜测";
                                    break;
                                case DeadPlayer.CustomDeathReason.Shift:
                                    deathReasonString = $" - {Helpers.cs(Color.yellow, "偷窃")} {Helpers.cs(killerColor, deadPlayer.killerIfExisting.Data.PlayerName)}";
                                    break;
                                case DeadPlayer.CustomDeathReason.WitchExile:
                                    deathReasonString = $" - {Helpers.cs(Witch.color, "下咒")} 于 {Helpers.cs(killerColor, deadPlayer.killerIfExisting.Data.PlayerName)}";
                                    break;
                                case DeadPlayer.CustomDeathReason.LoverSuicide:
                                    deathReasonString = $" - {Helpers.cs(Lovers.color, "殉情")}";
                                    break;
                                case DeadPlayer.CustomDeathReason.LawyerSuicide:
                                    deathReasonString = $" - {Helpers.cs(Lawyer.color, "死亡律师")}";
                                    break;
                                case DeadPlayer.CustomDeathReason.Bomb:
                                    deathReasonString = $" - 被炸于 {Helpers.cs(killerColor, deadPlayer.killerIfExisting.Data.PlayerName)}";
                                    break;
                                case DeadPlayer.CustomDeathReason.Arson:
                                    deathReasonString = $" - 烧死于 {Helpers.cs(killerColor, deadPlayer.killerIfExisting.Data.PlayerName)}";
                                    break;
                            }
                            roleName = roleName + deathReasonString;
                        }
                    }
                }
            }
            return roleName;
        }


        static string ReadmePage = "";
        public static async Task loadReadme()
        {
            if (ReadmePage == "")
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync("https://raw.githubusercontent.com/TheOtherRolesAU/TheOtherRoles/main/README.md");
                response.EnsureSuccessStatusCode();
                string httpres = await response.Content.ReadAsStringAsync();
                ReadmePage = httpres;
            }
        }
        public static string GetRoleDescription(RoleInfo roleInfo)
        {
            while (ReadmePage == "")
            {
            }

            int index = ReadmePage.IndexOf($"## {roleInfo.name}");
            int endindex = ReadmePage.Substring(index).IndexOf("### 复盘:");
            return ReadmePage.Substring(index, endindex);

        }

    }
}
