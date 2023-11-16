using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    
    [SerializeField] private float batteryCharge = 50.0f;
    [SerializeField] private float batteryChargeRate = 5.0f;

    void Start()
    {
        batteryCharge = 30.0f;
        batteryChargeRate = 0.5f;
        
    }

    // Update is called once per frame
    void Update()
    {
        //batteryCharge += batteryChargeRate*Time.deltaTime;
        //Debug.Log(batteryCharge);
    }
}
