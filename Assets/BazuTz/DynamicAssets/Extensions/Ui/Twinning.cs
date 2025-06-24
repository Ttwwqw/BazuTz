

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Object = UnityEngine.Object;

public static class Twinning {

	public static IEnumerator LerpFill(float targetAmount, Image fillingImage, float speed = 1f) {

		float t = 0f;
		float a = fillingImage.fillAmount;
		while (t < speed) {
			fillingImage.fillAmount = Lerp(a, targetAmount, t / speed);
			t += Time.unscaledDeltaTime;
			yield return null;
		}

	}

	public static IEnumerator UseImage(Image tagetImage, float scaleUp = 1.1f, float speed = 0.3f, Gradient gradient = null) {

		var img = Object.Instantiate(tagetImage, tagetImage.transform.parent, false);
		img.transform.SetAsFirstSibling();
		img.fillAmount = 1f;

		gradient = gradient ?? FastAlphaGradient(img.color, img.color.CopyAndChangeAlpha(0f));

		Vector3 startScale = tagetImage.transform.localScale;
		Vector3 endScale = startScale * scaleUp;

		float t = 0f;
		float scaledTime = t / speed;

		while (t < speed) {
			scaledTime = t / speed;
			img.transform.localScale = Lerp(startScale, endScale, scaledTime);
			img.color = gradient.Evaluate(scaledTime);
			t += Time.unscaledDeltaTime;
			yield return null;
		}

		Object.Destroy(img.gameObject);

	}


	public static CoroutineWrapper LerpFill(this Image fillingImage, float targetAmount, float speed = 1f) {
		return CoroutineBehavior.StartCoroutine(LerpFill(targetAmount, fillingImage, speed));
	}
	public static CoroutineWrapper WriteSmooth(this TextMeshProUGUI label, string text) {
		return CoroutineBehavior.StartCoroutine(WriteLine_Smooth(label, text));
	}
	public static CoroutineWrapper ShowSmooth(this TextMeshProUGUI label, string text, float speed) {
		return CoroutineBehavior.StartCoroutine(WriteLine_ShowSmooth(label, text, speed));
	}
	public static CoroutineWrapper SmoothCounter(this TextMeshProUGUI label, float start, float end, float speed, string valueFormat = "0", string stringFormat = "{0}") {
		return CoroutineBehavior.StartCoroutine(WriteLine_SmoothCounter(label, start, end, speed, valueFormat, stringFormat));
	}

	public static IEnumerator WriteLine_ShowSmooth(TextMeshProUGUI target, string text, float speed) {

		target.text = text;
		target.color = Color.white;
		var gradient = FastAlphaGradient(Color.white.CopyAndChangeAlpha(0f), Color.white);
		float t = 0f;
		while (t < speed) {
			target.color = gradient.Evaluate(t / speed);
			t += Time.deltaTime;
			yield return null;
		}

	}

	public static  IEnumerator WriteLine_Smooth(TextMeshProUGUI target, string text) {

		int _smoothPower = 6;
		float _realWriteSpeed = 0.01f;

		Color mainTextColor = Color.white;

		target.text = "";
		target.text = text;
		target.color = Color.clear;

		int textLength = text.Length;
		int currCharIndex = -_smoothPower;
		int materialIndex;
		int vertexIndex;
		int iteration = 0;
		TMP_TextInfo textInfo = target.textInfo;

		Color32[] VertexColors;
		Color32 c0 = mainTextColor;
		Color32 c1 = mainTextColor;
		byte smooth = (byte)(Mathf.RoundToInt((255f / _smoothPower)));

		while (true) {
			iteration = 0;
			for (int i = currCharIndex; i < currCharIndex + _smoothPower; i++) {
				if (i >= textLength || i < 0 || !textInfo.characterInfo[i].isVisible) {
					continue;
				}
				materialIndex = textInfo.characterInfo[i].materialReferenceIndex;
				VertexColors = textInfo.meshInfo[materialIndex].colors32;
				vertexIndex = textInfo.characterInfo[i].vertexIndex;
				c0.a = (byte)(smooth * (_smoothPower - iteration));
				c1.a = (byte)(c0.a - smooth);
				iteration++;

				VertexColors[vertexIndex + 0] = c0;
				VertexColors[vertexIndex + 1] = c0;
				VertexColors[vertexIndex + 2] = c1;
				VertexColors[vertexIndex + 3] = c1;
			}
			target.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
			yield return new WaitForSecondsRealtime(_realWriteSpeed);
			if (++currCharIndex >= textLength) {
				
				// Callback

				yield break;
			}
		}
	}

	public static IEnumerator WriteLine_SmoothCounter(TextMeshProUGUI target, float start, float end, float speed, string valueFormat = "0", string stringFormat = "{0}") {
		float time = 0f;
		while (time < speed) {
			target.text = Lerp(start, end, time / speed).ToString(valueFormat);
			time += Time.unscaledDeltaTime;
			yield return null;
		}
		target.text = string.Format(stringFormat, end.ToString(valueFormat));
	}

