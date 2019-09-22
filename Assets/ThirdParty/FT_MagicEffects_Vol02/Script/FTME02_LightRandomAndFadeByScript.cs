using UnityEngine;
using System.Collections;

public class FTME02_LightRandomAndFadeByScript : MonoBehaviour {

	Light ppLight;    
	public float minIntensity = 0;
	public float maxIntensity = 2;
	public float flickerSpeed = 0.03f;
	public float fadeTime = 1;
	public bool fadeOutSwitch = false;

	private float endTime = 0;
	private float startTime = 0;
	private float currentTime = 0;
	private float finalIntensity = 0;

	IEnumerator Start () {
		ppLight = GetComponent<Light>();


		while (fadeOutSwitch == false) {
			ppLight.intensity = (Random.Range (minIntensity, maxIntensity));
			yield return new WaitForSeconds(flickerSpeed);
		}
		startTime = Time.time;
		endTime= startTime + fadeTime;	
		finalIntensity = ppLight.intensity;
	}
	
	void Update () {
		currentTime += Time.deltaTime;
		if (fadeOutSwitch == true) {
			ppLight.intensity = Mathf.InverseLerp (endTime, startTime, Time.time)* finalIntensity;
		}	
	}
}
