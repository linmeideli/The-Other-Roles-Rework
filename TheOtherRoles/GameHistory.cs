using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TheOtherRoles;

public class DeadPlayer
{
    public enum CustomDeathReason
    {
        Exile,
        Kill,
        Disconnect,
        Guess,
        Shift,
        LawyerSuicide,
        LoverSuicide, // not necessary
        WitchExile,
        Bomb,
        Arson
    }

    public CustomDeathReason deathReason;
    public PlayerControl killerIfExisting;

    public PlayerControl player;
    public DateTime timeOfDeath;
    public bool wasCleaned;

    public DeadPlayer(PlayerControl player, DateTime timeOfDeath, CustomDeathReason deathReason,
        PlayerControl killerIfExisting)
    {
        this.player = player;
        this.timeOfDeath = timeOfDeath;
        this.deathReason = deathReason;
        this.killerIfExisting = killerIfExisting;
        wasCleaned = false;
    }
}

internal static class GameHistory
{
    public static List<Tuple<Vector3, bool>> localPlayerPositions = new();
    public static List<DeadPlayer> deadPlayers = new();

    public static void clearGameHistory()
    {
        localPlayerPositions = new List<Tuple<Vector3, bool>>();
        deadPlayers = new List<DeadPlayer>();
    }

    public static void overrideDeathReasonAndKiller(PlayerControl player, DeadPlayer.CustomDeathReason deathReason,
        PlayerControl killer = null)
    {
        var target = deadPlayers.FirstOrDefault(x => x.player.PlayerId == player.PlayerId);
        if (target != null)
        {
            target.deathReason = deathReason;
            if (killer != null) target.killerIfExisting = killer;
        }
        else if (player != null)
        {
            // Create dead player if needed:
            var dp = new DeadPlayer(player, DateTime.UtcNow, deathReason, killer);
            deadPlayers.Add(dp);
        }
    }
}