using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class Manager : MonoBehaviour {

	[System.NonSerialized]public bool isBattle = false;
	[System.NonSerialized]public bool UIshield = true;

	public static Manager instance = null;
	public GameObject mainCamera;

	public float levelStartDelay = 2f;
	public float turnDelay = 0.3f;

	public GameObject[] allBattleUnitList;

	// For Initialization, it must be set before the game begins
	public BaseSurvivor[] initialSurvivorList = new BaseSurvivor[4];

//	public string survivorInPos1;
//	public string survivorInPos2;
//	public string survivorInPos3;
//	public string survivorInPos4;

//	public string enemyInPos1;
//	public string enemyInPos2;
//	public string enemyInPos3;
//	public string enemyInPos4;

	public enum TurnOrder
	{
		SURVIVOR,
		ENEMY
	}

	public TurnOrder currentTurn;

	public List<TurnHandler> performList = new List<TurnHandler> ();
	public List<BaseSurvivor> survivorList = new List<BaseSurvivor>();
	public List<BaseEnemy> enemyList = new List<BaseEnemy>();

	private static Vector3 survivorStartPos1 = new Vector3 (-3.5f, -1f, 10f);
	private static Vector3 survivorStartPos2 = new Vector3 (-7.0f, -1f, 10f);
	private static Vector3 survivorStartPos3 = new Vector3 (-10.5f, -1f, 10f);
	private static Vector3 survivorStartPos4 = new Vector3 (-14.0f, -1f, 10f);
	[System.NonSerialized]public Vector3[] survivorStartPositions = {
		survivorStartPos1,
		survivorStartPos2,
		survivorStartPos3,
		survivorStartPos4
	};

	private static Vector3 enemyStartPos1 = new Vector3 (3.5f, -0.75f, 10f);
	private static Vector3 enemyStartPos2 = new Vector3 (7.0f, -0.75f, 10f);
	private static Vector3 enemyStartPos3 = new Vector3 (10.5f, -0.75f, 10f);
	private static Vector3 enemyStartPos4 = new Vector3 (14.0f, -0.75f, 10f);
	[System.NonSerialized]public Vector3[] enemyStartPositions = { 
		enemyStartPos1,
		enemyStartPos2,
		enemyStartPos3,
		enemyStartPos4
	};

	public BaseSurvivor activeSurvivor;
	public BaseEnemy activeEnemy;

	private int level = 1;
	private GameObject levelImage;
	private Text levelText;
	private Text turnCounter;

	// Methods


	public void BeginUIShield(){
		this.UIshield = true;
	}

	public void EndUIShield(){
		this.UIshield = false;
	}

	public BaseSurvivor GetSurvivorInPos(int pos){
		return this.survivorList [pos - 1];
	}

	public BaseEnemy GetEnemyInPos(int pos){
		return this.enemyList [pos - 1];
	}

	// Use this for initialization
	void Awake () {
		if(instance == null){
			instance = this;
		} else if (instance != this){
			Destroy (gameObject);
		}

		DontDestroyOnLoad (gameObject);

		InitGame ();
	}


	void InitGame(){
		this.BeginUIShield ();

		mainCamera = GameObject.FindGameObjectWithTag ("MainCamera");
		levelImage = GameObject.Find ("LevelImage");
		levelText = GameObject.Find ("LevelText").GetComponent<Text> ();
		levelText.text = "Day " + level;
		levelImage.SetActive (true);

		this.DeploySurvivors (initialSurvivorList);

		Invoke ("HideLevelImage", levelStartDelay);
	}

	void HideLevelImage(){
		levelImage.SetActive (false);

		this.EndUIShield ();
	}


	public void InitBattle(BaseEnemy[] initialEnemyList){
		this.isBattle = true;

		this.DeployEnemies (initialEnemyList);
		this.GetUnitList ();
		this.DecideTurn ();

		if(activeEnemy){
			this.SimulatePlay (activeEnemy);
		} else if(activeSurvivor){
			this.StartTurn (activeSurvivor);
		} else {
			Debug.Log ("Something is wrong with DecideTurn(). ActiveUnit is not decided.");
		}
	}

	void DeploySurvivors(BaseSurvivor[] initialSurvivorList){
		for(int i = 0; i < initialSurvivorList.Length; i++){
			BaseSurvivor survivor = Instantiate (initialSurvivorList [i], survivorStartPositions [i] + mainCamera.transform.position, Quaternion.identity);
			survivorList.Insert (i, survivor);
			survivor.SetPosition (i + 1);
		}
	}

	// Creating Enemies in the battle
	void DeployEnemies(BaseEnemy[] initialEnemyList){
		for(int i = 0; i < initialEnemyList.Length; i++){
			BaseEnemy enemy = Instantiate (initialEnemyList [i], enemyStartPositions [i] + mainCamera.transform.position, Quaternion.identity);
			enemyList.Insert (i, enemy);
			enemy.SetPosition (i + 1);
		}
	}
		
	void GetUnitList(){
		GameObject[] surTemp = GameObject.FindGameObjectsWithTag ("Survivor");
		GameObject[] eneTemp = GameObject.FindGameObjectsWithTag ("Enemy");
		allBattleUnitList = surTemp.Concat (eneTemp).ToArray ();
	}

	void DecideTurn(){
		int rand = Random.Range (1, 3);
		if(rand == 1){
			this.currentTurn = TurnOrder.SURVIVOR;
			activeEnemy = null;
			activeSurvivor = this.GetActiveSurvivor ();
		} else {
			this.currentTurn = TurnOrder.ENEMY;
			activeSurvivor = null;
			activeEnemy = this.GetActiveEnemy ();
		}
	}

	private BaseSurvivor GetActiveSurvivor(){
		int rand = Random.Range (1, survivorList.Count + 1);
		return this.GetSurvivorInPos (rand);
	}

	private BaseEnemy GetActiveEnemy(){
		int rand = Random.Range (1, enemyList.Count + 1);
		return this.GetEnemyInPos (rand);
	}


	// StartTurn
	void StartTurn(BaseSurvivor activeSurvivor){
		this.EndUIShield ();

		activeSurvivor.turnState = BaseSurvivor.TurnState.SELECTING;
//		GameObject temp = allBattleUnitList [0];
//		var tempScript = temp.GetComponent<BaseSurvivor> ();
//		tempScript. ~~ I CAN ACCESS SCRIPT THROUGH GAMEOBJECT variables.......


		Debug.Log ("Player's Turn");
	}

	// EnemyStartTurn
	void SimulatePlay(BaseEnemy activeEnemy){

		Debug.Log ("Enemy's Turn");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void getWinner(){
		levelText.text = "After " + level + " days, you failed to survive.";
		levelImage.SetActive (true);
		enabled = false;
	}
}
