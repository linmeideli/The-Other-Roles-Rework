using System;
using System.Linq;
using HarmonyLib;
using Hazel;
using InnerNet;
using TheOtherRoles.Utilities;

namespace TheOtherRoles.Modules;

[HarmonyPatch]
public static class ChatCommands
{
    public static bool isLover(this PlayerControl player)
    {
        return !(player == null) && (player == Lovers.lover1 || player == Lovers.lover2);
    }

    [HarmonyPatch(typeof(ChatController), nameof(ChatController.SendChat))]
    private static class SendChatPatch
    {
        private static bool Prefix(ChatController __instance)
        {
            var text = __instance.freeChatField.Text;
            var handled = false;
            if (AmongUsClient.Instance.GameState != InnerNetClient.GameStates.Started)
            {
                if (text.ToLower().StartsWith("/kick "))
                {
                    var playerName = text.Substring(6);
                    var target = PlayerControl.AllPlayerControls.ToArray()
                        .FirstOrDefault(x => x.Data.PlayerName.Equals(playerName));
                    if (target != null && AmongUsClient.Instance != null && AmongUsClient.Instance.CanBan())
                    {
                        var client = AmongUsClient.Instance.GetClient(target.OwnerId);
                        if (client != null)
                        {
                            AmongUsClient.Instance.KickPlayer(client.Id, false);
                            handled = true;
                        }
                    }
                }
                else if (text.ToLower().StartsWith("/ban "))
                {
                    var playerName = text.Substring(5);
                    var target = PlayerControl.AllPlayerControls.ToArray()
                        .FirstOrDefault(x => x.Data.PlayerName.Equals(playerName));
                    if (target != null && AmongUsClient.Instance != null && AmongUsClient.Instance.CanBan())
                    {
                        var client = AmongUsClient.Instance.GetClient(target.OwnerId);
                        if (client != null)
                        {
                            AmongUsClient.Instance.KickPlayer(client.Id, true);
                            handled = true;
                        }
                    }
                }
                else if (text.ToLower().StartsWith("/gm"))
                {
                    var gm = text.Substring(4).ToLower();
                    var gameMode = CustomGamemodes.Classic;
                    if (gm.StartsWith("prop") || gm.StartsWith("ph"))
                        gameMode = CustomGamemodes.PropHunt;
                    else if (gm.StartsWith("guess") || gm.StartsWith("gm"))
                        gameMode = CustomGamemodes.Guesser;
                    else if (gm.StartsWith("hide") || gm.StartsWith("hn")) gameMode = CustomGamemodes.HideNSeek;
                    // else its classic!

                    if (AmongUsClient.Instance.AmHost)
                    {
                        var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                            (byte)CustomRPC.ShareGamemode, SendOption.Reliable);
                        writer.Write((byte)TORMapOptions.gameMode);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                        RPCProcedure.shareGamemode((byte)gameMode);
                        RPCProcedure.shareGamemode((byte)TORMapOptions.gameMode);
                    }
                    else
                    {
                        __instance.AddChat(PlayerControl.LocalPlayer,
                            "Nice try, but you have to be the host to use this feature");
                    }

                    handled = true;
                }
            }

            if (AmongUsClient.Instance.NetworkMode == NetworkModes.FreePlay)
            {
                if (text.ToLower().Equals("/murder"))
                {
                    PlayerControl.LocalPlayer.Exiled();
                    FastDestroyableSingleton<HudManager>.Instance.KillOverlay.ShowKillAnimation(
                        PlayerControl.LocalPlayer.Data, PlayerControl.LocalPlayer.Data);
                    handled = true;
                }
                else if (text.ToLower().StartsWith("/color "))
                {
                    handled = true;
                    int col;
                    if (!int.TryParse(text.Substring(7), out col))
                        __instance.AddChat(PlayerControl.LocalPlayer, "Unable to parse color id\nUsage: /color {id}");
                    col = Math.Clamp(col, 0, Palette.PlayerColors.Length - 1);
                    PlayerControl.LocalPlayer.SetColor(col);
                    __instance.AddChat(PlayerControl.LocalPlayer, "Changed color succesfully");
                    ;
                }
            }

            if (text.ToLower().StartsWith("/tp ") && PlayerControl.LocalPlayer.Data.IsDead)
            {
                var playerName = text.Substring(4).ToLower();
                var target = PlayerControl.AllPlayerControls.ToArray()
                    .FirstOrDefault(x => x.Data.PlayerName.ToLower().Equals(playerName));
                if (target != null)
                {
                    PlayerControl.LocalPlayer.transform.position = target.transform.position;
                    handled = true;
                }
            }

            if (text.ToLower().StartsWith("/role"))
            {
                var localRole = RoleInfo.getRoleInfoForPlayer(PlayerControl.LocalPlayer, false).FirstOrDefault();
                if (localRole != RoleInfo.impostor && localRole != RoleInfo.crewmate)
                {
                    var info = RoleInfo.GetRoleDescription(localRole);
                    __instance.AddChat(PlayerControl.LocalPlayer, info);
                    handled = true;
                }
            }

            if (handled)
            {
                __instance.freeChatField.Clear();
                __instance.quickChatMenu.Clear();
            }

            return !handled;
        }
    }

    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public static class EnableChat
    {
        public static void Postfix(HudManager __instance)
        {
            if (!__instance.Chat.isActiveAndEnabled && (AmongUsClient.Instance.NetworkMode == NetworkModes.FreePlay ||
                                                        (PlayerControl.LocalPlayer.isLover() && Lovers.enableChat)))
                __instance.Chat.SetVisible(true);
        }
    }

    [HarmonyPatch(typeof(ChatBubble), nameof(ChatBubble.SetName))]
    public static class SetBubbleName
    {
        public static void Postfix(ChatBubble __instance, [HarmonyArgument(0)] string playerName)
        {
            var sourcePlayer = PlayerControl.AllPlayerControls.ToArray().ToList()
                .FirstOrDefault(x => x.Data != null && x.Data.PlayerName.Equals(playerName));
            if (sourcePlayer != null && PlayerControl.LocalPlayer != null &&
                PlayerControl.LocalPlayer.Data?.Role?.IsImpostor == true &&
                ((Spy.spy != null && sourcePlayer.PlayerId == Spy.spy.PlayerId) ||
                 (Sidekick.sidekick != null && Sidekick.wasTeamRed &&
                  sourcePlayer.PlayerId == Sidekick.sidekick.PlayerId) ||
                 (Jackal.jackal != null && Jackal.wasTeamRed && sourcePlayer.PlayerId == Jackal.jackal.PlayerId)) &&
                __instance != null) __instance.NameText.color = Palette.ImpostorRed;
        }
    }

    [HarmonyPatch(typeof(ChatController), nameof(ChatController.AddChat))]
    public static class AddChat
    {
        public static bool Prefix(ChatController __instance, [HarmonyArgument(0)] PlayerControl sourcePlayer)
        {
            if (__instance != FastDestroyableSingleton<HudManager>.Instance.Chat)
                return true;
            var localPlayer = PlayerControl.LocalPlayer;
            return localPlayer == null || MeetingHud.Instance != null || LobbyBehaviour.Instance != null ||
                   localPlayer.Data.IsDead || (localPlayer.isLover() && Lovers.enableChat) ||
                   sourcePlayer.PlayerId == PlayerControl.LocalPlayer.PlayerId;
        }
    }
}