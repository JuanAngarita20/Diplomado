using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableController : MonoBehaviour
{

    [SerializeField] private int Hp = 3;
    [SerializeField] GameObject objectToDestroy;

    void Start()
    {
        EventManager.DamageObject += quitarHpObjeto; //Suscribirse al evento de Inicio de Video
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Hp < 1){
            Destroy(objectToDestroy);
            Debug.Log("Objeto destruido");
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
