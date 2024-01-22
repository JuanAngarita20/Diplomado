using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEscotilla : MonoBehaviour
{
    
    [SerializeField] private int idTrigger;

    
    private void OnTriggerEnter(Collider other){

        if(other.gameObject.CompareTag("Player")){
            EventManager.InterruptorTrigger(idTrigger);
        }
    }
}
