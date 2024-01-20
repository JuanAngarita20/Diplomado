using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMOD.Studio; // Asegúrate de incluir este espacio de nombres
using FMODUnity; // y este también

public class BotonInicialUI : MonoBehaviour
{
    public void StartDemo()
    {
        StopAllFMODSounds();
        SceneManager.LoadScene("Nivel1");
    }

    private void StopAllFMODSounds()
    {
        Bus masterBus = RuntimeManager.GetBus("bus:/"); // Obtén el bus principal
        masterBus.stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE); // Detén todos los eventos en este bus
    }
}

