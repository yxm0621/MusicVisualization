using UnityEngine;
using System.Collections;

public class pattern : MonoBehaviour {
	public GameObject obj;
	public GameObject[] specBar;
	public float[] length;
	public int fitness = 0;
	public Vector3 pos;
	public Vector3 rot;
	public int[] random;
	public float radius = 5;

	// Use this for initialization
	void Start () {
		for(int i = 0; i < 12; i++) {
			specBar [i] = obj;
		}
		initialPattern ();
	}
	
	// Update is called once per frame
	void Update () {
		for (int i=0; i<12; i++){
			if ((specBar[i] != null) && (length[i] != null)) {
//				Debug.Log(i);
				switch (random[i]) {
				case 0:
					specBar[i].transform.localScale = new Vector3(length[i] * 10,
					                                              length[i] * 10,
					                                              length[i] * 10);
					specBar[i].particleSystem.startSize = length[i] * 200;
					if (specBar[i].transform.childCount > 0) {
						for (int childIndex = 0; childIndex < specBar[i].transform.childCount; childIndex++){
							if (specBar[i].transform.GetChild(childIndex).particleSystem != null) {
								specBar[i].transform.GetChild(childIndex).particleSystem.startSize = length[i] * 200;
							}
						}
					}
					break;
				case 1:
//					specBar[i].transform.localScale = new Vector3(length[i] * 20,
//					                                              length[i] * 20,
//					                                              length[i] * 20);
					specBar[i].particleSystem.startSize = length[i] * 100;
					if (specBar[i].transform.childCount > 0) {
						for (int childIndex = 0; childIndex < specBar[i].transform.childCount; childIndex++){
							if (specBar[i].transform.GetChild(childIndex).particleSystem != null) {
								specBar[i].transform.GetChild(childIndex).particleSystem.startSize = length[i] * 100;
							}
						}
					}
//				specBar[i].transform.localScale = new Vector3(.5f,length[i] * 10,1);
//				specBar [i].transform.eulerAngles += new Vector3(0,0,length[i] * Time.deltaTime);
					specBar [i].transform.position = new Vector3 (Mathf.Sin(Time.timeSinceLevelLoad * 6/Mathf.PI) * i,
					                                              Mathf.Cos(Time.timeSinceLevelLoad * 6/Mathf.PI) * i,
					                                              (specBar [i].transform.position.z > 20) ? (-Time.timeSinceLevelLoad * random[i]) : (Time.timeSinceLevelLoad * random[i]));
					
//					iTween.MoveTo(specBar[i], iTween.Hash("x", length[i] * 10, "y", length[i] * 10, "easeType", "easeInOutExpo", "time", .1f));
					break;
				case 2:
					specBar[i].transform.localScale = new Vector3(length[i] * 30,
					                                              length[i] * 30,
					                                              length[i] * 30);
					specBar[i].particleSystem.startSize = length[i] * 100;
					if (specBar[i].transform.childCount > 0) {
						for (int childIndex = 0; childIndex < specBar[i].transform.childCount; childIndex++){
							if (specBar[i].transform.GetChild(childIndex).particleSystem != null) {
								specBar[i].transform.GetChild(childIndex).particleSystem.startSize = length[i] * 100;
							}
						}
					}
					specBar[i].particleSystem.emissionRate = length[i];
//					iTween.MoveTo(specBar[i], iTween.Hash("x", length[i] * 10, "y", length[i] * 10, "easeType", "easeInOutExpo", "time", .1f));
				break;
				case 3:
					specBar[i].transform.localScale = new Vector3(length[i] * 20,
					                                              length[i] * 20,
					                                              length[i] * 20);
					specBar[i].particleSystem.startSize = length[i] * 100;
					if (specBar[i].transform.childCount > 0) {
						for (int childIndex = 0; childIndex < specBar[i].transform.childCount; childIndex++){
							if (specBar[i].transform.GetChild(childIndex).particleSystem != null) {
								specBar[i].transform.GetChild(childIndex).particleSystem.startSize = length[i] * 100;
							}
						}
					}
				specBar[i].particleSystem.randomSeed = (uint)length[i];
				break;
				case 4:
					specBar[i].transform.localScale = new Vector3(length[i] * 20,
					                                              length[i] * 20,
					                                              length[i] * 20);
					specBar[i].particleSystem.startSize = length[i] * 100;
					if (specBar[i].transform.childCount > 0) {
						for (int childIndex = 0; childIndex < specBar[i].transform.childCount; childIndex++){
							if (specBar[i].transform.GetChild(childIndex).particleSystem != null) {
								specBar[i].transform.GetChild(childIndex).particleSystem.startSize = length[i] * 100;
							}
						}
					}
//					iTween.MoveTo(specBar[i], iTween.Hash("x", length[i] * 10, "y", length[i] * 10, "easeType", "easeInOutExpo"));
//				specBar[i].renderer.material.color =  new Color(length[i]%255, length[i]%255,length[i]%255,length[i]%255);
				break;
			default:
					specBar[i].transform.localScale = new Vector3(length[i] * 20,
					                                              length[i] * 20,
					                                              length[i] * 20);
//				specBar[i].transform.localScale = new Vector3(.5f,length[i] * 10,1);
				break;
			}
			}
		}
	}

	public void initialPattern() {
		for(int i = 0; i < 12; i++) {
			pos = new Vector3 (Mathf.Sin(i * 6/Mathf.PI) * radius, Mathf.Cos(i * 6/Mathf.PI) * radius, 0);
			rot = new Vector3(0, 0 , (-30f)*i);
//			pos = new Vector3 (Random.Range(-10f,10f),Random.Range(-10f,10f),Random.Range(-5f,10f));
//			rot = new Vector3 (Random.Range(0f,360f),Random.Range(0f,360f),Random.Range(0f,360f));
			specBar [i].transform.position = pos;
			specBar [i].transform.eulerAngles = rot;
			specBar [i].tag = "pattern";
			random [i] = Random.Range(0, 5);
			GameObject spec = (GameObject)Instantiate (specBar [i], specBar[i].transform.position, specBar[i].transform.rotation);
			spec.name = specBar [i].name;
			specBar [i] = spec;
			spec = null;
			specBar [i].transform.parent = gameObject.transform;
		}
	}
}
