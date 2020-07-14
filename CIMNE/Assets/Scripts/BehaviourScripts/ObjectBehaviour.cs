using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBehaviour : MonoBehaviour
{
    public string description;
    public string scriptInteract = null;
    public string scriptTouch = null;
    public bool showName = false;
    public bool showDescription = true;
    private static GameObject canvas;
    private GameObject floatingLabel; //Floating Label with the name of the risk
    private InteractObject intObj;
    private TouchObject touObj;

    void Start() //Called when start
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

        if (showName) //Create floatingLabel
        {
            canvas = GameObject.Find("PopupUI");
            var popupText = Resources.Load("Prefabs/PopUpObject");
            floatingLabel = (GameObject)Instantiate(popupText);
            floatingLabel.transform.SetParent(canvas.transform);
            floatingLabel.transform.GetComponent<UnityEngine.UI.Text>().text = this.name;
        }
    }

    void Update() //Called every frame
    {
        updateLabelPosition();
    }

    public void Interact() //Called whene player interact with object
    {
        EventController.current.ObjectTriggerEnter(this.name);

        if (intObj != null)
        {
            intObj.Interact();
        }
    }

    void OnTriggerEnter(Collider other) //Called whene player touches object Collider
    {
        EventController.current.ColliderTriggerEnter(this.name);

        if (touObj != null)
        {
            touObj.Touch();
        }
    }

    private void updateLabelPosition()
    {
        if (showName)
        {
            Vector3 screenposition = Camera.main.WorldToScreenPoint(this.transform.position);
            if (screenposition.z >= 0)
            {
                floatingLabel.SetActive(true);
                floatingLabel.transform.position = screenposition;
            }
            else
            {
                floatingLabel.SetActive(false);
            }
        }
    }
}
