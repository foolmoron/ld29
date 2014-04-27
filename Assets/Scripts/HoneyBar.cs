using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HoneyBar : MonoBehaviour {

	public SpriteRenderer HoneyFill;
	public GameObject HoneyBarParticles;

	public float HoneyLevel = 0f;

	void Start() {
	}

	void Update() {
		var newHoneyJars = Mathf.FloorToInt(HoneyLevel);
		HoneyLevel -= newHoneyJars;
		
		var currentFillScale = HoneyFill.transform.localScale;
		currentFillScale.y = HoneyLevel;
		HoneyFill.transform.localScale = currentFillScale;
		
		var topOfFill = HoneyFill.bounds.extents.y + HoneyFill.transform.position.y;
		var currentParticlePosition = HoneyBarParticles.transform.position;
		currentParticlePosition.y = topOfFill;
		HoneyBarParticles.transform.position = currentParticlePosition;
	}

	public void AddHoney(float honey) {
		HoneyLevel += honey;
	}

	public void StopParticles() {
		HoneyBarParticles.particleSystem.enableEmission = false;
	}

	public void StartParticles() {
		HoneyBarParticles.particleSystem.enableEmission = true;
	}
}