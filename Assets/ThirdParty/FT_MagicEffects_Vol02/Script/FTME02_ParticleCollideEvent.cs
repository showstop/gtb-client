using UnityEngine;
using System.Collections;

public class FTME02_ParticleCollideEvent : MonoBehaviour {	
	public ParticleSystem[] particleSystem;
	public GameObject[] meshParticle;
	public GameObject[] endMeshParticle;
	public Transform trailParent;
	public Light light;
	FTME02_LightRandomAndFadeByScript lightFadeOff;
	FTME02_TrailRotation trailFade;

	void Start() {
		if(endMeshParticle != null){
			for (int i = 0; i < endMeshParticle.Length; i++) {
				endMeshParticle [i].SetActive (false);
			}
		}	
		
	}

	void OnCollisionEnter(Collision collision) {
		if(particleSystem != null){
			for (int i = 0; i < particleSystem.Length; i++) {
				particleSystem [i].loop = false;
			}
		}

		if(meshParticle != null){
			for (int i = 0; i < meshParticle.Length; i++) {
				Destroy (meshParticle [i]);
			}
		}

		if(endMeshParticle != null){
			for (int i = 0; i < endMeshParticle.Length; i++) {
				endMeshParticle [i].SetActive (true);
			}
		}	

		if (light != null) {
			lightFadeOff = light.GetComponent<FTME02_LightRandomAndFadeByScript> ();
			lightFadeOff.fadeOutSwitch = true;
		}

		if(trailParent != null){
			trailFade = trailParent.GetComponent<FTME02_TrailRotation>();
			trailFade.FadeOut();
			trailFade.fadeOutSwitch = true;
		}
		
	}
}
