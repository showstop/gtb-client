using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// spline F : 0.0 ~ 1.0
public class MinMax
{
    public float _min;
    public float _max;
}

public class BoostPedalManager : MonoBehaviour 
{
    [SerializeField]
    private List<GameObject> _pedals = new List<GameObject>();

    [SerializeField]
    private List<MinMax> _range = new List<MinMax>();

    void Start()
    {
        GameObject startSplineGO = GameObject.FindWithTag(Constants.START_SPLINE_TAG_NAME);
        CurvySpline startSpline = startSplineGO.GetComponent<CurvySpline>();        

        foreach (MinMax mm in _range)
        {
            float f = Random.Range(mm._min, mm._max);
            SpawnPedals(startSpline, startSpline.LaneCount, f);
        }
    }

    private void SpawnPedals(CurvySpline aCS, int aLaneCount, float aInitialF)
    {
        List<int> spawnedLane = new List<int>();
        for (int index = 0; index < aLaneCount; ++index)
        {
            int lane = Random.Range(0, aLaneCount) + 1;
            while (spawnedLane.Contains(lane))
            {
                lane = Random.Range(0, aLaneCount) + 1;
            }
            spawnedLane.Add(lane);

            GameObject go = Instantiate(_pedals[index], Vector3.zero, Quaternion.identity) as GameObject;
            BoostPedal bp = go.GetComponent<BoostPedal>();
            bp.SetInfo(aCS, lane, aInitialF);
        }
    }
}
