using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiAlphaChanger : MonoBehaviour {

	[Range(0f,1f)]
	public float Alpha = 1f;
	private float _currentAlpha = 1f;

	private List<ColoringElement> _components = new List<ColoringElement>();

	private void Awake() {
		CatchComponents();
	}

	private void LateUpdate() {
		if (_currentAlpha != Alpha) {
			_currentAlpha = Alpha;
			try {
				_components.ForEach(x => x.SetAlphaColor = _currentAlpha);
			} catch {
				CatchComponents();
				_components.ForEach(x => x.SetAlphaColor = _currentAlpha);
			}
		}
	}

	private void CatchComponents() {

		_components.Clear();
		FindIn(transform);

		void FindIn(Transform target) {
			foreach (Transform t in target) {
				TryFindGraphicElement(t);
				if (t.childCount > 0) {
					FindIn(t);
				}
			}
		}

		void TryFindGraphicElement(Transform target) {
			if (target.TryGetComponent<Graphic>(out var graphic)) {
				_components.Add(new ColoringElement(graphic));
			}
		}

	}

	private class ColoringElement {

		public Graphic Element;
		public Color Original;

		public float SetAlphaColor {
			set {
				Element.color = new Color(Original.r, Original.g, Original.b, value);
			}
		}

		public ColoringElement(Graphic graphic) {
			Element = graphic;
			Original = Element.color;
		}

	}

}
