using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TheOtherRoles.Modules;
using TheOtherRoles.Players;
using TheOtherRoles.Utilities;
using UnityEngine;

namespace TheOtherRoles
{
    // Token: 0x02000029 RID: 41
    public class RoleInfo
    {
        // Token: 0x060000D9 RID: 217 RVA: 0x00012124 File Offset: 0x00010324
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

        // Token: 0x060000DA RID: 218 RVA: 0x00012180 File Offset: 0x00010380
        public static List<RoleInfo> getRoleInfoForPlayer(PlayerControl p, bool showModifier = true)
        {
            List<RoleInfo> list = new List<RoleInfo>();
            bool flag = p == null;
            List<RoleInfo> result;
            if (flag)
            {
                result = list;
            }
            else
            {
                if (showModifier)
                {
                    bool flag2 = !CustomOptionHolder.modifiersAreHidden.getBool() || PlayerControl.LocalPlayer.Data.IsDead || AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Ended;
                    if (flag2)
                    {
                        bool flag3 = Bait.bait.Any((PlayerControl x) => x.PlayerId == p.PlayerId);
                        if (flag3)
                        {
                            list.Add(RoleInfo.bait);
                        }
                        bool flag4 = Bloody.bloody.Any((PlayerControl x) => x.PlayerId == p.PlayerId);
                        if (flag4)
                        {
                            list.Add(RoleInfo.bloody);
                        }
                        bool flag5 = Vip.vip.Any((PlayerControl x) => x.PlayerId == p.PlayerId);
                        if (flag5)
                        {
                            list.Add(RoleInfo.vip);
                        }
                    }
                    bool flag6 = p == Lovers.lover1 || p == Lovers.lover2;
                    if (flag6)
                    {
                        list.Add(RoleInfo.lover);
                    }
                    bool flag7 = p == Tiebreaker.tiebreaker;
                    if (flag7)
                    {
                        list.Add(RoleInfo.tiebreaker);
                    }
                    bool flag8 = AntiTeleport.antiTeleport.Any((PlayerControl x) => x.PlayerId == p.PlayerId);
                    if (flag8)
                    {
                        list.Add(RoleInfo.antiTeleport);
                    }
                    bool flag9 = Sunglasses.sunglasses.Any((PlayerControl x) => x.PlayerId == p.PlayerId);
                    if (flag9)
                    {
                        list.Add(RoleInfo.sunglasses);
                    }
                    bool flag10 = Lighterln.lighterln.Any((PlayerControl x) => x.PlayerId == p.PlayerId);
                    if (flag10)
                    {
                        list.Add(RoleInfo.lighterln);
                    }
                    bool flag11 = p == Mini.mini;
                    if (flag11)
                    {
                        list.Add(RoleInfo.mini);
                    }
                    bool flag12 = Invert.invert.Any((PlayerControl x) => x.PlayerId == p.PlayerId);
                    if (flag12)
                    {
                        list.Add(RoleInfo.invert);
                    }
                    bool flag13 = Chameleon.chameleon.Any((PlayerControl x) => x.PlayerId == p.PlayerId);
                    if (flag13)
                    {
                        list.Add(RoleInfo.chameleon);
                    }
                    bool flag14 = p == Shifter.shifter;
                    if (flag14)
                    {
                        list.Add(RoleInfo.shifter);
                    }
                }
                int count = list.Count;
                bool flag15 = p == TheOtherRoles.Jester.jester;
                if (flag15)
                {
                    list.Add(RoleInfo.jester);
                }
                bool flag16 = p == TheOtherRoles.Mayor.mayor;
                if (flag16)
                {
                    list.Add(RoleInfo.mayor);
                }
                bool flag17 = p == TheOtherRoles.Portalmaker.portalmaker;
                if (flag17)
                {
                    list.Add(RoleInfo.portalmaker);
                }
                bool flag18 = p == TheOtherRoles.Engineer.engineer;
                if (flag18)
                {
                    list.Add(RoleInfo.engineer);
                }
                bool flag19 = p == TheOtherRoles.Sheriff.sheriff || p == TheOtherRoles.Sheriff.formerSheriff;
                if (flag19)
                {
                    list.Add(RoleInfo.sheriff);
                }
                bool flag20 = p == TheOtherRoles.Deputy.deputy;
                if (flag20)
                {
                    list.Add(RoleInfo.deputy);
                }
                bool flag21 = p == TheOtherRoles.Lighter.lighter;
                if (flag21)
                {
                    list.Add(RoleInfo.lighter);
                }
                bool flag22 = p == TheOtherRoles.Godfather.godfather;
                if (flag22)
                {
                    list.Add(RoleInfo.godfather);
                }
                bool flag23 = p == TheOtherRoles.Mafioso.mafioso;
                if (flag23)
                {
                    list.Add(RoleInfo.mafioso);
                }
                bool flag24 = p == TheOtherRoles.Janitor.janitor;
                if (flag24)
                {
                    list.Add(RoleInfo.janitor);
                }
                bool flag25 = p == Morphling.morphling;
                if (flag25)
                {
                    list.Add(RoleInfo.morphling);
                }
                bool flag26 = p == Camouflager.camouflager;
                if (flag26)
                {
                    list.Add(RoleInfo.camouflager);
                }
                bool flag27 = p == Vampire.vampire;
                if (flag27)
                {
                    list.Add(RoleInfo.vampire);
                }
                bool flag28 = p == Eraser.eraser;
                if (flag28)
                {
                    list.Add(RoleInfo.eraser);
                }
                bool flag29 = p == Trickster.trickster;
                if (flag29)
                {
                    list.Add(RoleInfo.trickster);
                }
                bool flag30 = p == Cleaner.cleaner;
                if (flag30)
                {
                    list.Add(RoleInfo.cleaner);
                }
                bool flag31 = p == Warlock.warlock;
                if (flag31)
                {
                    list.Add(RoleInfo.warlock);
                }
                bool flag32 = p == Witch.witch;
                if (flag32)
                {
                    list.Add(RoleInfo.witch);
                }
                bool flag33 = p == Ninja.ninja;
                if (flag33)
                {
                    list.Add(RoleInfo.ninja);
                }
                bool flag34 = p == Bomber.bomber;
                if (flag34)
                {
                    list.Add(RoleInfo.bomber);
                }
                bool flag35 = p == Yoyo.yoyo;
                if (flag35)
                {
                    list.Add(RoleInfo.yoyo);
                }
                bool flag36 = p == TheOtherRoles.Detective.detective;
                if (flag36)
                {
                    list.Add(RoleInfo.detective);
                }
                bool flag37 = p == TimeMaster.timeMaster;
                if (flag37)
                {
                    list.Add(RoleInfo.timeMaster);
                }
                bool flag38 = p == Medic.medic;
                if (flag38)
                {
                    list.Add(RoleInfo.medic);
                }
                bool flag39 = p == Swapper.swapper;
                if (flag39)
                {
                    list.Add(RoleInfo.swapper);
                }
                bool flag40 = p == Seer.seer;
                if (flag40)
                {
                    list.Add(RoleInfo.seer);
                }
                bool flag41 = p == Hacker.hacker;
                if (flag41)
                {
                    list.Add(RoleInfo.hacker);
                }
                bool flag42 = p == Tracker.tracker;
                if (flag42)
                {
                    list.Add(RoleInfo.tracker);
                }
                bool flag43 = p == Snitch.snitch;
                if (flag43)
                {
                    list.Add(RoleInfo.snitch);
                }
                bool flag44 = p == Prophet.prophet;
                if (flag44)
                {
                    list.Add(RoleInfo.investigator);
                }
                bool flag45 = p == Jackal.jackal || (Jackal.formerJackals != null && Jackal.formerJackals.Any((PlayerControl x) => x.PlayerId == p.PlayerId));
                if (flag45)
                {
                    list.Add(RoleInfo.jackal);
                }
                bool flag46 = p == Sidekick.sidekick;
                if (flag46)
                {
                    list.Add(RoleInfo.sidekick);
                }
                bool flag47 = p == Spy.spy;
                if (flag47)
                {
                    list.Add(RoleInfo.spy);
                }
                bool flag48 = p == SecurityGuard.securityGuard;
                if (flag48)
                {
                    list.Add(RoleInfo.securityGuard);
                }
                bool flag49 = p == Arsonist.arsonist;
                if (flag49)
                {
                    list.Add(RoleInfo.arsonist);
                }
                bool flag50 = p == Guesser.niceGuesser;
                if (flag50)
                {
                    list.Add(RoleInfo.goodGuesser);
                }
                bool flag51 = p == Guesser.evilGuesser;
                if (flag51)
                {
                    list.Add(RoleInfo.badGuesser);
                }
                bool flag52 = p == BountyHunter.bountyHunter;
                if (flag52)
                {
                    list.Add(RoleInfo.bountyHunter);
                }
                bool flag53 = p == Vulture.vulture;
                if (flag53)
                {
                    list.Add(RoleInfo.vulture);
                }
                bool flag54 = p == Medium.medium;
                if (flag54)
                {
                    list.Add(RoleInfo.medium);
                }
                bool flag55 = p == Lawyer.lawyer && !Lawyer.isProsecutor;
                if (flag55)
                {
                    list.Add(RoleInfo.lawyer);
                }
                bool flag56 = p == Lawyer.lawyer && Lawyer.isProsecutor;
                if (flag56)
                {
                    list.Add(RoleInfo.prosecutor);
                }
                bool flag57 = p == Trapper.trapper;
                if (flag57)
                {
                    list.Add(RoleInfo.trapper);
                }
                bool flag58 = p == Pursuer.pursuer;
                if (flag58)
                {
                    list.Add(RoleInfo.pursuer);
                }
                bool flag59 = p == Thief.thief;
                if (flag59)
                {
                    list.Add(RoleInfo.thief);
                }
                bool flag60 = p == Fraudster.fraudster;
                if (flag60)
                {
                    list.Add(RoleInfo.fraudster);
                }
                bool flag61 = list.Count == count;
                if (flag61)
                {
                    bool isImpostor = p.Data.Role.IsImpostor;
                    if (isImpostor)
                    {
                        list.Add((TORMapOptions.gameMode == CustomGamemodes.HideNSeek || TORMapOptions.gameMode == CustomGamemodes.PropHunt) ? RoleInfo.hunter : RoleInfo.impostor);
                    }
                    else
                    {
                        list.Add((TORMapOptions.gameMode == CustomGamemodes.HideNSeek) ? RoleInfo.hunted : ((TORMapOptions.gameMode == CustomGamemodes.PropHunt) ? RoleInfo.prop : RoleInfo.crewmate));
                    }
                }
                result = list;
            }
            return result;
        }

