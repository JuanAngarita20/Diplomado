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
    [SerializeField] private Rigidbody characterRigidBody = null;
    [SerializeField] private State estado = State.normal;
    [SerializeField] private Mov_Dir dir_mov = Mov_Dir.still;
    [SerializeField] private Transform VerificadorFrenteTransform;
    [SerializeField] private  List<Transform> myList;
    [SerializeField] private LayerMask WallMask;
    [SerializeField] private LayerMask MoveMask;
    [SerializeField] private LayerMask DestroyMask;
    [SerializeField] private Camera cameraVideo = null;
    [SerializeField] private Animator animator = null;
    [SerializeField] private Transform colliderTransform = null;
    [SerializeField] private Transform modelTransform = null;
  

    public Vector3 movement; 
    private bool checkWallOnFront = false;
    private bool moveDetected = false;
    private bool climbPossible  = false;
    private bool stopMovement  = false;
    public bool grounded = false;
     public bool cinematic_On = false;
    

    private float timeForVideo = 0.0f;
    private float timeForStill = 0.0f;
    private float jumpMax = 0.0f;
    public float moveCollider = 0.0f;

   

    //Safe rotation
     
  

    // Start is called before the first frame update
    void Start()
    {
        estado = State.normal;
        dir_mov =Mov_Dir.right;
        movement = new Vector3(0.0f, 0.0f ,0.0f);
        timeForVideo = 0.0f;

        EventManager.TurnOnVideo += EventoVideoStarted; //Suscribirse al evento de Inicio de Video
        EventManager.StopMovForAnim += EventStopMovement;
        EventManager.RestartMovAfterAnim += EventRestartMovement;

    }

    //Funciones de Eventos
    private void EventoVideoStarted(){ //Respuesta al evento de Inicio de video
        stopMovement  = true;
        cinematic_On = true;
        Debug.Log("Video Inicio");
    }

    private void EventStopMovement(){ //Respuesta al evento de Inicio de video
        stopMovement  = true;
        Debug.Log("stop mov");
    }

    private void EventRestartMovement(){ //Respuesta al evento de Inicio de video
        stopMovement  = false;
        Debug.Log("iniciar mov");
    }




    // Update is called once per frame
    void Update()
    {

        //Verificar si camara2 esta activada
        if(cinematic_On == true){
            iniciarvideo();
            //stopMovement  = true; -> Se activa con eventoVideoStarted
        }
        
       

        //Verificar input para girar
        if(Input.GetKey("right") && !stopMovement){
            
            /*characterRigidBody.transform.rotation = Quaternion.Euler(0, 0, 0);*/
            
            if(dir_mov == Mov_Dir.left){
            
                
                characterRigidBody.MoveRotation(Quaternion.AngleAxis(180.0f,Vector3.up)*characterRigidBody.rotation);
               
                dir_mov = Mov_Dir.right;
                movement = new Vector3(0.0f, 0.0f ,0.0f);
                moveDetected = true;
                
               
                
                Debug.Log("Girar derecha");
            }
            else{
                dir_mov = Mov_Dir.right;
                movement = new Vector3(1.0f, 0.0f ,0.0f);
                moveDetected = true;

            }

            
            //Caminar
            //Animacion
            animator.SetBool("isWalking",true);

           
        }
        
        if(Input.GetKey("left") && !stopMovement){
            

            if(dir_mov == Mov_Dir.right){
                characterRigidBody.MoveRotation(Quaternion.AngleAxis(180.0f,Vector3.up)*characterRigidBody.rotation);
                
                dir_mov = Mov_Dir.left;
                movement = new Vector3(0.0f, 0.0f ,0.0f);
                moveDetected = true;
                
                
                
                Debug.Log("Girar izq");
            }
            else{
                dir_mov = Mov_Dir.left;
                movement = new Vector3(-1.0f, 0.0f ,0.0f);
                moveDetected = true;

            }


            
            //EventoCaminar
            //Animacion
            animator.SetBool("isWalking",true);

            
        }

      
        
        //movement = new Vector3(Input.GetAxis("Horizontal"), 0.0f ,0.0f);
        
        
        if(Input.GetKeyDown("space") && jumpMax < 1){

            jumpMax++;
    
            characterRigidBody.AddForce(Vector3.up*jumpStrength,ForceMode.Impulse);
            estado = State.jumping;
            //EventoSaltar
            animator.SetBool("jump",true);

            grounded  = false;
            
        }

        //Accion Escalar Escalera
        if(Input.GetKey("a") && climbPossible && (grounded == true)){


            /*if(colliderTransform.transform.rotation.eulerAngles.z == 90 ){
                colliderTransform.transform.Rotate(new Vector3(0f, 0f, 1f),90);
            }*/

            if(dir_mov == Mov_Dir.left){
                animator.SetBool("escaleraIzq",true);
            }
            if(dir_mov == Mov_Dir.right){
                animator.SetBool("escaleraDer",true);
            }

            
            estado = State.goingUp;
            movement = new Vector3(0.0f, 1.0f ,0.0f);
            moveDetected = true;
            grounded  = false;
            

        }
        else{
            animator.SetBool("escaleraIzq",false);
            animator.SetBool("escaleraDer",false);
        }

        /*
        //Esfera identificar si hay objeto adelante para arreglar movimiento
        if (Physics.CheckSphere(myList[0].position, 0.03f,WallMask)){
            //Debug.Log("Pared adelante 1");
            //checkWallOnFront = true;
           
            
        }
        else if (Physics.CheckSphere(myList[1].position, 0.03f,WallMask)){
            //Debug.Log("Pared adelante 2");
            //checkWallOnFront = true;
            
            
        }
        else if (Physics.CheckSphere(myList[2].position, 0.03f,WallMask)){
            //Debug.Log("Pared adelante 3");
            //checkWallOnFront = true;
           
            
        }
        else{
            checkWallOnFront = false;
        }*/

        if(Physics.CheckBox(myList[0].position,new Vector3(0.03f,0.7f,0.6f),Quaternion.identity,WallMask)){
            Debug.Log("Caja detecto");
            checkWallOnFront = true;
        }
        else{
            checkWallOnFront = false;
        }


        if(Physics.CheckBox(myList[0].position,new Vector3(0.03f,0.7f,0.6f),Quaternion.identity,MoveMask)  && (grounded == false)){
            Debug.Log("Movil adelante");
            movement = new Vector3(0.0f, -5.0f ,0.0f);
        }

        if(Physics.CheckBox(myList[0].position,new Vector3(0.03f,0.7f,0.6f),Quaternion.identity,DestroyMask)  && (grounded == false)){
            Debug.Log("destruible adelante");
            movement = new Vector3(0.0f, -5.0f ,0.0f);
        }

        if(Physics.CheckBox(myList[0].position,new Vector3(0.03f,0.7f,0.6f),Quaternion.identity,MoveMask)  && (grounded == true)){
            //Eventoenpuja
            animator.SetBool("push", true);

            moveCollider +=1;
            
            if(moveCollider == 1f){
                colliderTransform.localPosition =  new Vector3(-0.8f, -0.15f, 0.0f);
            }
            
            
        }
        


        /*
        //Identificar si hay colision con objeto movible
        if (Physics.CheckSphere(myList[0].position, 0.03f,MoveMask) && (grounded == false)){
            Debug.Log("Movil adelante 1");
            movement = new Vector3(0.0f, -5.0f ,0.0f);
            
        }
        else if (Physics.CheckSphere(myList[1].position, 0.03f,MoveMask) && (grounded == false)){
            Debug.Log("Movil  adelante 2");
            movement = new Vector3(0.0f, -5.0f ,0.0f);
           
        }
        else if (Physics.CheckSphere(myList[2].position, 0.03f,MoveMask) && (grounded == false)){
            Debug.Log("Movil  adelante 3");
           movement = new Vector3(0.0f, -5.0f ,0.0f);
        
        }*/



        if(characterRigidBody.velocity.y < -2f){
            grounded  = false;

        }

        //Debug.Log(Mathf.Ceil(characterRigidBody.velocity.y));

        /*if( Mathf.Ceil(characterRigidBody.velocity.y) == 0.0f && grounded == false){
            grounded  = true;

        }*/
        
   

    }

    void FixedUpdate() //Movimiento
    {
       if(moveDetected && !stopMovement){ //Verificar si se detecto movimiento y si algun evento no le pidio que se detuviera
            moveCharacter(movement); // We call the function 'moveCharacter' in FixedUpdate for Physics movement
       }
       else{
        timeForStill += 1.0f * Time.fixedDeltaTime; //Para activar idle

        if(timeForStill>0.1f){
            //dir_mov = Mov_Dir.still;
            timeForStill = 0;
            //Poner animacion Idle
            //Debug.Log("Animacion Still");
            //EventoChillidos
            animator.SetBool("isWalking",false);
            //EventoDetener caminado

        }

        //dir_mov = Mov_Dir.still;
       }
       
    }


    void moveCharacter(Vector3 direction)
    {
        //animator.SetBool("rotate",false);
       
       //Modo movimiento 2 - Fisicas Notan buenas - Sirve en modo lento y Rapido - HAY QUE TENER ENCENDIDO INTERPOLATE

        if(dir_mov == Mov_Dir.left){ //Para moverse correctamente cuando se gira
            if(!checkWallOnFront){
                Vector3 moveVector = direction*speed;
                characterRigidBody.velocity = new Vector3(moveVector.x,characterRigidBody.velocity.y, characterRigidBody.velocity.z);
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

        //Saltar
        if(estado == State.goingUp){
          
            Vector3 moveVector = direction*speed;
            characterRigidBody.velocity = new Vector3(characterRigidBody.velocity.x,moveVector.y, characterRigidBody.velocity.z);
            
        }

        //Modo movimiento 2 - Fisicas No buenas en modo Rapido - Sirve en modo lento en speed = 20

        
        /*if(dir_mov == Mov_Dir.left){ 
            if(!checkWallOnFront){
                Vector3 moveVector = new Vector3(direction.x,direction.y,direction.z);
                characterRigidBody.Move(characterRigidBody.transform.position + speed * Time.fixedDeltaTime * moveVector,Quaternion.identity*characterRigidBody.rotation);
            }
            
        }
        if(dir_mov == Mov_Dir.right){
            if(!checkWallOnFront){
                Vector3 moveVector = new Vector3(direction.x,direction.y,direction.z);
                characterRigidBody.Move(transform.position + speed * Time.fixedDeltaTime * moveVector,Quaternion.identity*characterRigidBody.rotation);
                
            }
        }*/

        



        //Simpre debe Pasar
        movement = new Vector3(0.0f, 0.0f ,0.0f);
        moveDetected = false;

        if(grounded){
            estado = State.normal; //-> desactivado mejora el estado de GoingUp, pero no rompe el movimiento si se activa
        }
        
        
        
    }

    private void iniciarvideo(){

        if(timeForVideo < 5){
            timeForVideo += 1.0f * Time.deltaTime;
        }
        else{
            stopMovement = false;
            cinematic_On = false;
            timeForVideo = 0.0f;
            EventManager.VideoEnded();
        }

    }


    //Volver a dejarlo en estado normal cuando halla colisiones
    private void OnCollisionEnter(Collision collision){


        if(collision.gameObject.tag.Equals("Floor")){
            estado = State.normal;
            jumpMax = 0;
            animator.SetBool("escaleraIzq",false);
            animator.SetBool("escaleraDer",false);
            animator.SetBool("jump",false);
            //Eventoempujar(Talves)
            animator.SetBool("push", false);
            //animator.SetBool("falling",false);

            grounded = true;

            /*if(colliderTransform.transform.rotation.eulerAngles.z == 180 ){
                colliderTransform.transform.Rotate(new Vector3(0f, 0f, 1f),-90);
            }*/
          
        }

        if(collision.gameObject.layer.ToString() == "8"){
            Debug.Log("Entro en contacto con un moveable");

        }
        
        
    }

   

    private void OnCollisionExit(Collision collision){

        if(collision.gameObject.layer.ToString() == "8"){
            //Eventoempujar
            animator.SetBool("push", false);
            moveCollider = 0;
            colliderTransform.localPosition =  new Vector3(0.0f, -0.15f, 0.0f);

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
            animator.SetBool("escaleraIzq",false);
            animator.SetBool("escaleraDer",false);
        }

    }


    private void OnDisable(){
        EventManager.TurnOnVideo -= EventoVideoStarted; //Suscribirse al evento de Inicio de Video
        EventManager.StopMovForAnim -= EventStopMovement;
        EventManager.RestartMovAfterAnim -= EventRestartMovement;

    }




}