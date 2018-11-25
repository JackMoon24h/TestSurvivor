using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class TurnControlMachine : MonoBehaviour 
{
    // Turn Related
    public enum TurnState
    {
        Player,
        Enemy
    }
    public TurnState turnState = TurnState.Player;
    public int turnCount = 1;

    public enum TurnStep
    {
        Selecting, // Player must select a skill command to use
        Targetting, // Player must decide the skill command he has chosen
        Confirming,
        Acting,
        Calculating,
        Finishing
    }
    public TurnStep turnStep = TurnStep.Selecting;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
