
using UnityEngine;
using System.Collections.Generic;

public delegate void LangugeIsChanged();

// Work with translating and variables
public static partial class Localization {
	
	public static LangugeIsChanged OnLangugeIsChanged;
	public static LocalizationsList Localizations { get; private set; }
	private static Dictionary<string, string> s_localizationLines = new Dictionary<string, string>();

	private static SystemLanguage s_currentLanguage = SystemLanguage.English;
	public static SystemLanguage Language {
		get { return s_currentLanguage; }
		set {
			s_currentLanguage = value;
			if (s_resourceLoader != null) {
				LoadLanguage(value);
				PlayerPrefs.SetString("lang", value.ToString());
			} else {
				Debug.Log("Language is changed. But resource loader is missing. First initialize this component.");
			}
		}
	}

	public static string TryTranslate(object key, string originalValue = null) {
		if (s_IsInitialized && s_localizationLines.TryGetValue(key.ToString(), out var result)) {
			return result.Replace("{new_line}", "\n");
		} else {
			return originalValue;
		}
	}
}
