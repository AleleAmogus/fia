using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Individual
{
    private float angle;
    private float wait;
    private float power;
    private float fitness;

    // Costruttore
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
        set { angle = value; }
    }

    // Getter e setter per 'wait'
    public float Wait
    {
        get { return wait; }
        set { wait = value; }
    }

    // Getter e setter per 'power'
    public float Power
    {
        get { return power; }
        set { power = value; }
    }

    // Getter e setter per 'fitness'
        public float Fitness
        {
            get { return fitness; }
            set { fitness = value; }
        }
}

