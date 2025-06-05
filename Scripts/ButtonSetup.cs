using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required for UI elements

public class ButtonSetup : MonoBehaviour
{
    public GameObject modelToRotate; // Assign your GLB model in the inspector
    private RotationController rotationController;
    public Button rotateButton; // Assign your button in the inspector
    
    void Start()
    {
        // Get the rotation controller from the model
        if (modelToRotate != null)
        {
            rotationController = modelToRotate.GetComponent<RotationController>();
            
            // If no rotation controller exists, add one
            if (rotationController == null)
            {
                rotationController = modelToRotate.AddComponent<RotationController>();
            }
        }
        
        // Setup button listener
        if (rotateButton != null && rotationController != null)
        {
            rotateButton.onClick.AddListener(rotationController.ToggleRotation);
        }
        else
        {
            Debug.LogError("Button or model reference is missing!");
        }
    }
}