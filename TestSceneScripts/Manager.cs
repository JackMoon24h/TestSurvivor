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
	// StatusWindow statusWindow means this variable will get a reference of "StatusWindow Script" to access StatusWindow Script and use functions

	public GameObject cursorPrefab;
	public GameObject targetCursorPrefab;
	[HideInInspector]public GameObject cursor;
	
	[HideInInspector]public Vector3 thumbPos = new Vector3 (-15f, -7.7f, 0f);
	[HideInInspector]public Vector3 thumbHidePos = new Vector3 (-50f, -50f, 0f);
	[HideInInspector]public GameObject confirmBtn;

	public float levelStartDelay = 2f;
	public float turnDelay = 0.3f;

	public GameObject[] allBattleUnitList;

	// For Initialization, it must be set before the game begins
	public BaseSurvivor[] initialSurvivorList = new BaseSurvivor[4];

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

	public Vector3 survivorStagePos = new Vector3 (-2f, -1.5f, 10f);

	public BaseSurvivor selectedSurvivor;
	public BaseSurvivor activeSurvivor;
	public BaseEnemy activeEnemy;

	private int level = 1;
	private GameObject levelImage;
	private Text levelText;
	private Text turnCounter;
	public List<Button> activeCommands = new List<Button>();
	public Skills activeSkill;
	// Skills.DrawTarget will use it
	public List<GameObject> targetCursors = new List<GameObject>();
	public List<BaseSurvivor> survivorTargetList = new List<BaseSurvivor>();
	public List<BaseEnemy> enemyTargetList = new List<BaseEnemy>();



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
		//statusWindow = GameObject.Find ("StatusWindow").GetComponent<StatusWindow> ();
		cursor = Instantiate (cursorPrefab, new Vector3(-50f, -50f, 0f), Quaternion.identity) as GameObject;
		confirmBtn = GameObject.Find ("ConfirmBtn");
		confirmBtn.SetActive (false);

		this.DeploySurvivors (initialSurvivorList);
		Invoke ("HideLevelImage", levelStartDelay);
	}

	void HideLevelImage(){
		levelImage.SetActive (false);

		this.EndUIShield ();
		//this.statusWindow.UpdateWindow (survivorList [0]);
		this.MoveCursor (survivorList [0]);
		this.SetSkills (survivorList [0]);
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

	public void MoveCursor(BaseSurvivor selected){

		if (this.selectedSurvivor){
			this.selectedSurvivor.thumb.transform.position = this.thumbHidePos + new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, 0f);
			this.selectedSurvivor = null;
		}
			
		this.selectedSurvivor = selected;
		var pos = selected.GetPosition (selected);

		if(this.selectedSurvivor == this.activeSurvivor)
		{
			cursor.transform.position = survivorStartPositions [pos - 1] + new Vector3(-0.8f, 3.2f, 0f) + new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, 0f);
		}
		selected.thumb.transform.position = this.thumbPos + new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, 0f);
	}

	public void SetSkills(BaseSurvivor selected){
		for(int i = 0; i < activeCommands.Count; i++){
			activeCommands [i].gameObject.SetActive (false);
		}
		activeCommands.Clear ();


		activeCommands.AddRange (selected.skillList);
		for(int i = 0; i < activeCommands.Count; i++){
			activeCommands [i].gameObject.SetActive (true);
		}
	}
		
	void GetUnitList(){
		GameObject[] surTemp = GameObject.FindGameObjectsWithTag ("Survivor");
		GameObject[] eneTemp = GameObject.FindGameObjectsWithTag ("Enemy");
		allBattleUnitList = surTemp.Concat (eneTemp).ToArray ();
	}

	void DecideTurn(){
		// For test playing, set Random.Range(1,2) in order to create int 1 only
//		int rand = Random.Range (1, 3);
		int rand = Random.Range (1, 2);
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

	public void PerformAction(){
		Debug.Log ("Manager gave an order to units to play");
	}


	// StartTurn
	void StartTurn(BaseSurvivor activeSurvivor){
		this.EndUIShield ();

		activeSurvivor.turnState = BaseSurvivor.TurnState.SELECTING;
		this.MoveCursor (this.activeSurvivor);
		this.SetSkills (this.activeSurvivor);
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
