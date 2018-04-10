using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skills : MonoBehaviour {

	Manager gameManager;

	public enum Target
	{
		SELF,
		SURVIVOR,
		ENEMY
	}

	public enum Type
	{
		DAMAGE,
		HEAL,
		BUFF,
		DEBUFF
	}

	public enum AddEffect
	{
		NONE,
		INFECT,
		BLEED,
		STUN,
		MOVE
	}

	public Target skillTarget;
	public Type skillType;
	public AddEffect addEffect;
	public int[] availablePositions = new int[4];
	public int[] castRange = new int[4];
	public bool canSelectRange;

	void Start(){
		gameManager = Manager.instance;
	}

//	public GameObject[] GetTargets(){

//		switch(this.skillType){
//		case Target.SELF:
//			
//			return gameManager.activeSurvivor;
//			break;
//		case Target.SURVIVOR:
//			
//			foreach(BaseSurvivor t in gameManager.survivorList){
//				if()
//			}
//			break;
//		case Target.ENEMY:
//			
//			break;
//		}
//
//		foreach(GameObject t in gameManager.enemyList){
//			
//		}
//	}

	public void DrawTarget(){
		
	}

	public void CastSkill(){
		
	}
}