        // Token: 0x060000DB RID: 219 RVA: 0x00012AD0 File Offset: 0x00010CD0
        public static string GetRolesString(PlayerControl p, bool useColors, bool showModifier = true, bool suppressGhostInfo = false)
        {
            string text = string.Join(" ", (from x in RoleInfo.getRoleInfoForPlayer(p, showModifier)
                                            select useColors ? Helpers.cs(x.color, x.name) : x.name).ToArray<string>());
            bool flag = Lawyer.target != null && p.PlayerId == Lawyer.target.PlayerId && CachedPlayer.LocalPlayer.PlayerControl != Lawyer.target;
            if (flag)
            {
                text += (useColors ? Helpers.cs(Pursuer.color, " §") : " §");
            }
            bool flag2 = HandleGuesser.isGuesserGm && HandleGuesser.isGuesser(p.PlayerId);
            if (flag2)
            {
                text += "GuessserGMInfo".Translate();
            }
            bool flag3 = !suppressGhostInfo && p != null;
            if (flag3)
            {
                bool flag4 = p == Shifter.shifter && (CachedPlayer.LocalPlayer.PlayerControl == Shifter.shifter || Helpers.shouldShowGhostInfo()) && Shifter.futureShift != null;
                if (flag4)
                {
                    text += Helpers.cs(Color.yellow, " ← " + Shifter.futureShift.Data.PlayerName);
                }
                bool flag5 = p == Vulture.vulture && (CachedPlayer.LocalPlayer.PlayerControl == Vulture.vulture || Helpers.shouldShowGhostInfo());
                if (flag5)
                {
                    string str = text;
                    Color c = Vulture.color;
                    DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(9, 1);
                    defaultInterpolatedStringHandler.AppendLiteral(" (剩余");
                    defaultInterpolatedStringHandler.AppendFormatted<int>(Vulture.vultureNumberToWin - Vulture.eatenBodies);
                    defaultInterpolatedStringHandler.AppendLiteral(" 个尸体)");
                    text = str + Helpers.cs(c, defaultInterpolatedStringHandler.ToStringAndClear());
                }
                bool @bool = CustomOptionHolder.bountyHunterShowCooldownForGhosts.getBool();
                if (@bool)
                {
                    bool flag6 = p == BountyHunter.bountyHunter && (CachedPlayer.LocalPlayer.PlayerControl == BountyHunter.bountyHunter || Helpers.shouldShowGhostInfo());
                    if (flag6)
                    {
                        string str2 = text;
                        Color c2 = BountyHunter.color;
                        DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(16, 2);
                        defaultInterpolatedStringHandler.AppendLiteral(" (正确击杀：");
                        defaultInterpolatedStringHandler.AppendFormatted<float>(BountyHunter.bountyKillCooldown);
                        defaultInterpolatedStringHandler.AppendLiteral("秒 错误击杀：");
                        defaultInterpolatedStringHandler.AppendFormatted<float>(BountyHunter.punishmentTime);
                        defaultInterpolatedStringHandler.AppendLiteral("秒)");
                        text = str2 + Helpers.cs(c2, defaultInterpolatedStringHandler.ToStringAndClear());
                    }
                }
                bool flag7 = Helpers.shouldShowGhostInfo();
                if (flag7)
                {
                    bool flag8 = Eraser.futureErased.Contains(p);
                    if (flag8)
                    {
                        text = Helpers.cs(Color.gray, $"({ModTranslation.GetString("ButtoneraserErase")}) ") + text;
                    }
                    bool flag9 = Vampire.vampire != null && !Vampire.vampire.Data.IsDead && Vampire.bitten == p && !p.Data.IsDead;
                    if (flag9)
                    {
                        Color c3 = Vampire.color;
                        DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(7, 1);
                        defaultInterpolatedStringHandler.AppendLiteral($"({ModTranslation.GetString("ButtonvampireBite")}: ");
                        defaultInterpolatedStringHandler.AppendFormatted<int>((int)HudManagerStartPatch.vampireKillButton.Timer + 1);
                        defaultInterpolatedStringHandler.AppendLiteral(") ");
                        text = Helpers.cs(c3, defaultInterpolatedStringHandler.ToStringAndClear()) + text;
                    }
                    bool flag10 = TheOtherRoles.Deputy.handcuffedPlayers.Contains(p.PlayerId);
                    if (flag10)
                    {
                        text = Helpers.cs(Color.gray, $"({ModTranslation.GetString("ButtondeputyHandcuff")}) ") + text;
                    }
                    bool flag11 = TheOtherRoles.Deputy.handcuffedKnows.ContainsKey(p.PlayerId);
                    if (flag11)
                    {
                        text = Helpers.cs(TheOtherRoles.Deputy.color, $"({ModTranslation.GetString("ButtondeputyHandcuff")}) ") + text;
                    }
                    bool flag12 = p == Warlock.curseVictim;
                    if (flag12)
                    {
                        text = Helpers.cs(Warlock.color, $"({ModTranslation.GetString("ButtonwarlockCurse")}) ") + text;
                    }
                    bool flag13 = p == Ninja.ninjaMarked;
                    if (flag13)
                    {
                        text = Helpers.cs(Ninja.color, $"({ModTranslation.GetString("ButtonMark")}) ") + text;
                    }
                    bool flag14 = Pursuer.blankedList.Contains(p) && !p.Data.IsDead;
                    if (flag14)
                    {
                        text = Helpers.cs(Pursuer.color, $"({ModTranslation.GetString("ButtonpursuerBlank")}) ") + text;
                    }
                    bool flag15 = Witch.futureSpelled.Contains(p) && !MeetingHud.Instance;
                    if (flag15)
                    {
                        text = Helpers.cs(Witch.color, "☆ ") + text;
                    }
                    bool flag16 = BountyHunter.bounty == p;
                    if (flag16)
                    {
                        text = Helpers.cs(BountyHunter.color, "☆") + text;
                    }
                    bool flag17 = Arsonist.dousedPlayers.Contains(p);
                    if (flag17)
                    {
                        text = Helpers.cs(Arsonist.color, "♨ ") + text;
                    }
                    bool flag18 = p == Arsonist.arsonist;
                    if (flag18)
                    {
                        string str3 = text;
                        Color c4 = Arsonist.color;
                        DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(7, 1);
                        defaultInterpolatedStringHandler.AppendLiteral("VultrueLeft".Translate());
                        defaultInterpolatedStringHandler.AppendFormatted<int>(CachedPlayer.AllPlayers.Count((CachedPlayer x) => x.PlayerControl != Arsonist.arsonist && !x.Data.IsDead && !x.Data.Disconnected && !Arsonist.dousedPlayers.Any((PlayerControl y) => y.PlayerId == x.PlayerId)));
                        defaultInterpolatedStringHandler.AppendLiteral("VultrueLeft2".Translate());
                        text = str3 + Helpers.cs(c4, defaultInterpolatedStringHandler.ToStringAndClear());
                    }
                    bool flag19 = p == Jackal.fakeSidekick;
                    if (flag19)
                    {
                        text = Helpers.cs(Sidekick.color, "FakeSidekick".Translate()) + text;
                    }
                    bool isDead = p.Data.IsDead;
                    if (isDead)
                    {
                        string str4 = "";
                        DeadPlayer deadPlayer = GameHistory.deadPlayers.FirstOrDefault((DeadPlayer x) => x.player.PlayerId == p.PlayerId);
                        Color c5 = default(Color);
                        bool flag20 = deadPlayer != null && deadPlayer.killerIfExisting != null;
                        if (flag20)
                        {
                            c5 = RoleInfo.getRoleInfoForPlayer(deadPlayer.killerIfExisting, false).FirstOrDefault<RoleInfo>().color;
                        }
                        bool flag21 = deadPlayer != null;
                        if (flag21)
                        {
                            switch (deadPlayer.deathReason)
                            {
                                case DeadPlayer.CustomDeathReason.Exile:
                                    str4 = "DeadReasonExile".Translate();
                                    break;
                                case DeadPlayer.CustomDeathReason.Kill:
                                    str4 = "DeadReasonKill".Translate() + Helpers.cs(c5, deadPlayer.killerIfExisting.Data.PlayerName);
                                    break;
                                case DeadPlayer.CustomDeathReason.Disconnect:
                                    str4 = "DeadReasonDisconnect".Translate();
                                    break;
                                case DeadPlayer.CustomDeathReason.Guess:
                                    {
                                        bool flag22 = deadPlayer.killerIfExisting.Data.PlayerName == p.Data.PlayerName;
                                        if (flag22)
                                        {
                                            str4 = "DeadReasonFailGuess".Translate();
                                        }
                                        else
                                        {
                                            str4 = "DeadReasonGuess" + Helpers.cs(c5, deadPlayer.killerIfExisting.Data.PlayerName) + " 猜测";
                                        }
                                        break;
                                    }
                                case DeadPlayer.CustomDeathReason.Shift:
                                    str4 = " - " + Helpers.cs(Color.yellow, "DeadReasonthief".Translate()) + " " + Helpers.cs(c5, deadPlayer.killerIfExisting.Data.PlayerName);
                                    break;
                                case DeadPlayer.CustomDeathReason.LawyerSuicide:
                                    str4 = " - " + Helpers.cs(Lawyer.color, "DeadReasonLawyerSuicide".Translate());
                                    break;
                                case DeadPlayer.CustomDeathReason.LoverSuicide:
                                    str4 = " - " + Helpers.cs(Lovers.color, "DeadReasonLoverSuicide".Translate());
                                    break;
                                case DeadPlayer.CustomDeathReason.WitchExile:
                                    str4 = " - " + Helpers.cs(Witch.color, "DeadReasonWitchExile1".Translate()) + "DeadReasonWitchExile2".Translate() + Helpers.cs(c5, deadPlayer.killerIfExisting.Data.PlayerName);
                                    break;
                                case DeadPlayer.CustomDeathReason.Bomb:
                                    str4 = "DeadReasonBomb".Translate() + Helpers.cs(c5, deadPlayer.killerIfExisting.Data.PlayerName);
                                    break;
                                case DeadPlayer.CustomDeathReason.Arson:
                                    str4 = "DeadReasonArson".Translate() + Helpers.cs(c5, deadPlayer.killerIfExisting.Data.PlayerName);
                                    break;
                            }
                            text += str4;
                        }
                    }
                }
            }
            return text;
        }

