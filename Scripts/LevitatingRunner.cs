using UnityEngine;
using System.Collections.Generic;

public class LevitatingRunner : MonoBehaviour
{
    [Header("Assign your robot's root GameObject here")]
    public GameObject robotRoot;

    [Header("Animation Settings")]
    public float levitationHeight = 0.2f;
    public float levitationSpeed = 2f;
    public float limbSwingSpeed = 5f;
    public float limbSwingAmount = 25f;

    private List<Transform> mechParts = new List<Transform>();
    private Dictionary<Transform, Vector3> originalRotations = new Dictionary<Transform, Vector3>();
    private Vector3 originalPosition;

    void Start()
    {
        if (robotRoot == null)
        {
            Debug.LogError("❌ Please assign the robotRoot GameObject in the Inspector.");
            return;
        }

        originalPosition = robotRoot.transform.position;

        foreach (Transform child in robotRoot.GetComponentsInChildren<Transform>())
        {
            if (child != robotRoot.transform)
            {
                mechParts.Add(child);
                originalRotations[child] = child.localEulerAngles;
            }
        }
    }

    void Update()
    {
        if (robotRoot == null) return;

        // Levitate up and down
        float levitateOffset = Mathf.Sin(Time.time * levitationSpeed) * levitationHeight;
        robotRoot.transform.position = originalPosition + new Vector3(0, levitateOffset, 0);

        // Simulate running by rotating parts
        for (int i = 0; i < mechParts.Count; i++)
        {
            Transform part = mechParts[i];
            float offset = (i % 4) * 0.3f; // Create variation between parts
            Vector3 newRot = originalRotations[part];
            newRot.x += Mathf.Sin(Time.time * limbSwingSpeed + offset) * limbSwingAmount;
            newRot.y += Mathf.Cos(Time.time * limbSwingSpeed + offset) * (limbSwingAmount / 2f);
            part.localEulerAngles = newRot;
        }
    }
}
