using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem; // New Input System

public class LungsExplode : MonoBehaviour
{
    public Transform Left, Middle, Right, Extra;

    [Header("Exploded Offsets")]
    public Vector3 LeftOffset = new Vector3(-0.07f, 0f, 0f);
    public Vector3 MiddleOffset = new Vector3(0f, -0.016f, 0f);
    public Vector3 RightOffset = new Vector3(0.07f, 0f, 0f);
    public Vector3 ExtraOffset = new Vector3(-0.01f, 0.14f, 0f);

    private Vector3[] originalPositions;
    private Vector3[] explodedPositions;
    private Transform[] interactableObjects;

    private bool isExploded = false;
    public float moveSpeed = 2.0f;

    private Transform selectedObject = null;
    private Vector3 offset;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;

        interactableObjects = new Transform[] {
            Left, Middle, Right, Extra
        };

        originalPositions = new Vector3[interactableObjects.Length];
        explodedPositions = new Vector3[interactableObjects.Length];

        for (int i = 0; i < interactableObjects.Length; i++)
        {
            originalPositions[i] = interactableObjects[i].localPosition;
            explodedPositions[i] = originalPositions[i] + GetExplodedOffset(interactableObjects[i]);
        }
    }

    Vector3 GetExplodedOffset(Transform obj)
    {
        if (obj == Right) return RightOffset;
        if (obj == Middle) return MiddleOffset;
        if (obj == Left) return LeftOffset;
        if (obj == Extra) return ExtraOffset;
        return Vector3.zero;
    }

    public void ToggleExplode()
    {
        StopAllCoroutines();

        for (int i = 0; i < interactableObjects.Length; i++)
        {
            Vector3 targetPos = isExploded ? originalPositions[i] : explodedPositions[i];
            StartCoroutine(MoveToPosition(interactableObjects[i], targetPos));
        }

        isExploded = !isExploded;
    }

    IEnumerator MoveToPosition(Transform obj, Vector3 targetPosition)
    {
        float timeElapsed = 0;
        Vector3 startPosition = obj.localPosition;

        while (timeElapsed < 1)
        {
            obj.localPosition = Vector3.Lerp(startPosition, targetPosition, timeElapsed);
            timeElapsed += Time.deltaTime * moveSpeed;
            yield return null;
        }

        obj.localPosition = targetPosition;
    }

    void Update()
    {
        HandleTouchInput();
    }

    void HandleTouchInput()
    {
        if (Touchscreen.current == null || Touchscreen.current.primaryTouch == null) return;

        var touch = Touchscreen.current.primaryTouch;

        if (touch.press.wasPressedThisFrame && selectedObject == null)
        {
            Ray ray = mainCamera.ScreenPointToRay(touch.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (IsInteractable(hit.transform))
                {
                    selectedObject = hit.transform;
                    offset = selectedObject.position - GetTouchWorldPosition();
                    Debug.Log($"Touched on: {selectedObject.name}");
                }
            }
        }

        if (touch.press.isPressed && selectedObject != null)
        {
            Vector3 touchPos = GetTouchWorldPosition() + offset;
            selectedObject.position = touchPos;
        }

        if (touch.press.wasReleasedThisFrame && selectedObject != null)
        {
            int index = System.Array.IndexOf(interactableObjects, selectedObject);
            Vector3 targetPos = isExploded ? explodedPositions[index] : originalPositions[index];
            StartCoroutine(MoveToPosition(selectedObject, targetPos));

            selectedObject = null;
        }
    }

    Vector3 GetTouchWorldPosition()
    {
        Vector3 touchPoint = Touchscreen.current.primaryTouch.position.ReadValue();
        touchPoint.z = 10f; // Adjust based on object's distance from the camera
        return mainCamera.ScreenToWorldPoint(touchPoint);
    }

    bool IsInteractable(Transform obj)
    {
        return System.Array.IndexOf(interactableObjects, obj) >= 0;
    }
}
