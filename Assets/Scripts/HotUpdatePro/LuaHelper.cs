using System.Collections;
using System.Collections.Generic;
using System.IO;
using XLua;

public class LuaHelper {
    private static LuaHelper instance;

    public static LuaHelper Instance {

        get{
            if (null == instance) {
                instance = new LuaHelper();     
            }
            return instance;
        }
    }

    private LuaEnv _LuaEvn;

    private Dictionary<string, byte[]> _DicCacheLua = new Dictionary<string, byte[]>();

    private LuaHelper() {
        _DicCacheLua = new Dictionary<string, byte[]>();
        _LuaEvn.AddLoader (CustomLoaderByName);
    }

    public void DoString(string chunk, string chunkName = "chunk", LuaTable env = null) {
        _LuaEvn.DoString(chunk, chunkName, env);
    }

    public object[] CallLuaFunc(string lusScriptsName,string luaMethodName,params object[] args) {
        LuaTable _luaTable = _LuaEvn.Global.Get<LuaTable>(lusScriptsName);
        LuaFunction _luaFunc = _luaTable.Get<LuaFunction>(luaMethodName);
        return _luaFunc.Call(args);
    }

    private byte[] CustomLoaderByName(ref string luaName) {
        if (_DicCacheLua != null && _DicCacheLua.ContainsKey(luaName)) {
            return _DicCacheLua[luaName];
        }
        else {
            string luaPath = PathTools.GetPackerABPath() + "/LUA";
            return ProcessLuaDir(new DirectoryInfo(luaPath), luaName);
        }
    }


    private byte[] ProcessLuaDir(FileSystemInfo fileSysInfo,string luaFileName) {
        DirectoryInfo _DirInfo = fileSysInfo as DirectoryInfo;
        FileSystemInfo[] _fileSysInfos =  _DirInfo.GetFileSystemInfos();
        for (int i = 0; i < _fileSysInfos.Length; i++) {
            FileInfo _fileInfo = _fileSysInfos[i] as FileInfo;
            if (null == _fileInfo) {
                ProcessLuaDir(_fileSysInfos[i], luaFileName);
            }
            else {
                string curfileName = Path.GetFileNameWithoutExtension(_fileInfo.Name);
                if (_fileInfo.Extension== "meta"|| curfileName != luaFileName) {
                    continue;
                }
                if (_DicCacheLua == null) {
                    _DicCacheLua = new Dictionary<string, byte[]>();
                }
                if (_DicCacheLua.ContainsKey(luaFileName)) {
                    UnityEngine.Debug.LogError(GetType()+ "/ProcessLuaDir（）/存在相同名字的文件，luaFileName： "+ luaFileName);
                    continue;
                }
                byte[] ret = File.ReadAllBytes(_fileInfo.FullName);
                _DicCacheLua.Add(luaFileName, ret);
                return ret;
            }
        }

        return null;
    }
}
