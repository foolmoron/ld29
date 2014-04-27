using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BeeFireflyBoost : MonoBehaviour {

	public int FirefliesNeededForBoost = 3;
	public int CurrentFirefliesInARow;

	HoneyBar honeyBar;

	void Start() {
		honeyBar = Object.FindObjectOfType<HoneyBar>();
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.name.Contains("Missed")){
			CurrentFirefliesInARow = 0;
		} else {
			CurrentFirefliesInARow++;
		}
		Destroy(other.transform.root.gameObject);

		if (CurrentFirefliesInARow >= FirefliesNeededForBoost) {
			honeyBar.EnableBoost();
		} else {
			honeyBar.DisableBoost();
		}
	}
}