using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class RadioSoundController : MonoBehaviour
{
    private StudioEventEmitter emitter;



    void Start()
    {
        emitter = AudioManager.instance.InitializeEventEmitter(FMODEvents.instance.Radio, this.gameObject);
        emitter.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
