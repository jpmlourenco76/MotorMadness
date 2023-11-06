using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapData
{
    public float lapTime;
    public List<Vector3> positions;
    public List<Quaternion> rotations;
    public List<float> timestamps;
}

public class LapManager : MonoBehaviour
{
    [SerializeField]
    private GameObject finalLapTrig;
    [SerializeField]
    private GameObject halfLapTrig;
    [SerializeField]
    private GameObject ghostCarPrefab;

    private LapData bestLapData;
    private int lapCount = 0;
    private float lapTime = 0f;
    private List<Vector3> positions = new List<Vector3> ();
    private List<Quaternion> rotations = new List<Quaternion> ();
    private List<float> timestamps = new List<float> ();

    private bool finalLapTriggered = false;


    // Start is called before the first frame update
    void Start()
    {
       bestLapData = new LapData();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == finalLapTrig && !finalLapTriggered)
        {
            IncrementLap();
            finalLapTriggered = true;
            finalLapTrig.SetActive(false);
        }
        else if (other.gameObject == halfLapTrig)
        {
            finalLapTriggered = false;
            finalLapTrig.SetActive(true);
        }
    }

    public void IncrementLap()
    {
        lapCount++;

        if (bestLapData == null || lapTime < bestLapData.lapTime)
        {
            bestLapData = new LapData
            {
                lapTime = lapTime,
                positions = new List<Vector3>(positions),
                rotations = new List<Quaternion>(rotations),
                timestamps = new List<float>(timestamps),
            };          
        }

        if (lapCount > 0)
        {
            SpawnGhostCar();
        }

        positions.Clear();
        rotations.Clear();
        timestamps.Clear();
        lapTime = 0f;
    }

    private void SpawnGhostCar()
    {
        GameObject ghostCar = Instantiate(ghostCarPrefab, startPosition, startRotation);
        GhostManager ghostCarController = ghostCar.GetComponent<GhostManager>();
        ghostCarController.SetLapData(bestLapData);
    }

    
    // Update is called once per frame
    void Update()
    {
        if (lapCount > 0)
        {
            lapTime += Time.deltaTime;
        }

        Vector3 currentPosition = tansform.position;
        Quaternion currentRotation = transform.rotation;

        positions.Add(currentPosition);
        rotations.Add(currentRotation);
        timestamps.Add(Time.time);
    }
    
    public void SaveBestLapData()
    {
        string jsonData = JsonUtility.ToJson(bestLapData);

        PlayerPrefs.SetString("BestLapData", jsonData);
        PlayerPrefs.Save();
    }

}
