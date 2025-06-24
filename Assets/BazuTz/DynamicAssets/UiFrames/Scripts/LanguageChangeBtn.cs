
using UnityEngine;
using UnityEngine.UI;

public class LanguageChangeBtn : MonoBehaviour {

	[SerializeField] private Image _langImage;

	private void Awake() {
		Localization.OnLangugeIsChanged += RefreshLangTooltip;
	}
	private void OnDestroy() {
		Localization.OnLangugeIsChanged -= RefreshLangTooltip;
	}

	public void SwitchLanguage() {
		Localization.Language = Localization.Language == SystemLanguage.English ? SystemLanguage.Japanese : SystemLanguage.English;
	}

	private void RefreshLangTooltip() {
		var filter = Localization.Language.ToString();
		_langImage.sprite = Localization.Localizations.LocalizationIcons.Find(x => x.Label == filter).Image;
	}

}
