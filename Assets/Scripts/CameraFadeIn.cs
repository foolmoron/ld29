using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraFadeIn : MonoBehaviour {

	public SpriteRenderer OverlaySprite;
	public float FadeInTime = 2f;

	SegmentManager segmentManager;
	float fadeTime = 0f;

	void Start() {
		segmentManager = Object.FindObjectOfType<SegmentManager>();
		segmentManager.StopScrolling();

		OverlaySprite.gameObject.SetActive(true);
		var currentColor = OverlaySprite.color;
		currentColor.a = 1;
		OverlaySprite.color = currentColor;
	}

	void Update() {
		fadeTime += Time.deltaTime;
		var interp = fadeTime / FadeInTime;
		if (interp > 1) {
			OverlaySprite.gameObject.SetActive(false);
			segmentManager.StartScrolling();

			enabled = false;
			return;
		}

		var currentColor = OverlaySprite.color;
		currentColor.a = Mathf.Lerp(1, 0, interp);
		OverlaySprite.color = currentColor;
	}
}