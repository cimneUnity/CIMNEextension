using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Condition : MonoBehaviour
{
    protected StorytellController.ConditionStruct cond;
    public abstract void AddData(StorytellController.ConditionStruct tmpCond);
    protected void Compleated()
    {
        GameObject.Find("Controller").GetComponent<StorytellController>().conditionCompleted(cond.nextId);
        Destroy(this);
    }
}

public class InteractCondition : Condition
{
    public override void AddData(StorytellController.ConditionStruct tmpCond)
    {
        cond = tmpCond;
        EventController.current.onObjectTriggerEnter += CheckName;
    }

    private void CheckName(string name)
    {
        if (cond.target == name)
        {
            Compleated();
        }
    }
}

public class TouchCondition : Condition
{
    public override void AddData(StorytellController.ConditionStruct tmpCond)
    {
        cond = tmpCond;
        EventController.current.onColliderTriggerEnter += CheckName;
    }

    private void CheckName(string name)
    {
        if (cond.target == name)
        {
            Compleated();
        }
    }
}

public class TimeCondition : Condition
{
    private float time;
    public override void AddData(StorytellController.ConditionStruct tmpCond)
    {
        cond = tmpCond;
        time = tmpCond.time;
    }

    void Update()
    {
        time -= Time.deltaTime;
        if (time <= 0.0f) Compleated();
    }
}