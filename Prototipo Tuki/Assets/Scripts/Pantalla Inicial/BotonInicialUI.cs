using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BotonInicialUI : MonoBehaviour
{
  
    public void StartDemo(){
        SceneManager.LoadScene("Nivel1");
    }
}
