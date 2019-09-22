using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

// TO DO : change class name.
[System.Serializable]
public class ItemChance
{
    public int _itemID;
    public int _chance;
}

public class ItemManager : NetworkBehaviour
{
    [SerializeField]
    private List<GameObject> _items = new List<GameObject>();

    private Dictionary<int, GameObject> _itemPrefab = new Dictionary<int, GameObject>();

    [SerializeField]
    private List<int> _targetableItemIDList = new List<int>();

    [SerializeField]
    private List<ItemChance> _first = new List<ItemChance>();

    [SerializeField]
    private List<ItemChance> _second = new List<ItemChance>();

    [SerializeField]
    private List<ItemChance> _third = new List<ItemChance>();

    [SerializeField]
    private List<ItemChance> _fouth = new List<ItemChance>();

    private int[] _totalChance = new int[4];

    void Start()
    {
        for (int index = 0; index < _items.Count; ++index)
        {
            Item item = _items[index].GetComponentInChildren<Item>();
            _itemPrefab.Add(item._itemID, _items[index]);
            //YPLog.Log("itemID = " + item._itemID + ", item = " + _items[index]);
        }

        for (int index = 0; index < _totalChance.Length; ++index)
        {
            List<ItemChance> itemList = GetItemList(index + 1);
            foreach (ItemChance item in itemList)
            {
                _totalChance[index] += item._chance;
            }
        }
    }

	internal void GiveItem(CarController aCar, int aRank)
    {
        //YPLog.Trace();
        //YPLog.Log("car = " + aCar + ", rank = " + aRank);

        aCar.UpdatePlayData((short)Constants.StatKey.ACQ_INGAME_ITEM, 1);
        aCar.GetItemBox();
        if (!aCar.CanStackItem)
        {   
            return;
        }

        int itemID;
        //tutorial
        if(TutorialConstants._tutorialPlaying)
        {
            if(TutorialConstants._tutorialAbility)
            {
                itemID = 21;
            }
            else
            {
                itemID = 2;
            }
        }

        else
        {
            itemID = DecideItem(aRank);
        }
        if (!_itemPrefab.ContainsKey(itemID))
        {
            YPLog.LogError("check item ID[" + itemID + "]!");
            return;
        }

        // change item ability
        int abilityID = -1;
        int applyLevel = 1;
        int changeItemID = aCar.ChangeItem(itemID, ref abilityID, ref applyLevel);
        if (-1 != changeItemID)
        {
            if (!_itemPrefab.ContainsKey(changeItemID))
            {
                YPLog.LogError("check item ID[" + changeItemID + "]!");
                return;
            }

            itemID = changeItemID;
        }

        // TO DO : enhance item like ItemOneWay(increase apply time)
        GameObject go = Instantiate(_itemPrefab[itemID], Vector3.zero, Quaternion.identity) as GameObject;
        Item item = go.GetComponent<Item>();
        item.GiveTo(aCar);
        item.SetLevel(applyLevel);
        AbilityDatabase.Instance.EnhanceItem(abilityID, applyLevel, ref item);

        NetworkServer.Spawn(go);
        item.SpawnFireUnit();
    }

    private int DecideItem(int aRank)
    {
        List<ItemChance> itemList = GetItemList(aRank);
        int pick = Random.Range(1, _totalChance[aRank - 1] + 1);
        int current = 0;
        for (int index = 0; index < itemList.Count; ++index)
        {
            current += itemList[index]._chance;
            if (pick <= current)
            {
                return itemList[index]._itemID;
            }
        }

        return -1;
    }

    private List<ItemChance> GetItemList(int aRank)
    {
        switch (aRank)
        {
            case 1: return _first;
            case 2: return _second;
            case 3: return _third;
            case 4: return _fouth;
        }

        return null;
    }

    internal void GiveSpecifiedItem(CarController aCar, int aItemID)
    {
        if (!aCar.CanStackItem)
        {
            aCar.ForceRemoveItem();
        }

        if (!_itemPrefab.ContainsKey(aItemID))
        {
            YPLog.LogError("check item ID[" + aItemID + "]!");
            return;
        }
        int applyLevel = 1;
        GameObject go = Instantiate(_itemPrefab[aItemID], Vector3.zero, Quaternion.identity) as GameObject;
        Item item = go.GetComponent<Item>();

        item.GiveTo(aCar);
        item.SetLevel(applyLevel);
        NetworkServer.Spawn(go);
        item.SpawnFireUnit();

    }

    internal bool IsTargetableItem(int aItemID)
    {
        return _targetableItemIDList.Contains(aItemID);
    }

    internal List<GameObject> GetItemList()
    {
        return _items;
    }
}