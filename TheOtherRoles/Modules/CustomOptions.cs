﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AmongUs.GameOptions;
using BepInEx.Configuration;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using Hazel;
using InnerNet;
using Reactor.Utilities.Extensions;
using TheOtherRoles.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static TheOtherRoles.TheOtherRoles;
using static TheOtherRoles.CustomOption;
using Object = UnityEngine.Object;
using TheOtherRoles.Modules;
using Reactor.Localization.Utilities;

namespace TheOtherRoles;

public class CustomOption
{
    public enum CustomOptionType
    {
        General,
        Impostor,
        Neutral,
        Crewmate,
        Modifier,
        Guesser,
        HideNSeekMain,
        HideNSeekRoles,
        PropHunt
    }

    public static List<CustomOption> options = new();
    public static int preset;
    public static ConfigEntry<string> vanillaSettings;

    public int defaultSelection;
    public ConfigEntry<int> entry;
    public string heading = "";

    public int id;
    public int id2;
    public bool invertedParent;
    public bool isHeader;
    public string name;
    public Action onChange;
    public OptionBehaviour optionBehaviour;
    public CustomOption parent;
    public int selection;
    public object[] selections;
    public CustomOptionType type;

    // Option creation

    public CustomOption(int id, CustomOptionType type, string name, object[] selections, object defaultValue,
        CustomOption parent, bool isHeader, Action onChange = null, string heading = "", bool invertedParent = false, int id2 = 0)
    {
        this.id = id;
        this.id2 = id2;
        this.name = parent == null ? name : "- " + name;
        this.selections = selections;
        var index = Array.IndexOf(selections, defaultValue);
        defaultSelection = index >= 0 ? index : 0;
        this.parent = parent;
        this.isHeader = isHeader;
        this.type = type;
        this.onChange = onChange;
        this.heading = heading;
        this.invertedParent = invertedParent;
        selection = 0;
        if (id != 0)
        {
            entry = TheOtherRolesPlugin.Instance.Config.Bind($"Preset{preset}", id.ToString(), defaultSelection);
            selection = Mathf.Clamp(entry.Value, 0, selections.Length - 1);
        }

        options.Add(this);
    }
    public static int numId = 0;
    public static CustomOption CreateRoleOption(CustomOptionType type, RoleId roleId, string[] selections,
        CustomOption parent = null, bool isHeader = false, Action onChange = null, string heading = "",
        bool invertedParent = false, int id2 = 0)
    {
        return new CustomOption(numId++, type, Helpers.cs(RoleInfo.roleInfoById[roleId].color, RoleInfo.roleInfoById[roleId].name), selections, "", parent, isHeader, onChange, heading, invertedParent, id2);
    }
    public static CustomOption CreateModifierOption(CustomOptionType type, RoleId roleId, string[] selections,
        CustomOption parent = null, bool isHeader = false, Action onChange = null, string heading = "",
        bool invertedParent = false, int id2 = 0)
    {
        return new CustomOption(numId++, type, Helpers.cs(RoleInfo.roleInfoById[roleId].color, RoleInfo.roleInfoById[roleId].name), selections, "", parent, isHeader, onChange, heading, invertedParent, id2);
    }
    public static CustomOption Create(CustomOptionType type, string name, string[] selections,
        CustomOption parent = null, bool isHeader = false, Action onChange = null, string heading = "",
        bool invertedParent = false, int id2 = 0)
    {
        return new CustomOption(numId++, type, name, selections, "", parent, isHeader, onChange, heading, invertedParent, id2);
    }

    public static CustomOption Create(CustomOptionType type, string name, float defaultValue, float min,
        float max, float step, CustomOption parent = null, bool isHeader = false, Action onChange = null,
        string heading = "", bool invertedParent = false, int id2 = 0)
    {
        List<object> selections = new();
        for (var s = min; s <= max; s += step)
            selections.Add(s);
        return new CustomOption(numId++, type, name, selections.ToArray(), defaultValue, parent, isHeader, onChange, heading,
            invertedParent, id2);
    }

    public static CustomOption Create(CustomOptionType type, string name, bool defaultValue,
        CustomOption parent = null, bool isHeader = false, Action onChange = null, string heading = "",
        bool invertedParent = false, int id2 = 0)
    {
        return new CustomOption(numId++, type, name, new[] { "<color=#696969>optionOff</color>".Translate(), "<color=#00FFFF>optionOn</color>".Translate() }, defaultValue ? "<color=#00FFFF>optionOn</color>".Translate() : "<color=#696969>optionOff</color>".Translate(), parent, isHeader,
            onChange, heading, invertedParent, id2);
    }

    // Static behaviour

    public static void switchPreset(int newPreset)
    {
        saveVanillaOptions();
        preset = newPreset;
        vanillaSettings = TheOtherRolesPlugin.Instance.Config.Bind($"Preset{preset}", "GameOptions", "");
        loadVanillaOptions();
        foreach (var option in options)
        {
            if (option.id == 0) continue;

            option.entry =
                TheOtherRolesPlugin.Instance.Config.Bind($"Preset{preset}", option.id.ToString(),
                    option.defaultSelection);
            option.selection = Mathf.Clamp(option.entry.Value, 0, option.selections.Length - 1);
            if (option.optionBehaviour != null && option.optionBehaviour is StringOption stringOption)
            {
                stringOption.oldValue = stringOption.Value = option.selection;
                stringOption.ValueText.text = option.getString();
            }
        }

        // make sure to reload all tabs, even the ones in the background, because they might have changed when the preset was switched!
        if (AmongUsClient.Instance?.AmHost == true)
            foreach (var entry in GameOptionsMenuStartPatch.currentGOMs)
            {
                var optionType = (CustomOptionType)entry.Key;
                var gom = entry.Value;
                if (gom != null) GameOptionsMenuStartPatch.updateGameOptionsMenu(optionType, gom);
            }
    }

    public static void saveVanillaOptions()
    {
        vanillaSettings.Value =
            Convert.ToBase64String(
                GameOptionsManager.Instance.gameOptionsFactory.ToBytes(
                    GameManager.Instance.LogicOptions.currentGameOptions, false));
    }

    public static bool loadVanillaOptions()
    {
        var optionsString = vanillaSettings.Value;
        if (optionsString == "") return false;
        var gameOptions =
            GameOptionsManager.Instance.gameOptionsFactory.FromBytes(Convert.FromBase64String(optionsString));
        if (gameOptions.Version < 8)
        {
            TheOtherRolesPlugin.Logger.LogMessage("tried to paste old settings, not doing this!");
            return false;
        }

        GameOptionsManager.Instance.GameHostOptions = gameOptions;
        GameOptionsManager.Instance.CurrentGameOptions = GameOptionsManager.Instance.GameHostOptions;
        GameManager.Instance.LogicOptions.SetGameOptions(GameOptionsManager.Instance.CurrentGameOptions);
        GameManager.Instance.LogicOptions.SyncOptions();
        return true;
    }

