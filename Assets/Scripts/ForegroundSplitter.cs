using UnityEngine;
using System.Collections;

public class ForegroundSplitter : MonoBehaviour {

	public GameObject Top;
	public GameObject Bottom;
	public float OpenSpeed = 2;
	public float CloseSpeed = 4;

	Vector3 originalTop;
	Vector3 originalBottom;
	Vector3 destinationTop;
	Vector3 destinationBottom;
	bool open;

	void Start() {
		Object.FindObjectOfType<InputManager>().RegisterForRightClicks(gameObject);

		originalTop = Top.transform.localPosition;
		originalBottom = Bottom.transform.localPosition;
		destinationTop = new Vector3(originalTop.x, originalTop.y + 2.2f, originalTop.z);
		destinationBottom = new Vector3(originalBottom.x, originalBottom.y - 2.2f, originalBottom.z);
	}
	
	void OnRightClick() {
		iTween.Stop(Top);
		iTween.Stop(Bottom);
		if (open) {
			iTween.MoveTo(Top, iTween.Hash(
				"position", originalTop,
				"speed", CloseSpeed,
				"easeType", iTween.EaseType.easeInBack,
				"isLocal", true
				));
			iTween.MoveTo(Bottom, iTween.Hash(
				"position", originalBottom,
				"speed", CloseSpeed,
				"easeType", iTween.EaseType.easeInBack,
				"isLocal", true
				));
		} else {
			iTween.MoveTo(Top, iTween.Hash(
				"position", destinationTop,
				"speed", OpenSpeed,
				"easeType", iTween.EaseType.easeOutElastic,
				"isLocal", true
				));
			iTween.MoveTo(Bottom, iTween.Hash(
				"position", destinationBottom,
				"speed", OpenSpeed,
				"easeType", iTween.EaseType.easeOutElastic,
				"isLocal", true
				));
		}
		if(Input.GetMouseButtonDown(2))
			Debug.Log("Pressed middle click.  " + 
		Time.realtimeSinceStartup);
		open = !open;
	}
}
