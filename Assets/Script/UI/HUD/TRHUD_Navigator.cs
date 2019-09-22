using UnityEngine;
using System.Collections;

public class TRHUD_Navigator : MonoBehaviour 
{
    //[SerializeField]
    //private UIPanel                         _areaPanel;

    [SerializeField]
    private TRHUDUnit_Indicator[] _indicators = new TRHUDUnit_Indicator[Constants.MAX_PLAYER_NUM];

    [SerializeField]
    private UILabel _currentLapCount;

    [SerializeField]
    private UILabel _goalLapCount;

    [SerializeField]
    private UITexture _profileImage;

    [SerializeField]
    private UILabel _rank;

    [SerializeField]
    private GameObject _lapTextGO;

    [SerializeField]
    private UILabel _lapText;

    private CarController _target;
    private float _lapDistance = 0.0f;
    private int _laneCount;
    private float _prevMoveDistance = 0;
    private float _distanceRatio = 15.0f;
    private float _minY = -180.0f;
    private float _maxY = 180.0f;
    private bool _showLapText = true;

	// Use this for initialization
    /*
	void Start () 
    {
		if ( null != _areaPanel )
		{
			m_width = _areaPanel.baseClipRegion.z;
			m_height = _areaPanel.baseClipRegion.w;
		}
	}
    */

    void Start()
    {
        GameObject splineGO = GameObject.FindWithTag(Constants.START_SPLINE_TAG_NAME);
        CurvySpline spline = splineGO.GetComponent<CurvySpline>();
        _lapDistance = spline.Length;
        _laneCount = spline.LaneCount;
    }

    void Update()
    {
        if (null == _target)
        {
            return;
        }

        float diff = _prevMoveDistance - _target._moveDistance;
        Vector3 pos = _lapTextGO.transform.localPosition;
        pos.y = diff * _distanceRatio;
        TweenPosition.Begin(_lapTextGO, 0.2f, pos).method = UITweener.Method.EaseOut;

        if (pos.y < _minY && _showLapText)
        {
            StartCoroutine("ScrollLapText");
        }
    }

    private IEnumerator ScrollLapText()
    {
        _showLapText = false;
        TweenAlpha.Begin(_lapTextGO, 0.2f, 0.0f);
        yield return new WaitForSeconds(0.2f);

        _prevMoveDistance += _lapDistance;
        _lapText.text = _target.GetLapText();

        float distance = (_prevMoveDistance - _target._moveDistance) * _distanceRatio;
        while (distance > _maxY)
        {
            distance = (_prevMoveDistance - _target._moveDistance) * _distanceRatio;
            yield return null;
        }

        TweenAlpha.Begin(_lapTextGO, 0.2f, 1.0f);
        _showLapText = true;
    }
	
    public void SetupIndicator(CarController aCar)
	{
        YPLog.Trace();
        YPLog.Log("aCar = " + aCar + ", lane NO = " + aCar._laneNO + ", goalLapCount = " + aCar._goalLapCount);

        int index = aCar._laneNO - 1;
		if (0 > index || index >= Constants.MAX_PLAYER_NUM)
		{
			YPLog.Log("SetupPlayerIndicator, over player!, index = " + index);
			return;
		}
        
        _indicators[index].SetTargetCar(aCar, _laneCount);
        if (aCar.isLocalPlayer)
        {
            for (int tIndex = 0; tIndex < _indicators.Length; ++tIndex)
            {
                _indicators[tIndex]._centerCar = aCar;
            }

            _target = aCar;            
            _goalLapCount.text = string.Format("/{0}", aCar._goalLapCount);
            //_profileImage.mainTexture = TRDataManager.instance.PlayerImage;            
        }
	}

    internal void UpdateRank(int aRank)
    {
        _rank.text = aRank.ToString();
    }

    internal void UpdateLapCount(int aCount)
    {
        _currentLapCount.text = (aCount + 1).ToString();
    }
}