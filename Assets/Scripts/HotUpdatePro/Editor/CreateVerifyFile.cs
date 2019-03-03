using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using SUIFW;
using System.Collections.Generic;

public class CreateVerifyFile {
    static string m_ABOutPath;
    [MenuItem("ABTools/CreateVerifyFile")]
    public static void GenerateVerifyFile() {
        m_ABOutPath = PathTools.GetPackerABPath().Replace("\\","/");

        string m_VerifyFileOutPath = m_ABOutPath + "/VerifyFile.txt";
        if (File.Exists(m_VerifyFileOutPath)) {
            File.Delete(m_VerifyFileOutPath);
        }

        DirectoryInfo dirInfo = new DirectoryInfo(m_ABOutPath);
        List<string> m_ListFiles = new List<string>();
        ErgodicABOutPath(dirInfo, m_ListFiles);

        WriteVerifyFile(m_VerifyFileOutPath, m_ListFiles);
        Debug.Log("创建校验文件成功！！！");
        AssetDatabase.Refresh();
    }

    private static void ErgodicABOutPath(FileSystemInfo fileSysInfo,List<string> fileList){
        if (fileSysInfo == null ) {
            Debug.LogError("CreateVerifyFile.cs/ErgodicABOutPath()/参数fileSysInfo不能为空");
            return;
        }

        DirectoryInfo m_DirInfo = fileSysInfo as DirectoryInfo;
        FileSystemInfo[] m_FileSysInfo =  m_DirInfo.GetFileSystemInfos();
        foreach (FileSystemInfo item_FileSystemInfo in m_FileSysInfo) {
            FileInfo m_FileInfo = item_FileSystemInfo as FileInfo;
            if (m_FileInfo !=null) {
                string strFileName = m_FileInfo.FullName.Replace("\\", "/");
                string strFileExtension = m_FileInfo.Extension;
                if (strFileExtension == ".meta"|| strFileExtension == ".bak") {
                    continue;
                }
                string abOutPath = PathTools.GetPackerABPath();
                string tmpStrFileName = strFileName.Substring(abOutPath.Length+1);
                if (fileList.Contains(tmpStrFileName)) {
                    Debug.LogError("CreateVerifyFile.cs/ErgodicABOutPath()/有重名的资源，请检查！tmpStrFileName="+ tmpStrFileName);
                    continue;
                }
                fileList.Add(tmpStrFileName);
            }
            else {
                ErgodicABOutPath(item_FileSystemInfo, fileList);
            }
        }
        
    }

    private static void WriteVerifyFile(string verifyFileOutPath,List<string> fileList) {
        if (string.IsNullOrEmpty(verifyFileOutPath) ) {
            Debug.LogError("CreateVerifyFile.cs/WriteVerifyFile()/参数verifyFileOutPath不能为空！");
            return;
        }
        if (fileList == null || fileList.Count<=0) {
            Debug.LogError("CreateVerifyFile.cs/WriteVerifyFile()/参数fileList不能为空list！");
            return;
        }

        using (FileStream fs = new FileStream(verifyFileOutPath,FileMode.CreateNew)) {
            using (StreamWriter sw = new StreamWriter(fs)) {
                foreach (string item in fileList) {
                    if (string.IsNullOrEmpty(item)) {
                        continue;
                    }
                    Debug.Log("--->"+item);
                    Debug.Log("--->" + m_ABOutPath + "/" + item);
                    string strMD5 = CommonFunc.GenerateMD5StrByStream(m_ABOutPath+"/"+item);
                    sw.WriteLine(item+"|"+strMD5);
                }
            }
        }
    }


}
