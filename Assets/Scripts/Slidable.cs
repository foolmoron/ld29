using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Slidable : MonoBehaviour {
	
	public bool Horizontal = false;
	public float MinPos = 0f;
	public float MaxPos = 0f;

	Camera camera;
	Collider2D collider;

	float initialPos;
	Vector3 previousMousePos;
	public bool Sliding {get; private set;}

	void Start() {
		camera = Object.FindObjectOfType<Camera>();
		collider = GetComponent<Collider2D>();
		if (!camera)
			Debug.LogError("Camera required to receive mouse input!");
		if (!collider)
			Debug.LogError("Collider2D required to receive mouse input!");

		Sliding = false;
		initialPos = Horizontal ? transform.localPosition.x : transform.localPosition.y;
	}

	void OnDisable() {
		Sliding = false;
	}

	void Update() {
		if (Input.GetMouseButtonDown(0)) {
			var worldMouse = camera.ScreenToWorldPoint(Input.mousePosition);
			if (collider.OverlapPoint(new Vector2(worldMouse.x, worldMouse.y))) {
				Sliding = true;
				previousMousePos = worldMouse;
			}
		} else if (Input.GetMouseButtonUp(0)) {
			Sliding = false;
		}

		if (Sliding) {
			var currentMousePos = camera.ScreenToWorldPoint(Input.mousePosition);
			var delta = currentMousePos - previousMousePos;

			if (Horizontal)
				transform.Translate(new Vector3(delta.x, 0));
			else
				transform.Translate(new Vector3(0, delta.y));

			var currentPos = Horizontal ? transform.localPosition.x : transform.localPosition.y; 
			if (currentPos < MinPos)
				currentPos = MinPos;
			else if (currentPos > MaxPos)
				currentPos = MaxPos;

			var newPos = transform.localPosition;
			if (Horizontal) newPos.x = currentPos; else newPos.y = currentPos;
			transform.localPosition = newPos;

			previousMousePos = currentMousePos;
		}
	}

	void OnDrawGizmosSelected() {
		Gizmos.color = Color.yellow;

		if (Horizontal) {
			Gizmos.DrawWireSphere(new Vector3(MinPos, transform.position.y), 0.25f);
			Gizmos.DrawWireSphere(new Vector3(MaxPos, transform.position.y), 0.25f);
		} else {
			Gizmos.DrawWireSphere(new Vector3(transform.position.x, MinPos), 0.25f);
			Gizmos.DrawWireSphere(new Vector3(transform.position.x, MaxPos), 0.25f);
		}
	}
}