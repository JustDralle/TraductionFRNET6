using MelonLoader;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using Il2CppTMPro;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MelonLoader.Utils;

[assembly: MelonInfo(typeof(TranslationPatch.Main), "FR Translation Patch", "1.0.0", "Dralle")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace TranslationPatch
{
    public class Main : MelonMod
{
        public override void OnInitializeMelon()
        {
            TranslationManager.Load();

            var harmony = new HarmonyLib.Harmony("fr.translation.patch");
            harmony.PatchAll();

            MelonLogger.Msg("Translation system loaded and patched.");
        }


        public override void OnUpdate()
         {
             TranslationManager.ApplyTranslations();
         }

    }


    public static class TranslationManager
    {
        private static Dictionary<string, string> translations = new();
        private static HashSet<Il2CppSystem.Object> alreadyTranslated = new();

        public static void Load()
        {
            string path = Path.Combine(MelonEnvironment.UserDataDirectory, "fr.json");

            if (!File.Exists(path))
            {
                MelonLogger.Warning("Translation file not found!");
                return;
            }

            try
            {
                string json = File.ReadAllText(path);
                translations = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                MelonLogger.Msg($"[Trad] {translations.Count} entries loaded.");
            }
            catch (System.Exception ex)
            {
                MelonLogger.Error($"Failed to load translation file: {ex.Message}");
            }
        }

        public static string Translate(string original)
        {
            if (string.IsNullOrWhiteSpace(original)) return original;
            string key = original.Trim();
            return translations.TryGetValue(key, out var value) && !string.IsNullOrEmpty(value) ? value : original;
        }

        public static void ApplyTranslations()
        {
            foreach (var tmp in GameObject.FindObjectsOfType<TextMeshProUGUI>())
            {
                if (tmp == null || alreadyTranslated.Contains(tmp)) continue;

                string current = tmp.text?.Trim();
                if (string.IsNullOrWhiteSpace(current)) continue;

                string translated = Translate(current);
                if (translated != current)
                {
                    tmp.text = translated;
                    alreadyTranslated.Add(tmp);
                }
            }

            foreach (var ui in GameObject.FindObjectsOfType<Text>())
            {
                if (ui == null || alreadyTranslated.Contains(ui)) continue;

                string current = ui.text?.Trim();
                if (string.IsNullOrWhiteSpace(current)) continue;

                string translated = Translate(current);
                if (translated != current)
                {
                    ui.text = translated;
                    alreadyTranslated.Add(ui);
                }
            }
        }
    }
}
