using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RobotRotationController : MonoBehaviour
{
    public enum RotationAxis { X, Y, Z }

    [System.Serializable]
    public class PartPair
    {
        public Transform leader; // The main part (rotated directly)
        public Transform follower; // Should follow the leader like a true child
        public RotationAxis axis; // Axis around which the leader rotates

        [Tooltip("Rotation amount in degrees. Positive for clockwise, negative for counter-clockwise.")]
        public float rotationAmount = 30f; // User-defined angle direction

        [HideInInspector] public Vector3 localOffset;
        [HideInInspector] public Quaternion rotationOffset;
    }

    [Header("Part Relationship Settings")]
    public List<PartPair> parts = new List<PartPair>();

    [Tooltip("Rotation speed in degrees per second.")]
    public float rotationSpeed = 50f;

    [Header("UI Settings")]
    public Button rotateButton;

    private bool isRotating = false;

    void Start()
    {
        // Calculate the local offset from leader to follower (in leader's local space)
        foreach (var pair in parts)
        {
            if (pair.leader != null && pair.follower != null)
            {
                pair.localOffset = pair.leader.InverseTransformPoint(pair.follower.position);
                pair.rotationOffset = Quaternion.Inverse(pair.leader.rotation) * pair.follower.rotation;
            }
        }

        if (rotateButton != null)
        {
            rotateButton.onClick.AddListener(ToggleRotation);
        }
        else
        {
            Debug.LogError("Rotate Button not assigned!");
        }
    }

    void Update()
    {
        if (!isRotating) return;

        foreach (var pair in parts)
        {
            if (pair.leader == null || pair.follower == null) continue;

            // Get axis vector
            Vector3 axisVector = GetAxisVector(pair.axis);

            // Get direction from rotationAmount sign (+ or -)
            float direction = Mathf.Sign(pair.rotationAmount);

            // Apply rotation step based on direction and speed
            float step = direction * rotationSpeed * Time.deltaTime;
            pair.leader.Rotate(axisVector * step, Space.Self);

            // Simulate child-like behavior for follower
            Vector3 worldOffset = pair.leader.TransformPoint(pair.localOffset);
            pair.follower.position = worldOffset;
            pair.follower.rotation = pair.leader.rotation * pair.rotationOffset;
        }
    }

    public void ToggleRotation()
    {
        isRotating = !isRotating;
    }

    private Vector3 GetAxisVector(RotationAxis axis)
    {
        switch (axis)
        {
            case RotationAxis.X: return Vector3.right;
            case RotationAxis.Y: return Vector3.up;
            case RotationAxis.Z: return Vector3.forward;
            default: return Vector3.up;
        }
    }
}
