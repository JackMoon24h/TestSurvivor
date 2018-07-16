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
            return this.positions[posNum - 1].transform.GetChild(0).GetComponent<Enemies>();
        }
        else
        {
            return null;
        }
    }
}
