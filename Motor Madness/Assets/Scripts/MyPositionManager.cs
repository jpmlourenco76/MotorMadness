using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class MyPositionManager : MonoBehaviour
{
    public TextMeshProUGUI positionDisplay;
    private Dictionary<Transform, int> carPositions = new Dictionary<Transform, int>();

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider is a car and has a corresponding Transform.
        Transform carTransform = other.transform;
        if (carTransform.CompareTag("Car"))
        {
            // Check if the car has passed this checkpoint.
            if (!carPositions.ContainsKey(carTransform))
            {
                // Increment the car's progress.
                carPositions[carTransform] = carPositions.Count + 1;

                // Update the car's position in the UI.
                UpdatePositionUI();
            }
        }
    }

    private void UpdatePositionUI()
    {
        // Sort the cars based on the number of checkpoints they've passed.
        List<Transform> sortedCars = new List<Transform>(carPositions.Keys);
        sortedCars.Sort((car1, car2) => carPositions[car1].CompareTo(carPositions[car2]));

        // Update the UI based on the sorted car positions.
        int currentPosition = sortedCars.IndexOf(transform);
        positionDisplay.text = (currentPosition + 1) + GetPositionSuffix(currentPosition + 1);
    }

    private string GetPositionSuffix(int position)
    {
        if (position <= 0) return "th";
        if (position % 100 >= 11 && position % 100 <= 13) return "th";
        switch (position % 10)
        {
            case 1: return "st";
            case 2: return "nd";
            case 3: return "rd";
            default: return "th";
        }
    }
}