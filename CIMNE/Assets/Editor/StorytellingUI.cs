using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class StorytellingUI : EditorWindow
{
    [SerializeField] private int savedId = 0; //We need to serialize the variables and structs we want to store after we start playmode
    private int temporalId = 0;
    private float width = 300f;
    private float height = 110f;
    private float preWidth = 150f;
    private float preHeight = 70f;
    private float heightElements = 20f;
    private string text = "Create your Storytell:";

    private Color[] colors = new Color[] //Set of colors of lines
        {
        Color.red, Color.blue, Color.green, Color.yellow, Color.magenta, Color.cyan, Color.black
        };

    private string[] optionsConsequences = new string[] //Set of consequences options
        {
            "Add points", "Lose points", "Win", "Lose", "Activate Obj.", "Deactivate Obj.", "Activate Comp.", "Deactivate Cmp",
        };

    private string[] optionsConditions = new string[] //Set of consitions options
        {
        "Timer", "Interact", "Touch",
        };

    [System.Serializable]
    public struct ConditionStructUI //Struct of conditions
    {
        public int popupPos;
        public int nextId;
        public int time;
        public Object target;
        public Color color;
    }

    [System.Serializable]
    public struct ConsequenceStructUI //Struct of consequences
    {
        public int popupPos;
        public int integer;
        public Object objectAtr;
        public string primString;
    }

    [System.Serializable]
    public struct NodeStruct //Struct of node
    {
        public float heightWindow;
        public int nodeId;
        public int parentId;
        public string nodeName;
        public string nodeDescription;
        public List <ConditionStructUI> conditions;
        public List <ConsequenceStructUI> consequences;
        public Rect rectangle;
        public int consequenceId;
        public int conditionId;
    }

    //Struct of prenode before a new reference is created
    public struct PrenodeStruct
    {
        public int nodeId;
        public int parentId;
        public int futureId;
        public Rect rectangle;
        public int condId;
    }

    private Dictionary<int, NodeStruct> nodeSet = new Dictionary<int, NodeStruct>();
    private Dictionary<int, PrenodeStruct> prenodeSet = new Dictionary<int, PrenodeStruct>();
    private List<NodeStruct> listNode = new List<NodeStruct>();

    [MenuItem("Cimne/Storytell Window")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(StorytellingUI));
    }

    void OnGUI()
    {

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(text);
        if (GUILayout.Button("Restart")) 
        {
            temporalId = 0;
            nodeSet = new Dictionary<int, NodeStruct>();
            prenodeSet = new Dictionary<int, PrenodeStruct>();
            //listNode = new List<NodeStruct>();
            CreateFirst();
        }

        if (GUILayout.Button("Recover"))
        {
            if (listNode.Count > 0)
            {
                ListToDictionary();
            } else {
                
                StorytellController smg = GameObject.Find("Controller").GetComponent<StorytellController>() as StorytellController;
                List<StorytellController.StepStruct> stepList = smg.listSteps;
                if (stepList.Count > 0)
                {
                    foreach (StorytellController.StepStruct scTmp in stepList)
                    {
                        NodeStruct nsTmp = new NodeStruct();
                        nsTmp.nodeId = scTmp.id;
                        nsTmp.nodeName = scTmp.nameStep;
                        nsTmp.nodeDescription = scTmp.descriptionStep;
                        nsTmp.rectangle = scTmp.rectangle;
                        nsTmp.heightWindow = scTmp.heightWindow;
                        nsTmp.parentId = scTmp.parentId;
                        
                        List<ConditionStructUI> conditions = new List<ConditionStructUI>();
                        nsTmp.conditionId = 0;
                        foreach (StorytellController.ConditionStruct cdTmp in scTmp.conditions) 
                        {
                            ConditionStructUI tmpC = new ConditionStructUI();
                            tmpC.nextId = cdTmp.nextId;
                            tmpC.popupPos = cdTmp.popupPos;
                            tmpC.time = cdTmp.time;
                            tmpC.target = cdTmp.objTarget;
                            tmpC.color = cdTmp.color;
                            nsTmp.conditionId++;
                            conditions.Add(tmpC);
                        }
                        nsTmp.conditions = conditions;

                        List<ConsequenceStructUI> consequences = new List<ConsequenceStructUI>();
                        nsTmp.consequenceId = 0;
                        foreach (StorytellController.ConsequenceStruct csTmp in scTmp.consequences)
                        {
                            ConsequenceStructUI tmpC = new ConsequenceStructUI();
                            tmpC.popupPos = csTmp.popupPos;
                            tmpC.integer = csTmp.integer;
                            tmpC.objectAtr = csTmp.objectAtr;
                            tmpC.primString = csTmp.secondaryAtribute;
                            nsTmp.consequenceId++;
                            consequences.Add(tmpC);
                        }
                        nsTmp.consequences = consequences;

                        listNode.Add(nsTmp);
                    }
                    ListToDictionary();

                } else
                {
                    text = "Create your Storytell: No storytell saved";
                }
            }
        }

        if (nodeSet.Count == 0)
        {
            temporalId = 0;
        }

        List<int> keysN = new List<int>(nodeSet.Keys); //We are going to iterate over dictionary keys

        if (GUILayout.Button("Save"))
        {
            if (prenodeSet.Count == 0) //All prenodes assigned
            { 
                savedId = temporalId;
                List<StorytellController.StepStruct> stepList = new List<StorytellController.StepStruct>();
                listNode = new List<NodeStruct>(); //Set listNode values, because it's Serilizable
                
                foreach (int key in keysN)
                {
                    NodeStruct node = nodeSet[key];
                    listNode.Add(node);
                    StorytellController.StepStruct tmpStep = new StorytellController.StepStruct();
                    tmpStep.id = node.nodeId;
                    tmpStep.nameStep = node.nodeName;
                    tmpStep.descriptionStep = node.nodeDescription;
                    tmpStep.rectangle = node.rectangle;
                    tmpStep.heightWindow = node.heightWindow;
                    tmpStep.parentId = node.parentId;
                    List<StorytellController.ConsequenceStruct> consList = new List<StorytellController.ConsequenceStruct>();
                    
                    foreach (ConsequenceStructUI cons in node.consequences)
                    {
                        StorytellController.ConsequenceStruct tcons = new StorytellController.ConsequenceStruct();
                        tcons.type = optionsConsequences[cons.popupPos];
                        tcons.integer = cons.integer;
                        string TobjArt;

                        if (cons.objectAtr == null)
                        {
                            TobjArt = "null";
                        } else
                        {
                            TobjArt = cons.objectAtr.name;
                        }

                        tcons.primaryAtribute = TobjArt;
                        tcons.secondaryAtribute = cons.primString;
                        //New
                        tcons.popupPos = cons.popupPos;
                        tcons.objectAtr = cons.objectAtr;

                        consList.Add(tcons);
                    }

                    tmpStep.consequences = consList;
                    List<StorytellController.ConditionStruct> condList = new List<StorytellController.ConditionStruct>();
                    
                    foreach (ConditionStructUI cond in node.conditions)
                    {
                        StorytellController.ConditionStruct tcond = new StorytellController.ConditionStruct();
                        tcond.nextId = cond.nextId;
                        tcond.type = optionsConditions[cond.popupPos];
                        tcond.time = cond.time;
                        string target;

                        if (cond.target == null)
                        {
                            target = "null";
                        } else
                        {
                            target = cond.target.name;
                        }

                        tcond.target = target;
                        //New
                        tcond.popupPos = cond.popupPos;
                        tcond.objTarget = cond.target;
                        tcond.color = cond.color;

                        condList.Add(tcond);
                    }

                    tmpStep.conditions = condList;
                    stepList.Add(tmpStep);
                }
                StorytellController smg = GameObject.Find("Controller").GetComponent<StorytellController>() as StorytellController;
                smg.listSteps = stepList;
                text = "Create your Storytell:";
            } else //Still some prenodes without assign
            {
                text = "Create your Storytell: Assign all steps";
            }
        }

        EditorGUILayout.EndHorizontal();
        BeginWindows();

        foreach (int key in keysN)
        {
            NodeStruct node = nodeSet[key];

            foreach (ConditionStructUI cond in node.conditions) 
            {
                int chId = cond.nextId;
                if (nodeSet.ContainsKey(chId) || prenodeSet.ContainsKey(chId))
                {
                    DrawLine(chId, node.nodeId, cond.color);
                }
            }

            node.rectangle = SetRectangle(node.rectangle);
            node.rectangle = GUI.Window(node.nodeId, node.rectangle, WindowFunction, "" + node.nodeId);
            node.rectangle.height = node.heightWindow;
            nodeSet[key] = node;
        }

        List<int> keysP = new List<int>(prenodeSet.Keys);

        foreach (int key in keysP)
        {
            PrenodeStruct prenode = prenodeSet[key];
            prenode.rectangle = SetRectangle(prenode.rectangle);
            prenode.rectangle = GUI.Window(prenode.nodeId, prenode.rectangle, WindowFunctionPreNode, "Create Node");
            prenodeSet[key] = prenode;
        }

        EndWindows();
    }

    void ListToDictionary()
    {
        temporalId = savedId;
        nodeSet = new Dictionary<int, NodeStruct>();
        prenodeSet = new Dictionary<int, PrenodeStruct>();

        foreach (NodeStruct node in listNode) nodeSet.Add(node.nodeId, node);
    }

    Rect SetRectangle(Rect rectangle) //Set rectangle position if its out of window
    {
        if (rectangle.xMin < 0) rectangle.x = 0;
        if (rectangle.yMin < 22) rectangle.y = 22;
        if (rectangle.xMax > position.width) rectangle.x = rectangle.x - (rectangle.xMax - position.width);
        if (rectangle.yMax > position.height) rectangle.y = rectangle.y - (rectangle.yMax - position.height);
        return rectangle;
    }

    void CreateFirst() //Create first Step/Node
    {
        NodeStruct newNode = new NodeStruct();
        newNode.heightWindow = 110f;
        newNode.nodeId = temporalId;
        newNode.parentId = -1;
        newNode.nodeName = "Name Step";
        newNode.nodeDescription = "Description Step";
        newNode.conditionId = 0;
        newNode.consequenceId = 0;
        newNode.rectangle = new Rect(10f, 20f, width, height); //Initialize first windows on pos (10f, 20f)
        newNode.conditions = new List <ConditionStructUI>();
        newNode.consequences = new List <ConsequenceStructUI>();
        nodeSet.Add(newNode.nodeId, newNode);
    }

    void CreatePrenode(int parentId, int prenodeId, int precond) //Create Prenode
    {
        NodeStruct parent = nodeSet[parentId];
        Rect parentRect = parent.rectangle;
        PrenodeStruct newPrenode = new PrenodeStruct();
        newPrenode.nodeId = prenodeId;
        newPrenode.parentId = parent.nodeId;
        newPrenode.futureId = parent.nodeId;
        newPrenode.condId = precond;
        float nX = parentRect.x + width + width / 4;
        float nY = parentRect.y;
        newPrenode.rectangle = new Rect(nX, nY, preWidth, preHeight);
        prenodeSet.Add(newPrenode.nodeId, newPrenode);
    }

    void CreateNode(int parentId, int prenodeId)
    {
        NodeStruct parent = nodeSet[parentId];
        Rect parentRect = parent.rectangle;
        NodeStruct newNode = new NodeStruct();
        newNode.heightWindow = 110f;
        newNode.nodeId = prenodeId;
        newNode.parentId = parent.nodeId;
        newNode.nodeName = "New Node";
        newNode.nodeDescription = "New Description";
        newNode.conditionId = 0;
        newNode.consequenceId = 0;
        float nX = prenodeSet[prenodeId].rectangle.x;
        float nY = prenodeSet[prenodeId].rectangle.y;
        newNode.rectangle = new Rect(nX, nY, width, height);
        newNode.conditions = new List<ConditionStructUI>();
        newNode.consequences = new List <ConsequenceStructUI>();
        nodeSet.Add(newNode.nodeId, newNode);
        prenodeSet.Remove(prenodeId);
    }

    void DrawLine(int childId, int parentId, Color tmpCol)
    {        
        Rect parentRect = nodeSet[parentId].rectangle;

        if (parentId != childId)
        {
            Rect childRect;

            if (prenodeSet.ContainsKey(childId)) childRect = prenodeSet[childId].rectangle;
            else if (nodeSet.ContainsKey(childId)) childRect = nodeSet[childId].rectangle;
            else
            {
                nodeSet[parentId].conditions.RemoveAt(childId - 1);
                return;
            }

            Handles.BeginGUI();
            int curve = 6;
            float xdist = childRect.xMin - parentRect.xMax;
            if (xdist < 0)
            {
                curve = 2;
                xdist = -xdist;
            }

            float ydist = childRect.center.y - parentRect.center.y;

            Handles.DrawBezier(
                new Vector2(parentRect.xMax, parentRect.center.y),
                new Vector2(childRect.xMin, childRect.center.y),
                new Vector2(parentRect.xMax + xdist /2, parentRect.center.y + ydist / curve),
                new Vector2(childRect.xMin - xdist/2, childRect.center.y - ydist / curve),
                tmpCol, null, 5f);

            Handles.EndGUI();
        } else
        {
            Handles.BeginGUI();

            float xdist = parentRect.xMax - parentRect.xMin;
            float ydist = parentRect.yMax - parentRect.yMin;

            Handles.DrawBezier(
                new Vector2(parentRect.xMax, parentRect.center.y),
                new Vector2(parentRect.xMin, parentRect.center.y),
                new Vector2(parentRect.xMax + xdist/2, parentRect.yMax + ydist/2),
                new Vector2(parentRect.xMin - xdist/2, parentRect.yMax + ydist/2),
                tmpCol, null, 5f);

            Handles.EndGUI();
        }

    }

    void WindowFunction(int windowID)
    {
        if (nodeSet.ContainsKey(windowID))
        {
            NodeStruct tmpNode = nodeSet[windowID];
            GUI.DragWindow(new Rect(0, 0, width, heightElements));

            EditorGUILayout.BeginHorizontal();
            tmpNode.nodeName = EditorGUILayout.TextField(nodeSet[windowID].nodeName);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            tmpNode.nodeDescription = EditorGUILayout.TextField(nodeSet[windowID].nodeDescription);
            EditorGUILayout.EndHorizontal();

            for (int i = 0; i < tmpNode.consequences.Count; i++)
            {
                ConsequenceStructUI cons = tmpNode.consequences[i];
                int oldPos = cons.popupPos;

                EditorGUILayout.BeginHorizontal();
                cons.popupPos = EditorGUILayout.Popup(cons.popupPos, optionsConsequences);
                if (oldPos != cons.popupPos)
                {
                    if (oldPos == 6 || oldPos == 7)
                    {
                        tmpNode.heightWindow -= 20f;
                    }
                    if (cons.popupPos == 6 || cons.popupPos == 7)
                    {
                        tmpNode.heightWindow += 20f;
                        cons.primString = "ComponentName";
                    }

                    if (cons.popupPos == 3 || cons.popupPos == 2)
                    {
                        tmpNode.heightWindow += 20f;
                        cons.primString = "Reason";
                    }

                }

                if (cons.popupPos < 2)
                {
                    cons.integer = EditorGUILayout.IntField(cons.integer); //Timer
                    if (cons.integer < 0) cons.integer = 0;
                }
                else if (cons.popupPos < 4) cons.primString = EditorGUILayout.TextField(cons.primString); //Finish
                else if (cons.popupPos < 6) cons.objectAtr = EditorGUILayout.ObjectField(cons.objectAtr, typeof(Object), true); //Interact
                else if (cons.popupPos < 8)
                {
                    //Touch
                    cons.objectAtr = EditorGUILayout.ObjectField(cons.objectAtr, typeof(Object), true);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    cons.primString = EditorGUILayout.TextField(cons.primString);
                }

                EditorGUILayout.EndHorizontal();
                tmpNode.consequences[i] = cons;
            }

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Add Consequence"))
            {
                ConsequenceStructUI tmpCs = new ConsequenceStructUI();
                tmpCs.popupPos = 0;
                tmpCs.integer = 1;
                tmpCs.objectAtr = null;
                tmpCs.primString = "null2";
                tmpNode.consequences.Add(tmpCs);
                tmpNode.consequenceId = tmpNode.consequenceId + 1;
                tmpNode.heightWindow += 20f;
            }

            EditorGUILayout.EndHorizontal();

            for (int i = 0; i < tmpNode.conditions.Count; i++)
            {
                ConditionStructUI cond = tmpNode.conditions[i];
                GUIStyle gsTest = new GUIStyle();
                gsTest.normal.textColor = colors[i % colors.Length];
                EditorGUILayout.BeginHorizontal();
                cond.popupPos = EditorGUILayout.Popup(cond.popupPos, optionsConditions, gsTest);

                if (cond.popupPos == 0)
                {
                    cond.time = EditorGUILayout.IntField(cond.time);
                    if (cond.time < 0) cond.time = 0;
                }
                else if (cond.popupPos == 1) cond.target = EditorGUILayout.ObjectField(cond.target, typeof(Object), true);
                else if (cond.popupPos == 2) cond.target = EditorGUILayout.ObjectField(cond.target, typeof(Object), true);

                EditorGUILayout.EndHorizontal();
                tmpNode.conditions[i] = cond;
            }

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Add Condition"))
            {
                ConditionStructUI tmpC = new ConditionStructUI();
                tmpC.popupPos = 0;
                tmpC.nextId = temporalId + 1;
                ++temporalId;
                tmpC.time = 1;
                tmpC.target = null;
                tmpNode.consequenceId = tmpNode.conditionId + 1;
                tmpC.color = colors[tmpNode.conditionId % colors.Length];
                tmpNode.conditions.Add(tmpC);
                tmpNode.heightWindow += 20f;
                CreatePrenode(windowID, tmpC.nextId, tmpNode.conditionId);
                tmpNode.conditionId++;
            }

            EditorGUILayout.EndHorizontal();
            nodeSet[windowID] = tmpNode;
        }
    }

    void WindowFunctionPreNode(int windowID)
    {
        bool keep = true;
        PrenodeStruct tmpPreode = prenodeSet[windowID];
        GUI.DragWindow(new Rect(0, 0, preWidth, heightElements));
        EditorGUILayout.BeginHorizontal();
        tmpPreode.futureId = EditorGUILayout.IntField(tmpPreode.futureId);

        if (GUILayout.Button("Reference"))
        {
            if (nodeSet.ContainsKey(tmpPreode.futureId))
            {
                NodeStruct tmpNd = nodeSet[tmpPreode.parentId];
                ConditionStructUI tmpNc = tmpNd.conditions[tmpPreode.condId];
                tmpNc.nextId = tmpPreode.futureId;
                tmpNd.conditions[tmpPreode.condId] = tmpNc;
                nodeSet[tmpPreode.parentId] = tmpNd;
                prenodeSet.Remove(tmpPreode.nodeId);
                keep = false;
            }
        }

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("New"))
        {
            CreateNode(tmpPreode.parentId, tmpPreode.nodeId);
            keep = false;
        } 

        EditorGUILayout.EndHorizontal();

        if (keep)
        {
            prenodeSet[windowID] = tmpPreode;
        }
    }
}