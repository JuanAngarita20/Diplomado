using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class InterruptorController : MonoBehaviour
{
    private bool shockPossible = false;
    private bool noMovement;
    [SerializeField] private int idInterruptor;


    private  GameController battery;

    // Start is called before the first frame update
    void Start()
    {
        battery = FindAnyObjectByType<GameController>();
        noMovement = false;

        EventManager.StopMovForAnim += EventStopMove;
        EventManager.RestartMovAfterAnim += EventRestartMove;
    }

     private void EventStopMove(){ //Respuesta al evento de Inicio de video
        noMovement  = true;
        //Debug.Log("stop mov");
    }

    private void EventRestartMove(){ //Respuesta al evento de Inicio de video
        noMovement  = false;
        Debug.Log("iniciar mov en interruptor");
    }

    // Update is called once per frame
    void Update()
    {
        //Evento triggerInterruptor

        if(Input.GetKeyDown(KeyCode.S) && shockPossible && (noMovement == false)){
            //Evento Bajar carga
            EventManager.triggerReduceBattery();
            EventManager.InterruptorTrigger(idInterruptor);
            //ShockElectrico
            

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
