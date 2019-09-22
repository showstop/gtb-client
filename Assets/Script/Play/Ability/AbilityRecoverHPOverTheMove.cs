using UnityEngine;
using System.Collections;

public class AbilityRecoverHPOverTheMove : Ability 
{
    [SerializeField]
    private int _recoverHP;

    internal override void ApplyAtStartGame(int aLevel, CarController aCar)
    {
        StartCoroutine(RecoverHP(_levelValue[aLevel], aCar));
    }

    private IEnumerator RecoverHP(float aMoveDistance, CarController aCar)
    {
        while (!aCar._matchStart)
        {
            yield return null;
        }

        float lastMoveDistance = aCar._moveDistance;        
        while (aCar._matchStart)
        {
            if (!aCar._runawayActivated && aMoveDistance <= aCar._moveDistance - lastMoveDistance)
            {   
                aCar.UpdateHP(_recoverHP, null, false);
            }

            lastMoveDistance = aCar._moveDistance;
            yield return new WaitForSeconds(0.05f);
        }
    }
}