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
    [SerializeField] private GameObject ModeloTuki;
    

    private bool destroyPossible = false;
    

    private EventInstance Elect;
    private EventInstance RoerS;

    void Start()
    {
        //animator = gameObject.GetComponentInChildren<Animator>();
        EventManager.ReduceBattery += electricShockAnim; //Cuando un interruptor se activó, se le pide realizar la funcion reduceCharge;
        EventManager.AnimGnaw +=  GnawAnim; // Cuando roen, activar animacion y sonido


        Elect = AudioManager.instance.CreateInstance(FMODEvents.instance.Elect);
        RoerS = AudioManager.instance.CreateInstance(FMODEvents.instance.RoerS);
    }

    private void ElectStart(){
       
        Elect.start();
    }
    private void RoerStart(){
       
        RoerS.start();
    }

    private void GnawAnim(){
        
        //Destuir objetos
        if(varPlayerController.grounded){
    
            animator.SetBool("gnaw",true);
            EventManager.ActionStopMovement();
            
            //EventoRoer
            RoerStart();
            

            
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(rg.velocity.y );
        if(rg.velocity.y < -1.5f){
            //Debug.Log("Cayendo!!");
            animator.SetBool("jump",false);
            animator.SetBool("falling",true);

        }
        else{
            if(varPlayerController.grounded){
                animator.SetBool("falling",false);
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

    private void OnDisable(){
        EventManager.ReduceBattery -= electricShockAnim;
        EventManager.AnimGnaw -=  GnawAnim; // Cuando roen, activar animacion y sonido
    }


}
