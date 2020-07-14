using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiskBehaviour : MonoBehaviour
{
    public int importance; //Importance of the accident between 1 to 3
    public int score = 0; //Score penalization to the player everytime it touch the risc
    public bool showName = false; //Show the name on PlayMode
    private GameObject floatingLabel; //Floating Label with the name of the risk

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
        if (score < 0) score = -score;
        score = Mathf.Clamp(score, 0, 9999); // Set the score between 0 and 9999
        importance = Mathf.Clamp(importance, 1, 3); // Set the importance between 1 and 3
    }

    private void OnTriggerEnter(Collider other) //Called whene player enters Collider
    {
        EventController.current.TriggerEnterRisk(importance);
        if (score != 0) GlobalController.current.globalScore -= score;
    }

    private void OnTriggerExit(Collider other) //Called whene player leaves Collider
    {
        EventController.current.TriggerExitRisk(importance);
    }

    private void updateLabelPosition(){ 
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
