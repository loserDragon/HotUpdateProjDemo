using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace SUIFW{
    public delegate void DelABLoadComplete(string ABName);
    public class SingleABLoader : System.IDisposable {

        private AssetLoader m_AssetLoader;
        private DelABLoadComplete m_DelABLoadComplete;
        private string m_ABName;

        private string m_ABDownloadPath;


        public SingleABLoader(string ABName, DelABLoadComplete m_DelABLoadComplete) {
            if (ABName == null) {
                Debug.LogError(GetType()+ "/构造函数/参数ABName为null，请检查！");
                return;
            }
            this.m_ABName = ABName;
            this.m_ABDownloadPath = PathTools.GetABWWWDownloadPath()+"/"+ this.m_ABName;
            this.m_DelABLoadComplete = m_DelABLoadComplete;
            //Debug.Log(this.m_ABDownloadPath);
        }

        public IEnumerator LoadSingleAB() {
            using (WWW m_www = new WWW(this.m_ABDownloadPath)) {
                yield return m_www;
                if (m_www.progress >= 1) {
                    m_AssetLoader = new AssetLoader(m_www.assetBundle);
                    if (this.m_DelABLoadComplete!=null) {
                        this.m_DelABLoadComplete(this.m_ABName);
                    }
                }
                else{
                    Debug.Log(GetType()+ "/LoadSingleAB()/www下载失败，请检查！下载路径this.m_ABDownloadPath："+ this.m_ABDownloadPath);
                }
            }
        }

        public UnityEngine.Object LoadAsset(string assetName,bool isCache) {
            if (m_AssetLoader!=null) {
               return m_AssetLoader.LoadAsset(assetName, isCache);
            }
            Debug.Log(GetType()+ "/LoadAsset()/参数m_AssetLoader为空，请检查！");
            return null;
        }

        public void UnLoadAsset(UnityEngine.Object assetObj) {
            if (m_AssetLoader != null) {
                m_AssetLoader.UnLoadAsset(assetObj);
                m_AssetLoader = null;
            }
            else {
                Debug.Log(GetType()+ "/UnLoadAsset()/字段m_AssetLoader为空，请检查！");
            }
        }

        public void Dispose() {
            if (m_AssetLoader != null) {
                m_AssetLoader.Dispose();
                m_AssetLoader = null;
            }
            else {
                Debug.Log(GetType() + "/Dispose()/字段m_AssetLoader为空，请检查！");
            }
        }

        public void DisposeAll() {
            if (m_AssetLoader != null) {
                m_AssetLoader.DisposeAll();
                m_AssetLoader = null;
            }
            else {
                Debug.Log(GetType() + "/DisposeAll()/字段m_AssetLoader为空，请检查！");
            }
        }

        public string[] RetrivalAllAssetName() {
            if (m_AssetLoader != null) {
               return m_AssetLoader.RetrivalAllAssetName();
            }
            Debug.Log(GetType() + "/DisposeAll()/字段m_AssetLoader为空，请检查！");
            return null;
        }
    }
}

