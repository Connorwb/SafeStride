using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Diagnostics;
using System;

public class PythonAPICaller : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        String m_Path = Application.dataPath;
        ProcessStartInfo start = new ProcessStartInfo();
        start.FileName = "C:\\Users\\conno\\AppData\\Local\\Programs\\Python\\Python39\\python.exe";
        start.Arguments = m_Path + "\\Scripts\\OpenStreetMapInfo.py";
        start.UseShellExecute = false;
        start.RedirectStandardOutput = true;
        using (Process process = Process.Start(start))
        {
            using (StreamReader reader = process.StandardOutput)
            {
                string result = reader.ReadToEnd();
                UnityEngine.Debug.Log(result);
            }
        }
    }

}
