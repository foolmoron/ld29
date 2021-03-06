﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BeeDeath : MonoBehaviour {

	public GameObject[] CreateOnDeath;
	public bool Dying {get; private set;}
	
	CameraFadeIn cameraFadeIn;
	SegmentManager segmentManager;
	BeeGuard beeGuard;
	FollowMouse followMouse;
	Animator[] animators;
	List<Collider2D> electricsInside = new List<Collider2D>();

	GameObject text;

	void Start() {
		cameraFadeIn = Object.FindObjectOfType<CameraFadeIn>();
		segmentManager = Object.FindObjectOfType<SegmentManager>();
		beeGuard = transform.root.GetComponentInChildren<BeeGuard>();
		followMouse = transform.root.GetComponentInChildren<FollowMouse>();
		animators = transform.root.GetComponentsInChildren<Animator>();
		text = GameObject.Find("Text");
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
		foreach (var obj in CreateOnDeath) {
			var newObj = (GameObject) Instantiate(obj, transform.position, transform.rotation);
			newObj.transform.parent = transform.parent;
			var currentPos = newObj.transform.localPosition;
			currentPos.z = -2;
			newObj.transform.localPosition = currentPos;
		}
		foreach (var animator in animators) {
			animator.Play("Dead");
		}
		
		iTween.MoveTo(text, iTween.Hash(
			"position", Vector3.zero,
			"time", 2f,
			"easeType", iTween.EaseType.easeOutElastic,
			"isLocal", true
		));
		StartCoroutine(Util.PerformActionWithDelay(2f,() => {	
			var cont = Object.FindObjectOfType<ClickToContinue>();
			cont.enabled = true;
		}));
	}
}
