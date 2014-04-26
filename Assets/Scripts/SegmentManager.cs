using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SegmentManager : MonoBehaviour {

	public GameObject[] SegmentPrefabs;

	public float ScrollSpeed = 1;
	public float ScrollAcceleration = 0.1f;
	public float MaxScrollSpeed = 10;

	List<Segment> segments;
	List<Segment> segmentsToKill = new List<Segment>();
	float segmentGap = 3.2f;
	float segmentPositionKillLimit = -6.5f;
	float segmentPositionSpawnLimit = 6.5f;

	float segmentOpenTime = 1.5f;
	float segmentCloseTime = 0.75f;
	
	public bool ActionAllowed { get; private set; }
	public bool ScrollingAllowed { get; private set; }
	public bool IsOpen { get; private set; }

	void Start() {
		segments = new List<Segment>(GetComponentsInChildren<Segment>());
		Object.FindObjectOfType<InputManager>().RegisterForRightClicks(gameObject);

		ActionAllowed = true;
		ScrollingAllowed = true;
		IsOpen = false;
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
			
			if (rightmostSegmentPos.x <= segmentPositionSpawnLimit) {
				GameObject newSegmentObj = (GameObject) Instantiate(SegmentPrefabs[Mathf.FloorToInt(Random.value * SegmentPrefabs.Length)]);
				newSegmentObj.transform.parent = transform;
				newSegmentObj.transform.localPosition = new Vector3(rightmostSegmentPos.x + segmentGap, rightmostSegmentPos.y, rightmostSegmentPos.z);
				
				Segment newSegment = newSegmentObj.GetComponent<Segment>();
				segments.Add(newSegment);
			}
			
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

	void OnRightClick() {
		if (!ActionAllowed)
			return;

		if (IsOpen) {
			foreach (var segment in segments) {
				segment.Close(segmentCloseTime);
			}
			StartCoroutine(WaitForAnimation(segmentCloseTime));
		} else {
			ScrollingAllowed = false;
			foreach (var segment in segments) {
				segment.Open(segmentOpenTime);
			}
			StartCoroutine(WaitForAnimation(segmentOpenTime));
			ScrollingAllowed = false;
		}
		ActionAllowed = false;
		IsOpen = !IsOpen;
	}

	IEnumerator WaitForAnimation(float seconds) {
		yield return new WaitForSeconds(seconds);
		ActionAllowed = true;
		if (!IsOpen)
			ScrollingAllowed = true;
	}
}
