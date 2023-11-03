using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameras : MonoBehaviour
{
    public GameObject hoodViewCamera;
    public GameObject normalViewCamera;
    public GameObject farViewCamera;

    private GameObject activeCamera;

    void Start()
    {
        // Initially, set the normal view camera as active.
        ActivateCamera(normalViewCamera);
    }

    void Update()
    {
        // Check for the 'V' key press to toggle cameras (objects).
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (activeCamera == hoodViewCamera)
            {
                ActivateCamera(normalViewCamera);
            }
            else if (activeCamera == normalViewCamera)
            {
                ActivateCamera(farViewCamera);
            }
            else if (activeCamera == farViewCamera)
            {
                ActivateCamera(hoodViewCamera);
            }
        }
    }

    private void ActivateCamera(GameObject cameraToActivate)
    {
        // Deactivate the currently active camera (object).
        if (activeCamera != null)
        {
            activeCamera.SetActive(false);
        }

        // Activate the new camera (object).
        cameraToActivate.SetActive(true);
        activeCamera = cameraToActivate;
    }
}
