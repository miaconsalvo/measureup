using Mystie.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;
using UnityEngine.InputSystem;

namespace Mystie.Core
{
    public class GameSettings
    {
        public Dictionary<AudioBusType, RuntimeAudioBus> audioBuses;

        private InputActionAsset inputActions;

        private const float defaultVolume = 0.5f;
        public const string LOCALE_KEY = "locale";
        public const string REBIND_KEY = "rebinds";

        public GameSettings(SystemDataScriptable data)
        {
            LoadAudioBuses(data);
            inputActions = data.actions;

            string rebinds = PlayerPrefs.GetString(REBIND_KEY);
            if (!string.IsNullOrEmpty(rebinds))
                inputActions.LoadBindingOverridesFromJson(rebinds);
        }

        public int LoadLocale()
        {
            //Get locale code
            string localeCode = PlayerPrefs.GetString(LOCALE_KEY);

            //Find locale index
            int localeIndex = 0;
            for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; ++i)
            {
                Locale locale = LocalizationSettings.AvailableLocales.Locales[i];
                if (locale.Identifier.Code == localeCode)
                {
                    localeIndex = i;
                    LocalizationSettings.SelectedLocale = locale;
                    return localeIndex;
                }
            }

            //If locale was not found, reset locale to default
            LocaleSelected(localeIndex);
            return localeIndex;
        }

        public void LocaleSelected(int index)
        {
            if (index < 0 || index >= LocalizationSettings.AvailableLocales.Locales.Count)
            {
                Debug.LogWarning("Locale at index " + index + " not found.");
                return;
            }

            Locale locale = LocalizationSettings.AvailableLocales.Locales[index];
            string localeCode = locale.Identifier.Code;
            PlayerPrefs.SetString(LOCALE_KEY, localeCode);

            LocalizationSettings.SelectedLocale = locale;
        }

        public void LoadAudioBuses(SystemDataScriptable data)
        {
            if (data == null) return;

            audioBuses = new Dictionary<AudioBusType, RuntimeAudioBus>();

            foreach (AudioBus b in data.audioBuses)
            {
                if (!audioBuses.ContainsKey(b.type))
                {
                    RuntimeAudioBus audioBus = new RuntimeAudioBus(b);
                    if (!audioBus.IsValid) continue;

                    audioBuses.Add(b.type, audioBus);
                }
            }
        }

        public float GetVolume(AudioBusType busType)
        {
            if (!audioBuses.ContainsKey(busType))
            {
                Debug.LogWarning("GameSettings: No audio bus of type " + busType);
                return defaultVolume;
            }

            return audioBuses[busType].Volume;
        }

        public void SetVolume(AudioBusType busType, float volume)
        {
            if (!audioBuses.ContainsKey(busType))
            {
                Debug.LogWarning("GameSettings: No audio bus of type " + busType);
                return;
            }

            audioBuses[busType].Volume = volume;
        }

        public void SaveBindings()
        {
            string rebinds = inputActions.SaveBindingOverridesAsJson();
            PlayerPrefs.SetString(REBIND_KEY, rebinds);
        }

        public void ResetAllBindings()
        {
            foreach (InputActionMap map in inputActions.actionMaps)
            {
                map.RemoveAllBindingOverrides();
            }

            PlayerPrefs.DeleteKey(REBIND_KEY);
        }
    }
}
