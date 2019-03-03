using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SUIFW {
    public class ABMgr : MonoBehaviour{
        private static ABMgr m_Instance;
        private Dictionary<string, MultiABMgr> m_DicMultiABMgr;
        private AssetBundleManifest m_ABManifestObj;


        public static ABMgr GetInstance() {
            if (null == m_Instance) {
                m_Instance = new GameObject("AssetBundleMgr").AddComponent<ABMgr>();
            }
            return m_Instance;
        }


        private void Awake() {
            DontDestroyOnLoad(this);
            m_DicMultiABMgr = new Dictionary<string, MultiABMgr>();

            StartCoroutine(ABManifestLoader.GetInstance().LoadABManifestFile());
        }

        public void LoadAssetBundlePack(string sceneName, string abName, DelABLoadComplete loadAllComplete) {

            StartCoroutine(LoadAssetBundle(sceneName, abName, loadAllComplete));
        }

        private IEnumerator LoadAssetBundle(string sceneName,string abName,DelABLoadComplete loadAllComplete) {
            if (string.IsNullOrEmpty(sceneName)||string.IsNullOrEmpty(abName)) {
                Debug.LogError(GetType()+ "/LoadAssetBundlePack()/参数sceneName或abName不能为空!");
                yield break;
            }
            while (!ABManifestLoader.GetInstance().IsFinishLoad) {
                yield return null;
            }
            m_ABManifestObj = ABManifestLoader.GetInstance().GetABManifest();
            if (m_ABManifestObj == null) {
                Debug.LogError(GetType() + "/LoadAssetBundlePack()/字段m_ABManifestObj不能为空,请检查清单文件类是否加载成功!");
                yield break;
            }

            if (!m_DicMultiABMgr.ContainsKey(sceneName)) {
                MultiABMgr tmpMultiABMgr = new MultiABMgr(sceneName,abName, loadAllComplete);
                m_DicMultiABMgr.Add(sceneName, tmpMultiABMgr);
            }
            MultiABMgr MultiABMgrObj = m_DicMultiABMgr[sceneName];
            if (MultiABMgrObj == null ) {
                Debug.LogError(GetType() + "/LoadAssetBundlePack()/字段MultiABMgrObj不能为空!");
                yield break;
            }
            yield return MultiABMgrObj.LoadAssetbundle(abName);
        }

        /// <summary>
        /// 加载指定ab包中的制定资源
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        /// <param name="abName">ab包名称</param>
        /// <param name="resName">ab包中资源名称</param>
        /// <param name="isCache">缓存</param>
        /// <returns></returns>
        public UnityEngine.Object LoadAsset(string sceneName,string abName,string resName,bool isCache = false) {
            if (string.IsNullOrEmpty(sceneName)||string.IsNullOrEmpty(abName)||string.IsNullOrEmpty(resName)) {
                Debug.LogError(GetType()+ "/LoadAsset()/参数sceneName或abName或resName不能为空，请检查！");
                return null;
            }

            if (m_DicMultiABMgr == null || !m_DicMultiABMgr.ContainsKey(sceneName)) {
                Debug.LogError(GetType() + "/LoadAsset()/字段m_DicMultiABMgr为空或对应的资源不存在改，请检查！sceneName="+ sceneName);
                return null;
            }

            return m_DicMultiABMgr[sceneName].LoadAsset(abName, resName, isCache);
        }

        /// <summary>
        /// 释放某个场景的内存和镜像内存
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        public void DisposeAllAssets(string sceneName) {
            if(string.IsNullOrEmpty(sceneName)){
                Debug.LogError(GetType() + "/DisposeAllAssets()/参数不能为空，请检查！");
                return;
            }

            if (m_DicMultiABMgr == null|| !m_DicMultiABMgr.ContainsKey(sceneName)) {
                Debug.LogError(GetType() + "/DisposeAllAssets()/字段m_DicMultiABMgr为空或m_DicMultiABMgr中不存在改场景名称，请检查！");
                return;
            }

            m_DicMultiABMgr[sceneName].DisposeAllAsset() ;
            m_DicMultiABMgr.Remove(sceneName);
        }

        /// <summary>
        /// 释放所有场景的内存和镜像内存，请谨慎使用
        /// </summary>
        public void DisposeAllSceneAllAssets() {

            if (m_DicMultiABMgr == null ) {
                Debug.LogError(GetType() + "/DisposeAllAssets()/字段m_DicMultiABMgr为空，请检查！");
                return;
            }
            foreach (MultiABMgr item_MultiABMgr in m_DicMultiABMgr.Values) {
                item_MultiABMgr.DisposeAllAsset();
            }
        }

    }//Class_End
}
