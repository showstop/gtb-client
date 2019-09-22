using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestHUD : MonoBehaviour 
{
    [SerializeField]
    private GameManager _gm;

    [SerializeField]
    private ItemManager _itemManager;

    [SerializeField]
    private UIPopupList _itemList;

    [SerializeField]
    private UILabel _currentItem;

    private CarController _target;

	void Start()
    {
        ItemListUp();
        StartCoroutine(GetPlayer());
    }

    private void ItemListUp()
    {
        _itemList.items.Clear();

        List<GameObject> items = _itemManager.GetItemList();
        for (int index = 0; index < items.Count; ++index)
        {
            _itemList.items.Add(items[index].name);
        }

        _itemList.value = _itemList.items[0];
        SelectItem();
    }

    private IEnumerator GetPlayer()
    {
        while (!_gm._startMatch)
        {
            yield return null;
        }

        _target = _gm.GetLocalPlayer();
    }

#region UI event
    public void SelectItem()
    {
        _currentItem.text = _itemList.value;
    }

    public void GetItem()
    {
        string[] split = _itemList.value.Split('.');
        int itemID = System.Convert.ToInt32(split[0]);
        _gm.GiveSpecifiedItem(_target, itemID);
    }
#endregion
}