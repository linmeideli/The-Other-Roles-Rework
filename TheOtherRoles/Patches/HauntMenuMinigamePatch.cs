﻿using System;
using System.Linq;
using AmongUs.GameOptions;
using HarmonyLib;
using TheOtherRoles.CustomGameModes;
using TheOtherRoles.Modules;
using UnityEngine;

namespace TheOtherRoles.Patches;

[HarmonyPatch]
public static class HauntMenuMinigamePatch
{
    // Show the role name instead of just Crewmate / Impostor
    [HarmonyPostfix]
    [HarmonyPatch(typeof(HauntMenuMinigame), nameof(HauntMenuMinigame.SetFilterText))]
    public static void Postfix(HauntMenuMinigame __instance)
    {
        if (GameOptionsManager.Instance.currentGameOptions.GameMode != GameModes.Normal) return;
        var target = __instance.HauntTarget;
        var roleInfo = RoleInfo.getRoleInfoForPlayer(target, false);
        var roleString = roleInfo.Count > 0 && TORMapOptions.ghostsSeeRoles ? roleInfo[0].name : "";
        if (__instance.HauntTarget.Data.IsDead)
        {
            __instance.FilterText.text = roleString + "filterTextGhost".Translate();
            return;
        }

        __instance.FilterText.text = roleString;
    }

    // The impostor filter now includes neutral roles
    [HarmonyPostfix]
    [HarmonyPatch(typeof(HauntMenuMinigame), nameof(HauntMenuMinigame.MatchesFilter))]
    public static void MatchesFilterPostfix(HauntMenuMinigame __instance, PlayerControl pc, ref bool __result)
    {
        if (GameOptionsManager.Instance.currentGameOptions.GameMode != GameModes.Normal) return;
        if (__instance.filterMode == HauntMenuMinigame.HauntFilters.Impostor)
        {
            var info = RoleInfo.getRoleInfoForPlayer(pc, false);
            __result = (pc.Data.Role.IsImpostor || info.Any(x => x.isNeutral)) && !pc.Data.IsDead;
        }
    }


    // Shows the "haunt evil roles button"
    [HarmonyPrefix]
    [HarmonyPatch(typeof(HauntMenuMinigame), nameof(HauntMenuMinigame.Start))]
    public static bool StartPrefix(HauntMenuMinigame __instance)
    {
        if (GameOptionsManager.Instance.currentGameOptions.GameMode != GameModes.Normal ||
            !TORMapOptions.ghostsSeeRoles) return true;
        __instance.FilterButtons[0].gameObject.SetActive(true);
        var numActive = 0;
        var numButtons = __instance.FilterButtons.Count(s => s.isActiveAndEnabled);
        var edgeDist = 0.6f * numButtons;
        for (var i = 0; i < __instance.FilterButtons.Length; i++)
        {
            var passiveButton = __instance.FilterButtons[i];
            if (passiveButton.isActiveAndEnabled)
            {
                passiveButton.transform.SetLocalX(FloatRange.SpreadToEdges(-edgeDist, edgeDist, numActive, numButtons));
                numActive++;
            }
        }

        return false;
    }

    // Moves the haunt menu a bit further down
    [HarmonyPostfix]
    [HarmonyPatch(typeof(HauntMenuMinigame), nameof(HauntMenuMinigame.FixedUpdate))]
    public static void UpdatePostfix(HauntMenuMinigame __instance)
    {
        if (GameOptionsManager.Instance.currentGameOptions.GameMode != GameModes.Normal) return;
        if (PlayerControl.LocalPlayer.Data.Role.IsImpostor && Vampire.vampire != PlayerControl.LocalPlayer)
            __instance.gameObject.transform.localPosition =
                new Vector3(-6f, -1.1f, __instance.gameObject.transform.localPosition.z);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(AbilityButton), nameof(AbilityButton.Update))]
    public static void showOrHideAbilityButtonPostfix(AbilityButton __instance)
    {
        var isGameMode = GameOptionsManager.Instance.currentGameOptions.GameMode == GameModes.HideNSeek ||
                         PropHunt.isPropHuntGM || HideNSeek.isHideNSeekGM;
        if (PlayerControl.LocalPlayer.Data.IsDead &&
            (CustomOptionHolder.finishTasksBeforeHauntingOrZoomingOut.getBool() || isGameMode))
        {
            // player has haunt button.
            var (playerCompleted, playerTotal) = TasksHandler.taskInfo(PlayerControl.LocalPlayer.Data);
            var numberOfLeftTasks = playerTotal - playerCompleted;
            if (numberOfLeftTasks <= 0 || isGameMode)
                __instance.Show();
            else
                __instance.Hide();
        }
    }
}