using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BeeFireflyBoost : MonoBehaviour {
	
	public int FirefliesNeededForBoost = 3;
	public int CurrentFirefliesInARow;
	
	HoneyBar honeyBar;
	BorderFlasher borderFlasher;
	bool alreadySetBorder;
	
	void Start() {
		honeyBar = Object.FindObjectOfType<HoneyBar>();
		borderFlasher = Object.FindObjectOfType<BorderFlasher>();
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.name.Contains("Missed")){
			CurrentFirefliesInARow = 0;
			borderFlasher.SetTarget(0);
		} else {
			CurrentFirefliesInARow++;
			borderFlasher.Flash();
		}
		Destroy(other.transform.root.gameObject);
		
		if (CurrentFirefliesInARow >= FirefliesNeededForBoost) {
			honeyBar.EnableBoost();
			if (CurrentFirefliesInARow == FirefliesNeededForBoost)
				borderFlasher.SetTarget(0.75f);
		} else {
			honeyBar.DisableBoost();
		}
	}
}