    public static void ShareOptionChange(uint optionId)
    {
        var option = options.FirstOrDefault(x => x.id == optionId);
        if (option == null) return;
        var writer = AmongUsClient.Instance!.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
            (byte)CustomRPC.ShareOptions, SendOption.Reliable);
        writer.Write((byte)1);
        writer.WritePacked((uint)option.id);
        writer.WritePacked(Convert.ToUInt32(option.selection));
        AmongUsClient.Instance.FinishRpcImmediately(writer);
    }

    public static void ShareOptionSelections()
    {
        if (PlayerControl.AllPlayerControls.Count <= 1 ||
            (AmongUsClient.Instance!.AmHost == false && PlayerControl.LocalPlayer == null)) return;
        var optionsList = new List<CustomOption>(options);
        while (optionsList.Any())
        {
            var amount = (byte)Math.Min(optionsList.Count, 200); // takes less than 3 bytes per option on average
            var writer = AmongUsClient.Instance!.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                (byte)CustomRPC.ShareOptions, SendOption.Reliable);
            writer.Write(amount);
            for (var i = 0; i < amount; i++)
            {
                var option = optionsList[0];
                optionsList.RemoveAt(0);
                writer.WritePacked((uint)option.id);
                writer.WritePacked(Convert.ToUInt32(option.selection));
            }

            AmongUsClient.Instance.FinishRpcImmediately(writer);
        }
    }

    // Getter

    public int getSelection()
    {
        return selection;
    }

    public bool getBool()
    {
        return selection > 0;
    }

    public float getFloat()
    {
        return (float)selections[selection];
    }

    public int getQuantity()
    {
        return selection + 1;
    }
    public string getString()
    {
        string sel = selections[selection].ToString();
        return ModTranslation.GetString(sel);
    }

    public string getName()
    {
        return ModTranslation.GetString(name);
    }

    public string getHeading()
    {
        if (heading == "") return "";
        return ModTranslation.GetString(heading);
    }


    public void updateSelection(int newSelection, bool notifyUsers = true)
    {
        newSelection = Mathf.Clamp((newSelection + selections.Length) % selections.Length, 0, selections.Length - 1);
        if (AmongUsClient.Instance?.AmClient == true && notifyUsers && selection != newSelection)
        {
            DestroyableSingleton<HudManager>.Instance.Notifier.AddSettingsChangeMessage(CustomStringName.CreateAndRegister(getName()), selections[newSelection].ToString(), false);
            try
            {
                selection = newSelection;
                if (GameStartManager.Instance != null && GameStartManager.Instance.LobbyInfoPane != null &&
                    GameStartManager.Instance.LobbyInfoPane.LobbyViewSettingsPane != null &&
                    GameStartManager.Instance.LobbyInfoPane.LobbyViewSettingsPane.gameObject.activeSelf)
                    LobbyViewSettingsPaneChangeTabPatch.Postfix(
                        GameStartManager.Instance.LobbyInfoPane.LobbyViewSettingsPane,
                        GameStartManager.Instance.LobbyInfoPane.LobbyViewSettingsPane.currentTab);
            }
            catch
            {
            }
        }

        selection = newSelection;
        try
        {
            if (onChange != null) onChange();
        }
        catch
        {
        }


        if (optionBehaviour != null && optionBehaviour is StringOption stringOption)
        {
            stringOption.oldValue = stringOption.Value = selection;
            stringOption.ValueText.text = getString();
            if (AmongUsClient.Instance?.AmHost == true && PlayerControl.LocalPlayer)
            {
                if (id == 0 && selection != preset)
                {
                    switchPreset(selection); // Switch presets
                    ShareOptionSelections();
                }
                else if (entry != null)
                {
                    entry.Value = selection; // Save selection to config
                    ShareOptionChange((uint)id); // Share single selection
                }
            }
        }
        else if (id == 0 && AmongUsClient.Instance?.AmHost == true && PlayerControl.LocalPlayer)
        {
            // Share the preset switch for random maps, even if the menu isnt open!
            switchPreset(selection);
            ShareOptionSelections(); // Share all selections
        }

        if (AmongUsClient.Instance?.AmHost == true)
        {
            var currentTab = GameOptionsMenuStartPatch.currentTabs.FirstOrDefault(x => x.active)
                .GetComponent<GameOptionsMenu>();
            if (currentTab != null)
            {
                var optionType = options.First(x => x.optionBehaviour == currentTab.Children[0]).type;
                GameOptionsMenuStartPatch.updateGameOptionsMenu(optionType, currentTab);
            }
        }
    }

    public static byte[] serializeOptions()
    {
        using (var memoryStream = new MemoryStream())
        {
            using (var binaryWriter = new BinaryWriter(memoryStream))
            {
                var lastId = -1;
                foreach (var option in options.OrderBy(x => x.id))
                {
                    if (option.id == 0) continue;
                    var consecutive = lastId + 1 == option.id;
                    lastId = option.id;

                    binaryWriter.Write((byte)(option.selection + (consecutive ? 128 : 0)));
                    if (!consecutive) binaryWriter.Write((ushort)option.id);
                }

                binaryWriter.Flush();
                memoryStream.Position = 0L;
                return memoryStream.ToArray();
            }
        }
    }

    public static int deserializeOptions(byte[] inputValues)
    {
        var reader = new BinaryReader(new MemoryStream(inputValues));
        var lastId = -1;
        var somethingApplied = false;
        var errors = 0;
        while (reader.BaseStream.Position < inputValues.Length)
            try
            {
                int selection = reader.ReadByte();
                var id = -1;
                var consecutive = selection >= 128;
                if (consecutive)
                {
                    selection -= 128;
                    id = lastId + 1;
                }
                else
                {
                    id = reader.ReadUInt16();
                }

                if (id == 0) continue;
                lastId = id;
                var option = options.First(option => option.id == id);
                option.entry = TheOtherRolesPlugin.Instance.Config.Bind($"Preset{preset}", option.id.ToString(),
                    option.defaultSelection);
                option.selection = selection;
                if (option.optionBehaviour != null && option.optionBehaviour is StringOption stringOption)
                {
                    stringOption.oldValue = stringOption.Value = option.selection;
                    stringOption.ValueText.text = option.getString();
                }

                somethingApplied = true;
            }
            catch (Exception e)
            {
                TheOtherRolesPlugin.Logger.LogWarning(
                    $"id:{lastId}:{e}: while deserializing - tried to paste invalid settings!");
                errors++;
            }

        return Convert.ToInt32(somethingApplied) + (errors > 0 ? 0 : 1);
    }

    // Copy to or paste from clipboard (as string)
    public static void copyToClipboard()
    {
        GUIUtility.systemCopyBuffer =
            $"{TheOtherRolesPlugin.VersionString}!{Convert.ToBase64String(serializeOptions())}!{vanillaSettings.Value}";
    }

    public static int pasteFromClipboard()
    {
        var allSettings = GUIUtility.systemCopyBuffer;
        var torOptionsFine = 0;
        var vanillaOptionsFine = false;
        try
        {
            var settingsSplit = allSettings.Split("!");
            var versionInfo = Version.Parse(settingsSplit[0]);
            var torSettings = settingsSplit[1];
            var vanillaSettingsSub = settingsSplit[2];
            torOptionsFine = deserializeOptions(Convert.FromBase64String(torSettings));
            ShareOptionSelections();
            if (TheOtherRolesPlugin.Version > versionInfo && versionInfo < Version.Parse("2.0.0"))
            {
                vanillaOptionsFine = false;
                FastDestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer,
                    "pastingVanillaFailed".Translate());
            }
            else
            {
                vanillaSettings.Value = vanillaSettingsSub;
                vanillaOptionsFine = loadVanillaOptions();
            }
        }
        catch (Exception e)
        {
            TheOtherRolesPlugin.Logger.LogWarning($"{e}: tried to paste invalid settings!\n{allSettings}");
            var errorStr = allSettings.Length > 2 ? allSettings.Substring(0, 3) : "emptyClipboard".Translate();
            FastDestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer,
                string.Format("pastingInvalidSettings".Translate(), errorStr));
            SoundEffectsManager.Load();
            SoundEffectsManager.play("fail");
        }

        return Convert.ToInt32(vanillaOptionsFine) + torOptionsFine;
    }
}

[HarmonyPatch(typeof(GameSettingMenu), nameof(GameSettingMenu.ChangeTab))]
internal class GameOptionsMenuChangeTabPatch
{
    public static void Postfix(GameSettingMenu __instance, int tabNum, bool previewOnly)
    {
        if (previewOnly) return;
        foreach (var tab in GameOptionsMenuStartPatch.currentTabs)
            if (tab != null)
                tab.SetActive(false);
        foreach (var pbutton in GameOptionsMenuStartPatch.currentButtons) pbutton.SelectButton(false);
        if (tabNum > 2)
        {
            tabNum -= 3;
            GameOptionsMenuStartPatch.currentTabs[tabNum].SetActive(true);
            GameOptionsMenuStartPatch.currentButtons[tabNum].SelectButton(true);
        }
    }
}

[HarmonyPatch(typeof(LobbyViewSettingsPane), nameof(LobbyViewSettingsPane.SetTab))]
internal class LobbyViewSettingsPaneRefreshTabPatch
{
    public static bool Prefix(LobbyViewSettingsPane __instance)
    {
        if ((int)__instance.currentTab < 15)
        {
            LobbyViewSettingsPaneChangeTabPatch.Postfix(__instance, __instance.currentTab);
            return false;
        }

        return true;
    }
}

[HarmonyPatch(typeof(LobbyViewSettingsPane), nameof(LobbyViewSettingsPane.ChangeTab))]
internal class LobbyViewSettingsPaneChangeTabPatch
{
    public static void Postfix(LobbyViewSettingsPane __instance, StringNames category)
    {
        var tabNum = (int)category;

        foreach (var pbutton in LobbyViewSettingsPatch.currentButtons) pbutton.SelectButton(false);
        if (tabNum > 20) // StringNames are in the range of 3000+ 
            return;
        __instance.taskTabButton.SelectButton(false);

        if (tabNum > 2)
        {
            tabNum -= 3;
            //GameOptionsMenuStartPatch.currentTabs[tabNum].SetActive(true);
            LobbyViewSettingsPatch.currentButtons[tabNum].SelectButton(true);
            LobbyViewSettingsPatch.drawTab(__instance, LobbyViewSettingsPatch.currentButtonTypes[tabNum]);
        }
    }
}

[HarmonyPatch(typeof(LobbyViewSettingsPane), nameof(LobbyViewSettingsPane.Update))]
internal class LobbyViewSettingsPaneUpdatePatch
{
    public static void Postfix(LobbyViewSettingsPane __instance)
    {
        if (LobbyViewSettingsPatch.currentButtons.Count == 0)
        {
            LobbyViewSettingsPatch.gameModeChangedFlag = true;
            LobbyViewSettingsPatch.Postfix(__instance);
        }
    }
}

[HarmonyPatch(typeof(LobbyViewSettingsPane), nameof(LobbyViewSettingsPane.Awake))]
internal class LobbyViewSettingsPatch
{
    public static List<PassiveButton> currentButtons = new();
    public static List<CustomOptionType> currentButtonTypes = new();
    public static bool gameModeChangedFlag;

    public static void createCustomButton(LobbyViewSettingsPane __instance, int targetMenu, string buttonName,
        string buttonText, CustomOptionType optionType)
    {
        buttonName = "View" + buttonName;
        var buttonTemplate = GameObject.Find("OverviewTab");
        var torSettingsButton = GameObject.Find(buttonName);
        if (torSettingsButton == null)
        {
            torSettingsButton = GameObject.Instantiate(buttonTemplate, buttonTemplate.transform.parent);
            torSettingsButton.transform.localPosition += Vector3.right * 1.75f * (targetMenu - 2);
            torSettingsButton.name = buttonName;
            __instance.StartCoroutine(Effects.Lerp(2f,
                new Action<float>(p =>
                {
                    torSettingsButton.transform.FindChild("FontPlacer").GetComponentInChildren<TextMeshPro>().text =
                        buttonText.Translate();
                })));
            var torSettingsPassiveButton = torSettingsButton.GetComponent<PassiveButton>();
            torSettingsPassiveButton.OnClick.RemoveAllListeners();
            torSettingsPassiveButton.OnClick.AddListener((Action)(() =>
            {
                __instance.ChangeTab((StringNames)targetMenu);
            }));
            torSettingsPassiveButton.OnMouseOut.RemoveAllListeners();
            torSettingsPassiveButton.OnMouseOver.RemoveAllListeners();
            torSettingsPassiveButton.SelectButton(false);
            currentButtons.Add(torSettingsPassiveButton);
            currentButtonTypes.Add(optionType);
        }
    }

    public static void Postfix(LobbyViewSettingsPane __instance)
    {
        currentButtons.ForEach(x => x?.Destroy());
        currentButtons.Clear();
        currentButtonTypes.Clear();

        removeVanillaTabs(__instance);

        createSettingTabs(__instance);
    }

    public static void removeVanillaTabs(LobbyViewSettingsPane __instance)
    {
        GameObject.Find("RolesTabs")?.Destroy();
        var overview = GameObject.Find("OverviewTab");
        if (!gameModeChangedFlag)
        {
            overview.transform.localScale = new Vector3(0.5f * overview.transform.localScale.x,
                overview.transform.localScale.y, overview.transform.localScale.z);
            overview.transform.localPosition += new Vector3(-1.2f, 0f, 0f);
        }

        overview.transform.Find("FontPlacer").transform.localScale = new Vector3(1.35f, 1f, 1f);
        overview.transform.Find("FontPlacer").transform.localPosition = new Vector3(-0.6f, -0.1f, 0f);
        gameModeChangedFlag = false;
    }

