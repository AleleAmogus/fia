using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingObstacle : MonoBehaviour
{
    bool stop = false;
    [SerializeField] float rotationSpeed = 90f;

    public static void ResetObstacles(){
        RotatingObstacle[] obstacles = FindObjectsOfType<RotatingObstacle>();
        foreach(RotatingObstacle o in obstacles)
            o.transform.rotation = Quaternion.identity;
    }

    void FixedUpdate()
    {
        if(!stop)
            transform.Rotate(0f, 0f ,rotationSpeed*Time.fixedDeltaTime);
    }

    public void SetStop(bool val){
        stop = val;
    }
}
