using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ScenaryUI : EditorWindow
{
    private Object target;
    private bool toggleObject, toggleChilds;
    [MenuItem("Cimne/Scenary")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(ScenaryUI));
    }

    void Start()
    {
        target = null;
    }
    void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Object you want to put Collider");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        target = EditorGUILayout.ObjectField(target, typeof(Object), true);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Object");
        toggleObject = EditorGUILayout.Toggle(toggleObject);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Childs");
        toggleChilds = EditorGUILayout.Toggle(toggleChilds);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add Collider"))
        {
            GameObject newObject = GameObject.Find(target.name);
            if (toggleObject)
            {
                if (newObject.gameObject.GetComponent<MeshCollider>() == null)
                {
                    newObject.gameObject.AddComponent<MeshCollider>();
                }
            }

            if (toggleChilds)
            {
                Transform[] ts = newObject.GetComponentsInChildren<Transform>();
                foreach (Transform child in ts)
                {
                    if (child != ts[0])
                    {
                        if (child.gameObject.GetComponent<MeshCollider>() == null)
                        {
                            child.gameObject.AddComponent<MeshCollider>();
                        }
                    }
                }
            }
        }
        EditorGUILayout.EndHorizontal();
    }
}
