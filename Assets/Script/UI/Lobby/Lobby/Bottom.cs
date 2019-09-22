using UnityEngine;
using System.Collections;
using Network;

public class Bottom : MonoBehaviour
{
    public void StartGame()
    {
        LobbyRequest.Instance.MatchInfoReq(PlayerDataRepository.Instance.CurrentGameMode);
    }
}