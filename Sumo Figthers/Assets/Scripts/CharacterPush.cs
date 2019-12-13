using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPush : MonoBehaviour
{
    [SerializeField]
    float shootAngle;                                           // Elevation Angle

    private const float force_min = 0f;                         //Min Force that will be applied.
    private float force = force_min;                            //The FORCE that will be applied.
    private const float force_max = 400f;                       //Max Force it can be applied.

    private float multiplier = 5.53f;                           //The needed increment per frame to make it last 1.2 seconds.

    private bool has_punched = false;                           //Knows wheter the player has punched or not.
    private float timer = 0;                                    //Counter of the current time.
    private const float time_reset_punch = 0.2f;                //Time it will last the boolean HAS_PUNCHED.

    // Update is called once per frame
    void Update()
    {   
        if (Input.GetButton("Push"))
        {
            force += multiplier;

            if (force >= force_max)
            {
                force = force_max;
                multiplier *= -1;
            }

            if (force <= force_min)
            {
                force = force_min;
                multiplier *= -1;
            }
        }

        if (Input.GetButtonUp("Push"))
        {
            has_punched = true;
        }

        if (has_punched)
        {
            timer += Time.deltaTime;
            if (timer >= time_reset_punch)
            {
                ResetPunchStats();
            }
        }
    }

    private void OnDrawGizmos()
    {
        float percent = force / force_max;
        Gizmos.DrawSphere(transform.position, percent);           //For the moment is the way you can know visually with the force you're hitting. 
    }

    private void OnTriggerStay(Collider other)
    {
        if (has_punched)
        {
            Vector3 positionThrowed = other.transform.position + (transform.forward * force * Time.deltaTime);

            if(other.GetComponent<Rigidbody>())
                other.GetComponent<Rigidbody>().AddForce(ParabollicVel(other.transform.position, positionThrowed, shootAngle), ForceMode.Impulse);


            ResetPunchStats();
        }
    }


    private Vector3 ParabollicVel(Vector3 origin, Vector3 throwed_position, float angle) 
    {
        Vector3 dir = throwed_position - origin;                                             //Get target direction
        float h = dir.y;                                                                     //Get height difference
        dir.y = 0;                                                                           //Retain only the horizontal direction
        
        float dist = dir.magnitude;                                                          //Get horizontal distance
        float radian_angle = angle * Mathf.Deg2Rad;                                          //Convert angle to radians
        
        dir.y = dist* Mathf.Tan(radian_angle);                                               //Set dir to the elevation angle
        dist += h / Mathf.Tan(radian_angle);                                                 //Correct for small height differences

        // calculate the velocity magnitude
        float vel = Mathf.Sqrt(dist * Physics.gravity.magnitude / Mathf.Sin(2 * radian_angle));
         
        return vel* dir.normalized;
    }

    private void ResetPunchStats() 
    {
        has_punched = false;
        force = 0;
        timer = 0;
    }



}

