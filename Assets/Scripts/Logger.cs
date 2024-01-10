using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Logger
{
    static int subOptimals = 0;
    static int solutionsAcc = 0;
    static int solutionsCount = 0;

    public static void AddSolution(int gens, bool suboptimal){
        string message;
        if(suboptimal){
            subOptimals++;
            message = "\n\nSub-optimal solution identified\n\n";
        }
        else{
            solutionsAcc+=gens;
            solutionsCount++;
            message = "\n\nCurrent average generations to find a solution: " + (float)solutionsAcc/(float)solutionsCount + "\n\n";
        }
        using(StreamWriter sw = File.AppendText("AIlog.txt"))
                {
                    sw.WriteLine(message);
                    sw.WriteLine("\n\nCurrent sub-optimal solutions: " + subOptimals + "\n\n");
                    sw.WriteLine("\n\nTotal number of solutions: " + (subOptimals+solutionsCount) + "\n\n");
                }
    }

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
