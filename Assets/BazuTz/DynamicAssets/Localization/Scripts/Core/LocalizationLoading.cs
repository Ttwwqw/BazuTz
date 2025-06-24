
using UnityEngine;
using System.Collections.Generic;

// Worck with data
public static partial class Localization {

	private static bool s_IsInitialized = false;
	private static INativeResourceLoader s_resourceLoader;

	public static void Initialize(SystemLanguage? language = null, INativeResourceLoader resourcesLoader = null) {
		
		s_resourceLoader = resourcesLoader ?? Managers.GetManager<ResourcesLoader>().NativeLoader;
		Localizations = s_resourceLoader.Load<LocalizationsList>("Localization", "Localization");
		s_IsInitialized = true;

		if (language != null && language.HasValue) {
			Language = language.Value;
		} else {
			LoadLanguage(s_currentLanguage);
		}

	}

	private static void LoadLanguage(SystemLanguage language) {

		ReadFile(s_resourceLoader.Load<TextAsset>("Localization/Sheets", language.ToString()));

		void ReadFile(TextAsset asset) {
			s_localizationLines = ReadLocalizationFile(asset.text);
			OnLangugeIsChanged?.Invoke();
		}

	}

	public static Dictionary<string, string> ReadLocalizationFile(string text) {

		string[] lines = text.Split(';');
		Dictionary<string, string> result = new Dictionary<string, string>(lines.Length);

		foreach (var value in lines) {
			if (string.IsNullOrEmpty(value)) {
				continue;
			}
			string[] final = value.Split('|');
			result.Add(final[0], final[1]);
		}

		return result;

	}

}
