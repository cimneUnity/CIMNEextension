using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadAssetBundles : MonoBehaviour
{
    AssetBundle loadedAssetBundle;
    public string path;
    public string assetName;

    void Start() //Called when start
    {
        LoadAssetBundle(path);
        InstantiateObjectFromBundle(assetName);
    }

    void LoadAssetBundle(string bundleURL)
    {
        loadedAssetBundle = AssetBundle.LoadFromFile(bundleURL);
        Debug.Log(loadedAssetBundle == null ? " Failed to lad asset" : " asset load succesfully");
    }

    void InstantiateObjectFromBundle(string tmpAssetName)
    {
        var prefab = loadedAssetBundle.LoadAsset(tmpAssetName);
        Instantiate(prefab);
    }
}
