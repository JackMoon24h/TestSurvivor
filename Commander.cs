using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Commander : MonoBehaviour 
{

    public static Commander instance;
    public TurnControlMachine turnControlMachine;

    public float leftBorder = 0f;
    public float rightBorder;

    public float initGameDelay = 2.5f;

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

    bool m_isActing = false;
    public bool IsActing { get { return m_isBattle; } set { m_isBattle = value; } }

 

    // Unity Events
    public UnityEvent setupEvent;
    public UnityEvent startLevelEvent;
    public UnityEvent playLevelEvent;
    public UnityEvent endLevelEvent;

    public UnityEvent battleEvent;
    public UnityEvent battleOverEvent;
    public UnityEvent loseLevelEvent;
    public UnityEvent narrationEvent;

    private void Awake()
    {
        MakeSingleton();

        goal = GameObject.FindWithTag("Goal");
        rightBorder = goal.transform.position.x;
        turnControlMachine = GetComponent<TurnControlMachine>();
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
            Debug.LogWarning("No Commander or PlayerManager found");
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
        PlayerManager.instance.playerInput.InputEnabled = false;

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
        PlayerManager.instance.playerInput.InputEnabled = true;

        if (playLevelEvent != null)
        {
            // Fade off start screens
            playLevelEvent.Invoke();
        }

        while (!m_isGameOver)
        {
            yield return null;

            m_isGameOver = IsWinner();

        }

        Debug.Log("WIN! ===================");
    }

    IEnumerator EndLevelRoutine()
    {
        Debug.Log("END LEVEL");

        PlayerManager.instance.playerInput.InputEnabled = false;

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

    public void InitBattle()
    {
        m_isBattle = true;

        // Battle setup
        // Show battle screen

        turnControlMachine.turnCount = 1;

        StartCoroutine("BattleLevelRoutine");
    }

    IEnumerable BattleLevelRoutine()
    {
        Debug.Log("BATTLE LEVEL");

        while(instance.IsBattle)
        {
            yield return null;

            // Win or Lose conditions can trigger IsBattle to false
        }
    }

    // Check if player has won the battle
    //bool IsSurvived()
    //{
    //    // Check if all enemies are dead when every turn ends
    //    if()
    //    {
    //        return true;
    //    }
    //    return false;
    //}

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
}
