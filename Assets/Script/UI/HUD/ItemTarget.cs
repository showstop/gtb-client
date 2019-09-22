using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SimpleJSON;

public class ItemTarget : MonoBehaviour
{
    [SerializeField]
    private TRPanel_UseItem _useItemPanel;

    [SerializeField]
    private Image m_targetPlayerAvatar;
    public Image TargetPlayerAvatar
    {
        set { if (null != value) { m_targetPlayerAvatar = value; } }
    }

    [SerializeField]
    private Text m_targetPlayerName;

    [SerializeField]
    private Text m_targetPlayerRank;
    private int m_rank = 1;
    public int Rank
    {
        get { return m_rank; }
        set
        {
            m_rank = value;

            m_targetPlayerRank.text = m_rank.ToString();
        }
    }

    [SerializeField]
    private Image m_targetPlayerHP;
    private float m_durationTime = 1.0f;
    private float m_currentHP = 0.0f;
    private float m_hp = 0.0f;
    public float HP
    {
        get { return m_hp; }
        set
        {
            m_hp = value;

            StopCoroutine("TweenValue");
            StartCoroutine("TweenValue");
        }
    }

    [SerializeField]
    private GameObject m_activated;
    public bool Active { set { m_activated.SetActive(value); } }

    [SerializeField]
    private GameObject m_jamming;
    public bool Jamming
    {
        get
        {
            return m_jamming.activeInHierarchy;
        }
        set
        {
            m_jamming.SetActive(value);
        }
    }

    [SerializeField]
    private Button m_button;

    public string PlayerUUID { get; private set; }

    void Start()
    {
        PlayerUUID = Constants.INVALID_PLAYER_UUID;
    }

    public void OnTargetPlayerClick()
    {
        _useItemPanel.OnTargetPlayerClick(PlayerUUID);
        //TRCarController myCar = TRStatic.GetGameManager().GetMyCarController();
        //if (null != myCar)
        //{
        //    if (Network.NetworkClient.active)
        //        myCar.GetComponent<NetworkView>().RPC("UseItemToTarget", RPCMode.Server, PlayerUUID.ToString());
        //    else
        //        myCar.UseItemToTarget(PlayerUUID.ToString());
        //}
    }

    internal void SetupTargetPlayer(CarController aTarget, float aOffsetY, bool aJamming)
    {
        PlayerUUID = aTarget._playerNo;
        HP = 1.0f;
        Rank = aTarget._rank;
        Jamming = aJamming;
        Active = !aJamming;
        m_button.enabled = !aJamming;

        // maybe useless, because image & name is already cached at loading scene.
        //protocol.integrated_info info = TRDataManager.instance.GetMatchedPlayerInfo(PlayerUUID);
        //if (null != info)
        //{
        //    TargetPlayerAvatar = TRPlatformWrapper.Instance.GetPlayerImage(PlayerUUID, delegate(Texture texture) { TargetPlayerAvatar = texture; }, info.GetInfo().GetPlayerProfileUrl(), info.GetInfo().GetIsProfileOpen());
        //    m_targetPlayerName.text = info.GetInfo().GetPlayerNickname();
        //}
        //else
        //{
        //    TargetPlayerAvatar = TRPlatformWrapper.Instance.GetPlayerImage(PlayerUUID, delegate(Texture texture) { TargetPlayerAvatar = texture; }, TRConstants.EMPTY_STRING, true);
        //    m_targetPlayerName.text = TRPlatformWrapper.Instance.GetPlayerName(PlayerUUID, delegate(string buffer) { m_targetPlayerName.text = buffer; }, TRConstants.EMPTY_STRING);
        //}
    }

    internal void UpdateTargetPlayer(int currentHP, int maxHP, float aOffsetY, int rank, bool aJamming)
    {
        float ratioHP = (float)currentHP / (float)maxHP;
        HP = ratioHP;

        //gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, aOffsetY, gameObject.transform.localPosition.z);
        gameObject.SetActive(true);

        Rank = rank;
        Jamming = aJamming;
        Active = !aJamming;
        m_button.enabled = !aJamming;
    }

    private IEnumerator TweenValue()
    {
        float t = 0.0f;
        float value = 0.0f;
        float fromValue = m_currentHP;

        while (true)
        {
            t += Time.deltaTime / m_durationTime;
            value = Mathf.Lerp(fromValue, m_hp, t);

            m_currentHP = value;
            m_targetPlayerHP.fillAmount = value;

            if (value == m_hp)
                break;

            yield return null;
        }

        yield return null;
    }
}