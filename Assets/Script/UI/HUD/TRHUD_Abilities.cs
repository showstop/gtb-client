using UnityEngine;
using System.Collections.Generic;

public class TRHUD_Abilities : MonoBehaviour {

	[SerializeField]
	private TRHUDUnit_Ability[] _abilities = new TRHUDUnit_Ability[3];

    internal void SetAbilities(List<int> aAbilityIDs)
    {
        YPLog.Trace();
        YPLog.Log("ability count = " + aAbilityIDs.Count);

        for (int index = 0; index < aAbilityIDs.Count; ++index)
        {
            YPLog.Log("ability ID = " + aAbilityIDs[index]);

            _abilities[index].Ability(aAbilityIDs[index]);
            _abilities[index].gameObject.SetActive(true);
        }
    }

    internal void UpdateAbilityGauge(int aAbilityID, float aGauge)
    {
        for (int index = 0; index < _abilities.Length; ++index)
        {
            if (aAbilityID == _abilities[index].AbilityID)
                _abilities[index].Value = aGauge;
        }
    }

    internal void AbilityActivated(int aAbilityID)
    {
        for (int index = 0; index < _abilities.Length; ++index)
        {
            if (aAbilityID == _abilities[index].AbilityID)
            {
                _abilities[index].Activated();
            }
        }
    }
}
