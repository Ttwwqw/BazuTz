
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using TMPro;
using System.Linq;
using System.IO;

[CustomEditor(typeof(LocalizationElement), true)]
public class ElementsInspector : Editor {

	public override void OnInspectorGUI() {

		var element = (LocalizationElement)target;
		var fields = element.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

		var labelField = fields.First(x => x.Name == "_label");
		var keyField = fields.First(x => x.Name == "_key");

		string key = keyField.GetValue(element)?.ToString();
		UnityEngine.Object label = labelField.GetValue(element) as UnityEngine.Object;

		GUILayout.BeginHorizontal();
		GUILayout.Label("Label:", GUILayout.Width(50));
		labelField.SetValue(element, EditorGUILayout.ObjectField(label, typeof(TextMeshProUGUI), true));
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		GUILayout.Label("Key:", GUILayout.Width(50));
		if (label != null && GUILayout.Button("Auto", GUILayout.Width(50))) {
			key = EditorLocalizationUtility.FindKey(((TextMeshProUGUI)label).text);
		}
		keyField.SetValue(element, GUILayout.TextField(key));
		GUILayout.EndHorizontal();

		//base.OnInspectorGUI();

	}

}

public static class EditorLocalizationUtility {

	private static Dictionary<string, string> s_localizationLines = null;

	public static string TranslateKey(string key) {
		LoadDefaultLocalizationFile();

		if (!string.IsNullOrEmpty(key) && s_localizationLines.TryGetValue(key, out string result)) {
			return result;
		} else {
			return "Key do not exist";
		}

	}

	public static string FindKey(string originalText) {
		LoadDefaultLocalizationFile();

		foreach (var kvp in s_localizationLines) {
			if (kvp.Value == originalText) {
				return kvp.Key;
			}
		}

		return "Key do not exist";
	}

	private static void LoadDefaultLocalizationFile() {
		if (s_localizationLines == null || s_localizationLines.Count > 0) {
			Configs configs = LoadConfigs();
			var path = string.Format("{0}/English.txt", configs.LanguageSheetsPath);
			try {
				s_localizationLines = Localization.ReadLocalizationFile(File.ReadAllText(path));
			} catch {
				throw new System.NotImplementedException(string.Format("Default localization file do no exist! Path: {0}", path));
			}
		}
	}



	#region Configs

	public static string ConfigsPath {
		get {
			return "Assets/DynamicAssets/Localization/Editor/Configs.txt";
		}
	}

	public static Configs LoadConfigs() {
		try {
			return JsonUtility.FromJson<Configs>(File.ReadAllText(ConfigsPath));
		} catch {
			var configs = new Configs();
			File.WriteAllText(ConfigsPath, JsonUtility.ToJson(configs, true));
			return configs;
		}
	}

	[System.Serializable]
	public class Configs {

		public string GooleTableId = "0";
		public string GoogleSheetId = "15g6jMH7yNnyOh-T8P24hKNbN4zZ6fUTWvj3wYCQifx8";

		public string TsvFileFullPath = "Assets/DynamicAssets/Localization/Sheets/DownloadedSheet.tsv";

		public string LanguageIconsPath = "Assets/DynamicAssets/Localization/Icons";
		public string LanguageSheetsPath = "Assets/DynamicAssets/Localization/Sheets";
		public string LocalizationAssetPath = "Assets/DynamicAssets/Localization";

	}

	#endregion


}

