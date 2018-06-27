using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Positions : MonoBehaviour 
{
    public int position;
    public BoxCollider2D positionCollider;
    public GameObject positionedUnit;


	// Use this for initialization
	void Start () 
    {
        positionCollider = GetComponent<BoxCollider2D>();
        positionedUnit = this.transform.GetChild(position - 1).gameObject;
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    void OnClick()
    {
        // If it is in the battle, Start Coroutine

        // Get positioned units's information

        // Show the information on command panel

        // Select a Skill to use

        // Select targets

        // Skill Animations

    }
}
