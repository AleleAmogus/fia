using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{
    int successi = 0;
    [SerializeField] TMP_Text testo;

    public void Victory(){
        successi++;
        testo.text = "Successi: " + successi;
    }
}
