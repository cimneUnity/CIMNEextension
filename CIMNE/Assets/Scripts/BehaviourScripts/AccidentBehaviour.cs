using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class AccidentBehaviour : MonoBehaviour
{
    public int probability;
    public bool showName;

    private static GameObject canvas;
    private GameObject instance;

    void Start()
    {
        if (showName)
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
        if (showName)
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

}
