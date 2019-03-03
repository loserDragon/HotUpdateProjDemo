using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using SUIFW;
using System;
/// <summary>
/// 从服务器更新资源
/// </summary>
public class UpdateResByServer : MonoBehaviour {

    //服务器地址
    private string serverUrl = "http://127.0.0.1:6080/UpdateAssets/";

    private string downloadPath = string.Empty;

    public bool isEnableUpdate = true;

    private void Awake() {
        downloadPath = PathTools.GetPackerABPath();

        if (isEnableUpdate) {
            StartCoroutine(UpdateResAndCheckRes(serverUrl,UpdateComplete));
            Debug.Log("资源更新完毕！！！");
        }
        else {
            //不启用热更新，并通知其他模块进行初始化
            Debug.Log("开始游戏！！！");
        }
    }
    private void UpdateComplete() {
        //LuaHelper.Instance.DoString("require('LuanchABFW')");
        //LuaHelper.Instance.CallLuaFunc("LuanchABFW", "startABFW");
        ABMgr.GetInstance().LoadAssetBundlePack("mainscene", "mainscene/prefabs.ab", Complete);
        //ABMgr.GetInstance().LoadAssetBundlePack("mainscene", "mainscene/prefabs.ab", Complete);
    }
    private void Complete(string abName) {
        UnityEngine.Object obj = ABMgr.GetInstance().LoadAsset("mainscene", "mainscene/prefabs.ab", "UI_Notice.prefab", false);
        if (null == obj) {
            Debug.Log("null ");
        }
        else {
            Instantiate(obj);
        }
    }

    private IEnumerator UpdateResAndCheckRes(string serverUrl,Action UpdateComplete) {
        if (string.IsNullOrEmpty(serverUrl)) {
            Debug.LogError(GetType()+ "/UpdateResAndCheckRes()/参数serverUrl不能为空，请检查！");
            yield break;
        }
        //string downloadFilePath = serverUrl + downloadPath+ "/VerifyFile.txt";
        string verifyFilePath = serverUrl + "/VerifyFile.txt";
        Debug.Log("verifyFilePath: " + verifyFilePath);

        WWW www = new WWW(verifyFilePath);
        yield return www;
        //下载成功
        if (www.isDone && string.IsNullOrEmpty(www.error)) {
            if (!Directory.Exists(downloadPath)) {
                Directory.CreateDirectory(downloadPath);
            }
            File.WriteAllBytes(downloadPath+ "/VerifyFile.txt", www.bytes);
            string[] files = www.text.Split('\n');
            for (int i = 0; i < files.Length; i++) {
                if (string.IsNullOrEmpty(files[i])) {
                    continue;
                }
                string[] lineFiles = files[i].Split('|');
                string filePath = lineFiles[0].Trim();
                string MD5Value = lineFiles[1].Trim();

                string localFilePath = downloadPath + "/" + filePath;
                //Debug.Log("localFilePath->" + localFilePath);
                if (!File.Exists(localFilePath)) {
                    string dirPath = Path.GetDirectoryName(localFilePath);
                    if (!string.IsNullOrEmpty(dirPath)) {
                        Directory.CreateDirectory(dirPath);
                    }
                    //加载文件并写入
                    Debug.Log(GetType()+ "/UpdateResAndCheckRes()/资源不存在从服务器下载，filePath： "+ filePath);
                    yield return LoadNewFile(serverUrl + "/" + filePath, localFilePath);
                }
                else {
                    string md5Value = CommonFunc.GenerateMD5Str(filePath);
                    if (!MD5Value.Equals(md5Value)) {
                        File.Delete(localFilePath);
                        Debug.Log(GetType() + "/UpdateResAndCheckRes()/资源与服务器不一致，下载新的资源，filePath： " + filePath);
                        //加载文件并写入
                        yield return LoadNewFile(serverUrl+"/"+ filePath, localFilePath);
                    }
                }
            }

            yield return new WaitForEndOfFrame();
            if (UpdateComplete != null) {
                UpdateComplete();
            }
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
        }
        else {
            Debug.LogError(GetType() + "/UpdateResAndCheckRes()/下载出错，请检查！错误： "+www.error);
            yield break;
        }

    }

    private IEnumerator LoadNewFile(string serverUrl,string filePath) {
        WWW _www = new WWW(serverUrl);
        yield return _www;
        if (_www.isDone && string.IsNullOrEmpty(_www.error)) {
            Debug.Log("filePath-->" + filePath);
            File.WriteAllBytes(filePath, _www.bytes);
        }
        else {
            Debug.LogError(GetType() + "/LoadNewFile()/下载出错，请检查！错误： " + _www.error);
            yield break;
        }
    }

}
