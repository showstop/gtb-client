using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System;

public class Builder
{
    private static readonly string playserverSceneFilePath = "Assets/Editor/Builder/PlayServerScene.txt";
    private static readonly string clientSceneFilePath = "Assets/Editor/Builder/ClientScene.txt";
    private static readonly string packageName = "GTB";
    private static readonly string bundleIdentifier = "com.YippeePlanet.GTB";

    [MenuItem("YP Tools/Builder/Android/BuildPlayServer")]
    public static void BuildPlayServerAndroid()
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
        BuildPipeline.BuildPlayer(GetScenes(BuildTarget.Android, false), GetFileName(BuildTarget.Android, false), BuildTarget.Android, BuildOptions.None);
    }

    [MenuItem("YP Tools/Builder/Android/BuildClient")]
    public static void BuildClientAndroid()
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
        BuildPipeline.BuildPlayer(GetScenes(BuildTarget.Android, true), GetFileName(BuildTarget.Android, true), BuildTarget.Android, BuildOptions.None);
    }

    [MenuItem("YP Tools/Builder/PC/BuildPlayServer")]
    public static void BuildPlayServerPC()
    {   
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneLinux64);
        BuildPipeline.BuildPlayer(GetScenes(BuildTarget.StandaloneLinux64, false), GetFileName(BuildTarget.StandaloneLinux64, false), BuildTarget.StandaloneLinux64, BuildOptions.None);
    }

    [MenuItem("YP Tools/Builder/PC/BuildClient")]
    public static void BuildClientPC()
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows64);
        BuildPipeline.BuildPlayer(GetScenes(BuildTarget.StandaloneWindows64, true), GetFileName(BuildTarget.StandaloneWindows64, true), BuildTarget.StandaloneWindows64, BuildOptions.None);
    }

    private static string GetFilePath(BuildTarget aTarget, string aPath)
    {
        if (BuildTarget.StandaloneWindows == aTarget || BuildTarget.StandaloneWindows64 == aTarget)
        {
            return Application.dataPath + "/../" + aPath;
        }

        return aPath;
    }

    private static string[] GetScenes(BuildTarget aTarget, bool aClient)
    {
        string scenePath = clientSceneFilePath;
        if (!aClient)
        {
            scenePath = playserverSceneFilePath;
        }
        scenePath = GetFilePath(aTarget, scenePath);

        List<string> scenes = new List<string>();
        string[] lines = File.ReadAllLines(scenePath);
        YPLog.Log(scenes.Count);
        foreach (string scene in lines)
        {
            YPLog.Log("scene = " + scene);
            scenes.Add(scene);
        }

        return scenes.ToArray();
    }

    private static string GetFileName(BuildTarget aTarget, bool aClient)
    {
        string cs = aClient ? "c" : "ps";
        string dt = string.Format(@"{0:yyyyMMdd}", DateTime.Now);

        string extension = "";
        switch (aTarget)
        {
            case BuildTarget.Android:
                extension = "apk";

                PlayerSettings.Android.bundleVersionCode += 1;
                PlayerSettings.applicationIdentifier = bundleIdentifier;
                PlayerSettings.bundleVersion = "1.0." + PlayerSettings.Android.bundleVersionCode + "." + dt;

                break;

            case BuildTarget.StandaloneWindows:
            case BuildTarget.StandaloneWindows64:
                extension = "exe";

                PlayerSettings.applicationIdentifier = bundleIdentifier;
                PlayerSettings.bundleVersion = "1.0." + dt;

                break;

            default:
                break;
        }

        return GetFilePath(aTarget, string.Format("{0}.{1}", packageName, extension));
    }
}