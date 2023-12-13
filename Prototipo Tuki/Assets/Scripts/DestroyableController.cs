using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
public class DestroyableController : MonoBehaviour
{

    [SerializeField] private int Hp = 3;
    [SerializeField] private int idTriggThatDestroy;
    private EventInstance Madera;


    private void MaderaStart(){
        //Debug.Log("Objeto destruido");
        Madera.start();
    }

    void Start()

    {
        EventManager.DamageObject += quitarHpObjeto;
        //Audio
        Madera = AudioManager.instance.CreateInstance(FMODEvents.instance.Madera);
        
        
        
        
    }

    private void quitarHpObjeto(float idTrigger){
        //Debug.Log(idTrigger);

        if(idTrigger == idTriggThatDestroy){//Verificar que trigger destruible sea el correcto
            Debug.Log("me quitaste Hp");
            Hp--;

        }

    }

    // Update is called once per frame
    void Update()
    {
        if(Hp < 1){
            Destroy(gameObject);
            //Destroy(objectToDestroy);
            Debug.Log("Objeto destruido");
            MaderaStart();
        }

        
    }

    private void OnDisable(){
        EventManager.DamageObject -= quitarHpObjeto;
    }

   

    
}
