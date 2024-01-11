
using TMPro;
using UnityEngine;

public class MLevelUi : MonoBehaviour
{
    private MultiplayerManager multiplayerManager;

    public CarController2 carController;
    private GameObject neeedle;
    private float startPosition = 32f, endPosition = -211f;
    private float desiredPosition;
    private TMPro.TextMeshProUGUI gear;
    private TMPro.TextMeshProUGUI kph;
    private bool once = true;

    // Start is called before the first frame update
    private void Awake()
    {
        multiplayerManager = GetComponent<MultiplayerManager>();
        gear = GameObject.Find("Gear").GetComponent<TextMeshProUGUI>();
        kph = GameObject.Find("Speed").GetComponent<TextMeshProUGUI>();
        neeedle = GameObject.Find("needle");
    }


 
    // Update is called once per frame
    private void FixedUpdate()
    {
        carController = multiplayerManager.FindLocalPlayer();
       

        if (carController != null)
        {
            kph.text = carController.KPH.ToString("0");
            updateNeedle();
            changeGear();
        }

    }

    public void updateNeedle()
    {
        desiredPosition = startPosition - endPosition;
        float temp = carController.EngineRPM / 10000;
        neeedle.transform.eulerAngles = new Vector3(0, 0, (startPosition - temp * desiredPosition));

    }

    public void changeGear()
    {
        gear.text = (carController.CurrentGear != -1) ? (carController.CurrentGear).ToString() : "R";

    }
}
