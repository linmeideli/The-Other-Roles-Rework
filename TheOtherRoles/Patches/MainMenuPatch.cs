using System;
using AmongUs.Data;
using Assets.InnerNet;
using HarmonyLib;
using Il2CppSystem.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.UI.Button;
using Object = UnityEngine.Object;

namespace TheOtherRoles.Modules;

[HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.Start))]
public class MainMenuPatch
{
    private static bool horseButtonState = TORMapOptions.enableHorseMode;

    //private static Sprite horseModeOffSprite = null;
    //private static Sprite horseModeOnSprite = null;
    private static AnnouncementPopUp popUp;

    private static void Prefix(MainMenuManager __instance)
    {
        // Force Reload of SoundEffectHolder
        SoundEffectsManager.Load();

        var template = GameObject.Find("ExitGameButton");
        var template2 = GameObject.Find("CreditsButton");
        if (template == null || template2 == null) return;
        template.transform.localScale = new Vector3(0.42f, 0.84f, 0.84f);
        template.GetComponent<AspectPosition>().anchorPoint = new Vector2(0.625f, 0.5f);
        template.transform.FindChild("FontPlacer").transform.localScale = new Vector3(1.8f, 0.9f, 0.9f);
        template.transform.FindChild("FontPlacer").transform.localPosition = new Vector3(-1.1f, 0f, 0f);

        template2.transform.localScale = new Vector3(0.42f, 0.84f, 0.84f);
        template2.GetComponent<AspectPosition>().anchorPoint = new Vector2(0.378f, 0.5f);
        template2.transform.FindChild("FontPlacer").transform.localScale = new Vector3(1.8f, 0.9f, 0.9f);
        template2.transform.FindChild("FontPlacer").transform.localPosition = new Vector3(-1.1f, 0f, 0f);


        var buttonDiscord = Object.Instantiate(template, template.transform.parent);
        buttonDiscord.transform.localScale = new Vector3(0.42f, 0.84f, 0.84f);
        buttonDiscord.GetComponent<AspectPosition>().anchorPoint = new Vector2(0.542f, 0.5f);

        var textDiscord = buttonDiscord.transform.GetComponentInChildren<TMP_Text>();
        __instance.StartCoroutine(Effects.Lerp(0.5f, new Action<float>(p => { textDiscord.SetText("Discord"); })));
        var passiveButtonDiscord = buttonDiscord.GetComponent<PassiveButton>();

        passiveButtonDiscord.OnClick = new ButtonClickedEvent();
        passiveButtonDiscord.OnClick.AddListener((Action)(() => Application.OpenURL("https://discord.gg/77RkMJHWsM")));


        // TOR credits button
        if (template == null) return;
        var creditsButton = Object.Instantiate(template, template.transform.parent);

        creditsButton.transform.localScale = new Vector3(0.42f, 0.84f, 0.84f);
        creditsButton.GetComponent<AspectPosition>().anchorPoint = new Vector2(0.462f, 0.5f);

        var textCreditsButton = creditsButton.transform.GetComponentInChildren<TMP_Text>();
        __instance.StartCoroutine(Effects.Lerp(0.5f,
            new Action<float>(p => { textCreditsButton.SetText("Credits"); })));
        var passiveCreditsButton = creditsButton.GetComponent<PassiveButton>();

        passiveCreditsButton.OnClick = new ButtonClickedEvent();

        passiveCreditsButton.OnClick.AddListener((Action)delegate
        {
            // do stuff
            if (popUp != null) Object.Destroy(popUp);
            var popUpTemplate = Object.FindObjectOfType<AnnouncementPopUp>(true);
            if (popUpTemplate == null)
            {
                TheOtherRolesPlugin.Logger.LogError("couldnt show credits, popUp is null");
                return;
            }

            popUp = Object.Instantiate(popUpTemplate);

            popUp.gameObject.SetActive(true);

            Announcement creditsAnnouncement = new()
            {
                Id = "torrCredits",
                Language = 0,
                Number = 500,
                Title = "creditsAnnouncementTittle".Translate(),
                ShortTitle = "creditsAnnouncementShortTittle".Translate(),
                SubTitle = "creditsAnnouncementSubTitle".Translate(),
                PinState = false,
                Date = "05.01.2025",
                Text = "mianMenuManagerCreditsMenuText".Translate()
            };
            __instance.StartCoroutine(Effects.Lerp(0.1f, new Action<float>(p =>
            {
                if (p == 1)
                {
                    var backup = DataManager.Player.Announcements.allAnnouncements;
                    DataManager.Player.Announcements.allAnnouncements = new List<Announcement>();
                    popUp.Init(false);
                    DataManager.Player.Announcements.SetAnnouncements(new[] { creditsAnnouncement });
                    popUp.CreateAnnouncementList();
                    popUp.UpdateAnnouncementText(creditsAnnouncement.Number);
                    popUp.visibleAnnouncements._items[0].PassiveButton.OnClick.RemoveAllListeners();
                    DataManager.Player.Announcements.allAnnouncements = backup;
                }
            })));
        });
    }

