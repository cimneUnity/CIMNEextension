using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishManager : MonoBehaviour
{
    public static FinishManager current;

    public int puntuation = 10;
    private Button restartButton;

    private void Awake()
    {
        current = this;
    }

    void Start()
    {
        Debug.Log("FinishManager Start");

        restartButton = GameObject.Find("FinishButtonRestart").GetComponent<Button>();
        restartButton.onClick.AddListener(() => { Restart(); });
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Application.LoadLevel(0);
        }
    }

    //public void SetData(string reason, int score, bool table, List<GlobalController.TableStruct> globalList)
    public void SetData(string reason)
    {
        GameObject.Find("FinishReasonLabel").GetComponent<UnityEngine.UI.Text>().text = reason;
        int score = GlobalController.current.globalScore;
        GameObject.Find("FinishPuntuationLabel").GetComponent<UnityEngine.UI.Text>().text = "Puntuation: " + score;
    }

    void Restart()
    {
        Application.LoadLevel(0);
    }
}