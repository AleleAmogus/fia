using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticAlgorithm : MonoBehaviour
{
    public static float maxWait = 4f;

    int population = 4;
    Individual[] individuals;
    Palla palla;

    // Start is called before the first frame update
    void Start()
    {
        palla = FindObjectOfType<Palla>();
        individuals = new Individual[population];
        Initialize();
        StartCoroutine(Execute());
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.X)){
            int scale = (int) Time.timeScale;
            if(scale >4)
                Time.timeScale = 1;
            else
                Time.timeScale *= 2;
        }
    }

    void Initialize(){
        for(int i = 0; i < population; i++){
            individuals[i] = new Individual(Random.Range(0f, 360f), Random.Range(0f, maxWait), Random.Range(0f, Palla.maxPower));
        }
    }

    IEnumerator Execute(){
        while(!Input.GetKey(KeyCode.S)){
        foreach(Individual i in individuals){
                palla.SetRotation(i.Angle);
                yield return new WaitForSeconds(i.Wait);
                palla.SwitchToSelectingPower();
                yield return new WaitForSeconds(i.Power/palla.GetPowerIncreaseSpeed());
                palla.SwitchToShot();
                yield return new WaitUntil(() => palla.GetGameState() == GameState.SelectingDirection);
                i.Fitness = palla.GetPerformanceIndicator();
            }
            Selection();
            Crossover();
            Mutation();
        }
    }
    //Roulette Wheel
    void Selection(){
        Debug.Log("\nSTARTING SELECTION\n" + individuals[0].ToString() + "\n" + individuals[1].ToString() + "\n" + individuals[2].ToString() + "\n" + individuals[3].ToString());
        Individual[] temp = new Individual[population];
        /*DEBUG*/temp[0] = individuals[0];temp[1] = individuals[1];temp[2] = individuals[2];temp[3] = individuals[3];
        float sum = 0f;
        foreach(Individual i in individuals)
            sum += i.Fitness;
        for(int j = 0; j < population; j++){
            float acc = 0f;
            float val = Random.Range(0f, sum);
            foreach(Individual i in individuals){
                acc += i.Fitness;
                if(val <= acc){
                    temp[j] = (Individual) i.Clone();
                    break;
                }
            }
            Debug.Log("\nITERATION NUMBER \n" + j +temp[0].ToString() + "\n" + temp[1].ToString() + "\n" + temp[2].ToString() + "\n" + temp[3].ToString());
        }
        individuals = temp;
    }

    void Crossover(){
        Debug.Log("\nSTARTING CROSSOVER\n" + individuals[0].ToString() + "\n" + individuals[1].ToString() + "\n" + individuals[2].ToString() + "\n" + individuals[3].ToString());
        for(int i = 0; i < population-1; i++){
            int val = Random.Range(0, 3);
            if(val == 0){
                float temp = individuals[i].Angle;
                individuals[i].Angle = individuals[i+1].Angle;
                individuals[i+1].Angle = temp;
            }else if(val == 1){
                float temp = individuals[i].Wait;
                individuals[i].Wait = individuals[i+1].Wait;
                individuals[i+1].Wait = temp;
            }else{
                float temp = individuals[i].Power;
                individuals[i].Power = individuals[i+1].Power;
                individuals[i+1].Power = temp;
            }
        Debug.Log("\nITERATION NUMBER \n" + i +individuals[0].ToString() + "\n"+ individuals[1].ToString() + "\n" + individuals[2].ToString() + "\n" + individuals[3].ToString());
        }
    }

    void Mutation(){
        Debug.Log("\nSTARTING MUTATION\n" + individuals[0].ToString() + "\n" + individuals[1].ToString() + "\n" + individuals[2].ToString() + "\n" + individuals[3].ToString());
        foreach(Individual i in individuals){
            int val = Random.Range(0, 3);
            if(val == 0){
                float amount = Random.Range(-10f, 10f);
                i.Angle += amount;
            }else if(val == 1){
                float amount = Random.Range(-0.5f, 0.5f);
                i.Wait += amount;
            }else{
                float amount = Random.Range(-0.3f, 0.3f);
                i.Power += amount;
            }
        }
        Debug.Log("\nAFTER MUTATION\n" +individuals[0].ToString() + "\n"+ individuals[1].ToString() + "\n" + individuals[2].ToString() + "\n" + individuals[3].ToString());
    }
}
