using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputManager : MonoBehaviour {
	
	List<GameObject> leftClickListeners = new List<GameObject>();
	List<GameObject> rightClickListeners = new List<GameObject>();

	void Update() {
		if (Input.GetMouseButtonDown(0)) {
			for (int i = 0; i < leftClickListeners.Count; i++) {
				leftClickListeners[i].SendMessage("OnLeftClick");
			}
		}
		if (Input.GetMouseButtonDown(1)) {
			for (int i = 0; i < rightClickListeners.Count; i++) {
				rightClickListeners[i].SendMessage("OnRightClick");
			}
		}
	}
	
	public void RegisterForLeftClicks(GameObject listener) {
		leftClickListeners.Add(listener);
	}
	
	public void RegisterForRightClicks(GameObject listener) {
		rightClickListeners.Add(listener);
	}
}