        // Token: 0x060000DC RID: 220 RVA: 0x00013308 File Offset: 0x00011508
        [DebuggerStepThrough]
        public static async Task loadReadme()
 {
     if (ReadmePage == "")
     {
         System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
                System.Net.Http.HttpResponseMessage response = await client.GetAsync("https://raw.githubusercontent.com/TheOtherRolesAU/TheOtherRoles/main/README.md");
         response.EnsureSuccessStatusCode();
         string httpres = await response.Content.ReadAsStringAsync();
         ReadmePage = httpres;
     }
 }

        // Token: 0x060000DD RID: 221 RVA: 0x00013348 File Offset: 0x00011548
        public static string GetRoleDescription(RoleInfo roleInfo)
        {
            while (RoleInfo.ReadmePage == "")
            {
            }
            int startIndex = RoleInfo.ReadmePage.IndexOf("## " + roleInfo.name);
            int length = RoleInfo.ReadmePage.Substring(startIndex).IndexOf("GameReason".Translate());
            return RoleInfo.ReadmePage.Substring(startIndex, length);
        }

        // Token: 0x04000283 RID: 643
        public Color color;

        // Token: 0x04000284 RID: 644
        public string name;

        // Token: 0x04000285 RID: 645
        public string introDescription;

        // Token: 0x04000286 RID: 646
        public string shortDescription;

