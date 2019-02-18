using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class CopyLuaFileToABOutPath {
    [MenuItem("ABTools/CopyLuaFileToABOutPath")]
    static void _CopyLuaFileToABOutPath() {
        string m_LuaFileEditorPath = string.Empty;
        string m_LuaFileOutPath = string.Empty;

        m_LuaFileEditorPath = Application.dataPath + "/LuaScripts";
        m_LuaFileOutPath = PathTools.GetPackerABPath()+"/LUA";

        if (!Directory.Exists(m_LuaFileOutPath)) {
            Directory.CreateDirectory(m_LuaFileOutPath);
        }

        DirectoryInfo luaFileOutDir = new DirectoryInfo(m_LuaFileEditorPath);
        FileSystemInfo[] luaFiles = luaFileOutDir.GetFileSystemInfos();
        List<string> fileList = new List<string>();
        Debug.Log(luaFiles.Length);
        foreach (FileSystemInfo item in luaFiles) {
            GetCopyFile(item, fileList);
        }
        //Debug.LogError("m_LuaFileEditorPath---->" + m_LuaFileEditorPath);
        foreach (string item in fileList) {
            string tmpItem = ChangePathChar(item);
            string tmpFilePath = tmpItem.Replace(m_LuaFileEditorPath,string.Empty);
            string tmpFileName = Path.GetFileName(tmpItem);
            string tmpFileDir = tmpFilePath.Replace(tmpFileName,string.Empty);
            if (!Directory.Exists(m_LuaFileOutPath + tmpFileDir)) {
                Directory.CreateDirectory(m_LuaFileOutPath + tmpFileDir);
            }
            File.Copy(item, m_LuaFileOutPath+ tmpFileDir+ tmpFileName,true);
        }

        AssetDatabase.Refresh();
    }

    private static void GetCopyFile(FileSystemInfo fileSysInfo, List<string> fileList) {
        if (fileSysInfo == null) {
            Debug.LogError("CopyLuaFileToABOutPath.cs/GetCopyFile(),参数fileSysInfo不能为空！");
            return;
        }
        FileInfo fileInfo = fileSysInfo as FileInfo;

        if (fileInfo != null) {
            fileList.Add(fileInfo.FullName);
        }
        else{
            DirectoryInfo dirInfo = fileSysInfo as DirectoryInfo;
            FileSystemInfo[] luaFiles = dirInfo.GetFileSystemInfos();
            foreach (FileSystemInfo item in luaFiles) {
                GetCopyFile(item, fileList);
            }
        }
    }

    private static string ChangePathChar(string path) {
        return path == null ? null : path.Replace("\\","/");
    }
}