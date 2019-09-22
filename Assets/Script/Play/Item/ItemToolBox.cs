using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ItemToolBox : ItemPeriod 
{
    [SerializeField]
    private Animation _toolAnimation;

    [SerializeField]
    private Animation _boxAnimation;

    [SerializeField]
    private int _recoverHP;

    [SerializeField]
    private int _recoverCount;

    internal override void ToggleShow()
    {
        YPLog.Trace();

        _itemGO.SetActive(_show);
        if (_show)
        {
            YPLog.Log("show anim!!");
            _boxAnimation.Play(_showAnimName);
            if (_owner.isLocalPlayer)
            {
                _owner._updateItemUseButton = true;
            }
        }
    }

    protected override void CommonUse()
    {
        _owner.ApplyItemFX(_applyItemFX, _applyItemSound, _applyTime);
        StartCoroutine(Repair());
    }  

    private IEnumerator Repair()
    {
        _toolAnimation.Play(_showAnimName);
        _boxAnimation[_useAnimName].wrapMode = WrapMode.Loop;
        _boxAnimation.Play(_useAnimName);
        while (_toolAnimation.isPlaying)
        {
            yield return null;
        }

        _toolAnimation.Play(_useAnimName);        
        float length = _toolAnimation[_useAnimName].length;
        float interval = length / _recoverCount;
        for (int index = 0; index < _recoverCount; ++index)
        {
            YPLog.Log("index = " + index + ", count = " + _recoverCount + ", recoverHP = " + _recoverHP + ", current = " + _owner._hp);
            YPLog.Log("server = " + NetworkServer.active + ", client = " + NetworkClient.active);
            if (NetworkServer.active)
            {   
                _owner.UpdateHP(_recoverHP, null, false);
            }
            
            yield return new WaitForSeconds(interval);
        }
    }

    [ClientRpc]
    public override void RpcEndApply()
    {
        _boxAnimation.Play(_hideAnimName);
        _toolAnimation.Play(_hideAnimName);
    }

    protected override void ApplyEffect(CarController aCar)
    {   
    }
}