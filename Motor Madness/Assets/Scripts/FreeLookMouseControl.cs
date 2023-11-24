using UnityEngine;
using Cinemachine;

public class FreeLookMouseControl : MonoBehaviour
{
    public float sensitivity = 2f;
    public float resetDelay = 2f; // Time in seconds to wait before resetting position

    public GameObject car; // Reference to your car GameObject

    private CinemachineVirtualCamera virtualCamera;
    private Transform originalFollowTarget;
    private bool isMouseMoving;
    private float lastMouseMoveTime;
    private GameManager gameManager;


    private void Start()
    {
        gameManager = GameManager.Instance;



        
    }

    private void Update()
    {
        if (gameManager.inrace)
        {
            if (virtualCamera == null)
            {
                virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
                originalFollowTarget = virtualCamera.Follow;

            }
            if (virtualCamera != null)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            HandleMouseInput();
            ResetCameraPositionIfNeeded();
        }

       
    }

    private void HandleMouseInput()
    {
        float mouseX = Input.GetAxis("Mouse X");

        // Adjust the sensitivity based on your preferences
        float rotationDelta = mouseX * sensitivity;

        // Get the virtual camera's follow target and rotate it around the Y-axis
        Transform followTarget = virtualCamera.Follow;

        if (followTarget != null)
        {
            followTarget.Rotate(Vector3.up, rotationDelta);

            // Check if the mouse is currently moving
            isMouseMoving = Mathf.Abs(mouseX) > 0.01f;
            if (isMouseMoving)
            {
                lastMouseMoveTime = Time.time;
            }
        }
        else
        {
            Debug.LogError("Follow target not found in the VirtualCamera!");
        }
    }

    private void ResetCameraPositionIfNeeded()
    {
        if (!isMouseMoving && Time.time - lastMouseMoveTime > resetDelay)
        {
            // Reset camera position
            virtualCamera.Follow = originalFollowTarget;
        }
    }
}

