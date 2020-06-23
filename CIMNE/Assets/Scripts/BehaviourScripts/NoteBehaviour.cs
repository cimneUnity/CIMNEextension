using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteBehaviour : MonoBehaviour
{
    private static GameObject canvas;
    private GameObject instance;
    private bool showLabel;

    void Start()
    {
        Debug.Log("Note created");
        showLabel = true;
        if (showLabel)
        {
            canvas = GameObject.Find("PopupUI");
            var popupText = Resources.Load("Prefabs/PopUpObject");
            instance = (GameObject)Instantiate(popupText);
            instance.transform.SetParent(canvas.transform);
            instance.transform.GetComponent<UnityEngine.UI.Text>().text = this.name;
        }
    }

    void Update()
    {
        if (showLabel)
        {
            Vector3 screenposition = Camera.main.WorldToScreenPoint(this.transform.position);
            if (screenposition.z >= 0)
            {
                instance.SetActive(true);
                instance.transform.position = screenposition;
            }
            else
            {
                instance.SetActive(false);
            }
        }
    }

    public void Interact()
    {
        GameObject.Find("Controller").GetComponent<WritterController>().Write("Player interact with " + this.name);
        Material M = transform.GetComponent<Renderer>().material;
        Destroy(M);
    }
}