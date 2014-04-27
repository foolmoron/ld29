using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public static class Util {

	public static IEnumerator PerformActionWithDelay(float seconds, Action action) {
		yield return new WaitForSeconds(seconds);
		if (action != null) action();
	}
}