using UnityEngine;
using System.Collections;

public class FTME02_MeshFadeOut : MonoBehaviour {

	public Transform meshObject;  
	public bool fadeOutSwitch = false;

	Animator meshAnimator;

	void Start(){
		meshAnimator = meshObject.GetComponent<Animator>();
	}
	
	void Update(){
		if (fadeOutSwitch == true) 
			meshAnimator.SetTrigger ("FadeOut");		
	}
}
