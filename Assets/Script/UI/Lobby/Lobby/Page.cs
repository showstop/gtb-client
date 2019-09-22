using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Page : MonoBehaviour 
{
    [SerializeField]
    private GameObject _quickMatch;

    [SerializeField]
    private GameObject _grandPrix;

    [SerializeField]
    private GameObject _garage;

    [SerializeField]
    private GameObject _medal;

    [SerializeField]
    private GameObject _ability;

    [SerializeField]
    private GameObject _shop;

    [SerializeField]    
    private List<GameObject> _pages = new List<GameObject>();

    void Awake()
    {
        ShowGameModePage();
    }

    private void ShowGameModePage()
    {
        if (Constants.GameMode.QUICK == PlayerDataRepository.Instance.CurrentGameMode)
        {
            _grandPrix.SetActive(false);
            _quickMatch.SetActive(true);
        }
        else
        {
            _quickMatch.SetActive(false);
            _grandPrix.SetActive(true);
        }
        EventManager.Instance.SendGameEvent(GameEventType.ChangeGameMode);
    }

    private void HideAllPages()
    {
        foreach (GameObject go in _pages)
        {
            go.SetActive(false);
        }
    }

    public void ChangeGameMode(int aGameMode)
    {
        PlayerDataRepository.Instance.CurrentGameMode = (Constants.GameMode)aGameMode;
        ShowGameModePage();        
    }

    public void Lobby()
    {
        HideAllPages();
        if (Constants.GameMode.QUICK == PlayerDataRepository.Instance.CurrentGameMode)
        {
            _grandPrix.SetActive(false);
            _quickMatch.SetActive(true);
        }
        else
        {
            _quickMatch.SetActive(false);
            _grandPrix.SetActive(true);
        }
    }

    public void Garage()
    {
        HideAllPages();
        _garage.SetActive(true);
    }

    public void Medal()
    {
        HideAllPages();
        _medal.SetActive(true);
    }

    public void Ability()
    {
        HideAllPages();
        _ability.SetActive(true);
    }

    public void Shop()
    {
        HideAllPages();
        _shop.SetActive(true);
    }
}