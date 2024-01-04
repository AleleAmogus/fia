using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticAlgorithm : MonoBehaviour
{
    int population = 4;
    Individual[] individuals;
    int individual = 0;
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
            individuals[i] = new Individual(Random.Range(0f, 360f), Random.Range(0f, 4f), Random.Range(0f, Palla.maxPower));
        }
    }

    IEnumerator Execute(){
        foreach(Individual i in individuals){
            Debug.Log("angle: " + i.Angle + ", power: " + i.Power + ", wait: " + i.Wait);
            palla.SetRotation(i.Angle);
            yield return new WaitForSeconds(i.Wait);
            palla.SwitchToSelectingPower();
            yield return new WaitForSeconds(i.Power);
            palla.SwitchToShot();
            yield return new WaitUntil(() => palla.GetGameState() == GameState.SelectingDirection);
            i.Fitness = palla.GetPerformanceIndicator();
            Debug.Log(i.Fitness);
        }
        void Selection(){

        }
    }

    void Selection(){
        float sum = 0;
        individuals.ForEach( i =>sum += i.Fitness);
        int val = Random.Range(0f, sum)
        if(val < )
    }
}
