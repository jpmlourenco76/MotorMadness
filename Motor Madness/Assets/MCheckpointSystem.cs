using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;





public class MCheckpointSystem : MonoBehaviour
{
    public List<CarData> cars;

    public List<CarData> distanceArray = new List<CarData>();

    public TextMeshProUGUI carText;

    public GameObject nextCheckPoint;

    private MultiplayerManager multiplayerManager;
    private MLevelManager levelManager;

    public bool isFinishLine = false;
    private float timeSinceLastExecution = 0f;
    private float executionInterval = 0.5f; // 0.2 seconds interval

    private bool once = true;

    private void Awake()
    {
       

        multiplayerManager = GameObject.Find("MultiplayerManager").GetComponent<MultiplayerManager>();
        levelManager= GameObject.Find("MultiplayerManager").GetComponent<MLevelManager>();

        carText = GameObject.Find("PosDisplay").GetComponent<TextMeshProUGUI>();

    }

    private void Update()
    {
        if(once && multiplayerManager.startlevel)
        {
            cars = multiplayerManager.cars;
            once = false;
        }

        if(!once && !multiplayerManager.startlevel)
        {
            cars.Clear();
            multiplayerManager.cars.Clear();
           once = true;
        }

        if (multiplayerManager.startlevel && !multiplayerManager.racefinish)
        {
            timeSinceLastExecution += Time.deltaTime;

            // Check if the desired time interval has passed
            if (timeSinceLastExecution >= executionInterval)
            {
                // Calculate distances for all cars
                distanceArray.Clear();

                for (int i = 0; i < cars.Count; i++)
                {
                    float distance = Vector3.Distance(transform.position, GameObject.Find("Car" + i).GetComponent<Rigidbody>().position);
                    if (distance < 25f)
                    {
                        nextCheckPoint.SetActive(true);
                        gameObject.SetActive(false);
                    }

                    cars[i].distance = distance;
                    cars[i].lap = GameObject.Find("Car" + i).GetComponent<CarController2>().laps;
                    cars[i].checkpoints = GameObject.Find("Car" + i).GetComponent<CarController2>().pchecks;
                    distanceArray.Add(cars[i]);
                    levelManager.distanceArray = distanceArray;

                }

                // Sort distances
                distanceArray.Sort(new CarDataComparer());

                GameObject myCar = multiplayerManager.FindLocalPlayer().gameObject;


                // Update car text based on distance
                for (int i = 0; i < cars.Count; i++)
                {
                    if (Vector3.Distance(transform.position, GameObject.Find("Car" + myCar.GetComponent<MultiplayerCar>().carIdentifier).GetComponent<Rigidbody>().position) == distanceArray[i].distance)
                    {
                        carText.text = $"{i + 1}/{cars.Count}";

                    }
                }

                // Reset the timer
                timeSinceLastExecution = 0f;
            }
        }
       
    }



    public class CarDataComparer : IComparer<CarData>
    {
        public int Compare(CarData x, CarData y)
        {
            // First, compare by lap in descending order
            if (x.lap != y.lap)
            {
                return y.lap.CompareTo(x.lap);
            }

            // If lap is the same, compare by checkpoints in descending order
            if (x.checkpoints != y.checkpoints)
            {
                return y.checkpoints.CompareTo(x.checkpoints);
            }

            // If lap and checkpoints are the same, compare by distance in ascending order
            return x.distance.CompareTo(y.distance);
        }
    }

    public void SetupNextCheckpoint(MCheckpointSystem nextCheckpointSystem)
    {
        nextCheckPoint = nextCheckpointSystem.gameObject;
    }

}
