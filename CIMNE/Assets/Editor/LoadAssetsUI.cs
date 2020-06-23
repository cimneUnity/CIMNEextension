using System;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LoasAssetsUI : EditorWindow

{
    private int position;
    private bool loaded;
    private string pathName;
    private AssetBundleController st;
    private string[] listNames = new string[] {"Null"};

    public struct AssetStruct
    {
        public string Assetname;
        public string ObjectName;
        public string ObjectDescription;
    }

    private List<AssetBundleController.AssetStruct> listAssets = new List<AssetBundleController.AssetStruct>();

    [MenuItem("Cimne/LoadObjects")]

    static void Init()
    {
        EditorWindow.GetWindow(typeof(LoasAssetsUI));

    }
    public void Awake()
    {
        st = new AssetBundleController();
        loaded = false;
    }
    
    void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Path");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
            pathName = EditorGUILayout.TextField(pathName);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Save"))
        {
            st.SaveAssetBundle(pathName);
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Load Bundle"))
        {
            listNames = st.LoadAssetNames();
            if (listNames.Length > 0)
            {
                loaded = true;
                position = 0;
                listAssets = st.LoadAssetBundle(listNames[position]);
            } else
            {
                loaded = false;
            }
        }
        EditorGUILayout.EndHorizontal();

        if (loaded) {
            EditorGUILayout.BeginHorizontal();
                int oldPos = position;
                position = EditorGUILayout.Popup(position, listNames);
                if (position != oldPos) listAssets = st.LoadAssetBundle(listNames[position]);

            EditorGUILayout.EndHorizontal();

            if (listAssets != null)
            {
                for (var i = 0; i < listAssets.Count; i++)
                {
                    AssetBundleController.AssetStruct node = listAssets[i];
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(node.Assetname);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Object Name");
                    node.ObjectName = EditorGUILayout.TextField(node.ObjectName);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Object Description");
                    node.ObjectDescription = EditorGUILayout.TextField(node.ObjectDescription);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("Add"))
                    {
                        st.InstantiateObjectFromBundle(node);
                    }
                    EditorGUILayout.EndHorizontal();
                    listAssets[i] = node;
                }
            }
        }
        
    }
}