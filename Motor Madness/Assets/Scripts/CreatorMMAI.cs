using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreatorMMAI : MonoBehaviour
{
    public GameObject minimapCirclePrefab;

    private GameObject minimapCircleInstance;

    private bool hasInstantiatedPrefab = false;


    private void Update()
    {
        if (!hasInstantiatedPrefab)
        {
            InstantiatePrefab();
            hasInstantiatedPrefab = true;
        }
        UpdatePrefabPosition();
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
            transform.position.y + 140,
            transform.position.z
        );

        // Set the initial rotation to (90, 0, 0)
        minimapCircleInstance.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void UpdatePrefabPosition()
    {
        // Update the position based on the current GameObject's position
        if (minimapCircleInstance != null)
        {
            minimapCircleInstance.transform.position = new Vector3(
                transform.position.x,
                transform.position.y + 140,
                transform.position.z
            );
            minimapCircleInstance.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
