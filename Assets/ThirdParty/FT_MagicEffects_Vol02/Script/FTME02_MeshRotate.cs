using UnityEngine;
using System.Collections;

public class FTME02_MeshRotate : MonoBehaviour {
	public float rotateSpeed = 1f;
	public bool stopSwitch = false;
	float rot = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(stopSwitch == false){
			rot += rotateSpeed; 
			transform.Rotate (new Vector3(0, rot, 0));
		}
	}
}
