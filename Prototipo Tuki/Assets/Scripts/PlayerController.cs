using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;
using UnityEngineInternal;


enum State {
        normal=0, jumping = 1, falling = 2
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
  

    public Vector3 movement; 
    private bool checkWallOnFront = false;


    //Safe rotation
     
  

    // Start is called before the first frame update
    void Start()
    {
        estado = State.normal;
        dir_mov =Mov_Dir.right;
        movement = new Vector3(0.0f, 0.0f ,0.0f);
    
    }

    // Update is called once per frame
    void Update()
    {
       

        //Verificar input para girar
        if(Input.GetKey("right") ){
            
            /*characterRigidBody.transform.rotation = Quaternion.Euler(0, 0, 0);*/
            
            if(dir_mov == Mov_Dir.left){
                //gameObject.transform.rotation =  Quaternion.AngleAxis(180.0f,Vector3.up)*gameObject.transform.rotation; -> TECNICAMENTE DEBERIA SERVIR, SUPONGO QUE NO
                // POR USAR transformp,rotate cuando hay fisicas
            
                characterRigidBody.MoveRotation(Quaternion.AngleAxis(180.0f,Vector3.up)*characterRigidBody.rotation);
                //characterRigidBody.rotation = Quaternion.Euler(0, 180, 0)*characterRigidBody.rotation;
                Debug.Log("Girar derecha");
            }

            dir_mov = Mov_Dir.right;
            movement = new Vector3(1.0f, 0.0f ,0.0f);
            

           
        }
        
        if(Input.GetKey("left")){
            
            /*characterRigidBody.transform.rotation = Quaternion.Euler(0, 180, 0);*/
            //gameObject.transform.rotation =  Quaternion.Euler(0, 180, 0);

            if(dir_mov == Mov_Dir.right){
                
                characterRigidBody.MoveRotation(Quaternion.AngleAxis(180.0f,Vector3.up)*characterRigidBody.rotation);
                //characterRigidBody.rotation = Quaternion.Euler(0, 180, 0)*characterRigidBody.rotation;
                Debug.Log("Girar izq");
            }


            dir_mov = Mov_Dir.left;
            movement = new Vector3(-1.0f, 0.0f ,0.0f);
            
        }
        
        //movement = new Vector3(Input.GetAxis("Horizontal"), 0.0f ,0.0f);
        

        
        if(Input.GetKeyDown("space") && jumpMax < 1 ){

            jumpMax++;
    
            characterRigidBody.AddForce(Vector3.up*jumpStrength,ForceMode.Impulse);
            estado = State.jumping;
            
            
        }

        if (Physics.CheckSphere(VerificadorFrenteTransform.position, 0.03f,WallMask )){
            Debug.Log("Pared adelante");
            checkWallOnFront = true;
        }
        else{
            checkWallOnFront = false;
        }
        
       

    }

    void FixedUpdate()
    {
       
       moveCharacter(movement); // We call the function 'moveCharacter' in FixedUpdate for Physics movement

       
    }


    void moveCharacter(Vector3 direction)
    {
        //int verificarNumero;

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
        

        //Movimiento con Fisicas no tan correctas - Menos pesado
        /*if(Input.GetKey("right")  && dir_mov == Mov_Dir.right){
            if(!checkWallOnFront){
                
                characterRigidBody.Move(new Vector3(characterRigidBody.transform.position.x + (speed*Time.fixedDeltaTime),characterRigidBody.transform.position.y,
                characterRigidBody.transform.position.z),Quaternion.identity);

            }
            
        }

        
        if(Input.GetKey("left")  && dir_mov == Mov_Dir.left){
            if(!checkWallOnFront){
                characterRigidBody.Move(new Vector3(characterRigidBody.transform.position.x + (-speed*Time.fixedDeltaTime),characterRigidBody.transform.position.y,
                characterRigidBody.transform.position.z),Quaternion.identity);

            }

            
        }*/



        movement = new Vector3(0.0f, 0.0f ,0.0f);
        
        

        
    }


    //Volver a dejarlo en estado normal cuando halla colisiones
    private void OnCollisionEnter(Collision collision){


        if(collision.gameObject.tag.Equals("Floor")){
            estado = State.normal;
            jumpMax = 0;
        }
       
    }





}