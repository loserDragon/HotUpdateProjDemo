using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using SUIFW;
using System.Collections.Generic;

public class CreateVerifyFile {
    [MenuItem("ABTools/CreateVerifyFile")]
    public static void GenerateVerifyFile() {
        string m_ABOutPath = PathTools.GetPackerABPath();
        Debug.Log("abOutPath=" + m_ABOutPath);
        string m_VerifyFileOutPath = m_ABOutPath + "/VerifyFile.txt";
        Debug.Log("m_VerifyFileOutPath=" + m_VerifyFileOutPath);
        DirectoryInfo dirInfo = new DirectoryInfo(m_ABOutPath);
        List<string> m_ListFiles = new List<string>();
        ErgodicABOutPath(dirInfo, m_ListFiles);


    }

    private static void ErgodicABOutPath(FileSystemInfo fileSysInfo,List<string> fileList){
        
    }
}
