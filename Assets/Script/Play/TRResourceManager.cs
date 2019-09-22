using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TRResourceManager : MonoBehaviour
{
    private static Dictionary<string, Object> m_cache = new Dictionary<string, Object>();

    private static void Load(string aPath)
    {
        Object obj = Resources.Load(aPath) as Object;
        m_cache[aPath] = obj;

        //YPLog.Log("TRResourceManager - Load, aPath = " + aPath + ", name = " + obj.name);
    }

    internal static T Get<T>(string aPath) where T : Object
    {
        if (!m_cache.ContainsKey(aPath))
            Load(aPath);

        return m_cache[aPath] as T;
    }

    /** 아이템 이미지 얻어오기 */
    internal static Texture GetItemImage(int imageID)
    {
        string path = "Icon/" + imageID.ToString();
        return Get<Texture>(path);
    }

    internal static Texture GetItemImage(string id)
    {
        string path = "Icon/" + id;
        return Get<Texture>(path);
    }

    internal static Texture GetAbilityImage(int aAbilityID)
    {
        int abilityID = TRStatic.GetAbilityBaseNo(aAbilityID);
        string path = "Icon/" + abilityID.ToString();
        return Get<Texture>(path);
    }

    internal static GameObject Instantiate(string aPath)
    {   
        if (!m_cache.ContainsKey(aPath))
            Load(aPath);

        return GameObject.Instantiate(m_cache[aPath], Vector3.zero, Quaternion.identity) as GameObject;
    }

    internal static void RemoveAll()
    {
        m_cache.Clear();
    }
}