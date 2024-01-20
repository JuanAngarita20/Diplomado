using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    private EventInstance ambienceEventInstance;
    private EventInstance musicnivEventInstance;
    public static AudioManager instance {get; private set;}

    private List<EventInstance> eventInstances;
    private List<StudioEventEmitter> eventEmitters;

    [Header("Volume")]
    [Range(0,1)]
    public float masterVolume = 1;
    [Range(0,1)]
    public float ambienceVolume = 1;
    [Range(0,1)]
    public float SFXVolume = 1;
    [Range(0,1)]
    public float MusicVolume = 1;

    private Bus masterBus;
    private Bus ambienceBus;
    private Bus sfxBus;
    private Bus MusicBus;


    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Se encontro mas de un audio manager en la Ecena");
        }
        instance = this;

        eventInstances = new List<EventInstance>();
        eventEmitters = new List<StudioEventEmitter>();

        masterBus = RuntimeManager.GetBus("bus:/");
        ambienceBus = RuntimeManager.GetBus("bus:/Ambiente");
        sfxBus = RuntimeManager.GetBus("bus:/SFX");
        MusicBus = RuntimeManager.GetBus("bus:/Musica");
    }

    private void Start(){
        EmpezarAmbiente(FMODEvents.instance.Ambiente);
        EmpezarMusica(FMODEvents.instance.MusicaNiv);
    }

    private void Update(){
        masterBus.setVolume(masterVolume);
        ambienceBus.setVolume(ambienceVolume);
        sfxBus.setVolume(SFXVolume);
        MusicBus.setVolume(MusicVolume);
    }

    private void EmpezarAmbiente(EventReference ambienceEventReference){
        ambienceEventInstance = CreateInstance(ambienceEventReference);
        ambienceEventInstance.start();
    }
    private void EmpezarMusica(EventReference musicnivEventReference){
        musicnivEventInstance = CreateInstance(musicnivEventReference);
        musicnivEventInstance.start();
    }
    public void PlayOneShot(EventReference sound, Vector3 worldPos){
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    public EventInstance CreateInstance(EventReference eventReference){
        
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventInstances.Add(eventInstance);
        return eventInstance;
    }

    public StudioEventEmitter InitializeEventEmitter(EventReference eventReference, GameObject emitterGameObject)
    {
        StudioEventEmitter emitter = emitterGameObject.GetComponent<StudioEventEmitter>();
        emitter.EventReference = eventReference;
        eventEmitters.Add(emitter);
        return emitter;
    }

    private void cleanUp(){
        foreach(EventInstance eventInstance in eventInstances){
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }

        foreach(StudioEventEmitter emitter in eventEmitters){
            emitter.Stop();
        }
    }


    private void OnDestroy(){
        cleanUp();
    }


}

