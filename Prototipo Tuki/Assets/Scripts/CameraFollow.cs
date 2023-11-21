using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform targetObject;

    [SerializeField] private Vector3 cameraOffset;
    [SerializeField] private float smoothFactor = 0.5f;
    private Vector3 posicionTuki;


    // Start is called before the first frame update
    void Start()
    {
        cameraOffset = targetObject.transform.position - new Vector3(targetObject.transform.position.x, targetObject.transform.position.y-3f, targetObject.transform.position.z+15f);
        //transform.position = new Vector3(targetObject.transform.position.x+2f, targetObject.transform.position.y+1f, targetObject.transform.position.z+10f);
        transform.position = targetObject.transform.position + cameraOffset;
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 newPosition = targetObject.transform.position + cameraOffset;
        //transform.position = newPosition;
        transform.position = Vector3.Slerp(transform.position,newPosition,smoothFactor);
        
    }
}
