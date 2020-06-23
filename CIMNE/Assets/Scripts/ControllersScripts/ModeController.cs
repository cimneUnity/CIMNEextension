using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeController : MonoBehaviour
{
    public static ModeController current;

    private GameObject playUI, typeUI, finishUI, tableUI, popupUI;
    private GameObject playerCamara;
    private string mode;

    private void Awake()
    {
        current = this;
    }

    void Start()
    {
        Debug.Log("EventController Start");
        EventController.current.onChangeMode += ChangeMode;
        playerCamara = GameObject.Find("PlayerCamera");
        playerCamara.SetActive(true);
        playUI = GameObject.Find("PlayUI");
        typeUI = GameObject.Find("TypeUI");
        finishUI = GameObject.Find("FinishUI");
        popupUI = GameObject.Find("PopupUI");
        Cursor.lockState = CursorLockMode.Locked;
        playUI.SetActive(true);
        popupUI.SetActive(true);
        typeUI.SetActive(false);
        finishUI.SetActive(false);
        //tableUI.SetActive(false);
    }

    private void ChangeMode(string newMode, string extra)
    {
        if (newMode == "play") ActivatePlay();
        else
        {
            popupUI.SetActive(false);
            if (newMode == "type") ActivateType();
            else if (newMode == "finish") ActivateFinish(extra);
        }
        
    }

    void ActivateType()
    {
        mode = "type";
        playUI.SetActive(false);
        typeUI.SetActive(true);
        PlayerManager.current.Deactivate();
        CameraManager.current.Deactivate();
        Cursor.lockState = CursorLockMode.None;
    }

    public void ActivateFinish(string reason)
    {
        mode = "finish";
        playUI.SetActive(false);
        finishUI.SetActive(true);
        finishUI.GetComponent<FinishManager>().SetData(reason);
        PlayerManager.current.Deactivate();
        CameraManager.current.Deactivate();
        Cursor.lockState = CursorLockMode.None;
    }

    public void ActivatePlay()
    {
        mode = "play";
        playUI.SetActive(true);
        popupUI.SetActive(true);
        typeUI.SetActive(false);
        finishUI.SetActive(false);
        PlayerManager.current.Activate();
        CameraManager.current.Activate();
        Cursor.lockState = CursorLockMode.Locked;
    }

    public string GetMode()
    {
        return mode;
    }
}
