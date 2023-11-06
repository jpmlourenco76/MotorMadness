using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostManager : MonoBehaviour
{
    private LapData lapData;
    private int currentDataIndex = 0;
    private float startTime;
    private Vector3 startPosition;
    private Quaternion startRotation;

    [SerializeField]
    private float playbackSpeed = 1.0f;


    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    public void SetLapData(LapData lapData)
    {
        this.lapData = lapData;
        currentDataIndex = 0;
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (lapData == null || currentDataIndex >= lapData.positions.Count)
        {
            return;
        }

        float currentTime = Time.time - startTime;

        Vector3 targetPosition = lapData.positions[currentDataIndex];
        Quaternion targetRotation = lapData.rotations[currentDataIndex];
        float timestamp = lapData.timestamps[currentDataIndex];

        float interpolationTime = (currentTime - timestamp) * playbackSpeed;

        transform.position = Vector3.Lerp(startPosition, targetPosition, interpolationTime);
        transform.rotation = Quaternion.Slerp(startRotation, targetRotation, interpolationTime);

        if(interpolationTime >= 1.0f)
        {
            currentDataIndex++;
            if(currentDataIndex < lapData.positions.Count)
            {
                startTime = Time.time;
                startRotation = transform.rotation;
                startPosition = transform.position;
            }
        }
    }
}