    public static void drawTab(LobbyViewSettingsPane __instance, CustomOptionType optionType)
    {
        var relevantOptions = options.Where(x =>
                x.type == optionType || (x.type == CustomOptionType.Guesser && optionType == CustomOptionType.General))
            .ToList();

        if ((int)optionType == 99)
        {
            // Create 4 Groups with Role settings only
            relevantOptions.Clear();
            relevantOptions.AddRange(options.Where(x => x.type == CustomOptionType.Impostor && x.isHeader));
            relevantOptions.AddRange(options.Where(x => x.type == CustomOptionType.Neutral && x.isHeader));
            relevantOptions.AddRange(options.Where(x => x.type == CustomOptionType.Crewmate && x.isHeader));
            relevantOptions.AddRange(options.Where(x => x.type == CustomOptionType.Modifier && x.isHeader));
            foreach (var option in options)
                if (option.parent != null && option.parent.getSelection() > 0)
                {
                    if (option.id2 == 1) //Deputy
                        relevantOptions.Insert(relevantOptions.IndexOf(CustomOptionHolder.sheriffSpawnRate) + 1,
                            option);
                    else if (option.id2 == 2) //Sidekick
                        relevantOptions.Insert(relevantOptions.IndexOf(CustomOptionHolder.jackalSpawnRate) + 1, option);
                    else if (option.id2 == 3) //Prosecutor
                        relevantOptions.Insert(relevantOptions.IndexOf(CustomOptionHolder.lawyerSpawnRate) + 1, option);
                }
        }

        if (TORMapOptions.gameMode == CustomGamemodes.Guesser) // Exclude guesser options in neutral mode
            relevantOptions = relevantOptions
                .Where(x => !new List<int> { 310, 311, 312, 313, 314, 315, 316, 317, 318 }.Contains(x.id)).ToList();

        for (var j = 0; j < __instance.settingsInfo.Count; j++) __instance.settingsInfo[j].gameObject.Destroy();
        __instance.settingsInfo.Clear();

        var num = 1.44f;
        var i = 0;
        var singles = 1;
        var headers = 0;
        var lines = 0;
        var curType = CustomOptionType.Modifier;
        var numBonus = 0;

        foreach (var option in relevantOptions)
        {
            if ((option.isHeader && (int)optionType != 99) || ((int)optionType == 99 && curType != option.type))
            {
                curType = option.type;
                if (i != 0)
                {
                    num -= 0.85f;
                    numBonus++;
                }

                if (i % 2 != 0) singles++;
                headers++; // for header
                var categoryHeaderMasked = Object.Instantiate(__instance.categoryHeaderOrigin);
                categoryHeaderMasked.SetHeader(StringNames.ImpostorsCategory, 61);
                categoryHeaderMasked.Title.text = option.getHeading() != "" ? option.getHeading() : option.getName();
                if ((int)optionType == 99)
                    categoryHeaderMasked.Title.text = new Dictionary<CustomOptionType, string>
                    {
                        { CustomOptionType.Impostor, "categoryHeaderMaskedImp".Translate() }, { CustomOptionType.Neutral, "categoryHeaderMaskedNeut".Translate() },
                        { CustomOptionType.Crewmate, "categoryHeaderMaskedCrew".Translate() }, { CustomOptionType.Modifier, "categoryHeaderMaskedMod".Translate() }
                    }[curType];
                //categoryHeaderMasked.Title.outlineColor = Color.white;
                //categoryHeaderMasked.Title.outlineWidth = 0.2f;
                categoryHeaderMasked.transform.SetParent(__instance.settingsContainer);
                categoryHeaderMasked.transform.localScale = Vector3.one;
                categoryHeaderMasked.transform.localPosition = new Vector3(-9.77f, num, -2f);
                __instance.settingsInfo.Add(categoryHeaderMasked.gameObject);
                num -= 1.05f;
                i = 0;
            }
            else if (option.parent != null && (option.parent.selection == 0 ||
                                               (option.parent.parent != null && option.parent.parent.selection == 0)))
            {
                continue; // Hides options, for which the parent is disabled!
            }

            if (option == CustomOptionHolder.crewmateRolesCountMax ||
                option == CustomOptionHolder.neutralRolesCountMax ||
                option == CustomOptionHolder.impostorRolesCountMax || option == CustomOptionHolder.modifiersCountMax ||
                option == CustomOptionHolder.crewmateRolesFill)
                continue;

            var viewSettingsInfoPanel = Object.Instantiate(__instance.infoPanelOrigin);
            viewSettingsInfoPanel.transform.SetParent(__instance.settingsContainer);
            viewSettingsInfoPanel.transform.localScale = Vector3.one;
            float num2;
            if (i % 2 == 0)
            {
                lines++;
                num2 = -8.95f;
                if (i > 0) num -= 0.85f;
            }
            else
            {
                num2 = -3f;
            }

            viewSettingsInfoPanel.transform.localPosition = new Vector3(num2, num, -2f);
            var value = option.getSelection();
            var settingTuple = handleSpecialOptionsView(option, option.getName(), option.getString());
            viewSettingsInfoPanel.SetInfo(StringNames.ImpostorsCategory, settingTuple.Item2, 61);
            viewSettingsInfoPanel.titleText.text = settingTuple.Item1;
            if (option.isHeader && (int)optionType != 99 && option.getHeading() == "" &&
                (option.type == CustomOptionType.Neutral || option.type == CustomOptionType.Crewmate ||
                 option.type == CustomOptionType.Impostor ||
                 option.type == CustomOptionType.Modifier)) viewSettingsInfoPanel.titleText.text = "spawnChance".Translate();
            if ((int)optionType == 99)
                //viewSettingsInfoPanel.titleText.outlineColor = Color.white;
                //viewSettingsInfoPanel.titleText.outlineWidth = 0.2f;
                if (option.type == CustomOptionType.Modifier)
                    viewSettingsInfoPanel.settingText.text = viewSettingsInfoPanel.settingText.text +
                                                             LegacyGameOptionsPatch.buildModifierExtras(option);

            __instance.settingsInfo.Add(viewSettingsInfoPanel.gameObject);

            i++;
        }

        var actual_spacing = (headers * 1.05f + lines * 0.85f) / (headers + lines) * 1.01f;
        __instance.scrollBar.CalculateAndSetYBounds(__instance.settingsInfo.Count + singles * 2 + headers, 2f, 5f,
            actual_spacing);
    }

    private static Tuple<string, string> handleSpecialOptionsView(CustomOption option, string defaultString,
        string defaultVal)
    {
        var name = defaultString;
        var val = defaultVal;
        if (option == CustomOptionHolder.crewmateRolesCountMin)
        {
            val = "";
            name = "categoryHeaderMaskedCrew";
            var min = CustomOptionHolder.crewmateRolesCountMin.getSelection();
            var max = CustomOptionHolder.crewmateRolesCountMax.getSelection();
            if (CustomOptionHolder.crewmateRolesFill.getBool())
            {
                var crewCount = PlayerControl.AllPlayerControls.Count -
                                GameOptionsManager.Instance.currentGameOptions.NumImpostors;
                var minNeutral = CustomOptionHolder.neutralRolesCountMin.getSelection();
                var maxNeutral = CustomOptionHolder.neutralRolesCountMax.getSelection();
                if (minNeutral > maxNeutral) minNeutral = maxNeutral;
                min = crewCount - maxNeutral;
                max = crewCount - minNeutral;
                if (min < 0) min = 0;
                if (max < 0) max = 0;
                val = "specialOptionsViewCrew";
            }

            if (min > max) min = max;
            val += min == max ? $"{max}" : $"{min} - {max}";
        }

        if (option == CustomOptionHolder.neutralRolesCountMin)
        {
            name = "categoryHeaderMaskedNeut";
            var min = CustomOptionHolder.neutralRolesCountMin.getSelection();
            var max = CustomOptionHolder.neutralRolesCountMax.getSelection();
            if (min > max) min = max;
            val = min == max ? $"{max}" : $"{min} - {max}";
        }

        if (option == CustomOptionHolder.impostorRolesCountMin)
        {
            name = "categoryHeaderMaskedImp";
            var min = CustomOptionHolder.impostorRolesCountMin.getSelection();
            var max = CustomOptionHolder.impostorRolesCountMax.getSelection();
            if (max > GameOptionsManager.Instance.currentGameOptions.NumImpostors)
                max = GameOptionsManager.Instance.currentGameOptions.NumImpostors;
            if (min > max) min = max;
            val = min == max ? $"{max}" : $"{min} - {max}";
        }

        if (option == CustomOptionHolder.modifiersCountMin)
        {
            name = "categoryHeaderMaskedMod";
            var min = CustomOptionHolder.modifiersCountMin.getSelection();
            var max = CustomOptionHolder.modifiersCountMax.getSelection();
            if (min > max) min = max;
            val = min == max ? $"{max}" : $"{min} - {max}";
        }

        return new Tuple<string, string>(name.Translate(), val.Translate());
    }

    public static void createSettingTabs(LobbyViewSettingsPane __instance)
    {
        // Handle different gamemodes and tabs needed therein.
        var next = 3;
        if (TORMapOptions.gameMode == CustomGamemodes.Guesser || TORMapOptions.gameMode == CustomGamemodes.Classic)
        {
            // create TOR settings
            createCustomButton(__instance, next++, "TORSettings", "TORSettings", CustomOptionType.General);
            // create TOR settings
            createCustomButton(__instance, next++, "RoleOverview", "RoleOverview", (CustomOptionType)99);
            // IMp
            createCustomButton(__instance, next++, "ImpostorSettings", "categoryHeaderMaskedImp", CustomOptionType.Impostor);

            // Neutral
            createCustomButton(__instance, next++, "NeutralSettings", "categoryHeaderMaskedNeut", CustomOptionType.Neutral);
            // Crew
            createCustomButton(__instance, next++, "CrewmateSettings", "categoryHeaderMaskedCrew", CustomOptionType.Crewmate);
            // Modifier
            createCustomButton(__instance, next++, "ModifierSettings", "categoryHeaderMaskedMod", CustomOptionType.Modifier);
        }
        else if (TORMapOptions.gameMode == CustomGamemodes.HideNSeek)
        {
            // create Main HNS settings
            createCustomButton(__instance, next++, "HideNSeekMain", "HideNSeekMain", CustomOptionType.HideNSeekMain);
            // create HNS Role settings
            createCustomButton(__instance, next++, "HideNSeekRoles", "HideNSeekRoles",
                CustomOptionType.HideNSeekRoles);
        }
        else if (TORMapOptions.gameMode == CustomGamemodes.PropHunt)
        {
            createCustomButton(__instance, next++, "PropHunt", "PropHuntSetting", CustomOptionType.PropHunt);
        }
    }
}

