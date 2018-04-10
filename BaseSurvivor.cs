using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class BaseSurvivor : MonoBehaviour {

	public int currentPos;
	public float velocity = 3.0f;

	public string name;

	public int healthMax;
	public int health;
	public int mentalMax;
	public int mental;
	public int damage;
	public int protection;
	public int tolerance;
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
	public float deathBlow;

	public enum PhysicalState
	{
		FINE,
		INFECT,
		BLEED,
		STUN,
		FEAR, // DEBUFF
		COUNTER,
		BUFF,
		MOVE
	}

	public enum PsychologicalState
	{
		IDLE,
		BROKEN,
		VIRTUOUS
	}

	public enum TurnState
	{
		WAITING,
		SELECTING,
		CONFIRMING,
		ACTING,
		SKIPPING,
		DEAD
	}

	public PhysicalState physicalState;
	public PsychologicalState psychologicalState;
	public TurnState turnState;


	private BoxCollider2D collider;
	private Rigidbody2D rb2d;
	private Animator animator;
	private Manager gameManager;
	private GameObject mainCamera;


	public bool isActive = false;
	public bool actedInTurn = false;
	// For ienumrator stuff
	private bool actionStarted = false;


	public GameObject thumbnailPrefab;

	// Use this for initialization
	protected virtual void Start () {
		collider = this.GetComponent<BoxCollider2D> ();
		rb2d = this.GetComponent<Rigidbody2D> ();
		animator = this.GetComponent<Animator>();
		mainCamera = GameObject.FindGameObjectWithTag ("MainCamera");
		gameManager = Manager.instance;

		GameObject thumb = Instantiate (thumbnailPrefab, new Vector3 (-50f, -50f, 0f), Quaternion.identity);

		physicalState = PhysicalState.FINE;
		psychologicalState = PsychologicalState.IDLE;
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		
		if (gameManager.UIshield)
			return;

		switch(gameManager.isBattle)
		{
		case false:
			if (Input.GetKey (KeyCode.RightArrow)) {
				animator.SetBool ("walk", true);
				animator.SetFloat ("direction", 1);
				this.transform.Translate (new Vector3 (velocity * Time.deltaTime, 0f, 0f));
				mainCamera.transform.Translate (new Vector3 (velocity * 0.25f * Time.deltaTime, 0f, 0f));
			} else if (Input.GetKey (KeyCode.LeftArrow)) {
				animator.SetBool ("walk", true);
				animator.SetFloat ("direction", -1);
				this.transform.Translate (new Vector3 (velocity * -0.5f * Time.deltaTime, 0f, 0f));
				mainCamera.transform.Translate (new Vector3 (velocity * -0.5f * 0.25f * Time.deltaTime, 0f, 0f));
			} else {
				animator.SetBool ("walk", false);
			}
			break;

		case true:
			animator.SetBool ("walk", false);

			switch(this.turnState)
			{
			case TurnState.WAITING:
				// do nothind. ideal state.
				break;
			case TurnState.SELECTING:
				// this unit's skill command should draw targets
				break;
			case TurnState.CONFIRMING:
				// confirm button should show on the screeen
				break;
			case TurnState.ACTING:
				// Do actions defined in Ienumrator

				StartCoroutine (TimeforAction ());

				break;
			case TurnState.SKIPPING:

				break;
			case TurnState.DEAD:

				break;
			}

			break;
		}

	}

	protected virtual void SetActive(bool selection){
		this.isActive = selection;
	}

	public void SetPosition(int posNum){
		this.currentPos = posNum;
	}

	private IEnumerator TimeforAction(){
		if(actionStarted){
			yield break;
		}

//		actionStarted = true;
//		Vector3 stagePosition = new Vector3 (0f,0f,0f);
//		while(MoveToStage){
//			yield return null;
//		}
		// Move to the action stage whith camera zooming

		// Wait a bit if necessary
//		yield return new WaitForSeconds(0.5f);
		// Do damage

		// Move back to start position

		// Reset its turnState

		actionStarted = false;
		actedInTurn = true;
		this.SetActive (false);
	}

//	private bool MoveToStage(Vector3 position){
//		return target != (this.transform.position = Vector3.MoveTowards (this.transform.position, position, animSpeed * Time.deltaTime));
//	}

//	public void MovePosition(BaseSurvivor survivor, int startPosNum, int endPosNum){
//		
//	}

}
