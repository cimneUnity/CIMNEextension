using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class AccidentUI : EditorWindow

{
    private VisualElement root;
    private TextField nameField;
    private SliderInt  probabilityField;
    private Button buttonAccident;
    private Toggle tsphd, tcub, tcyl;
    private GameObject accidentParent;
    private string lastChanged = "sphere";
    [MenuItem("Cimne/Accidents")]

    static void Init()
    {
        EditorWindow.GetWindow(typeof(AccidentUI));
    }

    public void ShowWindow()
    {
        var window = GetWindow<AccidentUI>();
        window.titleContent = new GUIContent("Accidents");
        window.minSize = new Vector2(250, 50);
    }

    private void OnEnable()
    {
        accidentParent = GameObject.Find("Accidents");

        root = rootVisualElement;
        VisualTreeAsset quickToolVisualTree = Resources.Load<VisualTreeAsset>("AccidentUI_Main");
        quickToolVisualTree.CloneTree(root);

        VisualElement tmp = root.Query(name: "sphere-toggle");
        tsphd = tmp.Query<Toggle>();

        tmp = root.Query(name: "cube-toggle");
        tcub = tmp.Query<Toggle>();

        tmp = root.Query(name: "cylinder-toggle");
        tcyl = tmp.Query<Toggle>();

        tmp = root.Query(name: "name-field");
        nameField = tmp.Query<TextField>();

        tmp = root.Query(name: "probability-field");
        probabilityField = tmp.Query<SliderInt>();

        tmp = root.Query(name: "accident-button");
        buttonAccident = tmp.Query<Button>();
        buttonAccident.clickable.clicked += () => InstantiateAccident();
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

    private void InstantiateAccident()
    {
        GameObject newAccident = new GameObject();

        if (nameField.value != "")
        {
            newAccident.name = nameField.value;
        }
        else
        {
            newAccident.name = "Accident";
        }

        AccidentBehaviour ab = newAccident.AddComponent<AccidentBehaviour>() as AccidentBehaviour;

        ab.probability = probabilityField.value;

        if (tcub.value)
        {
            BoxCollider sc = newAccident.AddComponent<BoxCollider>();
            sc.isTrigger = true;
        }
        else if (tcyl.value)
        {
            CapsuleCollider sc = newAccident.AddComponent<CapsuleCollider>();
            sc.isTrigger = true;
        }
        else
        {
            SphereCollider sc = newAccident.AddComponent<SphereCollider>();
            sc.isTrigger = true;
        }
        newAccident.transform.parent = accidentParent.transform;
    }
}