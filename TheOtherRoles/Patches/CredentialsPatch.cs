using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AmongUs.GameOptions;
using HarmonyLib;
using InnerNet;
using TheOtherRoles.CustomGameModes;
using TheOtherRoles.Utilities;
using TMPro;
using UnityEngine;

namespace TheOtherRoles.Patches;

[HarmonyPatch]
public static class CredentialsPatch
{
    public static string modName =
        "<color=#C1FFC1>Among Us<color=#FF0000> The Other Roles <color=#8470FF>Rework</color></color></color>";

    public static string fullCredentialsVersion =
        $@"<size=130%>{modName}</size> v{TheOtherRolesPlugin.Version + (TheOtherRolesPlugin.betaDays > 0 ? "-BETA" : "")}";

    public static string fullCredentials =
        @"<size=60%>Modded by <color=#00FFFF>ELinmei</color>
Based on <color=#FCCE03FF>TheOtherRoles</color></size>";

    public static string mainMenuCredentials =
        @"Modded by <color=#00FFFF>ELinmei</color>
Based on <color=#FCCE03FF>TheOtherRoles</color>";

    public static string contributorsCredentials =
        @"<size=60%> <color=#FCCE03FF>Special thanks to <color=#00FFFF>FangkuaiYa</color></color></size>";

    [HarmonyPatch(typeof(PingTracker), nameof(PingTracker.Update))]
    internal static class PingTrackerPatch
    {
        private static void Postfix(PingTracker __instance)
        {
            __instance.text.alignment = TextAlignmentOptions.Top;
            var position = __instance.GetComponent<AspectPosition>();
            position.Alignment = AspectPosition.EdgeAlignments.Top;
            if (AmongUsClient.Instance.GameState == InnerNetClient.GameStates.Started)
            {
                var gameModeText = "";
                if (HideNSeek.isHideNSeekGM) gameModeText = "Hide 'N Seek";
                else if (HandleGuesser.isGuesserGm) gameModeText = "Guesser";
                else if (PropHunt.isPropHuntGM) gameModeText = "Prop Hunt";
                if (gameModeText != "")
                    gameModeText = Helpers.cs(Color.yellow, gameModeText) + (MeetingHud.Instance ? " " : "\n");
                __instance.text.text =
                    $"<size=130%>{modName}</size> v{TheOtherRolesPlugin.Version + (TheOtherRolesPlugin.betaDays > 0 ? "-BETA" : "")}\n{gameModeText}" +
                    __instance.text.text;
                position.DistanceFromEdge =
                    MeetingHud.Instance ? new Vector3(1.25f, 0.15f, 0) : new Vector3(1.55f, 0.15f, 0);
            }
            else
            {
                var gameModeText = "";
                if (TORMapOptions.gameMode == CustomGamemodes.HideNSeek) gameModeText = "Hide 'N Seek";
                else if (TORMapOptions.gameMode == CustomGamemodes.Guesser) gameModeText = "Guesser";
                else if (TORMapOptions.gameMode == CustomGamemodes.PropHunt) gameModeText = "Prop Hunt";
                if (gameModeText != "") gameModeText = Helpers.cs(Color.yellow, gameModeText);

                __instance.text.text = $"{fullCredentialsVersion}\n{fullCredentials}\n {__instance.text.text}";
                position.DistanceFromEdge = new Vector3(0f, 0.1f, 0);

                try
                {
                    var GameModeText = GameObject.Find("GameModeText")?.GetComponent<TextMeshPro>();
                    GameModeText.text = gameModeText == ""
                        ? GameOptionsManager.Instance.currentGameOptions.GameMode == GameModes.HideNSeek
                            ? "Van. HideNSeek"
                            : "Classic"
                        : gameModeText;
                    var ModeLabel = GameObject.Find("ModeLabel")?.GetComponentInChildren<TextMeshPro>();
                    ModeLabel.text = "Game Mode";
                }
                catch
                {
                }
            }

            position.AdjustPosition();
        }
    }

