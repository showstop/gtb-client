using UnityEngine;
using System.Collections;

public class TRHUDUnit_Indicator : MonoBehaviour 
{
    //private TRCarController		m_centerPlayerController = null;
    //public TRCarController 		centerPlayerController
    //{
    //    get { return m_centerPlayerController; }
    //    set {
    //        m_centerPlayerController = value;
    //    }
    //}

    public CarController _target;
    public CarController _centerCar;

    //private TRCarController 	m_playerController = null;
    //public TRCarController 		playerController
    //{
    //    get { return m_playerController; }
    //    set {
    //        m_playerController = value;
    //    }
    //}

    //public string            PlayerUUID              
    //{ 
    //    get 
    //    {
    //        if (null == m_playerController)
    //            return "";
			
    //        return m_playerController.PlayerUUID; 
    //    } 
    //}

	[SerializeField]
	private GameObject      _mine;
	
	[SerializeField]
	private GameObject      _other;

	[SerializeField]
	private UITexture       _otherAvatar;

	private float 			_distanceRatio = 15.0f;
	private float			_minY = -180.0f;
	private float			_maxY = 180.0f;
    private int             _laneCount;

	void Update ()
	{
        if (null == _target)
        {
            return;
        }

        if(_target.isLocalPlayer)
        {
            Vector3 pos = _mine.transform.localPosition;
            pos.x = ((_target._laneNO - 1) * (100.0f / _laneCount)) + 15.0f;
            TweenPosition.Begin(_mine, 0.2f, pos).method = UITweener.Method.EaseOut;
        }
        else
        {
            float distance = _target._moveDistance - _centerCar._moveDistance;
            Vector3 pos = _other.transform.localPosition;
            pos.x = ((_target._laneNO - 1) * (100.0f / _laneCount)) + 15.0f;
            pos.y = Mathf.Clamp(distance * _distanceRatio, _minY, _maxY);
            TweenPosition.Begin(_other, 0.2f, pos).method = UITweener.Method.EaseOut;
        }

        //if (null == m_playerController || !m_playerController.MatchStart)
        //    return;

        //if ( true == m_playerController.Mine )
        //{
        //    Vector3 pos = m_mine.transform.localPosition;
        //    pos.x = ( ( m_playerController.LaneNO - 1 ) *  ( 100.0f / m_playerController.SWC.Spline.LaneCount ) ) + 15.0f;
        //    TweenPosition.Begin ( m_mine, 0.2f, pos ).method = UITweener.Method.EaseOut;
        //}else
        //{
        //    if ( null == _centerCar )
        //        return;

        //    float distance = m_playerController.MoveDistance - _centerCar.MoveDistance;

        //    Vector3 pos = m_other.transform.localPosition;
        //    pos.x = ((m_playerController.LaneNO - 1) * (100.0f / m_playerController.SWC.Spline.LaneCount)) + 15.0f;
        //    pos.y = Mathf.Clamp ( distance * m_distanceRatio, m_minY, m_maxY );
        //    TweenPosition.Begin ( m_other, 0.2f, pos ).method = UITweener.Method.EaseOut;
        //}
	}

    //public void SetupPlayerIndicator(TRCarController aPlayer)
    internal void SetTargetCar(CarController aCar, int aLaneCount)
	{
		_target = aCar;
        _laneCount = aLaneCount;

        _mine.SetActive(_target.isLocalPlayer);
        _other.SetActive(!_target.isLocalPlayer);
	}

    //public void SetPlayerMine()    
    //{
    //    //if ( null != m_mine ) m_mine.SetActive(true);
    //    //if ( null != m_other ) m_other.SetActive(false);
    //}
}
