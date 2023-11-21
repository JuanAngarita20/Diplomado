using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerLowerFloor : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Camera secondCamera;

    void Awake()
    {
        secondCamera.enabled = false;
    }

    private void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Player"){
            secondCamera.enabled = true;
        }

    }

    private void OnTriggerExit(Collider other){
        if(other.gameObject.tag == "Player"){
            secondCamera.enabled = false;
        }

    }
}
