
using UnityEngine;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

public static class LocalSaveSystem {

	public static string Profile { get; private set; } = "Guest";

	public static void ChangeProfileName(string profile) {
		Profile = profile;
	}

	public static void DestroyProfile(string profile) {
		var keys = new Keys();
		foreach (var fieldIfo in typeof(Keys).GetFields()) {

			var fieldValue = fieldIfo.GetValue(keys).ToString();
			PlayerPrefs.DeleteKey(string.Format("{0}.{1}", profile, fieldValue));

		}
	}

	public static string TryGetLocalValue(string key, string defaultValue, string profile = "") {

		profile = string.IsNullOrEmpty(profile) ? Profile : profile;

		var result = PlayerPrefs.GetString(string.Format("{0}.{1}", profile, key), string.Empty);
		if (result == string.Empty) {
			SetLocalValue(string.Format("{0}.{1}", profile, key), defaultValue);
			return defaultValue;
		} else {
			return result;
		}

	}

	public static string GetLocalValue(string key, string profile = "") {

		profile = string.IsNullOrEmpty(profile) ? Profile : profile;

		return PlayerPrefs.GetString(string.Format("{0}.{1}", profile, key), string.Empty);

	}

	public static void SetLocalValue(string key, string value, string profile = "") {

		profile = string.IsNullOrEmpty(profile) ? Profile : profile;

		PlayerPrefs.SetString(string.Format("{0}.{1}", profile, key), value);

	}

	public class Keys {

		// Miner 69
		public const string DigStage = "stg";


		// --------

		public const string Skins = "chSkin";
		public const string Stats = "chUpd";
		public const string PlayerLvl = "chLv";
		public const string Curency = "plCh";
		public const string BackpackStack = "plBpk";


		public const string Username = "usName";
		public const string Password = "usPass";

		public const string Local_Inventory = "usInv";
		public const string Local_AnimalsSave = "animalsSv";
		public const string Local_AnimalTempSave = "animalTemp";
		public const string Local_SC = "usSC";

		public const string Local_DayReward_CurrDay = "currDayInfo";

		public const string Local_AutoIncrement = "autoIncrement";
		public const string Local_FarmBuildings = "farmBuildings";
		public const string Local_FarmStorage = "farmStorage";

	}

}
