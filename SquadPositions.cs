using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


// Controls characters's positions in squad

public class SquadPositions : MonoBehaviour 
{
    public GameObject[] unitPrefabs = new GameObject[4];
    public List<GameObject> positions = new List<GameObject>();
    List<MainPanel> mainPanels = new List<MainPanel>();

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

                if(child.gameObject.tag == "Survivor")
                {
                    mainPanels[3 - i].AssignCharacter(child);
                }
            }
        }
    }

    // Returns bool[4] checking if characters are assigned at each position
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
            return this.positions[posNum - 1].transform.GetChild(0).GetComponent<Character>();
        }
        else
        {
            return null;
        }
    }


}
