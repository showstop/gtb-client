using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ItemButton : MonoBehaviour
{
    [SerializeField]
    private Image _itemSprite;
    public Image ItemSprite { get { return _itemSprite; } }

    [SerializeField]
    private ImageSelector _itemImage;

    [SerializeField]
    private Animation _itemSpriteAnimation;
    public Animation ItemSpriteAnimation { get { return _itemSpriteAnimation; } }


    [SerializeField]
    private GameObject m_jamming;

    public int KeyItemID { get; set; }

    public Vector3 SwipeItemPos { get { return gameObject.transform.localPosition; } }

    private float m_leftBoundary;
    private float m_rightBoundary;
    private float m_mostLeftOffsetX;
    private float m_focusOffsetX;
    public float FocusOffsetX { set { m_focusOffsetX = value; } }

    public float Scale { set { gameObject.transform.localScale = new Vector3(value, value, value); } }

    public Image m_activated;
    private bool _active = false;

    public void SetBoundary(float aLeftBoundary, float aRightBoundary, float aMostLeftOffset)
    {
        m_leftBoundary = aLeftBoundary;
        m_rightBoundary = aRightBoundary;
        m_mostLeftOffsetX = aMostLeftOffset;
    }

    public void ProgressSwipe(float aOffsetX)
    {
        Vector3 swipeItemPos = gameObject.transform.localPosition;
        swipeItemPos = new Vector3(swipeItemPos.x + aOffsetX, swipeItemPos.y, swipeItemPos.z);
        if (swipeItemPos.x >= m_rightBoundary)
        {
            float delta = swipeItemPos.x - m_rightBoundary;
            swipeItemPos = new Vector3(m_mostLeftOffsetX + delta, swipeItemPos.y, swipeItemPos.z);
        }
        else if (swipeItemPos.x <= m_leftBoundary)
        {
            float delta = m_leftBoundary - swipeItemPos.x;
            swipeItemPos = new Vector3(m_focusOffsetX - delta, swipeItemPos.y, swipeItemPos.z);
        }

        gameObject.transform.localPosition = swipeItemPos;
        Scale = 0.8f;
    }

    public void EndSwipe(float aOffsetX, bool aFocus)
    {
        Vector3 swipeItemPos = gameObject.transform.localPosition;
        swipeItemPos = new Vector3(swipeItemPos.x + aOffsetX, swipeItemPos.y, swipeItemPos.z);
        if (!aFocus && (swipeItemPos.x >= m_rightBoundary || swipeItemPos.x >= m_focusOffsetX))
            swipeItemPos = new Vector3(m_mostLeftOffsetX, swipeItemPos.y, swipeItemPos.z);

        gameObject.transform.localPosition = swipeItemPos;

        if (aFocus)
        {
            Scale = 1f;
        }
        else
        {
            Scale = 0.8f;
            DeactivateUseSprites();
        }
    }

    public void SwipeToOffset(float aOffsetX)
    {
        gameObject.transform.localPosition = new Vector3(aOffsetX, gameObject.transform.localPosition.y, gameObject.transform.localPosition.z);
    }

    internal void SetItemSprite(int aItemID)
    {
        _itemImage.SetImage(aItemID);
        _itemSpriteAnimation.Play("item_stack");
    }

    public void UseItem()
    {
        //int itemID = KeyItemID % 100;
        //float applyTime = TRStatic.GetGameManager().PlayItemManager.GetItemApplyTime(itemID);
        //if (0f == applyTime)
        //{
        //    KeyItemID = -1;
        //}
    }

    public void HideItem()
    {
        Scale = 0.8f;
        _itemSprite.gameObject.SetActive(false);
        gameObject.SetActive(false);

        DeactivateUseSprites();
    }

    public void ShowItem()
    {
        Debug.LogError("ShowItem.Name --> " + gameObject.name);
        Scale = 0.8f;
        _itemSprite.gameObject.SetActive(true);
        gameObject.SetActive(true);
        ActivateUseSprites();
    }

    //internal IEnumerator ActivateUseSprites()
    //{
    //    while (_itemSpriteAnimation.isPlaying)
    //    {
    //        yield return null;
    //    }

    //    m_activated.gameObject.SetActive(true);
    //    _active = true;
    //    //_itemSprite.gameObject.GetComponent<Animation>().Play();

    //    yield return new WaitForSeconds(0.25f);

    //    //m_delayActivatedSprite.SetActive(true);
    //}

    internal void ActivateUseSprites()
    {
        _active = true;
        m_activated.gameObject.SetActive(_active);
        this.gameObject.GetComponent<Button>().enabled = _active;
    }

    private void DeactivateUseSprites()
    {
        //m_delayActivatedSprite.SetActive(false);

        //newHUD
        _active = false;
        m_activated.gameObject.SetActive(_active);
        this.gameObject.GetComponent<Button>().enabled = _active;
        //_itemSpriteAnimation.Stop();
    }

    internal void UpdateJamming(bool aApply)
    {
        m_jamming.SetActive(aApply);
        if (aApply)
        {
            DeactivateUseSprites();
        }
        else
        {
            //StartCoroutine(ActivateUseSprites());
            ActivateUseSprites();
        }
    }
}