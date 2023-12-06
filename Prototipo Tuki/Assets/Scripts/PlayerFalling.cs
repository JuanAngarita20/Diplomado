using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFalling : MonoBehaviour
{
    [SerializeField] private Rigidbody rg;
    [SerializeField] private Animator animator = null;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(rg.velocity.y );
        if(rg.velocity.y < -2f){
            //Debug.Log("Cayendo!!");
            animator.SetBool("jump",false);
            animator.SetBool("falling",true);

        }
        else{
            animator.SetBool("falling",false);
        }

       
{
   // Avoid any reload.
}

    }

      private void OnCollisionEnter(Collision collision){


        if(collision.gameObject.tag.Equals("Floor")){
            animator.SetBool("falling",false);
        }

        
    }

}
