using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;
using UnityEngineInternal;
using UnityEngine.InputSystem;
using System.Security;
using System.Runtime.CompilerServices;
using FMOD.Studio;


enum State {
        normal=0, jumping = 1, goingUp = 2, falling = 3
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
    public bool climbPossible  = false;
    private bool stopMovement  = false;
    public bool grounded = false;
    public bool cinematic_On = false;
    public bool loseControl = false;
    public bool pushing = false;
    

    private float timeForVideo = 0.0f;
    private float timeForStill = 0.0f;
    private float jumpMax = 0.0f;
    public float moveCollider = 0.0f;

    //Variables Audio
    private EventInstance Pasos;
    private EventInstance Para;
    private EventInstance Salto;
    private EventInstance Chillidos;
    private EventInstance Emp;
    private EventInstance Esca;
  
    private bool isChillidosPlaying = false;
    private bool isEscalarPlaying = false;

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
        EventManager.StartLosingControl += EventLosingControl;
        EventManager.RegainControl += EventRegainControl;
        EventManager.GameOver += EventGameOver;
        EventManager.ZoneGameOver += EventGameOver;

        //Audio
        Pasos = AudioManager.instance.CreateInstance(FMODEvents.instance.Pasos);
        Para = AudioManager.instance.CreateInstance(FMODEvents.instance.Para);
        Salto = AudioManager.instance.CreateInstance(FMODEvents.instance.Salto);
        Chillidos = AudioManager.instance.CreateInstance(FMODEvents.instance.Chillidos);
        Emp = AudioManager.instance.CreateInstance(FMODEvents.instance.Emp);
        Esca = AudioManager.instance.CreateInstance(FMODEvents.instance.Esca);
        
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
        //Debug.Log("iniciar mov");
    }

    private void EventLosingControl(){ //Respuesta al evento de Inicio de video
        loseControl = true;
    }
    private void EventRegainControl(){ //Respuesta al evento de Inicio de video
        loseControl = false;
    }

     private void EventGameOver(){ //Respuesta al evento de Inicio de video
        stopMovement = true;
        //Debug.Log("GAME OVER");
    }


    //EVENTOS AUDIO
    private void PasosStart(){
       
        Pasos.start();
    }

    private void StopPasosSound()
    {
        // Detener la reproducción del evento de audio
        Pasos.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    private void ParaStart(){
        Debug.Log("para");
        Para.start();
    }

    private void SaltoStart(){
       
        Salto.start();
    }

    private void ChillidosStart(){
       
        Chillidos.start();
    }
    private void StopChillidosSound()
    {
        if (isChillidosPlaying)
        {
            Chillidos.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            isChillidosPlaying = false;
        }
    }

    private void EmpujarStart()
    {
        Emp.start();
    }

    private void StopEmpujarSound()
    {
        Emp.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    private void EscalarStart()
    {
        if (!isEscalarPlaying)
        {
            Esca.start();
            isEscalarPlaying = true;
        }
    }

    private void StopEscalarSound()
    {
        if (isEscalarPlaying)
        {
            Esca.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            isEscalarPlaying = false;
        }
    }


  




    //Prueba

    private void PlayPasosSound()
    {
        // Obtener el estado actual de reproducción
        //Debug.Log(Input.GetKeyDown("right"));
        PLAYBACK_STATE playbackState;
        Pasos.getPlaybackState(out playbackState);

        // Comprobar si el estado es diferente de PLAYING
        if (playbackState != PLAYBACK_STATE.PLAYING)
        {
            PasosStart();
        }
    }

    private void PlayChillidosSound()
    {
        if (!isChillidosPlaying)
        {
            ChillidosStart();
            isChillidosPlaying = true;
        }
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
        if(Input.GetKey("right") && !stopMovement && !(estado == State.goingUp)){
            
            /*characterRigidBody.transform.rotation = Quaternion.Euler(0, 0, 0);*/
            
            if(!loseControl){
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
                    if((estado == State.jumping) || (estado == State.falling)){
                        movement = new Vector3(1.15f, 0.0f ,0.0f);
                    }

                }

            }
            else{ //invertrir controles
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

            }
            
            //Animacion
            animator.SetBool("isWalking",true);

        }

        
        
        if(Input.GetKey("left") && !stopMovement && !(estado == State.goingUp)){
            

            if(!loseControl){
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
                    if((estado == State.jumping) || (estado == State.falling)){
                        movement = new Vector3(-1.15f, 0.0f ,0.0f);
                    }
                }

            }
            else{ // invertir controles
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
            }
           
            //Animacion
            animator.SetBool("isWalking",true);

            
        }

        //Debug.Log(moveDetected);
        
        //Debug.Log(Input.GetKeyDown("left"));

        //Caminado
        if(!moveDetected || !grounded){
            StopPasosSound(); // Detener sonido de pasos
        }

        if ((Input.GetKey(KeyCode.RightArrow)  || Input.GetKey(KeyCode.LeftArrow)) && estado != State.jumping && grounded) //Logica para Eventos SONIDO
        {
            
            if (moveDetected)
            {
                StopEscalarSound();
                PlayPasosSound(); // Iniciar sonido de pasos
            }
        }
        /*else if (!moveDetected || !grounded) // Se ejecuta cuando se sueltan las teclas
        {
            StopPasosSound(); // Detener sonido de pasos
        }*/
        
        
        
        
        //movement = new Vector3(Input.GetAxis("Horizontal"), 0.0f ,0.0f);
        
        
        if(Input.GetKeyDown("space") && jumpMax < 1 && !pushing){

            jumpMax++;
    
            characterRigidBody.AddForce(Vector3.up*jumpStrength,ForceMode.Impulse);
            estado = State.jumping;

            //EventoSaltar
            animator.SetBool("jump",true);
            SaltoStart();

            grounded  = false;
            Debug.Log("Saltar");

            //Mover Box de Fisicas
            /*myList[0].localPosition = new Vector3(2.419f, 1.16f, 0.0f);
            colliderTransform.localPosition =  new Vector3(0.0f, 0.85f, 0.0f);*/
           
            
        }

        if (Input.GetKeyDown(KeyCode.Space) && estado != State.jumping)
        {
            estado = State.jumping;
            // Otras acciones cuando inicia el salto
        }

        //Accion Escalar Escalera
        if(Input.GetKey("a") && climbPossible){


            /*if(colliderTransform.transform.rotation.eulerAngles.z == 90 ){
                colliderTransform.transform.Rotate(new Vector3(0f, 0f, 1f),90);
            }*/

            

            if(dir_mov == Mov_Dir.left){
                animator.SetBool("escaleraIzq",true);
                EscalarStart();
            }
            if(dir_mov == Mov_Dir.right){
                animator.SetBool("escaleraDer",true);
                EscalarStart();
            }

            
            estado = State.goingUp;
            movement = new Vector3(0.0f, 1.0f ,0.0f);
            moveDetected = true;
            grounded  = false;
            Debug.Log("Escalar");
            

        }
        

        if (Input.GetKeyUp("a") && climbPossible){ //Pra activar estado de caer cuando se suelta la tecla "a"
           animator.SetBool("escaleraIzq",false);
           animator.SetBool("escaleraDer",false);
           estado = State.falling;
           StopEscalarSound();
            Debug.Log("Escalar paro");
        }


        if (Input.GetKeyUp("right") || Input.GetKeyUp("left")){
            animator.SetBool("push", false);
            EventManager.NolongerPushing();
            pushing = false;
            StopEmpujarSound();
            //Debug.Log(animator.GetBool("push"));
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
            
            if(movement.x != 0 && !pushing){
                animator.SetBool("push", true);
                pushing = true;
                Debug.Log("Empizar animacion empujar");
                EventManager.CurrentlyPushing();
                EmpujarStart();
            }

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
            estado = State.falling;
            animator.SetBool("jump",false);

        }

        //Debug.Log(Mathf.Ceil(characterRigidBody.velocity.y));

        /*if( Mathf.Ceil(characterRigidBody.velocity.y) == 0.0f && grounded == false){
            grounded  = true;

        }*/
        

        //Chillidos
        if (!moveDetected && estado == State.normal)
        {
            PlayChillidosSound(); // Reproducir sonido de chillidos
        }
        else if (moveDetected || estado != State.normal)
        {
            StopChillidosSound(); // Detener sonido de chillidos
        }

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
            //ParaStart();
           
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

        //Escalar
        if(estado == State.goingUp){
          
            Vector3 moveVector = direction*speed;
            characterRigidBody.velocity = new Vector3(characterRigidBody.velocity.x,moveVector.y, characterRigidBody.velocity.z);
            
        }

        if(estado == State.falling){

            if(!checkWallOnFront){
                Vector3 moveVector = direction*speed;
                characterRigidBody.velocity = new Vector3(moveVector.x,characterRigidBody.velocity.y*1.02f, characterRigidBody.velocity.z);

            }

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
            movement.y = 0f; //Quitar peso gravedad
            jumpMax = 0;
            animator.SetBool("escaleraIzq",false);
            animator.SetBool("escaleraDer",false);
            animator.SetBool("jump",false);

            //Evento empujar(Talves)

            //animator.SetBool("push", false);
            //EventManager.NolongerPushing();
            

            grounded = true;
            //SaltoStart();

            //Regresar box de fisicas
            /*myList[0].localPosition = new Vector3(2.419f, 0.16f, 0.0f);
            colliderTransform.localPosition =  new Vector3(0.0f, -0.15f, 0.0f);*/

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
            EventManager.NolongerPushing();
            pushing = false;
            moveCollider = 0;
            colliderTransform.localPosition =  new Vector3(0.0f, -0.15f, 0.0f);

        }
    }

  

    private void OnTriggerEnter(Collider other){

        if(other.gameObject.tag == "Stairs" && grounded){
            climbPossible = true;
        }

    }

     private void OnTriggerStay(Collider other){

        if(other.gameObject.tag == "Stairs" && grounded){
            climbPossible = true;
        }

    }

  
    private void OnTriggerExit(Collider other){
        if(other.gameObject.tag == "Stairs"){
            climbPossible = false;
            estado = State.normal;
            animator.SetBool("escaleraIzq",false);
            animator.SetBool("escaleraDer",false);
            StopEscalarSound();
        }

    }


    private void OnDisable(){
        EventManager.TurnOnVideo -= EventoVideoStarted; //Suscribirse al evento de Inicio de Video
        EventManager.StopMovForAnim -= EventStopMovement;
        EventManager.RestartMovAfterAnim -= EventRestartMovement;
        EventManager.StartLosingControl -= EventLosingControl;
        EventManager.RegainControl -= EventRegainControl;
        EventManager.GameOver -= EventGameOver;

    }




}