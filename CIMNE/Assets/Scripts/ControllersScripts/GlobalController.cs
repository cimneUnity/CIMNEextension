using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GlobalController : MonoBehaviour
{
    //Pull satisfactorio
    public static GlobalController current;
    public int globalScore;
    public bool restart, type, finish, instructions;
    private float globalTime;
    private List<GameObject> riskList;
    private GameObject risk;

    private void Awake()
    {
        current = this;
    }

    void Start()
    {
        //Debug.Log("GlobalController Start");

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

    void Update()
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
        //Debug.Log(InitiatedAsset == null ? " Failed" : " Loaded: " + nameItem.value);
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
            //InitiatedAsset.layer = 10;

            ob.description = descriptionObj;
        }
    }
}