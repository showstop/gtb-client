using UnityEngine;
using System.Collections;

public class AbilityChargeShield : Ability 
{
    internal override void ApplyAtStartGame(int aLevel, CarController aCar)
    {
        StartCoroutine(CheckMoveDistance(_levelValue[aLevel], aCar));
    }

    private IEnumerator CheckMoveDistance(float aCheckDistance, CarController aCar)
    {
        while (!aCar._matchStart)
        {
            yield return null;
        }

        float lastMoveDistance = aCar._moveDistance;
        while (aCar._matchStart)
        {
            yield return new WaitForSeconds(0.1f);

            if (!aCar._chargeShield)
            {
                float gauge = (aCar._moveDistance - lastMoveDistance) / aCheckDistance;
                if (1f <= gauge)
                {
                    aCar._chargeShield = true;
                    gauge = 1f;

                    lastMoveDistance = aCar._moveDistance;
                }

                aCar.RpcAbilityActivated(_id, gauge, false);
            }
            else
            {
                lastMoveDistance = aCar._moveDistance;
            }
        }

        yield return new WaitForSeconds(0.1f);
    }
}