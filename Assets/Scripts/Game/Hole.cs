using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hole : MonoBehaviour
{
    void Update(){
        if(Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene("Menu");
    }

    void OnTriggerEnter2D(Collider2D collider){
        collider.GetComponent<Palla>().Reset(true);
        FindObjectOfType<GameUI>().Victory();
    }
}
