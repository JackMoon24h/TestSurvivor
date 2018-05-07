using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour {

	public int currentPos;
	public string name;

	public int healthMax;
	public int health;
	public int damage;
	public int mentalDamage;
	public int protection;
	public int speed;

	public float accuracy;
	public float dodge;
	public float critical;
	public float virtue;

	public float infectRes;
	public float bleedRes;
	public float fearRes;
	public float stunRes;
	public float moveRes;


	public enum PhysicalState
	{
		FINE,
		INFECTED,
		BLEEDING,
		FEARED,
		VIGOROUS
	}

	public enum TurnState
	{
		WAITING,
		SELECTING,
		CONFIRMING,
		ACTING
	}

	public PhysicalState physicalState;
	public TurnState turnState;
	public bool isTargeted = false;

	public bool isActive = false;
	public bool wasActivePrev = false;
	public bool actedInTurn = false;

	[HideInInspector]public BoxCollider2D collider;
	private Animator animator;
	private Manager gameManager;
	private GameObject mainCamera;


	// Use this for initialization
	protected virtual void Start () {
		collider = this.GetComponent<BoxCollider2D> ();
		animator = this.GetComponent<Animator>();
		gameManager = Manager.instance;
		mainCamera = GameObject.FindGameObjectWithTag ("MainCamera");

		physicalState = PhysicalState.FINE;
	}

	public int GetPosition(BaseEnemy enemy){
		return enemy.currentPos;
	}

	public void OnClickEnterEnemy(){
		if(this.isTargeted)
		{
			gameManager.PerformAction ();
		}
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		if (!gameManager.isBattle)
			gameObject.SetActive (false);
	}

	protected virtual void SetActive(bool selection){
		this.isActive = selection;
	}

	public void SetPosition(int posNum){
		this.currentPos = posNum;
	}

}
