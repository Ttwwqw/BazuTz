

using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public static class UiFramesHandler {

	[MenuItem("Tools/UiFrames/Create frames info")]
	public static void CreateFrameAssotiationsList() {

		var dataPath = Application.dataPath.Replace("Assets", "");
		var folderPath = Path.Combine(dataPath, "Assets/Resources/UiFrames");
		var toReplace = Path.Combine(dataPath, "Assets/Resources/");

		var files = (from filePath in Directory.GetFiles(folderPath, "*prefab") select filePath.Replace(toReplace, "").Replace(".prefab", "")).ToArray();
		List<Association> associations = new List<Association>();

		foreach (var filePath in files) {
			var asset = Resources.Load<GameObject>(filePath);
			if (asset.TryGetComponent<IFrame>(out var frame)) {
				associations.Add(new Association(frame.GetType().FullName, asset.name));
			}
		}

		File.WriteAllText(Path.Combine(dataPath, folderPath, "Associtations.txt"), JsonHelper.ToJson(associations.ToArray()));

	}

	[MenuItem("Tools/UiFrames/Open prefabs folder")]
	public static void OpenPrefabsLocation() {

		var path = string.Format("{0}/Resources/UiFrames", Application.dataPath);
		if (!Directory.Exists(path)) {
			Directory.CreateDirectory(path);
			AssetDatabase.Refresh();
		}

		Object obj = AssetDatabase.LoadAssetAtPath("Assets/Resources/UiFrames", typeof(UnityEngine.Object));
		Object[] selection = new UnityEngine.Object[1];
		selection[0] = obj;
		Selection.objects = selection;

	}

}
