﻿using AmongUs.GameOptions;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TheOtherRoles;
using TheOtherRoles.CustomGameModes;
using TheOtherRoles.Modules;
using TheOtherRoles.Players;
using TheOtherRoles.Utilities;
using TMPro;
using UnityEngine;

namespace TheOtherRoles.Patches
{
    [HarmonyPatch]
    public static class CredentialsPatch
    {
        //        public static string fullCredentialsVersion =
        //$@"<size=130%><color=#ff351f>TheOtherRolesCE</color></size> v{TheOtherRolesPlugin.Version.ToString() + (TheOtherRolesPlugin.betaDays > 0 ? "-BETA" : "")}";
        public static string ModName = $"<size=130%><color=#C1FFC1>Among Us<color=#FF0000> The Other Roles <color=#8470FF>Rework</color></color></color></size> v{TheOtherRolesPlugin.Version.ToString() + (TheOtherRolesPlugin.betaDays > 0 ? "-BETA" : "")}";
        public static string JustASysAdmin = "<color=#FCCE03FF>JustASysAdmin</color>";
        public static string FangKuai = "<color=#00FFFF>FangKuai</color>";
        public static string TOREisbison = "TheOtherRoles by <color=#FCCE03FF>Eisbison</color>";
        public static string SvettyScribbles = "<color=#FCCE03FF>SvettyScribbles</color>";
        public static string LuanMa = "<color=#9932CC>乱码</color>";
        public static string ELinmei = "<color=#00FFFF>ELinmei</color>";
        public static string mxyx = "<color=#FFB793>mxyx</color>";