	public static IEnumerator ClipTransform(this Transform target, Transform to, float speed) {
		float time = 0f;
		Vector3 startPos = target.position;
		Quaternion startRoot = target.rotation;
		while (time < speed) {
			target.position = Lerp(startPos, to.position, time / speed);
			target.rotation = Quaternion.Lerp(target.rotation, to.rotation, 10f * Time.unscaledDeltaTime);
			time += Time.unscaledDeltaTime;
			yield return null;
		}
	}

	public static Gradient FastAlphaGradient(Color a, Color b = default) {
		if (b == default) {
			b = a.CopyAndChangeAlpha(0f);
		}
		return new Gradient() {
			alphaKeys = new GradientAlphaKey[] { new GradientAlphaKey(a.a, 0f), new GradientAlphaKey(b.a, 1f) },
			colorKeys = new GradientColorKey[] { new GradientColorKey(a, 0f), new GradientColorKey(b, 1f) }
		};
	}

	// 99 - skip changes
	public static Color CopyAndChange(this Color color, float r = 99f, float g = 99f, float b = 99f, float a = 99f) {
		return new Color(r == 99 ? color.r : r, g == 99 ? color.g : g, b == 99 ? color.b : b, a == 99 ? color.a : a);
	}
	public static Color CopyAndChangeAlpha(this Color color, float a) {
		return new Color(color.r, color.g, color.b, a);
	}

	public static Vector3 Lerp(Vector3 a, Vector3 b, float t) {
		return a + ((b - a) * t);
	}
	public static Quaternion Lerp(Quaternion a, Quaternion b, float t) {

		return new Quaternion(
			a.x + ((b.x - a.x) * t),
			a.y + ((b.y - a.y) * t),
			a.z + ((b.z - a.z) * t),
			a.w + ((b.w - a.w) * t));

	}
	public static float Lerp(float a, float b, float t) {
		return a + ((b - a) * t);
	}



}

public class TwinTextWriter {

	private int _smoothPower = 6;
	private float _writeSpeed = 0.01f;
	private float _realWriteSpeed = 0.01f;

	public bool IsWriteNow { get { return _activeWriter != null; } }
	public bool IsAccelerated { get { return _realWriteSpeed == 0f; } }

	public void WriteText(WriterParams settings) {
		_realWriteSpeed = _writeSpeed;
		_activeWriter?.Stop();
		_activeWriter = CoroutineBehavior.StartCoroutine(Writer(settings));
	}
	public void StopWrite() {
		if (_activeWriter != null) {
			_activeWriter.Stop();
		}
	}
	public void Accelerate() {
		_realWriteSpeed = 0f;
	}

	public struct WriterParams {
		public string text;
		public TextMeshProUGUI target;
		public Color mainTextColor;
		public Action onEndCallback;
		public float callbackAwait;
		public float startWait;

		public WriterParams(string _text, TextMeshProUGUI _target, Color _mainTextColor) {
			text = _text; target = _target; mainTextColor = _mainTextColor;
			onEndCallback = null;
			startWait = callbackAwait = 0f;
		}
		public WriterParams(string _text, TextMeshProUGUI _target, Color _mainTextColor, Action _callback, float _startWait, float _awaitCall) {
			text = _text; target = _target; mainTextColor = _mainTextColor;
			onEndCallback = _callback;
			callbackAwait = _awaitCall;
			startWait = _startWait;
		}

	}

	private CoroutineWrapper _activeWriter;
	private IEnumerator Writer(WriterParams settings) {

		settings.target.text = "";
		settings.target.text = settings.text;
		settings.target.color = Color.clear;

		yield return new WaitForSecondsRealtime(settings.startWait);

		int textLength = settings.text.Length;
		int currCharIndex = -_smoothPower;
		int materialIndex;
		int vertexIndex;
		int iteration = 0;
		TMP_TextInfo textInfo = settings.target.textInfo;

		Color32[] VertexColors;
		Color32 c0 = settings.mainTextColor;
		Color32 c1 = settings.mainTextColor;
		byte smooth = (byte)(Mathf.RoundToInt((255f / _smoothPower)));

		while (true) {
			iteration = 0;
			for (int i = currCharIndex; i < currCharIndex + _smoothPower; i++) {
				if (i >= textLength || i < 0 || !textInfo.characterInfo[i].isVisible) {
					continue;
				}
				materialIndex = textInfo.characterInfo[i].materialReferenceIndex;
				VertexColors = textInfo.meshInfo[materialIndex].colors32;
				vertexIndex = textInfo.characterInfo[i].vertexIndex;
				c0.a = (byte)(smooth * (_smoothPower - iteration));
				c1.a = (byte)(c0.a - smooth);
				iteration++;

				VertexColors[vertexIndex + 0] = c0;
				VertexColors[vertexIndex + 1] = c0;
				VertexColors[vertexIndex + 2] = c1;
				VertexColors[vertexIndex + 3] = c1;
			}
			settings.target.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
			yield return new WaitForSecondsRealtime(_realWriteSpeed);
			if (++currCharIndex >= textLength) {
				yield return new WaitForSecondsRealtime(settings.callbackAwait);
				settings.onEndCallback?.Invoke();
				yield break;
			}
		}
	}
}
