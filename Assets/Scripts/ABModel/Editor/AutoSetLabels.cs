using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class AutoSetLabels : MonoBehaviour {

    [MenuItem("ABTools/Set AB Labels")]
    static void SetLabels() {
        AssetDatabase.RemoveUnusedAssetBundleNames();

        //E:\project\unity_proj\unity2018.2.16\ABFramework\Assets\AB_Res
        string sceneDirRoot = PathTools.GetSceneDirRoot();
        DirectoryInfo dirInfo = new DirectoryInfo(sceneDirRoot);
        DirectoryInfo[] dirInfos = dirInfo.GetDirectories();
        foreach (DirectoryInfo curDirInfo in dirInfos) {
            string sceneName = curDirInfo.Name;
            //这种写法是错误的，会得到一个不存在的目录，且不报错
            //DirectoryInfo sceneDirInfo = new DirectoryInfo (sceneName);
            JudgIsDirOrFileByRecursive(curDirInfo, sceneName);

        }

        AssetDatabase.Refresh();

        Debug.Log("标记AB包名完成！！！");

    }

    private static void JudgIsDirOrFileByRecursive(FileSystemInfo curFileSysInfo,string sceneName) {
        //Debug.Log("curFileSysInfo--->" + curFileSysInfo.FullName);
        //Debug.Log("sceneName--->" + sceneName);

        //if (curFileSysInfo == null ||!( curFileSysInfo is DirectoryInfo)) {
        //    Debug.Log("This curFileSysInfo is null   Or not DirectoryInfo");
        //    return;
        //}

        if (!curFileSysInfo.Exists) {
            Debug.Log("This "+ curFileSysInfo .FullName+ "is null !!!");
            return;
        }
        DirectoryInfo dirInfo = curFileSysInfo as DirectoryInfo;
        foreach (FileSystemInfo fileSysInfo in dirInfo.GetFileSystemInfos()) {
            //是文件
            if (fileSysInfo is FileInfo) {
                SetFileLabels(fileSysInfo, sceneName);
            }
            else {  //是目录
                JudgIsDirOrFileByRecursive(fileSysInfo, sceneName);
            }
        }
    }

    private static void SetFileLabels(FileSystemInfo fileSysInfo,string sceneName) {
        if (fileSysInfo.Extension == ".meta") {
            return;
        }
        string fileLabels = GetABName(fileSysInfo, sceneName);
        string VariantLabel = GetVariantLabel(fileSysInfo);

        //这里是要用工具类来帮我们做
        string path = fileSysInfo.FullName.Replace("\\","/");
        int index = path.IndexOf("Assets");
        path = path.Substring(index);
        //END

        AssetImporter m_AssetImporter = AssetImporter.GetAtPath(path);
        m_AssetImporter.SetAssetBundleNameAndVariant(fileLabels, VariantLabel);
    }

    private static string GetABName(FileSystemInfo fileSysInfo, string sceneName) {
        int tmpIndex = fileSysInfo.FullName.IndexOf(sceneName);
        string tmpStr = fileSysInfo.FullName.Substring(tmpIndex + sceneName.Length + 1);
        string fileLabels = null;
        if (tmpStr.Contains("\\")) {
            string[] strs = tmpStr.Split('\\');
            fileLabels = sceneName + "/" + strs[0];
        }
        else {
            fileLabels = sceneName + "/" + sceneName;
        }

        return fileLabels;
    }


    private static string GetVariantLabel(FileSystemInfo fileSysInfo) {
        string VariantLabels = null;

        if (fileSysInfo.Extension == ".unity3d") {
            VariantLabels = "u3d";
        }
        else {
            VariantLabels = "ab";
        }

        return VariantLabels;
    }
}//Class_END
