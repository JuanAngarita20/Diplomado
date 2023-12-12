using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    private EventInstance ambienceEventInstance;
    public static AudioManager instance {get; private set;}


    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Se encontro mas de un audio manager en la Ecena");
        }
        instance = this;
    }

    private void Start(){
        EmpezarAmbiente(FMODEvents.instance.Ambiente);
    }
    private void EmpezarAmbiente(EventReference ambienceEventReference){
        ambienceEventInstance = CreateInstance(ambienceEventReference);
        ambienceEventInstance.start();
    }
    public void PlayOneShot(EventReference sound, Vector3 worldPos){
         RuntimeManager.PlayOneShot(sound, worldPos);
    }
    public EventInstance CreateInstance(EventReference eventReference){
        
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        return eventInstance;
    }


}

