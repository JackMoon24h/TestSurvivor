using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySquadPositions : SquadPositions
{
    public Enemies GetEnemyAtPos(int posNum)
    {
        UpdatePosStatus();

        if (positionStatus[posNum - 1])
        {
            Debug.Log("Enemy at position number " + posNum + " is selected!");
            return this.positions[posNum - 1].transform.GetChild(0).GetComponent<Enemies>();
        }
        else
        {
            Debug.Log("Enemy at position number " + posNum + " is null=====");
            return null;
        }
    }
}
