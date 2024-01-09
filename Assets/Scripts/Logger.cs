using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Logger : MonoBehaviour
{
    public static void AppendAILog(string content){
        using(StreamWriter sw = File.AppendText("AIlog.txt"))
        {
            sw.WriteLine(content);
        }
    }
}
