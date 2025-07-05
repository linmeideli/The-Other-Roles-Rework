﻿using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace TheOtherRoles.Modules;
public class ModTranslation
{
    public static int defaultLanguage = (int)SupportedLangs.SChinese;
    public static Dictionary<string, Dictionary<int, string>> stringData;
    private const string blankText = "[BLANK]";

    public static void Load()
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        Stream stream = assembly.GetManifestResourceStream("TheOtherRoles.Resources.stringData.json");
        var byteArray = new byte[stream.Length];
        var read = stream.Read(byteArray, 0, (int)stream.Length);
        string json = System.Text.Encoding.UTF8.GetString(byteArray);

        stringData = new Dictionary<string, Dictionary<int, string>>();
        JObject parsed = JObject.Parse(json);

        for (int i = 0; i < parsed.Count; i++)
        {
            JProperty token = parsed.ChildrenTokens[i].TryCast<JProperty>();
            if (token == null) continue;

            string stringName = token.Name;
            var val = token.Value.TryCast<JObject>();

            if (token.HasValues)
            {
                var strings = new Dictionary<int, string>();

                for (int j = 0; j < (int)SupportedLangs.Irish + 1; j++)
                {
                    string key = j.ToString();
                    var text = val[key]?.TryCast<JValue>().Value.ToString();

                    if (text != null && text.Length > 0)
                    {
                        if (text == blankText) strings[j] = "";
                        else strings[j] = text;
                    }
                }

                stringData[stringName] = strings;
            }
        }
    }

    public static string GetString(string key, string def = null)
    {
        // Strip out color tags.
        string keyClean = Regex.Replace(key, "<.*?>", "");
        keyClean = Regex.Replace(keyClean, "^-\\s*", "");
        keyClean = keyClean.Trim();

        def ??= key;
        if (!stringData.ContainsKey(keyClean))
            return def;

        var data = stringData[keyClean];
        int lang = (int)AmongUs.Data.DataManager.Settings.Language.CurrentLanguage;

        if (data.ContainsKey(lang))
            return key.Replace(keyClean, data[lang]);
        else if (data.ContainsKey(defaultLanguage))
        {
            return key.Replace(keyClean, data[defaultLanguage]);
        }

        return key;
    }
}

internal static class LanguageExtension
{
    internal static string Translate(this string key)
    {
        return ModTranslation.GetString(key);
    }
}