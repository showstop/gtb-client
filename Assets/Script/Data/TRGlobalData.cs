using UnityEngine;
using System.Collections;

public class TRGlobalData : MonoBehaviour 
{
    /** 싱글턴 */
    private static TRGlobalData m_Instance;
    static public TRGlobalData Instance()
    {
        return instance;
    }
    static public TRGlobalData instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = Object.FindObjectOfType(typeof(TRGlobalData)) as TRGlobalData;
                if (m_Instance == null)
                {
                    GameObject go = new GameObject("_TRGlocalData");
                    DontDestroyOnLoad(go);
                    m_Instance = go.AddComponent<TRGlobalData>();
                }
            }
            return m_Instance;
        }
    }


    public bool PlayBGM = true;					//< BGM
	public bool PlaySoundEffect = true;			//< Sound Effect
	public bool PushAlert = true;
	public bool ProfileOpen = true;
	public bool PlayFX  = true;
    public string CurrentLanguage = "Korean";
    
    private int m_cameraIndex = 0;
    public int CameraIndex
    {
        get 
        {
            m_cameraIndex = PlayerPrefs.GetInt("CameraIndex");
            return m_cameraIndex; 
        }
        set
        {
            m_cameraIndex = value;
            PlayerPrefs.SetInt("CameraIndex", m_cameraIndex);
        }
    }

    /** Awake */
    void Awake()
    {
        if (m_Instance == null)
        {
            m_Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /** Start */
    void Start()
    {
        Localization.instance.currentLanguage = CurrentLanguage;
		LoadPlayerPrefs();
    }

    
    /** PlayerPref 저장 */
    public void SavePlayerPrefs()
    {
        PlayerPrefs.SetString("Language", CurrentLanguage);
        PlayerPrefs.SetInt("BGM", (PlayBGM) ? 1 : 0);
        PlayerPrefs.SetInt("FX", (PlayFX) ? 1 : 0);
		PlayerPrefs.SetInt("PushAlert", (PushAlert) ? 1 : 0);
		PlayerPrefs.SetInt("SOUNDEFFECT", (PlaySoundEffect) ? 1 : 0);
        PlayerPrefs.SetInt("CameraIndex", CameraIndex);
    }

    /** PlayerPref 로드 */
    public void LoadPlayerPrefs()
    {
        CurrentLanguage = PlayerPrefs.GetString("Language");
        PlayBGM = (PlayerPrefs.GetInt("BGM", 1) > 0) ? true : false;
        PlayFX = (PlayerPrefs.GetInt("FX", 1) > 0) ? true : false;
		PushAlert = (PlayerPrefs.GetInt("PushAlert", 1) > 0) ? true : false;
		PlaySoundEffect = (PlayerPrefs.GetInt("SOUNDEFFECT", 1) > 0 ) ? true : false;
        m_cameraIndex = PlayerPrefs.GetInt("CameraIndex");
    }
}
