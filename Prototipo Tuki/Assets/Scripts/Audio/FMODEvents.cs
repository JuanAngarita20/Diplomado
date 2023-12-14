using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
public class FMODEvents : MonoBehaviour
{
    [field: Header("Ambiente")]
    [field: SerializeField] public EventReference Ambiente {get; private set;}

    [field: Header("Tuki SFX")]
    [field: SerializeField] public EventReference Pasos {get; private set;}
    [field: SerializeField] public EventReference Para {get; private set;}
    [field: SerializeField] public EventReference Salto {get; private set;}
    [field: SerializeField] public EventReference Elect {get; private set;}
    [field: SerializeField] public EventReference RoerS {get; private set;}
    [field: SerializeField] public EventReference Chillidos {get; private set;}
    [field: SerializeField] public EventReference CamaraAlarm {get; private set;}

    [field: Header("Props SFX")]
    [field: SerializeField] public EventReference Madera {get; private set;}

    public static FMODEvents instance {get; private set;}

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Se encontro mas de un Evento en la Ecena");
        }
        instance = this;
    }

}
