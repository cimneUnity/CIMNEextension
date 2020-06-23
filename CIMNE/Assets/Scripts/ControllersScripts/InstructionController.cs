using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionController : MonoBehaviour
{
    public static InstructionController current;
    private bool open = false;
    private GameObject instructions;

    private void Awake()
    {
        current = this;
    }
    void Start()
    {
        string fileText = System.IO.File.ReadAllText("Assets/Resources/instructions.txt");
        GameObject.Find("InstructionsText").GetComponent<UnityEngine.UI.Text>().text = fileText;
        instructions = GameObject.Find("InstructionsUI");
        instructions.SetActive(false);
        EventController.current.onOpenInstructuions += OpenInstructuions;
    }

    private void OpenInstructuions()
    {
        open = !open;
        instructions.SetActive(open);
    }
}
