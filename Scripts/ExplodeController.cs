using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem; // New Input System

public class ExplodeController : MonoBehaviour
{
    public Transform LeftHemisphere, Cortex, RightHemisphere, Allen_thalamus_L;

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
            LeftHemisphere, Cortex, RightHemisphere, Allen_thalamus_L
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
        if (obj == RightHemisphere) return new Vector3(0.04f, 0, 0);
        if (obj == Cortex) return new Vector3(0, -0.015f, 0);
        if (obj == LeftHemisphere) return new Vector3(-0.04f, 0, 0);
        if (obj == Allen_thalamus_L) return new Vector3(-0.01f,0.13f,0);
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