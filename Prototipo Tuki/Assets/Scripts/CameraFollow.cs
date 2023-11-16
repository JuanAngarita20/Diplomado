using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform targetObject;

    [SerializeField] private Vector3 cameraOffset;
    [SerializeField] private float smoothFactor = 0.5f;


    // Start is called before the first frame update
    void Start()
    {
        cameraOffset = transform.position - targetObject.transform.position;
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 newPosition = targetObject.transform.position + cameraOffset;
        //transform.position = newPosition;
        transform.position = Vector3.Slerp(transform.position,newPosition,smoothFactor);
        
    }
}
