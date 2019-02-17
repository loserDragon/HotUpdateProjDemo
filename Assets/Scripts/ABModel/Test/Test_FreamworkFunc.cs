using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SUIFW {
    public class Test_FreamworkFunc :MonoBehaviour{
        private string sceneName = "Scene1";

        private string abName= "scene1/prefabs.ab";

        private string resName= "Cube.prefab";

        private void Start() {

            //这里要在做一个封装，这个太长了，不是很好
            MgrAssetBundle.GetInstance().LoadAssetBundlePack(sceneName, abName, OnLoadAssetComplete);
            //StartCoroutine(MgrAssetBundle.GetInstance().LoadAssetBundlePack(sceneName, abName,OnLoadAssetComplete));
        }

        private void OnLoadAssetComplete(string ABName) {
            UnityEngine.Object tmpObj = MgrAssetBundle.GetInstance().LoadAsset(sceneName, abName, resName);
            if (tmpObj!= null) {
                Instantiate((GameObject)tmpObj);
            }
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.P)) {
                Debug.Log("释放场景资源");
                MgrAssetBundle.GetInstance().DisposeAllAssets(sceneName);
            }
        }
    }
}
