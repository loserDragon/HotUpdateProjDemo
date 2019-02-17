using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathTools  {

    public const string ABDIR = "AB_Res";

    /// <summary>
    /// 获取AB包标记的路径
    /// </summary>
    /// <returns></returns>
    public static string GetSceneDirRoot() {
        return Application.dataPath + "/"+ ABDIR;
    }


    public static string GetABWWWDownloadPath() {

        string strReturnWWWDownloadPath = string.Empty;
        switch (Application.platform) {

            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.WindowsEditor:
                strReturnWWWDownloadPath = "file://" + GetPackerABPath();
                break;
            case RuntimePlatform.IPhonePlayer:
                //TODO
                break;

            case RuntimePlatform.Android:
                strReturnWWWDownloadPath = "jar:file://" + GetPackerABPath();
                break;
        }

        return strReturnWWWDownloadPath;
    }

    /// <summary>
    /// 获取AB打包路径
    /// </summary>
    /// <returns></returns>
    public static string GetPackerABPath() {
        return GetPlatformPath() +"/"+ GetPlatformName();
    }

    /// <summary>
    /// AB清单文件加载路径
    /// </summary>
    /// <returns></returns>
    public static string GetABManifest() {           
        return GetPlatformPath() + "/" + GetPlatformName()+"/" + GetPlatformName();
    }



    private static string GetPlatformPath() {
        string strReturnPlatformPath = string.Empty;

        switch (Application.platform) {
            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.WindowsEditor:
                strReturnPlatformPath = Application.streamingAssetsPath;
                break;
            case RuntimePlatform.IPhonePlayer:
            case RuntimePlatform.Android:
                strReturnPlatformPath = Application.persistentDataPath;
                break;
        }

        return strReturnPlatformPath;
    }


    public static string GetPlatformName() {
        string strReturnPlatformName = string.Empty;

        switch (Application.platform) {
            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.WindowsEditor:
                strReturnPlatformName = "Windows";
                break;
            case RuntimePlatform.IPhonePlayer:
                strReturnPlatformName = "Ios";
                break;
            case RuntimePlatform.Android:
                strReturnPlatformName = "Android";
                break;
        }

        return strReturnPlatformName;
    }
}