        // Token: 0x04000287 RID: 647
        public RoleId roleId;

        // Token: 0x04000288 RID: 648
        public bool isNeutral;

        // Token: 0x04000289 RID: 649
        public bool isModifier;

        // Token: 0x0400028A RID: 650
        public static RoleInfo jester = new RoleInfo("Jester", TheOtherRoles.Jester.color, "JesterIntroDesc", "JesterShortDesc", RoleId.Jester, true, false);

        // Token: 0x0400028B RID: 651
        public static RoleInfo mayor = new RoleInfo("Mayor", TheOtherRoles.Mayor.color, "MayorIntroDesc", "MayorShortDesc", RoleId.Mayor, false, false);

        // Token: 0x0400028C RID: 652
        public static RoleInfo portalmaker = new RoleInfo("Portalmaker", TheOtherRoles.Portalmaker.color, "PortalmakerIntroDesc", "PortalmakerShortDesc", RoleId.Portalmaker, false, false);

        // Token: 0x0400028D RID: 653
        public static RoleInfo engineer = new RoleInfo("Engineer", TheOtherRoles.Engineer.color, "EngineerIntroDesc", "EngineerShortDesc", RoleId.Engineer, false, false);

        // Token: 0x0400028E RID: 654
        public static RoleInfo sheriff = new RoleInfo("Sheriff", TheOtherRoles.Sheriff.color, "SheriffIntroDesc", "SheriffShortDesc", RoleId.Sheriff, false, false);

