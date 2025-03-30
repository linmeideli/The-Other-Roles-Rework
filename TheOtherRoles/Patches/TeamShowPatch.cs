using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmongUs.GameOptions;
using HarmonyLib;
using Rewired.Utils.Platforms.Windows;
using TheOtherRoles.Modules;
using TheOtherRoles.Players;
using UnityEngine;

namespace TheOtherRoles.Patches
{
    [HarmonyPatch(typeof(IntroCutscene._ShowTeam_d__38), nameof(IntroCutscene._ShowTeam_d__38.MoveNext))]
    public static class IntroCutscene_ShowTeam__d_MoveNext
    {
        public static void Prefix(IntroCutscene._ShowTeam_d__38 __instance)
        {
            if (GameOptionsManager.Instance.currentGameMode == GameModes.HideNSeek) return;
          
            if (PlayerControl.LocalPlayer.Data.Role.IsImpostor)
            {
                __instance.__4__this.TeamTitle.text = $"{ModTranslation.GetString("ImpostorRolesText")}";
                __instance.__4__this.ImpostorText.text = ModTranslation.GetString("Atts");
                __instance.__4__this.ImpostorText.gameObject.SetActive(true);
                __instance.__4__this.ImpostorText.transform.localScale = new Vector3(0.7f, 0.7f);

            }

        }
    }
}
