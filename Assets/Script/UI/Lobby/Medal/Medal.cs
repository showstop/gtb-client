using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Medal : GUIComponent 
{
    [SerializeField]
    private GameObject _unit;

    [SerializeField]
    private RectTransform _listTransform;

    private List<MedalListUnit> _list = new List<MedalListUnit>();

    void Awake()
    {
        SetupList();
    }

    private void SetupList()
    {
        var achievementInfos = PlayerDataRepository.Instance.GetAchievementInfos();
        int count = achievementInfos.Count;
        float unitHeight = _unit.GetComponent<RectTransform>().rect.height;
        float scrollHeight = count * unitHeight;
        _listTransform.offsetMin = new Vector2(_listTransform.offsetMin.x, -scrollHeight / 2);
        _listTransform.offsetMax = new Vector2(_listTransform.offsetMax.x, scrollHeight / 2 + unitHeight / 4f);

        foreach (var kv in achievementInfos)
        {
            GameObject unit = Instantiate(_unit) as GameObject;
            unit.transform.SetParent(_listTransform.gameObject.transform);
            unit.transform.localScale = Vector3.one;

            MedalListUnit listUnit = unit.GetComponent<MedalListUnit>();
            listUnit.UpdateInfo(kv.Value);

            _list.Add(listUnit);
        }

        Transform transform = _listTransform.gameObject.transform;
        transform.localPosition = new Vector3(transform.localPosition.x, _listTransform.rect.height / -2f, transform.localPosition.y);
        
    }

    public override void OnHandleEvent(GameEventType gameEventType, params System.Object[] args)
    {
//        switch (gameEventType)
//        {
//            case GameEventType.AchievementReceiveRewardAnsOK:                
//                protocol.achievement_info info = ((protocol.achievement_receive_reward_ans)args[0]).GetUpdatedInfo();
//                MedalListUnit target = _list.Find(delegate (MedalListUnit unit) { return unit.ID == info.GetAchievementId(); });
//                target.UpdateInfo(info);
//
//                break;
//        }
    }
}