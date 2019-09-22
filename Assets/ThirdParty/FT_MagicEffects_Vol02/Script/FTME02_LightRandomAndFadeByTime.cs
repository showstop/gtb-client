using UnityEngine;
using System.Collections;

public class FTME02_LightRandomAndFadeByTime : MonoBehaviour {

	Light ppLight;    
	public float minIntensity = 0;
	public float maxIntensity = 2;
	public float flickerSpeed = 0.03f;

	public float fadeStartTiming = 1;
	public float fadeTime = 1;


	private float endTime = 0;
	private float startTime = 0;
	private float currentTime = 0;
	private bool switchOn = false;
	private float finalIntensity = 0;

	IEnumerator Start () {
		ppLight = GetComponent<Light>();


		while (currentTime < fadeStartTiming) {
			ppLight.intensity = (Random.Range (minIntensity, maxIntensity));
			yield return new WaitForSeconds(flickerSpeed);
		}
		startTime = Time.time;
		endTime= startTime + fadeTime;	
		finalIntensity = ppLight.intensity;
		switchOn = true;
	}
	
	void Update () {
		currentTime += Time.deltaTime;
		if (switchOn == true) {
			ppLight.intensity = Mathf.InverseLerp (endTime, startTime, Time.time)* finalIntensity;
		}
			
	
	}
}
