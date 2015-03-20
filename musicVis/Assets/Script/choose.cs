using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class choose : MonoBehaviour, AudioProcessor.AudioCallbacks
{
//	public GameObject[] temp;
	public GameObject[] patternGroups;
	public pattern[] patterns;
	public int index = 0;
	public int fitness;
	public string patternTag = "pattern";
	public List<GameObject> choosePatterns = new List<GameObject> ();
	public string next;
	public bool isBeat = false;

	// Use this for initialization
	void Start () {
		//Select the instance of AudioProcessor and pass a reference to this object
		AudioProcessor processor = FindObjectOfType<AudioProcessor> ();
		processor.addAudioCallback (this);
		for (int i = 0; i < patternGroups.Length; i++) {
//			patternGroups [i] = (GameObject)Instantiate (patternGroups [i], new Vector3 (0, 0, 0), Quaternion.identity);
			patterns [i] = patternGroups [i].GetComponent<pattern> ();
//			for (int j = 0; j < 12; j++) {
//				Instantiate (patterns [i].specBar[j], patterns [i].specBar[j].transform.position, patterns [i].specBar[j].transform.rotation);
//			}
			patternGroups [i].SetActive (false);
		}
		patternGroups [index].SetActive (true);
		next = "";
	}

	// Update is called once per frame
	void Update () {
//		if (isBeat) {
//			GameObject.Find("Beat").transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
//		} else {
//			GameObject.Find("Beat").transform.localScale = new Vector3(1, 1, 1);
//		}
		if (Input.GetKeyDown (KeyCode.Space)) {
			next = "";
			patternGroups [index].SetActive (false);
			index++;
			if (index == patternGroups.Length) {
				selection();
				evolvePattern ();
				index = 0;
				next = "New Generation!";
			}
			patternGroups [index].SetActive (true);
		}
		if (Input.GetMouseButtonDown (0)) {
			patterns [index].fitness++;
		}
		isBeat = false;
	}

	void OnGUI () {
//		GUI.Label (new Rect (10, 10, 500, 20), "Please click if you like. " + patterns [index].fitness);
		GUI.Label (new Rect (10, 10, 500, 20), "Please click if you like. ");
		GUI.Label (new Rect (10, 20, 500, 20), next);
	}

	public void onOnbeatDetected () {
		Debug.Log ("Beat!!!");
		isBeat = true;
	}


	//This event will be called every frame while music is playing
	public void onSpectrum (float[] spectrum) {
		//The spectrum is logarithmically averaged to 12 bands

		for (int i = 0; i < spectrum.Length; ++i) {
			Vector3 start = new Vector3 (i, 0, 0);
			Vector3 end = new Vector3 (i, spectrum [i], 0);
			Vector3 change = end - start;
			if ((patternGroups[index].transform.childCount != 0) && (patterns [index] != null)) {
				patterns [index].length [i] = change.y;
			}
//			Debug.DrawLine (start, end);
		}
	}

	//select high fitness patterns for evolving
	public  void selection () {
		for (int i = 0; i < patternGroups.Length; i++) {
			if (patterns [i].fitness > 0) {
				for (int j = 0; j < patterns [i].specBar.Length; j++) {
					choosePatterns.Add (patterns [i].specBar[j]);
				}
			}
		}
		if (choosePatterns.Count == 0) {
			for (int i = 0; i < patternGroups.Length; i++) {
				for (int j = 0; j < patterns [i].specBar.Length; j++) {
					choosePatterns.Add (patterns [i].specBar[j]);
				}
			}
		}

		//clear all
		for (int i =0; i< patternGroups.Length;i++) {
			foreach (Transform child in patternGroups[i].transform) {
				GameObject.Destroy(child.gameObject);
			}
			patterns [i].fitness = 0;
		}
	}

	//randomly choose two patterns from selected patterns as parents and crossover,mutation
	public void evolvePattern () {
		int patternOne;
		int patternTwo;
		for (int i = 0; i < patternGroups.Length; i++) {
			patternOne = getRandomInt (0, choosePatterns.Count);
			patternTwo = getRandomInt (0, choosePatterns.Count);
			for (int j = 0; j < 12; j++) {
				patterns [i].specBar[j] = crossover (choosePatterns [patternOne], choosePatterns [patternTwo], getRandomInt (0, 2));
				patterns [i].specBar[j] = mutation(patterns [i].specBar[j]);
			}
			patterns[i].initialPattern();
			patternGroups [i].SetActive (false);
		}
		choosePatterns.Clear ();
	}

	public GameObject crossover (GameObject one, GameObject two, int baseObj) {
		GameObject[] parents = {one, two};

		//crossover based on baseObj
		if (getRandomBool()) {
			//switch color
			parents[baseObj].particleSystem.startColor = parents[1-baseObj].particleSystem.startColor;
			if(parents[baseObj].transform.childCount > 0) {
				foreach (Transform child in parents[baseObj].transform) {
					if(child.particleSystem != null) {
						child.particleSystem.startColor = parents[1-baseObj].particleSystem.startColor;
					}
				}
			}
				switch (getRandomInt(0, 3)){
				case 0:
					//switch color and speed
					parents[baseObj].particleSystem.startSpeed = parents[1-baseObj].particleSystem.startSpeed;
					break;
				case 1:
					//switch color and size
					parents[baseObj].particleSystem.startSize = parents[1-baseObj].particleSystem.startSize;
					break;
				case 2:
					//switch color and material
					parents[baseObj].particleSystem.renderer.material = parents[1-baseObj].particleSystem.renderer.material;
					break;
				default:
					//switch color and size
					parents[baseObj].particleSystem.startSize = parents[1-baseObj].particleSystem.startSize;
					break;
				}
				return parents[baseObj];
			} else if(getRandomBool()){
				//switch speed
				parents[baseObj].particleSystem.startSpeed = parents[1-baseObj].particleSystem.startSpeed;
				switch (getRandomInt(0, 2)){
				case 0:
					//switch speed and size
					parents[baseObj].particleSystem.startSize = parents[1-baseObj].particleSystem.startSize;
					break;
				case 1:
					//switch speed and material
					parents[baseObj].particleSystem.renderer.material = parents[1-baseObj].particleSystem.renderer.material;
					break;
				default:
					//switch speed and size
					parents[baseObj].particleSystem.startSize = parents[1-baseObj].particleSystem.startSize;
					break;
				}
				return parents[baseObj];
			} else {
				//switch size and material
				parents[baseObj].particleSystem.startSize = parents[1-baseObj].particleSystem.startSize;
				parents[baseObj].particleSystem.renderer.material = parents[1-baseObj].particleSystem.renderer.material;
				return parents[baseObj];
			}
	}

	public GameObject mutation(GameObject obj) {
		switch (getRandomInt(0, 3)){
		case 0:
			if (getRandomBool()) {
				Color newColor = Color.yellow;
				newColor.r = getRandomNum(0,1);
				newColor.g = getRandomNum(0,1);
				newColor.b = getRandomNum(0,1);
				newColor.a = getRandomNum(0,1);
				obj.particleSystem.startColor = newColor;
				foreach (Transform child in obj.transform) {
					if(child.particleSystem != null) {
						Color color = Color.yellow;
						color.r = getRandomNum(0,1);
						color.g = getRandomNum(0,1);
						color.b = getRandomNum(0,1);
						color.a = getRandomNum(0,1);
						child.particleSystem.startColor = color;
					}
				}
			}
			break;
		case 1:
			if (getRandomBool()) {
				obj.particleSystem.startSpeed = getRandomNum(0,50f);
			}
			break;
		case 2:
			if (getRandomBool()) {
				obj.particleSystem.startSize = obj.particleSystem.startSize;
			}
			break;
		default:
			break;
		}
		return obj;
	}

	public bool getRandomBool(){
		int i = UnityEngine.Random.Range (0,2);
		if (i == 0) {
			return false;
		} else {
			return true;
		}
	}
	public float getRandomNum(float min, float max){
		float i = UnityEngine.Random.Range (min, max);
		return i;
	}
	public int getRandomInt(int min, int max){
		int i = UnityEngine.Random.Range (min, max);
		return i;
	}
}