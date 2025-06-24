
using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;

public class LoadingSliderFill : MonoBehaviour {

    [SerializeField]private AnimatorWithCallback _animator;

    [SerializeField] private Image _progresFill;
    [SerializeField] private TextMeshProUGUI _procentLabel;

    [Header("Random backgrounds")]
    [SerializeField] private Image _background;
    [SerializeField] private Sprite[] _backgrounds;

	private void Awake() {
        if (_backgrounds != null && _backgrounds.Length > 0) {
            _background.sprite = _backgrounds[UnityEngine.Random.Range(0, _backgrounds.Length)];
        }
    }

	public void SetValue(float value) {

        value = Mathf.Clamp(value, 0f, 1f);

        if (_progresFill != null) {
            _progresFill.fillAmount = value;
        }

        if (_procentLabel != null) {
            _procentLabel.text = string.Format("{0}%", (100f * value).ToString("00"));
        }
        
    }

    public void Hide(Action onDone) {

        if (_animator != null) {
            _animator.SetTrigger("Hide", onDone);
        } else {
            onDone?.Invoke();
        }

    }

}
