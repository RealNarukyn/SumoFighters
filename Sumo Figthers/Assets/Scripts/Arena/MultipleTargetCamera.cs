using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MultipleTargetCamera : MonoBehaviour
{
    [SerializeField]
    private List<Transform> targets;

    public Vector3 offset = new Vector3(0f, 16f, -30f);
    private float smooth_time = .5f;

    private Vector3 velocity;

    private float zoom_min = 50f;
    private float zoom_max = 25f;
    private float zoom_limiter = 40f;
    
    private Camera cam;

    // Start is called before the first frame update
    void Awake()
    {
        cam = GetComponent<Camera>();
        targets = new List<Transform>();
    }

    private void LateUpdate()
    {
        if (targets.Count == 0)
            return;

        Move();
        Zoom();

        CheckTargets();
    }

    Vector3 GetCenterPoint()
    {
        if (targets.Count == 1)
            return targets[0].position;

        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }

        return bounds.center;
    }

    float GetGreatestDistance()
    {
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }

        return bounds.size.x;
    }

    private void Move()
    {
        Vector3 center_point = GetCenterPoint();

        Vector3 new_position = center_point + offset;

        transform.position = Vector3.SmoothDamp(transform.position, new_position, ref velocity, smooth_time);
    }

    private void Zoom()
    {
        float zoom_new = Mathf.Lerp(zoom_max, zoom_min, GetGreatestDistance() / zoom_limiter);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, zoom_new, Time.deltaTime);
    }

    private void CheckTargets() 
    {
        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i].position.y < -1)
            {
                targets.RemoveAt(i);
            }
        }
    }

    public void AddTargetToCamera(Transform target) { targets.Add(target); }
    public void ClearCamera() { targets.Clear(); }
}
