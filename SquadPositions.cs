using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


// Controls characters's positions in squad

public class SquadPositions : MonoBehaviour 
{
    public GameObject[] unitPrefabs = new GameObject[4];
    public List<GameObject> positions = new List<GameObject>();
    public List<MainPanel> mainPanels = new List<MainPanel>();

    public bool[] positionStatus = new bool[4];

    private void Awake()
    {
        mainPanels = (Object.FindObjectsOfType<MainPanel>() as MainPanel[]).ToList();
    }


    public void DeployUnits()
    {
        for (int i = 0; i < unitPrefabs.Length; i++)
        {
            if (unitPrefabs[i] != null)
            {
                var child = Instantiate(unitPrefabs[i], Vector3.zero, Quaternion.identity);
                child.transform.parent = positions[i].transform;
                child.transform.localPosition = Vector3.zero;

                mainPanels[3-i].AssignCharacter(child);
            }
        }
    }

    public void UpdatePosStatus()
    {
        for (int i = 0; i < positions.Count; i++)
        {
            var chara = positions[i].transform.GetChild(0);

            if(chara == null)
            {
                positionStatus[i] = false;
            }
            else
            {
                positionStatus[i] = true;
            }
        }
    }

    public Character GetCharacterAtPos(int posNum)
    {
        UpdatePosStatus();

        if(positionStatus[posNum - 1])
        {
            Debug.Log("Character at position number " + posNum + " is selected!");
            return this.positions[posNum - 1].transform.GetChild(0).GetComponent<Character>();
        }
        else
        {
            Debug.Log("Character at position number " + posNum + " is null=====");
            return null;
        }
    }

}
