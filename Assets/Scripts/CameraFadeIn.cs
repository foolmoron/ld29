using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CameraFadeIn : MonoBehaviour {

	public SpriteRenderer OverlaySprite;
	public float FadeTime = 0.5f;

	SegmentManager segmentManager;

	bool fading = true;
	float startAlpha;
	float endAlpha;
	Action afterFade;
	float fadeTime;

	void Start() {
		segmentManager = UnityEngine.Object.FindObjectOfType<SegmentManager>();

		if (segmentManager) {
			segmentManager.StopScrolling();
			FadeIn(() => {
				OverlaySprite.gameObject.SetActive(false);
				segmentManager.StartScrolling();
			});
		}
	}
	
	public void FadeOut(Action afterFadeOut) {
		startAlpha = 0;
		endAlpha = 1;
		fadeTime = 0;
		OverlaySprite.gameObject.SetActive(true);
		fading = true;
		afterFade = afterFadeOut;
	}
	
	public void FadeIn(Action afterFadeIn) {
		startAlpha = 1;
		endAlpha = 0;
		fadeTime = 0;
		OverlaySprite.gameObject.SetActive(true);
		fading = true;
		afterFade = afterFadeIn;
	}

	void Update() {
		if (!fading)
			return;

		fadeTime += Time.deltaTime;
		var interp = fadeTime / FadeTime;
		if (interp > 1) {
			if (afterFade != null)
				afterFade();
			fading = false;
			return;
		}

		var currentColor = OverlaySprite.color;
		currentColor.a = Mathf.Lerp(startAlpha, endAlpha, interp);
		OverlaySprite.color = currentColor;
	}
}