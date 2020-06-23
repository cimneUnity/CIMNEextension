using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;

public class WritterController: MonoBehaviour
{
    public static WritterController current;
    public string fileName;
    public string path;
    private string pathTxt;
    private string text;
    private string date;

    private void Awake()
    {
        current = this;
    }

    void Start()
    {
        if (path == null)
        {
            Debug.Log("Path null");
            path = "Assets/Resources/Data";
        }
        
        if (!Directory.Exists(path))
        {
            Debug.Log("Path don't exist");
            path = "Assets/Resources/Data";
        }

        if (fileName == null)
        {
            Debug.Log("Name null");
            fileName = "Data";
        }
        pathTxt = path + "/" + fileName + ".txt";
        
        if (!File.Exists(pathTxt))
        {
            File.CreateText(pathTxt);
        }

        Debug.Log(pathTxt);
        //File.WriteAllText(pathTxt, String.Empty);
        StreamWriter writer = new StreamWriter(pathTxt, true);
        date = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        writer.WriteLine("New Execution (" + date + "):");
        writer.Close();
        EventController.current.onWrite += Write;
        //Debug.Log("Erased");
    }

    public void Write(string textN)
    {
        text = textN;
        WriteTxt();
        Debug.Log("writed!");
    }

    public void WriteTxt()
    {
        date = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        StreamWriter writer = new StreamWriter(pathTxt, true);
        writer.WriteLine(date + ": " + text);
        writer.Close();
        //Debug.Log("writed!");
    }
}
