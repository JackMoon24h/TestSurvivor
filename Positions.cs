using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Positions : MonoBehaviour 
{
    public int number;
    public GameObject positionedUnit;
    public Character character;


	// Use this for initialization
	void Start () 
    {
        
	}
	
    public void GetPositionedUnit()
    {
        positionedUnit = this.gameObject.transform.GetChild(0).gameObject;
        if(positionedUnit != null)
        {
            character = positionedUnit.GetComponent<Character>();
        }
    }

    // If this.collider is clicked
    void OnClick()
    {
        // If it is in the middle of battle, Start Coroutine

        // Get positioned units's information

        // Show the information on command panel

        // Select a Skill to use

        // If the unit positioned is active

        // Draw targets

        // Confirm order

        // Skill animations

    }


}
