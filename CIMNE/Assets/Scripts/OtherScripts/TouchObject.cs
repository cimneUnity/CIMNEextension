using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchObject
{
    private string action;
    public TouchObject(string act)
    {
        action = act;
        //Debug.Log("Touch: " + action);
    }

    public void Touch()
    {
        if (action == null)
        {
            //Debug.Log("Touched");
        } else
        {

            if (action == "pain")
            {
                System.Random r = new System.Random();
                int rInt = r.Next(0, 2);
                //Debug.Log("Num: " + rInt);
                if (rInt == 0)
                {
                    //Debug.Log("Pain");
                    GameObject tmpScript = new GameObject();
                    PainScript ps = tmpScript.AddComponent<PainScript>();
                    tmpScript.transform.parent = GameObject.Find("Tmp").transform;
                }
            }
        }
    }
}
