using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterMovement : MonoBehaviour
{
    private float Speed = 6f;
    private float speed_limiter = 0.5f;
    private float JumpHeight = 2f;
    private float GroundDistance = 0.2f;
    
    //Maybe I can add it to the game.
    //private float DashDistance = 5f;
    
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

        Debug.Log("Player: " + player);

        if (!_isGrounded)
        {
            _inputs.x *= speed_limiter;
            _inputs.z *= speed_limiter;
        }
        

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