        // Token: 0x0400028F RID: 655
        public static RoleInfo deputy = new RoleInfo("Deputy", TheOtherRoles.Sheriff.color, "DeputyIntroDesc", "DeputyShortDesc", RoleId.Deputy, false, false);

        // Token: 0x04000290 RID: 656
        public static RoleInfo lighter = new RoleInfo("Lighter", TheOtherRoles.Lighter.color, "LighterIntroDesc", "LighterShortDesc", RoleId.Lighter, false, false);

        // Token: 0x04000291 RID: 657
        public static RoleInfo godfather = new RoleInfo("godfather", TheOtherRoles.Godfather.color, "godfatherntroDesc", "godfatherShortDesc", RoleId.Godfather, false, false);

        // Token: 0x04000292 RID: 658
        public static RoleInfo mafioso = new RoleInfo("mafioso", TheOtherRoles.Mafioso.color, "mafiosoIntroDesc", "mafiosoShortDesc", RoleId.Mafioso, false, false);

        // Token: 0x04000293 RID: 659
        public static RoleInfo janitor = new RoleInfo("janitor", TheOtherRoles.Janitor.color, "janitorIntroDesc", "janitorShortDesc", RoleId.Janitor, false, false);

        // Token: 0x04000294 RID: 660
        public static RoleInfo morphling = new RoleInfo("Morphling", Morphling.color, "MorphlingIntroDesc", "MorphlingShortDesc", RoleId.Morphling, false, false);

