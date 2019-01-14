using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Commander : MonoBehaviour 
{

    public static Commander instance;
    public TurnStateMachine turnStateMachine;
    Camera mainCamera;
    public Narrator narrator;
    public TouchInput touchInput;

    public float leftBorder = 0f;
    public float rightBorder;

    public float initGameDelay = 2.5f;
    public float initBattleDelay = 0.1f;

    // Ref
    public GameObject goal;

    // Overall Gameplay Control
    bool m_hasLevelStarted = false;
    public bool HasLevelStarted { get { return m_hasLevelStarted; } set { m_hasLevelStarted = value; } }

    bool m_isGamePlaying = false;
    public bool IsGamePlaying { get { return m_isGamePlaying; } set { m_isGamePlaying = value; } }

    bool m_isGameOver = false;
    public bool IsGameOver { get { return m_isGameOver; } set { m_isGameOver = value; } }

    bool m_hasLevelFinished = false;
    public bool HasLevelFinished { get { return m_hasLevelFinished; } set { m_hasLevelFinished = value; } }

    // Battle Related
    bool m_isBattle = false;
    public bool IsBattle { get { return m_isBattle; } set { m_isBattle = value; } }

    // Animation time
    bool m_isActing = false;
    public bool IsActing { get { return m_isActing; } set { m_isActing = value; } }


    // Unity Events
    public UnityEvent setupEvent;
    public UnityEvent startLevelEvent;
    public UnityEvent playLevelEvent;
    public UnityEvent endLevelEvent;

    public UnityEvent battleEvent;
    public UnityEvent actionEvent;
    public UnityEvent battleOverEvent;
    public UnityEvent loseLevelEvent;
    public UnityEvent narrationEvent;

    private void Awake()
    {
        MakeSingleton();

        goal = GameObject.FindWithTag("Goal");
        rightBorder = goal.transform.position.x;
        turnStateMachine = GetComponent<TurnStateMachine>();
        mainCamera = Camera.main;
        narrator = GetComponent<Narrator>();
        touchInput = mainCamera.GetComponent<TouchInput>();
    }

    // Use this for initialization
    void Start () 
    {
        if(Commander.instance && PlayerManager.instance)
        {
            StartCoroutine("RunGameLoop");
        }
        else
        {
            Debug.LogWarning("No Commander or PlayerManager Found");
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    void MakeSingleton()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        } else {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    IEnumerator RunGameLoop()
    {
        yield return StartCoroutine("StartLevelRoutine");
        yield return StartCoroutine("PlayLevelRoutine");
        yield return StartCoroutine("EndLevelRoutine");
    }

    IEnumerator StartLevelRoutine()
    {
        Debug.Log("SETUP LEVEL");
        if(setupEvent != null)
        {
            setupEvent.Invoke();
        }

        Debug.Log("START LEVEL");

        // Disable controlling player
        EnableInput(false);

        while (!m_hasLevelStarted)
        {
            // Show start screen
            // User presses button to start
            // HasLevelStarted = true
            yield return null;
        }

        if (startLevelEvent != null)
        {
            startLevelEvent.Invoke();
        }
    }

    IEnumerator PlayLevelRoutine()
    {
        Debug.Log("PLAY LEVEL");
        m_isGamePlaying = true;
        yield return new WaitForSeconds(initGameDelay);

        // Input Enabled
        EnableInput(true);
        touchInput.InputEnabled = true;

        if (playLevelEvent != null)
        {
            // Fade off start screens
            playLevelEvent.Invoke();
        }

        while (!m_isGameOver)
        {
            yield return null;

            m_isGameOver = IsWinner(); // true if the player has reached the goal

        }

        Debug.Log("WIN! ===================");
    }

    // This will be implemented after player has reached the goal
    IEnumerator EndLevelRoutine()
    {
        Debug.Log("END LEVEL");

        EnableInput(false);
        mainCamera.GetComponent<TouchInput>().InputEnabled = false;

        if (endLevelEvent != null)
        {
            endLevelEvent.Invoke();
        }

        // Show end screen
        while (!m_hasLevelFinished)
        {
            // User presses button to continue

            // HasLevelFinished = true
            yield return null;
        }

        // Reload the current scene
        RestartLevel();
    }

    public void EnableInput(bool value)
    {
        PlayerManager.instance.playerInput.InputEnabled = value;
        Debug.Log("Input Enabled. You can move now");
    }

    public void InitBattle(Trigger t)
    {
        m_isBattle = true;
        EnableInput(false);
        touchInput.InputEnabled = false;
        UIManager.instance.BeginUIShield();

        EnemyManager.instance.Deploy(t);

        Destroy(t.gameObject);

        // Battle setup


        // Show battle screen

        StartCoroutine("BattleLevelRoutine");
    }

    IEnumerator BattleLevelRoutine()
    {
        Debug.Log("BATTLE LEVEL");

        if(!narrator.IsNarrating)
        {
            narrator.Narrate("Enemy Encountered...!");
        }

        if (battleEvent != null)
        {
            battleEvent.Invoke();
            // Initiate battle UI
        }

        while(narrator.IsNarrating)
        {
            yield return null;
        }

        yield return new WaitForSeconds(initBattleDelay);
        turnStateMachine.Initialize();

        while (IsBattle)
        {
            yield return StartCoroutine(turnStateMachine.StartTurnRoutine());
            yield return StartCoroutine(turnStateMachine.UpdateTurnRoutine());
            yield return StartCoroutine(turnStateMachine.EndTurnRoutine());

            yield return new WaitForSeconds(0.5f);

            // Win or Lose conditions can trigger IsBattle to false
            switch(turnStateMachine.currentTurn)
            {
                case TurnStateMachine.Turn.PLAYER:
                    IsBattle = AreEnemiesAllDead();
                    break;
                case TurnStateMachine.Turn.ENEMY:
                    IsBattle = AreEnemiesAllDead();
                    break;
            }
            Debug.Log("Enemy has " + EnemyManager.instance.characterList.Count + " units left");
            Debug.Log("Player has " + PlayerManager.instance.characterList.Count + " units left");
        }

        FinishBattle();
    }

   

    void FinishBattle()
    {
        // Give some rewards
        // Enable Input again
        Debug.Log("Battle Finished");
        foreach(var t in UIManager.instance.skillDisplays)
        {
            t.IsAvailable = true;
            t.btn.interactable = true;
        }
        EnableInput(true);
    }

    // Check if player has won the battle
    bool AreEnemiesAllDead()
    {
        // Check with list number
        if (EnemyManager.instance.characterList.Count == 0)
        {
            return false; // It should be false here in order to assign this value to boolean <IsBattle>
        }
        return true;
    }

    bool AreCharactersAllDead()
    {
        // Check with list number
        if (PlayerManager.instance.characterList.Count == 0)
        {
            return false; // It should be false here in order to assign this value to boolean <IsBattle>
        }
        return true;
    }

    // Check if player has reached the goal
    bool IsWinner()
    {
        if(PlayerManager.instance.isReached)
        {
            return true;
        }

        return false;
    }

    public void PlayLevel()
    {
        m_hasLevelStarted = true;
    }

    // Restart te current level
    void RestartLevel()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    void DeployEnemy(Trigger battleTrigger)
    {

    }
}
