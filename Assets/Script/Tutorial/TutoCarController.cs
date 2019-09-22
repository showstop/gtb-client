using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class TutoCarController : CarController
{
    public bool _tutorialCar = false;
    public TutorialGameManager _GM;
    // Use this for initialization
    void Start ()
    {
        _GM = GameObject.FindWithTag(Constants.GAME_MANAGER_TAG_NAME).GetComponent<TutorialGameManager>();
        StartCoroutine(RegisterCarToHUD());
    }
    private void Update()
    {
        if (_GM._tutorialStart)
        {
            _matchStart = true;
        }

        if (!_matchStart)
        {
            return;
        }

        if(_tutorialCar)
        {
            UpdateSpeed();
        }
        else
        {
            UpdateLane();
            UpdateSpeed();
            UpdateJump();
            TouchSlideMove();
            MouseTouchMove();
        }
    }

    internal override void Collide(CarController aOtherCar)
    {
        base.Collide(aOtherCar);
        if (TutorialConstants.IngameState == TutorialConstants.TUTORIAL_INGAME_STATE.TUTO_INGAME_COLLISION_END)
        {
            StartCoroutine(_GM.DelayTime());
        }
    }

    protected override void ChangeLane(bool aLeft, int aLaneNO)
    {
        base.ChangeLane(aLeft, aLaneNO);
        if (aLeft)
        {

            if (TutorialConstants.IngameState == TutorialConstants.TUTORIAL_INGAME_STATE.TUTO_INGAME_LANEMOVE_LEFT_FINGER)
            {
                TutorialConstants.IngameState++;
                StartCoroutine(_GM.DelayTime());
            }
            else
            {
                return;
            }
        }
        else
        {
            if (TutorialConstants.IngameState == TutorialConstants.TUTORIAL_INGAME_STATE.TUTO_INGAME_LANEMOVE_RIGHT_FINGER)
            {
                TutorialConstants.IngameState++;
                StartCoroutine(_GM.DelayTime());
            }
            else
            {
                return;
            }
        }
    }

    internal override bool GetItem(Item aItem)
    {
        base.GetItem(aItem);
        if (TutorialConstants._tutorialPlaying)
        {
            TutorialConstants.IngameState++;
            _GM.ItemboxEnable(false);
            StartCoroutine(_GM.DelayTime());
        }
        return (_stackItems.Count == 1);
    }

    public override void CmdCollideItem(NetworkInstanceId aNetID)
    {
        TutorialConstants.IngameState++;
        TutorialConstants._tutorialAbility = true;
        StartCoroutine(_GM.DelayTime());
    }
}
