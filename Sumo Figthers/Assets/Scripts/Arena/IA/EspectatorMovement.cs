using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EspectatorMovement : MonoBehaviour
{
    [SerializeField]
    private Material[] materials;
    [SerializeField]
    private Rigidbody rb;

    private float JumpHeight = 2f;

    // Start is called before the first frame update
    void Start()
    {
        transform.LookAt(new Vector3(0,0,0));

        int randNumber = Random.Range(0, materials.Length);
        GetComponent<MeshRenderer>().material = materials[randNumber];

        int randomTime = Random.Range(0, 6);

        InvokeRepeating("Jump", 2, randomTime);
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * Mathf.Sqrt(JumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
    }

    

}
