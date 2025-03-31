// Runtime translation patch for Unity Il2Cpp (MelonLoader + Harmony)
// Customized for Schedule I - Coded by Dralle

using MelonLoader;
using HarmonyLib;
using Il2CppTMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Il2CppSystem;
using System.Reflection;
using MelonLoader.Utils;

[assembly: MelonInfo(typeof(Localization.Main), "LocalizationPatch", "1.1.2", "Dralle")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace Localization
{
    public class Main : MelonMod
    {
        public override void OnInitializeMelon()
        {
            LocalizationManager.Load();
        }
    }

    public static class LocalizationManager
    {
        private static Dictionary<string, string> translations = new Dictionary<string, string>();

        public static void Load()
        {
            string path = Path.Combine(MelonEnvironment.UserDataDirectory, "fr.json");
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                translations = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                MelonLogger.Msg($"Loaded {translations.Count} translation entries.");
            }
            else
            {
                MelonLogger.Warning("Translation file 'fr.json' not found.");
            }
        }

        public static string Translate(string original)
        {
            if (string.IsNullOrWhiteSpace(original)) return original;
            var cleaned = original.Trim();
            return translations.TryGetValue(cleaned, out var value) ? value : original;
        }
    }

    [HarmonyPatch]
    public class Patch_TMP_Text
    {
        static MethodBase TargetMethod()
        {
            return AccessTools.PropertySetter(typeof(TextMeshProUGUI), "text");
        }

        public static void Prefix(ref Il2CppSystem.String value)
        {
            var original = value?.ToString();
            MelonLogger.Msg($"[TMP] Intercepted text: {original}");
            var translated = LocalizationManager.Translate(original);
            value = translated;
        }
    }

    [HarmonyPatch]
    public class Patch_UI_Text
    {
        static MethodBase TargetMethod()
        {
            return AccessTools.PropertySetter(typeof(Text), "text");
        }

        public static void Prefix(ref Il2CppSystem.String value)
        {
            var original = value?.ToString();
            MelonLogger.Msg($"[UI.Text] Intercepted text: {original}");
            var translated = LocalizationManager.Translate(original);
            value = translated;
        }
    }
}