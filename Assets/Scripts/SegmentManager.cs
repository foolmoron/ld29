﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SegmentManager : MonoBehaviour {

	public GameObject[] SegmentPrefabs;

	public float ScrollSpeed = 1;
	public float ScrollAcceleration = 0.5f;
	public float MaxScrollSpeed = 8;

	List<Segment> segments;
	List<Segment> segmentsToKill = new List<Segment>();
	float segmentGap = 3.2f;
	float segmentPositionKillLimit = -6.5f;
	float segmentPositionSpawnLimit = 6.5f;
	public bool IsOpen { get; private set; }

	void Start() {
		segments = new List<Segment>(GetComponentsInChildren<Segment>());
		Object.FindObjectOfType<InputManager>().RegisterForRightClicks(gameObject);

		IsOpen = false;
	}

	void Update() {
		var rightmostSegmentPos = segments[0].transform.localPosition;

		foreach (var segment in segments) {
			segment.transform.Translate(-ScrollSpeed * Time.deltaTime, 0, 0);
			if (segment.transform.localPosition.x > rightmostSegmentPos.x)
				rightmostSegmentPos = segment.transform.localPosition;

			if (segment.transform.localPosition.x < segmentPositionKillLimit)
				segmentsToKill.Add(segment);
		}
		foreach (var segment in segmentsToKill) {
			segments.Remove(segment);
			Destroy(segment.gameObject);
		}
		segmentsToKill.Clear();

		if (rightmostSegmentPos.x <= segmentPositionSpawnLimit) {
			GameObject newSegmentObj = (GameObject) Instantiate(SegmentPrefabs[Mathf.FloorToInt(Random.value * SegmentPrefabs.Length)]);
			newSegmentObj.transform.parent = gameObject.transform;
			newSegmentObj.transform.localPosition = new Vector3(rightmostSegmentPos.x + segmentGap, rightmostSegmentPos.y, rightmostSegmentPos.z);

			Segment newSegment = newSegmentObj.GetComponent<Segment>();
			segments.Add(newSegment);
		}

		ScrollSpeed += ScrollAcceleration * Time.deltaTime;
		if (ScrollSpeed >= MaxScrollSpeed)
			ScrollSpeed = MaxScrollSpeed;
	}

	void OnRightClick() {
		if (IsOpen) {
			foreach (var segment in segments) {
				segment.Close();
			}
		} else {
			foreach (var segment in segments) {
				segment.Open();
			}
		}
		IsOpen = !IsOpen;
	}
}