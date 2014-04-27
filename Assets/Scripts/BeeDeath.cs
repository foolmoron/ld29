using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BeeDeath : MonoBehaviour {

	public GameObject[] ActivateOnDeath;

	SegmentManager segmentManager;
	BeeGuard beeGuard;
	Animator[] animators;
	List<Collider2D> electricsInside = new List<Collider2D>();

	void Start() {
		segmentManager = Object.FindObjectOfType<SegmentManager>();
		beeGuard = transform.root.GetComponentInChildren<BeeGuard>();
		animators = transform.root.GetComponentsInChildren<Animator>();
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		electricsInside.Add(other);
		AttemptDie();
	}

	void OnTriggerExit2D(Collider2D other) {
		electricsInside.Remove(other);
	}

	public void AttemptDie() {
		if (segmentManager.IsOpen || beeGuard.IsInGuard || electricsInside.Count == 0)
			return;

		segmentManager.StopScrolling();
		foreach (var obj in ActivateOnDeath) {
			obj.SetActive(true);
		}
		foreach (var animator in animators) {
			animator.Play("Dead");
		}
	}
}
