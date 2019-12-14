using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterMovement : MonoBehaviour
{
    public float Speed = 5f;
    public float JumpHeight = 2f;
    public float GroundDistance = 0.2f;
    public float DashDistance = 5f;
    public LayerMask Ground;

    
    private Vector3 _inputs = Vector3.zero;
    public bool _isGrounded = true;

    [SerializeField]
    private Rigidbody _body;
    [SerializeField]
    private Transform _groundChecker;


    public void Move(int player)
    {
        Debug.Log("Player: " + player);

        _isGrounded = Physics.CheckSphere(_groundChecker.position, GroundDistance, Ground, QueryTriggerInteraction.Ignore);

        _inputs = Vector3.zero;
        switch (player)
        {
            case 0:
                _inputs.x = Input.GetAxis("Joy0X");
                _inputs.z = Input.GetAxis("Joy0Z") * -1;
                break;
            case 1:
                _inputs.x = Input.GetAxis("Joy1X");
                _inputs.z = Input.GetAxis("Joy1Z") * -1;
                break;
            case 2:
                _inputs.x = Input.GetAxis("Joy2X");
                _inputs.z = Input.GetAxis("Joy2Z") * -1;
                break;
            case 3:
                _inputs.x = Input.GetAxis("Joy3X");
                _inputs.z = Input.GetAxis("Joy3Z") * -1;
                break;
            default: break;
        }

        //_inputs.x = Input.GetAxis("Horizontal");
        //_inputs.z = Input.GetAxis("Vertical");


        Debug.Log("Player: " + player + " -- Inputs: " + _inputs);

        if (_inputs != Vector3.zero)
            transform.forward = _inputs;
    }

    public void Jump()
    {
        if(_isGrounded)
            _body.AddForce(Vector3.up * Mathf.Sqrt(JumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
    }

    void FixedUpdate()
    {
        _body.MovePosition(_body.position + _inputs * Speed * Time.fixedDeltaTime);
    }
}
