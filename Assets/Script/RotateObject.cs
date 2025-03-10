using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public float rotationSpeed = 50f; // Speed of rotation in degrees per second

    void Update()
    {
        // Rotate around the Z axis
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}
