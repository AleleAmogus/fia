using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Individual : ICloneable
{
    private float angle;
    private float wait;
    private float power;
    private float fitness;

    // Costruttori
    public Individual(float initialAngle, float initialWait, float initialPower, float initialFitness)
    {
        angle = initialAngle;
        wait = initialWait;
        power = initialPower;
        fitness = initialFitness;
    }
    public Individual(float initialAngle, float initialWait, float initialPower)
    {
        angle = initialAngle;
        wait = initialWait;
        power = initialPower;
        fitness = -1f;
    }

    // Getter e setter per 'angle'
    public float Angle
    {
        get { return angle; }
        set { angle = value%360; }
    }

    // Getter e setter per 'wait'
    public float Wait
    {
        get { return wait; }
        set { wait = value; if(wait > GeneticAlgorithm.maxWait)wait = GeneticAlgorithm.maxWait;if(wait < 0)wait = 0;}
    }

    // Getter e setter per 'power'
    public float Power
    {
        get { return power; }
        set { power = value; if(power > Palla.maxPower)power = Palla.maxPower;if(power < 0)power = 0;}
    }

    // Getter e setter per 'fitness'
        public float Fitness
        {
            get { return fitness; }
            set { fitness = value; }
        }

    public object Clone(){
        return new Individual(angle, wait, power, fitness);
    }

    public override string ToString()
       {
          return "angle: " + angle + ", wait: " + wait + ", power: " + power + ", fitness: " + fitness;
       }
}

