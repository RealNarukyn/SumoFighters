using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaScript : MonoBehaviour
{
     private Vector3 size_min = new Vector3(8, 1.8f, 8);
     private Vector3 size_normal = new Vector3(14, 1.8f, 14);
     private float speed_decrease = 0.4f;

    public void UpdateSize()
    {
        Vector3 new_size = transform.localScale - new Vector3(1, 1, 1) * speed_decrease * Time.deltaTime;

        if (transform.localScale.magnitude > size_min.magnitude)
        {
            transform.localScale = new Vector3(new_size.x, 1.8f, new_size.z);
        }
    }

    public void RestartSize() { transform.localScale = size_normal; }
}
