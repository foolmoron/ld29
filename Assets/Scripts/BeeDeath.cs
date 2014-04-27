using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BeeDeath : MonoBehaviour {

	BeeGuard beeGuard;
	List<Collider2D> electricsInside = new List<Collider2D>();

	void Start() {
		beeGuard = transform.root.GetComponentInChildren<BeeGuard>();
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		electricsInside.Add(other);
		AttemptDie();
	}

	void OnTriggerExit2D(Collider2D other) {
		electricsInside.Remove(other);
	}

	public void AttemptDie() {
		if (beeGuard.IsInGuard || electricsInside.Count == 0)
			return;

		Debug.Log("DEAD'D");
	}
}
