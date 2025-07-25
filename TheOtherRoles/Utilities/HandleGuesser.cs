﻿using TheOtherRoles.CustomGameModes;
using UnityEngine;

namespace TheOtherRoles.Utilities;

public static class HandleGuesser
{
    private static Sprite targetSprite;
    public static bool isGuesserGm;
    public static bool hasMultipleShotsPerMeeting;
    public static bool killsThroughShield = true;
    public static bool evilGuesserCanGuessSpy = true;
    public static bool guesserCantGuessSnitch;

    public static int tasksToUnlock =
        Mathf.RoundToInt(CustomOptionHolder.guesserGamemodeCrewGuesserNumberOfTasks.getFloat());

    public static Sprite getTargetSprite()
    {
        if (targetSprite) return targetSprite;
        targetSprite = Helpers.loadSpriteFromResources("TargetIcon.png", 150f);
        return targetSprite;
    }

    public static bool isGuesser(byte playerId)
    {
        if (isGuesserGm) return GuesserGM.isGuesser(playerId);
        return Guesser.isGuesser(playerId);
    }

    public static void clear(byte playerId)
    {
        if (isGuesserGm) GuesserGM.clear(playerId);
        else Guesser.clear(playerId);
    }

    public static int remainingShots(byte playerId, bool shoot = false)
    {
        if (isGuesserGm) return GuesserGM.remainingShots(playerId, shoot);
        return Guesser.remainingShots(playerId, shoot);
    }

    public static void clearAndReload()
    {
        Guesser.clearAndReload();
        GuesserGM.clearAndReload();
        isGuesserGm = TORMapOptions.gameMode == CustomGamemodes.Guesser;
        if (isGuesserGm)
        {
            guesserCantGuessSnitch = CustomOptionHolder.guesserGamemodeCantGuessSnitchIfTaksDone.getBool();
            hasMultipleShotsPerMeeting = CustomOptionHolder.guesserGamemodeHasMultipleShotsPerMeeting.getBool();
            killsThroughShield = CustomOptionHolder.guesserGamemodeKillsThroughShield.getBool();
            evilGuesserCanGuessSpy = CustomOptionHolder.guesserGamemodeEvilCanKillSpy.getBool();
            tasksToUnlock = Mathf.RoundToInt(CustomOptionHolder.guesserGamemodeCrewGuesserNumberOfTasks.getFloat());
        }
        else
        {
            guesserCantGuessSnitch = CustomOptionHolder.guesserCantGuessSnitchIfTaksDone.getBool();
            hasMultipleShotsPerMeeting = CustomOptionHolder.guesserHasMultipleShotsPerMeeting.getBool();
            killsThroughShield = CustomOptionHolder.guesserKillsThroughShield.getBool();
            evilGuesserCanGuessSpy = CustomOptionHolder.guesserEvilCanKillSpy.getBool();
        }
    }
}