using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBehaviour : MonoBehaviour
{
    public string description;
    public string scriptInteract = null;
    public string scriptTouch = null;
    public bool showname = false;
    public bool showDescription = true;
    //public bool tableInvolved = false;
    private static GameObject canvas;
    private GameObject instance;
    private InteractObject intObj;
    private TouchObject touObj;

    void Start()
    {
        if (description == "") description = "";
        if (scriptInteract != null)
        {
            intObj = new InteractObject(scriptInteract);
        }

        if (scriptTouch != null)
        {
            touObj = new TouchObject(scriptTouch);
        }

        if (showname)
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
        if (showname)
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
        EventController.current.ObjectTriggerEnter(this.name);

        if (intObj != null)
        {
            intObj.Interact();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        EventController.current.ColliderTriggerEnter(this.name);
        if (touObj != null)
        {
            touObj.Touch();
        }
    }
}
