
using UnityEngine;
using System.Collections.Generic;

public interface ILogger {
	void Log(string value);
	void Clear();
}

public abstract class MonoDebugLogger : MonoBehaviour, ILogger {

	public abstract void Log(string value);
	public abstract void Clear();

	public virtual void Awake() {
		DebugUtility.AddLogger(this);
	}

	public virtual void OnDestroy() {
		DebugUtility.RemoveLogger(this);
	}

}

public static class DebugUtility {

	private static List<ILogger> s_Loggers = new List<ILogger>();

	public static void AddLogger(ILogger logger) {
		if (!s_Loggers.Contains(logger)) {
			s_Loggers.Add(logger);
		}
	}

	public static void RemoveLogger(ILogger logger) {
		s_Loggers.Remove(logger);
	}

	public static void Log(string value) {
		Debug.Log(value);
		s_Loggers.ForEach(x => x.Log(value));
	}

	public static void LogError(string value) {
		Debug.LogError(value);
		s_Loggers.ForEach(x => x.Log(value));
	}

	public static void ClearAllLogs() {
		s_Loggers.ForEach(x => x.Clear());
	}

}
