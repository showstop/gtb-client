using UnityEngine;
using System.Collections;

public class ItemAttachFX : MonoBehaviour 
{
    [SerializeField]
    private Animation _animation;

    [SerializeField]
    private string _showAnimName;

    [SerializeField]
    private GameObject _destoryFX;

    internal void Play(float aTime, CarController aCar)
    {
        transform.parent = aCar.CarTransform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        _animation.Play(_showAnimName);

        Invoke("StopPlay", aTime);
    }

    private void StopPlay()
    {
        if (null != _destoryFX)
        {
            GameObject go = Instantiate(_destoryFX, Vector3.zero, Quaternion.identity) as GameObject;
            go.transform.parent = transform.parent;

            TRCarEffectParticleLocalTransform localTransform = go.GetComponent<TRCarEffectParticleLocalTransform>();
            if (null != localTransform)
            {
                go.transform.localPosition = localTransform.m_localPosition;
                go.transform.localRotation = Quaternion.Euler(localTransform.m_localRotation);
            }
        }

        Destroy(gameObject);
    }
}
