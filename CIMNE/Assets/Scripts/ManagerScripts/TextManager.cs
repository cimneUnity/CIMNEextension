using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    public static TextManager current;

    private InputField textName;
    private Button createButton, cancelButton;
    private Toggle visibility;
    private GameObject canvas;

    private void Awake() //Called when awake
    {
        current = this;
    }

    public void Start() //Called when start
    {
        canvas = GameObject.Find("PopupUI");
        textName = GameObject.Find("InputAnotationName").GetComponent<InputField>(); ;
        createButton = GameObject.Find("AcceptTypeButton").GetComponent<Button>();
        cancelButton = GameObject.Find("CancelTypeButton").GetComponent<Button>();
        visibility = GameObject.Find("VisibilityToggle").GetComponent<Toggle>();
        createButton.onClick.AddListener(() => { Save(); });
        cancelButton.onClick.AddListener(() => { Exit(); });
    }

    void Update() //Called every frame
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("Enter pressed");
            Save();
        }

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Button 0");
        }
    }

    void Save()
    {
        if (visibility.isOn)
        {
            GameObject newNote = new GameObject();
            newNote.name = textName.text;
            newNote.AddComponent<NoteBehaviour>();
            
            newNote.transform.SetParent(canvas.transform);
            newNote.transform.position = GameObject.Find("Player").transform.position;
            Debug.Log(newNote == null ? "No creat" : "Creat");
        }

        Exit();
    }

    void Exit()
    {
        EventController.current.ChangeMode("play", null);
    }
}
