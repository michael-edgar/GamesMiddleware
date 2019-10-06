using System;
using UnityEngine;

public class Plane : MonoBehaviour
{
    public Vector3 point;
    public Vector3 normal;

    private void Start()
    {
        PlacePlane();
    }

    private void Update()
    {
        PlacePlane();
    }

    public float DistanceTo(Vector3 s)
    {
        return CollisionManager.Parallel((point - s), normal).magnitude;
    }

    private void PlacePlane()
    {
        transform.position = point;
        transform.rotation = Quaternion.LookRotation(transform.forward, normal);
    }

    public Vector3 PerpToSurface(Vector3 velocity)
    {
        return CollisionManager.Parallel(velocity, normal);
    }

    public Vector3 ParallelToSurface(Vector3 velocity)
    {
        return CollisionManager.Perpendicular(velocity, normal);
    }
}