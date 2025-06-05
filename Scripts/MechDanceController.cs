using UnityEngine;
using System.Collections.Generic;

public class MechDanceController : MonoBehaviour
{
    [Header("Assign the root of your FBX model here")]
    public GameObject robotRoot;

    [Header("Dance Settings")]
    public float danceSpeed = 2f;
    public float danceAmount = 15f;

    private bool isDancing = false;
    private Dictionary<Transform, Vector3> originalRotations = new Dictionary<Transform, Vector3>();
    private List<Transform> mechParts = new List<Transform>();

    void Start()
    {
        if (robotRoot == null)
        {
            Debug.LogError("❌ Please assign the robotRoot GameObject in the Inspector.");
            return;
        }

        // Get all child parts from the assigned root object
        foreach (Transform child in robotRoot.GetComponentsInChildren<Transform>())
        {
            // Ignore the root itself
            if (child != robotRoot.transform)
            {
                mechParts.Add(child);
                originalRotations[child] = child.localEulerAngles;
            }
        }
    }

    void Update()
    {
        // Toggle dance mode with 'D'
        if (Input.GetKeyDown(KeyCode.D))
        {
            isDancing = !isDancing;
        }

        if (isDancing)
        {
            float wave = Mathf.Sin(Time.time * danceSpeed) * danceAmount;

            for (int i = 0; i < mechParts.Count; i++)
            {
                Transform part = mechParts[i];
                float offset = (i % 3) * 0.5f;

                Vector3 newRot = originalRotations[part];
                newRot.x += Mathf.Sin(Time.time * danceSpeed + offset) * (danceAmount / 2f);
                newRot.y += Mathf.Cos(Time.time * danceSpeed + offset) * (danceAmount / 2f);

                part.localEulerAngles = newRot;
            }
        }
        else
        {
            // Reset all parts to original rotations
            foreach (var part in mechParts)
            {
                part.localEulerAngles = originalRotations[part];
            }
        }
    }
}
