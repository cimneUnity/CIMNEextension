using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPathBehaviour : MonoBehaviour
{
    
    public float movementSpeed = 5.0f; //Speed of movement
    public float rotationSpeed = 0.1f; //Speed rotation of movement
    public string pathName; //Path name to follow
    public bool isAccident = false; //If the objects runs over player an accident happens
    public bool isCicle = false; //The path it's a circle

    private int CurrentWayPointID = 0;
    private bool loop = false;
    private float oldSpeed;
    private float reachDistance = 0.5f;
    private List<Transform> path_objs = new List<Transform>();

    void Start()
    {
        oldSpeed = movementSpeed;
        SetPath();
    }

    void Update() // Update is called once per frame
    {
        if (loop)
        {
            float distance = Vector3.Distance(path_objs[CurrentWayPointID].position, transform.position);
            transform.position = Vector3.MoveTowards(transform.position, path_objs[CurrentWayPointID].position, Time.deltaTime * movementSpeed);
            var lookPos = path_objs[CurrentWayPointID].position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);

            if (distance <= reachDistance) CurrentWayPointID++;
            if (CurrentWayPointID >= path_objs.Count)
            {
                if (isCicle)
                {
                    CurrentWayPointID = 0;
                } else
                {
                    loop = false;
                }
            }
        }
    }

    void OnValidate()   //It's called every time you change public values on the Inspector
    {
        movementSpeed = Mathf.Clamp(movementSpeed, 0.0f, 999.0f); // Set the movementSpeed between 0.0f and 999.0f
        rotationSpeed = Mathf.Clamp(rotationSpeed, 0.0f, 999.0f); // Set the rotationSpeed between 0.0f and 999.0f

    }

    void SetPath()
    {
        if (pathName != null)
        {
            GameObject path = GameObject.Find(pathName);
            if (path != null)
            {
                Transform child = path.transform;
                if (child != null)
                {
                    path_objs.Clear();
                    int children = child.transform.childCount;
                    for (int i = 0; i < children; ++i) path_objs.Add(child.transform.GetChild(i));
                    if (path_objs.Count != 0) loop = true;
                }
            }
            else
            {
                Debug.Log("GameObject " + name + " with FollowPathBehaviour.cs has a invalid path assigned");
            }
        }
        else
        {
            Debug.Log("GameObject " + name + " with FollowPathBehaviour.cs has no path assigned");
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            if (isAccident)
            {
                string reason = this.name + " run over you";
                EventController.current.ChangeMode("finish", reason);
            }
            else movementSpeed = 0;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            new WaitForSeconds(1f);
            movementSpeed = oldSpeed;
        }
    }
}
