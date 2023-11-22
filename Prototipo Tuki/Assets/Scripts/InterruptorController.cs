using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterruptorController : MonoBehaviour
{
    private bool shockPossible = false;
    [SerializeField] private int idInterruptor;


    private  GameController battery;

    // Start is called before the first frame update
    void Start()
    {
        battery = FindAnyObjectByType<GameController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        //Evento triggerInterruptor

        if(Input.GetKeyDown(KeyCode.S) && shockPossible && (battery.batteryCharge > 5.0f)){
            //Evento Bajar carga
            EventManager.triggerReduceBattery();
            EventManager.InterruptorTrigger(idInterruptor);
            

        }

        
    }

    private void OnTriggerEnter(Collider other){

        if(other.gameObject.CompareTag("Player")){
            shockPossible = true;
            //Debug.Log("Shock Electrico Posible");
        }



    }


    private void OnTriggerExit(Collider other){
        if(other.gameObject.tag == "Player"){
           shockPossible= false;
        }

    }
}
