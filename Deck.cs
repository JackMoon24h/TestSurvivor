using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour 
{
    Overseer overseer;
    Camera mainCamera;
    Camera characterCamera;
    public InputManager inputManager;

    public List<GameObject> unitPrefabs = new List<GameObject>();
    public List<Unit> slots = new List<Unit>();
    public List<Unit> currentUnitList = new List<Unit>();
    public Unit activeUnit;

    private void Awake()
    {
        overseer = Object.FindObjectOfType<Overseer>().GetComponent<Overseer>();
        mainCamera = Camera.main;
        characterCamera = GameObject.Find("CharacterCamera").GetComponent<Camera>();
        inputManager = this.GetComponent<InputManager>();
    }

    // Use this for initialization
    void Start () 
	{
        SetActiveUnitByPosition(1);
	}

    public Unit GetUnitByPosition(int position)
    {
        if(this.slots[position - 1].isDead)
        {
            return null; 
        }
        return this.slots[position - 1];
    }


    public Unit GetUnitByClick(Collider2D collider)
    {
        var target = collider.gameObject.GetComponent<Unit>();

        if(!overseer.IsBattle)
        {
            this.SetActiveUnitByClick(target);
        }

        return target;
    }

    public List<Unit> GetCurrentUnitList()
    {
        currentUnitList.Clear();

        for (int i = 0; i < 4; i++)
        {
            currentUnitList.Add(this.GetUnitByPosition(i + 1));
        }

        return currentUnitList;
    }

    public GameObject GetObjectByClick(Collider2D collider)
    {
        var target = collider.gameObject;

        return target;
    }

    public void SetActiveUnitByPosition(int position)
    {
        // Check if a unit exists at the given position
        if (slots[position - 1].isDead)
        {
            Debug.Log("Position number " + position + " is already dead");
        }
        else
        {
            // Check if already active unit exists
            if (activeUnit != null)
            {
                activeUnit.SetActive(false);
            }

            activeUnit = slots[position - 1];
            activeUnit.SetActive(true);
        }
    }

    public void SetActiveUnitByClick(Unit target)
    {
        if (activeUnit != null)
        {
            activeUnit.SetActive(false);
        }

        activeUnit = target;
        activeUnit.SetActive(true);
    }

	
	// Update is called once per frame
	void Update () 
	{
        inputManager.GetTouchInput();
	}
}
