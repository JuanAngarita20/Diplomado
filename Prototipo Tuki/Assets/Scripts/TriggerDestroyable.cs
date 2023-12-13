using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class TriggerDestroyable : MonoBehaviour
{
    

    [SerializeField] private float idTrigger;
    private EventInstance Madera;

    private bool gnawPossible;

    private void MaderaStart(){
        //Debug.Log("Objeto destruido");
        Madera.start();
    }

    void Start()
    {

        //Audio
        Madera = AudioManager.instance.CreateInstance(FMODEvents.instance.Madera);
        gnawPossible = false;
        
    }

    // Update is called once per frame
    void Update()
    {
    

        if(Input.GetKeyDown(KeyCode.D) && gnawPossible){
            //Debug.Log("Trigger y roer activado");
            EventManager.GnawObject(idTrigger);
            EventManager.StartGnawAnim();
            MaderaStart();
        }
        
    }

    private void OnTriggerEnter(Collider other){

        if(other.gameObject.CompareTag("Player")){
            gnawPossible = true;
            //Debug.Log("Shock Electrico Posible");
        }

    }

    private void OnTriggerExit(Collider other){
        if(other.gameObject.tag == "Player"){
           gnawPossible = false;
        }

    }


}