        //        public static string contributorsCredentials =
        //$@"<size=60%> <color=#FCCE03FF>Special thanks to <color=#00FFFF>FangKuai<color=#FCCE03FF> & Smeggy</color></size>";
        private static float deltaTime;
        [HarmonyPatch(typeof(PingTracker), nameof(PingTracker.Update))]
        internal static class PingTrackerPatch
        {
            static void Postfix(PingTracker __instance)
            {
                var ping = AmongUsClient.Instance.Ping;
                string PingColor = "#ff4500";
                if (ping < 50) PingColor = "#44dfcc";
                else if (ping < 100) PingColor = "#7bc690";
                else if (ping < 200) PingColor = "#f3920e";
                else if (ping < 400) PingColor = "#ff146e";

                deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
                float fps = Mathf.Ceil(1.0f / deltaTime);

                __instance.text.alignment = AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Started ? TextAlignmentOptions.Top : TextAlignmentOptions.TopLeft;
                var position = __instance.GetComponent<AspectPosition>();
                position.Alignment = AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Started ? AspectPosition.EdgeAlignments.Top : AspectPosition.EdgeAlignments.LeftTop;
                if (AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Started)
                {
                    string gameModeText = $"";
                    if (HideNSeek.isHideNSeekGM) gameModeText = ModTranslation.GetString("isHideNSeekGM");
                    else if (HandleGuesser.isGuesserGm) gameModeText = ModTranslation.GetString("isGuesserGm");
                    else if (PropHunt.isPropHuntGM) gameModeText = ModTranslation.GetString("isPropHuntGM");
                    if (gameModeText != "") gameModeText = Helpers.cs(Color.yellow, gameModeText) + "\n";
                    __instance.text.text = $"<size=130%><color=#ff351f>TheOtherRoles Rework</color></size> v{TheOtherRolesPlugin.Version.ToString() + (TheOtherRolesPlugin.betaDays > 0 ? "-BETA" : "")}";
                    position.DistanceFromEdge = new Vector3(1.5f, 0.11f, 0);
                }
                else
                {
                    string gameModeText = $"";
                    if (TORMapOptions.gameMode == CustomGamemodes.HideNSeek) gameModeText = ModTranslation.GetString("isHideNSeekGM");
                    else if (TORMapOptions.gameMode == CustomGamemodes.Guesser) gameModeText = ModTranslation.GetString("isGuesserGm");
                    else if (TORMapOptions.gameMode == CustomGamemodes.PropHunt) gameModeText = ModTranslation.GetString("isPropHuntGM");
                    if (gameModeText != "") gameModeText = Helpers.cs(Color.yellow, gameModeText);

                    __instance.text.text = $"{ModName}<br><size=70%>{ModTranslation.GetString("PingText1")} {ELinmei} & {FangKuai}<br>{ModTranslation.GetString("PingText2")} {TOREisbison}<br>{ModTranslation.GetString("PingText3")} {SvettyScribbles}<br>{ModTranslation.GetString("PingText4")}{mxyx} & {ELinmei}<br></size>";
                    position.DistanceFromEdge = new Vector3(0.5f, 0.11f);

                    try
                    {
                        var GameModeText = GameObject.Find("GameModeText")?.GetComponent<TextMeshPro>();
                        GameModeText.text = gameModeText == "" ? (GameOptionsManager.Instance.currentGameOptions.GameMode == GameModes.HideNSeek ? ModTranslation.GetString("LobbyText4") : ModTranslation.GetString("LobbyText5")) : gameModeText;
                        var ModeLabel = GameObject.Find("ModeLabel")?.GetComponentInChildren<TextMeshPro>();
                        ModeLabel.text = ModTranslation.GetString("LobbyText6");
                    }
                    catch { }
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

            static void Postfix(PingTracker __instance)
            {
                var torLogo = new GameObject("bannerLogo_TOR");
                torLogo.transform.SetParent(GameObject.Find("RightPanel").transform, false);
                torLogo.transform.localPosition = new Vector3(-0.4f, 1f, 5f);

                renderer = torLogo.AddComponent<SpriteRenderer>();
                loadSprites();
                renderer.sprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.NewBanner.png", 1145141919810);
                //这些先不删了
                instance = __instance;
                loadSprites();
                // renderer.sprite = TORMapOptions.enableHorseMode ? horseBannerSprite : bannerSprite;
                renderer.sprite = EventUtility.isEnabled ? banner2Sprite : bannerSprite;
                var credentialObject = new GameObject("credentialsTOR");
                var credentials = credentialObject.AddComponent<TextMeshPro>();
                credentials.alignment = TMPro.TextAlignmentOptions.Center;
                credentials.fontSize *= 0.05f;

                credentials.transform.SetParent(torLogo.transform);
                credentials.transform.localPosition = Vector3.down * 1.25f;
                motdObject = new GameObject("torMOTD");
                motdText = motdObject.AddComponent<TextMeshPro>();
                motdText.alignment = TMPro.TextAlignmentOptions.Center;
                motdText.fontSize *= 0.04f;

                motdText.transform.SetParent(torLogo.transform);
                motdText.enableWordWrapping = true;
                var rect = motdText.gameObject.GetComponent<RectTransform>();
                rect.sizeDelta = new Vector2(5.2f, 0.25f);

                motdText.transform.localPosition = Vector3.down * 2.25f;
                motdText.color = new Color(1, 53f / 255, 31f / 255);
                Material mat = motdText.fontSharedMaterial;
                mat.shaderKeywords = new string[] { "OUTLINE_ON" };
                motdText.SetOutlineColor(Color.white);
                motdText.SetOutlineThickness(0.025f);
            }

            public static void loadSprites()
            {
                if (bannerSprite == null) bannerSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.Banner.png", 1145141919810);
                if (banner2Sprite == null) banner2Sprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.Banner2.png", 300f);
                if (horseBannerSprite == null) horseBannerSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.bannerTheHorseRoles.png", 300f);
            }

            public static void updateSprite()
            {
                loadSprites();
                if (renderer != null)
                {
                    float fadeDuration = 1f;
                    instance.StartCoroutine(Effects.Lerp(fadeDuration, new Action<float>((p) => {
                        renderer.color = new Color(1, 1, 1, 1 - p);
                        if (p == 1)
                        {
                            renderer.sprite = TORMapOptions.enableHorseMode ? horseBannerSprite : bannerSprite;
                            instance.StartCoroutine(Effects.Lerp(fadeDuration, new Action<float>((p) => {
                                renderer.color = new Color(1, 1, 1, p);
                            })));
                        }
                    })));
                }
            }
        }
        /// <summary>
        /// 666
        /// </summary>
        [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.LateUpdate))]
        public static class MOTD
        {
            public static List<string> motds = new List<string>();
            private static float timer = 0f;
            private static float maxTimer = 5f;
            private static int currentIndex = 0;

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
                float alpha = Mathf.Clamp01(Mathf.Min(new float[] { timer, maxTimer - timer }));
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
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync("https://raw.githubusercontent.com/TheOtherRolesAU/MOTD/main/motd.txt");
                response.EnsureSuccessStatusCode();
                string motds = await response.Content.ReadAsStringAsync();
                foreach (string line in motds.Split("\n", StringSplitOptions.RemoveEmptyEntries))
                {
                    MOTD.motds.Add(line);
                }
            }
        }
    }
}
