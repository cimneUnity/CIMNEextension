using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPathBehaviour : MonoBehaviour
{
    
    public float speed;
    public float rotationSpeed = 0.1f;
    public string pathName;
    public bool accident = false;
    public bool cicle = false;

    private int CurrentWayPointID = 0;
    private bool loop = false;
    private float oldSpeed;
    private float reachDistance = 0.5f;
    private List<Transform> path_objs = new List<Transform>();

    void Start()
    {
        oldSpeed = speed;
        SetPath();
    }

    void Update() // Update is called once per frame
    {

        if (loop)
        {
            float distance = Vector3.Distance(path_objs[CurrentWayPointID].position, transform.position);
            transform.position = Vector3.MoveTowards(transform.position, path_objs[CurrentWayPointID].position, Time.deltaTime * speed);
            var lookPos = path_objs[CurrentWayPointID].position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);

            if (distance <= reachDistance) CurrentWayPointID++;
            if (CurrentWayPointID >= path_objs.Count)
            {
                if (cicle)
                {
                    CurrentWayPointID = 0;
                } else
                {
                    loop = false;
                }
            }
            
        }
    }

    void SetPath()
    {
        Transform child = GameObject.Find(pathName).transform;
        if (child != null)
        {
            path_objs.Clear();
            int children = child.transform.childCount;

            for (int i = 0; i < children; ++i) path_objs.Add(child.transform.GetChild(i));
        }
        if (path_objs.Count != 0) loop = true;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            if (accident)
            {
                string reason = this.name + " run over you";
                EventController.current.ChangeMode("finish", reason);
            }
            else speed = 0;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            new WaitForSeconds(1f);
            speed = oldSpeed;
        }
    }

}
