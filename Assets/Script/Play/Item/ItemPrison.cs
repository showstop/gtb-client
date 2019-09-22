using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ItemPrison : Item 
{
    [SerializeField]
    private GameObject _prisonGO;

    internal override void UseToTarget(CarController aTarget)
    {
        YPLog.Trace();
        YPLog.Log("target = " + aTarget);

        _owner.UpdatePlayData((short)Constants.StatKey.USE_INGAME_ITEM, 1);
        CommonUseToTarget(aTarget);
        RpcUseToTarget(aTarget.netId);

        StartCoroutine(DelayApplyEffect(aTarget));
    }

    [ClientRpc]
    public override void RpcUseToTarget(NetworkInstanceId aNetID)
    {
        YPLog.Trace();
        YPLog.Log("target = " + aNetID);

        CarController target = ClientScene.FindLocalObject(aNetID).GetComponent<CarController>();
        CommonUseToTarget(target);
    }

    protected override void CommonUseToTarget(CarController aTarget)
    {
        ToggleItemFX(true);

        _owner.UseItemFX(_useItemFX, _useItemSound);
        _animation[_useAnimName].speed = 2f;
        _animation.Play(_useAnimName);        

        StartCoroutine(ThrowPrison(aTarget));
    }

    private IEnumerator ThrowPrison(CarController aTarget)
    {
        yield return new WaitForSeconds(0.5f);

        GameObject prison = Instantiate(_prisonGO, Vector3.zero, Quaternion.identity) as GameObject;
        prison.transform.parent = aTarget.CarTransform;
        prison.transform.localPosition = Vector3.zero;
        prison.transform.localRotation = Quaternion.Euler(new Vector3(-90f, 0f, 0f));

        Animation ani = prison.GetComponent<Animation>();
        ani.Play("prison");

        FTME02_DeadTime dead = prison.GetComponent<FTME02_DeadTime>();
        dead.SetDeadTime(_bonusSpeedApplyTime);
    }

    private IEnumerator DelayApplyEffect(CarController aTarget)
    {
        // use anim : 1f
        yield return new WaitForSeconds(0.5f);

        ApplyEffect(aTarget);
    }

    protected override void ApplyEffect(CarController aCar)
    {
        aCar.ApplyItemBonusSpeed(_bonusSpeed, _bonusSpeedApplyTime, false);
        aCar.RpcApplyItemBonusSpeed(_bonusSpeed, _bonusSpeedApplyTime, false);
    }

    internal override void Enhance(float aValue)
    {
        _bonusSpeedApplyTime = aValue;
    }
}