using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscotillaController : MonoBehaviour
{
   [SerializeField] private List<int> idInterruptores;
    [SerializeField] private Vector3 angulo0;
    [SerializeField] private Vector3 angulo1;
    [SerializeField] private int compToCheck;


    private bool animacionRotar;
    private bool alreadyRotating;
    private bool alreadyOpen;

    public Quaternion targetAngulo0 = Quaternion.Euler(0,0,0);
    public Quaternion targetAngulo1 = Quaternion.Euler(0,0,0);
    private Quaternion angleObjective;

    void Start()
    {
        EventManager.AccionInterruptor += RotarPlataforma;
        animacionRotar = false;
        alreadyRotating = false;
        alreadyOpen = false;

        //Rotacion Animaciones
        targetAngulo0 = Quaternion.Euler(angulo0.x,angulo0.y,angulo0.z);
        targetAngulo1 = Quaternion.Euler(angulo1.x,angulo1.y,angulo1.z);
        angleObjective = targetAngulo0;
    }

    private void RotarPlataforma(int interrupID){ //Recibir ID del interrruptor

        
        foreach(int id in idInterruptores){
        if(id == interrupID){
                if(alreadyOpen == false){
                    Debug.Log("Activaron a plataforma");
                    animacionRotar = true;
                    changeCurrectAngle();
                   
                    
                }
                
            }
        }   


    }


    void Update()
    {
        if(animacionRotar){
            transform.rotation = Quaternion.Slerp(transform.rotation,angleObjective,0.2f);
            alreadyRotating = true; //Designar que ya se esta girando
            //Debug.Log("ROTACION PLATAFORMA");
        }

        //Debug.Log("Plataforma: " + transform.localEulerAngles);
        //Debug.Log("Objetivo 0 : " + targetAngulo0.eulerAngles);
        //Debug.Log("Objetivo 1: " + targetAngulo1.eulerAngles);

        if(transform.rotation.eulerAngles == targetAngulo1.eulerAngles){
            
            if(alreadyRotating){
                Debug.Log("Transformada: " + transform.rotation.eulerAngles);
                Debug.Log("Obj 0: " + targetAngulo1.eulerAngles);
                Debug.Log("Detener Animacion 1 ------------------------------------------------------------------------rotar");
                animacionRotar = false;
                alreadyRotating = false;

                alreadyOpen = true;
            }
           
        }

        if(transform.rotation.eulerAngles == targetAngulo0.eulerAngles){
            
            if(alreadyRotating){
                Debug.Log("Transformada: " + transform.rotation.eulerAngles);
                Debug.Log("Obj 1: " + targetAngulo0.eulerAngles);

                Debug.Log("Detener Animacion 2 ------------------------------------------------------------------------rotar");
                animacionRotar = false;
                alreadyRotating = false;

                alreadyOpen = true;
            }
        }
        
        

    }

    private void changeCurrectAngle(){

        if(compToCheck == 1){ //Giro en x
            if(angleObjective.eulerAngles.x == targetAngulo0.eulerAngles.x){
                angleObjective = targetAngulo1;
            }
            else{
                angleObjective = targetAngulo0;
            }

        }

        if(compToCheck == 2){ //Giro en z
            if(angleObjective.eulerAngles.z == targetAngulo0.eulerAngles.z){
                angleObjective = targetAngulo1;
            }
            else{
                angleObjective = targetAngulo0;
               
            }

        }
        
    }
    
    private void OnDisable(){
        EventManager.AccionInterruptor -= RotarPlataforma;
    }
}
