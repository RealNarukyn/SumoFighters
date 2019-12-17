using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaScript : MonoBehaviour
{
     private Vector3 size_min = new Vector3(10, 1, 10);
     private Vector3 size_normal = new Vector3(30, 1, 30);
     private float speed_decrease = 0.8f;

    public void UpdateSize()
    {
        Vector3 new_size = transform.localScale - new Vector3(1, 1, 1) * speed_decrease * Time.deltaTime;

        if (transform.localScale.magnitude > size_min.magnitude)
        {
            transform.localScale = new Vector3(new_size.x, 1, new_size.z);
        }
    }

    public void RestartSize() { transform.localScale = size_normal; }
}
