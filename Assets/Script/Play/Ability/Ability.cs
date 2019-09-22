using UnityEngine;
using System.Collections;

public class Ability : MonoBehaviour 
{
    public int          _id;

    [SerializeField]
    protected float[]   _levelValue;

    public GameObject   _applyFX;
    public AudioClip    _applySound;

    internal virtual void ApplyAtStartGame(int aLevel, CarController aCar)  { }
    internal virtual void ApplyAtEndGame(int aLevel, CarController aCar)    { }

    internal virtual void ApplyAtLapCount(int aLevel, CarController aCar) { }
    internal virtual void ApplyAtZeroHP(int aLevel, CarController aCar) { }

    internal virtual int ChangeItem(int aLevel, int aItemID, CarController aCar) { return -1; }
    internal virtual void ChangeDamage(int aLevel, ref int oDamage, bool aCollide, bool aAttack, CarController aCar) { }
    internal virtual float ChangeSlowTime(int aLevel, float aTime, bool aAttack) { return aTime; }

    internal virtual void GetItemBox(int aLevel, CarController aCar) { }
    internal void Stop()
    {
        StopAllCoroutines();
    }

    internal virtual void EnhanceItem(int aLevel, ref Item oItem) { }
}