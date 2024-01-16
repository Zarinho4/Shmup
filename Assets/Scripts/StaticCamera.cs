using UnityEngine;

public class StaticCamera : MonoBehaviour
{
    private Vector3 initialPosition;

    void Start()
    {
        // Store the initial position of the camera
        initialPosition = transform.position;
    }

    void Update()
    {
        // Ensure the camera stays in its initial position
        transform.position = initialPosition;
    }
}