        // Token: 0x04000295 RID: 661
        public static RoleInfo camouflager = new RoleInfo("Camouflager", Camouflager.color, "CamouflagerIntroDesc", "CamouflagerShortDesc", RoleId.Camouflager, false, false);

        // Token: 0x04000296 RID: 662
        public static RoleInfo vampire = new RoleInfo("Vampire", Vampire.color, "VampireIntroDesc", "VampireShortDesc", RoleId.Vampire, false, false);

        // Token: 0x04000297 RID: 663
        public static RoleInfo eraser = new RoleInfo("Eraser", Eraser.color, "EraserIntroDesc", "EraserShortDesc", RoleId.Eraser, false, false);

        // Token: 0x04000298 RID: 664
        public static RoleInfo trickster = new RoleInfo("Trickster", Trickster.color, "TricksterIntroDesc", "TricksterShortDesc", RoleId.Trickster, false, false);

        // Token: 0x04000299 RID: 665
        public static RoleInfo cleaner = new RoleInfo("Cleaner", Cleaner.color, "CleanerIntroDesc", "CleanerShortDesc", RoleId.Cleaner, false, false);

        // Token: 0x0400029A RID: 666
        public static RoleInfo warlock = new RoleInfo("Warlock", Warlock.color, "WarlockIntroDesc", "WarlockShortDesc", RoleId.Warlock, false, false);

        // Token: 0x0400029B RID: 667
        public static RoleInfo bountyHunter = new RoleInfo("BountyHunter", BountyHunter.color, "BountyHunterIntroDesc", "BountyHunterShortDesc", RoleId.BountyHunter, false, false);

        // Token: 0x0400029C RID: 668
        public static RoleInfo detective = new RoleInfo("Detective", TheOtherRoles.Detective.color, "DetectiveIntroDesc", "DetectiveShortDesc", RoleId.Detective, false, false);

        // Token: 0x0400029D RID: 669
        public static RoleInfo timeMaster = new RoleInfo("TimeMaster", TimeMaster.color, "TimeMasterIntroDesc", "TimeMasterShortDesc", RoleId.TimeMaster, false, false);

        // Token: 0x0400029E RID: 670
        public static RoleInfo medic = new RoleInfo("Medic", Medic.color, "MedicIntroDesc", "MedicShortDesc", RoleId.Medic, false, false);

        // Token: 0x0400029F RID: 671
        public static RoleInfo swapper = new RoleInfo("Swapper", Swapper.color, "SwapperIntroDesc", "SwapperShortDesc", RoleId.Swapper, false, false);

        // Token: 0x040002A0 RID: 672
        public static RoleInfo seer = new RoleInfo("Seer", Seer.color, "SeerIntroDesc", "SeerShortDesc", RoleId.Seer, false, false);

        // Token: 0x040002A1 RID: 673
        public static RoleInfo hacker = new RoleInfo("Hacker", Hacker.color, "HackerIntroDesc", "HackerShortDesc", RoleId.Hacker, false, false);

        // Token: 0x040002A2 RID: 674
        public static RoleInfo tracker = new RoleInfo("Tracker", Tracker.color, "TrackerIntroDesc", "TrackerShortDesc", RoleId.Tracker, false, false);

        // Token: 0x040002A3 RID: 675
        public static RoleInfo snitch = new RoleInfo("Snitch", Snitch.color, "SnitchIntroDesc", "SnitchShortDesc", RoleId.Snitch, false, false);

        // Token: 0x040002A4 RID: 676
        public static RoleInfo investigator = new RoleInfo("Prophet", Prophet.color, "ProphetIntroDesc", "ProphetShortDesc", RoleId.Prophet, false, false);

        // Token: 0x040002A5 RID: 677
        public static RoleInfo jackal = new RoleInfo("Jackal", Jackal.color, "JackalIntroDesc", "JackalShortDesc", RoleId.Jackal, true, false);

        // Token: 0x040002A6 RID: 678
        public static RoleInfo sidekick = new RoleInfo("Sidekick", Sidekick.color, "SidekickIntroDesc", "SidekickShortDesc", RoleId.Sidekick, true, false);

        // Token: 0x040002A7 RID: 679
        public static RoleInfo spy = new RoleInfo("Spy", Spy.color, "SpyIntroDesc", "SpyShortDesc", RoleId.Spy, false, false);

