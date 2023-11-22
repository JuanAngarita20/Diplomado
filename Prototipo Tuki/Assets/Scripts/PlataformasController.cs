using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class PlataformasController : MonoBehaviour
{
    [SerializeField] private List<int> idInterruptores;
    [SerializeField] private Vector3 angulo0;
    [SerializeField] private Vector3 angulo1;
    [SerializeField] private int componentToCheck;


    private bool animacionRotar;
    private Quaternion targetAngulo0 = Quaternion.Euler(0,0,0);
    private Quaternion targetAngulo1 = Quaternion.Euler(0,0,0);
    private Quaternion angleObjective;

    void Start()
    {
        EventManager.AccionInterruptor += RotarPlataforma;
        animacionRotar = false;

        //Rotacion Animaciones
        targetAngulo0 = Quaternion.Euler(angulo0.x,angulo0.y,angulo0.z);
        targetAngulo1 = Quaternion.Euler(angulo1.x,angulo1.y,angulo1.z);
        angleObjective = targetAngulo0;
    }

    private void RotarPlataforma(int interrupID){ //Recibir ID del interrruptor

        foreach(int id in idInterruptores){
            if(id == interrupID){
                Debug.Log("Activaron a plataforma");
                //transform.rotation = Quaternion.AngleAxis(90.0f,Vector3.right)*transform.rotation;
                animacionRotar = true;
                changeCurrectAngle();
                //transform.rotation = Quaternion.Slerp(transform.rotation,angleObjective,0.2f);
            }
        }

    }


    void Update()
    {
        if((angleObjective.eulerAngles.z != targetAngulo0.eulerAngles.z) && (angleObjective.eulerAngles.z != targetAngulo1.eulerAngles.z)){
            transform.rotation = Quaternion.Slerp(transform.rotation,angleObjective,0.2f);
            //animacionRotar = false;
            Debug.Log("ROTACION PLATAFORMA");
        }
        

    }

    private void changeCurrectAngle(){

        if(componentToCheck == 1){ //Giro en x
            if(angleObjective.eulerAngles.x == targetAngulo0.eulerAngles.x){
                angleObjective = targetAngulo1;
                
            }
            else{
                angleObjective = targetAngulo0;
                
            }

        }

        if(componentToCheck == 2){ //Giro en z
            if(angleObjective.eulerAngles.z == targetAngulo0.eulerAngles.z){
                angleObjective = targetAngulo1;
                
            }
            else{
                angleObjective = targetAngulo0;
               
            }

        }
        
    }
}
