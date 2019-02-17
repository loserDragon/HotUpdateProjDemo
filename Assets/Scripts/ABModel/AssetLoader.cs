using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SUIFW {
    public class AssetLoader : System.IDisposable {

        private AssetBundle m_CurrentAssetBundle;

        private Hashtable m_Hashtable;

        public AssetLoader(AssetBundle m_CurrentAssetBundle) {
            if (m_CurrentAssetBundle == null) {
                Debug.LogError(GetType()+ "/构造函数/参数 m_CurrentAssetBundle为null，请检查！");
                return;
            }
            this.m_CurrentAssetBundle = m_CurrentAssetBundle;
            this.m_Hashtable = new Hashtable();
        }

        public UnityEngine.Object LoadAsset(string assetName,bool isCache = false) {
            return LoadResource<UnityEngine.Object>(assetName, isCache);
        }

        private T LoadResource<T>(string assetName, bool isCache = false) where T : UnityEngine.Object {
            if (this.m_CurrentAssetBundle == null) {
                Debug.LogError(GetType() + "/LoadResource()/this.m_CurrentAssetBundle为null，请检查！");
                return null;
            }
            if (this.m_Hashtable != null &&this.m_Hashtable.ContainsKey(assetName)) {
                return this.m_Hashtable[assetName] as T;
            }

            UnityEngine.Object tmpObj = m_CurrentAssetBundle.LoadAsset(assetName);
            if (isCache && this.m_Hashtable != null ) {
                this.m_Hashtable.Add(assetName, tmpObj);
            }

            return tmpObj as T;
        }

        public string[] RetrivalAllAssetName() {
            if (m_CurrentAssetBundle!=null) {
                return m_CurrentAssetBundle.GetAllAssetNames();
            }
            return null;
        }

        /// <summary>
        /// 卸载指定的资源
        /// </summary>
        /// <param name="assetName"></param>
        public void UnLoadAsset(UnityEngine.Object asset) {
            if (asset != null) {
                Resources.UnloadAsset(asset);                                //不管资源有没有使用都卸载
            }
            //卸载未使用的资源
            //Resources.UnloadUnusedAssets();
        }

        /// <summary>
        /// 释放assetbundle的内存镜像资源
        /// </summary>
        public void Dispose() {
            if (this.m_CurrentAssetBundle != null) {
                this.m_CurrentAssetBundle.Unload(false);
            }
        }

        /// <summary>
        /// 释放assetbundle的内存镜像资源,和内存
        /// </summary>
        public void DisposeAll() {
            if (this.m_CurrentAssetBundle != null) {
                this.m_CurrentAssetBundle.Unload(true);
            }
        }
    }
}

