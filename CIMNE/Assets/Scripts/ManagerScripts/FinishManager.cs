using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinishManager : MonoBehaviour
{
    public static FinishManager current;
    public int puntuation = 10;
    private Button restartButton;

    private void Awake() //Called when awake
    {
        current = this;
    }

    void Start() //Called when start
    {
        Debug.Log("FinishManager Start");

        restartButton = GameObject.Find("FinishButtonRestart").GetComponent<Button>();
        restartButton.onClick.AddListener(() => { Restart(); });
    }

    void Update() //Called every frame
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            /*
            SceneManager.LoadScene()
            Application.LoadLevel(0);
            */

            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
            Restart();

        }
    }

    public void SetData(string reason)
    {
        GameObject.Find("FinishReasonLabel").GetComponent<UnityEngine.UI.Text>().text = reason;
        int score = GlobalController.current.globalScore;
        GameObject.Find("FinishPuntuationLabel").GetComponent<UnityEngine.UI.Text>().text = "Puntuation: " + score;
    }

    private void Restart()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}