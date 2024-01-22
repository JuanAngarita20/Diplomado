using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMOD.Studio; // Asegúrate de incluir este espacio de nombres
using FMODUnity; // y este también
using UnityEngine.UI;
using TMPro;

public class BotonInicialUI : MonoBehaviour
{

    [SerializeField] private TMP_Text TextoDemo;
    [SerializeField] private GameObject UI_Instruc;
    [SerializeField] private GameObject UI_Principal;

    private void Awake(){
        UI_Instruc.SetActive(false);
        UI_Principal.SetActive(true);
    }

    public void StartDemo()
    {
        StopAllFMODSounds();
        SceneManager.LoadScene("Nivel1");
    }

    public void BotonInstrucciones(){

        UI_Instruc.SetActive(true);
        UI_Principal.SetActive(false);

    }

    public void BotonVolverPrincipal(){

        UI_Instruc.SetActive(false);
        UI_Principal.SetActive(true);

    }

    private void StopAllFMODSounds()
    {
        Bus masterBus = RuntimeManager.GetBus("bus:/"); // Obtén el bus principal
        masterBus.stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE); // Detén todos los eventos en este bus
    }

    public void HoverButtonEnter(){
        TextoDemo.fontSize = 50f;

    }

    public void HoverButtonExit(){
        TextoDemo.fontSize = 44f;
    }
}

