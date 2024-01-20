using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManagerMEN : MonoBehaviour
{
     private EventInstance musicmenEventInstance;
    public static AudioManagerMEN instance {get; private set;}

    private List<EventInstance> eventInstances;
    private List<StudioEventEmitter> eventEmitters;


    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Se encontro mas de un audio manager en la Ecena");
        }
        instance = this;
        eventInstances = new List<EventInstance>();
        eventEmitters = new List<StudioEventEmitter>();
    }

    private void Start(){
        EmpezarMusica(FMODEventsMEN.instance.MusicaMen);
    }
    private void EmpezarMusica(EventReference musicmenEventReference){
        musicmenEventInstance = CreateInstance(musicmenEventReference);
        musicmenEventInstance.start();
    }
    public void PlayOneShot(EventReference sound, Vector3 worldPos){
         RuntimeManager.PlayOneShot(sound, worldPos);
    }
    public EventInstance CreateInstance(EventReference eventReference){
        
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        return eventInstance;
    }


}