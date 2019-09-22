using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using SimpleJSON;

/** TR°ÔÀÓ¿¡Œ­ »ç¿ëµÇŽÂ Data žðÀœÁý */
public class TRDataManager : MonoBehaviour
{
    /** œÌ±ÛÅÏ */
    private static TRDataManager    m_Instance;  
    static public TRDataManager Instance()
    {
        return instance;
    }
    static public TRDataManager instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = Object.FindObjectOfType(typeof(TRDataManager)) as TRDataManager;
                if (m_Instance == null)
                {
                    GameObject go = new GameObject("_TRDataManager");
                    DontDestroyOnLoad(go);
                    m_Instance = go.AddComponent<TRDataManager>();
                }
            }
            return m_Instance;
        }
    }
}