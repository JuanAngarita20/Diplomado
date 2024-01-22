using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BotonesController : MonoBehaviour
{
    
    public enum BotonPantalla{
        INICIAR,
        INSTRUCCIONES,
        VOLVERP
    }

    [Header("Boton Pantalla")]
    [SerializeField] private BotonPantalla botonpantalla;

    [SerializeField]  private TMP_Text BotonIniciar;
    [SerializeField]  private TMP_Text BotonInstrucciones;
    [SerializeField]  private TMP_Text BotonVolverP;


    public void HoverButtonEnter(){

        switch(botonpantalla){
            
            case BotonPantalla.INICIAR:
               BotonIniciar.fontSize = 50f;
            break;

            case BotonPantalla.INSTRUCCIONES:
                BotonInstrucciones.fontSize = 50f;
            break;

            case BotonPantalla.VOLVERP:
                BotonVolverP.fontSize = 53f;

            break;
        }

    }

    public void HoverButtonExit(){

        switch(botonpantalla){
            
            case BotonPantalla.INICIAR:
               BotonIniciar.fontSize = 44f;
            break;

            case BotonPantalla.INSTRUCCIONES:
                BotonInstrucciones.fontSize = 44f;
            break;

            case BotonPantalla.VOLVERP:
                BotonVolverP.fontSize = 50f;

            break;
        }
       
    }


}
