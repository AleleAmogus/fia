using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hole : MonoBehaviour
{
    void OnTriggerEnter2D(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
