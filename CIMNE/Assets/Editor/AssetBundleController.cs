using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class AssetBundleController
{
    public AssetBundleController() { }
    private Dictionary<string, AssetBundle> bundleDictionary = new Dictionary<string, AssetBundle>();
    private AssetBundle currentAB;

    public string pathDefault = @"C:\Users\raimo\Desktop\AssetBundles\";

    public struct AssetStruct
    {
        public string Assetname;
        public string ObjectName;
        public string ObjectDescription;
    }

    private List<AssetStruct> listAssets = new List<AssetStruct>();

    public void SaveAssetBundle(string pathTmp)
    {
        if (Directory.Exists(pathTmp))
        {
            pathDefault = pathTmp;
            Debug.Log("PathTmp: " + pathDefault);
        } else {
            Debug.Log("DefaultPath: " + pathDefault);
        }

        BuildPipeline.BuildAssetBundles(pathDefault, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows64);
    }


    public List<AssetStruct> LoadAssetBundle(string name)
    {

        if (bundleDictionary.ContainsKey(name))
        {
            currentAB = bundleDictionary[name];
        } else
        {
            string bundleURL = pathDefault + name;
            currentAB = AssetBundle.LoadFromFile(bundleURL);
            bundleDictionary.Add(name, currentAB);
        }

        if (currentAB == null)
        {
            return null;
        }
        else
        {
            LoadAssets();
            return listAssets;
        }
    }

    public string[] LoadAssetNames()
    {
        List<string> listnames = new List<string>();
        string[] fileEntries = Directory.GetFiles(pathDefault);
        foreach (string fileName in fileEntries)
            listnames.Add(ProcessFile(fileName, pathDefault));
        listnames = listnames.Distinct().ToList();
        Debug.Log("Count: " + listnames.Count);

        string[] nombre = new string[listnames.Count];
        int i = 0;
        foreach (string str in listnames)
        {
            nombre[i] = str;
            ++i;
        }
        return nombre;
    }

    public string ProcessFile(string path, string root)
    {
        path = path.Replace(root, "");
        path = path.Replace(".manifest", "");
        return path;
    }

    private void LoadAssets()
    {
        String[] assetNames = currentAB.GetAllAssetNames();
        listAssets = new List<AssetStruct>();
        for (int i = 0; i < assetNames.Length; ++i)
        {
            AssetStruct tmpAs = new AssetStruct();

            String tmpName = assetNames[i];
            tmpName = tmpName.Replace(".prefab", "");
            string[] tmpchain = tmpName.Split('/');
            tmpName = tmpchain[tmpchain.Length - 1];
            tmpAs.Assetname = tmpName;
            tmpAs.ObjectName = "";
            tmpAs.ObjectDescription = "";
            listAssets.Add(tmpAs);
        }
    }

    public void InstantiateObjectFromBundle(AssetStruct node)
    {
        string nameItem = node.ObjectName;
        string descriptionItem = node.ObjectDescription;

        UnityEngine.Object prefab = currentAB.LoadAsset(node.Assetname);

        GameObject.Find("Controller").GetComponent<GlobalController>().InstantiatePrefab(prefab, nameItem, node.Assetname, descriptionItem);
    }
}
