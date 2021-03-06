﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SegmentManager : MonoBehaviour {

	public GameObject[] SegmentPrefabs;

	public float ScrollSpeed = 1;
	public float ScrollAcceleration = 0.1f;
	public float MaxScrollSpeed = 10;
	public float HoneyModifier = 0.1f;

	public GameObject FireflyPrefab;
	public float FireflyTopLaneY = 1.75f;
	public float FireflyBottomLaneY = -1.75f;
	List<GameObject> fireflies = new List<GameObject>();

	List<Segment> segments;
	List<Segment> segmentsToKill = new List<Segment>();
	float segmentGap = 3.2f;
	float segmentPositionKillLimit = -6.5f;
	float segmentPositionSpawnLimit = 6.5f;

	float segmentOpenTime = 1.5f;
	float segmentCloseTime = 0.75f;

	HoneyBar honeyBar;
	FollowMouse followMouse;
	
	public bool ActionAllowed { get; private set; }
	public bool ScrollingAllowed { get; private set; }
	public bool IsOpen { get; private set; }

	void Start() {
		segments = new List<Segment>(GetComponentsInChildren<Segment>());
		Object.FindObjectOfType<InputManager>().RegisterForRightClicks(gameObject);
		honeyBar = Object.FindObjectOfType<HoneyBar>();
		followMouse = Object.FindObjectOfType<FollowMouse>();
		
		foreach (var slidable in GetComponentsInChildren<Slidable>()) {
			slidable.enabled = false;
		}
		foreach (var rotatable in GetComponentsInChildren<Rotatable>()) {
			rotatable.enabled = false;
		}
	}

	void Update() {
		var rightmostSegmentPos = segments[0].transform.localPosition;

		if (ScrollingAllowed) {
			foreach (var segment in segments) {
				segment.transform.Translate(-ScrollSpeed * Time.deltaTime, 0, 0);
				if (segment.transform.localPosition.x > rightmostSegmentPos.x)
					rightmostSegmentPos = segment.transform.localPosition;

				if (segment.transform.localPosition.x < segmentPositionKillLimit)
					segmentsToKill.Add(segment);
			}

			fireflies.RemoveAll(obj => obj == null);
			foreach (var firefly in fireflies) {
				firefly.transform.Translate(-ScrollSpeed * Time.deltaTime, 0, 0);
			}
			
			if (rightmostSegmentPos.x <= segmentPositionSpawnLimit) {
				GameObject newSegmentObj = (GameObject) Instantiate(SegmentPrefabs[Mathf.FloorToInt(Random.value * SegmentPrefabs.Length)]);
				newSegmentObj.transform.parent = transform;
				newSegmentObj.transform.localPosition = new Vector3(rightmostSegmentPos.x + segmentGap, rightmostSegmentPos.y, rightmostSegmentPos.z);
				
				Segment newSegment = newSegmentObj.GetComponent<Segment>();
				segments.Add(newSegment);

				var fireflyY = (Random.value < 0.5f) ? FireflyTopLaneY: FireflyBottomLaneY;
				var newFirefly = (GameObject) Instantiate(FireflyPrefab, new Vector3(newSegment.transform.position.x, fireflyY), Quaternion.identity);
				fireflies.Add(newFirefly);
			}

			honeyBar.AddHoney(ScrollSpeed * Time.deltaTime * HoneyModifier);

			ScrollSpeed += ScrollAcceleration * Time.deltaTime;
			if (ScrollSpeed >= MaxScrollSpeed)
				ScrollSpeed = MaxScrollSpeed;
		}

		foreach (var segment in segmentsToKill) {
			segments.Remove(segment);
			Destroy(segment.gameObject);
		}
		segmentsToKill.Clear();
	}

	public void StartScrolling() {
		ActionAllowed = true;
		ScrollingAllowed = true;
	}

	public void StopScrolling() {
		ActionAllowed = false;
		ScrollingAllowed = false;
	}

	void OnRightClick() {
		if (!ActionAllowed)
			return;

		if (IsOpen) {
			followMouse.enabled = true;
			foreach (var segment in segments) {
				segment.Close(segmentCloseTime);
			}
			StartCoroutine(WaitForAnimation(segmentCloseTime, true));
			foreach (var electricGrid in GetComponentsInChildren<ElectricGrid>()) {
				electricGrid.TurnOn();
			}
			foreach (var slidable in GetComponentsInChildren<Slidable>()) {
				slidable.enabled = false;
			}
			foreach (var rotatable in GetComponentsInChildren<Rotatable>()) {
				rotatable.enabled = false;
			}
		} else {
			ScrollingAllowed = false;
			followMouse.enabled = false;
			foreach (var segment in segments) {
				segment.Open(segmentOpenTime);
			}
			StartCoroutine(WaitForAnimation(segmentOpenTime, false));
			ScrollingAllowed = false;
			honeyBar.StopParticles();
			foreach (var electricGrid in GetComponentsInChildren<ElectricGrid>()) {
				electricGrid.TurnOff();
			}
			foreach (var slidable in GetComponentsInChildren<Slidable>()) {
				slidable.enabled = true;
			}
			foreach (var rotatable in GetComponentsInChildren<Rotatable>()) {
				rotatable.enabled = true;
			}
		}
		if (IsOpen)
			ActionAllowed = false; // allow closing during opening animation
		if (!IsOpen)
			IsOpen = true; // go into open state instantly
	}

	IEnumerator WaitForAnimation(float seconds, bool closing) {
		yield return new WaitForSeconds(seconds);
		ActionAllowed = true;
		if (closing) {
			IsOpen = false; // wait until animation is done to go into closed state
			ScrollingAllowed = true;
			honeyBar.StartParticles();
		}
	}
}
