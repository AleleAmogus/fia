using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] GameObject levelsMenu;

    public void Gioca(){
        levelsMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    public void Esci(){
        Application.Quit();
    }
}
