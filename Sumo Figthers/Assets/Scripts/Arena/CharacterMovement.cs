using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterMovement : MonoBehaviour
{
    public float Speed = 8f;
    public float JumpHeight = 2f;
    public float GroundDistance = 0.2f;
    public float DashDistance = 5f;
    public LayerMask Ground;

    
    private Vector3 _inputs = Vector3.zero;
    private bool _isGrounded = true;

    [SerializeField]
    private Rigidbody _body;
    [SerializeField]
    private Transform _groundChecker;


    public void Move(int player)
    {
        _isGrounded = Physics.CheckSphere(_groundChecker.position, GroundDistance, Ground, QueryTriggerInteraction.Ignore);

        _inputs = Vector3.zero;

        _inputs.x = Input.GetAxis("Joy" + player + "X");
        _inputs.z = Input.GetAxis("Joy" + player + "Z") * -1;

        if (_inputs != Vector3.zero)
            transform.forward = _inputs;
    }

    public void Jump(int player)
    {
        if(_isGrounded)
            _body.AddForce(Vector3.up * Mathf.Sqrt(JumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
    }

    public void FixMove()
    {
        _body.MovePosition(_body.position + _inputs * Speed * Time.fixedDeltaTime);
    }
}
