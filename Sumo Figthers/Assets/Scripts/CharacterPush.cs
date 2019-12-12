using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPush : MonoBehaviour
{
    [SerializeField]
    public float force = 2f;
    [SerializeField]
    float shootAngle = 30;  // elevation angle

    const float distance_ray = 1.5f;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, distance_ray))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                Debug.Log("Did Hit");

                GameObject hittedObject = hit.transform.gameObject;
                Debug.Log("HIT: " + hittedObject);

                Vector3 positionThrowed = hittedObject.transform.position + (transform.forward * force * Time.deltaTime);

                hittedObject.GetComponent<Rigidbody>().AddForce(ParabollicVel(hittedObject.transform.position, positionThrowed, shootAngle), ForceMode.Impulse);  
            }
            else
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * distance_ray, Color.white);
                Debug.Log("Did not Hit");
            }
        }
    }

    private Vector3 ParabollicVel(Vector3 origin, Vector3 throwed_position, float angle) 
    {
        Vector3 dir = throwed_position - origin;  // get target direction
        float h = dir.y;  // get height difference
        dir.y = 0;  // retain only the horizontal direction
        
        float dist = dir.magnitude;  // get horizontal distance
        float radian_angle = angle * Mathf.Deg2Rad;  // convert angle to radians
        
        dir.y = dist* Mathf.Tan(radian_angle);  // set dir to the elevation angle
        dist += h / Mathf.Tan(radian_angle);  // correct for small height differences
         
        // calculate the velocity magnitude
         float vel = Mathf.Sqrt(dist * Physics.gravity.magnitude / Mathf.Sin(2 * radian_angle));
         
        return vel* dir.normalized;
    }
    
}
