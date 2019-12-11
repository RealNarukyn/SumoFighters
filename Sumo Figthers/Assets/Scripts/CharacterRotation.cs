using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRotation : MonoBehaviour
{
    [SerializeField]
    CharacterMovement controller;
    [SerializeField]
    float rotation_speed = 5f;

    private void Update()
    {
        if(controller.move != Vector3.zero)
           transform.rotation = Quaternion.Lerp(transform.rotation,Quaternion.LookRotation(controller.move),Time.deltaTime * rotation_speed);
    }

}
