
using UnityEngine;
using System.Collections;

public enum InstructionOrder {
	AfterAll, BeaforeAll
}

public abstract class SceneStartInstruction : MonoBehaviour {

	[field: SerializeField] public InstructionOrder Order { get; private set; }

	public virtual IEnumerator Initialize() {
		yield return null;
	}

	public virtual IEnumerator Dispose() {
		yield return null;
	}

}

[DefaultExecutionOrder(299)]
public class SceneInPoint : MonoBehaviour {

	[SerializeField] private SceneManager[] _sceneManagers;
	[SerializeField] private SceneStartInstruction[] _startinstructions;

	public IEnumerator Initialize() {

		foreach (var instruction in _startinstructions) {
			if (instruction.Order == InstructionOrder.BeaforeAll) {
				yield return CoroutineBehavior.StartCoroutine(instruction.Initialize());
			}
		}
		
		foreach (var m in _sceneManagers) {
			Managers.AddManager(m.GetType(), m, false);
		}

		foreach (var m in _sceneManagers) {
			yield return CoroutineBehavior.StartCoroutine(m.OnInitialize());
		}

		foreach (var m in _sceneManagers) {
			yield return CoroutineBehavior.StartCoroutine(m.OnStart());
		}

		foreach (var m in _sceneManagers) {
			yield return CoroutineBehavior.StartCoroutine(m.Final());
		}

		foreach (var instruction in _startinstructions) {
			if (instruction.Order == InstructionOrder.AfterAll) {
				yield return CoroutineBehavior.StartCoroutine(instruction.Initialize());
			}
		}

	}

	public IEnumerator Dispose() {

		foreach (var instruction in _startinstructions) {
			yield return CoroutineBehavior.StartCoroutine(instruction.Dispose());
		}

		foreach (var m in _sceneManagers) {
			Managers.RemoveManager(m);
		}

	}

}
