using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySquadManager : MonoBehaviour 
{
    // References
    public EnemySquadPositions enemySquadPositions;
    public GameManager gameManager;
    [HideInInspector] public Camera mainCamera;

    public Enemies activeUnit;
    public List<Enemies> enemyList = new List<Enemies>();


    void Awake()
    {
        gameManager = Object.FindObjectOfType<GameManager>().GetComponent<GameManager>();
        mainCamera = Camera.main;
        enemySquadPositions = this.GetComponent<EnemySquadPositions>();
        enemySquadPositions.UpdatePosStatus();
    }

	
    public List<Enemies> GetCurrentEnemyList()
    {
        enemyList.Clear();

        for (int i = 0; i < 4; i++)
        {
            enemyList.Add(this.enemySquadPositions.GetEnemyAtPos(i + 1));
        }

        return enemyList;
    }

    public void SetActiveUnit(Enemies enemy)
    {
        if (this.activeUnit)
        {
            this.activeUnit.cursor.SetActive(false);
            this.activeUnit.isActive = false;
        }

        this.activeUnit = enemy;
        enemy.isActive = true;

        enemy.cursor.SetActive(true);
    }
	
}
