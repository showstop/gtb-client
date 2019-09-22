using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DangerAlarmManager : MonoBehaviour 
{
    [SerializeField]
    private List<TRHUDUnit_Warning> _lane = new List<TRHUDUnit_Warning>();

    private Dictionary<int, List<Item>> _dangerItem = new Dictionary<int, List<Item>>();

    void Update()
    {
        for (int index = 0; index < _lane.Count; ++index)
        {
            int laneNO = index + 1;
            if (!_dangerItem.ContainsKey(laneNO))
            {
                continue;
            }
            
            _lane[index].UpdateWarning(_dangerItem[laneNO].Count != 0);
        }
    }

    internal void AddItem(int aLane, Item aItem)
    {
        if (_dangerItem.ContainsKey(aLane))
        {
            _dangerItem[aLane].Add(aItem);
        }
        else
        {
            List<Item> items = new List<Item>();
            items.Add(aItem);

            _dangerItem.Add(aLane, items);
        }
    }

    internal void RemoveItem(int aLane, Item aItem)
    {
        if (!_dangerItem.ContainsKey(aLane))
        {
            YPLog.LogError("never happen!!! lane = " + aLane + ", item = " + aItem);
            return;
        }

        _dangerItem[aLane].Remove(aItem);
    }
}