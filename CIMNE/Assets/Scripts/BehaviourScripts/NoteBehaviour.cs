using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteBehaviour : MonoBehaviour
{
    private GameObject floatingLabel;

    void Start() //Called when start
    {
        GameObject canvas = GameObject.Find("PopupUI");
        var popupText = Resources.Load("Prefabs/PopUpObject");
        floatingLabel = (GameObject)Instantiate(popupText);
        floatingLabel.transform.SetParent(canvas.transform);
        floatingLabel.transform.GetComponent<UnityEngine.UI.Text>().text = this.name;
    }

    void Update() //Called every frame
    {
        updateLabelPosition();
    }

    private void updateLabelPosition()
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