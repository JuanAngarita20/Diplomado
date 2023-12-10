using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
public class FMODEvents : MonoBehaviour
{
    [field: Header("Tuki SFX")]
    [field: SerializeField] public EventReference Pasos {get; private set;}
    [field: SerializeField] public EventReference Para {get; private set;}
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
