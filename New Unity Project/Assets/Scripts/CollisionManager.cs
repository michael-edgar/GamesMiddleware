using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    private static Plane[] _planes;
    private static BallBounce[] _ballBounces;
    private const float CollisionDistance = 0.0f;

    public static Plane[] getPlanes()
    {
        return _planes;
        
    }
    
    public static BallBounce[] getBallBounces()
    {
        return _ballBounces;
        
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _planes = FindObjectsOfType<Plane>();
        _ballBounces = FindObjectsOfType<BallBounce>();
    }

    // Update is called once per frame
    void Update()
    {
        _planes = FindObjectsOfType<Plane>();
        _ballBounces = FindObjectsOfType<BallBounce>();
        
        for (int i = 0; i < _ballBounces.Length; i++)
        {
            CollisionCheckPlane(i);
            CollisionCheckSphere(i);
        }
    }

    private static void CollisionCheckPlane(int currentIndex)
    {
        BallBounce currentBounce = _ballBounces[currentIndex];
        for (int i = 0; i < _planes.Length; i++)
        {
            Plane currentPlane = _planes[i];
            float distance = currentPlane.DistanceTo(currentBounce.transform.position) - currentBounce.GetRadius();
            if (distance < CollisionDistance)
            {
                ResolveCollisionPlane(currentBounce, currentPlane);
            }
        }
    }

    private static void CollisionCheckSphere(int currentIndex)
    {
        BallBounce bounce1 = _ballBounces[currentIndex];
        float radius1 = bounce1.GetRadius();
        Vector3 center1 = bounce1.transform.position;

        for (int i = currentIndex + 1; i < _ballBounces.Length; i++)
        {
            BallBounce bounce2 = _ballBounces[i];
            float radius2 = bounce2.GetRadius();
            Vector3 center2 = bounce2.transform.position;
            float distance = (center2 - center1).magnitude - (radius1 + radius2);
            
            if (distance < CollisionDistance)
            {
                ResolveCollisionSphere(bounce1, bounce2);
            }
        }
    }

    private static float GetTimeOfImpactSphere(BallBounce bounce1, BallBounce bounce2)
    {
        Vector3 center1 = bounce1.transform.position;
        Vector3 center2 = bounce2.transform.position;
        float distanceBefore = ((center2 - (bounce2.GetVelocity())) - (center1 - (bounce1.GetVelocity()))).magnitude;
        float distanceAfter = ((center2) - (center1)).magnitude;
        return -(distanceBefore * Time.deltaTime)/(distanceAfter - distanceBefore);
    }

    private static float GetTimeOfImpactPlane(BallBounce bounce, Plane plane)
    {
        Vector3 center = bounce.transform.position;
        float distanceBefore = plane.DistanceTo(center - bounce.GetVelocity()) - bounce.GetRadius();
        float distanceAfter = plane.DistanceTo(center) - bounce.GetRadius();
        float t = -(distanceBefore * Time.deltaTime)/(distanceAfter - distanceBefore);
        //Debug.Log("Distance Before: " +distanceBefore + "\nDistance After: " + distanceAfter + "\nTime: " +t+ "\nTime Delta: " + Time.deltaTime);
        return t;
    }

    private static float SetTimeOfImpactPositionSphere(BallBounce bounce1, BallBounce bounce2)
    {
        float t = Time.deltaTime - GetTimeOfImpactSphere(bounce1, bounce2);
        bounce1.transform.position -= (bounce1.GetVelocity() * t);
        bounce2.transform.position -= (bounce2.GetVelocity() * t);
        return t;
    }

    private static float SetTimeOfImpactPositionPlane(BallBounce bounce, Plane plane)
    {
        float t = Time.deltaTime - GetTimeOfImpactPlane(bounce, plane);
        bounce.transform.position -= (bounce.GetVelocity() * t);
        return t;
    }

    private static void ResolveCollisionSphere(BallBounce bounce1, BallBounce bounce2)
    {
        Vector3 velocity1 = bounce1.GetVelocity();
        Vector3 velocity2 = bounce2.GetVelocity();
        float mass1 = bounce1.GetMass();
        float mass2 = bounce2.GetMass();
        float timeOfImpact = SetTimeOfImpactPositionSphere(bounce1, bounce2);
        Vector3 center1 = bounce1.transform.position;
        Vector3 center2 = bounce2.transform.position;
        Vector3 normal = ((center2 - center1).normalized);
        Vector3 parallel1 = Parallel(velocity1, normal);
        Vector3 parallel2 = Parallel(velocity2, normal);
        Vector3 perpendicular1 = Perpendicular(velocity1, normal);
        Vector3 perpendicular2 = Perpendicular(velocity2, normal);
        Vector3 v1 = ((mass1 - mass2) / (mass1 + mass2))*parallel1 + ((2*mass2)/ (mass1 + mass2))*parallel2;
        Vector3 v2 = ((mass2 - mass1) / (mass1 + mass2))*parallel2 + ((2*mass1)/ (mass1 + mass2))*parallel1;
        bounce1.SetVelocity((v1*bounce1.GetCoefficientOfRestitution()) + perpendicular1.normalized);
        bounce2.SetVelocity((v2*bounce2.GetCoefficientOfRestitution()) + perpendicular2.normalized);
        bounce1.transform.position = center1 + (bounce1.GetVelocity() * timeOfImpact);
        bounce2.transform.position = center2 + (bounce2.GetVelocity() * timeOfImpact);
    }

    private static void ResolveCollisionPlane(BallBounce bounce, Plane plane)
    {
        Vector3 velocity = bounce.GetVelocity();
        Vector3 perpendicular = plane.PerpToSurface(velocity);
        Vector3 parallel = plane.ParallelToSurface(velocity);
        float timeOfImpact = SetTimeOfImpactPositionPlane(bounce, plane);
        Vector3 center = bounce.transform.position;
        float distance = plane.DistanceTo(center) - bounce.GetRadius();
        bounce.SetVelocity((parallel * bounce.GetCoefficientOfRestitution()) - perpendicular);
        bounce.transform.position = (center + (bounce.GetVelocity() * timeOfImpact)) - (-distance * perpendicular);
    }
    
    public static Vector3 Parallel(Vector3 v, Vector3 n)
    {
        //(test N is a normal/ normalise N)
        Vector3 normed = n.normalized;
        return (Vector3.Dot(v, normed) * normed);
    }

    public static Vector3 Perpendicular(Vector3 v, Vector3 n)
    {
        return v - Parallel(v, n);
    }
    
    public static void SetCurrentSelectedBallBounce()
    {
        int bounce = GetCurrentSelectedBallBounceIndex();
            if (bounce == -1)
            {
                _ballBounces[0].SetIsSelected(true);
            }
            else if (bounce == _ballBounces.Length -1)
            {
                _ballBounces[bounce].SetIsSelected(false);
            }
            else
            {
                _ballBounces[bounce].SetIsSelected(false);
                _ballBounces[bounce+1].SetIsSelected(true);
            }
    }

    public static int GetCurrentSelectedBallBounceIndex()
    {
        if (_ballBounces.Length != 0)
        {
            for (int i = 0; i < _ballBounces.Length; i++)
            {
                if (_ballBounces[i].IsSelected())
                {
                    return i;
                }
            }
        }

        return -1;
    }

    public static BallBounce GetCurrentSelectedBallBounce()
    {
        int index = GetCurrentSelectedBallBounceIndex();
        
        if (index == -1)
            return null;
        
        return _ballBounces[index];
    }
}
