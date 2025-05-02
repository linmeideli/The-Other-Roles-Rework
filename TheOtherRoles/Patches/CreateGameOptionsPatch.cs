using HarmonyLib;
using Reactor.Utilities.Extensions;
using System;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TheOtherRoles.Patches;

internal class CreateGameOptionsPatch
{
    public static PassiveButton modeButtonGS;
    public static PassiveButton modeButtonHK;
    public static PassiveButton modeButtonPH;

    [HarmonyPatch(typeof(CreateGameOptions), nameof(CreateGameOptions.Start))]
    public static class CreateGameOptionsStartPatch
    {
        private static void Postfix(CreateGameOptions __instance)
        {
            var blackSquare = GameObject.Find("BlackSquare");
            blackSquare.transform.localPosition = new Vector3(-2.35f, -4.04f, -1f);
            __instance.modeButtons[0].transform.localPosition = new Vector3(-0f, -4f, -3f);
            __instance.modeButtons[1].transform.localPosition = new Vector3(2.91f, -4f, -3f);
            __instance.serverDropdown.transform.SetLocalY(-2.63f);

            __instance.modeButtons[0].OnClick.AddListener((Action)(() =>
            {
                modeButtonGS.SelectButton(false);
                modeButtonHK.SelectButton(false);
                modeButtonPH.SelectButton(false);
            }
            ));

            __instance.modeButtons[1].OnClick.AddListener((Action)(() =>
            {
                modeButtonGS.SelectButton(false);
                modeButtonHK.SelectButton(false);
                modeButtonPH.SelectButton(false);
            }
            ));

            TORMapOptions.gameMode = CustomGamemodes.Classic;

            modeButtonGS = Object.Instantiate(__instance.modeButtons[0], __instance.modeButtons[0].transform);
            modeButtonGS.name = "TORGUESSER";
            changeButtonText(modeButtonGS, "TOR Guesser");
            modeButtonGS.transform.localPosition = new Vector3(5.8f, -0f, -3f);
            modeButtonGS.OnClick.RemoveAllListeners();
            __instance.StartCoroutine(Effects.Lerp(0.1f, new Action<float>(p => modeButtonGS.SelectButton(false))));
            modeButtonGS.OnMouseOver.AddListener((Action)(() => __instance.tooltip.SetText("An extension to the Classic-Gamemode and gives you a multitude of new options for Guessers.")));
            modeButtonGS.OnClick.AddListener((Action)(() =>
            {
                TORMapOptions.gameMode = CustomGamemodes.Guesser;
                modeButtonGS.SelectButton(true);
                __instance.modeButtons[0].SelectButton(false);
                __instance.modeButtons[1].SelectButton(false);
                modeButtonHK.SelectButton(false);
                modeButtonPH.SelectButton(false);
            }
            ));

            modeButtonHK = Object.Instantiate(modeButtonGS, __instance.modeButtons[0].transform);
            modeButtonHK.name = "TORHIDENSEEK";
            changeButtonText(modeButtonHK, "TOR Hide N Seek");
            modeButtonHK.transform.localPosition = new Vector3(0, -0.8f, -3f);
            modeButtonHK.OnClick.RemoveAllListeners();
            __instance.StartCoroutine(Effects.Lerp(0.1f, new Action<float>(p => modeButtonHK.SelectButton(false))));
            modeButtonHK.OnMouseOver.AddListener((Action)(() => __instance.tooltip.SetText("A standalone Gamemode where Hunter have to catch their prey (\"Hunted\" players).")));
            modeButtonHK.OnClick.AddListener((Action)(() =>
            {
                TORMapOptions.gameMode = CustomGamemodes.HideNSeek;
                modeButtonHK.SelectButton(true);
                __instance.modeButtons[0].SelectButton(false);
                __instance.modeButtons[1].SelectButton(false);
                modeButtonGS.SelectButton(false);
                modeButtonPH.SelectButton(false);
            }
            ));

            modeButtonPH = Object.Instantiate(modeButtonHK, __instance.modeButtons[0].transform);
            modeButtonPH.name = "TORPROPHUNT";
            changeButtonText(modeButtonPH, "TOR Prop Hunt");
            modeButtonPH.transform.localPosition = new Vector3(2.9f, -0.8f, -3f);
            modeButtonPH.OnClick.RemoveAllListeners();
            __instance.StartCoroutine(Effects.Lerp(0.1f, new Action<float>(p => modeButtonPH.SelectButton(false))));
            modeButtonPH.OnMouseOver.AddListener((Action)(() => __instance.tooltip.SetText("A standalone Gamemode where Hunters have to find the disguised players (\"Props\").")));
            modeButtonPH.OnClick.AddListener((Action)(() =>
            {
                TORMapOptions.gameMode = CustomGamemodes.PropHunt;
                modeButtonPH.SelectButton(true);
                __instance.modeButtons[0].SelectButton(false);
                __instance.modeButtons[1].SelectButton(false);
                modeButtonGS.SelectButton(false);
                modeButtonHK.SelectButton(false);
            }
            ));
        }
        internal static void changeButtonText(PassiveButton passiveButton, string buttonText)
        {
            passiveButton.transform.FindChild("SelectedInactive/ClassicText").gameObject.GetComponentInChildren<TextTranslatorTMP>().Destroy();
            passiveButton.transform.FindChild("Inactive/ClassicText").gameObject.GetComponentInChildren<TextTranslatorTMP>().Destroy();
            passiveButton.transform.FindChild("Highlight/ClassicText").gameObject.GetComponentInChildren<TextTranslatorTMP>().Destroy();
            passiveButton.transform.FindChild("SelectedHighlight/ClassicText").gameObject.GetComponentInChildren<TextTranslatorTMP>().Destroy();

            passiveButton.transform.FindChild("SelectedInactive/ClassicText").gameObject.GetComponentInChildren<TMP_Text>().SetText(buttonText);
            passiveButton.transform.FindChild("Inactive/ClassicText").gameObject.GetComponentInChildren<TMP_Text>().SetText(buttonText);
            passiveButton.transform.FindChild("Highlight/ClassicText").gameObject.GetComponentInChildren<TMP_Text>().SetText(buttonText);
            passiveButton.transform.FindChild("SelectedHighlight/ClassicText").gameObject.GetComponentInChildren<TMP_Text>().SetText(buttonText);
        }
    }
}