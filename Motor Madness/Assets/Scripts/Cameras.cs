using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameras : MonoBehaviour
{
    public GameObject hoodViewCamera;
    public GameObject normalViewCamera;
    public GameObject farViewCamera;
    public GameObject rearViewCamera;

    private GameObject activeCamera;

    public PlayerCarInput carInput;

    void Start()
    {
       if(normalViewCamera !=null){
            // Initially, set the normal view camera as active.
            ActivateCamera(normalViewCamera);
        }
        carInput = GetComponent<PlayerCarInput>();
    }

    void Update()
    {
        
        
        if(carInput != null)
        {
            if (carInput.cameraswitch)
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
            carInput.cameraswitch = false;

            if (Input.GetKey(KeyCode.B))
            {
                rearViewCamera.SetActive(true);
            }
            else
            {
                rearViewCamera.SetActive(false);
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
