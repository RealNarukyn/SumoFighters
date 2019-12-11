using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPush : MonoBehaviour
{
    [SerializeField]
    CharacterController controller;
    [SerializeField]
    public float force = 2f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                Debug.Log("Did Hit");

                GameObject hittedObject = hit.transform.gameObject;
                Debug.Log("HIT: " + hittedObject);

                Vector3 positionThrowed = hittedObject.transform.position + (transform.forward * force);


                hittedObject.transform.SetPositionAndRotation(positionThrowed, hittedObject.transform.rotation);
            }
            else
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
                Debug.Log("Did not Hit");
            }


        }
    }

    //private void OnDrawGizmos()
    //{
    //    // Draw a yellow sphere at the transform's position
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawLine(controller.transform.position, controller.transform.position.normalized);
    
    //}
}
