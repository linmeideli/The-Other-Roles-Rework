using System;
using System.Collections.Generic;
using HarmonyLib;
using Hazel;
using TheOtherRoles.Patches;
using UnityEngine;
using Random = System.Random;

namespace TheOtherRoles.Utilities;

[HarmonyPatch]
public static class EventUtility
{
    private static Sprite kickButtonSprite;

    public static DateTime enabled = new(DateTime.Today.Year, 4, 1);


    public static PlayerControl currentTarget;
    private static bool currentlyKicking;
    private static int kickCounter;
    public static bool isEventDate => DateTime.Today.Date == enabled;

    public static bool canBeEnabled =>
        DateTime.Today.Date >= enabled && DateTime.Today.Date <= enabled.AddDays(7); // One Week after the EVENT

    public static bool isEnabled => isEventDate || (canBeEnabled && CustomOptionHolder.enableEventMode != null &&
                                                    CustomOptionHolder.enableEventMode.getBool());

    public static Sprite getKickButtonSprite()
    {
        if (kickButtonSprite) return kickButtonSprite;
        kickButtonSprite = Helpers.loadSpriteFromResources("EventKickButton.png", 115f);
        return kickButtonSprite;
    }

    public static void Load()
    {
        if (!isEnabled) return;
    }

    public static void clearAndReload()
    {
        kickCounter = 0;
    }

    public static void Update()
    {
        //if (!isEnabled || AmongUsClient.Instance.GameState != InnerNet.InnerNetClient.GameStates.Started || TheOtherRoles.rnd == null || IntroCutscene.Instance) return;

        // set Target
        var untargetablePlayers = new List<PlayerControl>();
        foreach (var player in PlayerControl.AllPlayerControls)
            if (Mini.mini != player)
                untargetablePlayers.Add(player);
        currentTarget = PlayerControlFixedUpdatePatch.setTarget(untargetablePlayers: untargetablePlayers);
        PlayerControlFixedUpdatePatch.setPlayerOutline(currentTarget, Color.yellow);
    }

    public static void meetingEndsUpdate()
    {
        if (!isEnabled) return;
    }


    public static void meetingStartsUpdate()
    {
        if (!isEnabled) return;
    }

    public static void gameStartsUpdate()
    {
        if (!isEnabled) return;
    }

    public static void gameEndsUpdate()
    {
    }

    public static void kickTarget()
    {
        // send rpc to kick target
        var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
            (byte)CustomRPC.EventKick, SendOption.Reliable);
        writer.Write(PlayerControl.LocalPlayer.PlayerId);
        writer.Write(currentTarget.PlayerId);
        var rnd = new Random();
        var kickDistance = 1 + (float)rnd.NextDouble() * 1.5f; // 1- 2.5
        writer.Write(kickDistance);
        AmongUsClient.Instance.FinishRpcImmediately(writer);
        handleKick(PlayerControl.LocalPlayer, currentTarget, kickDistance);
    }

    public static void handleKick(PlayerControl source, PlayerControl target, float kickDistance)
    {
        if (currentlyKicking || !isEnabled) return;

        kickCounter++;

        // actual movement, canceled if meeting started
        if (Mini.growingProgress() * 18 >= CustomOptionHolder.eventHeavyAge.getFloat() ||
            kickCounter > CustomOptionHolder.eventKicksPerRound.getFloat())
        {
            // boing flip
            target = source;
            source = Mini.mini;
        }

        SoundEffectsManager.playAtPosition("fail", target.GetTruePosition(), 3, 3);

        if (target == PlayerControl.LocalPlayer)
        {
            PlayerControl.LocalPlayer.moveable = false;
            PlayerControl.LocalPlayer.NetTransform.Halt();
        }

        Vector2 direction = Vector3.Normalize(target.transform.position - source.transform.position);


        var kickDuration = 3f;
        var speed = kickDistance / kickDuration;
        var targetPosition = (Vector2)target.transform.position + direction * kickDistance;
        Vector2 startPosition = target.transform.position;

        HudManager.Instance.StartCoroutine(Effects.Lerp(kickDuration, new Action<float>(p =>
        {
            var rotAngle = 360 * 4 * (1 - Mathf.Pow(1 - p, 4)) * (direction.x > 0 ? 1 : -1);
            currentlyKicking = true;

            if (MeetingHud.Instance)
            {
                currentlyKicking = false;
                rotAngle = 0f;
            }
            else
            {
                if (p == 1)
                {
                    if (target == PlayerControl.LocalPlayer)
                    {
                        PlayerControl.LocalPlayer.moveable = true;
                        PlayerControl.LocalPlayer.NetTransform.RpcSnapTo(PlayerControl.LocalPlayer.transform.position);
                    }

                    rotAngle = 0f;
                    currentlyKicking = false;
                    target.NetTransform.Halt();
                }

                target.transform.SetLocalEulerAngles(new Vector3(0f, 0f, rotAngle), RotationOrder.OrderXYZ);

                // move the player:
                Vector3 targetStep = startPosition + (1 - Mathf.Pow(1 - p, 4)) * direction * kickDistance;

                if (!PhysicsHelpers.AnythingBetween(target.GetTruePosition(), target.GetTruePosition() + direction * 1f,
                        Constants.ShipAndObjectsMask, false)) target.transform.position = targetStep;
            }
        })));
    }


    [HarmonyPatch(typeof(ChatController), nameof(ChatController.AddChat))]
    public static class AddChatPatch
    {
        public static void Prefix(ChatController __instance, PlayerControl sourcePlayer, ref string chatText,
            bool censor)
        {
            if (!isEnabled) return;
            var charArray = chatText.ToCharArray();
            Array.Reverse(charArray);
            chatText = new string(charArray);
        }
    }
}