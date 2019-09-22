using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/*
 * currently only support x axis swipe(left - right) 
 */
public class TRSwipe : MonoBehaviour 
{
    private const int       MAX_SWIPE_ITEM_COUNT    = 3;

    public float            m_itemInterval          = 0f;
    public float            m_focusOffsetX          = -120f;
    private float           m_mostLeftItemOffsetX   = 0f;

    //public TRSwipeItem[] m_swipeItems = new ItemButton[MAX_SWIPE_ITEM_COUNT];
    //private TRSwipeItem m_focusItem;

    public ItemButton[]    m_swipeItems            = new ItemButton[MAX_SWIPE_ITEM_COUNT];
    private ItemButton m_focusItem;

    public GameObject[] m_buttonItem = new GameObject[MAX_SWIPE_ITEM_COUNT];
    
    private int             m_itemCount             = 1;
    public int              ItemCount
    {
        get
        {
            return m_itemCount;
        }
        set
        {
            int oldItemCount = m_itemCount;
            m_itemCount = value;            

            if (0 == m_itemCount)
            {
                ResetSwipeItems();
            }
            else if (1 == m_itemCount)
            {
                m_canSwipe = false;
            }
            else if (1 < m_itemCount)
            {   
                if (oldItemCount != m_itemCount)
                    m_canSwipe = true;

                UpdateBoundary();
            }
        }
    }

    private Vector2         m_oldPos;
    private Vector2         m_newPos;
    public float            m_swipeIntensity        = 1f;
    private bool            m_drag                  = false;
    private bool            m_canSwipe              = false;

    private TRPanel_UseItem m_useItemPanel          = null;
    public TRPanel_UseItem  UseItemPanel            { set { m_useItemPanel = value; } }

    public  bool            Jamming                 { get; private set; }

    void Start()
    {
        //m_focusItem = m_swipeItems[0];
        for (int index = 0; index < MAX_SWIPE_ITEM_COUNT; ++index)
        {
            m_swipeItems[index].FocusOffsetX = m_focusOffsetX;            
        }
    }
	
	void Update () 
    {
        if (m_canSwipe)        
            Swipe();
	}

    void ResetSwipeItems()
    {
        for (int index = 0; index < MAX_SWIPE_ITEM_COUNT; ++index)
        {
            m_swipeItems[index].HideItem();
            m_swipeItems[index].SwipeToOffset(m_focusOffsetX - (m_itemInterval * index));            
        }

        m_focusItem = null;
        //m_focusItem.Scale = 1f;
    }

    void UpdateBoundary()
    {
        float rightBoundary = m_focusOffsetX + m_itemInterval;
        float leftBoundary = m_focusOffsetX - (m_itemInterval * m_itemCount);
        m_mostLeftItemOffsetX = leftBoundary + m_itemInterval;

        for (int index = 0; index < MAX_SWIPE_ITEM_COUNT; ++index)
        {
            m_swipeItems[index].SetBoundary(leftBoundary, rightBoundary, m_mostLeftItemOffsetX);            
        }
    }

    void Swipe()
    {        
#if UNITY_IPHONE || UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);
            if (TouchPhase.Began == t.phase)
            {
                if ((t.position.x >= Screen.width - (m_itemInterval * (m_itemCount + 2))) && (t.position.y <= (m_itemInterval * 2f)))
                {
                    m_newPos = new Vector2(t.position.x, t.position.y);
                    m_drag = true;
                }
            }

            if (TouchPhase.Ended == t.phase && m_drag)
            {
                m_drag = false;

                int nearOneIndex = GetNearestOneIndex(m_focusOffsetX, true);
                float moveOffsetX = m_focusOffsetX - m_swipeItems[nearOneIndex].SwipeItemPos.x;
                ChangeFocusItem(nearOneIndex);

                for (int index = 0; index < MAX_SWIPE_ITEM_COUNT; ++index)
                {
                    if (m_swipeItems[index].gameObject.activeInHierarchy)
                        m_swipeItems[index].EndSwipe(moveOffsetX, index == nearOneIndex);
                }
            }

            if (m_drag)
            {
                Vector2 checkSwipe = new Vector2(t.position.x, t.position.y) - m_newPos;
                if (Mathf.Abs(checkSwipe.x) > 5f)
                {
                    m_oldPos = m_newPos;
                    m_newPos = new Vector2(t.position.x, t.position.y);

                    Vector2 swipe = m_newPos - m_oldPos;
                    swipe.Normalize();
                    float delta = swipe.x * m_swipeIntensity;

                    for (int index = 0; index < MAX_SWIPE_ITEM_COUNT; ++index)
                    {
                        if (m_swipeItems[index].gameObject.activeInHierarchy)
                            m_swipeItems[index].ProgressSwipe(delta);
                    }
                }
            }
        }   
#else
        if (Input.GetMouseButtonDown(0))
        {   
            m_newPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            m_drag = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            m_drag = false;

            int nearOneIndex = GetNearestOneIndex(m_focusOffsetX, true);
            float moveOffsetX = m_focusOffsetX - m_swipeItems[nearOneIndex].SwipeItemPos.x;
            ChangeFocusItem(nearOneIndex);

            for (int index = 0; index < MAX_SWIPE_ITEM_COUNT; ++index)
            {
                if (m_swipeItems[index].gameObject.activeInHierarchy)
                    m_swipeItems[index].EndSwipe(moveOffsetX, index == nearOneIndex);
            }            
        }

