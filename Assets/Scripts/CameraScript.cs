using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraScript : MonoBehaviour
{

    public GameObject[] targets;
    public Vector3 offset;
    public float smoothTime = 0.5f;
    public float zoomSpeed = 1f;


    public float minZoom = 40f;
    public float maxZoom = 10f;
    public float zoomLimiter = 50f;

    private Vector3 velocity;
    private Camera cam;
    private Camera importantCam;

    private void Start()
    {
        cam = GetComponent<Camera>();
        importantCam = transform.Find("Important_Camera").GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        targets = GameObject.FindGameObjectsWithTag("Player");

        if (targets.Length == 0)
        {
            return;
        }

        Move();
        Zoom();
        if (importantCam) SyncImportantCamera();
    }

    void Move()
    {
        Vector3 centerPoint = GetCenterPoint();

        Vector3 newPosition = centerPoint + offset;

        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }

    void Zoom()
    {
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / zoomLimiter);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime * zoomSpeed);
    }

    float GetGreatestDistance()
    {
        var bounds = new Bounds(targets[0].transform.position, Vector3.zero);
        for (int i = 0; i < targets.Length; i++)
        {
            bounds.Encapsulate(targets[i].transform.position);
        }

        return Mathf.Sqrt(Mathf.Pow(bounds.size.x, 2) + Mathf.Pow(bounds.size.y, 2));
    }

    Vector3 GetCenterPoint()
    {
        if (targets.Length == 1)
        {
            return targets[0].transform.position;
        }

        var bounds = new Bounds(targets[0].transform.position, Vector3.zero);
        for (int i = 0; i < targets.Length; i++)
        {
            bounds.Encapsulate(targets[i].transform.position);
        }

        return bounds.center;

    }

    void SyncImportantCamera()
    {
        importantCam.fieldOfView = cam.fieldOfView;
    }
}