[HarmonyPatch(typeof(GameOptionsMenu), nameof(GameOptionsMenu.CreateSettings))]
internal class GameOptionsMenuCreateSettingsPatch
{
    public static void Postfix(GameOptionsMenu __instance)
    {
        if (__instance.gameObject.name == "GAME SETTINGS TAB")
            adaptTaskCount(__instance);
    }

    private static void adaptTaskCount(GameOptionsMenu __instance)
    {
        // Adapt task count for main options
        var commonTasksOption = __instance.Children.ToArray()
            .FirstOrDefault(x => x.TryCast<NumberOption>()?.intOptionName == Int32OptionNames.NumCommonTasks)
            .Cast<NumberOption>();
        if (commonTasksOption != null) commonTasksOption.ValidRange = new FloatRange(0f, 4f);
        var shortTasksOption = __instance.Children.ToArray()
            .FirstOrDefault(x => x.TryCast<NumberOption>()?.intOptionName == Int32OptionNames.NumShortTasks)
            .TryCast<NumberOption>();
        if (shortTasksOption != null) shortTasksOption.ValidRange = new FloatRange(0f, 23f);
        var longTasksOption = __instance.Children.ToArray()
            .FirstOrDefault(x => x.TryCast<NumberOption>()?.intOptionName == Int32OptionNames.NumLongTasks)
            .TryCast<NumberOption>();
        if (longTasksOption != null) longTasksOption.ValidRange = new FloatRange(0f, 15f);
    }
}

[HarmonyPatch(typeof(GameSettingMenu), nameof(GameSettingMenu.Start))]
internal class GameOptionsMenuStartPatch
{
    public static List<GameObject> currentTabs = new();
    public static List<PassiveButton> currentButtons = new();
    public static Dictionary<byte, GameOptionsMenu> currentGOMs = new();

    public static void Postfix(GameSettingMenu __instance)
    {
        currentTabs.ForEach(x => x?.Destroy());
        currentButtons.ForEach(x => x?.Destroy());
        currentTabs = new List<GameObject>();
        currentButtons = new List<PassiveButton>();
        currentGOMs.Clear();

        if (GameOptionsManager.Instance.currentGameOptions.GameMode == GameModes.HideNSeek) return;

        removeVanillaTabs(__instance);

        createSettingTabs(__instance);

        var GOMGameObject = GameObject.Find("GAME SETTINGS TAB");


        // create copy to clipboard and paste from clipboard buttons.
        var template = GameObject.Find("PlayerOptionsMenu(Clone)").transform.Find("CloseButton").gameObject;
        var holderGO = new GameObject("copyPasteButtonParent");
        var bgrenderer = holderGO.AddComponent<SpriteRenderer>();
        bgrenderer.sprite = Helpers.loadSpriteFromResources("CopyPasteBG.png", 175f);
        holderGO.transform.SetParent(template.transform.parent, false);
        holderGO.transform.localPosition = template.transform.localPosition + new Vector3(-8.3f, 0.73f, -2f);
        holderGO.layer = template.layer;
        holderGO.SetActive(true);
        var copyButton = GameObject.Instantiate(template, holderGO.transform);
        copyButton.transform.localPosition = new Vector3(-0.3f, 0.02f, -2f);
        var copyButtonPassive = copyButton.GetComponent<PassiveButton>();
        var copyButtonRenderer = copyButton.GetComponentInChildren<SpriteRenderer>();
        var copyButtonActiveRenderer = copyButton.transform.GetChild(1).GetComponent<SpriteRenderer>();
        copyButtonRenderer.sprite = Helpers.loadSpriteFromResources("Copy.png", 100f);
        copyButton.transform.GetChild(1).transform.localPosition = Vector3.zero;
        copyButtonActiveRenderer.sprite =
            Helpers.loadSpriteFromResources("CopyActive.png", 100f);
        copyButtonPassive.OnClick.RemoveAllListeners();
        copyButtonPassive.OnClick = new Button.ButtonClickedEvent();
        copyButtonPassive.OnClick.AddListener((Action)(() =>
        {
            copyToClipboard();
            copyButtonRenderer.color = Color.green;
            copyButtonActiveRenderer.color = Color.green;
            __instance.StartCoroutine(Effects.Lerp(1f, new Action<float>(p =>
            {
                if (p > 0.95)
                {
                    copyButtonRenderer.color = Color.white;
                    copyButtonActiveRenderer.color = Color.white;
                }
            })));
        }));
        var pasteButton = GameObject.Instantiate(template, holderGO.transform);
        pasteButton.transform.localPosition = new Vector3(0.3f, 0.02f, -2f);
        var pasteButtonPassive = pasteButton.GetComponent<PassiveButton>();
        var pasteButtonRenderer = pasteButton.GetComponentInChildren<SpriteRenderer>();
        var pasteButtonActiveRenderer = pasteButton.transform.GetChild(1).GetComponent<SpriteRenderer>();
        pasteButtonRenderer.sprite = Helpers.loadSpriteFromResources("Paste.png", 100f);
        pasteButtonActiveRenderer.sprite =
            Helpers.loadSpriteFromResources("PasteActive.png", 100f);
        pasteButtonPassive.OnClick.RemoveAllListeners();
        pasteButtonPassive.OnClick = new Button.ButtonClickedEvent();
        pasteButtonPassive.OnClick.AddListener((Action)(() =>
        {
            pasteButtonRenderer.color = Color.yellow;
            var success = pasteFromClipboard();
            pasteButtonRenderer.color = success == 3 ? Color.green : success == 0 ? Color.red : Color.yellow;
            pasteButtonActiveRenderer.color = success == 3 ? Color.green : success == 0 ? Color.red : Color.yellow;
            __instance.StartCoroutine(Effects.Lerp(1f, new Action<float>(p =>
            {
                if (p > 0.95)
                {
                    pasteButtonRenderer.color = Color.white;
                    pasteButtonActiveRenderer.color = Color.white;
                }
            })));
        }));
    }

    private static void createSettings(GameOptionsMenu menu, List<CustomOption> options)
    {
        var num = 1.5f;
        foreach (var option in options)
        {
            if (option.isHeader)
            {
                var categoryHeaderMasked = Object.Instantiate(menu.categoryHeaderOrigin, Vector3.zero,
                    Quaternion.identity, menu.settingsContainer);
                categoryHeaderMasked.SetHeader(StringNames.ImpostorsCategory, 20);
                categoryHeaderMasked.Title.text = option.getHeading() != "" ? option.getHeading() : option.getName();
                //categoryHeaderMasked.Title.outlineColor = Color.white;
                //categoryHeaderMasked.Title.outlineWidth = 0.2f;
                categoryHeaderMasked.transform.localScale = Vector3.one * 0.63f;
                categoryHeaderMasked.transform.localPosition = new Vector3(-0.903f, num, -2f);
                num -= 0.63f;
            }
            else if (option.parent != null && ((option.parent.selection == 0 && !option.invertedParent) ||
                                               (option.parent.parent != null && option.parent.parent.selection == 0 &&
                                                !option.parent.invertedParent)))
            {
                continue; // Hides options, for which the parent is disabled!
            }
            else if (option.parent != null && option.parent.selection != 0 && option.invertedParent)
            {
                continue;
            }

            OptionBehaviour optionBehaviour = Object.Instantiate(menu.stringOptionOrigin, Vector3.zero,
                Quaternion.identity, menu.settingsContainer);
            optionBehaviour.transform.localPosition = new Vector3(0.952f, num, -2f);
            optionBehaviour.SetClickMask(menu.ButtonClickMask);

            // "SetUpFromData"
            SpriteRenderer[] componentsInChildren = optionBehaviour.GetComponentsInChildren<SpriteRenderer>(true);
            for (var i = 0; i < componentsInChildren.Length; i++)
                componentsInChildren[i].material.SetInt(PlayerMaterial.MaskLayer, 20);
            foreach (var textMeshPro in optionBehaviour.GetComponentsInChildren<TextMeshPro>(true))
            {
                textMeshPro.fontMaterial.SetFloat("_StencilComp", 3f);
                textMeshPro.fontMaterial.SetFloat("_Stencil", 20);
            }

            var stringOption = optionBehaviour as StringOption;
            stringOption.OnValueChanged = new Action<OptionBehaviour>(o => { });
            stringOption.TitleText.text = option.getName();
            if (option.isHeader && option.getHeading() == "" && (option.type == CustomOptionType.Neutral ||
                                                            option.type == CustomOptionType.Crewmate ||
                                                            option.type == CustomOptionType.Impostor ||
                                                            option.type == CustomOptionType.Modifier))
                stringOption.TitleText.text = "spawnChance".Translate();
            if (stringOption.TitleText.text.Length > 25)
                stringOption.TitleText.fontSize = 2.2f;
            if (stringOption.TitleText.text.Length > 40)
                stringOption.TitleText.fontSize = 2f;
            stringOption.Value = stringOption.oldValue = option.selection;
            stringOption.ValueText.text = option.getString();
            option.optionBehaviour = stringOption;

            menu.Children.Add(optionBehaviour);
            num -= 0.45f;
            menu.scrollBar.SetYBoundsMax(-num - 1.65f);
        }

        for (var i = 0; i < menu.Children.Count; i++)
        {
            OptionBehaviour optionBehaviour = menu.Children[i];
            if (AmongUsClient.Instance && !AmongUsClient.Instance.AmHost) optionBehaviour.SetAsPlayer();
        }
    }

