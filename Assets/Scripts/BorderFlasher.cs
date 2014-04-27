using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(SpriteRenderer))]
public class BorderFlasher : MonoBehaviour {

	public AnimationCurve TargetCurve;
	public AnimationCurve FlashCurve;

	SpriteRenderer sprite;
	float originalAlpha;
	float targetAlpha;
	AnimationCurve currentCurve;
	float animationTime;

	void Start() {
		sprite = GetComponent<SpriteRenderer>();
	}

	void Update() {
		if (originalAlpha != targetAlpha) {
			animationTime += Time.deltaTime;
			var interp = currentCurve.Evaluate(animationTime);

			var currentAlpha = Mathf.Lerp(originalAlpha, targetAlpha, interp);
			var currentColor = sprite.color;
			currentColor.a = currentAlpha;
			sprite.color = currentColor;

			if (interp > 1)
				originalAlpha = targetAlpha;
		}
	}

	public void SetOriginal(float alpha) {
		originalAlpha = alpha;
	}

	public void SetTarget(float alpha) {
		currentCurve = TargetCurve;
		originalAlpha = sprite.color.a;
		targetAlpha = alpha;
		animationTime = 0;
	}
	
	public void Flash() {
		currentCurve = FlashCurve;
		originalAlpha = sprite.color.a;
		targetAlpha = originalAlpha + 0.5f;
		animationTime = 0;
	}
}