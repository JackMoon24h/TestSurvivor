using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SquadManager : MonoBehaviour 
{
    // References
    public GameManager gameManager;
    [HideInInspector]public SquadInput squadInput;
    [HideInInspector]public SquadMover squadMover;
    [HideInInspector]public SquadPositions squadPositions;
    [HideInInspector]public Camera mainCamera;

    public Vector3 squadStartPos = new Vector3(0f, 0f, 0f);

    public Character activeUnit;
    public List<Character> characterList = new List<Character>();

	
	void Awake () 
    {
        gameManager = Object.FindObjectOfType<GameManager>().GetComponent<GameManager>();
        squadInput = this.GetComponent<SquadInput>();
        squadMover = this.GetComponent<SquadMover>();
        squadPositions = this.GetComponent<SquadPositions>();
        mainCamera = Camera.main;

        this.transform.position = squadStartPos;
        squadInput.InputEnabled = true;

	}

    private void Start()
    {
        squadPositions.DeployUnits();
        squadPositions.UpdatePosStatus();

        var temp = squadPositions.GetCharacterAtPos(1);
        this.SetActiveUnit(temp);
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

    public List<Character> GetCurrentCharacterList()
    {
        characterList.Clear();

        for (int i = 0; i < 4; i++)
        {
            characterList.Add(this.squadPositions.GetCharacterAtPos(i+1));
        }

        return characterList;
    }

    public void SetActiveUnit(Character character)
    {
        if(this.activeUnit)
        {
            this.activeUnit.cursor.SetActive(false);
            this.activeUnit.isActive = false;
        }

        this.activeUnit = character;
        character.isActive = true;

        character.cursor.SetActive(true);
    }
}