        if (m_drag)
        {
            Vector2 checkSwipe = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - m_newPos;
            if (Mathf.Abs(checkSwipe.x) > 0.5f)
            {
                m_oldPos = m_newPos;
                m_newPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

                Vector2 swipe = m_newPos - m_oldPos;
                swipe.Normalize();
                float delta = swipe.x * m_swipeIntensity / 5f;

                for (int index = 0; index < MAX_SWIPE_ITEM_COUNT; ++index)
                {
                    if (m_swipeItems[index].gameObject.activeInHierarchy)
                        m_swipeItems[index].ProgressSwipe(delta);
                }
            }
        }
#endif  
    }

    public void StopSwipe()
    {
        if (!m_drag)
            return;

        m_drag = false;

        int nearOneIndex = GetNearestOneIndex(m_focusOffsetX, true);
        float moveOffsetX = m_focusOffsetX - m_swipeItems[nearOneIndex].SwipeItemPos.x;
        ChangeFocusItem(nearOneIndex);        

        for (int index = 0; index < MAX_SWIPE_ITEM_COUNT; ++index)
        {
            if (m_swipeItems[index].gameObject.activeInHierarchy)
                m_swipeItems[index].EndSwipe(moveOffsetX, index == nearOneIndex);
        }
    }
    
    /*
     * @ param
     * @ aOnlyActive : true - only active swipe item, otherwise only inactive swipe item
     */ 
    int GetNearestOneIndex(float aOffsetX, bool aOnlyActive)
    {
        int nearOneIndex = 0;
        float dist = 10000000f;
        for (int index = 0; index < MAX_SWIPE_ITEM_COUNT; ++index)
        {
            if (aOnlyActive && !m_swipeItems[index].gameObject.activeInHierarchy)
                continue;

            if (!aOnlyActive && m_swipeItems[index].gameObject.activeInHierarchy)
                continue;

            float currentDist = Mathf.Abs(m_swipeItems[index].SwipeItemPos.x - aOffsetX);
            if (currentDist < dist)
            {
                dist = currentDist;
                nearOneIndex = index;
            }
        }

        return nearOneIndex;
    }

    void ChangeFocusItem(int aNewFocusItemIndex)
    {
        m_focusItem = m_swipeItems[aNewFocusItemIndex];
        m_focusItem.Scale = 1f;
        // TO DO : check this out.
        //TRStatic.GetGameManager().ChangeFocusItem(m_focusItem.KeyItemID);

        //StartCoroutine(m_focusItem.ActivateUseSprites());
        m_focusItem.UpdateJamming(Jamming);
        m_focusItem.ItemSpriteAnimation.Play("item_take");        

        int itemID = m_focusItem.KeyItemID % 100;
        m_useItemPanel.SetItemUseButton(itemID, Jamming);
    }

    public void SetFocusItem(int aKeyItemID)
    {
        if (null != m_focusItem && aKeyItemID == m_focusItem.KeyItemID)
        {   
            return;
        }
        
        /*
        if (0 < aKeyItemID && null != m_focusItem && aKeyItemID == m_focusItem.KeyItemID)
        {
            YPLog.Log("aKeyItemID = " + aKeyItemID + ", focusItemID = " + m_focusItem.KeyItemID);

            m_focusItem.SwipeToOffset(m_focusOffsetX);
            m_focusItem.Scale = 1f;
            m_useItemPanel.SetItemUseButton(m_focusItem.KeyItemID % 100, Jamming);
            TRStatic.GetGameManager().ChangeFocusItem(m_focusItem.KeyItemID);

            int itemID = m_focusItem.KeyItemID % 100;
            if (!TRStatic.GetGameManager().PlayItemManager.IsTargetableItem(itemID))
            {
                StartCoroutine(m_focusItem.ActivateUseSprites());
            }

            return;
        }
        */ 

        int focusItemIndex = -1;
        for (int index = 0; index < m_itemCount; ++index)
        {
            if (!m_swipeItems[index].gameObject.activeInHierarchy)
            {
                continue;
            }

            if (m_swipeItems[index].KeyItemID == aKeyItemID)
            {
                focusItemIndex = index;
                break;
            }
        }

        if (-1 == focusItemIndex)
        {
            focusItemIndex = GetNearestOneIndex(m_focusOffsetX, true);

            /*
             * by anemos. 2014.05.26.
             * case of firing guided missile.
             * [TO DO] Basically need to modify the "GetNearestOneIndex" function to return -1 when do not find matched item.
             */
            //if (!m_swipeItems[focusItemIndex].ItemTexture.gameObject.activeInHierarchy)
            //{
            //    YPLog.Log("TRSwipe::SetFocusItem, find one is not active! aKeyItemID = " + aKeyItemID + ", focusItemIndex = " + focusItemIndex);
            //    return;
            //}
        }

        //int nextItemIndex = GetNearestOneIndex(m_focusOffsetX, true);
        ChangeFocusItem(focusItemIndex);
        
        float moveOffsetX = m_focusOffsetX - m_focusItem.SwipeItemPos.x;
        for (int index = 0; index < m_itemCount; ++index)
        {
            if (m_swipeItems[index].gameObject.activeInHierarchy)
            {
                m_swipeItems[index].EndSwipe(moveOffsetX, index == focusItemIndex);
            }
        }
    }

    public void UseFocusItem()
    {
        m_canSwipe = false;
        m_drag = false;

        m_focusItem.UseItem();
        //m_focusItem = null;
    }

    internal void SetItem(int[] aItemKeys)
    {
        int count = aItemKeys.Length;
        for (int index = 0; index < count; ++index)
        {
            if (null != m_focusItem && aItemKeys[index] == m_focusItem.KeyItemID)
            {
                continue;
            }

            for (int sIndex = 0; sIndex < MAX_SWIPE_ITEM_COUNT; ++sIndex)
            {
                if (aItemKeys[index] == m_swipeItems[index].KeyItemID)
                {
                    continue;
                }
            }

            //float offsetX = m_focusOffsetX - (m_itemInterval * index);
            //int nearOneIndex = GetNearestOneIndex(offsetX, false);
            //m_swipeItems[nearOneIndex].SwipeToOffset(offsetX);
            m_swipeItems[index].KeyItemID = aItemKeys[index];
            m_swipeItems[index].SetItemSprite(aItemKeys[index] % 100);
            m_swipeItems[index].ShowItem();
        }
    }

    //public bool GetItemTexture(int aIndex, int aKeyItemID, out UITexture oItemTexture)
    //{
    //    if (null != m_focusItem && aKeyItemID == m_focusItem.KeyItemID)
    //    {
    //        oItemTexture = m_focusItem.ItemTexture;
    //        return false;
    //    }

    //    for (int index = 0; index < MAX_SWIPE_ITEM_COUNT; ++index)
    //    {
    //        if (aKeyItemID == m_swipeItems[index].KeyItemID)
    //        {
    //            oItemTexture = m_swipeItems[index].ItemTexture;
    //            return false;
    //        }
    //    }

    //    float offsetX = m_focusOffsetX - (m_itemInterval * aIndex);
    //    int nearOneIndex = GetNearestOneIndex(offsetX, false);

    //    m_swipeItems[nearOneIndex].SwipeToOffset(offsetX);
    //    m_swipeItems[nearOneIndex].KeyItemID = aKeyItemID;        
    //    m_swipeItems[nearOneIndex].ShowItem();
    //    m_swipeItems[nearOneIndex].SetItemSprite(aKeyItemID % 100);

    //    oItemTexture = m_swipeItems[nearOneIndex].ItemTexture;
    //    return true;
    //}

    public void StolenItem()
    {
        m_focusItem.HideItem();
    }

    internal void UpdatePendingItems(int[] aItemKeys)
    {
        List<int> have = new List<int>();
        for (int index = 0; index < aItemKeys.Length; ++index)
        {
            for (int swipeIndex = 0; swipeIndex < MAX_SWIPE_ITEM_COUNT; ++swipeIndex)
            {
                if (m_swipeItems[swipeIndex].KeyItemID == aItemKeys[index])
                {
                    have.Add(swipeIndex);
                }
            }
        }

        for (int index = 0; index < MAX_SWIPE_ITEM_COUNT; ++index)
        {
            if (!have.Contains(index))
            {
                m_swipeItems[index].HideItem();
            }
        }
    }

    internal void UpdateJamming(bool aApply)
    {
        Jamming = aApply;
        if (m_focusItem)
        {
            m_focusItem.UpdateJamming(aApply);
        }
    }
}