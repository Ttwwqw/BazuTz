
using UnityEditor;
using UnityEngine;

public static class EditorLocalizationHandler {

	[MenuItem("Tools/Localization/Download localization sheet")]
	public static void DownloadLocalizationSheet() {

		var configs = EditorLocalizationUtility.LoadConfigs();
		GoogleSheets.DownloadGoogleSheet(configs.GoogleSheetId, configs.GooleTableId, configs.TsvFileFullPath);

	}

	[MenuItem("Tools/Localization/Refresh localization assets")]
	public static void CreateLocalizationAssets() {

		var configs = EditorLocalizationUtility.LoadConfigs();
		LocalizationSheet.GenerateLocalizationFiles(configs.TsvFileFullPath, configs.LanguageSheetsPath, configs.LanguageIconsPath, configs.LocalizationAssetPath);

	}

	[MenuItem("Tools/Localization/Prepare to Build")]
	public static void AllTogeather() {

		var configs = EditorLocalizationUtility.LoadConfigs();
		Download();

		void Download() {
			GoogleSheets.DownloadGoogleSheet(configs.GoogleSheetId, configs.GooleTableId, configs.TsvFileFullPath, Save);
		}
		void Save() {
			LocalizationSheet.GenerateLocalizationFiles(configs.TsvFileFullPath, configs.LanguageSheetsPath, configs.LanguageIconsPath, configs.LocalizationAssetPath);
		}

	}

}


