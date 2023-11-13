using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;


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
    [SerializeField] private float speedRight = 5.0f;
    [SerializeField] private float speedLeft = 5.0f;

    [SerializeField] private Rigidbody characterRigidBody = null;
    [SerializeField] private State estado = State.normal;
    [SerializeField] private Mov_Dir dir_mov = Mov_Dir.still;
  
    private bool OnColli_Inmov = false ;
    

    public Vector3 movement; 


    // Start is called before the first frame update
    void Start()
    {
        estado = State.normal;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxis("Horizontal") >= 1){
            dir_mov = Mov_Dir.right;
        }
        else if(Input.GetAxis("Horizontal") <= 1){
            dir_mov = Mov_Dir.left;
        }
        else{
             dir_mov = Mov_Dir.still;
        }

        movement = new Vector3(Input.GetAxis("Horizontal"), 0.0f ,0.0f);
        //moveCharacter(movement);

        
        if(Input.GetKeyDown("space") && (estado == State.normal||dir_mov == Mov_Dir.right||dir_mov == Mov_Dir.left) && jumpMax < 1 ){

            jumpMax++;
    
            characterRigidBody.AddForce(Vector3.up*jumpStrength,ForceMode.Impulse);
            estado = State.jumping;
            
        }
        
      

    }

    void FixedUpdate()
    {
       moveCharacter(movement); // We call the function 'moveCharacter' in FixedUpdate for Physics movement
    }

    void moveCharacter(Vector3 direction)
    {
        if (!OnColli_Inmov){
            Vector3 moveVector = transform.TransformDirection(direction)*speed;
            characterRigidBody.velocity = new Vector3(moveVector.x, characterRigidBody.velocity.y, characterRigidBody.velocity.z);

        }
       
    }



    //Volver a dejarlo en estado normal cuando halla colisiones
    private void OnCollisionEnter(Collision collision){

        if(collision.gameObject.tag.Equals("inmovible")){
            OnColli_Inmov = true;
        }


        if(collision.gameObject.tag.Equals("Floor")){
            estado = State.normal;
            jumpMax = 0;
            OnColli_Inmov = false;

            //OnColli_Inmov = false;
        }
       
    }




}
