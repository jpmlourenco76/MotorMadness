using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelType : MonoBehaviour
{
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
    }


    public void SetStory()
    {
        gameManager.SetLevelType(GameManager.LevelType.Story);
    }
    public void SetTraining()
    {
        gameManager.SetLevelType(GameManager.LevelType.Training);
    }
    public void SetQuickplay()
    {
        gameManager.SetLevelType(GameManager.LevelType.QuickPlay);
    }

}
