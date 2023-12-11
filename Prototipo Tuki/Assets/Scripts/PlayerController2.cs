using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class PlayerController2 : MonoBehaviour
{
    [SerializeField] private Rigidbody rg;
    [SerializeField] private Animator animator = null;
    [SerializeField] private Transform cajaCheck;
    [SerializeField] private PlayerController varPlayerController;

    private bool destroyPossible = false;

    private EventInstance Elect;
    private EventInstance RoerS;

    void Start()
    {
        EventManager.ReduceBattery += electricShockAnim; //Cuando un interruptor se activó, se le pide realizar la funcion reduceCharge;
        Elect = AudioManager.instance.CreateInstance(FMODEvents.instance.Elect);
        RoerS = AudioManager.instance.CreateInstance(FMODEvents.instance.RoerS);
    }

    private void ElectStart(){
       
        Elect.start();
    }
    private void RoerStart(){
       
        RoerS.start();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(rg.velocity.y );
        if(rg.velocity.y < -2f){
            //Debug.Log("Cayendo!!");
            animator.SetBool("jump",false);
            animator.SetBool("falling",true);

        }
        else{
            animator.SetBool("falling",false);
        }



        //Destuir objetos
        if(destroyPossible && varPlayerController.grounded){
            if(Input.GetKeyDown(KeyCode.D)){
                EventManager.GnawObject();
                animator.SetBool("gnaw",true);
                //EventoRoer
                RoerStart();
                EventManager.ActionStopMovement();

            }
        }

        //Debug.Log(animator.GetCurrentAnimatorStateInfo(0).normalizedTime);

        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Roer1") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.94f){
          
            animator.SetBool("gnaw",false);
            EventManager.RestartMovement();
            
        }

        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Roer2") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.94f ){
           
            animator.SetBool("gnaw",false);
            EventManager.RestartMovement();
            
        }

        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Shock") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.85f  && !animator.IsInTransition(0)){
           
            animator.SetBool("electricShock",false);
            EventManager.RestartMovement();
            
        }
        
        



       

    }

  

    private void OnTriggerEnter(Collider other){

        if(other.gameObject.tag.Equals("TriggerDest")){
            //Debug.Log("Esta en contacto con un destruible");
            destroyPossible = true;
        }

    }

     private void OnTriggerExit(Collider other){

        if(other.gameObject.tag.Equals("TriggerDest")){
            //Debug.Log("Esta en contacto con un destruible");
            destroyPossible = false;
        }

    }

    private void electricShockAnim(){

        animator.SetBool("electricShock",true);
        //EventoShock
        ElectStart();
        EventManager.ActionStopMovement();

    }


}
