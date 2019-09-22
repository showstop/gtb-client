using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using SmartLocalization;

public class TutorialGameManager : GameManager
{
    public GameObject _tutoCar;
    public TutoCarController[] _tutorialCars = new TutoCarController[4];

    private float _startTime = 2.0f;
    private int _updateEnd = 0;
    public bool _tutorialStart = false;

    void Start ()
    {
        StartCoroutine(IngameStart());
    }
	
	void Update ()
    {
        if(_updateEnd > 1)
        {
            return;
        }

        if(_countDown > 3)
        {
            ItemboxEnable(false);
        }
        else if(_countDown == 0)
        {
            _tutorialStart = true;
            TutorialManager.Instance.IngameInit();
            _updateEnd++;
        }
    }

    internal void ItemboxEnable(bool aShow)
    {
        foreach (var a in _itemBoxSpawners)
        {
            a.BoxEnable(aShow);
        }
    }

    private IEnumerator IngameStart()
    {
        yield return new WaitForSeconds(2.0f);
        _playerWaitTime = 0.25f;
        StartHost();
    }
}
