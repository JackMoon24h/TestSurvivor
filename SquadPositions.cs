using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// Controls characters's positions in squad

public class SquadPositions : MonoBehaviour 
{
    public GameObject[] unitPrefabs = new GameObject[4];
    public List<GameObject> positions = new List<GameObject>();
    public List<Character> survivorList = new List<Character>();

    private void Start()
    {
        
    }

	
	// Update is called once per frame
	void Update () 
    {
		
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
            }
        }
    }

    public void UpdateSurvivorList()
    {
        survivorList = (Object.FindObjectsOfType<Character>() as Character[]).ToList();
    }
}
