using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationController : MonoBehaviour 
{
    public Vector3 rotationVector = new Vector3(0, 30, 0); // Default rotation around Y axis
    public bool isRotating = false; // Flag to track rotation state
    
    private void Update()
    {
        // Only rotate if isRotating is true
        if (isRotating)
        {
            transform.Rotate(rotationVector * Time.deltaTime);
        }
    }
    
    // Method to toggle rotation on/off
    public void ToggleRotation()
    {
        isRotating = !isRotating;
    }
    
    // Method to start rotation
    public void StartRotation()
    {
        isRotating = true;
    }
    
    // Method to stop rotation
    public void StopRotation()
    {
        isRotating = false;
    }
}