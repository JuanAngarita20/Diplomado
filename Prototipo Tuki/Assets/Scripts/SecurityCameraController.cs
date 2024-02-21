using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class SecurityCameraController : MonoBehaviour
{
    
    [SerializeField] private List<int> idInterruptores;
    [SerializeField] private Vector3 angulo0;
    [SerializeField] private Vector3 angulo1;
    [SerializeField] private int compToCheck;


    [SerializeField] private LayerMask MaskToFind;
    [SerializeField] private float distMaxLaser;
    [SerializeField] private int idInterruptor;
    [SerializeField] private GameObject indicador;
    [SerializeField] private Light luzCamara1;


    private EventInstance CamaraAlarm;

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

        indicador.SetActive(true);
        
        //Audio
        CamaraAlarm = AudioManager.instance.CreateInstance(FMODEvents.instance.CamaraAlarm);

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
                    CamaraAlarm.start();
                    

                    
                }

            }
            if(playerDetected){
                iniciarLaserCooldown(); //Para interaccion de la camara con player (Laser rojo o verde)
                Debug.DrawRay(rayo.origin,rayo.direction*distMaxLaser,Color.red);
                luzCamara1.color = Color.red;
                
            }
            else{
                Debug.DrawRay(rayo.origin,rayo.direction*distMaxLaser,Color.green);
                luzCamara1.color = Color.green;
            }
        

        }
        else{
            camaraCooldown();  //Para apagar o luego prender la camara despues de 7 seg

            if(playerDetected){ //Para interaccion de la camara con player (Laser rojo o verde)
                iniciarLaserCooldown();
            }
        }
        
       


       
       
        /*transform.rotation = Quaternion.Slerp(transform.rotation,angleObjective,0.02f);
        if((transform.rotation.eulerAngles - targetAngulo1.eulerAngles) == new Vector3(0f,0f,0f)){
            Debug.Log("Cambia Direccion a 25");
            changeCurrectAngle();
           
        }
        if(transform.rotation.eulerAngles == targetAngulo0.eulerAngles){
            Debug.Log("Cambia Direccion a 35");
            changeCurrectAngle();
           
        }
        if(transform.rotation.eulerAngles == new Vector3(0.00f,0.00f,35.00f)){
            Debug.Log("Cambia Direccion a 25");
            changeCurrectAngle();
           
        }*/
        

        //Debug.Log("Objetivo: "+ angleObjective.eulerAngles);
        //Debug.Log("Target 0: "+targetAngulo0.eulerAngles);
        //Debug.Log("Target 1: "+ targetAngulo1.eulerAngles);
        //Debug.Log("Camara: "+ transform.rotation.eulerAngles);
        //Debug.Log("Resta: "+ (transform.rotation.eulerAngles - targetAngulo1.eulerAngles));
        
        
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

        if(laserCoolDown < 15){
            laserCoolDown += 1.0f * Time.deltaTime;
            indicador.SetActive(false);
            luzCamara1.intensity = 0f;
        }
        else{
            laserCoolDown = 0.0f;
            cameraActivated = true;
            indicador.SetActive(true);
            luzCamara1.intensity = 9.5f;
        }

    }

    private void OnDisable(){
        EventManager.AccionInterruptor -= InterruptorCamara;
    }






}
