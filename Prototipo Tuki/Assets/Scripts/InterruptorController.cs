using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterruptorController : MonoBehaviour
{
    private bool shockPossible = false;
    private bool isColliding = false;
    [SerializeField] private int idInterruptor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        //Evento triggerInterruptor


        if(Input.GetKeyDown(KeyCode.S) && shockPossible){
            //Evento Bajar carga
            EventManager.triggerReduceBattery();
            

        }

        
    }

    private void OnTriggerEnter(Collider other){

        if(other.gameObject.CompareTag("Player")){
            shockPossible = true;
            Debug.Log("Shock Electrico Posible");
        }



    }


    private void OnTriggerExit(Collider other){
        if(other.gameObject.tag == "Player"){
           shockPossible= false;
        }

    }
}
