using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

//对这套框架做点修改
//只作为参考

public class PackAB  {
    [MenuItem("ABTools/PackerAB")]
    static void PackAssetBundle() {
        string outPath =PathTools. GetPackerABPath();
        //这里直接调用就好了，没必要再去做一个工具
        bool ret = EditorUtility.DisplayDialog("MyTool", "确认打包 ？", "YES", "NO");
        if (ret) {
            DeleteAB(outPath);
            if (!Directory.Exists(outPath)) {
                Directory.CreateDirectory(outPath);
            }
            BuildPipeline.BuildAssetBundles(outPath, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
            AssetDatabase.Refresh();
        }
    }



    //[MenuItem("ABTools/DeleteAB")]
    static void DeleteAB(string path) {
        //string outPath = PathTools.GetPackerABPath();
        if (Directory.Exists(path)) {
            Directory.Delete(path, true);
            AssetDatabase.Refresh();
        }
    }

}
