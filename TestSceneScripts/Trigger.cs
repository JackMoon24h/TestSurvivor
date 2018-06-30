using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour {

	public enum Type
	{
		NORMAL,
		BOSS,
		ITEM,
		DOOR,
		GOAL
	}

	public Type triggerType;

	private Manager gameManager;
	public BaseEnemy[] initialEnemyList = new BaseEnemy[4];

	void Start(){
		gameManager = Manager.instance;
	}

	void OnTriggerEnter2D(Collider2D other){
		if(other.tag == "Survivor"){
			gameManager.InitBattle (initialEnemyList);
			Destroy (gameObject);
		}
	}
}
