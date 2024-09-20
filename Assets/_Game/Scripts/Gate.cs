using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Gate : MonoBehaviour
{
    public GameObject goodCarpet; 
    public GameObject badCarpet;
    public GameObject goodGate;
    public GameObject badGate;

    [SerializeField] private CharacterMove character;

    private void Update()
    {
        if (character.isGood)
        {
            goodGate.SetActive(false);
            goodCarpet.SetActive(true);
            character.isGood = false;
            
          
            character.finalScore += 3;
        }
        if (character.isBad )
        {
            badGate.SetActive(false);
            badCarpet.SetActive(true);
           character.isBad = false;
           
            
            character.finalScore -= 4;
        }
        

    }
 
}
