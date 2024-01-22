using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour

{
    
    private bool shockActivated = false;
    private bool noMovement;
    private bool loseControl;
    private bool gameOver;
    private bool restartButton;
    private bool onMenu;
    private bool levelFinished;
    float timeForRestart;

    [SerializeField] public float batteryCharge = 30.0f;
    [SerializeField] private float batteryChargeRate = 0.03f;
    [SerializeField] private TMP_Text batterytext;
    [SerializeField] private GameObject panelMenu;
    [SerializeField] private GameObject  botonRestart;
    [SerializeField] private GameObject  botonVolver;
    [SerializeField] private GameObject  botonSalir;
    [SerializeField] private GameObject  SlidersVolumen;
    [SerializeField] private GameObject  TextoVolumen;
    [SerializeField] private GameObject  BatteryUI;
    [SerializeField] private GameObject  UI_LevelFinished;
    
    

    void Start()
    {
        //batteryCharge = 30.0f;
        //batteryChargeRate = 0.05f;

        EventManager.ReduceBattery += reduceCharge; //Cuando un interruptor se activó, se le pide realizar la funcion reduceCharge;
        EventManager.GameOver += GameOver;
        EventManager.ZoneGameOver += GameOver;
        EventManager.FinisehdLevel1 += EventFinishedLevel;

        batterytext.text = "Bateria: 30%";
        noMovement = false;
        loseControl = false;
        gameOver = false;
        restartButton = false;
        onMenu = false;
        levelFinished = false;

        panelMenu.SetActive(false);
        botonRestart.SetActive(false);
        botonVolver.SetActive(false);
        botonSalir.SetActive(false);
        SlidersVolumen.SetActive(false);
        TextoVolumen.SetActive(false);
        BatteryUI.SetActive(true);
        UI_LevelFinished.SetActive(false);
       
    }

    public void restartLevel(){
        restartButton = true;       
    }


    
    public void funcionBotonVolver(){
        SceneManager.LoadScene("PantallaInicial");
    }
    public void funcionBotonSalir(){
        Application.Quit();
    }



    private void reduceCharge(){
        shockActivated = true;
    }

    private void GameOver(){
        gameOver = true;
        panelMenu.SetActive(true);
        botonRestart.SetActive(true);
        batterytext.text = "GAME OVER";
    }

    private void EventFinishedLevel(){
        levelFinished = true;
        botonVolver.SetActive(true);
        UI_LevelFinished.SetActive(true);

        BatteryUI.SetActive(false);
        EventManager.ActionStopMovement();

    }

    // Update is called once per frame
    void Update()
    {

        
        batteryTextColor();
        if(!gameOver){
            batterytext.text = "Bateria: "+ Mathf.Floor(batteryCharge) + "%";
        }
        

        

        

        if(batteryCharge > 99){
            EventManager.BatteryReach100();
            gameOver = true;
            panelMenu.SetActive(true);
            botonRestart.SetActive(true);
            batterytext.text = "GAME OVER";
        }
        
        


        if(!onMenu){
            batteryCharge += batteryChargeRate*Time.deltaTime;
        }
        
        if(shockActivated){
            if(batteryCharge > 5 && !gameOver){ //Verificar que bateria no pase a ser negativo
                batteryCharge -= 5.0f;
                Debug.Log("Bateria Descargada");
                
            }
            shockActivated = false;

        }
            

        //Debug.Log(batteryCharge);
        if(batteryCharge > 80 && (loseControl == false)){
            //Confundir
            EventManager.BatteryOver80();
            loseControl = true;
        }
        if(loseControl && batteryCharge < 79){
            EventManager.BatteryUnder80();
            loseControl = false;
        }

        if(restartButton){

            if(timeForRestart < 2){
                timeForRestart += 1.0f * Time.deltaTime;
            }
            else{
                SceneManager.LoadScene("Nivel1");
            }

        }


        if(Input.GetKeyDown(KeyCode.Escape) && !levelFinished){
            if(onMenu == false){
                onMenu = true;

                panelMenu.SetActive(true);
                botonRestart.SetActive(true);
                botonSalir.SetActive(true);
                botonVolver.SetActive(true);
                SlidersVolumen.SetActive(true);
                TextoVolumen.SetActive(true);
                BatteryUI.SetActive(false);
                EventManager.ActionStopMovement();
            }
            else{
                onMenu = false;
                panelMenu.SetActive(false);
                botonRestart.SetActive(false);
                botonSalir.SetActive(false);
                botonVolver.SetActive(false);
                SlidersVolumen.SetActive(false);
                TextoVolumen.SetActive(false);
                BatteryUI.SetActive(true);
                EventManager.RestartMovement();
            }
        }
        
        
    }

    private void batteryTextColor(){
        if(batteryCharge < 30){ //verde
            batterytext.color = new Color (0,1,0,1);
        }
        else if(batteryCharge >= 30 && batteryCharge < 60){ //amarillo
            batterytext.color = new Color (0.9f,0.9f,0.1f,1);
        }
        else if(batteryCharge >= 60 && batteryCharge < 80){ //Naranja
            batterytext.color = new Color (1,0.5f,0,1);
        }
        else if(batteryCharge >= 80){
            batterytext.color = new Color (1,0,0,1); //Rojo
        }
        else {
            batterytext.color = new Color (0,0,0,1);
        }
    }


    private void OnDisable(){
        EventManager.ReduceBattery -= reduceCharge;
        EventManager.GameOver -= GameOver;
        EventManager.ZoneGameOver -= GameOver;
        EventManager.FinisehdLevel1 -= EventFinishedLevel;
    }


}
