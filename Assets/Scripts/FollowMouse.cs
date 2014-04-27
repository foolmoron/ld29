using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FollowMouse : MonoBehaviour {

	public enum Lane { TOP, BOTTOM }
	Camera camera;
	
	public float TopLanePosition = 1.75f;
	public float BottomLanePosition = -1.75f;
	public float LaneSwitchBoundary = 0f;
	public Lane TargetLane = Lane.TOP;
	public float LaneSwitchSpeed = 5f;

	void Start() {
		camera = Object.FindObjectOfType<Camera>();
		if (!camera)
			Debug.LogError("Camera required to receive mouse input!");
	}

	void Update() {
		var oldTarget = TargetLane;
		var currentMousePos = camera.ScreenToWorldPoint(Input.mousePosition);
		if (currentMousePos.y > LaneSwitchBoundary) {
			TargetLane = Lane.TOP;
		} else {
			TargetLane = Lane.BOTTOM;
		}

		if (TargetLane != oldTarget) {
			if (TargetLane == Lane.TOP) {
				iTween.MoveTo(gameObject, iTween.Hash(
					"position", new Vector3(transform.position.x, TopLanePosition),
					"speed", LaneSwitchSpeed,
					"easeType", iTween.EaseType.easeOutBack
				));
			} else if (TargetLane == Lane.BOTTOM) {
				iTween.MoveTo(gameObject, iTween.Hash(
					"position", new Vector3(transform.position.x, BottomLanePosition),
					"speed", LaneSwitchSpeed,
					"easeType", iTween.EaseType.easeOutBack
				));
			}
		}
	}

	void OnDrawGizmos() {
		Gizmos.color = Color.blue;
		Gizmos.DrawLine(new Vector3(-100, TopLanePosition), new Vector3(100, TopLanePosition));
		Gizmos.color = Color.red;
		Gizmos.DrawLine(new Vector3(-100, BottomLanePosition), new Vector3(100, BottomLanePosition));
		Gizmos.color = Color.magenta;
		Gizmos.DrawLine(new Vector3(-100, LaneSwitchBoundary), new Vector3(100, LaneSwitchBoundary));
	}
}