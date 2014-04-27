using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RotateToVelocity : MonoBehaviour {

	public float RotationScale = 120f;
	Vector3 oldPosition;

	void Start() {
		oldPosition = transform.position;
	}

	void Update() {
		var velocity = transform.position - oldPosition;
		if (velocity != Vector3.zero) {
			var newRotation = velocity.y * RotationScale;
			var currentRotation = transform.localRotation.eulerAngles;
			transform.localRotation = Quaternion.Euler(currentRotation.x, currentRotation.y, newRotation);
		}
		oldPosition = transform.position;
	}
}
