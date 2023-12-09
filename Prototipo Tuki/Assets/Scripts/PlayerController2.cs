using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    [SerializeField] private Rigidbody rg;
    [SerializeField] private Animator animator = null;
    [SerializeField] private Transform cajaCheck;
    [SerializeField] private PlayerController varPlayerController;

    private bool destroyPossible = false;

    void Start()
    {
        
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


}
