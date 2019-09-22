using UnityEngine;
using System.Collections.Generic;

public class LobbySound : MonoBehaviour
{
    public List<AudioClip> BGMClips = new List<AudioClip>();
    
    [SerializeField]
    private AudioSource source_;

    private void Start()
    {
        var selectedIdx = Random.Range(0, BGMClips.Count - 1);
        Debug.LogFormat("Selected BGM:{0}", selectedIdx);
        source_.clip = BGMClips[selectedIdx];
        source_.Play();
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        if (Application.loadedLevelName.StartsWith("scene"))
        {
            source_.Stop();
        }
    }
}
