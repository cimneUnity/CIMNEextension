using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorytellController : MonoBehaviour
{

    public Dictionary<int, StepStruct> steps = new Dictionary<int, StepStruct>();
    public List<StepStruct> listSteps = new List<StepStruct>();
    private GameObject triggers;
    private bool endNode = false;

    [System.Serializable]
    public struct ConditionStruct
    {
        public int nextId;
        public string type;
        public int time;
        public string target;

        public int popupPos;
        public Object objTarget;
        public Color color;
    }

    [System.Serializable]
    public struct ConsequenceStruct
    {
        public string type; //"Add points", "Lose points", "Win", "Lose", "Activate Obj.", "Deactivate Obj.", "Activate Comp.", "Deactivate Cmp"
        public int integer;
        public string primaryAtribute;
        public string secondaryAtribute;

        public int popupPos;
        public Object objectAtr;
    }

    [System.Serializable]
    public struct StepStruct
    {
        public int id;
        public int parentId;
        public float heightWindow;
        public string nameStep;
        public string descriptionStep;
        public Rect rectangle;
        public List<ConsequenceStruct> consequences;
        public List<ConditionStruct> conditions;
    }

    void Start()
    {
        triggers = GameObject.Find("Triggers");
        foreach (StepStruct subStep in listSteps) steps.Add(subStep.id, subStep);
        if (steps.Count != 0) initializeStep(0);
    }

    public void conditionCompleted(int nextStp)
    {
        for (int i = 0; i < triggers.transform.childCount; i++) Destroy(triggers.transform.GetChild(i));
        initializeStep(nextStp);
    }

    private void initializeStep(int idStp)
    {
        StepStruct currentStep = steps[idStp];
        List<ConsequenceStruct> consequences = currentStep.consequences;

        foreach (ConsequenceStruct cons in consequences) initializeConsequence(cons);

        if (!endNode)
        {
            if (currentStep.conditions.Count != 0)
            {
                List<ConditionStruct> conditions = currentStep.conditions;
                foreach (ConditionStruct cond in conditions) initializeCondition(cond);
            }
            
            GameObject.Find("TaskName").GetComponent<UnityEngine.UI.Text>().text = currentStep.nameStep;
            GameObject.Find("TaskDescription").GetComponent<UnityEngine.UI.Text>().text = currentStep.descriptionStep;
        }
    }

    private void initializeCondition(ConditionStruct tempStruct)
    {
        GameObject newCond = new GameObject();
        newCond.name = tempStruct.type;
        Condition tmpCond;
        if (tempStruct.type == "Timer")
        {
            tmpCond = newCond.AddComponent<TimeCondition>();
            tmpCond.AddData(tempStruct);
        }
        if (tempStruct.type == "Interact") 
        {
            tmpCond = newCond.AddComponent<InteractCondition>();
            tmpCond.AddData(tempStruct);
        } 
        if (tempStruct.type == "Touch")
        {
            tmpCond = newCond.AddComponent<TouchCondition>();
            tmpCond.AddData(tempStruct);
        }

        newCond.transform.parent = GameObject.Find("Tmp").transform;
    }

    private void initializeConsequence(ConsequenceStruct tempStruct)
    {
        if (tempStruct.type == "Add points")
        {
            GlobalController.current.globalScore += tempStruct.integer;

        } else if (tempStruct.type == "Lose points")
        {
            GlobalController.current.globalScore -= tempStruct.integer;

        } else if (tempStruct.type == "Win")
        {
            endNode = true;
            string reason = "You finished succesfully the simulation: " + tempStruct.secondaryAtribute;
            EventController.current.ChangeMode("finish", reason);

        } else if (tempStruct.type == "Lose")
        {
            endNode = true;
            string reason = "You failed the simulation: " + tempStruct.secondaryAtribute;
            EventController.current.ChangeMode("finish", reason);

        } else if (tempStruct.type == "Activate Obj." || tempStruct.type == "Deactivate Obj.")
        {
            GameObject.Find(tempStruct.primaryAtribute).SetActive(tempStruct.type == "Activate Obj.");

        } else if (tempStruct.type == "Activate Comp." || tempStruct.type == "Deactivate Cmp")
        {
            Component cmp = GameObject.Find(tempStruct.primaryAtribute).GetComponent(tempStruct.secondaryAtribute);
            Behaviour bhvr = (Behaviour)cmp;
            bhvr.enabled = tempStruct.type == "Activate Comp.";
        }
    }
}
