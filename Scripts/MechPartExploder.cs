using UnityEngine;

public class MechPartExploder : MonoBehaviour
{
    [Header("Assign the FBX root object here")]
    public GameObject robotRoot;

    [Header("Explosion settings")]
    public float explodeRadius = 1.5f;
    public bool addLabels = true;

    void Start()
    {
        if (robotRoot == null)
        {
            Debug.LogError("🚫 Please assign the robotRoot GameObject in the inspector.");
            return;
        }

        // Get all child transforms inside the robot
        Transform[] allParts = robotRoot.GetComponentsInChildren<Transform>();

        foreach (Transform part in allParts)
        {
            // Skip the root object itself
            if (part == robotRoot.transform)
                continue;

            // Calculate explosion direction
            Vector3 direction = (part.position - robotRoot.transform.position).normalized;
            if (direction == Vector3.zero)
                direction = Random.onUnitSphere;

            // Move the part outward
            part.position += direction * explodeRadius;

            // Add label if enabled
            if (addLabels)
            {
                GameObject label = CreateLabel(part.name);
                label.transform.SetParent(part);
                label.transform.localPosition = Vector3.up * 0.3f;
            }
        }

        Debug.Log("✅ Explosion complete!");
    }

    // Creates a floating name label
    GameObject CreateLabel(string text)
    {
        GameObject labelObj = new GameObject("Label_" + text);
        TextMesh textMesh = labelObj.AddComponent<TextMesh>();
        textMesh.text = text;
        textMesh.characterSize = 0.025f;
        textMesh.fontSize = 40;
        textMesh.color = Color.yellow;
        textMesh.anchor = TextAnchor.MiddleCenter;
        textMesh.alignment = TextAlignment.Center;

        

        return labelObj;
    }
}
