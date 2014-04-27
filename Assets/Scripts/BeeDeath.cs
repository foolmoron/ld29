using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BeeDeath : MonoBehaviour {

	public GameObject[] ActivateOnDeath;
	public bool Dying {get; private set;}
	
	CameraFadeIn cameraFadeIn;
	SegmentManager segmentManager;
	BeeGuard beeGuard;
	FollowMouse followMouse;
	Animator[] animators;
	List<Collider2D> electricsInside = new List<Collider2D>();

	void Start() {
		cameraFadeIn = Object.FindObjectOfType<CameraFadeIn>();
		segmentManager = Object.FindObjectOfType<SegmentManager>();
		beeGuard = transform.root.GetComponentInChildren<BeeGuard>();
		followMouse = transform.root.GetComponentInChildren<FollowMouse>();
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
		if (Dying || segmentManager.IsOpen || beeGuard.IsInGuard || electricsInside.Count == 0)
			return;

		Dying = true;

		segmentManager.StopScrolling();
		iTween.Stop(followMouse.gameObject);
		followMouse.enabled = false;
		foreach (var obj in ActivateOnDeath) {
			obj.SetActive(true);
		}
		foreach (var animator in animators) {
			animator.Play("Dead");
		}

		StartCoroutine(Util.PerformActionWithDelay(2f,() => {		
			cameraFadeIn.FadeOut(() => {
				Application.LoadLevel(Application.loadedLevel);
			});
		}));
	}
}
