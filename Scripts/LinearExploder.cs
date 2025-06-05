using UnityEngine;

public class LinearExploder : MonoBehaviour
{
    [Header("Assign the root object of the model")]
    public GameObject modelRoot;

    [Header("Linear Explosion Settings")]
    public float partSpacing = 0.00005f;
    public Vector3 explosionDirection = Vector3.right;

    void Start()
    {
        if (modelRoot == null)
        {
            Debug.LogError("🚫 Assign a model root GameObject to explode.");
            return;
        }

        Transform[] allParts = modelRoot.GetComponentsInChildren<Transform>();
        int index = 0;

        foreach (Transform part in allParts)
        {
            if (part == modelRoot.transform)
                continue;

            Vector3 offset = explosionDirection.normalized * partSpacing * index;
            part.position = modelRoot.transform.position + offset;
            index++;
        }

        Debug.Log("✅ Linear explosion complete.");
    }
}
