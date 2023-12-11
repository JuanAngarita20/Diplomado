using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class GameController : MonoBehaviour

{
    
    private bool shockActivated = false;

    [SerializeField] public float batteryCharge = 30.0f;
    [SerializeField] private float batteryChargeRate = 0.03f;
    [SerializeField] private TMP_Text batterytext;
    private bool noMovement;

    void Start()
    {
        //batteryCharge = 30.0f;
        //batteryChargeRate = 0.05f;

        EventManager.ReduceBattery += reduceCharge; //Cuando un interruptor se activó, se le pide realizar la funcion reduceCharge;

        batterytext.text = "Bateria: 30%";
        noMovement = false;
        
    }

    private void reduceCharge(){
        shockActivated = true;
    }



    // Update is called once per frame
    void Update()
    {

        batterytext.text = "Bateria: "+ Mathf.Floor(batteryCharge) + "%";
        


        if(!shockActivated){
            batteryCharge += batteryChargeRate*Time.deltaTime;
        }
        
        else{
            if(batteryCharge > 5){ //Verificar que bateria no pase a ser negativo
                batteryCharge -= 5.0f;
                Debug.Log("Bateria Descargada");
                
            }
            shockActivated = false;

        }
            

        //Debug.Log(batteryCharge);
        
    }

}
