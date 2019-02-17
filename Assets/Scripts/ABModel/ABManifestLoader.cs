using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SUIFW {
    public class ABManifestLoader : System.IDisposable {

        private static ABManifestLoader m_Instance;

        private AssetBundleManifest m_AssetBundleManifest;

        private string m_StrABManifestPath;

        private AssetBundle m_ABReadManifest;

        private bool m_isFinishLoad;
        public bool IsFinishLoad {
            get {
                return m_isFinishLoad;
            }
        }

        public ABManifestLoader() {
            m_StrABManifestPath = PathTools.GetABManifest();
            m_ABReadManifest = null;
            m_AssetBundleManifest = null;
            m_isFinishLoad = false;
            
        }

        public static ABManifestLoader GetInstance() {
            if (m_Instance == null) {
                m_Instance = new ABManifestLoader();
            }
            return m_Instance;
        }

        public IEnumerator LoadABManifestFile() {
            using (WWW m_www = new WWW(m_StrABManifestPath)) {
                yield return m_www;
                if (m_www.progress >= 1) {
                    m_ABReadManifest = m_www.assetBundle;
                    if (m_ABReadManifest == null) {
                        Debug.LogError(GetType() + "/LoadABManifestFile()/加载清单文件失败，请检查！m_StrABManifestPath：" + m_StrABManifestPath + "错误信息：" + m_www.error);
                        yield break;
                    }
                    m_AssetBundleManifest = m_ABReadManifest.LoadAsset("AssetBundleManifest") as AssetBundleManifest;
                    m_isFinishLoad = true;
                }
            }
        }

        public AssetBundleManifest GetABManifest() {
            if (m_isFinishLoad) {
                if (m_AssetBundleManifest == null) {
                    Debug.Log(GetType() + "/GetABManifest()/m_AssetBundleManifest is null,请检查！");
                    return null;
                }
                return m_AssetBundleManifest;
            }
            else {
                Debug.Log(GetType() + "/GetABManifest()/m_isFinishLoad ==false,加载未完成或未加载！");
                return null;
            }
        }

        public string[] RetervalABDependen(string abName) {
            if (m_AssetBundleManifest != null && !string.IsNullOrEmpty(abName)) {
                return m_AssetBundleManifest.GetAllDependencies(abName);
            }
            Debug.Log(GetType() + "/RetervalABDependen()/字段m_AssetBundleManifest为空,请检查！");
            return null;
        }


        public void Dispose() {
            if (m_ABReadManifest != null) {
                m_ABReadManifest.Unload(true);
            }
        }
    }
}
