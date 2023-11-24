using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLevel : MonoBehaviour
{
    public int level;
    private GameManager gameManager;
    public CharacterData characterData;


    private void Start()
    {
        gameManager = GameManager.Instance;
        
    }

    public void OnClick()
    {
        gameManager.desiredLevel = level;
    }
}
