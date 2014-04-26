using UnityEngine;
using System.Collections;

public class Bee : MonoBehaviour {

	void Start() {
	}

	void OnTriggerEnter2D(Collider2D other) {
		Debug.Log("Trig");
		Debug.Log(other);
	}

	void OnCollisionEnter2D(Collision2D collision) {
		Debug.Log("Col");
		Debug.Log(collision);
	}
}
