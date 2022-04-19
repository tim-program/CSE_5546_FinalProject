using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DebugText : MonoBehaviour
{
    public Text displayText;
    public static List<string> DebugLog;

    // Start is called before the first frame update
    void Start()
    {
        // displayText = GetComponent<Text>();
        DebugLog = new List<string>();
        Application.logMessageReceived += HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (DebugLog.Count == 10)
        {
            DebugLog.RemoveAt(0);
        }

        Debug.Log(logString);
        DebugLog.Add(logString);
        Debug.Log(DebugLog.Count);
        displayText.text = "";
        foreach (string s in DebugLog)
        {
            displayText.text += s + "\n";
        }
    }
}
