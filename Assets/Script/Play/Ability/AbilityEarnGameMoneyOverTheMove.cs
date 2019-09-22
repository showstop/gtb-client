using UnityEngine;
using System.Collections;

public class AbilityEarnGameMoneyOverTheMove : Ability 
{
    [SerializeField]
    private int _gameMoney;

    internal override void ApplyAtStartGame(int aLevel, CarController aCar)
    {
        StartCoroutine(EarnGameMoney(_levelValue[aLevel], aCar));
    }

    private IEnumerator EarnGameMoney(float aMoveDistance, CarController aCar)
    {
        while (!aCar._matchStart)
        {
            yield return null;
        }

        float lastMoveDistance = aCar._moveDistance;
        while (aCar._matchStart)
        {
            if (aMoveDistance <= aCar._moveDistance - lastMoveDistance)
            {
                aCar.UpdateHP(_gameMoney, null, false);
            }

            lastMoveDistance = aCar._moveDistance;
            yield return new WaitForSeconds(0.05f);
        }
    }   
}