using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Individual
{
    private float angle;
    private float wait;
    private float power;

    // Costruttore
    public Individual(float initialAngle, float initialWait, float initialPower)
    {
        angle = initialAngle;
        wait = initialWait;
        power = initialPower;
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
}