        // Token: 0x040002A8 RID: 680
        public static RoleInfo securityGuard = new RoleInfo("SecurityGuard", SecurityGuard.color, "SecurityGuardIntroDesc", "SecurityGuardShortDesc", RoleId.SecurityGuard, false, false);

        // Token: 0x040002A9 RID: 681
        public static RoleInfo arsonist = new RoleInfo("Arsonist", Arsonist.color, "ArsonistIntroDesc", "ArsonistShortDesc", RoleId.Arsonist, true, false);

        // Token: 0x040002AA RID: 682
        public static RoleInfo goodGuesser = new RoleInfo("Vigilante", Guesser.color, "VigilanteIntroDesc", "VigilanteShortDesc", RoleId.NiceGuesser, false, false);

        // Token: 0x040002AB RID: 683
        public static RoleInfo badGuesser = new RoleInfo("Assassin", Palette.ImpostorRed, "AssassinIntroDesc", "AssassinShortDesc", RoleId.EvilGuesser, false, false);

        // Token: 0x040002AC RID: 684
        public static RoleInfo vulture = new RoleInfo("Vulture", Vulture.color, "VultureIntroDesc", "VultureShortDesc", RoleId.Vulture, true, false);

        // Token: 0x040002AD RID: 685
        public static RoleInfo medium = new RoleInfo("Medium", Medium.color, "MediumIntroDesc", "MediumShortDesc", RoleId.Medium, false, false);

        // Token: 0x040002AE RID: 686
        public static RoleInfo trapper = new RoleInfo("Trapper", Trapper.color, "TrapperIntroDesc", "TrapperShortDesc", RoleId.Trapper, false, false);

        // Token: 0x040002AF RID: 687
        public static RoleInfo lawyer = new RoleInfo("Lawyer", Lawyer.color, "LawyerIntroDesc", "LawyerShortDesc", RoleId.Lawyer, true, false);

        // Token: 0x040002B0 RID: 688
        public static RoleInfo prosecutor = new RoleInfo("Prosecutor", Lawyer.color, "ProsecutorIntroDesc", "ProsecutorShortDesc", RoleId.Prosecutor, true, false);

        // Token: 0x040002B1 RID: 689
        public static RoleInfo pursuer = new RoleInfo("Pursuer", Pursuer.color, "PursuerIntroDesc", "PursuerShortDesc", RoleId.Pursuer, false, false);

        // Token: 0x040002B2 RID: 690
        public static RoleInfo impostor = new RoleInfo("Impostor", Palette.ImpostorRed, Helpers.cs(Palette.ImpostorRed, "ImpostorIntroDesc"), "ImpostorShortDesc", RoleId.Impostor, false, false);

        // Token: 0x040002B3 RID: 691
        public static RoleInfo crewmate = new RoleInfo("Crewmate", Color.white, "CrewmateIntroDesc", "CrewmateShortDesc", RoleId.Crewmate, false, false);

        // Token: 0x040002B4 RID: 692
        public static RoleInfo witch = new RoleInfo("Witch", Witch.color, "WitchIntroDesc", "WitchShortDesc", RoleId.Witch, false, false);

        // Token: 0x040002B5 RID: 693
        public static RoleInfo ninja = new RoleInfo("Ninja", Ninja.color, "NinjaIntroDesc", "NinjaShortDesc", RoleId.Ninja, false, false);

        // Token: 0x040002B6 RID: 694
        public static RoleInfo thief = new RoleInfo("Thief", Thief.color, "ThiefIntroDesc", "ThiefShortDesc", RoleId.Thief, true, false);

        // Token: 0x040002B7 RID: 695
        public static RoleInfo bomber = new RoleInfo("Bomber", Bomber.color, "BomberIntroDesc", "BomberShortDesc", RoleId.Bomber, false, false);

        // Token: 0x040002B8 RID: 696
        public static RoleInfo yoyo = new RoleInfo("Yoyo", Yoyo.color, "YoyoIntroDesc", "YoyoShortDesc", RoleId.Yoyo, false, false);

        // Token: 0x040002B9 RID: 697
        public static RoleInfo fraudster = new RoleInfo("Fraudster", Fraudster.color, "FraudsterIntroDesc", "FraudsterShortDesc", RoleId.Fraudster, false, false);

        // Token: 0x040002BA RID: 698
        public static RoleInfo hunter = new RoleInfo("Hunter", Palette.ImpostorRed, Helpers.cs(Palette.ImpostorRed, "HunterIntroDesc"), "HunterShortDesc", RoleId.Impostor, false, false);

        // Token: 0x040002BB RID: 699
        public static RoleInfo hunted = new RoleInfo("Hunted", Color.white, "HuntedIntroDesc", "HuntedShortDesc", RoleId.Crewmate, false, false);

        // Token: 0x040002BC RID: 700
        public static RoleInfo prop = new RoleInfo("Prop", Color.white, "PropIntroDesc", "PropShortDesc", RoleId.Crewmate, false, false);

