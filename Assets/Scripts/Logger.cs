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
    public static void AppendIndividualsAsLaTeX(Individual[] individuals){
            using(StreamWriter sw = File.AppendText("AIlog.txt"))
            {
                int j = 0;
                foreach(Individual i in individuals){
                    sw.WriteLine("\\hline\nIndividuo " + j + "&" + i.Angle + "&" + i.Wait + "&" + i.Power + "&" + i.Fitness + "\\\\");
                    j++;
                }
            }
        }
}
