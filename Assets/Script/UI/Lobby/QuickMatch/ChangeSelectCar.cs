using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Network;
using SmartLocalization;

public class ChangeSelectCar : PopupComponent
{
    [SerializeField]
    private ChangeSelectCarUnit _current;

    [SerializeField]
    private ChangeSelectCarUnit _change;

    [SerializeField]
    private Text _changeCarText;

    [SerializeField]
    private string _changeCarTextKeyword;

    [SerializeField]
    private TextSelector _carName;

    private int _id;

    internal void UpdateInfo(int aChangeCarID)
    {
        _id = aChangeCarID;
        _current.UpdateInfo(PlayerDataRepository.Instance.SelectCarNo);
        _change.UpdateInfo(aChangeCarID);

        _changeCarText.text = string.Format(LanguageManager.Instance.GetTextValue(_changeCarTextKeyword), LanguageManager.Instance.GetTextValue(_carName.GetKey(aChangeCarID)));
    }

    public void Change()
    {
        LobbyRequest.Instance.ChangeSelectCarReq(_id);
    }
}