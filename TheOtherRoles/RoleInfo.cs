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
            this.name = name;
            this.introDescription = introDescription;
            this.shortDescription = shortDescription;
            this.roleId = roleId;
            this.isNeutral = isNeutral;
            this.isModifier = isModifier;
        }

        public static RoleInfo jester = new RoleInfo("小丑", Jester.color, "被票出去", "被投出以获得胜利", RoleId.Jester, true);
        public static RoleInfo mayor = new RoleInfo("市长", Mayor.color, "你可以票人两次", "你可以票人两次", RoleId.Mayor);
        public static RoleInfo portalmaker = new RoleInfo("守门人", Portalmaker.color, "你可以创建传送门", "你可以创建传送门", RoleId.Portalmaker);
        public static RoleInfo engineer = new RoleInfo("工程师", Engineer.color, "观察<color=#FF0000>伪装者</color>并修理破坏", "修理飞船", RoleId.Engineer);
        public static RoleInfo sheriff = new RoleInfo("警长", Sheriff.color, "击毙<color=#FF1919FF>伪装者</color>", "击毙伪装者", RoleId.Sheriff);
        public static RoleInfo deputy = new RoleInfo("捕快", Sheriff.color, "拷住<color=#FF1919FF>伪装者</color>", "拷住伪装者", RoleId.Deputy);
        public static RoleInfo lighter = new RoleInfo("执灯人", Lighter.color, "光明倾向着你", "你的视野会是明亮的", RoleId.Lighter);
        public static RoleInfo godfather = new RoleInfo("教父", Godfather.color, "击杀船员", "击杀船员", RoleId.Godfather);
        public static RoleInfo mafioso = new RoleInfo("小弟", Mafioso.color, "跟随着<color=#FF1919FF>教父</color>去击杀所有船员", "击杀船员", RoleId.Mafioso);
        public static RoleInfo janitor = new RoleInfo("清洁工", Janitor.color, "跟随着<color=#FF1919FF>教父</color>清理尸体", "清理尸体", RoleId.Janitor);
        public static RoleInfo morphling = new RoleInfo("化形者", Morphling.color, "改变你的长相，以免被人发现", "改变你的长相", RoleId.Morphling);
        public static RoleInfo camouflager = new RoleInfo("隐蔽者", Camouflager.color, "隐蔽后击杀船员", "从人群中来", RoleId.Camouflager);
        public static RoleInfo vampire = new RoleInfo("吸血鬼", Vampire.color, "吸干他们的血", "吸血杀人", RoleId.Vampire);
        public static RoleInfo eraser = new RoleInfo("抹除者", Eraser.color, "击杀船员并抹去他们的职业", "抹除职业", RoleId.Eraser);
        public static RoleInfo trickster = new RoleInfo("骗术师", Trickster.color, "利用惊喜盒把船员们击杀", "恐吓船员", RoleId.Trickster);
        public static RoleInfo cleaner = new RoleInfo("清理者", Cleaner.color, "击杀所有人并且隐藏尸体", "隐藏尸体", RoleId.Cleaner);
        public static RoleInfo warlock = new RoleInfo("术士", Warlock.color, "诅咒船员", "诅咒船员", RoleId.Warlock);
        public static RoleInfo bountyHunter = new RoleInfo("赏金猎人", BountyHunter.color, "击杀目标以减少冷却", $"击杀目标", RoleId.BountyHunter);
        public static RoleInfo detective = new RoleInfo("侦探", Detective.color, "寻找<color=#FF1919FF>伪装者</color> 的足迹", "观察足迹", RoleId.Detective);
        public static RoleInfo timeMaster = new RoleInfo("时间之主", TimeMaster.color, "用时间之盾保全自己", "合理运用时间之盾", RoleId.TimeMaster);
        public static RoleInfo medic = new RoleInfo("医生", Medic.color, "给予船员保护", "保护船员", RoleId.Medic);
        public static RoleInfo swapper = new RoleInfo("换票师", Swapper.color, "把票交换到<color=#FF1919FF>伪装者</color>头上", "交换投票", RoleId.Swapper);
        public static RoleInfo seer = new RoleInfo("灵媒", Seer.color, "你与灵界互通", "你可以得知玩家的死亡", RoleId.Seer);
        public static RoleInfo hacker = new RoleInfo("黑客", Hacker.color, "黑进管理地图从而发现<color=#FF1919FF>伪装者</color>", "黑入伪装者", RoleId.Hacker);
        public static RoleInfo tracker = new RoleInfo("追踪者", Tracker.color, "追踪<color=#FF1919FF>伪装者</color>", "追踪伪装者", RoleId.Tracker);
        public static RoleInfo snitch = new RoleInfo("告密者", Snitch.color, "完成任务以发现<color=#FF1919FF>伪装者</color>", "完成所有任务", RoleId.Snitch);
        public static RoleInfo jackal = new RoleInfo("豺狼", Jackal.color, "杀死 船员 和 <color=#FF1919FF>伪装者</color>", "击杀所有人", RoleId.Jackal, true);
        public static RoleInfo sidekick = new RoleInfo("跟班", Sidekick.color, "帮助豺狼杀死所有人", "帮助豺狼杀死所有人", RoleId.Sidekick, true);
        public static RoleInfo spy = new RoleInfo("卧底", Spy.color, "在伪装者眼中你也是<color=#FF1919FF>伪装者</color>", "潜伏入伪装者", RoleId.Spy);
        public static RoleInfo securityGuard = new RoleInfo("保安", SecurityGuard.color, "封锁管道 放置摄像头", "封锁管道并放置摄像头", RoleId.SecurityGuard);
        public static RoleInfo arsonist = new RoleInfo("纵火犯", Arsonist.color, "火上浇油", "对所有人涂油后燃烧", RoleId.Arsonist, true);
        public static RoleInfo goodGuesser = new RoleInfo("正义的赌怪", Guesser.color, "生命日是豪赌", "猜测<color=#FF1919FF>伪装者</color>身份", RoleId.NiceGuesser);
        public static RoleInfo badGuesser = new RoleInfo("邪恶的赌怪", Palette.ImpostorRed, "生命日是豪赌", "猜测船员身份", RoleId.EvilGuesser);
        public static RoleInfo vulture = new RoleInfo("秃鹫", Vulture.color, "吞食尸体到一定数量获胜", "吞食尸体", RoleId.Vulture, true);
        public static RoleInfo medium = new RoleInfo("通灵师", Medium.color, "对灵魂提问", "提问灵魂", RoleId.Medium);
        public static RoleInfo trapper = new RoleInfo("陷阱师", Trapper.color, "设置陷阱从而找到<color=#FF0000>伪装者</color>", "设置陷阱", RoleId.Trapper);
        public static RoleInfo lawyer = new RoleInfo("律师", Lawyer.color, "辩论你的客户", "辩论你的客户", RoleId.Lawyer, true);
        public static RoleInfo prosecutor = new RoleInfo("检察官", Lawyer.color, "票出你的目标", "票出你的目标", RoleId.Prosecutor, true);
        public static RoleInfo pursuer = new RoleInfo("起诉人", Pursuer.color, "你很幸运，豺狼和伪装者是恋人", "使用空包弹抵御伪装者", RoleId.Pursuer);
        public static RoleInfo impostor = new RoleInfo("伪装者", Palette.ImpostorRed, Helpers.cs(Palette.ImpostorRed, "破坏并击杀船员"), "破坏并击杀船员", RoleId.Impostor);
        public static RoleInfo crewmate = new RoleInfo("船员", Color.white, "找出伪装者", "找出伪装者", RoleId.Crewmate);
        public static RoleInfo witch = new RoleInfo("女巫", Witch.color, "诅咒船员", "诅咒船员", RoleId.Witch);
        public static RoleInfo ninja = new RoleInfo("忍者", Ninja.color, "刺杀船员", "刺杀船员", RoleId.Ninja);
        public static RoleInfo thief = new RoleInfo("身份窃贼", Thief.color, "杀死他，成为他", "击杀而获取职业", RoleId.Thief, true);
        public static RoleInfo bomber = new RoleInfo("炸弹狂", Bomber.color, "我炸死你！", "炸死船员", RoleId.Bomber);
        public static RoleInfo yoyo = new RoleInfo("回旋者", Yoyo.color, "标记后一段时间返回", "标记一处地点", RoleId.Yoyo);

        public static RoleInfo hunter = new RoleInfo("猎人", Palette.ImpostorRed, Helpers.cs(Palette.ImpostorRed, "寻找并杀死所有人"), "寻找并杀死所有人", RoleId.Impostor);
        public static RoleInfo hunted = new RoleInfo("躲藏者", Color.white, "不择手段的活下去", "隐藏起来", RoleId.Crewmate);

        public static RoleInfo prop = new RoleInfo("魔术师", Color.white, "伪装成一个物体并生存下来", "伪装成一个物体并生存下来", RoleId.Crewmate);



        // Modifier
        public static RoleInfo bloody = new RoleInfo("血泊者", Color.yellow, "杀死你的凶手会拖着血液", "杀死你的凶手会拖着血液", RoleId.Bloody, false, true);
        public static RoleInfo antiTeleport = new RoleInfo("通讯兵", Color.yellow, "你不会被传送", "你不会被传送", RoleId.AntiTeleport, false, true);
        public static RoleInfo tiebreaker = new RoleInfo("破平者", Color.yellow, "你打破了平局", "你参与的投票不会有平局", RoleId.Tiebreaker, false, true);
        public static RoleInfo bait = new RoleInfo("诱饵", Color.yellow, "钓鱼执法", "凶手会自动报告你的尸体", RoleId.Bait, false, true);
        public static RoleInfo sunglasses = new RoleInfo("失明者", Color.yellow, "你失明了", "你的视野很小", RoleId.Sunglasses, false, true);
        public static RoleInfo lighterln = new RoleInfo("光明者", Color.yellow, "普罗米修斯", "你的视野很大", RoleId.Lighterln, false, true);
        public static RoleInfo lover = new RoleInfo("恋人", Lovers.color, "$你坠入了爱河与", $"你爱上了", RoleId.Lover, false, true);
        public static RoleInfo mini = new RoleInfo("迷你船员", Color.yellow, "没人能在你成年后杀了你", "没人能在你成年后杀了你", RoleId.Mini, false, true);
        public static RoleInfo vip = new RoleInfo("会员", Color.yellow, "你有特权", "每人都会在你死时得知你的阵营", RoleId.Vip, false, true);
        public static RoleInfo invert = new RoleInfo("醉鬼", Color.yellow, "你喝醉了", "你的移动方向是反的", RoleId.Invert, false, true);
        public static RoleInfo chameleon = new RoleInfo("变色龙", Color.yellow, "你停止移动的时候会隐身", "你停止移动的时候会隐身", RoleId.Chameleon, false, true);
        public static RoleInfo shifter = new RoleInfo("交换师", Color.yellow, "交换职业", "换走你的职业", RoleId.Shifter, false, true);


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
                if (BountyHunter.bountyHunterShowCooldownForGhosts)
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
