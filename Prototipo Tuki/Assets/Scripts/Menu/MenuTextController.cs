using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuTextController : MonoBehaviour
{
    
    public enum BotonMenu{
        REINICIAR,
        VOLVER,
        SALIR
    }

    [Header("Boton Menu")]
    [SerializeField] private BotonMenu botonmenu;

    [SerializeField]  private TMP_Text BotonReiniciar;
    [SerializeField]  private TMP_Text BotonVolver;
    [SerializeField]  private TMP_Text BotonSalir;

    

    public void HoverButtonEnter(){

        switch(botonmenu){
            case BotonMenu.REINICIAR:
                BotonReiniciar.fontSize = 28f;
            break;

            case BotonMenu.VOLVER:
                BotonVolver.fontSize = 28f;
            break;

            case BotonMenu.SALIR:
                BotonSalir.fontSize = 28f;

            break;
        }

    }

    public void HoverButtonExit(){

        switch(botonmenu){
            case BotonMenu.REINICIAR:
                BotonReiniciar.fontSize = 25f;
            break;

            case BotonMenu.VOLVER:
                BotonVolver.fontSize = 25f;
            break;

            case BotonMenu.SALIR:
                BotonSalir.fontSize = 25f;

            break;
        }
       
    }
}
