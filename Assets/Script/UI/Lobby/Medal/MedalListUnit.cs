using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MedalListUnit : MonoBehaviour 
{
    [SerializeField]
    private ImageSelector _icon;

    [SerializeField]
    private ImageSelector _iconBG;

    [SerializeField]
    private TextSelector _name;

    [SerializeField]
    private TextSelector _desc;

    [SerializeField]
    private ImageSelector _rewardIcon;

    [SerializeField]
    private GameObject _progress;

    [SerializeField]
    private GameObject _end;

    [SerializeField]
    private Slider _progressSlider;

    [SerializeField]
    private Text _progressState;

    [SerializeField]
    private Button _rewardButton;

    [SerializeField]
    private TextSelector _rewardButtonText;

    public int ID { get; private set; }

    internal void UpdateInfo(PlayerDataRepository.AchievementInfo aInfo)
    {
        ID = aInfo.AchievementId;
        
        _iconBG.SetImage(aInfo.IsReceiveReward ? 1 : 0);
        _icon.SetImage(ID);
        _name.SetText(ID);
        _desc.SetText(ID);

        _progress.SetActive(false);
        _end.SetActive(false);
        if (aInfo.Progress == aInfo.Goal)
        {
            if (aInfo.IsReceiveReward)
            {
                _end.SetActive(true);
            }
            else
            {
                _progressSlider.value = 1f;
                _progressState.text = StringFormat.CurrentWithMax(aInfo.Progress, aInfo.Goal);
                _progress.SetActive(true);

                _rewardButtonText.SetText(1);
                _rewardButton.interactable = true;
            }
        }
        else
        {
            _progressSlider.value = (float)aInfo.Progress / (float)aInfo.Goal;
            _progressState.text = StringFormat.CurrentWithMax(aInfo.Progress, aInfo.Goal);
            _progress.SetActive(true);

            _rewardButtonText.SetText(0);
            _rewardButton.interactable = false;
        }
    }

    public void ReceiveReward()
    {
        //LSConnector.Instance.AchievementReceiveRewardReq(ID);
    }
}