    private static void removeVanillaTabs(GameSettingMenu __instance)
    {
        GameObject.Find("What Is This?")?.Destroy();
        GameObject.Find("GamePresetButton")?.Destroy();
        GameObject.Find("RoleSettingsButton")?.Destroy();
        __instance.ChangeTab(1, false);
    }

    public static void createCustomButton(GameSettingMenu __instance, int targetMenu, string buttonName,
        string buttonText)
    {
        var leftPanel = GameObject.Find("LeftPanel");
        var buttonTemplate = GameObject.Find("GameSettingsButton");
        if (targetMenu == 3)
        {
            buttonTemplate.transform.localPosition -= Vector3.up * 0.85f;
            buttonTemplate.transform.localScale *= Vector2.one * 0.75f;
        }

        var torSettingsButton = GameObject.Find(buttonName);
        if (torSettingsButton == null)
        {
            torSettingsButton = GameObject.Instantiate(buttonTemplate, leftPanel.transform);
            torSettingsButton.transform.localPosition += Vector3.up * 0.5f * (targetMenu - 2);
            torSettingsButton.name = buttonName;
            __instance.StartCoroutine(Effects.Lerp(2f,
                new Action<float>(p =>
                {
                    torSettingsButton.transform.FindChild("FontPlacer").GetComponentInChildren<TextMeshPro>().text =
                        buttonText.Translate();
                })));
            var torSettingsPassiveButton = torSettingsButton.GetComponent<PassiveButton>();
            torSettingsPassiveButton.OnClick.RemoveAllListeners();
            torSettingsPassiveButton.OnClick.AddListener((Action)(() => { __instance.ChangeTab(targetMenu, false); }));
            torSettingsPassiveButton.OnMouseOut.RemoveAllListeners();
            torSettingsPassiveButton.OnMouseOver.RemoveAllListeners();
            torSettingsPassiveButton.SelectButton(false);
            currentButtons.Add(torSettingsPassiveButton);
        }
    }

    public static void createGameOptionsMenu(GameSettingMenu __instance, CustomOptionType optionType,
        string settingName)
    {
        var tabTemplate = GameObject.Find("GAME SETTINGS TAB");
        currentTabs.RemoveAll(x => x == null);

        var torSettingsTab = GameObject.Instantiate(tabTemplate, tabTemplate.transform.parent);
        torSettingsTab.name = settingName;

        var torSettingsGOM = torSettingsTab.GetComponent<GameOptionsMenu>();

        updateGameOptionsMenu(optionType, torSettingsGOM);

        currentTabs.Add(torSettingsTab);
        torSettingsTab.SetActive(false);
        currentGOMs.Add((byte)optionType, torSettingsGOM);
    }

    public static void updateGameOptionsMenu(CustomOptionType optionType, GameOptionsMenu torSettingsGOM)
    {
        foreach (var child in torSettingsGOM.Children) child.Destroy();
        torSettingsGOM.scrollBar.transform.FindChild("SliderInner").DestroyChildren();
        torSettingsGOM.Children.Clear();
        var relevantOptions = options.Where(x => x.type == optionType).ToList();
        if (TORMapOptions.gameMode == CustomGamemodes.Guesser) // Exclude guesser options in neutral mode
            relevantOptions = relevantOptions
                .Where(x => !new List<int> { 310, 311, 312, 313, 314, 315, 316, 317, 318 }.Contains(x.id)).ToList();
        createSettings(torSettingsGOM, relevantOptions);
    }

    private static void createSettingTabs(GameSettingMenu __instance)
    {
        // Handle different gamemodes and tabs needed therein.
        var next = 3;
        if (TORMapOptions.gameMode == CustomGamemodes.Guesser || TORMapOptions.gameMode == CustomGamemodes.Classic)
        {
            // create TOR settings
            createCustomButton(__instance, next++, "TORSettings", "TORSettings");
            createGameOptionsMenu(__instance, CustomOptionType.General, "TORSettings");
            // Guesser if applicable
            if (TORMapOptions.gameMode == CustomGamemodes.Guesser)
            {
                createCustomButton(__instance, next++, "GuesserSettings", "GuesserSettingsText");
                createGameOptionsMenu(__instance, CustomOptionType.Guesser, "GuesserSettings");
            }

            // IMp
            createCustomButton(__instance, next++, "ImpostorSettings", "ImpostorSettingsText");
            createGameOptionsMenu(__instance, CustomOptionType.Impostor, "ImpostorSettings");

            // Neutral
            createCustomButton(__instance, next++, "NeutralSettings", "NeutralSettingsText");
            createGameOptionsMenu(__instance, CustomOptionType.Neutral, "NeutralSettings");
            // Crew
            createCustomButton(__instance, next++, "CrewmateSettings", "CrewmateSettingsText");
            createGameOptionsMenu(__instance, CustomOptionType.Crewmate, "CrewmateSettings");
            // Modifier
            createCustomButton(__instance, next++, "ModifierSettings", "ModifierSettingsText");
            createGameOptionsMenu(__instance, CustomOptionType.Modifier, "ModifierSettings");
        }
        else if (TORMapOptions.gameMode == CustomGamemodes.HideNSeek)
        {
            // create Main HNS settings
            createCustomButton(__instance, next++, "HideNSeekMain", "HideNSeekMain");
            createGameOptionsMenu(__instance, CustomOptionType.HideNSeekMain, "HideNSeekMain");
            // create HNS Role settings
            createCustomButton(__instance, next++, "HideNSeekRoles", "HideNSeekRoles");
            createGameOptionsMenu(__instance, CustomOptionType.HideNSeekRoles, "HideNSeekRoles");
        }
        else if (TORMapOptions.gameMode == CustomGamemodes.PropHunt)
        {
            createCustomButton(__instance, next++, "PropHunt", "PropHuntSetting");
            createGameOptionsMenu(__instance, CustomOptionType.PropHunt, "PropHuntSetting");
        }
    }
}

[HarmonyPatch(typeof(StringOption), nameof(StringOption.Initialize))]
public class StringOptionEnablePatch
{
    public static bool Prefix(StringOption __instance)
    {
        var option = options.FirstOrDefault(option => option.optionBehaviour == __instance);
        if (option == null) return true;

        __instance.OnValueChanged = new Action<OptionBehaviour>(o => { });
        //__instance.TitleText.text = option.getName();
        __instance.Value = __instance.oldValue = option.selection;
        __instance.ValueText.text = option.getString();

        return false;
    }
}

[HarmonyPatch(typeof(StringOption), nameof(StringOption.Increase))]
public class StringOptionIncreasePatch
{
    public static bool Prefix(StringOption __instance)
    {
        var option = options.FirstOrDefault(option => option.optionBehaviour == __instance);
        if (option == null) return true;
        option.updateSelection(option.selection + 1);
        if (CustomOptionHolder.isMapSelectionOption(option))
        {
            var currentGameOptions = GameOptionsManager.Instance.CurrentGameOptions;
            currentGameOptions.SetByte(ByteOptionNames.MapId, (byte)option.selection);
            GameOptionsManager.Instance.GameHostOptions = GameOptionsManager.Instance.CurrentGameOptions;
            GameManager.Instance.LogicOptions.SyncOptions();
        }

        return false;
    }
}

[HarmonyPatch(typeof(StringOption), nameof(StringOption.Decrease))]
public class StringOptionDecreasePatch
{
    public static bool Prefix(StringOption __instance)
    {
        var option = options.FirstOrDefault(option => option.optionBehaviour == __instance);
        if (option == null) return true;
        option.updateSelection(option.selection - 1);
        if (CustomOptionHolder.isMapSelectionOption(option))
        {
            var currentGameOptions = GameOptionsManager.Instance.CurrentGameOptions;
            currentGameOptions.SetByte(ByteOptionNames.MapId, (byte)option.selection);
            GameOptionsManager.Instance.GameHostOptions = GameOptionsManager.Instance.CurrentGameOptions;
            GameManager.Instance.LogicOptions.SyncOptions();
        }

        return false;
    }
}

[HarmonyPatch(typeof(StringOption), nameof(StringOption.FixedUpdate))]
public class StringOptionFixedUpdate
{
    public static void Postfix(StringOption __instance)
    {
        if (!IL2CPPChainloader.Instance.Plugins.TryGetValue("com.DigiWorm.LevelImposter", out _)) return;
        var option = options.FirstOrDefault(option => option.optionBehaviour == __instance);
        if (option == null || !CustomOptionHolder.isMapSelectionOption(option)) return;
        if (GameOptionsManager.Instance.CurrentGameOptions.MapId == 6)
            if (option.optionBehaviour != null && option.optionBehaviour is StringOption stringOption)
            {
                stringOption.ValueText.text = option.getString();
            }
            else if (option.optionBehaviour != null && option.optionBehaviour is StringOption stringOptionToo)
            {
                stringOptionToo.oldValue = stringOptionToo.Value = option.selection;
                stringOptionToo.ValueText.text = option.getString();
            }
    }
}

[HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.RpcSyncSettings))]
public class RpcSyncSettingsPatch
{
    public static void Postfix()
    {
        //CustomOption.ShareOptionSelections();
        saveVanillaOptions();
    }
}

[HarmonyPatch(typeof(PlayerPhysics), nameof(PlayerPhysics.CoSpawnPlayer))]
public class AmongUsClientOnPlayerJoinedPatch
{
    public static void Postfix()
    {
        if (PlayerControl.LocalPlayer != null && AmongUsClient.Instance.AmHost)
        {
            GameManager.Instance.LogicOptions.SyncOptions();
            ShareOptionSelections();
        }
    }
}

[HarmonyPatch]
internal class LegacyGameOptionsPatch
{
    public static int maxPage = 7;

    private static string buildRoleOptions()
    {
        var impRoles = buildOptionsOfType(CustomOptionType.Impostor, true) + "\n";
        var neutralRoles = buildOptionsOfType(CustomOptionType.Neutral, true) + "\n";
        var crewRoles = buildOptionsOfType(CustomOptionType.Crewmate, true) + "\n";
        var modifiers = buildOptionsOfType(CustomOptionType.Modifier, true);
        return impRoles + neutralRoles + crewRoles + modifiers;
    }

