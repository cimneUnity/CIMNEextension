using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class AccidentBehaviour : MonoBehaviour
{
    public int probability; //Probability of the accident between 0 - 100
    public bool showName; //Show the name on PlayMode
    private GameObject floatingLabel; //Floating Label with the name of the accident

    void Start() //Called when start
    {
        if (showName) //Create floatingLabel
        {
            GameObject canvas = GameObject.Find("PopupUI");
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

    void OnValidate()   //It's called every time you change public values on the Inspector
    {
        probability = Mathf.Clamp(probability, 1, 100); // Set the score between 0 and 9999
    }

    void OnTriggerEnter(Collider other)
    {
        System.Random r = new System.Random();
        int rInt = r.Next(0, 100);
        Debug.Log("A");
        if (probability > rInt) { 
            Debug.Log("Dead");
            EventController.current.ChangeMode("finish", "You get burn");
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
