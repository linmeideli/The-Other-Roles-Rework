using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace TheOtherRoles.CustomGameModes
{
    public static  class TemporKillers
    {
        public static bool isTemporKillersGM;
        public static TMP_Text timerText;
        public static bool isWantedTimer = true;
        public static bool isWaitingTimer = true;
        public static DateTime startTime = DateTime.UtcNow;

        public static float timer = 300f;
        public static float hunterVision = 0.5f;

        public static float killCooldown = 10f;
        public static float hunterWaitingTime = 5f;
        public static float hunterWantedTime = 50f;

        public static bool isHunter()
        {
            return isTemporKillersGM && PlayerControl.LocalPlayer != null && PlayerControl.LocalPlayer.Data.Role.IsImpostor;
        }

        public static List<PlayerControl> getHunters()
        {
            var hunters = new List<PlayerControl>(PlayerControl.AllPlayerControls.ToArray());
            hunters.RemoveAll(x => !x.Data.Role.IsImpostor);
            return hunters;
        }

        public static bool isHunted()
        {
            return isTemporKillersGM && PlayerControl.LocalPlayer != null && !PlayerControl.LocalPlayer.Data.Role.IsImpostor;
        }

        public static void clearAndReload()
        {
            isTemporKillersGM = TORMapOptions.gameMode == CustomGamemodes.TemporKillers;
            if (timerText != null) UnityEngine.Object.Destroy(timerText);
            timerText = null;
            isWantedTimer = true;
            isWaitingTimer = true;
            startTime = DateTime.UtcNow;

            timer = CustomOptionHolder.hideNSeekTimer.getFloat() * 60;
            hunterVision = CustomOptionHolder.hideNSeekHunterVision.getFloat();
            killCooldown = CustomOptionHolder.hideNSeekKillCooldown.getFloat();
            hunterWaitingTime = CustomOptionHolder.hideNSeekHunterWaiting.getFloat();
            hunterWantedTime = CustomOptionHolder.hideNSeekTimer.getFloat();

            Hunter.clearAndReload();
            Hunted.clearAndReload();
        }
    }
}
