using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPush : MonoBehaviour
{
    [SerializeField]
    float shootAngle;                                           // Elevation Angle
    [SerializeField]
    Transform force_sphere;
    [SerializeField]
    Transform force_area;

    CharacterAudio audio;

    private const float force_min = 1f;                         //Min Force that will be applied.
    private float force = force_min;                            //The FORCE that will be applied.
    private const float force_max = 400f;                       //Max Force it can be applied.

    private float multiplier = 8f;                           //The needed increment per frame to make it last 1.2 seconds.

    private bool has_punched = false;                           //Knows wheter the player has punched or not.
    private float timer = 0;                                    //Counter of the current time.
    private const float time_reset_punch = 0.1f;                //Time it will last the boolean HAS_PUNCHED.

    public void ChargePush()
    {
        force += multiplier;
        CheckForce();
    }

    public void Push()
    {
        has_punched = true;
    }

    public void LookPunchCD()
    {
        if (has_punched)
        {
            timer += Time.deltaTime;
            if (timer >= time_reset_punch)
            {
                ResetPunchStats();
            }
        }
    }

    public void UpdateForceSphere()
    {
        float percent = force / force_max;
        force_sphere.localScale = new Vector3(percent, percent, percent);
    }

    public void UpdateForceArea()
    {
        float percent = force / force_max;
        force_area.localScale = new Vector3(percent, percent, percent);
    }

    private void CheckForce()
    {
        if (force > force_max)
        {
            force = force_max;
            multiplier *= -1;
        }

        if (force < force_min)
        {
            force = force_min;
            multiplier *= -1;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (has_punched)
        {
            Vector3 positionThrowed = other.transform.position + (transform.forward * force * Time.deltaTime);
            bool already_punched = false;

            if (other.GetComponent<Rigidbody>())
            {
                already_punched = !already_punched;
                other.GetComponent<Rigidbody>().AddForce(ParabollicVel(other.transform.position, positionThrowed, shootAngle), ForceMode.Impulse);
            }
              
            if (!already_punched)
            {
                if (other.GetComponentInParent<Rigidbody>())
                {
                    other.GetComponentInParent<Rigidbody>().AddForce(ParabollicVel(other.transform.position, positionThrowed, shootAngle), ForceMode.Impulse);
                }
                
            }
            
                
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
