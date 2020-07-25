using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GlobalController : MonoBehaviour
{
    public static GlobalController current;
    public int globalScore;
    public bool restart, type, finish;
    private float globalTime;
    private List<GameObject> riskList;
    private bool timeStop = false;
    private bool timeDirection = true;

    private void Awake() //Called when awake
    {
        current = this;
    }

    void Start() //Called when start
    {
        GameObject risk = GameObject.Find("Risks");
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
        if (timeStop)
        {
            if (timeDirection)
            {
                globalTime += Time.deltaTime;
            } else
            {
                globalTime -= Time.deltaTime;
            }
        }
    }

    //Score functions
    public int getScore()
    {
        return globalScore;
    }

    public void setScore(int newScore)
    {
        globalScore = newScore;

    }

    public void addScore(int newScore)
    {
        globalScore += newScore;
    }

    //TimeFunctions
    public float getTime()
    {
        return globalTime;
    }

    public bool setTime(float newTime)
    {
        if (newTime < 0) return false;
        globalTime = newTime;
        return true;
    }

    public void addTime(float newTime)
    {
        globalTime += newTime;
    }

    public void startTime()
    {
        timeStop = true;
    }

    public void stopTime()
    {
        timeStop = false;
    }

    public void setTimeDirection(bool direction)
    {
        timeDirection = direction;
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