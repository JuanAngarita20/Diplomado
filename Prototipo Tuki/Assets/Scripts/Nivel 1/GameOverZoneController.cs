using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverZoneController : MonoBehaviour
{
    [SerializeField] private Rigidbody tukiRG = null;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     private void OnTriggerEnter(Collider other){

        if(other.gameObject.CompareTag("Player")){
           EventManager.TriggerZoneGameOver();
           tukiRG.isKinematic = true;
        }

    }
}
