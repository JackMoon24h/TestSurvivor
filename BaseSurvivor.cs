using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public abstract class BaseSurvivor : MonoBehaviour {
	
	public GameObject thumbnailPrefab;
	[HideInInspector]public GameObject thumb;

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


	[HideInInspector]public BoxCollider2D collider;
	private Rigidbody2D rb2d;
	private Animator animator;
	private Manager gameManager;
	private GameObject mainCamera;

	public bool isActive = false;
	public bool wasActivePrev = false;
	public bool actedInTurn = false;

	public Button[] skillPrefabs = new Button[5];
	public Button[] skillList = new Button[5];

	// On Stage
	public Vector3 scaleOrigin = new Vector3(1f, 1f, 1f);
	public Vector3 scaleOnStage = new Vector3(1.2f, 1.2f, 1f);

	// Use this for initialization
	protected virtual void Start () {
		collider = this.GetComponent<BoxCollider2D> ();
		rb2d = this.GetComponent<Rigidbody2D> ();
		animator = this.GetComponent<Animator>();
		mainCamera = GameObject.FindGameObjectWithTag ("MainCamera");
		gameManager = Manager.instance;
		scaleOnStage = this.transform.localScale;

		this.thumb = Instantiate (thumbnailPrefab, gameManager.thumbHidePos, Quaternion.identity);
		thumb.transform.SetParent (this.gameObject.transform);
		gameManager.cursor.transform.SetParent (this.gameObject.transform);

		physicalState = PhysicalState.FINE;
		psychologicalState = PsychologicalState.IDLE;

		// Create skills
		for(int i = 0; i < 5; i++){
			var temp = Instantiate (skillPrefabs [i]);
			temp.transform.SetParent (GameObject.Find ("Skill" + (i + 1)).transform);
			//Set local Position to the parent
			temp.transform.localPosition = new Vector3 (0f, 0f, 0f);
			temp.gameObject.SetActive (false);
			skillList.SetValue (temp, i);
			temp.GetComponent<Skills> ().SetCaster (this);
		}
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

	public int GetPosition(BaseSurvivor survivor){
		return survivor.currentPos;
	}

	public void OnClick(){
		gameManager.statusWindow.UpdateWindow (this);
		gameManager.MoveCursor (this);
		gameManager.SetSkills (this);
	}

	protected virtual void SetActive(bool selection){
		this.isActive = selection;
	}

	public void SetPosition(int posNum){
		this.currentPos = posNum;
	}

	private IEnumerator TimeforAction(){

		while(MoveToStage()){
			yield return null;
		};

//		Do damage
		animator.SetTrigger ("skill_shoot");

		yield return new WaitForSeconds(1f);

//		Move back to start position
		while(MoveBack()){
			yield return null;
		}

		animator.ResetTrigger ("skill_shoot");

//		Do Damage

		yield return new WaitForSeconds(0.3f);

//		Reset its turnState
		this.turnState = BaseSurvivor.TurnState.WAITING;
	}

	private bool MoveToStage(){
		var targetPos = this.gameManager.survivorStagePos + mainCamera.transform.position;
		return targetPos != (this.transform.position = Vector3.MoveTowards (this.transform.position, targetPos, 0.75f * Time.deltaTime));
	}

	private bool MoveBack(){
		var targetPos = this.gameManager.survivorStartPositions [this.currentPos - 1] + mainCamera.transform.position;
		return targetPos != (this.transform.position = Vector3.MoveTowards (this.transform.position, targetPos, 0.25f * Time.deltaTime));
	}

}
