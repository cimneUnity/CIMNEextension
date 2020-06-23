using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    public static CameraManager current;

    public bool interact = false;
    public float mouseSensitivity = 200f;
    public float maxDistance = 7f;
    public Transform playerBody;

    private bool controlling = true;
    private bool seeingObject = false;
    private float xRotation = 0f;
    private RaycastHit hit;
    private GameObject newObjectLooking, oldObjectLooking;

    private void Awake()
    {
        current = this;
    }
    void Start()
    {
        Debug.Log("CameraManager Start");
        controlling = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (controlling)
        {
            MouseControl();
            CloseObjects();
            MouseButtons();
        }
    }

    void MouseControl()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime; ;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        playerBody.Rotate(Vector3.up * mouseX);
    }

    void MouseButtons()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (oldObjectLooking != null)
            {
                oldObjectLooking.GetComponent<ObjectBehaviour>().Interact();
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (oldObjectLooking != null)
            {
                EventController.current.ShowDescription();
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (oldObjectLooking != null)
            {
                EventController.current.ExtendDescription();
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (oldObjectLooking != null)
            {
                EventController.current.ChangeMode("table", oldObjectLooking.name);
            } else
            {
                EventController.current.ChangeMode("table", null);
            }
        }       
    }

    void CloseObjects()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, maxDistance))
        {
            newObjectLooking = hit.collider.gameObject;
            if (newObjectLooking.GetComponent<ObjectBehaviour >() != null)
            {
                if (oldObjectLooking == null || oldObjectLooking.name != newObjectLooking.name)
                {
                    if (newObjectLooking.GetComponent<ObjectBehaviour>().showDescription) {
                      EventController.current.SeeObject(newObjectLooking.name, newObjectLooking.GetComponent<ObjectBehaviour>().description);  
                    }
                    oldObjectLooking = newObjectLooking;
                    seeingObject = true;
                }
            }
        }
        else if (seeingObject)
        {
            EventController.current.UnseeObject();
            seeingObject = false;
            oldObjectLooking = null;
        }
    }

    public void Activate()
    {
        controlling = true;
    }

    public void Deactivate()
    {
        controlling = false;
    }
}