    public static string buildModifierExtras(CustomOption customOption)
    {
        // find options children with quantity
        var children = options.Where(o => o.parent == customOption);
        var quantity = children.Where(o => o.getName().Contains("Quantity")).ToList();
        if (customOption.getSelection() == 0) return "";
        if (quantity.Count == 1) return $" ({quantity[0].getQuantity()})";
        if (customOption == CustomOptionHolder.modifierLover)
            return " (1 " + "buildModifierExtras".Translate() + $" {CustomOptionHolder.modifierLoverImpLoverRate.getSelection() * 10}%)";
        return "";
    }

    private static string buildOptionsOfType(CustomOptionType type, bool headerOnly)
    {
        var sb = new StringBuilder("\n");
        var options = CustomOption.options.Where(o => o.type == type);
        if (TORMapOptions.gameMode == CustomGamemodes.Guesser)
        {
            if (type == CustomOptionType.General)
                options = CustomOption.options.Where(o => o.type == type || o.type == CustomOptionType.Guesser);
            var remove = new List<int> { 308, 310, 311, 312, 313, 314, 315, 316, 317, 318 };
            options = options.Where(x => !remove.Contains(x.id));
        }
        else if (TORMapOptions.gameMode == CustomGamemodes.Classic)
        {
            options = options.Where(x =>
                !(x.type == CustomOptionType.Guesser || x == CustomOptionHolder.crewmateRolesFill));
        }
        else if (TORMapOptions.gameMode == CustomGamemodes.HideNSeek)
        {
            options = options.Where(x =>
                x.type == CustomOptionType.HideNSeekMain || x.type == CustomOptionType.HideNSeekRoles);
        }
        else if (TORMapOptions.gameMode == CustomGamemodes.PropHunt)
        {
            options = options.Where(x => x.type == CustomOptionType.PropHunt);
        }

        foreach (var option in options)
            if (option.parent == null)
            {
                var line = $"{option.getName()}: {option.getString()}";
                if (type == CustomOptionType.Modifier) line += buildModifierExtras(option);
                sb.AppendLine(line);
            }
            else if (option.parent.getSelection() > 0 || (option.invertedParent && option.parent.getSelection() == 0))
            {
                if (option.id2 == 1) //Deputy
                    sb.AppendLine(
                        $"- {Helpers.cs(Deputy.color, "deputy".Translate())}: {option.getString()}");
                else if (option.id2 == 2) //Sidekick
                    sb.AppendLine(
                        $"- {Helpers.cs(Sidekick.color, "sidekick".Translate())}: {option.getString()}");
                else if (option.id2 == 3) //Prosecutor
                    sb.AppendLine(
                        $"- {Helpers.cs(Lawyer.color, "prosecutor".Translate())}: {option.getString()}");
            }

        if (headerOnly) return sb.ToString();
        sb = new StringBuilder();

        foreach (var option in options)
        {
            if (TORMapOptions.gameMode == CustomGamemodes.HideNSeek && option.type != CustomOptionType.HideNSeekMain &&
                option.type != CustomOptionType.HideNSeekRoles) continue;
            if (TORMapOptions.gameMode == CustomGamemodes.PropHunt &&
                option.type != CustomOptionType.PropHunt) continue;
            if (option.parent != null)
            {
                var isIrrelevant = (option.parent.getSelection() == 0 && !option.invertedParent) ||
                                   (option.parent.parent != null && option.parent.parent.getSelection() == 0 &&
                                    !option.parent.invertedParent);

                var c = isIrrelevant ? Color.grey : Color.white; // No use for now
                if (isIrrelevant) continue;
                sb.AppendLine(Helpers.cs(c, $"{option.getName()}: {option.getString()}"));
            }
            else
            {
                if (option == CustomOptionHolder.crewmateRolesCountMin)
                {
                    var optionName =
                        CustomOptionHolder.cs(new Color(204f / 255f, 204f / 255f, 0, 1f), "categoryHeaderMaskedCrew".Translate());
                    var min = CustomOptionHolder.crewmateRolesCountMin.getSelection();
                    var max = CustomOptionHolder.crewmateRolesCountMax.getSelection();
                    var optionValue = "";
                    if (CustomOptionHolder.crewmateRolesFill.getBool())
                    {
                        var crewCount = PlayerControl.AllPlayerControls.Count -
                                        GameOptionsManager.Instance.currentGameOptions.NumImpostors;
                        var minNeutral = CustomOptionHolder.neutralRolesCountMin.getSelection();
                        var maxNeutral = CustomOptionHolder.neutralRolesCountMax.getSelection();
                        if (minNeutral > maxNeutral) minNeutral = maxNeutral;
                        min = crewCount - maxNeutral;
                        max = crewCount - minNeutral;
                        if (min < 0) min = 0;
                        if (max < 0) max = 0;
                        optionValue = "specialOptionsViewCrew".Translate();
                    }

                    if (min > max) min = max;
                    optionValue += min == max ? $"{max}" : $"{min} - {max}";
                    sb.AppendLine($"{optionName}: {optionValue}");
                }
                else if (option == CustomOptionHolder.neutralRolesCountMin)
                {
                    var optionName = CustomOptionHolder.cs(new Color(204f / 255f, 204f / 255f, 0, 1f), "categoryHeaderMaskedNeut".Translate());
                    var min = CustomOptionHolder.neutralRolesCountMin.getSelection();
                    var max = CustomOptionHolder.neutralRolesCountMax.getSelection();
                    if (min > max) min = max;
                    var optionValue = min == max ? $"{max}" : $"{min} - {max}";
                    sb.AppendLine($"{optionName}: {optionValue}");
                }
                else if (option == CustomOptionHolder.impostorRolesCountMin)
                {
                    var optionName =
                        CustomOptionHolder.cs(new Color(204f / 255f, 204f / 255f, 0, 1f), "categoryHeaderMaskedImp".Translate());
                    var min = CustomOptionHolder.impostorRolesCountMin.getSelection();
                    var max = CustomOptionHolder.impostorRolesCountMax.getSelection();
                    if (max > GameOptionsManager.Instance.currentGameOptions.NumImpostors)
                        max = GameOptionsManager.Instance.currentGameOptions.NumImpostors;
                    if (min > max) min = max;
                    var optionValue = min == max ? $"{max}" : $"{min} - {max}";
                    sb.AppendLine($"{optionName}: {optionValue}");
                }
                else if (option == CustomOptionHolder.modifiersCountMin)
                {
                    var optionName = CustomOptionHolder.cs(new Color(204f / 255f, 204f / 255f, 0, 1f), "categoryHeaderMaskedMod".Translate());
                    var min = CustomOptionHolder.modifiersCountMin.getSelection();
                    var max = CustomOptionHolder.modifiersCountMax.getSelection();
                    if (min > max) min = max;
                    var optionValue = min == max ? $"{max}" : $"{min} - {max}";
                    sb.AppendLine($"{optionName}: {optionValue}");
                }
                else if (option == CustomOptionHolder.crewmateRolesCountMax ||
                         option == CustomOptionHolder.neutralRolesCountMax ||
                         option == CustomOptionHolder.impostorRolesCountMax ||
                         option == CustomOptionHolder.modifiersCountMax)
                {
                }
                else
                {
                    sb.AppendLine($"\n{option.getName()}: {option.getString()}");
                }
            }
        }

        return sb.ToString();
    }

    public static string buildAllOptions(string vanillaSettings = "", bool hideExtras = false)
    {
        if (vanillaSettings == "")
            vanillaSettings = GameOptionsManager.Instance.CurrentGameOptions.ToHudString(PlayerControl.AllPlayerControls.Count);
        int counter = TheOtherRolesPlugin.optionsPage;
        string hudString = counter != 0 && !hideExtras ? Helpers.cs(DateTime.Now.Second % 2 == 0 ? Color.white : Color.red, ModTranslation.GetString("optionScroll") + "\n\n") : "";

        if (TORMapOptions.gameMode == CustomGamemodes.HideNSeek)
        {
            if (TheOtherRolesPlugin.optionsPage > 1) TheOtherRolesPlugin.optionsPage = 0;
            maxPage = 2;
            switch (counter)
            {
                case 0:
                    hudString += ModTranslation.GetString("hideNSeekPage1") + buildOptionsOfType(CustomOption.CustomOptionType.HideNSeekMain, false);
                    break;
                case 1:
                    hudString += ModTranslation.GetString("hideNSeekPage2") + buildOptionsOfType(CustomOption.CustomOptionType.HideNSeekRoles, false);
                    break;
            }
        }
        else if (TORMapOptions.gameMode == CustomGamemodes.PropHunt)
        {
            maxPage = 1;
            switch (counter)
            {
                case 0:
                    hudString += ModTranslation.GetString("propHunt1") + buildOptionsOfType(CustomOptionType.PropHunt, false);
                    break;
            }
        }
        else
        {
            switch (counter)
            {
                case 0:
                    hudString += (!hideExtras ? "" : ModTranslation.GetString("page1")) + vanillaSettings;
                    break;
                case 1:
                    hudString += ModTranslation.GetString("page2") + buildOptionsOfType(CustomOption.CustomOptionType.General, false);
                    break;
                case 2:
                    hudString += ModTranslation.GetString("page3") + buildRoleOptions();
                    break;
                case 3:
                    hudString += ModTranslation.GetString("page4") + buildOptionsOfType(CustomOption.CustomOptionType.Impostor, false);
                    break;
                case 4:
                    hudString += ModTranslation.GetString("page5") + buildOptionsOfType(CustomOption.CustomOptionType.Neutral, false);
                    break;
                case 5:
                    hudString += ModTranslation.GetString("page6") + buildOptionsOfType(CustomOption.CustomOptionType.Crewmate, false);
                    break;
                case 6:
                    hudString += ModTranslation.GetString("page7") + buildOptionsOfType(CustomOption.CustomOptionType.Modifier, false);
                    break;
            }
        }

        if (!hideExtras || counter != 0) hudString += string.Format(ModTranslation.GetString("pressTabForMore"), (counter + 1), maxPage);
        return hudString;
    }


