using UnityEngine;

public class Plane : MonoBehaviour
{
    private Vector3 _point;
    private Vector3 _normal;

    private void Start()
    {
        _point = Vector3.one;
        _normal = new Vector3(1,4,0);
        PlacePlane();
    }

    public float DistanceTo(Vector3 s)
    {
        return CollisionManager.Parallel((_point - s), _normal).magnitude;
    }

    private void PlacePlane()
    {
        transform.position = _point;
        transform.rotation = Quaternion.LookRotation(transform.forward, _normal);
    }

    private void PlacePlane(Vector3 newPoint, Vector3 newNormal)
    {
        transform.position = newPoint;
        transform.rotation = Quaternion.LookRotation(newNormal);
    }

    public Vector3 PerpToSurface(Vector3 velocity)
    {
        return CollisionManager.Parallel(velocity, _normal);
    }

    public Vector3 ParallelToSurface(Vector3 velocity)
    {
        return CollisionManager.Perpendicular(velocity, _normal);
    }
}