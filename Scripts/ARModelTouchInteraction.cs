using UnityEngine;

public class ARModelTouchInteraction : MonoBehaviour
{
    private Vector2 prevTouchPos1;
    private Vector2 prevTouchPos2;
    private Vector3 initialModelPosition;

    public float moveSpeed = 0.001f;
    public float zoomSpeed = 0.01f;
    public float rotationSpeed = 0.2f;

    void Update()
    {
        int touchCount = Input.touchCount;

        if (touchCount == 1)
        {
            // MOVE with 1 finger
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                Vector2 delta = touch.deltaPosition;
                transform.Translate(new Vector3(delta.x * moveSpeed, delta.y * moveSpeed, 0), Space.Self);
            }
        }
        else if (touchCount == 2)
        {
            // ZOOM with 2 fingers (pinch)
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
            {
                Vector2 prevPos1 = touch1.position - touch1.deltaPosition;
                Vector2 prevPos2 = touch2.position - touch2.deltaPosition;

                float prevDist = (prevPos1 - prevPos2).magnitude;
                float currDist = (touch1.position - touch2.position).magnitude;
                float diff = currDist - prevDist;

                Vector3 newScale = transform.localScale + Vector3.one * diff * zoomSpeed;
                newScale = ClampScale(newScale, 0.1f, 3f);
                transform.localScale = newScale;
            }
        }
        else if (touchCount == 3)
        {
            // ROTATE with 3 fingers
            Touch touch = Input.GetTouch(0); // Just use one of the three fingers to get delta
            if (touch.phase == TouchPhase.Moved)
            {
                float rotationX = touch.deltaPosition.x * rotationSpeed;
                float rotationY = touch.deltaPosition.y * rotationSpeed;

                transform.Rotate(Vector3.up, -rotationX, Space.World); // Horizontal
                transform.Rotate(Vector3.right, rotationY, Space.World); // Vertical
            }
        }
    }

    private Vector3 ClampScale(Vector3 scale, float min, float max)
    {
        return new Vector3(
            Mathf.Clamp(scale.x, min, max),
            Mathf.Clamp(scale.y, min, max),
            Mathf.Clamp(scale.z, min, max)
        );
    }
}
