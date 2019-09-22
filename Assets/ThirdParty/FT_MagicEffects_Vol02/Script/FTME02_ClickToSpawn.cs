using UnityEngine;
using System.Collections;

public class FTME02_ClickToSpawn : MonoBehaviour {

	public GUIText prefabName;
	public GameObject[] particlePrefab;
	public int particleNum = 0;

	GameObject effectPrefab;
	bool checkBeam;

	// Use this for initialization
	void Start () {
	
	}

	/*void Update () {
		if (Input.GetMouseButtonDown (0)) { 
			effectPrefab = Instantiate (particlePrefab [particleNum],
				new Vector3 (0, 1, 0), Quaternion.Euler (0, 0, 0))as GameObject;
			effectPrefab.GetComponent<Rigidbody>().AddForce (Vector3.forward * 600,
				ForceMode.Acceleration);
		}
	}*/
	
	// Update is called once per frame
	void Update () {
		if ( particleNum < 6){ 
			if(checkBeam == false){
				effectPrefab = Instantiate(particlePrefab[particleNum],
				new Vector3(0,1,0), Quaternion.Euler(0,0,0))as GameObject;
				checkBeam = true;
			}
		}
		if ( Input.GetMouseButtonDown(0)&& particleNum >= 6){ 
			effectPrefab = Instantiate(particlePrefab[particleNum],
			new Vector3(0,1,0), Quaternion.Euler(0,0,0))as GameObject;
			effectPrefab.GetComponent<Rigidbody>().AddForce (Vector3.forward * 600,
				ForceMode.Acceleration);
		}

		if (Input.GetKeyDown(KeyCode.LeftArrow)){
			Destroy(effectPrefab);
			particleNum -= 1;
			if( particleNum < 0) {
				particleNum = particlePrefab.Length-1;
			}	
			checkBeam = false;
		}
		if (Input.GetKeyDown(KeyCode.RightArrow)){
			Destroy(effectPrefab);
			particleNum += 1;
			if(particleNum >(particlePrefab.Length - 1)) {
				particleNum = 0;
			}
			checkBeam = false;
		}
		
		prefabName.text= particlePrefab[particleNum].name;
	
	}
}
