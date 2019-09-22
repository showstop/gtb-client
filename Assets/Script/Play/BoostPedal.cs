using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class BoostPedal : NetworkBehaviour
{
    [SerializeField]
    private SplineWalkerCon _swc;

    [SerializeField]
    private GameObject _pedalGO;

    [SerializeField]
    private float _bonusSpeed;

    [SerializeField]
    private float _bonusSpeedApplyTime;

    [SyncVar]
    private string _splineTag;

    [SyncVar]
    private int _lane;

    [SyncVar]
    private float _initialF;

    void Start()
    {
        if (!NetworkServer.active && NetworkClient.active)
        {
            CurvySpline cs = GameObject.FindWithTag(_splineTag).GetComponent<CurvySpline>();
            InstallPedal(cs);

        }
    }

	internal void SetInfo(CurvySpline aSpline, int aLane, float aInitialF)
    {
        _splineTag = aSpline.tag;
        _lane = aLane;
        _initialF = aInitialF;

        InstallPedal(aSpline);
    }

    private void InstallPedal(CurvySpline aSpline)
    {
        _swc.Spline = aSpline;
        _swc.InitialF = _initialF;

        _pedalGO.transform.localPosition = new Vector3(aSpline.GetLaneOffsetX(_lane), aSpline.LaneOffsetY, _pedalGO.transform.localPosition.z);
    }

    [Command]
    public void CmdBoost(NetworkInstanceId aNetID)
    {
        GameObject go = NetworkServer.FindLocalObject(aNetID);
        CarController target = go.GetComponent<CarController>();
        Boost(target);
    }

    internal void Boost(CarController aTarget)
    {
        aTarget.ApplyItemBonusSpeed(_bonusSpeed, _bonusSpeedApplyTime, false);
        aTarget.RpcApplyItemBonusSpeed(_bonusSpeed, _bonusSpeedApplyTime, false);
    }
}