using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChangeCameraTrigger : MonoBehaviour
{

    [SerializeField] Camera mainCamera;
    [SerializeField] Camera secondCamera;
    [SerializeField] GameObject videoObject;

    private bool ontrigger = false;
    private bool videoAlredyPlay = false;
    private bool videoEventCalled = false;


    // Start is called before the first frame update
    void Start()
    {
        secondCamera.enabled = false;
        EventManager.TurnOffVideo += Offvideo;
    }

    // Update is called once per frame
    void Update()
    {
        if(ontrigger && !videoAlredyPlay){
            secondCamera.enabled = true;

            if(!videoEventCalled){
                //Inicio evento
                EventManager.VideoStarted();
                //Debug.Log("Trigger Video Inicio");
                videoEventCalled = true;

            }
            
        }
        
    }

   

    private void Offvideo(){
        secondCamera.enabled = false;
        videoEventCalled = false;
        videoAlredyPlay = true;
    }

    private void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Player"){
            ontrigger = true;
        }

    }

    private void OnTriggerExit(Collider other){
        if(other.gameObject.tag == "Player"){
            ontrigger = false;
            videoAlredyPlay = false;
        }

    }

    private void OnDisable(){
        EventManager.TurnOffVideo -= Offvideo;
    }
}
