using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HoneyBar : MonoBehaviour {

	public SpriteRenderer HoneyFill;
	public GameObject HoneyBarParticles;
	public GameObject HoneyBarFireflies;
	
	public float HoneyLevel = 0f;
	public int HoneyJars = 0;
	public bool IsBoosted {get; private set;}

	public Vector2 JarBasePosition = new Vector2(-0.35f, -1.15f); 
	public float JarSize = 0.18f;
	public GameObject Jar1Prefab;
	public GameObject Jar2Prefab;
	public GameObject Jar3Prefab;
	public GameObject Jar4Prefab;
	
	GameObject[] level1Jars = new GameObject[5];
	GameObject[] level2Jars = new GameObject[5];
	GameObject[] level3Jars = new GameObject[5];
	GameObject[] level4Jars = new GameObject[5];
	
	GUIText collectedText;
	GUIText highscoreText;
	float highscore;

	void Start() {
		DisableBoost();
		
		collectedText = GameObject.Find("Collected").GetComponent<GUIText>();
		highscoreText = GameObject.Find("Highscore").GetComponent<GUIText>();

		highscore = PlayerPrefs.GetFloat("highscore", 0);
	}

	void Update() {
		var newHoneyJars = Mathf.FloorToInt(HoneyLevel);
		for (int i = 0; i < newHoneyJars; i++)
			AddHoneyJar();
		HoneyLevel -= newHoneyJars;
		
		var currentFillScale = HoneyFill.transform.localScale;
		currentFillScale.y = HoneyLevel;
		HoneyFill.transform.localScale = currentFillScale;
		
		var topOfFill = HoneyFill.bounds.extents.y + HoneyFill.transform.position.y;
		var currentParticlePosition = HoneyBarParticles.transform.position;
		currentParticlePosition.y = topOfFill;
		HoneyBarParticles.transform.position = currentParticlePosition;

		var totalHoney = HoneyJars + HoneyLevel;
		collectedText.text = "Collected " + totalHoney.ToString("0.00");

		if (totalHoney >= highscore) {
			highscoreText.color = Color.white;
			highscore = totalHoney;
			PlayerPrefs.SetFloat("highscore", totalHoney);
		}
		highscoreText.text = "High: " + highscore.ToString("0.00");
	}

	public void AddHoney(float honey) {
		if (IsBoosted)
			honey *= 2;
		HoneyLevel += honey;
	}

	void AddHoneyJar() {
		HoneyJars++;

		var currentJars = HoneyJars;
		var level4s = currentJars / 125;
		currentJars -= level4s * 125;
		var level3s = currentJars / 25;
		currentJars -= level3s * 25;
		var level2s = currentJars / 5;
		currentJars -= level2s * 5;
		var level1s = currentJars;

		var basePosition = JarBasePosition;
		for (int i = 0; i < level1Jars.Length; i++) {
			var currentJar = level1Jars[i];
			if (i < level1s && currentJar == null) { 
				currentJar = (GameObject) Instantiate(Jar1Prefab, 
				                                      transform.position + new Vector3(basePosition.x + i * JarSize, basePosition.y),
				                                      Quaternion.identity);
				level1Jars[i] = currentJar;
			} else if (i >= level1s && currentJar != null) {
				Destroy(currentJar);
				level1Jars[i] = null;
			}
		}
		basePosition.x = JarBasePosition.x;
		basePosition.y -= JarSize;
		for (int i = 0; i < level2Jars.Length; i++) {
			var currentJar = level2Jars[i];
			if (i < level2s && currentJar == null) { 
				currentJar = (GameObject) Instantiate(Jar2Prefab, 
				                                      transform.position + new Vector3(basePosition.x + i * JarSize, basePosition.y),
				                                      Quaternion.identity);
				level2Jars[i] = currentJar;
			} else if (i >= level2s && currentJar != null) {
				Destroy(currentJar);
				level2Jars[i] = null;
			}
		}
		basePosition.x = JarBasePosition.x;
		basePosition.y -= JarSize;
		for (int i = 0; i < level3Jars.Length; i++) {
			var currentJar = level3Jars[i];
			if (i < level3s && currentJar == null) { 
				currentJar = (GameObject) Instantiate(Jar3Prefab, 
				                                      transform.position + new Vector3(basePosition.x + i * JarSize, basePosition.y),
				                                      Quaternion.identity);
				level3Jars[i] = currentJar;
			} else if (i >= level3s && currentJar != null) {
				Destroy(currentJar);
				level3Jars[i] = null;
			}
		}
		basePosition.x = JarBasePosition.x;
		basePosition.y -= JarSize;
		for (int i = 0; i < level4Jars.Length; i++) {
			var currentJar = level4Jars[i];
			if (i < level4s && currentJar == null) { 
				currentJar = (GameObject) Instantiate(Jar4Prefab, 
				                                      transform.position + new Vector3(basePosition.x + i * JarSize, basePosition.y),
				                                      Quaternion.identity);
				level4Jars[i] = currentJar;
			} else if (i >= level4s && currentJar != null) {
				Destroy(currentJar);
				level4Jars[i] = null;
			}
		}
	}

	public void StopParticles() {
		HoneyBarParticles.particleSystem.enableEmission = false;
	}

	public void StartParticles() {
		HoneyBarParticles.particleSystem.enableEmission = true;
	}
	
	public void DisableBoost() {
		IsBoosted = false;
		HoneyBarFireflies.particleSystem.enableEmission = false;
	}

	public void EnableBoost() {
		IsBoosted = true;
		HoneyBarFireflies.particleSystem.enableEmission = true;
	}

	void OnDrawGizmos() {
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(new Vector3(JarBasePosition.x, JarBasePosition.y, 0) + transform.position, 0.2f);
	}
}