    [HarmonyPatch(typeof(IGameOptionsExtensions), nameof(IGameOptionsExtensions.ToHudString))]
    private static void Postfix(ref string __result)
    {
        if (GameOptionsManager.Instance.currentGameOptions.GameMode == GameModes.HideNSeek)
            return; // Allow Vanilla Hide N Seek
        __result = buildAllOptions(__result);
    }
}

[HarmonyPatch]
public class AddToKillDistanceSetting
{
    [HarmonyPatch(typeof(LegacyGameOptions), nameof(LegacyGameOptions.AreInvalid))]
    [HarmonyPrefix]
    public static bool Prefix(LegacyGameOptions __instance, ref int maxExpectedPlayers)
    {
        //making the killdistances bound check higher since extra short is added
        return __instance.MaxPlayers > maxExpectedPlayers || __instance.NumImpostors < 1
                                                          || __instance.NumImpostors > 3 || __instance.KillDistance < 0
                                                          || __instance.KillDistance >=
                                                          LegacyGameOptions.KillDistances.Count
                                                          || __instance.PlayerSpeedMod <= 0f ||
                                                          __instance.PlayerSpeedMod > 3f;
    }

    [HarmonyPatch(typeof(NormalGameOptionsV07), nameof(NormalGameOptionsV07.AreInvalid))]
    [HarmonyPrefix]
    public static bool Prefix(NormalGameOptionsV07 __instance, ref int maxExpectedPlayers)
    {
        return __instance.MaxPlayers > maxExpectedPlayers || __instance.NumImpostors < 1
                                                          || __instance.NumImpostors > 3 || __instance.KillDistance < 0
                                                          || __instance.KillDistance >=
                                                          LegacyGameOptions.KillDistances.Count
                                                          || __instance.PlayerSpeedMod <= 0f ||
                                                          __instance.PlayerSpeedMod > 3f;
    }

    [HarmonyPatch(typeof(StringOption), nameof(StringOption.Initialize))]
    [HarmonyPrefix]
    public static void Prefix(StringOption __instance)
    {
        //prevents indexoutofrange exception breaking the setting if long happens to be selected
        //when host opens the laptop
        if (__instance.Title == StringNames.GameKillDistance && __instance.Value == 3)
        {
            __instance.Value = 1;
            GameOptionsManager.Instance.currentNormalGameOptions.KillDistance = 1;
            GameManager.Instance.LogicOptions.SyncOptions();
        }
    }

    [HarmonyPatch(typeof(StringOption), nameof(StringOption.Initialize))]
    [HarmonyPostfix]
    public static void Postfix(StringOption __instance)
    {
        if (__instance.Title == StringNames.GameKillDistance && __instance.Values.Count == 3)
            __instance.Values = new Il2CppStructArray<StringNames>(
                new[]
                {
                    (StringNames)49999, StringNames.SettingShort, StringNames.SettingMedium, StringNames.SettingLong
                });
    }

    [HarmonyPatch(typeof(IGameOptionsExtensions), nameof(IGameOptionsExtensions.AppendItem),
        typeof(Il2CppSystem.Text.StringBuilder), typeof(StringNames), typeof(string))]
    [HarmonyPrefix]
    public static void Prefix(ref StringNames stringName, ref string value)
    {
        if (stringName == StringNames.GameKillDistance)
        {
            int index;
            if (GameOptionsManager.Instance.currentGameMode == GameModes.Normal)
                index = GameOptionsManager.Instance.currentNormalGameOptions.KillDistance;
            else
                index = GameOptionsManager.Instance.currentHideNSeekGameOptions.KillDistance;
            value = LegacyGameOptions.KillDistanceStrings[index];
        }
    }

    [HarmonyPatch(typeof(TranslationController), nameof(TranslationController.GetString),
        new[] { typeof(StringNames), typeof(Il2CppReferenceArray<Il2CppSystem.Object>) })]
    [HarmonyPriority(Priority.Last)]

    public static bool Prefix(ref string __result, ref StringNames id)
    {
        if ((int)id == 49999)
        {
            __result = "killDistancesVS".Translate();
            return false;
        }
        return true;
    }

    public static void addKillDistance()
    {
        LegacyGameOptions.KillDistances = new(new float[] { 0.5f, 1f, 1.8f, 2.5f });
        LegacyGameOptions.KillDistanceStrings = new(new string[] { "killDistancesVS".Translate(), "killDistancesS".Translate(), "killDistancesM".Translate(), "killDistancesL".Translate() });
    }

    [HarmonyPatch(typeof(StringGameSetting), nameof(StringGameSetting.GetValueString))]
    [HarmonyPrefix]
    public static bool AjdustStringForViewPanel(StringGameSetting __instance, float value, ref string __result)
    {
        if (__instance.OptionName != Int32OptionNames.KillDistance) return true;
        __result = LegacyGameOptions.KillDistanceStrings[(int)value];
        return false;
    }
}

[HarmonyPatch(typeof(KeyboardJoystick), nameof(KeyboardJoystick.Update))]
public static class GameOptionsNextPagePatch
{
    public static void Postfix(KeyboardJoystick __instance)
    {
        var page = TheOtherRolesPlugin.optionsPage;
        if (Input.GetKeyDown(KeyCode.Tab)) TheOtherRolesPlugin.optionsPage = (TheOtherRolesPlugin.optionsPage + 1) % 7;
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)) TheOtherRolesPlugin.optionsPage = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)) TheOtherRolesPlugin.optionsPage = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3)) TheOtherRolesPlugin.optionsPage = 2;
        if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4)) TheOtherRolesPlugin.optionsPage = 3;
        if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5)) TheOtherRolesPlugin.optionsPage = 4;
        if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6)) TheOtherRolesPlugin.optionsPage = 5;
        if (Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown(KeyCode.Keypad7)) TheOtherRolesPlugin.optionsPage = 6;
        if (Input.GetKeyDown(KeyCode.F1))
            HudManagerUpdate.ToggleSettings(HudManager.Instance);
        if (Input.GetKeyDown(KeyCode.F2) && LobbyBehaviour.Instance)
            HudManagerUpdate.ToggleSummary(HudManager.Instance);
        if (TheOtherRolesPlugin.optionsPage >= LegacyGameOptionsPatch.maxPage) TheOtherRolesPlugin.optionsPage = 0;
    }
}

