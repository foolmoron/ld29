using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Rotatable : MonoBehaviour {

	Camera camera;
	Collider2D collider;

	float previousAngle;
	public bool Rotating {get; private set;}

	void Start() {
		camera = Object.FindObjectOfType<Camera>();
		collider = GetComponent<Collider2D>();
		if (!camera)
			Debug.LogError("Camera required to receive mouse input!");
		if (!collider)
			Debug.LogError("Collider2D required to receive mouse input!");

		Rotating = false;
	}

	void OnDisable() {
		Rotating = false;
	}

	void Update() {
		if (Input.GetMouseButtonDown(0)) {
			var worldMouse = camera.ScreenToWorldPoint(Input.mousePosition);
			if (collider.OverlapPoint(new Vector2(worldMouse.x, worldMouse.y))) {
				Rotating = true;

				var mousePosNormalized = (worldMouse - transform.position).normalized;
				previousAngle = Mathf.Atan2(mousePosNormalized.y, mousePosNormalized.x);
			}
		} else if (Input.GetMouseButtonUp(0)) {
			Rotating = false;
		}

		if (Rotating) {
			var currentNormalized = (camera.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
			var currentAngle = Mathf.Atan2(currentNormalized.y, currentNormalized.x);
			var delta = currentAngle - previousAngle;

			transform.Rotate(0f, 0f, delta * Mathf.Rad2Deg);
			previousAngle = currentAngle;
		}
	}
}