    [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.Start))]
    public static class LogoPatch
    {
        public static SpriteRenderer renderer;
        public static Sprite bannerSprite;
        public static Sprite horseBannerSprite;
        public static Sprite banner2Sprite;
        private static PingTracker instance;

        public static GameObject motdObject;
        public static TextMeshPro motdText;

        private static void Postfix(PingTracker __instance)
        {
            var torLogo = new GameObject("bannerLogo_TOR");
            torLogo.transform.SetParent(GameObject.Find("RightPanel").transform, false);
            torLogo.transform.localPosition = new Vector3(-0.4f, 1f, 5f);

            renderer = torLogo.AddComponent<SpriteRenderer>();
            loadSprites();
            renderer.sprite = Helpers.loadSpriteFromResources("Banner.png", 300f);

            instance = __instance;
            loadSprites();
            // renderer.sprite = TORMapOptions.enableHorseMode ? horseBannerSprite : bannerSprite;
            renderer.sprite = EventUtility.isEnabled ? banner2Sprite : bannerSprite;
            var credentialObject = new GameObject("credentialsTOR");
            var credentials = credentialObject.AddComponent<TextMeshPro>();
            credentials.SetText(
                $"v{TheOtherRolesPlugin.Version + (TheOtherRolesPlugin.betaDays > 0 ? "-BETA" : "")}\n<size=30f%>\n</size>{mainMenuCredentials}\n<size=30%>\n</size>{contributorsCredentials}");
            credentials.alignment = TextAlignmentOptions.Center;
            credentials.fontSize *= 0.05f;

            credentials.transform.SetParent(torLogo.transform);
            credentials.transform.localPosition = Vector3.down * 1.25f;
            motdObject = new GameObject("torMOTD");
            motdText = motdObject.AddComponent<TextMeshPro>();
            motdText.alignment = TextAlignmentOptions.Center;
            motdText.fontSize *= 0.04f;

            motdText.transform.SetParent(torLogo.transform);
            motdText.enableWordWrapping = true;
            var rect = motdText.gameObject.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(5.2f, 0.25f);

            motdText.transform.localPosition = Vector3.down * 2.25f;
            motdText.color = new Color(1, 53f / 255, 31f / 255);
            var mat = motdText.fontSharedMaterial;
            mat.shaderKeywords = new[] { "OUTLINE_ON" };
            motdText.SetOutlineColor(Color.white);
            motdText.SetOutlineThickness(0.025f);
        }

        public static void loadSprites()
        {
            if (bannerSprite == null)
                bannerSprite = Helpers.loadSpriteFromResources("Banner.png", 300f);
            if (banner2Sprite == null)
                banner2Sprite = Helpers.loadSpriteFromResources("Banner2.png", 300f);
            if (horseBannerSprite == null)
                horseBannerSprite =
                    Helpers.loadSpriteFromResources("bannerTheHorseRoles.png", 300f);
        }

        public static void updateSprite()
        {
            loadSprites();
            if (renderer != null)
            {
                var fadeDuration = 1f;
                instance.StartCoroutine(Effects.Lerp(fadeDuration, new Action<float>(p =>
                {
                    renderer.color = new Color(1, 1, 1, 1 - p);
                    if (p == 1)
                    {
                        renderer.sprite = TORMapOptions.enableHorseMode ? horseBannerSprite : bannerSprite;
                        instance.StartCoroutine(Effects.Lerp(fadeDuration,
                            new Action<float>(p => { renderer.color = new Color(1, 1, 1, p); })));
                    }
                })));
            }
        }
    }

    [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.LateUpdate))]
    public static class MOTD
    {
        public static List<string> motds = new();
        private static float timer;
        private static readonly float maxTimer = 5f;
        private static int currentIndex;

        public static void Postfix()
        {
            if (motds.Count == 0)
            {
                timer = maxTimer;
                return;
            }

            if (motds.Count > currentIndex && LogoPatch.motdText != null)
                LogoPatch.motdText.SetText(motds[currentIndex]);
            else return;

            // fade in and out:
            var alpha = Mathf.Clamp01(Mathf.Min(new[] { timer, maxTimer - timer }));
            if (motds.Count == 1) alpha = 1;
            LogoPatch.motdText.color = LogoPatch.motdText.color.SetAlpha(alpha);
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = maxTimer;
                currentIndex = (currentIndex + 1) % motds.Count;
            }
        }

        public static async Task loadMOTDs()
        {
            var client = new HttpClient();
            var response =
                await client.GetAsync("https://raw.githubusercontent.com/TheOtherRolesAU/MOTD/main/motd.txt");
            response.EnsureSuccessStatusCode();
            var motds = await response.Content.ReadAsStringAsync();
            foreach (var line in motds.Split("\n", StringSplitOptions.RemoveEmptyEntries)) MOTD.motds.Add(line);
        }
    }
}