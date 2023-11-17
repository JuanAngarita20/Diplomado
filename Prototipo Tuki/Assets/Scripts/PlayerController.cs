using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;
using UnityEngineInternal;
using UnityEngine.InputSystem;
using System.Security;
using System.Runtime.CompilerServices;


enum State {
        normal=0, jumping = 1, goingUp = 2
    }
   
enum Mov_Dir {
        still = 0, left=1, right = 2
    }

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 100.0f;
    [SerializeField] private float jumpStrength = 5.0f;
    [SerializeField] private float jumpMax = 0.0f;
    [SerializeField] private Rigidbody characterRigidBody = null;
    [SerializeField] private State estado = State.normal;
    [SerializeField] private Mov_Dir dir_mov = Mov_Dir.still;
    [SerializeField] private Transform VerificadorFrenteTransform;
    [SerializeField] private LayerMask WallMask;
    [SerializeField] private InputActionAsset inputActions = null;
    [SerializeField] private Camera cameraVideo = null;
  

    public Vector3 movement; 
    private bool checkWallOnFront = false;
    private bool moveDetected = false;
    private bool climbPossible  = false;
    private float timeForVideo = 0.0f;
    private bool stopMovement  = false;
   

    //Safe rotation
     
  

    // Start is called before the first frame update
    void Start()
    {
        estado = State.normal;
        dir_mov =Mov_Dir.right;
        movement = new Vector3(0.0f, 0.0f ,0.0f);
        timeForVideo = 0.0f;

        EventManager.TurnOnVideo += eventoVideoStarted; //Suscribirse al evento de Inicio de Video

    }

    private void eventoVideoStarted(){ //Respuesta al evento de Inicio de video
        stopMovement  = true;
        Debug.Log("Video Inicio");
    }


    // Update is called once per frame
    void Update()
    {

        //Verificar si camara2 esta activada
        if(stopMovement == true){
            iniciarvideo();
            //stopMovement  = true; -> Se activa con eventoVideoStarted
        }
        
       

        //Verificar input para girar
        if(Input.GetKey("right") ){
            
            /*characterRigidBody.transform.rotation = Quaternion.Euler(0, 0, 0);*/
            
            if(dir_mov == Mov_Dir.left){
             
                characterRigidBody.MoveRotation(Quaternion.AngleAxis(180.0f,Vector3.up)*characterRigidBody.rotation);
                Debug.Log("Girar derecha");
            }

            dir_mov = Mov_Dir.right;
            movement = new Vector3(1.0f, 0.0f ,0.0f);
            moveDetected = true;
            

           
        }
        
        if(Input.GetKey("left")){
            

            if(dir_mov == Mov_Dir.right){
        
                characterRigidBody.MoveRotation(Quaternion.AngleAxis(180.0f,Vector3.up)*characterRigidBody.rotation);
                Debug.Log("Girar izq");
            }


            dir_mov = Mov_Dir.left;
            movement = new Vector3(-1.0f, 0.0f ,0.0f);
            moveDetected = true;
            
        }
        
        //movement = new Vector3(Input.GetAxis("Horizontal"), 0.0f ,0.0f);
        
        
        if(Input.GetKeyDown("space") && jumpMax < 1 ){

            jumpMax++;
    
            characterRigidBody.AddForce(Vector3.up*jumpStrength,ForceMode.Impulse);
            estado = State.jumping;
            
            
        }

        //Accion Escalar Escalera
        if(Input.GetKey(KeyCode.A) && climbPossible){

            estado = State.goingUp;
            movement = new Vector3(0.0f, 1.0f ,0.0f);
            moveDetected = true;
            

        }


        //Esfera identificar si hay objeto adelante para arreglar movimiento
        if (Physics.CheckSphere(VerificadorFrenteTransform.position, 0.03f,WallMask)){
            Debug.Log("Pared adelante");
            checkWallOnFront = true;
        }
        else{
            checkWallOnFront = false;
        }
       
    }

    void FixedUpdate() //Movimiento
    {
       if(moveDetected && !stopMovement){ //Verificar si se detecto movimiento y si algun evento no le pidio que se detuviera
            moveCharacter(movement); // We call the function 'moveCharacter' in FixedUpdate for Physics movement
       }
       
    }


    void moveCharacter(Vector3 direction)
    {
       

        //Movimiento con Fisicas correctas - Mas pesado
        if(dir_mov == Mov_Dir.left){ //Para moverse correctamente cuando se gira
            if(!checkWallOnFront){
                Vector3 moveVector = direction*speed;
                characterRigidBody.velocity = new Vector3(moveVector.x, characterRigidBody.velocity.y, characterRigidBody.velocity.z);
                //Debug.Log(characterRigidBody.velocity);

            }
            
        }
        if(dir_mov == Mov_Dir.right){
            if(!checkWallOnFront){
                Vector3 moveVector = direction*speed;
                characterRigidBody.velocity = new Vector3(moveVector.x, characterRigidBody.velocity.y, characterRigidBody.velocity.z);
                //Debug.Log(characterRigidBody.velocity);
                

            }
        }

        if(estado == State.goingUp){
          
            Vector3 moveVector = direction*speed;
            characterRigidBody.velocity = new Vector3(characterRigidBody.velocity.x,moveVector.y, characterRigidBody.velocity.z);
            //Debug.Log(characterRigidBody.velocity);
            
        }

        //Modo movimeinto 2 - Fisicas Notan buenas

        /*characterRigidBody.Move(new Vector3(characterRigidBody.transform.position.x + (movement.x*speed*Time.fixedDeltaTime),characterRigidBody.transform.position.y,
        characterRigidBody.transform.position.z),Quaternion.identity*characterRigidBody.rotation);*/

        //Simpre debe Pasar
        
        movement = new Vector3(0.0f, 0.0f ,0.0f);
        moveDetected = false;
        //estado = State.normal; -> desactivado mejora el estado de GoingUp, pero no rompe el movimiento si se activa
        
        
    }

    private void iniciarvideo(){

        if(timeForVideo < 5){
            timeForVideo += 1.0f * Time.deltaTime;
        }
        else{
            stopMovement = false;
            timeForVideo = 0.0f;
            EventManager.VideoEnded();
        }

    }


    //Volver a dejarlo en estado normal cuando halla colisiones
    private void OnCollisionEnter(Collision collision){


        if(collision.gameObject.tag.Equals("Floor")){
            estado = State.normal;
            jumpMax = 0;
        }

        
    }


    private void OnTriggerEnter(Collider other){

        if(other.gameObject.tag == "Stairs"){
            climbPossible = true;
        }

    }

    private void OnTriggerExit(Collider other){
        if(other.gameObject.tag == "Stairs"){
            climbPossible = false;
            estado = State.normal;
        }

    }


        private void OnDisable(){
            EventManager.TurnOnVideo -= eventoVideoStarted;
        }




}