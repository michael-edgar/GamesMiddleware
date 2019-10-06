using UnityEngine;

public class BallBounce : MonoBehaviour
{
    public Vector3 velocity;
    public Vector3 acceleration;
    public float coefficientOfRestitution = 1.0f;
    public float accelerationDueToGravity = 9.8f;
    public float mass = 1.0f;
    private float _radius;
    public bool isSelected;
    public bool hasCollided;
    public int collisionCoolDown = 25;

    private void Start()
    {
        isSelected = false;
        hasCollided = false;
        acceleration = Vector3.down * accelerationDueToGravity;
        _radius = transform.localScale.y / 2;
        SetColor(false);
    }

    private void Update()
    {
        if (hasCollided && collisionCoolDown > 0)
        {
            collisionCoolDown--;
        }
        else
        {
            collisionCoolDown = 5;
            hasCollided = false;
            velocity += acceleration * Time.deltaTime;
            transform.position += velocity * Time.deltaTime;
        }
    }

    public void SetIsSelected(bool selected)
    {
        isSelected = selected;
        SetColor(selected);
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
}
