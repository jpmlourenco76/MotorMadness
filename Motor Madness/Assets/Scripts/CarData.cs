using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;


[System.Serializable]
public class CharacterData
{
    public string characterName;
    public int currentLevel;
    public int money;
    public int position;
    public List<CarData> OwnedCars;
    public CarData SelectedCar;
    public List<Material> CarMaterials;
    


    public CharacterData(string name)
    {
        characterName = name;
        currentLevel = 1;
        position = 0;
        money = 0;
        OwnedCars = new List<CarData>();
        SelectedCar = new CarData();
        
        
    }

    public void SetRacerNameForOwnedCars()
    {
        foreach (var ownedCar in OwnedCars)
        {
            ownedCar.RacerName = characterName;
        }
    }

    public void setPoints()
    {
        int maxPoints = int.MinValue;

        foreach (var ownedCar in OwnedCars)
        {
            if (maxPoints < ownedCar.points)
            {
                maxPoints = ownedCar.points;
            }
        }

  
    
        foreach (var ownedCar in OwnedCars)
        {
            ownedCar.points = maxPoints;
        }
    }
    public void updateMaterials()
    {
        foreach(var ownedCar in OwnedCars)
        {
            if (ownedCar.CarPrefab.name.Contains("1"))
            {
                ownedCar.material = CarMaterials[0];
            }
            else if (ownedCar.CarPrefab.name.Contains("2"))
            {
                ownedCar.material = CarMaterials[1];
            }
            else if (ownedCar.CarPrefab.name.Contains("3"))
            {
                ownedCar.material = CarMaterials[2];
            }
            else if (ownedCar.CarPrefab.name.Contains("4"))
            {
                ownedCar.material = CarMaterials[3];
            }
            else if (ownedCar.CarPrefab.name.Contains("5"))
            {
                ownedCar.material = CarMaterials[4];
            }
            else if (ownedCar.CarPrefab.name.Contains("6"))
            {
                ownedCar.material = CarMaterials[5];
            }
            else if (ownedCar.CarPrefab.name.Contains("7"))
            {
                ownedCar.material =CarMaterials[6];
            }
            else if (ownedCar.CarPrefab.name.Contains("8"))
            {
                ownedCar.material = CarMaterials[7];
            }
            else if (ownedCar.CarPrefab.name.Contains("9"))
            {
                ownedCar.material = CarMaterials[8];
            }
            else if (ownedCar.CarPrefab.name.Contains("10"))
            {
                ownedCar.material = CarMaterials[9];
            }
            else if (ownedCar.CarPrefab.name.Contains("11"))
            {
                ownedCar.material = CarMaterials[10];
            }
            else if (ownedCar.CarPrefab.name.Contains("12"))
            {
                ownedCar.material = CarMaterials[11];
            }
            else if (ownedCar.CarPrefab.name.Contains("13"))
            {
                ownedCar.material = CarMaterials[12];
            }
            else if (ownedCar.CarPrefab.name.Contains("14"))
            {
                ownedCar.material = CarMaterials[13];
            }
        }
    }

}


[System.Serializable]
public class CarData
{
    public int CarID;
    public string CarName;
    public string RacerName;
    public GameObject CarPrefab;
    public float distance;
    public float lap;
    public float checkpoints;
    public float wanderAmount;
    public int points;
    public int price;
    public Material material;
    public CarUpgrade upgradelvls;

    public CarData()
    {
        upgradelvls = new CarUpgrade();
    }
    public object Clone()
    {
        return new CarData
        {
            CarID = CarID,
            CarName = CarName,
            RacerName = RacerName,
            CarPrefab = CarPrefab,
            distance = distance,
            lap = lap,
            checkpoints = checkpoints,
            wanderAmount = wanderAmount,
            points = points,
            price = price
        };
    }
}



[System.Serializable]
public class CarUpgrade
{
    public int TorqueUpgrade;
    public int GearUpgrade;
    public int BreakUpgrade;
    public int TireUpdrage;

    public CarUpgrade()
    {
        TorqueUpgrade = 0;
        GearUpgrade = 0;
        BreakUpgrade = 0;
        TireUpdrage = 0;

    }
}


[System.Serializable]
public class GameData{
    public List<CharacterData> characters;
    public List<CarData> GameCars;


    public GameData()
    {
        characters = new List<CharacterData>();
        GameCars = new List<CarData>();
    }
}

