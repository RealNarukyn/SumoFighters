using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private float Speed = 7f;
    private float speed_limiter = 0.5f;
    private float JumpHeight = 2f;
    private float GroundDistance = 0.2f;

    private int whoIAm;
    public int getPlayer() { return whoIAm; }
    public void setPlayer(int player) { whoIAm = player; }


    public bool amIFighting = true;

    //Maybe I can add it to the game.
    //private float DashDistance = 5f;
    
    public LayerMask Ground;
    public LayerMask Floor;

    
    private Vector3 _inputs = Vector3.zero;
    private bool _isGrounded = true;
    private bool _inFloor = false;
    private bool _alreadyChecked = false;
    public bool touchingFloor() { return _inFloor; }
    public bool alreadyChecked() { return _alreadyChecked; }

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

        if (!_isGrounded)
        {
            _inputs.x *= speed_limiter;
            _inputs.z *= speed_limiter;
        }
        

        if (_inputs != Vector3.zero)
            transform.forward = _inputs;
    }

    public void Jump()
    {
        if (_isGrounded)
        {
            _body.AddForce(Vector3.up * Mathf.Sqrt(JumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);

            //AudioManager.Instance.PlaySFX((int)AudioManager.SFXSounds.Jump);
        }
        
    }

    public void FixMove()
    {
        _inFloor = Physics.CheckSphere(_groundChecker.position, GroundDistance, Floor, QueryTriggerInteraction.Ignore);

        if(_inputs.magnitude > 0.4f && !_inFloor)
            _body.MovePosition(_body.position + _inputs * Speed * Time.fixedDeltaTime);
        
        if (_inFloor && !_alreadyChecked)
        {
            GetComponent<CharacterMovement>().enabled = false;
            GetComponentInChildren<CharacterPush>().enabled = false;

            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }
    }

    public void changeCheckedCondition(bool condition) { _alreadyChecked = condition; }
}
