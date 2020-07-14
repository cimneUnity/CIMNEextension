using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    public static CameraManager current;

    public float mouseSensitivity = 200f;
    public float maxDistance = 7f;
    public Transform playerBody;

    private bool controlling = true;
    private bool seeingObject = false;
    private float xRotation = 0f;
    private RaycastHit hit;
    private GameObject newObjectLooking, oldObjectLooking;

    private void Awake() //Called when awake
    {
        current = this;
    }
    void Start() //Called when awake
    {
        //Debug.Log("CameraManager Start");
        controlling = true;
    }

    void Update() //Called every frame
    {
        if (controlling)
        {
            MouseControl(); //Mouse movement input
            CloseObjects(); //Detect close objects
            MouseButtons(); //Mouse buttons inputs
        }
    }

    void OnValidate()   //It's called every time you change public values on the Inspector
    {
        mouseSensitivity = Mathf.Clamp(mouseSensitivity, 1.0f, 9999.0f);
        maxDistance = Mathf.Clamp(maxDistance, 1.0f, 30.0f);
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