using UnityEngine;
using System.Collections;

public class Segment : MonoBehaviour {

	public GameObject Top;
	public GameObject Bottom;

	float OpenOffset = 2.2f;

	Vector3 originalTop;
	Vector3 originalBottom;
	Vector3 destinationTop;
	Vector3 destinationBottom;

	void Start() {
		originalTop = Top.transform.localPosition;
		originalBottom = Bottom.transform.localPosition;
		destinationTop = new Vector3(originalTop.x, originalTop.y + OpenOffset, originalTop.z);
		destinationBottom = new Vector3(originalBottom.x, originalBottom.y - OpenOffset, originalBottom.z);
	}

	public void Open(float openTime) {
		iTween.MoveTo(Top, iTween.Hash(
			"position", destinationTop,
			"time", openTime,
			"easeType", iTween.EaseType.easeOutElastic,
			"isLocal", true
			));
		iTween.MoveTo(Bottom, iTween.Hash(
			"position", destinationBottom,
			"time", openTime,
			"easeType", iTween.EaseType.easeOutElastic,
			"isLocal", true
			));
	}

	public void Close(float closeTime) {
		iTween.MoveTo(Top, iTween.Hash(
			"position", originalTop,
			"time", closeTime,
			"easeType", iTween.EaseType.easeInBack,
			"isLocal", true
			));
		iTween.MoveTo(Bottom, iTween.Hash(
			"position", originalBottom,
			"time", closeTime,
			"easeType", iTween.EaseType.easeInBack,
			"isLocal", true
			));
	}
}
