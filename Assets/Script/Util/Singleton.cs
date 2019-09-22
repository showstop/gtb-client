using UnityEngine;
using System.Collections;

/// <summary>
/// Be aware this will not prevent a non singleton constructor
///   such as `T myT = new T();`
/// To prevent that, add `protected T () {}` to your singleton class.
/// 
/// As a note, this is made as MonoBehaviour because we need Coroutines.
/// </summary>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T      _instance;
    private static object   _lock = new object();

    public static T Instance
    {
        get
        {
            if (_applicationIsQuitting)
            {
                YPLog.LogWarning("SingleTon, Instance '" + typeof(T) + "' already destroyed on application quit." + " Won't create again - returning null.");
                return null;
            }

            lock (_lock)
            {
                if (null == _instance)
                {
                    _instance = (T)FindObjectOfType(typeof(T));
                    if (1 < FindObjectsOfType(typeof(T)).Length)
                    {
                        YPLog.LogError("SingleTon, Something went really wrong " + " - there should never be more than 1 singleton!" + " Reopenning the scene might fix it.");
                        return _instance;
                    }

                    if (null == _instance)
                    {
                        GameObject singleton = new GameObject();
                        _instance = singleton.AddComponent<T>();
                        singleton.name = "(singleton) " + typeof(T).ToString();

                        DontDestroyOnLoad(singleton);

                        YPLog.Log("SingleTon, An instance of " + typeof(T) + " is needed in the scene, so '" + singleton + "' was created with DontDestroyOnLoad.");
                    }
                    else
                    {
                        YPLog.Log("SingleTon, Using instance already created: " + _instance.gameObject.name);
                    }
                }

                return _instance;
            }
        }
    }

    private static bool _applicationIsQuitting = false;
    /// <summary>
    /// When Unity quits, it destroys objects in a random order.
    /// In principle, a Singleton is only destroyed when application quits.
    /// If any script calls Instance after it have been destroyed, 
    ///   it will create a buggy ghost object that will stay on the Editor scene
    ///   even after stopping playing the Application. Really bad!
    /// So, this was made to be sure we're not creating that buggy ghost object.
    /// </summary>
    public void OnApplicationQuit()
    {
        _applicationIsQuitting = true;
    }
}
