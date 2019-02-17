using System;
using System.Collections;
using System.Collections.Generic;
using SUIFW;
using UnityEngine;

public class Test_SingleABLoader : MonoBehaviour {
    private string ABName1 = "scene1/textures.ab";
    private string ABName2 = "scene1/materials.ab";
    private string ABName3 = "scene1/prefabs.ab";

    private string resName1 = "Assets/AB_Res/Scene1/Prefabs/Sphere.prefab";
    private string resName2 = "Assets/AB_Res/Scene1/Prefabs/Cube.prefab";
    private string resName3 = "Assets/AB_Res/Scene1/Prefabs/Capsule.prefab";

    SingleABLoader m_SingleABLoader1;
    SingleABLoader m_SingleABLoader2;
    SingleABLoader m_SingleABLoader3;

    void Start () {
        m_SingleABLoader1 = new SingleABLoader(ABName1, OnLoadComplete1);
        StartCoroutine(m_SingleABLoader1.LoadSingleAB());
    }

    private void OnLoadComplete1(string ABName) {
        Debug.Log("加载ab包完毕");
        m_SingleABLoader2 = new SingleABLoader(ABName2, OnLoadComplete2);
        StartCoroutine(m_SingleABLoader2.LoadSingleAB());
    }
    private void OnLoadComplete2(string ABName) {
        Debug.Log("加载ab包完毕");
        m_SingleABLoader3 = new SingleABLoader(ABName3, OnLoadComplete3);
        StartCoroutine(m_SingleABLoader3.LoadSingleAB());
    }
    UnityEngine.Object obj;
    private void OnLoadComplete3(string ABName) {
        obj =  m_SingleABLoader3.LoadAsset(resName2,false);
        Instantiate(obj,new Vector3(0,0,0),Quaternion.identity);

         obj = m_SingleABLoader3.LoadAsset(resName1, false);
        Instantiate(obj, new Vector3(1, 0, 0), Quaternion.identity);

         obj = m_SingleABLoader3.LoadAsset(resName3, false);
        Instantiate(obj, new Vector3(2, 0, 0), Quaternion.identity);
    }

    public void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            Debug.Log("释放内存镜像资源");
            m_SingleABLoader3.Dispose();

        }

        if (Input.GetKeyDown(KeyCode.O)) {
            Debug.Log("释放内存镜像资源和内存资源");
            m_SingleABLoader3.DisposeAll();
        }
        if (Input.GetKeyDown(KeyCode.   I)) {
            Debug.Log("释放内存镜像资源和内存资源");
            m_SingleABLoader3.UnLoadAsset(obj);
        }
    }
}
