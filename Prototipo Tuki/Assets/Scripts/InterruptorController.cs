using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class InterruptorController : MonoBehaviour
{
    private bool shockPossible = false;
    private bool noMovement;
    private bool isPushing;
    [SerializeField] private int idInterruptor;
   

    private  GameController battery;

    // Start is called before the first frame update
    void Start()
    {

        EventManager.StopMovForAnim += EventStopMove;
        EventManager.RestartMovAfterAnim += EventRestartMove;
        EventManager.StopInteractionForPushing += EventPushing;
        EventManager.RestartInteractionForPushing += EventNoLongerPushing;

        battery = FindAnyObjectByType<GameController>();
        noMovement = false;
        isPushing = false;
        
    }

     private void EventStopMove(){ //Respuesta al evento de Inicio de video
        noMovement  = true;
        //Debug.Log("stop mov");
    }

    private void EventRestartMove(){ //Respuesta al evento de Inicio de video
        noMovement  = false;
        //Debug.Log("iniciar mov en interruptor");
    }

    private void EventPushing(){ //Respuesta al empujar
        isPushing = true;
        
    }
     private void EventNoLongerPushing(){ //Respuesta al no empujar
        isPushing  = false;
       
    }

    // Update is called once per frame
    void Update()
    {

        //Evento triggerInterruptor
        //Debug.Log(isPushing);

        if(Input.GetKeyDown(KeyCode.S) && shockPossible && (isPushing==false) && (noMovement == false) && (battery.batteryCharge > 5.0f)){
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


    private void OnDisable(){
        EventManager.StopMovForAnim -= EventStopMove;
        EventManager.RestartMovAfterAnim -= EventRestartMove;

    }
}
