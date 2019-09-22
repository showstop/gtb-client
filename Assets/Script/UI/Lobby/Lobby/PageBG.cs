using UnityEngine;
using System.Collections;

public class PageBG : MonoBehaviour 
{
    [SerializeField]
    private GameObject _quickMatch;

    [SerializeField]
    private GameObject _garage;

    public void ChangeGameMode()
    {   
//        if (protocol.GameMode.QUICK == PlayerDataRepository.Instance.CurrentGameMode)
//        {
//            _quickMatch.SetActive(true);
//        }
//        else
//        {
//            _quickMatch.SetActive(false);
//        }
    }

    public void Garage()
    {
        _quickMatch.SetActive(false);
        _garage.SetActive(true);
    }

    public void Lobby()
    {
//        _garage.SetActive(false);
//        if (protocol.GameMode.QUICK == PlayerDataRepository.Instance.CurrentGameMode)
//        {
//            _quickMatch.SetActive(true);
//        }
//        else
//        {
//            _quickMatch.SetActive(false);
//        }
    }
}