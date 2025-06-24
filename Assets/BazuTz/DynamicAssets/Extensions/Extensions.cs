
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System;
using UnityEngine.EventSystems;
using Unity.Mathematics;

public static class Extensions {

	public static bool AddWithoutDoubles<T>(this IList<T> to, T what) {
		if (what != null && !to.Contains(what)) {
			to.Add(what);
			return true;
		}
		return false;
	}
	public static void AddWithoutDoubles<T>(this IList<T> to,params T[] what) {
		for (int i = 0; i < what.Length; i++) {
			if (what[i] != null && !to.Contains(what[i])) {
				to.Add(what[i]);
			}
		}
	}

	public static bool IsOneOf<T>(this T _obj, params T[] _objects) {
		return _objects.ContainsAny(_obj);
	}
	public static bool IsOneOf<T>(this T[] _oneOfThis, params T[] _objects) {
		foreach (var obj in _oneOfThis) {
			if (_objects.ContainsAny(obj)) {
				return true;
			}
		}
		return false;
	}

	public static bool RemoveWhere<T>(this IList<T> lst, Predicate<T> filter) {
		for (int i = 0; i < lst.Count; i++) {
			if (filter(lst[i])) {
				lst.RemoveAt(i);
				return true;
			}
		}
		return false;
	}
	public static bool RemoveWhere<T>(this IList<T> lst, Predicate<T> filter, out T removed) {
		for (int i = 0; i < lst.Count; i++) {
			if (filter(lst[i])) {
				removed = lst[i];
				lst.RemoveAt(i);
				return true;
			}
		}
		removed = default;
		return false;
	}
	public static bool RemoveAllWhere<T>(this IList<T> lst, Predicate<T> filter) {
		bool result = false;
		for (int i = 0; i < lst.Count; i++) {
			if (filter(lst[i])) {
				lst.RemoveAt(i);
				result = true;
			}
		}
		return result;
	}

	public static bool ContainsAny<T>(this IList<T> _obj, params T[] _objects) {
		for (int i = 0; i < _objects.Length; i++) {
			if (_obj.Contains(_objects[i])) return true;
		}
		return false;
	}

	public static bool ContaitsWhere<T, J>(this Dictionary<T, J> dict, Predicate<J> filter) {
		foreach (var kvp in dict) {
			if (filter.Invoke(kvp.Value)) {
				return true;
			}
		}
		return false;
	}
	public static bool ContaitsWhere<T>(this IList<T> lst, Predicate<T> filter) {
		foreach (var el in lst) {
			if (filter(el)) {
				return true;
			}
		}
		return false;
	}
	public static bool ContaitsWhere<T>(this IList<T> lst, Predicate<T> filter, out T result) {
		foreach (var el in lst) {
			if (filter(el)) {
				result = el;
				return true;
			}
		}
		result = default;
		return false;
	}
	public static bool ContaitsWhere<T>(this T[] array, Predicate<T> filter) {
		foreach (var el in array) {
			if (filter(el)) {
				return true;
			}
		}
		return false;
	}
	public static bool ContaitsWhere<T>(this T[] array, int lenght, Predicate<T> filter) {
		for (int i = 0; i < lenght; i++) {
			if (filter(array[i])) {
				return true;
			}
		}
		return false;
	}
	public static bool ContaitsWhere<T>(this T[] array, Predicate<T> filter, out T result) {
		foreach (var el in array) {
			if (filter(el)) {
				result = el;
				return true;
			}
		}
		result = default;
		return false;
	}

	public static int FindFirstIndexOf<T>(this IList<T> array, Predicate<T> filter) {
		for (int i = 0; i < array.Count; i++) {
			if (filter(array[i])) {
				return i;
			}
		}
		return -1;
	}
	public static int FindFirstIndexOf<T>(this T[] array, Predicate<T> filter) {
		for (int i = 0; i < array.Length; i++) {
			if (filter(array[i])) {
				return i;
			}
		}
		return -1;
	}

	public static bool TryGet<T>(this object link) where T : class {
		if (link is T tAsJ) {
			return true;
		} else if (link is MonoBehaviour mono) {
			return mono.TryGetComponent<T>(out var t);
		}
		return false;
	}

	public static bool TryGet<T>(this object link, out T component) where T : class {
		if (link is T tAsJ) {
			component = tAsJ;
			return true;
		} else if (link is MonoBehaviour mono) {
			if (mono.TryGetComponent(out T tComponent)) {
				component = tComponent;
				return true;
			} else {
				var c = mono.transform.GetComponentInParent<T>(true);
				if (c != null) {
					component = c;
					return true;
				}
			}
		}
		component = null;
		return false;
	}

	// Any
	public static bool IsNullOrEmpty<T>(this T[] array) {
		return array == null || array.Length == 0;
	}
	// Any
	public static bool IsNullOrEmpty<T>(this IList<T> list) {
		return list == null || list.Count == 0;
	}
	
	public static void MoveTo<T>(this List<T> _from, List<T> _to, bool _clearFrom = true) {
		_to.AddRange(_from);
		if (_clearFrom) {
			_from.Clear();
		}
	}
	public static T RemoveLast<T>(this IList<T> _from) {
		if (_from.Count <= 0) {
			return default;
		}
		T obj = _from[^1];
		_from.RemoveAt(_from.Count - 1);
		return obj;
	}

