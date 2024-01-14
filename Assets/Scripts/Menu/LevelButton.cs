using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    [SerializeField] Scene level;

    public void LoadLevel(){
        SceneManager.LoadScene(level.name);
    }
}
