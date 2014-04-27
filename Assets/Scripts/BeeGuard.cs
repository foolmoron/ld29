using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BeeGuard : MonoBehaviour {

	BeeDeath beeDeath;
	List<Collider2D> guardsInside = new List<Collider2D>();
	public bool IsInGuard { get { return guardsInside.Count > 0; } }
	
	void Start() {
		beeDeath = transform.root.GetComponentInChildren<BeeDeath>();
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		guardsInside.Add(other);
	}
	
	void OnTriggerExit2D(Collider2D other) {
		guardsInside.Remove(other);
		if (guardsInside.Count == 0)
			beeDeath.AttemptDie();
	}
}