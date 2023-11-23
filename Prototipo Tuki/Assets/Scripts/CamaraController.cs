using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraController : MonoBehaviour
{
    [SerializeField] private List<int> idInterruptores;
    [SerializeField] private Vector3 angulo0;
    [SerializeField] private Vector3 angulo1;
    [SerializeField] private int compToCheck;

    public Quaternion targetAngulo0 = Quaternion.Euler(0,0,0);
    public Quaternion targetAngulo1 = Quaternion.Euler(0,0,0);
    private Quaternion angleObjective;

    void Start()
    {
        //Rotacion Animaciones
        targetAngulo0 = Quaternion.Euler(angulo0.x,angulo0.y,angulo0.z);
        targetAngulo1 = Quaternion.Euler(angulo1.x,angulo1.y,angulo1.z);
        angleObjective = targetAngulo0;
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation,angleObjective,0.02f);
        if(transform.rotation.eulerAngles == targetAngulo0.eulerAngles){
            Debug.Log("Cambia Direccion");
            changeCurrectAngle();
           
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
}
