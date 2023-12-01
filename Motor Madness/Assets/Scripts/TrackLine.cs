using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackLine : MonoBehaviour
{
    public List<Transform> waypoints = new List<Transform>();
    public LineRenderer trackLineRenderer;
    public float startWidth;
    public float endWidth;
    public bool useMaterial1 = true;
    public Material material1;
    public Material material2;

    void Start()
    {
        CollectWaypoints();

        // Initialize LineRenderer
        if (trackLineRenderer != null)
        {
            int numWaypoints = waypoints.Count;
            int numSegments = numWaypoints * 10; // Adjust the multiplier for smoother curves

            trackLineRenderer.positionCount = numSegments;

            for (int i = 0; i < numSegments; i++)
            {
                float t = i / (float)(numSegments - 1);
                trackLineRenderer.SetPosition(i, GetCatmullRomPosition(t));
            }

            trackLineRenderer.startWidth = startWidth;
            trackLineRenderer.endWidth = endWidth;

            Material selectedMaterial = useMaterial1 ? material1 : material2;
            Shader unlitShader = Shader.Find("Unlit/Color");
            selectedMaterial.shader = unlitShader;
            trackLineRenderer.material = selectedMaterial;

        }
    }

    void CollectWaypoints()
    {
        Transform[] childTransforms = GetComponentsInChildren<Transform>();

        foreach (Transform childTransform in childTransforms)
        {
            if (childTransform != transform) // Exclude the parent (this GameObject)
            {
                waypoints.Add(childTransform);
            }
        }
    }

    Vector3 GetCatmullRomPosition(float t)
    {
        int numPoints = waypoints.Count;
        float segment = t * (numPoints - 1);
        int i = Mathf.FloorToInt(segment);
        float u = segment - i;

        Vector3 p0 = waypoints[Mathf.Clamp(i - 1, 0, numPoints - 1)].position;
        Vector3 p1 = waypoints[i].position;
        Vector3 p2 = waypoints[Mathf.Clamp(i + 1, 0, numPoints - 1)].position;
        Vector3 p3 = waypoints[Mathf.Clamp(i + 2, 0, numPoints - 1)].position;

        return 0.5f * (
            (-p0 + 3f * p1 - 3f * p2 + p3) * (u * u * u) +
            (2f * p0 - 5f * p1 + 4f * p2 - p3) * (u * u) +
            (-p0 + p2) * u +
            2f * p1
        );
    }
}