//This class is taken and adapted from Town of Us Reactivated, https://github.com/eDonnes124/Town-Of-Us-R/blob/master/source/Patches/CustomOption/Patches.cs, Licensed under GPLv3
[HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
public class HudManagerUpdate
{
    private static readonly TextMeshPro GameSettings = null;

    public static float
        MinX, /*-5.3F*/
        OriginalY = 2.9F,
        MinY = 2.9F;

    public static Scroller Scroller;
    private static Vector3 LastPosition;
    private static float lastAspect;
    private static bool setLastPosition;

    private static readonly TextMeshPro[] settingsTMPs = new TextMeshPro[4];
    private static GameObject settingsBackground;

    private static TextMeshPro summaryTMP;
    private static GameObject summaryBackground;

    private static PassiveButton toggleSettingsButton;
    private static GameObject toggleSettingsButtonObject;

    private static PassiveButton toggleSummaryButton;
    private static GameObject toggleSummaryButtonObject;

    private static GameObject toggleZoomButtonObject;
    private static PassiveButton toggleZoomButton;

    public static void Prefix(HudManager __instance)
    {
        if (GameSettings?.transform == null) return;

        // Sets the MinX position to the left edge of the screen + 0.1 units
        var safeArea = Screen.safeArea;
        var aspect = Mathf.Min(Camera.main.aspect, safeArea.width / safeArea.height);
        var safeOrthographicSize = CameraSafeArea.GetSafeOrthographicSize(Camera.main);
        MinX = 0.1f - safeOrthographicSize * aspect;

        if (!setLastPosition || aspect != lastAspect)
        {
            LastPosition = new Vector3(MinX, MinY);
            lastAspect = aspect;
            setLastPosition = true;
            if (Scroller != null) Scroller.ContentXBounds = new FloatRange(MinX, MinX);
        }

        CreateScroller(__instance);

        Scroller.gameObject.SetActive(GameSettings.gameObject.activeSelf);

        if (!Scroller.gameObject.active) return;

        var rows = GameSettings.text.Count(c => c == '\n');
        var LobbyTextRowHeight = 0.06F;
        var maxY = Mathf.Max(MinY, rows * LobbyTextRowHeight + (rows - 38) * LobbyTextRowHeight);

        Scroller.ContentYBounds = new FloatRange(MinY, maxY);

        // Prevent scrolling when the player is interacting with a menu
        if (PlayerControl.LocalPlayer.CanMove != true)
        {
            GameSettings.transform.localPosition = LastPosition;

            return;
        }

        if (GameSettings.transform.localPosition.x != MinX ||
            GameSettings.transform.localPosition.y < MinY) return;

        LastPosition = GameSettings.transform.localPosition;
    }

    private static void CreateScroller(HudManager __instance)
    {
        if (Scroller != null) return;

        var target = GameSettings.transform;

        Scroller = new GameObject("SettingsScroller").AddComponent<Scroller>();
        Scroller.transform.SetParent(GameSettings.transform.parent);
        Scroller.gameObject.layer = 5;

        Scroller.transform.localScale = Vector3.one;
        Scroller.allowX = false;
        Scroller.allowY = true;
        Scroller.active = true;
        Scroller.velocity = new Vector2(0, 0);
        Scroller.ScrollbarYBounds = new FloatRange(0, 0);
        Scroller.ContentXBounds = new FloatRange(MinX, MinX);
        Scroller.enabled = true;

        Scroller.Inner = target;
        target.SetParent(Scroller.transform);
    }

    [HarmonyPrefix]
    public static void Prefix2(HudManager __instance)
    {
        if (!settingsTMPs[0]) return;
        foreach (var tmp in settingsTMPs) tmp.text = "";
        var settingsString = LegacyGameOptionsPatch.buildAllOptions(hideExtras: true);
        var blocks = settingsString.Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
        ;
        var curString = "";
        string curBlock;
        var j = 0;
        for (var i = 0; i < blocks.Length; i++)
        {
            curBlock = blocks[i];
            if (Helpers.lineCount(curBlock) + Helpers.lineCount(curString) < 43)
            {
                curString += curBlock + "\n\n";
            }
            else
            {
                settingsTMPs[j].text = curString;
                j++;

                curString = "\n" + curBlock + "\n\n";
                if (curString.Substring(0, 2) != "\n\n") curString = "\n" + curString;
            }
        }

        if (j < settingsTMPs.Length) settingsTMPs[j].text = curString;
        var blockCount = 0;
        foreach (var tmp in settingsTMPs)
            if (tmp.text != "")
                blockCount++;
        for (var i = 0; i < blockCount; i++)
            settingsTMPs[i].transform.localPosition = new Vector3(-blockCount * 1.2f + 2.7f * i, 2.2f, -500f);
    }

    public static void OpenSettings(HudManager __instance)
    {
        if (__instance.FullScreen == null || (MapBehaviour.Instance && MapBehaviour.Instance.IsOpen)) return;
        if (summaryTMP) CloseSummary();
        settingsBackground = GameObject.Instantiate(__instance.FullScreen.gameObject, __instance.transform);
        settingsBackground.SetActive(true);
        var renderer = settingsBackground.GetComponent<SpriteRenderer>();
        renderer.color = new Color(0.2f, 0.2f, 0.2f, 0.9f);
        renderer.enabled = true;

        for (var i = 0; i < settingsTMPs.Length; i++)
        {
            settingsTMPs[i] = GameObject.Instantiate(__instance.KillButton.cooldownTimerText, __instance.transform);
            settingsTMPs[i].alignment = TextAlignmentOptions.TopLeft;
            settingsTMPs[i].enableWordWrapping = false;
            settingsTMPs[i].transform.localScale = Vector3.one * 0.25f;
            settingsTMPs[i].gameObject.SetActive(true);
        }
    }

    public static void CloseSettings()
    {
        foreach (var tmp in settingsTMPs)
            if (tmp)
                tmp.gameObject.Destroy();

        if (settingsBackground) settingsBackground.Destroy();
    }

    public static void ToggleSettings(HudManager __instance)
    {
        if (settingsTMPs[0]) CloseSettings();
        else OpenSettings(__instance);
    }

    [HarmonyPrefix]
    public static void Prefix3(HudManager __instance)
    {
        if (!summaryTMP) return;
        summaryTMP.text = Helpers.previousEndGameSummary;

        summaryTMP.transform.localPosition = new Vector3(-3 * 1.2f, 2.2f, -500f);
    }

    public static void OpenSummary(HudManager __instance)
    {
        if (__instance.FullScreen == null || (MapBehaviour.Instance && MapBehaviour.Instance.IsOpen) ||
            Helpers.previousEndGameSummary.IsNullOrWhiteSpace()) return;
        if (settingsTMPs[0]) CloseSettings();
        summaryBackground = GameObject.Instantiate(__instance.FullScreen.gameObject, __instance.transform);
        summaryBackground.SetActive(true);
        var renderer = summaryBackground.GetComponent<SpriteRenderer>();
        renderer.color = new Color(0.2f, 0.2f, 0.2f, 0.9f);
        renderer.enabled = true;


        summaryTMP = GameObject.Instantiate(__instance.KillButton.cooldownTimerText, __instance.transform);
        summaryTMP.alignment = TextAlignmentOptions.TopLeft;
        summaryTMP.enableWordWrapping = false;
        summaryTMP.transform.localScale = Vector3.one * 0.3f;
        summaryTMP.gameObject.SetActive(true);
    }

    public static void CloseSummary()
    {
        summaryTMP?.gameObject.Destroy();
        summaryTMP = null;
        if (summaryBackground) summaryBackground.Destroy();
    }

    public static void ToggleSummary(HudManager __instance)
    {
        if (summaryTMP) CloseSummary();
        else OpenSummary(__instance);
    }

    [HarmonyPostfix]
    public static void Postfix(HudManager __instance)
    {
        if (AmongUsClient.Instance.GameState != InnerNetClient.GameStates.Started) return;
        if (!toggleSettingsButton || !toggleSettingsButtonObject)
        {
            // add a special button for settings viewing:
            toggleSettingsButtonObject =
                GameObject.Instantiate(__instance.MapButton.gameObject, __instance.MapButton.transform.parent);
            toggleSettingsButtonObject.transform.localPosition =
                __instance.MapButton.transform.localPosition + new Vector3(0, -1.25f, -500f);
            toggleSettingsButtonObject.name = "TOGGLESETTINGSBUTTON";
            var renderer = toggleSettingsButtonObject.transform.Find("Inactive").GetComponent<SpriteRenderer>();
            var rendererActive = toggleSettingsButtonObject.transform.Find("Active").GetComponent<SpriteRenderer>();
            toggleSettingsButtonObject.transform.Find("Background").localPosition = Vector3.zero;
            renderer.sprite = Helpers.loadSpriteFromResources("Settings_Button.png", 100f);
            rendererActive.sprite =
                Helpers.loadSpriteFromResources("Settings_ButtonActive.png", 100);
            toggleSettingsButton = toggleSettingsButtonObject.GetComponent<PassiveButton>();
            toggleSettingsButton.OnClick.RemoveAllListeners();
            toggleSettingsButton.OnClick.AddListener((Action)(() => ToggleSettings(__instance)));
        }

        toggleSettingsButtonObject.SetActive(__instance.MapButton.gameObject.active &&
                                             !(MapBehaviour.Instance && MapBehaviour.Instance.IsOpen) &&
                                             GameOptionsManager.Instance.currentGameOptions.GameMode !=
                                             GameModes.HideNSeek);
        toggleSettingsButtonObject.transform.localPosition =
            __instance.MapButton.transform.localPosition + new Vector3(0, -0.8f, -500f);

        if (!toggleZoomButton || !toggleZoomButtonObject)
        {
            // add a special button for settings viewing:
            toggleZoomButtonObject =
                GameObject.Instantiate(__instance.MapButton.gameObject, __instance.MapButton.transform.parent);
            toggleZoomButtonObject.transform.localPosition =
                __instance.MapButton.transform.localPosition + new Vector3(0, -1.25f, -500f);
            toggleZoomButtonObject.name = "TOGGLEZOOMBUTTON";
            var tZrenderer = toggleZoomButtonObject.transform.Find("Inactive").GetComponent<SpriteRenderer>();
            var tZArenderer = toggleZoomButtonObject.transform.Find("Active").GetComponent<SpriteRenderer>();
            toggleZoomButtonObject.transform.Find("Background").localPosition = Vector3.zero;
            tZrenderer.sprite = Helpers.loadSpriteFromResources("Minus_Button.png", 100f);
            tZArenderer.sprite = Helpers.loadSpriteFromResources("Minus_ButtonActive.png", 100);
            toggleZoomButton = toggleZoomButtonObject.GetComponent<PassiveButton>();
            toggleZoomButton.OnClick.RemoveAllListeners();
            toggleZoomButton.OnClick.AddListener((Action)(() => Helpers.toggleZoom()));
        }

        var (playerCompleted, playerTotal) = TasksHandler.taskInfo(PlayerControl.LocalPlayer.Data);
        var numberOfLeftTasks = playerTotal - playerCompleted;
        var zoomButtonActive = !(PlayerControl.LocalPlayer == null || !PlayerControl.LocalPlayer.Data.IsDead ||
                                 (PlayerControl.LocalPlayer.Data.Role.IsImpostor &&
                                  !CustomOptionHolder.deadImpsBlockSabotage.getBool()) || MeetingHud.Instance);
        zoomButtonActive &= numberOfLeftTasks <= 0 ||
                            !CustomOptionHolder.finishTasksBeforeHauntingOrZoomingOut.getBool();
        toggleZoomButtonObject.SetActive(zoomButtonActive);
        var posOffset = Helpers.zoomOutStatus ? new Vector3(-1.27f, -7.92f, -52f) : new Vector3(0, -1.6f, -52f);
        toggleZoomButtonObject.transform.localPosition =
            HudManager.Instance.MapButton.transform.localPosition + posOffset;
    }

    [HarmonyPostfix]
    public static void Postfix2(HudManager __instance)
    {
        if (AmongUsClient.Instance.GameState == InnerNetClient.GameStates.Started)
        {
            if (toggleSummaryButtonObject != null)
            {
                toggleSummaryButtonObject.SetActive(false);
                toggleSummaryButtonObject.Destroy();
                toggleSummaryButton.Destroy();
            }

            return;
        }

        if (!toggleSummaryButton || !toggleSummaryButtonObject)
        {
            // add a special button for settings viewing:
            toggleSummaryButtonObject =
                GameObject.Instantiate(__instance.MapButton.gameObject, __instance.MapButton.transform.parent);
            toggleSummaryButtonObject.transform.localPosition =
                __instance.MapButton.transform.localPosition + new Vector3(0, -1.25f, -500f);
            toggleSummaryButtonObject.name = "TOGGLESUMMARYSBUTTON";
            var renderer = toggleSummaryButtonObject.transform.Find("Inactive").GetComponent<SpriteRenderer>();
            var rendererActive = toggleSummaryButtonObject.transform.Find("Active").GetComponent<SpriteRenderer>();
            toggleSummaryButtonObject.transform.Find("Background").localPosition = Vector3.zero;
            renderer.sprite = Helpers.loadSpriteFromResources("Endscreen.png", 100f);
            rendererActive.sprite =
                Helpers.loadSpriteFromResources("EndscreenActive.png", 100f);
            toggleSummaryButton = toggleSummaryButtonObject.GetComponent<PassiveButton>();
            toggleSummaryButton.OnClick.RemoveAllListeners();
            toggleSummaryButton.OnClick.AddListener((Action)(() => ToggleSummary(__instance)));
        }

        toggleSummaryButtonObject.SetActive(__instance.SettingsButton.gameObject.active && LobbyBehaviour.Instance &&
                                            !Helpers.previousEndGameSummary.IsNullOrWhiteSpace() &&
                                            GameOptionsManager.Instance.currentGameOptions.GameMode !=
                                            GameModes.HideNSeek
                                            && AmongUsClient.Instance.GameState != InnerNetClient.GameStates.Started);
        toggleSummaryButtonObject.transform.localPosition =
            __instance.SettingsButton.transform.localPosition + new Vector3(-1.45f, 0.03f, -500f);
    }
}