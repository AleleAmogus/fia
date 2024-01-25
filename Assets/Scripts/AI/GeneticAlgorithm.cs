using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticAlgorithm : MonoBehaviour
{
    public static float maxWait = 4f;
    [SerializeField] int maxGens = 12;
    int population = 6;//4 nella versione iniziale
    Individual[] individuals;
    Palla palla;
    Individual bestFitting;
    //nota: la fitness di un individuo è assegnata nella classe "Palla", metodo "Reset".

    void Start()
    {
        // Quando viene caricato il livello controlla che l'IA sia attiva. Se lo è, avvia l'algoritmo.
        if(Palla.GetAIactive()){
            palla = FindObjectOfType<Palla>();
            individuals = new Individual[population];
            StartCoroutine(Execute());
        }
        else
            gameObject.SetActive(false);
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
    /* Prima versione: inizializzazione casuale.
    void RandomInitialize(){
        for(int i = 0; i < population; i++){
            individuals[i] = new Individual(Random.Range(0f, 360f), Random.Range(0f, maxWait), Random.Range(0f, Palla.maxPower));
        }
    }*/

    //Versione finale: l'i-esimo individuo ha un'angolazione compresa tra 60*i e 60*(i+1) gradi.
    void Initialize(){
            for(int i = 0; i < population; i++){
                individuals[i] = new Individual(Random.Range(i*60f,(i+1)*60f), Random.Range(0f, maxWait), Random.Range(0f, Palla.maxPower));
            }
        }

    //Avvia la simulazione
    IEnumerator Execute(){
    //Per testare le prestazioni, la simulazione si ferma dopo aver trovato 50 soluzioni.
        for(int c = 0; c < 50; c++){
            Initialize(); //Inizializza la popolazione
            Individual winner = null;
            int gen = 0;
            while(true){
                //Esegui tutti i passaggi della simulazione (regola angolo, attendi, seleziona potenza)
                foreach(Individual i in individuals){
                    palla.SetRotation(i.Angle);
                    yield return new WaitForSeconds(i.Wait);
                    palla.SwitchToSelectingPower();
                    yield return new WaitForSeconds(i.Power/palla.GetPowerIncreaseSpeed());
                    palla.SwitchToShot();
                    yield return new WaitUntil(() => palla.GetGameState() == GameState.SelectingDirection);
                    i.Fitness = palla.GetPerformanceIndicator();
                    //Il valore 0 alla fitness è un valore sentinella: indica che la pallina ha centrato la buca.
                    if(i.Fitness == 0){
                        winner = i;
                        break;
                    }
                }
                if(winner != null || gen > maxGens)
                    break;
                Elitism(); //Applica elitismo;
                Selection(); //Applica selezione;
                Crossover(); //Applica crossover;
                Mutation(); //Applica mutazione;
                gen++; //aumenta il conto delle generazioni
            }
            //La classe Logger è utilizzata per tenere traccia delle prestazioni dell'algoritmo su un documento di testo.
            if(winner != null){
                Logger.AppendAILog("\nSOLUTION WAS FOUND IN GENERATION " + gen + ": " + winner.ToString());
                Logger.AddSolution(gen, false);
            }else
                Logger.AddSolution(gen, true);
        }
    }
    void Elitism(){
        int worst = 0;
        int best = 0;
        //trova migliore e peggiore della popolazione attuale
        for(int i = 1; i < population-1; i++){
            if(individuals[i].Fitness < individuals[worst].Fitness)
                worst = i;
            else if(individuals[i].Fitness > individuals[best].Fitness)
                best = i;
        }
        //Si confrontano il migliore della scorsa generazione (bestFitting) con il peggiore di quella attuale:
        //se il primo ha fitness maggiore si sostituisce al secondo.
        if(bestFitting != null && individuals[worst].Fitness < bestFitting.Fitness)
            individuals[worst] = bestFitting;
        //Si confrontano il migliore della scorsa generazione (bestFitting) con il migliore di quella attuale:
        //se il secondo ha fitness maggiore diventa il nuovo best-fitting.
        if(bestFitting == null || individuals[best].Fitness > bestFitting.Fitness)
            bestFitting = (Individual)individuals[best].Clone();
    }

    //Roulette Wheel: in sostanza, ogni individuo viene selezionato nella nuova generazione con una probabilità
    //proporzionale alla fitness.
    void Selection(){
        Logger.AppendAILog("\nSTARTING SELECTION\n");
        Logger.AppendIndividualsAsLaTeX(individuals);
        Individual[] temp = new Individual[population];
        for(int j = 0; j < population; j++)
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

    //Si selezionano gli individui a coppie: per ogni coppia si scambia un parametro casuale tra angolazione, attesa e
    //potenza.
    //NOTA: non funziona con popolazioni dispari. Essendo tuttavia questo algoritmo finalizzato ad una presenzatione,
    //non si è considerato necessario risolvere il problema.
    void Crossover(){
        Logger.AppendAILog("\nSTARTING CROSSOVER\n");
        Logger.AppendIndividualsAsLaTeX(individuals);
        for(int i = 0; i < population-1; i= i+2){
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
        individuals[i].Fitness = (individuals[i].Fitness + individuals[i+1].Fitness)/2;
        individuals[i+1].Fitness = individuals[i].Fitness;
        Logger.AppendAILog("\nITERATION NUMBER " + i);
        Logger.AppendIndividualsAsLaTeX(individuals);
        }
    }

    //si sceglie un parametro casuale tra angolazione, attesa e potenza: questo viene mutato di un valore casuale
    //compreso in un range
    void Mutation(){
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
}
