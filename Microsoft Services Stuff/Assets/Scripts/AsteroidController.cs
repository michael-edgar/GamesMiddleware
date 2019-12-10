using Tobii.Gaming;
using UnityEngine;
using Random = UnityEngine.Random;

public class AsteroidController : MonoBehaviour
{
    [SerializeField] private GazeAware gazeAware;
    private Collider _collider;
    private Material _material;
    [SerializeField] private float movementSpeed = 0.125f;

    private void Awake()
    {
        gazeAware = GetComponent<GazeAware>();
        _collider = GetComponent<Collider>();
        _material = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (gazeAware.HasGazeFocus)
        {
            print(this.name + " is being focused");
            // Code from: https://docs.unity3d.com/ScriptReference/Renderer-material.html
            _material.color = Color.red;
        }
        else if (_material.color == Color.red && !gazeAware.HasGazeFocus)
        {
            _material.color = Color.white;
        }
        
        transform.position -= Vector3.forward * movementSpeed;
    }

    public void SetAsteroidPosition(float currentZ)
    {
        float z = Random.Range(currentZ, currentZ + 25);    
        float x = Random.Range(-10f, 10f);
        float y = Random.Range(-10f, 10f);
        transform.position = new Vector3(x,y,z);
    }

    public bool ShouldBeDestroyed(bool shouldBeDestroying)
    {
        return (!IsVisible() || (_material.color == Color.red && shouldBeDestroying));
    }
    
    

    //Code from: https://docs.unity3d.com/ScriptReference/GeometryUtility.TestPlanesAABB.html
    public bool IsVisible()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        return GeometryUtility.TestPlanesAABB(planes, _collider.bounds);
    }
}
