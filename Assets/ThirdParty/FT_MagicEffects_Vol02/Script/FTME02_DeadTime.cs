using UnityEngine;
using System.Collections;

public class FTME02_DeadTime : MonoBehaviour {

	public float deadtime;

	void Awake (){
		if (deadtime != 0) {
			Destroy (gameObject, deadtime);
		}
	}

    internal void SetDeadTime(float aTime)
    {
        deadtime = aTime;
        StartCoroutine(Dead());
    }

    private IEnumerator Dead()
    {
        yield return new WaitForSeconds(deadtime);
        Destroy(gameObject);
    }
}
