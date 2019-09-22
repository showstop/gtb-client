using UnityEngine;
using System.Collections;

public class CarSound : MonoBehaviour 
{
    [SerializeField]
    private AudioSource _as;

    [SerializeField]
    private AudioClip _collideSound;

    [SerializeField]
    private AudioClip _runningSound;

    [SerializeField]
    private AudioClip _landingSound;

    [SerializeField]
    private AudioClip _itemGetSound;

    [SerializeField]
    private AudioClip _firstRankSound;

    [SerializeField]
    private AudioClip _middleRankSound;

    [SerializeField]
    private AudioClip _lastRankSound;

    [SerializeField]
    private AudioClip _scoreIncreaseSound;

    [SerializeField]
    private AudioClip _countDownSound;

    [SerializeField]
    private AudioClip _gameStartSound;

    internal void PlaySoundOneShot(AudioClip aClip)
    {
        if (!TRGlobalData.instance.PlaySoundEffect)
        {
            return;
        }

        _as.PlayOneShot(aClip);
    }

    internal void PlayRunningSound()
    {
        if (!TRGlobalData.instance.PlaySoundEffect)
        {
            return;
        }

        _as.clip = _runningSound;
        _as.loop = true;
        _as.Play();
    }

    internal void StopRunningSound()
    {
        _as.Stop();
    }

    internal void PlayCollideSound()
    {
        PlaySoundOneShot(_collideSound);
    }

    internal void PlayLandingSound()
    {
        PlaySoundOneShot(_landingSound);
    }

    internal void PlayItemGetSound()
    {
        PlaySoundOneShot(_itemGetSound);
    }

    internal void PlayFinalRankSound(int aRank)
    {
        AudioClip clip = null;
        switch (aRank)
        {
            case Constants.RANK_FIRST:
                clip = _firstRankSound;
                break;

            case Constants.RANK_SECOND:
            case 3:
                clip = _middleRankSound;
                break;

            case Constants.MAX_PLAYER_NUM:
                clip = _lastRankSound;
                break;

            default:
                break;
        }

        PlaySoundOneShot(clip);
    }

    internal void PlayScoreIncreaseSound()
    {
        PlaySoundOneShot(_scoreIncreaseSound);
    }

    internal void PlayCountDownSound(bool aGameStart)
    {
        if (aGameStart)
        {
            PlaySoundOneShot(_gameStartSound);
        }
        else
        {
            PlaySoundOneShot(_countDownSound);
        }
    }
}
