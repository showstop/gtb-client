using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class AssetBundleLoader : Singleton<AssetBundleLoader>
{
    private VehicleDB _vehicleDB = null;    
    private AssetBundleManifest _bundleManifest;

    private Dictionary<int, VehicleResourceUnit> _vehicleResources = new Dictionary<int, VehicleResourceUnit>();
    private Dictionary<int, SpecialAbilityData> _specialAbilityDB = new Dictionary<int, SpecialAbilityData>();

    private static readonly string dbAssetFileNameFormat = "db/{0}.dbasset";
    private static readonly string vehicleDBName = "vehicles";
    private static readonly string specialAbilityDBName = "specialability";

    //void Start()
    //{
    //    StartCoroutine(LoadAssetBundle());
    //}

    public bool ReadyToUse
    {
        get
        {
            if (_vehicleDB == null)
                return false;

            if (_vehicleResources.Count < _vehicleDB.param.Count)
                return false;

            return true;
        }
    }

    public VehicleResourceUnit GetVehicleResourceUnit(int aVehicleNo)
    {
        if (!_vehicleResources.ContainsKey(aVehicleNo))
            return null;

        return _vehicleResources[aVehicleNo];
    }

    public IEnumerator LoadAssetBundle(GameObject aAbilityDB)
    {
        YPLog.Trace();
        while (!Caching.ready)
        {
            yield return null;
        }
        
        yield return new WaitForSeconds(1f);
        StartCoroutine(DownloadAssetBundleManifest());
        while (!_bundleManifest)
        {
            yield return null;
        }

        // by anemos
        // TO DO : fix me later
        StartCoroutine(DownLoadVehicleDB());
        StartCoroutine(DownSpecialAbilityDB());
        while (!_vehicleDB || 0 == _specialAbilityDB.Count)
        {
            yield return null;
        }

        StartCoroutine(DownloadVehicleAssetBundle());

        while (_vehicleDB.param.Count > _vehicleResources.Count)
        {
            yield return null;
        }        

        // TO DO : loading client data..
        EventManager.Instance.SendGameEvent(GameEventType.LoadingClientData, 0f);
        AbilityDatabase.Instance.Load(aAbilityDB);
    }

    private IEnumerator DownloadAssetBundleManifest()
    {
        string bundleManifestUrl = AssetConfig.AssetBundleManifest();
        Debug.Log(bundleManifestUrl);
        WWW www = new WWW(bundleManifestUrl);
        yield return www;

        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.Log(www.error);
            yield break;
        }

        AssetBundle manifest = www.assetBundle;

        www.Dispose();
        _bundleManifest = manifest.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
    }

    private IEnumerator DownLoadVehicleDB()
    {
        var fileName = string.Format(dbAssetFileNameFormat, vehicleDBName);
        //var hash = _bundleManifest.GetAssetBundleHash(fileName);

        using (WWW www = new WWW(AssetConfig.AssetBundleLoadPath(fileName, true)))
        {
            yield return www;
            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.Log(www.error);
                yield break;
            }

            AssetBundle bundle = www.assetBundle;
            _vehicleDB = bundle.LoadAsset<VehicleDB>(bundle.GetAllAssetNames()[0]);
            bundle.Unload(false);
        }
    }

    private IEnumerator DownSpecialAbilityDB()
    {
        var fileName = string.Format(dbAssetFileNameFormat, specialAbilityDBName);
        //var hash = _bundleManifest.GetAssetBundleHash(fileName);
        var uri = AssetConfig.AssetBundleLoadPath(fileName, true);
        Debug.Log("Assetbundle uri : " + uri);
        using (WWW www = new WWW(AssetConfig.AssetBundleLoadPath(fileName, true)))
        {
            yield return www;
            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.Log(www.error);
                yield break;
            }

            AssetBundle bundle = www.assetBundle;
            SpecialAbilityDB db = bundle.LoadAsset<SpecialAbilityDB>(bundle.GetAllAssetNames()[0]);
            for(int index = 0; index < db._data.Count; ++index)
            {
                if(!_specialAbilityDB.ContainsKey(db._data[index]._id))
                {
                    _specialAbilityDB.Add(db._data[index]._id, db._data[index]);
                }
            }
            bundle.Unload(false);
        }
    }

    private IEnumerator DownloadVehicleAssetBundle()
    {
        for (int index = 0; index < _vehicleDB.param.Count; ++index)
        {            
            VehicleDB.Param item = _vehicleDB.param[index];            

            List<string> assetList = new List<string>();
            assetList.Add(item.BodyModel + AssetConfig.AssetBundleExtension);
            assetList.Add(item.TireModel + AssetConfig.AssetBundleExtension);
            assetList.Add(item.BodyMaterial + AssetConfig.AssetBundleExtension);
            assetList.Add(item.TireMaterial + AssetConfig.AssetBundleExtension);
            if (item.WindowMat != "")
                assetList.Add(item.WindowMat + AssetConfig.AssetBundleExtension);
            if (item.MetalMat != "")
                assetList.Add(item.MetalMat + AssetConfig.AssetBundleExtension);

            VehicleResourceUnit vr = new VehicleResourceUnit();            
            vr.Name = item.Name;
            for (int i = 0; i < assetList.Count; i++)
            {
                var fileName = assetList[i];
                var hash = _bundleManifest.GetAssetBundleHash(fileName);
                using (WWW www = WWW.LoadFromCacheOrDownload(AssetConfig.AssetBundleLoadPath(fileName, true), hash))
                {
                    float percentage = (float)i / (float)assetList.Count;
                    EventManager.Instance.SendGameEvent(GameEventType.DownloadUpdate, percentage);

                    yield return www;
                    if (!string.IsNullOrEmpty(www.error))
                    {
                        Debug.Log(www.error);
                        yield break;
                    }
                    AssetBundle bundle = www.assetBundle;
                    if (fileName.Contains("model"))
                    {
                        if (fileName.Contains("body"))
                            vr.BodyModel = bundle.LoadAsset<GameObject>(bundle.GetAllAssetNames()[0]);
                        else if (fileName.Contains("tire"))
                            vr.TireModel = bundle.LoadAsset<GameObject>(bundle.GetAllAssetNames()[0]);
                    }
                    else if (fileName.Contains("material"))
                    {
                        if (fileName.Contains("body"))
                        {
                            vr.BodyMaterial = vr.TireMaterial = bundle.LoadAsset<Material>(bundle.GetAllAssetNames()[0]);
                        }
                        else if (fileName.Contains("tire"))
                        {
                            vr.TireMaterial = bundle.LoadAsset<Material>(bundle.GetAllAssetNames()[0]);
                        }
                        else if (fileName.Contains("window"))
                        {
                            vr.WindowMaterial = bundle.LoadAsset<Material>(bundle.GetAllAssetNames()[0]);                                
                        }
                        else if (fileName.Contains("metal"))
                        {
                            vr.MetalMaterial = bundle.LoadAsset<Material>(bundle.GetAllAssetNames()[0]);                                
                        }
                    }
                    bundle.Unload(false);
                }
            }
            YPLog.Log("======= [DownloadVehicleAssetBundle] id = " + item.ID + ", bodyM = " + vr.BodyMaterial + ", tireM = " + vr.TireMaterial + ", windowM = " + vr.WindowMaterial + ", metalM = " + vr.MetalMaterial);
            if (!_vehicleResources.ContainsKey(item.ID))
            {
                _vehicleResources.Add(item.ID, vr);
            }
        }
    }

    public static void Copy(string sourceDir, string targetDir)
    {
        if (!Directory.Exists(targetDir))
        {
            Directory.CreateDirectory(targetDir);
        }

        foreach (string sourcePath in Directory.GetFiles(sourceDir))
        {
            string targetPath = Path.Combine(targetDir, Path.GetFileName(sourcePath));
            YPLog.Log("target  ::  " + targetPath);
            bool overwrite = File.Exists(targetPath);
            File.Copy(sourcePath, targetPath, overwrite);
        }

        foreach (string directory in Directory.GetDirectories(sourceDir))
        {
            Copy(directory, Path.Combine(targetDir, Path.GetFileName(directory)));
        }
    }

    internal VehicleDB.Param GetVehicleData(int aID)
    {
        for (int index = 0; index < _vehicleDB.param.Count; ++index)
        {
            if (aID == _vehicleDB.param[index].ID)
                return _vehicleDB.param[index];
        }

        return null;
    }

    internal SpecialAbilityData GetSpecialAbilityData(int aID)
    {
        if (!_specialAbilityDB.ContainsKey(aID))
        {
            YPLog.LogError("Can't find special ability data with id[=" + aID + "]");
            return null;
        }

        return _specialAbilityDB[aID];
    }

    internal int GetSpecialAbilityLevelUpPrice(int aID, int aLevel)
    {
        if (!_specialAbilityDB.ContainsKey(aID))
        {
            YPLog.LogError("Can't find special ability data with id[=" + aID + "]");
            return -1;
        }

        return _specialAbilityDB[aID]._levelUpPrice[aLevel - 1];
    }

    internal int GetRandomCarID()
    {
        int random = Random.Range(0, _vehicleDB.param.Count);
        return _vehicleDB.param[random].ID;
    }
}