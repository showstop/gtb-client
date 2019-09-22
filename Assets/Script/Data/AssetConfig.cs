using UnityEngine;

public static class AssetConfig
{
    public static readonly string AssetBundleServerURL = "http://gtb-asset.s3-website.ap-northeast-2.amazonaws.com";

    public static readonly string AndroidBundleManifest = "Android";
    public static readonly string WindowsBundleManifest = "Windows";

    public static readonly string StandAlonePlayerExtension = "exe";
    public static readonly string AndroidPlayerExtension = "apk";
    public static readonly string iOSPlayerExtension = "ipa";

    public static readonly string StandAloneAssetExtension = ".asset";
    public static readonly string AndroidAssetExtension = ".asset";
    public static readonly string iOSAssetExtension = ".asset";

    public static string rootPath = string.Empty;
    public static bool useWWW = false;

    public static string AssetBundleUrl()
    {
        return AssetBundleServerURL + "/" + AssetBundleBuildPath();
    }

    public static string AssetBundleManifest()
    {
        string ret = AssetBundleUrl();
        string manifest = "";
        Debug.Log("platform = " + Application.platform);
        
        switch (Application.platform)
        {            
            case RuntimePlatform.WindowsPlayer:     // window standalone
                {
                    manifest = WindowsBundleManifest;
                    break;
                }

            case RuntimePlatform.WindowsEditor:
            case RuntimePlatform.OSXEditor:
            case RuntimePlatform.Android:           // android
                {
                    manifest = AndroidBundleManifest;
                    break;
                }
        }

        YPLog.Log("ret = " + ret + ", manifest = " + manifest);

        return ret + "/" + manifest;
    }

    public static string AssetBundleLoadPath(string aName, bool fromWWW=false)
    {
        if(!fromWWW)
            return Application.streamingAssetsPath + "/" + AssetBundles.Utility.GetPlatformName() + "/" + aName;

        return AssetBundleUrl() + "/" + aName;
    }

    public static string AssetBundleBuildPath(string aName="")
    {
        if(aName == "")
            return AssetBundles.Utility.AssetBundlesOutputPath + "/" + AssetBundles.Utility.GetPlatformName();

        return AssetBundles.Utility.AssetBundlesOutputPath + "/" + AssetBundles.Utility.GetPlatformName() + "/" + aName;
    }

    public static string AssetBundleExtension
    {
        get
        {
            if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.OSXPlayer)
            {
                return AssetConfig.StandAloneAssetExtension;
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
                return AssetConfig.AndroidAssetExtension;
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                return AssetConfig.iOSAssetExtension;
            }
            else
            {
//#if UNITY_EDITOR
                return AssetConfig.StandAloneAssetExtension;
//#endif // UNITY_EDITOR
            }
        }
    }
    
}