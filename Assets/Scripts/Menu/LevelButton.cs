using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    [SerializeField] int level;

    public void LoadLevel(){
        SceneManager.LoadScene("Livello" + level);
    }

    public void ToggleAI(){
        Palla.ToggleAI();
    }
}
