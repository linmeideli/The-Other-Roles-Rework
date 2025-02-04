using System;
using System.Collections.Generic;
using System.Linq;
using AmongUs.Data;
using AmongUs.Data.Player;
using Assets.InnerNet;
using HarmonyLib;

namespace TheOtherRoles.Patch;

// ##https://github.com/Yumenopai/TownOfHost_Y
public class ModNews
{
    public int Number;
    public int BeforeNumber;
    public string Title;
    public string SubTitle;
    public string ShortTitle;
    public string Text;
    public string Date;

    public Announcement ToAnnouncement()
    {
        var result = new Announcement
        {
            Number = Number,
            Title = Title,
            SubTitle = SubTitle,
            ShortTitle = ShortTitle,
            Text = Text,
            Language = (uint)DataManager.Settings.Language.CurrentLanguage,
            Date = Date,
            Id = "ModNews"
        };

        return result;
    }
}
[HarmonyPatch]
public class ModNewsHistory
{
    public static List<ModNews> AllModNews = new();
    public static void Init()
    {
        // 创建新公告时，不能删除旧公告   
        {
            
            var news = new ModNews
            {
                Number = 100003,
                Title = "TheOtherRolesRework-v1.0.3",
                SubTitle = "★★★★Language，Roles★★★★",
                ShortTitle = "★TORR v1.0.3★",
                Text = "<size=100%>"+ "\n【声明】-本模组不隶属于 Among Us 或 Innersloth LLC 其中包含的内容未得到 Innersloth LLC 的认可或以其他方式赞助 此处包含的部分材料是 Innersloth LLC的财产 \r\n©Innersloth\nDisclaimer - This mod is not affiliated with Among Us or Innersloth LLC and the content contained therein is not endorsed by or otherwise sponsored by Innersloth LLC Some of the materials contained herein are the property of Innersloth LLC \r\n©Innersloth\n\n"
                + "好久不见啦！\nWelcome Crewmates!</size>\n\n<size=100%>我们适配到了Among Us v11.26s，BepInEx6.0.0-729，Reactor 2.3.1 基于TheOtherRoles-v4.6.0\nUpdate toAmong Us v11.26s，BepInEx6.0.0-729，Reactor 2.3.1 Base onTheOtherRoles-v4.6.0</size>\n\n"
                    + "\n新职业-预言家!\n<size=75%>预言家可以通过调查来查看他人阵营</size>\nNew Roles-Prophet!\n<size=75%>Prophet can search others type</size>\r"
                    + "\n我们跟进了TheOtherRoles的更新，添加了按Shift进入大厅的功能\nWe followed up with an update to TheOtherRoles to add the ability to press Shift to enter the lobby\n\n"
                    + "\n此外，我们添加了语言系统，保证了其他国家的人民可以基本正常的游玩我们的Mod\nIn addition, we've added a language system to ensure that people from other countries can play our mods as usual\n\n"
                    + "\n最后，我们减小了dll文件的体积，这一点要感谢TheOtherRolesCE的作者Fangkua\nFinally, we have reduced the size of the dll file thanks to Fangkuai, the author of TheOtherRolesCE\n\n"
                    + "\n尽情游玩吧！\nHave A Good Time!\n\n"
                    +"\nELinmei-XtremeWave"
                    ,
                Date = "2025-2-5T00:00:00Z"
            };
            AllModNews.Add(news);
        }
    }

    [HarmonyPatch(typeof(PlayerAnnouncementData), nameof(PlayerAnnouncementData.SetAnnouncements)), HarmonyPrefix]
    public static bool SetModAnnouncements(PlayerAnnouncementData __instance, [HarmonyArgument(0)] ref Il2CppReferenceArray<Announcement> aRange)
    {
        if (AllModNews.Count < 1)
        {
            Init();
            AllModNews.Sort((a1, a2) => { return DateTime.Compare(DateTime.Parse(a2.Date), DateTime.Parse(a1.Date)); });
        }

        List<Announcement> FinalAllNews = new();
        AllModNews.Do(n => FinalAllNews.Add(n.ToAnnouncement()));
        foreach (var news in aRange)
        {
            if (!AllModNews.Any(x => x.Number == news.Number))
                FinalAllNews.Add(news);
        }
        FinalAllNews.Sort((a1, a2) => { return DateTime.Compare(DateTime.Parse(a2.Date), DateTime.Parse(a1.Date)); });

        aRange = new(FinalAllNews.Count);
        for (int i = 0; i < FinalAllNews.Count; i++)
            aRange[i] = FinalAllNews[i];

        return true;
    }
}