using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySquadManager : MonoBehaviour 
{
    public EnemySquadPositions enemySquadPositions;

	// Use this for initialization
	void Start () 
    {
        enemySquadPositions = this.GetComponent<EnemySquadPositions>();

        enemySquadPositions.UpdatePosStatus();
	}
	
	
}
