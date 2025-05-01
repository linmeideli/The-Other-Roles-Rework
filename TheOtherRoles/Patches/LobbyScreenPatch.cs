using System.Text.RegularExpressions;
using AmongUs.Data;
using HarmonyLib;
using InnerNet;
using TMPro;
using UnityEngine;

namespace TheOtherRoles.Patches;

[HarmonyPatch]
public sealed class LobbyJoinBind
{
    private static int GameId;

    private static GameObject LobbyText;

    [HarmonyPatch(typeof(InnerNetClient), nameof(InnerNetClient.JoinGame))]
    [HarmonyPostfix]
    public static void Postfix(InnerNetClient __instance)
    {
        GameId = __instance.GameId;
    }

    [HarmonyPatch(typeof(MMOnlineManager), nameof(MMOnlineManager.Start))]
    [HarmonyPostfix]
    public static void Postfix()
    {
        if (!LobbyText)
        {
            LobbyText = new GameObject("lobbycode");
            var comp = LobbyText.AddComponent<TextMeshPro>();
            comp.fontSize = 2.5f;
            LobbyText.transform.localPosition = new Vector3(10.3f, -3.9f, 0);
            LobbyText.SetActive(true);
        }
    }

    [HarmonyPatch(typeof(MMOnlineManager), nameof(MMOnlineManager.Update))]
    [HarmonyPostfix]
    public static void Postfix(MMOnlineManager __instance)
    {
        var code2 = GUIUtility.systemCopyBuffer;

        if (code2.Length != 6 || !Regex.IsMatch(code2, @"^[a-zA-Z]+$"))
            code2 = "";
        var code2Disp = DataManager.Settings.Gameplay.StreamerMode ? "****" : code2.ToUpper();
        if (GameId != 0 && Input.GetKeyDown(KeyCode.LeftShift))
            __instance.StartCoroutine(AmongUsClient.Instance.CoJoinOnlineGameFromCode(GameId));
        else if (Input.GetKeyDown(KeyCode.RightShift) && code2 != "")
            __instance.StartCoroutine(AmongUsClient.Instance.CoJoinOnlineGameFromCode(GameCode.GameNameToInt(code2)));

        if (LobbyText)
        {
            LobbyText.GetComponent<TextMeshPro>().text = "";
            if (GameId != 0 && GameId != 32)
            {
                var code = GameCode.IntToGameName(GameId);
                if (code != "")
                {
                    code = DataManager.Settings.Gameplay.StreamerMode ? "****" : code;
                    LobbyText.GetComponent<TextMeshPro>().text = $"Prev Lobby: {code}   [LShift]";
                }
            }

            if (code2 != "") LobbyText.GetComponent<TextMeshPro>().text += $"\nClipboard: {code2Disp}  [RShift]";
        }
    }
}