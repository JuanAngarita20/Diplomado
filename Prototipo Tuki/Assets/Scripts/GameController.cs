using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour

{
    
    private bool shockActivated = false;

    [SerializeField] private float batteryCharge = 30.0f;
    [SerializeField] private float batteryChargeRate = 0.03f;


    void Start()
    {
        batteryCharge = 30.0f;
        batteryChargeRate = 0.05f;

        EventManager.ReduceBattery += reduceCharge; //Cuando un interruptor se activó, se le pide realizar la funcion reduceCharge;
        
    }

    private void reduceCharge(){
        shockActivated = true;
    }

  

    // Update is called once per frame
    void Update()
    {



        if(!shockActivated){
            batteryCharge += batteryChargeRate*Time.deltaTime;
        }
        
        else{
            batteryCharge = batteryCharge-5.0f;
            Debug.Log("Bateria Descargada");
            shockActivated = false;
        }

        //Debug.Log(batteryCharge);
        
    }

}
