
using System;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Localization", menuName = "OldRat/LocalizationList", order = 1)]
public class LocalizationsList : ScriptableObject {
	public List<LocalizationReference> LocalizationIcons = new List<LocalizationReference>();
}
[Serializable]
public class LocalizationReference {
	public Sprite Image;
	public string Label;
	public string OriginalName;
}


