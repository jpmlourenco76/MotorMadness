using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TMPro;
using UnityEngine;

public class UpgradeMenu : MonoBehaviour
{
    private CharacterData characterData;
    private GameManager gameManager;
    public CarMenuManager carMenuManager;
    


    private GameObject Torquebtn;
    private GameObject TorquePrice;

    private GameObject Breaksbtn;
    private GameObject BreaksPrice;

    private GameObject Gearbtn;
    private GameObject GearPrice;

    private GameObject Tiresbtn;
    private GameObject TiresPrice;

    public int[] price = {500, 1500, 4000}; 
    private void Awake()
    {
        gameManager = GameManager.Instance;
        characterData = gameManager.GetCurrentCharacter();
        carMenuManager = GameObject.Find("VehicleSelect").GetComponent<CarMenuManager>();


        Torquebtn = GameObject.Find("Torquebtn");
        TorquePrice = GameObject.Find("Torque");
        Breaksbtn = GameObject.Find("Breaksbtn");
        BreaksPrice = GameObject.Find("Breaks");
        Tiresbtn = GameObject.Find("Tiresbtn");
        TiresPrice = GameObject.Find("Tires");
        Gearbtn = GameObject.Find("Gearboxbtn");
        GearPrice = GameObject.Find("Gearbox");
    }

    private void Update()
    {
       
        Torquebtn.GetComponentInChildren<TextMeshProUGUI>().text = characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.TorqueUpgrade.ToString() + "/3";
        TorquePrice.GetComponent<TextMeshProUGUI>().text = "Engine \n" + price[characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.TorqueUpgrade].ToString();

        Breaksbtn.GetComponentInChildren<TextMeshProUGUI>().text = characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.BreakUpgrade.ToString() + "/3";
        BreaksPrice.GetComponent<TextMeshProUGUI>().text = "Breaks \n" + price[characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.BreakUpgrade].ToString();

        Tiresbtn.GetComponentInChildren<TextMeshProUGUI>().text = characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.TireUpdrage.ToString() + "/3";
        TiresPrice.GetComponent<TextMeshProUGUI>().text = "Tires \n" + price[characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.TireUpdrage].ToString();

        Gearbtn.GetComponentInChildren<TextMeshProUGUI>().text = characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.GearUpgrade.ToString() + "/3";
        GearPrice.GetComponent<TextMeshProUGUI>().text = "Gearbox \n" + price[characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.GearUpgrade].ToString();

    }




    public void TorqueUpgrade() {
    
    if(characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.TorqueUpgrade < 3 && characterData.money > price[characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.TorqueUpgrade])
        {
            characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.TorqueUpgrade++;
            characterData.money = characterData.money - price[characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.TorqueUpgrade];

            Torquebtn.GetComponentInChildren<TextMeshProUGUI>().text = characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.TorqueUpgrade.ToString() + "/3";
            TorquePrice.GetComponent<TextMeshProUGUI>().text = "Engine \n" + price[characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.TorqueUpgrade].ToString();
        }
    
    }

    public void BreaksUpgrade()
    {

        if (characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.BreakUpgrade < 3 && characterData.money > price[characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.BreakUpgrade])
        {
            characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.BreakUpgrade++;
            characterData.money = characterData.money - price[characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.BreakUpgrade];

            Torquebtn.GetComponentInChildren<TextMeshProUGUI>().text = characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.BreakUpgrade.ToString() + "/3";
            TorquePrice.GetComponent<TextMeshProUGUI>().text = "Breaks \n" + price[characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.BreakUpgrade].ToString();
        }

    }

    public void GearUpgrade()
    {

        if (characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.GearUpgrade < 3 && characterData.money > price[characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.GearUpgrade])
        {
            characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.GearUpgrade++;
            characterData.money = characterData.money - price[characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.GearUpgrade];

            Torquebtn.GetComponentInChildren<TextMeshProUGUI>().text = characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.GearUpgrade.ToString() + "/3";
            TorquePrice.GetComponent<TextMeshProUGUI>().text = "Gearbox \n" + price[characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.GearUpgrade].ToString();
        }

    }

    public void TireUpgrade()
    {

        if (characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.TireUpdrage < 3 && characterData.money > price[characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.TireUpdrage])
        {
            characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.TireUpdrage++;
            characterData.money = characterData.money - price[characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.TireUpdrage];

            Torquebtn.GetComponentInChildren<TextMeshProUGUI>().text = characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.TireUpdrage.ToString() + "/3";
            TorquePrice.GetComponent<TextMeshProUGUI>().text = "Tires \n" + price[characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.TireUpdrage].ToString();
        }

    }
}
