using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ItemBox : NetworkBehaviour 
{
    [SerializeField]
    private GameObject _boxGO;

    [SyncVar(hook = "ToggleActive")]
    private bool _active = true;
    public bool Active { get { return _active; } }

    [SyncVar]
    private int _laneNO = 0;

    [SyncVar]
    private float _distance = 0f;

    [SyncVar]
    private string _splineTag = "";

    private SplineWalkerDistance _swd;
    private ItemBoxSpawner _spawner;

    void Start()
    {
        if (NetworkClient.active)
        {   
            CurvySpline cs = GameObject.FindWithTag(_splineTag).GetComponent<CurvySpline>();
            SetLocation(cs);
        }
    }

    internal void SetInfo(ItemBoxSpawner aSpawner, CurvySpline aSpline, int aLaneNO, float aDistance)
    {
        _spawner = aSpawner;
        _laneNO = aLaneNO;
        _distance = aDistance;
        _splineTag = aSpline.tag;

        SetLocation(aSpline);
    }

    private void SetLocation(CurvySpline aSpline)
    {
        _swd = gameObject.AddComponent<SplineWalkerDistance>();
        _swd.SetOrientation = true;
        _swd.FastInterpolation = true;
        _swd.Forward = true;
        _swd.Spline = aSpline;
        _swd.InitialDistance = _distance;        

        _boxGO.transform.localPosition = new Vector3(aSpline.GetLaneOffsetX(_laneNO), aSpline.LaneOffsetY, _boxGO.transform.localPosition.z);
    }

    internal void ToggleActive(bool aActive)
    {
        _active = aActive;
        _boxGO.SetActive(_active);
    }
}