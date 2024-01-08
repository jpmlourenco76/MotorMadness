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

    public int[] price = {50000, 75000, 100000}; 
    private void Awake()
    {
        
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

    private void Start()
    {
        gameManager = GameManager.Instance;
        characterData = gameManager.GetCurrentCharacter();
    }
    private void Update()
    {
        if (gameManager.levelType == GameManager.LevelType.Story)
        {


            if (characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.TorqueUpgrade < 3)
            {
                Torquebtn.GetComponentInChildren<TextMeshProUGUI>().text = characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.TorqueUpgrade.ToString() + "/3";
                TorquePrice.GetComponent<TextMeshProUGUI>().text = "Engine \n" + price[characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.TorqueUpgrade].ToString();
            }
            else
            {
                Torquebtn.GetComponentInChildren<TextMeshProUGUI>().text = "3/3";
                TorquePrice.GetComponent<TextMeshProUGUI>().text = "Engine \n ---";
            }

            if (characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.BreakUpgrade < 3)
            {
                Breaksbtn.GetComponentInChildren<TextMeshProUGUI>().text = characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.BreakUpgrade.ToString() + "/3";
                BreaksPrice.GetComponent<TextMeshProUGUI>().text = "Breaks \n" + price[characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.BreakUpgrade].ToString();
            }
            else
            {
                Breaksbtn.GetComponentInChildren<TextMeshProUGUI>().text = "3/3";
                BreaksPrice.GetComponent<TextMeshProUGUI>().text = "Breaks \n ---";
            }

            if (characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.GearUpgrade < 3)
            {

                Gearbtn.GetComponentInChildren<TextMeshProUGUI>().text = characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.GearUpgrade.ToString() + "/3";
                GearPrice.GetComponent<TextMeshProUGUI>().text = "Gearbox \n" + price[characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.GearUpgrade].ToString();
            }
            else
            {
                Gearbtn.GetComponentInChildren<TextMeshProUGUI>().text = "3/3";
                GearPrice.GetComponent<TextMeshProUGUI>().text = "Gearbox \n ---";

            }

            if (characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.TireUpdrage < 3)
            {
                Tiresbtn.GetComponentInChildren<TextMeshProUGUI>().text = characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.TireUpdrage.ToString() + "/3";
                TiresPrice.GetComponent<TextMeshProUGUI>().text = "Tires \n" + price[characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.TireUpdrage].ToString();
            }
            else
            {
                Tiresbtn.GetComponentInChildren<TextMeshProUGUI>().text = "3/3";
                TiresPrice.GetComponent<TextMeshProUGUI>().text = "Tires \n ---";
            }



        }

    }




    public void TorqueUpgrade() 
    {
        
        if (characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.TorqueUpgrade < 3 && characterData.money >= price[characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.TorqueUpgrade])
        {
            characterData.money = characterData.money - price[characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.TorqueUpgrade];
            characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.TorqueUpgrade++;

            Torquebtn.GetComponentInChildren<TextMeshProUGUI>().text = characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.TorqueUpgrade.ToString() + "/3";
            if (characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.TorqueUpgrade < 2)
            {
                TorquePrice.GetComponent<TextMeshProUGUI>().text = "Engine \n" + price[characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.TorqueUpgrade].ToString();

            }
        }
        else
        {
            Torquebtn.GetComponentInChildren<TextMeshProUGUI>().text = "3/3";
            TorquePrice.GetComponent<TextMeshProUGUI>().text = "Engine \n ---";
        }

    }

    public void BreaksUpgrade()
    {

        if (characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.BreakUpgrade < 3 && characterData.money >= price[characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.BreakUpgrade])
        {            
            characterData.money = characterData.money - price[characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.BreakUpgrade];
            characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.BreakUpgrade++;

            Breaksbtn.GetComponentInChildren<TextMeshProUGUI>().text = characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.BreakUpgrade.ToString() + "/3";
            if(characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.BreakUpgrade < 2)
            {
                BreaksPrice.GetComponent<TextMeshProUGUI>().text = "Breaks \n" + price[characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.BreakUpgrade].ToString();

            }
        }
        else
        {
            Breaksbtn.GetComponentInChildren<TextMeshProUGUI>().text = "3/3";
            BreaksPrice.GetComponent<TextMeshProUGUI>().text = "Breaks \n ---";
        }

    }

    public void GearUpgrade()
    {

        if (characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.GearUpgrade < 3 && characterData.money >= price[characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.GearUpgrade])
        {            
            characterData.money = characterData.money - price[characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.GearUpgrade];
            characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.GearUpgrade++;

            Gearbtn.GetComponentInChildren<TextMeshProUGUI>().text = characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.GearUpgrade.ToString() + "/3";
            if (characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.GearUpgrade < 2)
            {
                GearPrice.GetComponent<TextMeshProUGUI>().text = "Gearbox \n" + price[characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.GearUpgrade].ToString();

            }
        }
        else
        {
            Gearbtn.GetComponentInChildren<TextMeshProUGUI>().text = "3/3";
            GearPrice.GetComponent<TextMeshProUGUI>().text = "Gearbox \n ---";

        }

    }

    public void TireUpgrade()
    {

        if (characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.TireUpdrage < 3 && characterData.money >= price[characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.TireUpdrage])
        {
            characterData.money = characterData.money - price[characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.TireUpdrage];
            characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.TireUpdrage++;

            Tiresbtn.GetComponentInChildren<TextMeshProUGUI>().text = characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.TireUpdrage.ToString() + "/3";
            if (characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.TireUpdrage < 2)
            {
                TiresPrice.GetComponent<TextMeshProUGUI>().text = "Tires \n" + price[characterData.OwnedCars[carMenuManager.vehiclePointer].upgradelvls.TireUpdrage].ToString();

            }
        }
        else
        {
            Tiresbtn.GetComponentInChildren<TextMeshProUGUI>().text = "3/3";
            TiresPrice.GetComponent<TextMeshProUGUI>().text = "Tires \n ---";
        }

    }
}
