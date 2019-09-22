using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameModeBase : GUIComponent
{
    [SerializeField]
    private Text _level;

    [SerializeField]
    private Image _expGauge;

    [SerializeField]
    private Text _name;

    [SerializeField]
    private ImageSelector _nation;

    [SerializeField]
    private List<GameObject> _toggleGO = new List<GameObject>();

    void Awake()
    {
        UpdateProfileInfo();
    }

    public override void OnHandleEvent(GameEventType gameEventType, params object[] args)
    {
        switch (gameEventType)
        {
            case GameEventType.PlayerIntegratedInfoAnsOK:         
            case GameEventType.UpdatePlayerProfileInfoAnsOK:
            case GameEventType.UpdatePlayerNickAnsOK:
                UpdateProfileInfo();

                break;
        }
    }

    private void UpdateProfileInfo()
    {
        var exp = PlayerDataRepository.Instance.MyPlayerInfo.Exp;
        _level.text = Constants.GetPlayerLevelByExp(exp).ToString();
        _expGauge.fillAmount = Constants.GetPlayerExpRatio(exp);
        _name.text = PlayerDataRepository.Instance.MyPlayerInfo.Nick;
        _nation.SetImage(PlayerDataRepository.Instance.MyPlayerInfo.NationCode);
    }

    public void Toggle(bool aShow)
    {
        foreach (GameObject go in _toggleGO)
        {
            go.SetActive(aShow);
        }
    }
}