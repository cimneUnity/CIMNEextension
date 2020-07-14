using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GlobalController : MonoBehaviour
{
    public static GlobalController current;
    public int globalScore;
    public bool restart, type, finish, instructions;
    private float globalTime;
    private List<GameObject> riskList;
    private GameObject risk;

    private void Awake() //Called when awake
    {
        current = this;
    }

    void Start() //Called when start
    {
        risk = GameObject.Find("Risks");
        globalScore = 0;
        globalTime = 0;
        riskList = new List<GameObject>();

        for (int i = 0; i < risk.transform.childCount; i++)
        {
            GameObject child = risk.transform.GetChild(i).gameObject;
            riskList.Add(child);
        }
    }

    void Update() //Called every frame
    {
        globalTime += Time.deltaTime;
         
    }

    public float GetTime()
    {
        return globalTime;
    }

    public List<GameObject> GetRiskList()
    {
        return riskList;
    }

    public void InstantiatePrefab(UnityEngine.Object prefab, string nameObj, string nameAsset, string descriptionObj)
    {
        GameObject InitiatedAsset = (GameObject)Instantiate(prefab);
        if (InitiatedAsset.GetComponent<ObjectBehaviour>() == null)
        {
            ObjectBehaviour ob = InitiatedAsset.AddComponent<ObjectBehaviour>() as ObjectBehaviour;
            if (nameObj != "")
            {
                InitiatedAsset.name = nameObj;
            }
            else
            {
                InitiatedAsset.name = nameAsset;
            }
            ob.description = descriptionObj;
        }
    }
}