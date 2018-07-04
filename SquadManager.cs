﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SquadManager : MonoBehaviour 
{
    public GameManager gameManager;
    [HideInInspector]public SquadInput squadInput;
    [HideInInspector]public SquadMover squadMover;
    [HideInInspector]public SquadPositions squadPositions;
    [HideInInspector]public Camera mainCamera;

    public Vector3 squadStartPos = new Vector3(0f, 0f, 0f);

    public MainPanel mainPanel;
    public Character activeUnit;
	
	void Awake () 
    {
        gameManager = Object.FindObjectOfType<GameManager>().GetComponent<GameManager>();
        squadInput = this.GetComponent<SquadInput>();
        squadMover = this.GetComponent<SquadMover>();
        squadPositions = this.GetComponent<SquadPositions>();
        mainCamera = Camera.main;

        mainPanel = GameObject.Find("MainPanel").GetComponent<MainPanel>();

        this.transform.position = squadStartPos;
        squadInput.InputEnabled = true;
	}

    private void Start()
    {
        squadPositions.DeployUnits();
    }


    void Update () 
    {
        // Get touch input and direction;
        squadInput.GetTouchInput();

        if(!gameManager.IsBattle && squadInput.moveInputDetected)
        {
            if(squadInput.Direction > 0)
            {
                squadMover.MoveForward();
            }
            else if (squadInput.Direction < 0)
            {
                squadMover.MoveBackWard();
            }
        }
	}


    public void UpdateActiveUnit()
    {
        
    }
}
