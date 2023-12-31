using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondSCController : MonoBehaviour
{
   
    [SerializeField] private List<int> idInterruptores;
    [SerializeField] private Vector3 angulo0;
    [SerializeField] private Vector3 angulo1;
    [SerializeField] private int compToCheck;


    [SerializeField] private LayerMask MaskToFind;
    [SerializeField] private float distMaxLaser;
    [SerializeField] private int idInterruptor;


    public Quaternion targetAngulo0 = Quaternion.Euler(0,0,0);
    public Quaternion targetAngulo1 = Quaternion.Euler(0,0,0);
    private Quaternion angleObjective;
    private float laserCoolDown;
    private bool playerDetected = false;
    private bool cameraActivated = true;

    void Start()
    {
        EventManager.AccionInterruptor += InterruptorCamara;


        //Rotacion Animaciones
        targetAngulo0 = Quaternion.Euler(angulo0.x,angulo0.y,angulo0.z);
        targetAngulo1 = Quaternion.Euler(angulo1.x,angulo1.y,angulo1.z);
        angleObjective = targetAngulo0;

        distMaxLaser = 22f;
        laserCoolDown = 0.0f;
        cameraActivated = true;

    }

    private void InterruptorCamara(int interrupID){

        foreach(int id in idInterruptores){

            if(id == interrupID){
                if(cameraActivated==true){
                    cameraActivated = false;
                }
                /*else{
                    cameraActivated = true;
                }*/
                
            }
        }   

    }


    // Update is called once per frame
    void Update()
    {


        if(cameraActivated){

            Ray rayo = new Ray(transform.position,transform.right*-1);
        
            if(laserCoolDown == 0.0f){

                if(Physics.Raycast(rayo,distMaxLaser,MaskToFind,QueryTriggerInteraction.Ignore)){
                    Debug.DrawRay(rayo.origin,rayo.direction*distMaxLaser,Color.red);
                    Debug.Log("Movimiento Detectado");
                    //DeteccionCamara
                    playerDetected = true;

                    EventManager.InterruptorTrigger(idInterruptor);
                    
                }

            }
            if(playerDetected){
                iniciarLaserCooldown(); //Para interaccion de la camara con player (Laser rojo o verde)
                Debug.DrawRay(rayo.origin,rayo.direction*distMaxLaser,Color.red);
            }
            else{
                Debug.DrawRay(rayo.origin,rayo.direction*distMaxLaser,Color.green);
            }
        

        }
        else{
            camaraCooldown();  //Para apagar o luego prender la camara despues de 7 seg

            if(playerDetected){ //Para interaccion de la camara con player (Laser rojo o verde)
                iniciarLaserCooldown();
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
                Debug.Log("Entra el IF");
            }
            else{
                angleObjective = targetAngulo0;
                Debug.Log("Entra el Else");
               
            }

        }
    }


    private void iniciarLaserCooldown(){

        if(laserCoolDown < 5){
            laserCoolDown += 1.0f * Time.deltaTime;
        }
        else{
            laserCoolDown = 0.0f;
            playerDetected = false;
        }

    }

    private void camaraCooldown(){

        if(laserCoolDown < 9){
            laserCoolDown += 1.0f * Time.deltaTime;
        }
        else{
            laserCoolDown = 0.0f;
            cameraActivated = true;;
        }

    }

    private void OnDisable(){
        EventManager.AccionInterruptor -= InterruptorCamara;
    }


}
