using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ItemBoxSpawner : NetworkBehaviour
{
    public GameObject _itemBoxPrefab;
    public CurvySpline _spline;
    public float _spawnDelay;
    public float _spawnDistance;

    private ItemBox[] _itemBoxes;
    private int _itemBoxCount = 0;
    private bool _startMatch = false;

    public override void OnStartServer()
    {
        //YPLog.Trace();
        SpawnItemBoxes();
        TutorialConstants.count++;
    }

    internal void SetInfo(GameObject aItemBoxPrefab, CurvySpline aSpline)
    {
        YPLog.Log("itemBoxPrefab = " + aItemBoxPrefab + ", spline = " + aSpline);

        _itemBoxPrefab = aItemBoxPrefab;
        _spline = aSpline;
    }

    internal void SpawnItemBoxes()
    {
        _itemBoxCount = _spline.LaneCount;
        _itemBoxes = new ItemBox[_itemBoxCount];

        for (int index = 0; index < _itemBoxCount; ++index)
        {
            GameObject go = Instantiate(_itemBoxPrefab, Vector3.zero, Quaternion.identity) as GameObject;
            ItemBox itemBox = go.GetComponent<ItemBox>();
            itemBox.SetInfo(this, _spline, index + 1, _spawnDistance);
            itemBox.name = "itemBox_" + index.ToString();
            _itemBoxes[index] = itemBox;

            NetworkServer.Spawn(go);
        }
    }

    internal void StartMatch()
    {
        _startMatch = true;
        StartCoroutine(UpdateItemBoxes());
    }

    internal void EndMatch()
    {
        _startMatch = false;
    }

    private IEnumerator UpdateItemBoxes()
    {   
        while (_startMatch)
        {
            for (int index = 0; index < _itemBoxes.Length; ++index)
            {   
                if (_itemBoxes[index].Active)
                {
                    continue;
                }

                _itemBoxes[index].ToggleActive(true);
            }

            yield return new WaitForSeconds(_spawnDelay);
        }
    }

    public void UpdateLocation()
    {
        if (null == _spline)
        {
            return;
        }

        int dir = 1;
        float tf = _spline.DistanceToTF(_spawnDistance);
        transform.position = _spline.MoveBy(ref tf, ref dir, 0f, CurvyClamping.Clamp);
    }

    internal void BoxEnable(bool aShow)
    {
        for(int i = 0; i < TutorialConstants.count; i++)
        {
            for (int j = 0; j < _itemBoxes.Length; j++)
            {
                _itemBoxes[j].gameObject.SetActive(aShow);
            }
        }
    }
}