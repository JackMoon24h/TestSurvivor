using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTrigger : MonoBehaviour 
{
    // Battle Trigger holds informations about enemies

    public GameManager gameManager;

    public GameObject[] enemyPrefabs = new GameObject[4];

	// Use this for initialization
	void Start () 
    {
        gameManager = Object.FindObjectOfType<GameManager>().GetComponent<GameManager>();
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            // Inform gameManager that this battle trigger has been initialized
            gameManager.InitBattle();
            Destroy(gameObject);
        }
    }
}
