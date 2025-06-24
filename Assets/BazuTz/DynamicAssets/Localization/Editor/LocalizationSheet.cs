
using System;
using System.IO;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public static class LocalizationSheet {

	public static void GenerateLocalizationFiles(string tsvFilePath, string sheetsPath, string iconsPath, string localizationAssetPath, Action callback = null) {

		string[] allLines = File.ReadAllLines(tsvFilePath);
		string[] titleLine = allLines[0].Split('\t');

		Dictionary<int, string> tempLocalizations = new Dictionary<int, string>();

		for (int i = 1; i < titleLine.Length; i++) {
			tempLocalizations.Add(i, "");
		}

		for (int i = 1; i < allLines.Length; i++) {
			string[] line = allLines[i].Split('\t');
			string key = line[0];
			if (string.IsNullOrEmpty(key)) {
				continue;
			}
			for (int colonum = 1; colonum < line.Length; colonum++) {
				tempLocalizations[colonum] += String.Format("{0}|{1};", key, line[colonum]);
			}
		}

		var localizationAsset = AssetDatabase.LoadAssetAtPath(String.Format("{0}/Localization.asset", localizationAssetPath), typeof(LocalizationsList)) as LocalizationsList;

		localizationAsset.LocalizationIcons.Clear();
		Debug.Log("Languages: " + titleLine.Length);

		for (int i = 1; i < titleLine.Length; i++) {

			string path = String.Format("{0}/{1}.txt", sheetsPath, titleLine[i]);
			File.WriteAllText(path, tempLocalizations[i]);

			var dict = Localization.ReadLocalizationFile(tempLocalizations[i]);
			var img = AssetDatabase.LoadAssetAtPath(String.Format("{0}/{1}.png", iconsPath, titleLine[i]), typeof(Sprite)) as Sprite;

			localizationAsset.LocalizationIcons.Add(new LocalizationReference() { Image = img, Label = titleLine[i], OriginalName = dict["0"] });

		}

		EditorUtility.SetDirty(localizationAsset);
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();

		callback?.Invoke();

	}

}
