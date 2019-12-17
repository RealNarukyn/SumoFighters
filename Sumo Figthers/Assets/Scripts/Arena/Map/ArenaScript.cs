using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaScript : MonoBehaviour
{
    [SerializeField]
    private Vector3 size_min;
    [SerializeField]
    private Vector3 size_normal;
    [SerializeField]
    private float speed_decrease;


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