	public static bool IsPointerOverUIObject() {
		PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
		eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		List<RaycastResult> results = new List<RaycastResult>();
		EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
		return results.Count > 0;
	}

	public static List<T> CastBox<T>(this BoxCollider castReferance, Predicate<T> filter) {
		List<T> components = new List<T>();
		Vector3 castSize = castReferance.size;
		Vector3 multiplyer = castReferance.transform.parent == null ? castReferance.transform.localScale : castReferance.transform.parent.localScale;
		Quaternion rotation = castReferance.transform.parent == null ? castReferance.transform.rotation : castReferance.transform.rotation * castReferance.transform.parent.rotation;
		castSize = new Vector3(castSize.x * multiplyer.x, castSize.y * multiplyer.y, castSize.z * multiplyer.z) / 2f;
		foreach (var hit in Physics.OverlapBox(castReferance.transform.TransformPoint(castReferance.center), castSize, rotation)) {
			if (hit.gameObject.TryGetComponent<T>(out var component) && filter(component)) {
				components.Add(component);
			}
		}
		return components.Distinct().ToList();
	}

	public static Transform MainParent(this Transform transform) {
		var parent = transform;
		while (parent.parent != null) {
			parent = parent.parent;
		}
		return parent;
	}

	public static List<Transform> Transforms(this Transform parent) {
		List<Transform> result = new List<Transform>();
		FindIn(parent);

		void FindIn(Transform target) {
			result.Add(target);
			foreach (Transform t in target) {
				result.Add(t);
				if (t.childCount > 0) {
					FindIn(t);
				}
			}
		}
		return result;
	}

	public static void Disabe(this GraphicRaycaster raycaster) {
		raycaster.enabled = false;
	}

	public static void Enable(this GraphicRaycaster raycaster) {
		raycaster.enabled = true;
	}

	public static bool OneDayHasAlreadyPassed(this DateTime last, DateTime now) {
		if (last.DayOfYear < now.DayOfYear || last.Year < now.Year) {
			return true;
		}
		return false;
	}

	public static void TrySetText(this TMP_Text text, string line) {
		if (text != null) text.text = line;
	}

	public static float SingedAndle(Quaternion a, Quaternion b) {

		Vector3 forwardA = a * Vector3.forward;
		Vector3 forwardB = b * Vector3.forward;

		/// Numeric angle on the X-Z plane (relative to world forward)
		float angleA = Mathf.Atan2(forwardA.x, forwardA.z) * Mathf.Rad2Deg;
		float angleB = Mathf.Atan2(forwardB.x, forwardB.z) * Mathf.Rad2Deg;

		/// Signed difference in these angles
		return Mathf.DeltaAngle(angleA, angleB);

	}

	public static T Random<T>(this T[] arr) {
		return arr[UnityEngine.Random.Range(0, arr.Length)];
	}
	public static T Random<T>(this IList<T> arr) {
		return arr[UnityEngine.Random.Range(0, arr.Count)];
	}

	public static void Shuffle<T>(this System.Random rng, T[] array) {
		int n = array.Length;
		while (n > 1) {
			int k = rng.Next(n--);
			T temp = array[n];
			array[n] = array[k];
			array[k] = temp;
		}
	}
	public static void Shuffle<T>(this IList<T> list, int rnd = int.MaxValue) {
		System.Random rng = rnd == int.MinValue ? new System.Random() : new System.Random(rnd);
		int n = list.Count;
		while (n > 1) {
			n--;
			int k = rng.Next(n + 1);
			T value = list[k];
			list[k] = list[n];
			list[n] = value;
		}
	}
	public static void Shuffle<T>(this T[] arr, int rnd = int.MaxValue) {
		System.Random rng = rnd == int.MinValue ? new System.Random() : new System.Random(rnd);
		int n = arr.Length;
		while (n > 1) {
			n--;
			int k = rng.Next(n + 1);
			T value = arr[k];
			arr[k] = arr[n];
			arr[n] = value;
		}
	}

	/// to slow
	public static T[] Add<T>(this T[] array, T obj) {
		T[] newAr = new T[array.Length + 1];
		Array.Copy(array, newAr, array.Length);
		newAr[^1] = obj;
		return newAr;
	}

	public static string ToSaveLine<T>(this IList<T> lst) {
		return string.Join("#", lst);
	}

	public static List<string> FromSaveLine(this string line) {
		return new List<string>(line.Split("#", StringSplitOptions.RemoveEmptyEntries));
	}

#if UNITY_EDITOR

	[ContextMenu("Catch all quest in project")]
	public static List<T> CatchAllScriptables<T>() where T : ScriptableObject {
		var pathes = UnityEditor.AssetDatabase.FindAssets($"t: { typeof(T).Name}").Select(UnityEditor.AssetDatabase.GUIDToAssetPath);
		return (from path in pathes select UnityEditor.AssetDatabase.LoadAssetAtPath(path, typeof(T)) as T).ToList();
	}

#endif


}