using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
public class FMODEventsMEN : MonoBehaviour
{
    
    [field: Header("Musica")]
    [field: SerializeField] public EventReference MusicaMen {get; private set;}

    public static FMODEventsMEN instance {get; private set;}

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Se encontro mas de un Evento en la Ecena");
        }
        instance = this;
    }

}