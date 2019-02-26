using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using SUIFW;
/// <summary>
/// 从服务器更新资源
/// </summary>
public class UpdateResByServer : MonoBehaviour {

    //服务器地址
    private string serverUrl = "http://127.0.0.1:8080/UpdateAssets/";

    private string downloadPath = string.Empty;

    private bool isEnableUpdate = true;

    private void Awake() {
        downloadPath = PathTools.GetPlatformName();

        if (isEnableUpdate) {
            StartCoroutine(UpdateResAndCheckRes(serverUrl));
        }
        else {
            //不启用热更新，并通知其他模块进行初始化
        }
    }

    private IEnumerator UpdateResAndCheckRes(string serverUrl) {
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
            string[] files = www.error.Split('\n');
            for (int i = 0; i < files.Length; i++) {
                if (string.IsNullOrEmpty(files[i])) {
                    continue;
                }
                string[] lineFiles = files[i].Split('|');
                string filePath = lineFiles[0].Trim();
                string MD5Value = lineFiles[1].Trim();

                string localFilePath = downloadPath + "/" + filePath;
                if (!File.Exists(localFilePath)) {
                    string dirPath = Path.GetDirectoryName(localFilePath);
                    if (!string.IsNullOrEmpty(dirPath)) {
                        Directory.CreateDirectory(dirPath);
                    }
                    //加载文件并写入
                    LoadNewFile(serverUrl + "/" + filePath, localFilePath);
                }
                else {
                    string md5Value = CommonFunc.GenerateMD5Str(filePath);
                    if (!MD5Value.Equals(md5Value)) {
                        File.Delete(localFilePath);
                        //加载文件并写入
                        LoadNewFile(serverUrl+"/"+ filePath, localFilePath);
                    }
                }
            }

            yield return new WaitForEndOfFrame();
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
            File.WriteAllBytes(filePath, _www.bytes);
        }
        else {
            Debug.LogError(GetType() + "/LoadNewFile()/下载出错，请检查！错误： " + _www.error);
            yield break;
        }
    }

}
