using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SnapToPlace : MonoBehaviour
{
    public Transform snapPoint;
    public float snapDistance = 0.1f;
    private bool isSnapped = false;

    private XRGrabInteractable grab;
    private Rigidbody rb;

    void Start()
    {
        grab = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (isSnapped || grab.isSelected) return;

        float dist = Vector3.Distance(transform.position, snapPoint.position);

        if (dist < snapDistance)
        {
            transform.position = snapPoint.position;
            transform.rotation = snapPoint.rotation;

            rb.isKinematic = true;
            grab.enabled = false;
            isSnapped = true;

            // Notify manager
            FindObjectOfType<AssemblyManager>().PartSnapped();
        }
    }
}
