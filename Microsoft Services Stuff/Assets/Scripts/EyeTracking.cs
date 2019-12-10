using UnityEngine;
using Tobii.Gaming;

public class EyeTracking : MonoBehaviour
{
    private GazePoint _gazePoint;
    void Update()
    {
        _gazePoint = TobiiAPI.GetGazePoint();

        if (_gazePoint.IsValid)
        {
            Vector2 sampleInput = _gazePoint.Screen;
            transform.position = sampleInput;
        }
    }
}
