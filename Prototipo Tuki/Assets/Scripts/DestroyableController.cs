using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
public class DestroyableController : MonoBehaviour
{

    [SerializeField] private int Hp = 3;
    [SerializeField] GameObject objectToDestroy;
    private EventInstance Madera;

    private void MaderaStart(){
        //Debug.Log("Objeto destruido");
        Madera.start();
    }

    void Start()
    {
        EventManager.DamageObject += quitarHpObjeto; //Suscribirse al evento de Inicio de Video

        //Audio
        Madera = AudioManager.instance.CreateInstance(FMODEvents.instance.Madera);
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Hp < 1){
            Destroy(objectToDestroy);
            Debug.Log("Objeto destruido");
            MaderaStart();
        }
        
    }

    private void quitarHpObjeto(){

        Debug.Log("me quitaste Hp");
        Hp--;

    }

    private void OnDisable(){
        EventManager.DamageObject -= quitarHpObjeto;
    }
}
