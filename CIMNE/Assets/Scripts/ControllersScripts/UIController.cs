using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController current;
    public bool UItask, UIobject, UIscore, UItime, UIrisk, UIpointer, UIstate, ViewObjects, ViewRisks;

    private int count3, count2, count1, oldState;
    private GameObject labelTime, labelScore, labelRiskName, labelRiskDistance, colorState, player;
    private GameObject labelObjectName, labelObjectNameExt, labelObjectDescription, labelObjectDescriptionExt, objectDescriptionPart, objectDescriptionExtPart, objectNamePart;
    private GameObject mainGUI;
    private RectTransform rtf;
    private List<GameObject> riskList;
    private string currentName, currentDescription;
    private bool showingDescription, showingExtension;

    private void Awake()
    {
        current = this;
    }

    void Start()
    {
        player = GameObject.Find("Player");
        mainGUI = GameObject.Find("PlayUI");
        configureUI();
        rtf = mainGUI.GetComponent<RectTransform>();
    }

    void Update()
    {
        if (UItime) timeController();
        if (UIscore) scoreController();
        if (UIrisk) riskController();
        if (UIstate) stateController();
        
        Vector2 vd = rtf.sizeDelta;
        if (rtf.sizeDelta.x != Screen.width) vd.x = Screen.width;
        if (rtf.sizeDelta.y != Screen.height) vd.y = Screen.height;
        rtf.sizeDelta = vd;
        
    }

    void configureUI()
    {

        if (UItime) labelTime = GameObject.Find("TimeLabel");
        else GameObject.Find("TimePart").SetActive(false);

        if (UIscore) labelScore = GameObject.Find("ScoreLabel");
        else GameObject.Find("ScorePart").SetActive(false);

        if (UIrisk)
        {
            labelRiskName = GameObject.Find("RiskNameLabel");
            labelRiskDistance = GameObject.Find("RiskDistanceLabel");
            riskList = GlobalController.current.GetRiskList();
        }
        else GameObject.Find("RiskPart").SetActive(false);

        if (UIstate)
        {
            EventController.current.onTriggerEnterRisk += enterZone;
            EventController.current.onTriggerExitRisk += exitZone;

            colorState = GameObject.Find("colorState");
        }
        else GameObject.Find("StatePart").SetActive(false);

        if (UIobject)
        {
            EventController.current.onSeeObject += SeeObject;
            EventController.current.onUnseeObject += UnseeObject;
            EventController.current.onExtendDescription += ExtendDescription;
            EventController.current.onShowDescription += ShowDescription;

            labelObjectName = GameObject.Find("ObjectName");
            labelObjectNameExt = GameObject.Find("ObjectNameExt");
            labelObjectDescription = GameObject.Find("ObjectDescription");
            labelObjectDescriptionExt = GameObject.Find("ObjectDescriptionExt");

            objectNamePart = GameObject.Find("ObjectNamePart");
            objectDescriptionPart = GameObject.Find("ObjectDescriptionPart");
            objectDescriptionExtPart = GameObject.Find("ObjectDescriptionExtPart");

            objectNamePart.SetActive(false);
            objectDescriptionPart.SetActive(false);
            objectDescriptionExtPart.SetActive(false);
        }
        else 
        {
            GameObject.Find("ObjectPart").SetActive(false);
        } 
        if (!UItask) GameObject.Find("TaskPart").SetActive(false);
        if (!UIpointer) GameObject.Find("pointer").SetActive(false);

    }

    void timeController()
    {
        float time = GlobalController.current.GetTime();
        string tmpStr = TimeSpan.FromSeconds(time).ToString(@"hh\:mm\:ss");
        labelTime.GetComponent<UnityEngine.UI.Text>().text = tmpStr;
    }

    void scoreController()
    {
        labelScore.GetComponent<UnityEngine.UI.Text>().text = GlobalController.current.globalScore.ToString();
    }

    void riskController()
    {
        float minDistance = 10000f;
        string nameRisk = "Nulle";
        if (riskList.Count != 0)
        {
            foreach (GameObject child in riskList)
            {
                var heading = child.GetComponent<Transform>().position - player.GetComponent<Transform>().position;
                var distance = heading.magnitude;
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nameRisk = child.name;
                }
            }
            labelRiskName.GetComponent<UnityEngine.UI.Text>().text = nameRisk;
            float truncated = (float)(Math.Truncate((double)minDistance * 10.0) / 10.0);
            labelRiskDistance.GetComponent<UnityEngine.UI.Text>().text = truncated.ToString() + " m";
        } else
        {
            labelRiskName.GetComponent<UnityEngine.UI.Text>().text = "";
            labelRiskDistance.GetComponent<UnityEngine.UI.Text>().text = "";
        }
    }

    void stateController()
    {
        if (count3 > 0)
        {
            if (oldState != 3)
            {
                oldState = 3;
                colorState.GetComponent<UnityEngine.UI.Image>().color = new Color32(254, 9, 0, 255);
            }
        }
        else if (count2 > 0)
        {
            if (oldState != 2)
            {
                oldState = 2;
                colorState.GetComponent<UnityEngine.UI.Image>().color = new Color32(254, 161, 0, 255);
            }
        }
        else if (count1 > 0)
        {
            if (oldState != 1)
            {
                oldState = 1;
                colorState.GetComponent<UnityEngine.UI.Image>().color = new Color32(254, 224, 0, 255);
            }
        }
        else
        {
            if (oldState != 0)
            {
                oldState = 0;
                colorState.GetComponent<UnityEngine.UI.Image>().color = new Color32(255, 255, 255, 255);
            }
        }
    }

    private void SeeObject(string name, string description)
    {
        currentName = name;
        currentDescription = description;
        labelObjectName.GetComponent<UnityEngine.UI.Text>().text = currentName;
        objectNamePart.SetActive(true);
        showingDescription = false;
        showingExtension = false;
    }

    private void UnseeObject()
    {
        objectNamePart.SetActive(false);
        objectDescriptionPart.SetActive(false);
        objectDescriptionExtPart.SetActive(false);
        showingDescription = false;
        showingExtension = false;
    }

    private void ShowDescription()
    {
        showingDescription = !showingDescription;
        if (showingDescription)
        {
            labelObjectDescription.GetComponent<UnityEngine.UI.Text>().text = currentDescription;
            objectDescriptionPart.SetActive(true);
        } else
        {
            objectDescriptionPart.SetActive(false);
            showingExtension = false;
        }
    }

    private void ExtendDescription()
    {
        showingExtension = !showingExtension;
        if (showingDescription)
        {
            if (showingExtension)
            {
                labelObjectDescriptionExt.GetComponent<UnityEngine.UI.Text>().text = currentDescription;
                labelObjectNameExt.GetComponent<UnityEngine.UI.Text>().text = currentName;
                objectDescriptionExtPart.SetActive(true);
            }
            else
            {
                objectDescriptionExtPart.SetActive(false);
            }
        }
    }

    public void enterZone(int importance)
    {

        if (importance == 3) ++count3;
        if (importance == 2) ++count2;
        if (importance == 1) ++count1;
    }

    public void exitZone(int importance)
    {
        if (importance == 3) --count3;
        if (importance == 2) --count2;
        if (importance == 1) --count1;
    }
}
