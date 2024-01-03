using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelect : MonoBehaviour
{
    public List<Character> CharacterList;

    private void Awake()
    {
        CharacterList = new List<Character>(GetComponentsInChildren<Character>());
    }

    
}
