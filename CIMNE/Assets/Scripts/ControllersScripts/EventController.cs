using System;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventController : MonoBehaviour
{
    public static EventController current;

    private void Awake() //Called when awake
    {
        current = this;
        //Debug.Log("EventController Start");
    }

    public event Action<string> onObjectTriggerEnter;
    public void ObjectTriggerEnter(string id)
    {
        //Debug.Log("Interact: " + id);
        if (onObjectTriggerEnter != null)
        {
            onObjectTriggerEnter(id);
        }
    }

    public event Action<string> onColliderTriggerEnter;
    public void ColliderTriggerEnter(string id)
    {
        //Debug.Log("Touch: " + id);
        if (onColliderTriggerEnter != null)
        {
            onColliderTriggerEnter(id);
        }
    }

    public event Action<string, string> onChangeMode;
    public void ChangeMode(string mode, string extra)
    {
        //Debug.Log("Mode: " + mode);
        if (onChangeMode != null)
        {
            onChangeMode(mode, extra);
        }
    }

    public event Action<string> onWrite;
    public void Write(string text)
    {
        //Debug.Log("Write: " + text);
        if (onWrite != null)
        {
            onWrite(text);
        }
    }

    public event Action<string, string> onSeeObject;
    public void SeeObject(string name, string description)
    {
        //Debug.Log("Name Obj: " + name + " Desc Obj: " + description);
        if (onSeeObject != null)
        {
            onSeeObject(name, description);
        }
    }

    public event Action onUnseeObject;
    public void UnseeObject()
    {
        //Debug.Log("Unsee Object");
        if (onUnseeObject != null)
        {
            onUnseeObject();
        }
    }

    public event Action onShowDescription;
    public void ShowDescription()
    {
        //Debug.Log("Show Description");
        if (onShowDescription != null)
        {
            onShowDescription();
        }
    }

    public event Action onExtendDescription;
    public void ExtendDescription()
    {
        //Debug.Log("Extend Description");
        if (onExtendDescription != null)
        {
            onExtendDescription();
        }
    }

    public event Action <int> onTriggerEnterRisk;
    public void TriggerEnterRisk(int importance)
    {
        //Debug.Log("EnterRisk");
        if (onTriggerEnterRisk != null)
        {
            onTriggerEnterRisk(importance);
        }
    }

    public event Action<int> onTriggerExitRisk;
    public void TriggerExitRisk(int importance)
    {
        //Debug.Log("EnterRisk");
        if (onTriggerExitRisk != null)
        {
            onTriggerExitRisk(importance);
        }
    }
}
