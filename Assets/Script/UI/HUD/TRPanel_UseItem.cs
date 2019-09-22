using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TRPanel_UseItem : MonoBehaviour//TRPanel
{
    [SerializeField]
    private GameObject _nonTargetButton;

    [SerializeField]
    private UISprite                        m_itemSprite;

    [SerializeField]
    private GameObject                      m_useItemButtonGO;

    [SerializeField]
    private GameObject                      m_targetUseGO;

    [SerializeField]
    //private TRUnit_UseItemTargetPlayer[]    m_targetPlayers     = new TRUnit_UseItemTargetPlayer[Constants.MAX_PLAYER_NUM - 1];
    private ItemTarget[] m_targetPlayers = new ItemTarget[Constants.MAX_PLAYER_NUM - 1];

    [SerializeField]
    private float[]                         m_offsetY           = new float[Constants.MAX_PLAYER_NUM - 1];

    private bool                            m_useItemAuto       = false;
    public bool                             UseItemAuto         { set { m_useItemAuto = value; } }

    private TRSwipe                         m_swipe;
    private bool                            _targetButtonShow  = false;
    
    private bool                            _targetable         = false;
    private CarController                   _playerCar          = null;
    private GameManager                     _gm;



    void Start()
    {
        m_swipe = gameObject.GetComponent<TRSwipe>();
        m_swipe.UseItemPanel = this;

        // TO DO : fix me
        _gm = GameObject.FindWithTag(Constants.GAME_MANAGER_TAG_NAME).GetComponent<GameManager>();
    }

    internal void SetPlayerCar(CarController aCar)
    {
        _playerCar = aCar;
    }

    private void SetItemImage(UITexture aTexture, int aItemID)
    {
        string fileName = string.Format("Icon/item_{0:00}", aItemID);
        Texture texture = TRResourceManager.Get<Texture>(fileName);
		if ( null != texture )
		{
			aTexture.gameObject.SetActive(true);
			aTexture.mainTexture = texture;
		}
        
		aTexture.gameObject.GetComponent<Animation>().Play("item_stack");
        //aSprite.MakePixelPerfect();
    }

    internal void SetItemUseButton(int aItemID, bool aJamming)
    {
        if (0 > aItemID)
        {
            return;
        }
        _targetable = _gm.IsTargetableItem(aItemID);
        _nonTargetButton.SetActive(true);

        if (_targetable)
        {
            m_targetUseGO.SetActive(true);

            for (int index = 0; index < m_targetPlayers.Length; ++index)
            {
                m_targetPlayers[index].Active = true;
            }

            int offsetIndex = 0;
            for (int index = 0; index < Constants.MAX_PLAYER_NUM; ++index)
            {
                CarController player = _gm.GetCarWithRank(index + 1);
                if (null == player || player == _playerCar)
                {
                    continue;
                }

                SetupTargetPlayer(player, offsetIndex, aJamming);
                ++offsetIndex;
            }
            if (!_targetButtonShow)
            {
                m_useItemButtonGO.SetActive(false);
                //m_targetUseGO.GetComponent<Animation>().Play("itemuse_targetplayers_show");
                _targetButtonShow = true;
                _nonTargetButton.SetActive(false);
            }

            StartCoroutine(UpdateTargetPlayers());
        }
        else
        {
            if (_targetButtonShow)
            {
                //m_targetUseGO.GetComponent<Animation>().Play("itemuse_targetplayers_hide");

                for (int index = 0; index < m_targetPlayers.Length; ++index)
                {
                    m_targetPlayers[index].Active = false;
                }
            }

            _targetButtonShow = false;
        }

        m_useItemButtonGO.SetActive(!aJamming);
    }

    internal void SetItem(int aKeyItemID, int[] aItemKeys)
    {
        YPLog.Log("keyitemID = " + aKeyItemID + ", itemcount = " + aItemKeys.Length);

        if (aItemKeys.Length == 1)
        {
            aKeyItemID = aItemKeys[0];
            m_swipe.SetItem(aItemKeys);
        }
        else if (aItemKeys.Length == 2)
        {
            aKeyItemID = aItemKeys[0];
            aItemKeys[0] = aItemKeys[1];
            m_swipe.SetItem(aItemKeys);
        }
        else if (aItemKeys.Length == 3)
        {
            aKeyItemID = aItemKeys[0];
            aItemKeys[0] = aItemKeys[1];
            aItemKeys[1] = aItemKeys[2];
            aItemKeys[2] = 0;
            m_swipe.SetItem(aItemKeys);
        }

        //m_swipe.StopSwipe();

        //int updateItemCount = aItemKeys.Length;
        //for (int index = 0; index < updateItemCount; ++index)
        //{   
        //    UITexture texture;
        //    if (m_swipe.GetItemTexture(index, aItemKeys[index], out texture))
        //    {
        //        int itemID = aItemKeys[index] % 100;
        //        SetItemImage(texture, itemID);
        //    }
        //}

        m_swipe.UpdatePendingItems(aItemKeys);

        int updateItemCount = aItemKeys.Length;
        if (0 != updateItemCount)
        {
            m_swipe.SetFocusItem(aKeyItemID);
        }
        m_swipe.ItemCount = updateItemCount;
    }

    internal void StolenItem()
    {
        m_swipe.StolenItem();

        if (_targetButtonShow)
        {
            //m_targetUseGO.GetComponent<Animation>().Play("itemuse_targetplayers_hide");

            for (int index = 0; index < m_targetPlayers.Length; ++index)
            {
                m_targetPlayers[index].Active = false;
            }
        }

        _targetButtonShow = false;

        m_targetUseGO.SetActive(false);
                  
        m_useItemButtonGO.SetActive(false);
    }

    private void SetupTargetPlayer(CarController aPlayer, int aOffsetIndex, bool aJamming)
    {
        int firstSetupIndex = -1;
        for (int index = 0; index < m_targetPlayers.Length; ++index)
        {
            if (Constants.INVALID_PLAYER_UUID == m_targetPlayers[index].PlayerUUID)
            {
                if (-1 == firstSetupIndex)
                {
                    firstSetupIndex = index;
                    continue;
                }
            }

            if (aPlayer._playerNo == m_targetPlayers[index].PlayerUUID)
            {
				m_targetPlayers[index].UpdateTargetPlayer(aPlayer._hp, aPlayer._maxHP, m_offsetY[aOffsetIndex], aPlayer._rank, aJamming);
                //m_targetPlayers[index].UpdateTargetPlayer(aPlayer.HP, aPlayer.MaxHP, m_offsetY[aOffsetIndex], aPlayer.Rank, aJamming);
                return;
            }
        }

        if (-1 != firstSetupIndex)
        {
            m_targetPlayers[firstSetupIndex].SetupTargetPlayer(aPlayer, m_offsetY[aOffsetIndex], aJamming);
        }
    }

    private IEnumerator UpdateTargetPlayers()
    {
        while (_targetButtonShow)
        {
            int offsetIndex = 0;            
            for (int index = 0; index < Constants.MAX_PLAYER_NUM; ++index)
            {
                CarController player = _gm.GetCarWithRank(index + 1);
                if (null == player || _playerCar == player)
                {
                    continue;
                }                

                SetupTargetPlayer(player, offsetIndex, m_swipe.Jamming);
                ++offsetIndex;
            }

            yield return new WaitForSeconds(0.1f);
        }

        //TRCarController myCar = TRStatic.GetGameManager().GetMyCarController();
        //while (_targetButtonShow)
        //{
        //    int offsetIndex = 0;            
        //    TRStatic.GetGameManager().ClientSortRank(false);
        //    for (int index = 0; index < Constants.MAX_PLAYER_NUM; ++index)
        //    {
        //        TRCarController player = TRStatic.GetGameManager().GetSortedPlayer(index);
        //        if (null != player && player != myCar)
        //        {
        //            SetupTargetPlayer(player, offsetIndex, m_swipe.Jamming);
        //            ++offsetIndex;
        //        }
        //    }

        //    yield return null;
        //}
    }

    internal void UpdateJamming(bool aApply)
    {
        m_swipe.UpdateJamming(aApply);
        m_useItemButtonGO.SetActive(!aApply);
    }

    public void OnUseItemButtonClick()
    {
        YPLog.Trace();

        if (m_useItemAuto)
        {
            return;
        }

        if (null == _playerCar)
        {
            return;
        }

        if (_playerCar._updateItemUseButton)
        {
            m_useItemButtonGO.SetActive(false);
            m_swipe.UseFocusItem();
        }

        if (_targetable)
        {
            _playerCar.CmdUseItemToTarget(Constants.EMPTY_STRING);
            //m_targetUseGO.GetComponent<Animation>().Play("itemuse_targetplayers_hide");
            for (int index = 0; index < m_targetPlayers.Length; ++index)
            {
                m_targetPlayers[index].Active = false;
            }

            _targetButtonShow = false;
        }
        else
        {
            _playerCar.CmdUseItem();
        }
    }

    internal void OnTargetPlayerClick(string aPlayerUUID)
    {
        m_useItemButtonGO.SetActive(false);
        m_swipe.UseFocusItem();
        //m_targetUseGO.GetComponent<Animation>().Play("itemuse_targetplayers_hide");

        for (int index = 0; index < m_targetPlayers.Length; ++index)
        {
            m_targetPlayers[index].Active = false;
        }

        _targetButtonShow = false;

        _playerCar.CmdUseItemToTarget(aPlayerUUID);

        m_targetUseGO.SetActive(false);
    }
}