        // Token: 0x040002BD RID: 701
        public static RoleInfo bloody = new RoleInfo("Bloody", Color.yellow, "BloodyIntroDesc", "BloodyShortDesc", RoleId.Bloody, false, true);

        // Token: 0x040002BE RID: 702
        public static RoleInfo antiTeleport = new RoleInfo("AntiTeleport", Color.yellow, "AntiTeleportIntroDesc", "AntiTeleportShortDesc", RoleId.AntiTeleport, false, true);

        // Token: 0x040002BF RID: 703
        public static RoleInfo tiebreaker = new RoleInfo("TieBreaker", Color.yellow, "TieBreakerIntroDesc", "TieBreakerShortDesc", RoleId.Tiebreaker, false, true);

        // Token: 0x040002C0 RID: 704
        public static RoleInfo bait = new RoleInfo("Bait", Color.yellow, "BaitIntroDesc", "BaitShortDesc", RoleId.Bait, false, true);

        // Token: 0x040002C1 RID: 705
        public static RoleInfo sunglasses = new RoleInfo("Sunglasses", Color.yellow, "SunglassesIntroDesc", "SunglassesShortDesc", RoleId.Sunglasses, false, true);

        // Token: 0x040002C2 RID: 706
        public static RoleInfo lighterln = new RoleInfo("Torch", Color.yellow, "TorchIntroDesc", "TorchShortDesc", RoleId.Lighterln, false, true);

        // Token: 0x040002C3 RID: 707
        public static RoleInfo lover = new RoleInfo("Lover", Lovers.color, "LoverIntroDesc", "LoverShortDesc", RoleId.Lover, false, true);

        // Token: 0x040002C4 RID: 708
        public static RoleInfo mini = new RoleInfo("Mini", Color.yellow, "MiniIntroDesc", "MiniShortDesc", RoleId.Mini, false, true);

        // Token: 0x040002C5 RID: 709
        public static RoleInfo vip = new RoleInfo("Vip", Color.yellow, "VipIntroDesc", "VipShortDesc", RoleId.Vip, false, true);

        // Token: 0x040002C6 RID: 710
        public static RoleInfo invert = new RoleInfo("醉鬼", Color.yellow, "InvertIntroDesc", "InvertShortDesc", RoleId.Invert, false, true);

        // Token: 0x040002C7 RID: 711
        public static RoleInfo chameleon = new RoleInfo("Chameleon", Color.yellow, "ChameleonIntroDesc", "ChameleonShortDesc", RoleId.Chameleon, false, true);

        // Token: 0x040002C8 RID: 712
        public static RoleInfo shifter = new RoleInfo("Shifter", Color.yellow, "ShifterIntroDesc", "ShifterShortDesc", RoleId.Shifter, false, true);

        // Token: 0x040002C9 RID: 713
        public static List<RoleInfo> allRoleInfos = new List<RoleInfo>
        {
            RoleInfo.impostor,
            RoleInfo.godfather,
            RoleInfo.mafioso,
            RoleInfo.janitor,
            RoleInfo.morphling,
            RoleInfo.camouflager,
            RoleInfo.vampire,
            RoleInfo.eraser,
            RoleInfo.trickster,
            RoleInfo.cleaner,
            RoleInfo.warlock,
            RoleInfo.bountyHunter,
            RoleInfo.witch,
            RoleInfo.ninja,
            RoleInfo.bomber,
            RoleInfo.yoyo,
            RoleInfo.goodGuesser,
            RoleInfo.badGuesser,
            RoleInfo.lover,
            RoleInfo.jester,
            RoleInfo.arsonist,
            RoleInfo.jackal,
            RoleInfo.sidekick,
            RoleInfo.vulture,
            RoleInfo.pursuer,
            RoleInfo.lawyer,
            RoleInfo.thief,
            RoleInfo.prosecutor,
            RoleInfo.crewmate,
            RoleInfo.mayor,
            RoleInfo.portalmaker,
            RoleInfo.engineer,
            RoleInfo.sheriff,
            RoleInfo.deputy,
            RoleInfo.lighter,
            RoleInfo.detective,
            RoleInfo.timeMaster,
            RoleInfo.medic,
            RoleInfo.swapper,
            RoleInfo.seer,
            RoleInfo.hacker,
            RoleInfo.tracker,
            RoleInfo.snitch,
            RoleInfo.spy,
            RoleInfo.securityGuard,
            RoleInfo.bait,
            RoleInfo.medium,
            RoleInfo.trapper,
            RoleInfo.bloody,
            RoleInfo.antiTeleport,
            RoleInfo.tiebreaker,
            RoleInfo.sunglasses,
            RoleInfo.lighterln,
            RoleInfo.mini,
            RoleInfo.vip,
            RoleInfo.invert,
            RoleInfo.chameleon,
            RoleInfo.shifter
        };

        // Token: 0x040002CA RID: 714
        private static string ReadmePage = "";
    }
}
