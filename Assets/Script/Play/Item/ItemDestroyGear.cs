using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ItemDestroyGear : Item
{
    [SerializeField]
    private float _cameraShakeRange;

    [SerializeField]
    private float _cameraShakeDuration;

    protected override void CommonUse()
    {
        DetachFromCar();

        _swc.Spline = _owner._swc.Spline;
        _swc.TF = _owner._swc.TF;
        _swc.Speed = _speed;
        _swc.Clamping = _owner._swc.Clamping;

        int dir = 1;
        float tf = _swc.TF;
        transform.position = _swc.Spline.MoveBy(ref tf, ref dir, 0f, _swc.Clamping);
        _itemGO.transform.localPosition = new Vector3(_swc.Spline.GetLaneOffsetX(_owner._laneNO) / transform.localScale.x, _itemGO.transform.localPosition.y + _spawnOffsetY, _itemGO.transform.localPosition.z);

        _currentOffsetY = _itemGO.transform.localPosition.y;
        _jumpState = Constants.JumpState.GoingDown;

        ToggleItemFX(true);
        _owner.UseItemFX(_useItemFX, _useItemSound);

        StartCoroutine(Rotate());
        StartCoroutine(CameraShake());
    }

    private IEnumerator Rotate()
    {
        while (_animation.isPlaying)
        {
            yield return null;
        }

        _animation.Play(_useAnimName);
    }

    private IEnumerator CameraShake()
    {
        CarController localPlayer = _owner.GM.GetLocalPlayer();
        if (null == localPlayer)
        {
            yield break;
        }

        while (true)
        {
            if (_cameraShakeRange > Vector3.Distance(localPlayer.CarTransform.position, transform.position))
            {
                localPlayer.ShakeCamera(_cameraShakeDuration);
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    internal override void Collide(CarController aCar)
    {
        // apply effect        
        ApplyEffect(aCar);
    }
}