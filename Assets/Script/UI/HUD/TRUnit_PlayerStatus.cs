using UnityEngine;
using System.Collections;
using SimpleJSON;

public class TRUnit_PlayerStatus : MonoBehaviour 
{
    [SerializeField]
    private UISlider            m_playerHP;

    [SerializeField]
    private UISlider            m_playerHPDecrease;    

    [SerializeField]
    private UILabel             m_playerName;

    [SerializeField]
    private GameObject[]        m_damageHP              = new GameObject[5];

    [SerializeField]
    private UIFont[]            m_hpFont                = new UIFont[2];

    [SerializeField]
    private Animation           _indicateAnimation;

    private CarController       _target;
    private int                 _oldHP                  = 0;
    private int                 _damageCount            = 0;
    private const float         DAMAGE_SHOW_TIME        = 1f;

    void LateUpdate()
    {
        if (null == _target)
        {
            return;
        }

        UpdateHPStatus();

        Camera worldCam = NGUITools.FindCameraForLayer(_target.gameObject.layer);
        Camera guiCam = NGUITools.FindCameraForLayer(gameObject.layer);
        Vector3 pos = guiCam.ViewportToWorldPoint(worldCam.WorldToViewportPoint(_target.NamePosition));
        pos.z = 0;
        gameObject.transform.position = pos;

        //if (null == m_player)
        //{
        //    return;
        //}
        
        //StartCoroutine("ShowDamageHP");        

        //m_playerHP.value = (float)m_player.HP / (float)m_player.MaxHP;
        //if (m_playerHP.value < m_playerHPDecrease.value)
        //{
        //    _decreaseRatio = (m_playerHPDecrease.value - m_playerHP.value) / 0.5f;
        //}
        //else
        //{
        //    _decreaseRatio = 0f;
        //    m_playerHPDecrease.value = m_playerHP.value;
        //}

        //m_playerHPDecrease.value -= _decreaseRatio * Time.deltaTime;

        //Camera worldCam = NGUITools.FindCameraForLayer(m_player.gameObject.layer);
        //Camera guiCam = NGUITools.FindCameraForLayer(gameObject.layer);

        //Vector3 pos = guiCam.ViewportToWorldPoint(worldCam.WorldToViewportPoint(m_player.NamePosition ));
        //pos.z = 0;

        //gameObject.transform.position = pos;
    }

    private void UpdateHPStatus()
    {
        if (_oldHP == _target._hp)
        {
            return;
        }

        StartCoroutine(ShowDamage());
        StartCoroutine(ChangeHPBar());
    }

    private IEnumerator ShowDamage()
    {
        int damage = _target._hp - _oldHP;
        _oldHP = _target._hp;

        if (0 != damage)
        {
            int index = _damageCount % 5;
            GameObject go = m_damageHP[index];
            Vector3 localPosition = go.transform.localPosition;

            ++_damageCount;

            UILabel label = go.GetComponent<UILabel>();
            if (damage > 0)
            {
                label.ambigiousFont = m_hpFont[0];
                label.text = "+" + damage.ToString();
            }
            else
            {
                label.ambigiousFont = m_hpFont[1];
                label.text = damage.ToString();
            }

            go.SetActive(true);
            go.GetComponent<Animation>().Play("hp_decrease_up");

            TweenAlpha tween = go.GetComponent<TweenAlpha>();
            tween.value = tween.from;
            TweenAlpha.Begin(go, DAMAGE_SHOW_TIME, tween.to);

            yield return new WaitForSeconds(DAMAGE_SHOW_TIME);

            go.GetComponent<Animation>().Stop();
            go.SetActive(false);
            go.transform.localPosition = localPosition;
        }
    }

    private IEnumerator ChangeHPBar()
    {
        m_playerHP.value = (float)_target._hp / (float)_target._maxHP;
        while (m_playerHP.value < m_playerHPDecrease.value)
        {
            float ratio = (m_playerHPDecrease.value - m_playerHP.value) / 0.5f;
            m_playerHPDecrease.value -= ratio * Time.deltaTime;

            yield return null;
        }

        m_playerHPDecrease.value = m_playerHP.value;
        
    }
    
    internal void SetupPlayerStatus(CarController aCar)
    {
        gameObject.SetActive(true);

        _target = aCar;
        //if (null != _target)
        //{
        //    _target.PlayerStatus = this;
        //}
        _oldHP = _target._hp;
        m_playerHP.value = 1f;
        //m_oldHP = _target.HP;

        // maybe useless, because image & name is already cached at loading scene.
   //     protocol.integrated_info info = TRDataManager.instance.GetMatchedPlayerInfo(_target._playerUUID);
   //     if (null != info)
   //     {
   //         //m_playerName.text = TRPlatformWrapper.Instance.GetPlayerName(m_player.PlayerUUID, delegate(string s) { m_playerName.text = s; }, info.GetInfo().GetPlayerNickname());
			//m_playerName.text = info.GetInfo().GetPlayerNickname();
   //     }
   //     else
   //     {
   //         m_playerName.text = TRPlatformWrapper.Instance.GetPlayerName(_target._playerUUID, delegate(string s) { m_playerName.text = s; }, Constants.EMPTY_STRING);
   //     }
    }

    internal IEnumerator Indicate()
    {
        _indicateAnimation.Play("my_car_arrow_move");
        _indicateAnimation.gameObject.SetActive(true);

        while (!_target._matchStart)
        {
            yield return null;
        }

        _indicateAnimation.Stop();
        _indicateAnimation.gameObject.SetActive(false);
    }    

    //internal IEnumerator ShowMyCar()
    //{
    //    m_myCar.SetActive(true);
    //    m_myCar.GetComponent<Animation>().Play("my_car_arrow_move");

    //    while (!_target._matchStart)
    //    {
    //        yield return null;
    //    }

    //    m_myCar.GetComponent<Animation>().Stop();
    //    m_myCar.SetActive(false);
    //}

    //IEnumerator ShowDamageHP()
    //{
    //    int damage = _target.HP - m_oldHP;
    //    m_oldHP = _target.HP;

    //    if (0 != damage)
    //    {
    //        int index = _damageCount % 5;
    //        GameObject go = m_damageHP[index];
    //        Vector3 localPosition = go.transform.localPosition;

    //        ++_damageCount;

    //        UILabel label = go.GetComponent<UILabel>();
    //        if (damage > 0)
    //        {
    //            label.ambigiousFont = m_hpFont[0];
    //            label.text = "+" + damage.ToString();
    //        }
    //        else
    //        {
    //            label.ambigiousFont = m_hpFont[1];
    //            label.text = damage.ToString();
    //        }

    //        go.SetActive(true);
    //        go.GetComponent<Animation>().Play("hp_decrease_up");

    //        TweenAlpha tween = go.GetComponent<TweenAlpha>();
    //        tween.value = tween.from;
    //        TweenAlpha.Begin(go, DAMAGE_SHOW_TIME, tween.to);

    //        yield return new WaitForSeconds(DAMAGE_SHOW_TIME);

    //        go.GetComponent<Animation>().Stop();
    //        go.SetActive(false);
    //        go.transform.localPosition = localPosition;
    //    }

    //    yield return null;
    //}
}