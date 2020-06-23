using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractObject
{
    private string action;
    public InteractObject(string act)
    {
        action = act;
    }
    public void Interact()
    {
        if (action == "exemple")
        {
            Debug.Log("Interacted");
        }
    }
}

