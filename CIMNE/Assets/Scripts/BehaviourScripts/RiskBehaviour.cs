using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiskBehaviour : MonoBehaviour
{
    public int importance;
    public int score;
    public bool viewName = false;

    private static GameObject canvas;
    private GameObject instance;

    void Start()
    {
        if (viewName)
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
        if (viewName)
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
        EventController.current.TriggerEnterRisk(importance);
        if (score != 0) GlobalController.current.globalScore -= score;
    }

    void OnTriggerExit(Collider other)
    {
        EventController.current.TriggerExitRisk(importance);
    }
}
