using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HUD : MonoBehaviour
{
    public GameObject _tutorial;
    public GameObject _ingame;

    public Text _tutorialText;
    public Text _currentLap;
    public Text _centerText;
    public Text _maxLap;
    public Text _lapTime;

    [SerializeField]
    private GameObject _tutorialIngameGO;

    [SerializeField]
    private Image _countDownImage;

    [SerializeField]
    private Sprite[] _changeImage;

    [SerializeField]
    private Image _goImage;

    [SerializeField]
    private Button[] _itemButton = new Button[3];

    [SerializeField]
    private TRPanel_UseItem m_useItemPanel;

    [SerializeField]
    private TRHUD_Abilities _ability;

    [SerializeField]
    private QuickMatchResult _quickMatchResult;

    private CarController _playerCar;

    public GameObject _finalEffect;
    private bool m_showFinalLap = false;
    public bool _stolenItem = false;

    private float _lapStartTime = 0;
    private int _min = 0;
    private int _second = 0;

    void Start ()
    {
        if(!TutorialConstants._tutorialPlaying)
        {
            _tutorialIngameGO.SetActive(false);
        }

        _countDownImage.gameObject.SetActive(false);
        _goImage.gameObject.SetActive(false);
        for (int i = 0; i < _itemButton.Length; i++)
        {
            _itemButton[i].gameObject.SetActive(false);
        }
    }

    /** 아이템 아이콘이 갱신되었을때 호출됨 */
    public void UpdateItemIcon(int aKeyItemID, int[] aItemKeys)
    {
        if (null != m_useItemPanel)
            m_useItemPanel.SetItem(aKeyItemID, aItemKeys);
    }

    public void StolenItem()
    {
        if (null != m_useItemPanel)
            m_useItemPanel.StolenItem();
    }

    internal void UpdateJamming(bool aApply)
    {
        if (null == m_useItemPanel)
        {
            return;
        }
        m_useItemPanel.UpdateJamming(aApply);
    }

    public IEnumerator CountDownImage(int aImageNumber, float aCountDown)
    {
        while (aImageNumber < 3)
        {
            _countDownImage.gameObject.SetActive(true);
            _countDownImage.sprite = _changeImage[aImageNumber];
            aImageNumber++;
            yield return new WaitForSeconds(aCountDown);
        }
        _countDownImage.gameObject.SetActive(false);
        _goImage.gameObject.SetActive(true);
    }

    internal void UpdateLapCount(int aCount)
    {
        _currentLap.text = (aCount + 1).ToString();
    }

    internal void SetLapCount(CarController aCar)
    {
        _playerCar = aCar;
        _currentLap.text = (_playerCar._lapCount + 1).ToString();
        _maxLap.text = (_playerCar._goalLapCount).ToString();
    }

    public IEnumerator ShowFinalLapEffect()
    {
        if (m_showFinalLap)
            yield break;
        _finalEffect.SetActive(true);
        m_showFinalLap = true;
        
        yield return new WaitForSeconds(2.5f);
        _finalEffect.SetActive(false);
    }

    internal void RegisterCar(CarController aCar)
    {
        //_navigator.SetupIndicator(aCar);
        //_playerStatusPanel.SetupPlayerStatus(aCar);

        if (aCar.isLocalPlayer)
        {
            _playerCar = aCar;
            m_useItemPanel.SetPlayerCar(aCar);
            
            //SetAbilities(_playerCar.GetCarAbilityID());
            
            //SetMatchCar(_playerCar.GetCarID());
            //ShowWarningHUD(_playerCar._dangerAlarm, _playerCar._swc.Spline.LaneCount);
        }
    }

    internal void EndGame(CarController aMe, int aFirstReward, int aSecondReward)
    {   
        if (Constants.GameMode.QUICK == PlayerDataRepository.Instance.CurrentGameMode)
        {
            _quickMatchResult.SetGameResult(aMe, aFirstReward, aSecondReward);
            _quickMatchResult.gameObject.SetActive(true);
        }
        else
        {

        }
    }

    internal void TextChange()
    {
        if (TutorialConstants._tutorialPlaying)
        {
            _ingame.SetActive(false);
            _tutorial.SetActive(true);
            _tutorialText.text = "TUTORIAL";
        }
        else
        {
            _ingame.SetActive(true);
            _tutorial.SetActive(false);
            if(m_showFinalLap)
            {
                _centerText.text = "Final";
                _currentLap.gameObject.SetActive(false);
                _maxLap.gameObject.SetActive(false);
            }
        }
    }

    internal void IngamePlayTime()
    {
        _lapStartTime += Time.deltaTime;
        if(_lapStartTime > 0.99f)
        {
            _lapStartTime = 0;
            _second++;
            if(_second > 59)
            {
                _second = 0;
                _min++;
            }
        }
        _lapTime.text = string.Format("{0:00}:{1:00}:{2:00}", _min,_second,_lapStartTime * 100);
    }
#region ability
    internal void SetAbilities(List<int> aAbilityIDs)
    {
        //_ability.SetAbilities(aAbilityIDs);
    }

    internal void UpdateAbilityGauge(int aAbilityID, float aGauge)
    {
        //if (null == _ability)
        //{
        //    YPLog.LogError("TRHUD::_abilities is null. please setup!");
        //    return;
        //}

        //_ability.UpdateAbilityGauge(aAbilityID, aGauge);
    }

    internal void AbilityActivated(int aAbilityID)
    {
        //if (null == _ability)
        //{
        //    YPLog.LogError("TRHUD::_abilities is null. please setup!");
        //    return;
        //}

        //_ability.AbilityActivated(aAbilityID);
    }
#endregion
}
