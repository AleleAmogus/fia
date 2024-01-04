using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Palla : MonoBehaviour
{
    private enum GameState{SelectingDirection, SelectingPower, Shot}

    [SerializeField] float rotationSpeed = 20f;
    [SerializeField] float  basePower = 700f;
    [SerializeField] GameObject directionMarker;
    [SerializeField] Slider powerSelector;

    Rigidbody2D rb;
    GameState state = GameState.SelectingDirection;

    public static float maxPower = 2f;
    float angle = 0f;
    float powerMultiplier = 0f;
    float powerIncreaseSpeed = 2f;
    int powerDirection = 1;
    float lastShootTime = 0f;

    void Start()
    {
        powerSelector.maxValue = 2f;
        powerSelector.value = 0f;
        powerSelector.gameObject.SetActive(false);
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        switch(state){
            case GameState.SelectingDirection: SelectingDirectionBehaviour(); break;
            case GameState.SelectingPower: SelectingPowerBehaviour(); break;
            case GameState.Shot: ShotBehaviour(); break;
        }
    }

        private void SwitchState(GameState newState){
            state = newState;
        }


    private void SelectingDirectionBehaviour(){
        float value = Input.GetAxisRaw("Horizontal")*rotationSpeed*Time.deltaTime;
                if(Input.GetKey(KeyCode.LeftShift))
                    value/=3;
                angle = (angle-value)%360;
                transform.rotation = Quaternion.Euler(0f, 0f, angle);

                if(Input.GetKeyDown(KeyCode.Space)){
                    SwitchState(GameState.SelectingPower);
                    SetObstaclesStop(true);
                    powerSelector.gameObject.SetActive(true);
                }
    }

    private void SelectingPowerBehaviour(){
        powerMultiplier = (powerMultiplier + (powerDirection * powerIncreaseSpeed * Time.deltaTime));
        powerSelector.value = powerMultiplier;
        if(powerMultiplier>maxPower)
            powerDirection = -1;
        else if(powerMultiplier<0f){
            powerDirection = +1;
            powerMultiplier = 0;
        }
        if(Input.GetKeyDown(KeyCode.Space)){
            SwitchState(GameState.Shot);
            SetObstaclesStop(false);
            rb.AddForce(transform.right*((powerMultiplier+0.5f)*basePower));
            powerSelector.gameObject.SetActive(false);
            lastShootTime = Time.time;
            directionMarker.SetActive(false);
        }
    }

    private void ShotBehaviour(){
        if((Time.time >= lastShootTime + 1 && rb.velocity.magnitude < 0.1f) || Time.time >= lastShootTime + 10){
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void SetObstaclesStop(bool val){
        RotatingObstacle[] obstacles = FindObjectsOfType<RotatingObstacle>();
        foreach(RotatingObstacle ro in obstacles)
            ro.SetStop(val);
    }

    /*public float AIbehaviour(Individual i){

    }*/
}
