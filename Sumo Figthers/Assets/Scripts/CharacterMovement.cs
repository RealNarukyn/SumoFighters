using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField]
    CharacterController controller;
    [SerializeField]
    public float speed = 12f;

    public float RotateSpeed = 30f;

    public float jumpHeight = 3.0f;

    Vector3 velocity;
    bool isGrounded;
   public Vector3 move;
    //int angle_direction = 1;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;


    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");


         move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);
        }


        velocity += Physics.gravity * Time.deltaTime * 2f;
        controller.Move(velocity * Time.deltaTime);

        
        //if ( x < 0 && z > 0 || x < 0 && z < 0 )
        //{
        //    angle_direction = -1;
        //}

        //if (x > 0 && z > 0 || x > 0 && z < 0)
        //{
        //    angle_direction = 1;
        //}

        //transform.Rotate(Vector3.up * RotateSpeed * angle_direction * Time.deltaTime);

        //   transform.Rotate(x < 0 ? -Vector3.up * RotateSpeed * Time.deltaTime : Vector3.up * RotateSpeed * Time.deltaTime);
    }
}
