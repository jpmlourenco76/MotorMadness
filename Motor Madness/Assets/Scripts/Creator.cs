using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Creator : MonoBehaviour
{
    public GameObject particleSystemPrefab;
    public GameObject minimapCirclePrefab;

    private GameObject particleSystemInstance;
    private GameObject minimapCircleInstance;

    private bool hasInstantiatedPrefab = false;
    private bool hasInstantiatedPPrefab = false;


    private void Update()
    {
        // Check if the scene "stage3.2" is loaded
        if (SceneManager.GetSceneByName("stage3.2").isLoaded)
        {
            // If the prefab hasn't been instantiated, instantiate it
            if (!hasInstantiatedPPrefab)
            {
                InstantiatePPrefab();
                hasInstantiatedPPrefab = true;
            }

            // Update the position of the instantiated prefab
            UpdatePPrefabPosition();
        }

        if (!hasInstantiatedPrefab)
        {
            InstantiatePrefab();
            hasInstantiatedPrefab = true;
        }
        UpdatePrefabPosition();
    }

    private void InstantiatePPrefab()
    {
        // Check if the particleSystemPrefab is assigned in the Unity Editor
        if (particleSystemPrefab == null)
        {
            Debug.LogError("Particle System Prefab is not assigned!");
            return;
        }

        // Instantiate the particle system prefab
        particleSystemInstance = Instantiate(particleSystemPrefab);

        // Set the initial position based on the current GameObject's position
        particleSystemInstance.transform.position = new Vector3(
            transform.position.x - 2,
            transform.position.y + 30,
            transform.position.z - 6
        );

        // Set the initial rotation to (90, 0, 0)
        particleSystemInstance.transform.rotation = Quaternion.Euler(90, 0, 0);
    }

    private void InstantiatePrefab()
    {
        // Check if the particleSystemPrefab is assigned in the Unity Editor
        if (minimapCirclePrefab == null)
        {
            Debug.LogError("minimap Circle Prefab is not assigned!");
            return;
        }

        // Instantiate the particle system prefab
        minimapCircleInstance = Instantiate(minimapCirclePrefab);

        // Set the initial position based on the current GameObject's position
        minimapCircleInstance.transform.position = new Vector3(
            transform.position.x,
            transform.position.y + 150,
            transform.position.z
        );

        // Set the initial rotation to (90, 0, 0)
        minimapCircleInstance.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void UpdatePPrefabPosition()
    {
        // Update the position based on the current GameObject's position
        if (particleSystemInstance != null)
        {
            particleSystemInstance.transform.position = new Vector3(
                transform.position.x - 2 ,
                transform.position.y + 30,
                transform.position.z - 6
            );

        }
    }

    private void UpdatePrefabPosition()
    {
        // Update the position based on the current GameObject's position
        if (minimapCircleInstance != null)
        {
            minimapCircleInstance.transform.position = new Vector3(
                transform.position.x,
                transform.position.y + 150,
                transform.position.z
            );
            minimapCircleInstance.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
