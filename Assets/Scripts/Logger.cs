using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Logger : MonoBehaviour
{
    public static void AppendAILog(String content){
        File.AppendText("AIlog.txt", content);
    }
}