    public static void addSceneChangeCallbacks()
    {
        SceneManager.add_sceneLoaded((Action<Scene, LoadSceneMode>)((scene, _) =>
        {
            if (!scene.name.Equals("MatchMaking", StringComparison.Ordinal)) return;
            TORMapOptions.gameMode = CustomGamemodes.Classic;
            // Add buttons For Guesser Mode, Hide N Seek in this scene.
            // find "HostLocalGameButton"
            var template = GameObject.FindObjectOfType<HostLocalGameButton>();
            var gameButton = template.transform.FindChild("CreateGameButton");
            var gameButtonPassiveButton = gameButton.GetComponentInChildren<PassiveButton>();

            var guesserButton = GameObject.Instantiate(gameButton, gameButton.parent);
            guesserButton.transform.localPosition += new Vector3(0f, -0.5f);
            var guesserButtonText = guesserButton.GetComponentInChildren<TextMeshPro>();
            var guesserButtonPassiveButton = guesserButton.GetComponentInChildren<PassiveButton>();

            guesserButtonPassiveButton.OnClick = new ButtonClickedEvent();
            guesserButtonPassiveButton.OnClick.AddListener((Action)(() =>
            {
                TORMapOptions.gameMode = CustomGamemodes.Guesser;
                template.OnClick();
            }));

            var HideNSeekButton = GameObject.Instantiate(gameButton, gameButton.parent);
            HideNSeekButton.transform.localPosition += new Vector3(1.7f, -0.5f);
            var HideNSeekButtonText = HideNSeekButton.GetComponentInChildren<TextMeshPro>();
            var HideNSeekButtonPassiveButton = HideNSeekButton.GetComponentInChildren<PassiveButton>();

            HideNSeekButtonPassiveButton.OnClick = new ButtonClickedEvent();
            HideNSeekButtonPassiveButton.OnClick.AddListener((Action)(() =>
            {
                TORMapOptions.gameMode = CustomGamemodes.HideNSeek;
                template.OnClick();
            }));

            var PropHuntButton = GameObject.Instantiate(gameButton, gameButton.parent);
            PropHuntButton.transform.localPosition += new Vector3(3.4f, -0.5f);
            var PropHuntButtonText = PropHuntButton.GetComponentInChildren<TextMeshPro>();
            var PropHuntButtonPassiveButton = PropHuntButton.GetComponentInChildren<PassiveButton>();

            PropHuntButtonPassiveButton.OnClick = new ButtonClickedEvent();
            PropHuntButtonPassiveButton.OnClick.AddListener((Action)(() =>
            {
                TORMapOptions.gameMode = CustomGamemodes.PropHunt;
                template.OnClick();
            }));

            template.StartCoroutine(Effects.Lerp(0.1f, new Action<float>(p =>
            {
                guesserButtonText.SetText("guesserButtonText".Translate());
                HideNSeekButtonText.SetText("HideNSeekButtonText".Translate());
                PropHuntButtonText.SetText("PropHuntButtonText".Translate());
            })));
        }));
    }
}