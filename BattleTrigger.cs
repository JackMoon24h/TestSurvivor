using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTrigger : MonoBehaviour 
{
    GameManager gameManager;
    public GameObject enemySquadPrefab;

	// Use this for initialization
	void Start () 
    {
        gameManager = Object.FindObjectOfType<GameManager>().GetComponent<GameManager>();
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(enemySquadPrefab != null && collision.tag == "Player")
        {
            // Inform gameManager that this battle trigger has been initialized
            gameManager.InitBattle(enemySquadPrefab);
            Destroy(gameObject);
        }
    }
}
