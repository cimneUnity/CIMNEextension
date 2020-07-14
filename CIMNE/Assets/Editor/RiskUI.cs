using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class RiskUI : EditorWindow

{
    private VisualElement root;
    private TextField nameField, scoreField;
    private SliderInt priorityField;
    private Button buttonRisk;
    private Toggle tsphd, tcub, tcyl;
    private GameObject riskParent;
    private string lastChanged = "sphere";
    [MenuItem("Cimne/Risks")]

    static void Init()
    {
        EditorWindow.GetWindow(typeof(RiskUI));
    }

    public void ShowWindow()
    {
        var window = GetWindow<RiskUI>();
        window.titleContent = new GUIContent("Risks");
        window.minSize = new Vector2(250, 50);
    }

    private void OnEnable()
    {
        riskParent = GameObject.Find("Risks");

        root = rootVisualElement;
        VisualTreeAsset quickToolVisualTree = Resources.Load<VisualTreeAsset>("RiskUI_Main");
        quickToolVisualTree.CloneTree(root);

        VisualElement tmp = root.Query(name: "sphere-toggle");
        tsphd = tmp.Query<Toggle>();

        tmp = root.Query(name: "cube-toggle");
        tcub = tmp.Query<Toggle>();

        tmp = root.Query(name: "cylinder-toggle");
        tcyl = tmp.Query<Toggle>();

        tmp = root.Query(name: "name-field");
        nameField = tmp.Query<TextField>();

        tmp = root.Query(name: "priority-field");
        priorityField = tmp.Query<SliderInt>();

        tmp = root.Query(name: "score-field");
        scoreField = tmp.Query<TextField>();

        tmp = root.Query(name: "risk-button");
        buttonRisk = tmp.Query<Button>();
        buttonRisk.clickable.clicked += () => InstantiateRisk();
    }

    void Update()
    {
        if (tsphd.value && lastChanged != "sphere")
        {
            lastChanged = "sphere";
            tcub.value = false;
            tcyl.value = false;
        }
        if (tcub.value && lastChanged != "cube")
        {
            lastChanged = "cube";
            tsphd.value = false;
            tcyl.value = false;
        }
        if (tcyl.value && lastChanged != "cylinder")
        {
            lastChanged = "cylinder";
            tsphd.value = false;
            tcub.value = false;
        }
        if (!tcyl.value && !tcub.value && !tsphd.value)
        {
            if (lastChanged == "sphere") tsphd.value = true;
            else if (lastChanged == "cube") tcub.value = true;
            else if (lastChanged == "cylinder") tcyl.value = true;
        }
    }

    private void InstantiateRisk()
    {
        GameObject newRisk = new GameObject();
        
        if (nameField.value != "")
        {
            newRisk.name = nameField.value;
        }
        else
        {
            newRisk.name = "Risk";
        }

        RiskBehaviour rb = newRisk.AddComponent<RiskBehaviour>() as RiskBehaviour;

        rb.importance = priorityField.value;

        if (scoreField.value != "")
        {

            int i = 0;
            if (!Int32.TryParse(scoreField.value, out i))
            {
                i = -1;
            }
            rb.score = i;
        } 
        else
        {
            rb.score = 0;
        }

        if (tcub.value)
        {
            BoxCollider sc = newRisk.AddComponent<BoxCollider>();
            sc.isTrigger = true;
        } 
        else if (tcyl.value)
        {
            CapsuleCollider sc = newRisk.AddComponent<CapsuleCollider>();
            sc.isTrigger = true;
        } 
        else
        {
            SphereCollider sc = newRisk.AddComponent<SphereCollider>();
            sc.isTrigger = true;
        }
        newRisk.transform.parent = riskParent.transform;
    }
}