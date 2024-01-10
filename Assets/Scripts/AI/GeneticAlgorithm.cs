using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticAlgorithm : MonoBehaviour
{
    public static float maxWait = 4f;
    [SerializeField] int maxGens = 12;
    int population = 4;//6
    Individual[] individuals;
    Palla palla;

    // Start is called before the first frame update
    void Start()
    {
        palla = FindObjectOfType<Palla>();
        individuals = new Individual[population];
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

    void RandomInitialize(){
        for(int i = 0; i < population; i++){
            individuals[i] = new Individual(Random.Range(0f, 360f), Random.Range(0f, maxWait), Random.Range(0f, Palla.maxPower));
        }
    }

    void Initialize(){
            for(int i = 0; i < population; i++){
                individuals[i] = new Individual(Random.Range(i*60f,(i+1)*60f), Random.Range(0f, maxWait), Random.Range(0f, Palla.maxPower));
            }
        }

    IEnumerator Execute(){
        for(int c = 0; c < 50; c++){
            RandomInitialize();
            Individual winner = null;
            int gen = 0;
            while(true){
                foreach(Individual i in individuals){
                    palla.SetRotation(i.Angle);
                    yield return new WaitForSeconds(i.Wait);
                    palla.SwitchToSelectingPower();
                    yield return new WaitForSeconds(i.Power/palla.GetPowerIncreaseSpeed());
                    palla.SwitchToShot();
                    yield return new WaitUntil(() => palla.GetGameState() == GameState.SelectingDirection);
                    i.Fitness = palla.GetPerformanceIndicator();
                    if(i.Fitness == 0){
                        winner = i;
                        break;
                    }
                }
                if(winner != null || gen > maxGens)
                    break;
                Selection();
                Crossover();
                RandomMutation();
                gen++;
            }
            if(winner != null){
                Logger.AppendAILog("\nSOLUTION WAS FOUND IN GENERATION " + gen + ": " + winner.ToString());
                Logger.AddSolution(gen, false);
            }else
                Logger.AddSolution(gen, true);
        }
    }
    //Roulette Wheel
    void Selection(){
        Logger.AppendAILog("\nSTARTING SELECTION\n");
        Logger.AppendIndividualsAsLaTeX(individuals);
        Individual[] temp = new Individual[population];
        /*DEBUG*/for(int j = 0; j < population; j++)
            temp[j] = individuals[j];
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
            Logger.AppendAILog("\nITERATION NUMBER " + j);
            Logger.AppendIndividualsAsLaTeX(temp);
        }
        individuals = temp;
    }

    void Crossover(){
        Logger.AppendAILog("\nSTARTING CROSSOVER\n");
        Logger.AppendIndividualsAsLaTeX(individuals);
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
        Logger.AppendAILog("\nITERATION NUMBER " + i);
        Logger.AppendIndividualsAsLaTeX(individuals);
        }
    }

    void RandomMutation(){
        Logger.AppendAILog("\nSTARTING MUTATION");
        Logger.AppendIndividualsAsLaTeX(individuals);
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
        Logger.AppendAILog("\nAFTER MUTATION\n");
        Logger.AppendIndividualsAsLaTeX(individuals);
    }

    void Mutation(){
            Logger.AppendAILog("\nSTARTING MUTATION");
            Logger.AppendIndividualsAsLaTeX(individuals);
            foreach(Individual i in individuals){
                int val = Random.Range(0, 3);
                float multiplier = 1/i.Fitness;
                if(val == 0){
                    float amount = Random.Range(-10f, 10f);
                    i.Angle += amount*multiplier;
                }else if(val == 1){
                    float amount = Random.Range(-0.5f, 0.5f);
                    i.Wait += amount*multiplier;
                }else{
                    float amount = Random.Range(-0.3f, 0.3f);
                    i.Power += amount*multiplier;
                }
            }
            Logger.AppendAILog("\nAFTER MUTATION\n");
            Logger.AppendIndividualsAsLaTeX(individuals);
        }
}
