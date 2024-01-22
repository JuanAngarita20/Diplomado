using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerFinishLevel1 : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider other){

        if(other.gameObject.CompareTag("Player")){
            EventManager.TriggerFinisehdLevel1();
        }
    }
}
