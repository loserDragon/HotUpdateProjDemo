using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SUIFW{
    public class MultiABMgr {
        private SingleABLoader m_SingleABLoader;

        private Dictionary<string, SingleABLoader> m_DicSingleABLoaderCache;

        private string currentSceneName;

        private string abName;

        private Dictionary<string, ABRelation> m_DicABRelation;

        private DelABLoadComplete m_DelAllABLoadComplete;

        public MultiABMgr(string sceneName,string ABName,DelABLoadComplete OnLoadAllABComplete) {
            if (string.IsNullOrEmpty(sceneName)|| string.IsNullOrEmpty(ABName)) {
                Debug.LogError(GetType()+ "/构造函数/参数sceneName或ABName不能为空，请检查！");
                return ;
            }
            this.currentSceneName = sceneName;
            this.abName = ABName;
            this.m_DelAllABLoadComplete = OnLoadAllABComplete;
            m_DicSingleABLoaderCache = new Dictionary<string, SingleABLoader>();
            m_DicABRelation = new Dictionary<string, ABRelation>();
        }

        public void OnLoadAllABComplete(string abName) {
            ///这里是加载当前ab包的时候，会先加载它的依赖包
            ///加载依赖包和加载当前包都会调用到这里，
            ///所以加了这个判断是，当加载依赖包的时候，不能执行回调的
            ///只有加载的包是当前的包，才执行回调，表示加载完成
            if (abName.Equals(this.abName)) {
                if (this.m_DelAllABLoadComplete!=null) {
                    this.m_DelAllABLoadComplete(abName);
                }
            }
        }

        public IEnumerator LoadAssetbundle(string abName) {
            if (string.IsNullOrEmpty(abName)) {
                Debug.LogError(GetType()+ "/LoadAssetbundle()/参数abName不能为空，请检查！");
                yield break;
            }

            if (!m_DicABRelation.ContainsKey(abName)) {
                ABRelation tmpABRelation = new ABRelation(abName);
                m_DicABRelation.Add(abName, tmpABRelation);
            }
            ABRelation m_ABRelation = m_DicABRelation[abName];
            string[] ABDependences =  ABManifestLoader.GetInstance().RetervalABDependen(abName);
            foreach (string ABDependence in ABDependences) {
                m_ABRelation.AddABDependence(ABDependence);
                yield return LoadABReference(ABDependence, abName);

            }

            if (!m_DicSingleABLoaderCache.ContainsKey(abName)) {
                SingleABLoader tmpSingleABLoader = new SingleABLoader(abName, OnLoadAllABComplete);
                m_DicSingleABLoaderCache.Add(abName, tmpSingleABLoader);
            }
            SingleABLoader m_SingleABLoader = m_DicSingleABLoaderCache[abName];

            yield return m_SingleABLoader.LoadSingleAB();
        }

        public IEnumerator LoadABReference(string abName,string refABName) {
            if (string.IsNullOrEmpty(abName)||string.IsNullOrEmpty(refABName)) {
                Debug.LogError(GetType()+ "/LoadABReference()/参数abName或refABName不能为空，请检查！");
                yield break;
            }
            if (m_DicABRelation.ContainsKey(abName)) {
                ABRelation tmpABRelation = m_DicABRelation[abName];
                tmpABRelation.AddABReference(refABName);
            }
            else {
                ABRelation tmpABRelation = new ABRelation( abName);
                tmpABRelation.AddABReference(refABName);
                m_DicABRelation.Add(abName, tmpABRelation);

                yield return LoadAssetbundle(abName);
            }

        }


        public UnityEngine.Object LoadAsset(string abName,string resName,bool isCache = false) {
            if (string.IsNullOrEmpty(abName)||string.IsNullOrEmpty(resName)) {
                Debug.LogError(GetType()+ "/LoadAsset()/参数abName或resName不能为空，请检查！");
                return null;
            }

            if (m_DicSingleABLoaderCache.ContainsKey(abName)) {
                return m_DicSingleABLoaderCache[abName].LoadAsset(resName, isCache);
            }
           
            Debug.LogError(GetType() + "/LoadAsset()/加载资源失败，请检查！abName="+abName+ "|resName=" + resName);
            return null;
        }


        public void DisposeAllAsset() {
            try {
                foreach (SingleABLoader item_SingleABLoader in m_DicSingleABLoaderCache.Values) {
                    item_SingleABLoader.DisposeAll();
                }
            }
            catch (Exception e) {
                Debug.LogError(e.ToString());
            }
            finally {
                m_DicSingleABLoaderCache.Clear();
                m_DicSingleABLoaderCache = null;

                m_DicABRelation.Clear();
                m_DicABRelation = null;

                m_DelAllABLoadComplete = null;
                currentSceneName = null;
                abName = null;

                Resources.UnloadUnusedAssets();
                GC.Collect();
            }

        }
    }//Class_End
}
