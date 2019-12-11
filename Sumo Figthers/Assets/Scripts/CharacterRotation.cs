using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRotation : MonoBehaviour
{
    [SerializeField]
    CharacterMovement controller;
    
    [SerializeField]
    float rotation_speed = 30f;

    float actualAngle = 0.0f;
    private void Update()
    {
        //if (controller.transform.position.x > 0)
        //    transform.Rotate(controller.transform.position.x < 0 ? -Vector3.up * rotation_speed * Time.deltaTime : Vector3.up * rotation_speed * Time.deltaTime);
       
        if(controller.move != Vector3.zero)
        transform.rotation = Quaternion.Lerp(transform.rotation,Quaternion.LookRotation(controller.move),Time.deltaTime * rotation_speed);
        //transform.Rotate(controller.transform.position.x < 0 ? -Vector3.up * rotation_speed * Time.deltaTime : Vector3.up * rotation_speed * Time.deltaTime);
    }

}
