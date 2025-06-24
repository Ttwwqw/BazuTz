
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(GameFrames))]
public class BackNavigation : SceneManager, IDisposable {

	private Stack<Action> _backStack;

	private List<string> _listenBtns = new List<string>() { "Cancel" };

	public BackNavigation AddListenButns(params string[] btns) {
		foreach (var btn in btns) {
			_listenBtns.AddWithoutDoubles(btn);
		}
		return this;
	}

	public BackNavigation RemoveListenBtns(params string[] btns) {
		foreach (var btn in btns) {
			_listenBtns.Remove(btn);
		}
		return this;
	}

	public override IEnumerator OnInitialize() {
		_backStack = new Stack<Action>();
		return base.OnInitialize();
	}

	public BackNavigation AddBackEvent(Action action, bool addBackSound = true) {
		if (addBackSound) {
			action += ()=> Audio.Instance.PlaySound("PressBtn", 1f);
		}
		_backStack.Push(action);
		return this;
	}

	public BackNavigation CallBack() {
		if (_backStack.TryPeek(out var result)) {
			_backStack.Pop()?.Invoke();
		}
		return this;
	}

	public BackNavigation RemoveLast() {
		_backStack.Pop();
		return this;
	}

	public void Dispose() {
		//throw new NotImplementedException();
	}

	private void Update() {
		foreach (var btn in _listenBtns) {
			if (Input.GetButtonDown(btn)) {
				CallBack();
				break;
			}
		}
	}

}
