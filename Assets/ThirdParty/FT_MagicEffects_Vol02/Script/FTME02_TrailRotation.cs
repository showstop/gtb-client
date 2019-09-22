using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FTME02_TrailRotation : MonoBehaviour {

	public TrailRenderer[] trail;
	public float trailRotationSpeed;
	public float fadeTime = 1;
	public bool fadeOutSwitch = false;
	private float endTime = 0;
	private float startTime = 0;
	float finalTime; 



	public void FadeOut(){
		startTime = Time.time;
		endTime= startTime + fadeTime;	
		finalTime = trail[0].time;
	}

	void Update(){
		transform.rotation = Quaternion.Euler (0, 0, Time.time * trailRotationSpeed);
		if (fadeOutSwitch == true) {		
			for (int i = 0; i < trail.Length; i++) {
				trail[i].time = Mathf.InverseLerp (endTime, startTime, Time.time)* finalTime;
			}
		}
	}
}
