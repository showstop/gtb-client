using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WaitMatchMaking : PopupComponent
{
    [SerializeField]
    private Text _estimatedTime;
    
    [SerializeField]
    private Text _elapsedTime;

    void OnEnable()
    {
        StartCoroutine(UpdateElapsedTime());
    }

    internal void SetTime(int aSeconds)
    {
        _estimatedTime.text = StringFormat.MinuteSecond(aSeconds);
    }

    private IEnumerator UpdateElapsedTime()
    {
        int time = 0;
        while (true)
        {
            _estimatedTime.text = StringFormat.MinuteSecond(time);
            ++time;
            yield return new WaitForSeconds(1f);
        }
    }

    public void CancelSearch()
    {
        //LSConnector.Instance.MatchStopReq(PlayerDataRepository.Instance.CurrentGameMode);
    }
}