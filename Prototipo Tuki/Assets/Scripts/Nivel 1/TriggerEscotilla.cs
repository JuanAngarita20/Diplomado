using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEscotilla : MonoBehaviour
{
    
    [SerializeField] private int idTrigger;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     private void OnTriggerEnter(Collider other){

        if(other.gameObject.CompareTag("Player")){
            EventManager.InterruptorTrigger(idTrigger);
        }



    }
}
