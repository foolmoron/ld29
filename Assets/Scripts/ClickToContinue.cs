using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClickToContinue : MonoBehaviour {

	public string SceneToGoTo;
	
	void Update() {
		if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) {
			Application.LoadLevel(SceneToGoTo);
		}
	}
}
