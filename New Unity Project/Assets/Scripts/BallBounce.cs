using UnityEngine;

public class BallBounce : MonoBehaviour
{
    private Vector3 _velocity;
    private Vector3 _acceleration;
    private float _radius;
    public float coefficientOfRestitution = 1.0f;
    public float accelerationDueToGravity = 9.8f;
    public float mass = 1.0f;
    private float _realDistance;
    private Plane _plane;
    private bool isSelected = false;

    private void Start()
    {
        _plane = FindObjectOfType<Plane>();
        _acceleration = Vector3.down * (accelerationDueToGravity * mass);
        _radius = transform.localScale.y / 2;
        SetColor(false);
    }

    private void Update()
    {
        _velocity += _acceleration * Time.deltaTime;
        transform.position += _velocity * Time.deltaTime;
        /*_realDistance = _plane.DistanceTo(transform.position) - _radius;
        if (_realDistance < 0)
        {
            Vector3 perpendicular = _plane.PerpToSurface(_velocity);
            Vector3 parallel = _plane.ParallelToSurface(_velocity);
            transform.position -= -(_realDistance) * perpendicular;
            _velocity = (parallel - perpendicular) * _coefficientOfRestitution;
        }*/
    }

    public void SetIsSelected(bool selected)
    {
        isSelected = selected;
        SetColor(selected);
    }

    public bool IsSelected()
    {
        return isSelected;
    }

    public void SetColor(bool isSelected)
    {
        if (isSelected)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.red;
        }
        else
        {
            gameObject.GetComponent<Renderer>().material.color = Color.black;
        }
    }

    public float GetRadius()
    {
        return _radius;
        
    }

    public Vector3 GetVelocity()
    {
        return _velocity;
    }

    public float GetCoefficientOfRestitution()
    {
        return coefficientOfRestitution;
    }

    public float GetMass()
    {
        return mass;
    }

    public void SetVelocity(Vector3 velocity)
    {
        _velocity = velocity;
    }

    private void FirstMethodUpdate()
    {
        _velocity += _acceleration * Time.deltaTime;
        transform.position += _velocity * Time.deltaTime;
        if (_plane.DistanceTo(transform.position) < _radius)
        {
            transform.position -= _velocity * Time.deltaTime;
            Vector3 perpendicular = _plane.PerpToSurface(_velocity);
            Vector3 parallel = _plane.ParallelToSurface(_velocity);
            _velocity = (parallel - perpendicular);
        }
    }
}
