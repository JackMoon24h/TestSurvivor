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
    public Camera mainCamera;
    public Narrator narrator;
    public TouchInput touchInput;

    public float leftBorder = 0f;
    public float rightBorder;

    public float initGameDelay = 2.5f;
    public float initBattleDelay = 0.1f;

    // Ref
    public GameObject goal;
    Button restartBtn;

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

    bool m_isMentalActing = false;
    public bool IsMentalActing { get { return m_isMentalActing; } set { m_isMentalActing = value; } }

    bool m_isSpeaking = false;
    public bool IsSpeaking { get { return m_isSpeaking; } set { m_isSpeaking = value; } }

    float m_actOutChance = 0.99f;
    public float ActOutChance { get { return m_actOutChance; } set { m_actOutChance = value; } }

    public List<Actor> actorList = new List<Actor>();
    public List<Trigger> triggers = new List<Trigger>();


    // Set in the inspector
    public List<GameObject> physicalEffectPrefabs;
    public List<GameObject> afflictionPrefabs;
    public List<GameObject> virtuePrefabs;
    public List<GameObject> sufferBackGrounds;
    public List<GameObject> bleedEffects;

    public Vector3 baseBleedOffset = new Vector3(-5f, 2f, 10f);

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
        Initialize();
    }

    // Use this for initialization
    void Start () 
    {
        // SceneManager.sceneLoaded will be called between Awake() and Start()
        SceneManager.sceneLoaded += OnSceneLoaded;

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

    void Initialize()
    {
        goal = GameObject.FindWithTag("Goal");
        rightBorder = goal.transform.position.x;
        turnStateMachine = GetComponent<TurnStateMachine>();
        mainCamera = Camera.main;
        narrator = GetComponent<Narrator>();
        touchInput = mainCamera.GetComponent<TouchInput>();
        triggers.AddRange(Object.FindObjectsOfType<Trigger>());
        foreach(var t in triggers)
        {
            t.gameObject.SetActive(true);
        }
    }

    // This will be called when scene is loaded (except first scene)
    void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        var actors = Object.FindObjectsOfType<Actor>();
        foreach(var t in actors)
        {
            Destroy(t.gameObject);
        }

        UIManager.instance.mainPanel.SetActive(true);
        UIManager.instance.commandPanel.SetActive(true);

        Initialize();
        Commander.instance.setupEvent.Invoke();
        PlayerManager.instance.Initialize();

        PlayerManager.instance.transform.position = Vector3.zero;
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

            m_isGameOver = IsReachedGoal();
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

        PlayerManager.instance.ClearActiveCharacter();
        EnemyManager.instance.Deploy(t);

        t.gameObject.SetActive(false);

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
        while (m_isBattle)
        {
            yield return StartCoroutine(turnStateMachine.RoundRoutine());
        }

        yield return new WaitForSeconds(0.5f);

        // Finish Battle will be called when player won the battle
        while(PlayerManager.instance.allDead)
        {
            yield return null;
        }
        FinishBattle();

        yield return new WaitForSeconds(2f);
        if(PlayerManager.instance.activeCharacter)
        {
            UIManager.instance.rewardWindow.GetComponent<RewardWindow>().OpenWindow();
        }
    }

    public void LoseLevel()
    {
        m_isGameOver = true;
        StartCoroutine(LoseLevelRoutine());
    }

    IEnumerator LoseLevelRoutine()
    {
        yield return new WaitForSeconds(1f);
        UIManager.instance.EndUIShield();
        m_isGamePlaying = false;

        if (loseLevelEvent != null)
        {
            loseLevelEvent.Invoke();
        }

        yield return new WaitForSeconds(2f);
        Debug.Log("Game Over !===================");

        //RestartLevel();
    }

    void FinishBattle()
    {
        // Give some rewards
        narrator.Narrate("Survived...");
        // Enable Input again
        Debug.Log("Battle Finished");
        foreach(var t in UIManager.instance.skillDisplays)
        {
            t.IsAvailable = true;
            t.btn.interactable = true;
        }

        PlayerManager.instance.SetActiveCharacterAtPos(1);
    }

    // Check if player has won the battle
    public bool AreEnemiesAllDead()
    {
        // Check with list number
        if (EnemyManager.instance.characterList.Count == 0)
        {
            Debug.Log("All Enemies are dead");
            return true;
        }
        return false;
    }

    public bool AreCharactersAllDead()
    {
        // Check with list number
        if (PlayerManager.instance.characterList.Count == 0)
        {
            Debug.Log("All Characters are dead");
            return true;
        }
        return false;
    }

    // Check if player has reached the goal
    bool IsReachedGoal()
    {
        return PlayerManager.instance.isReached;
    }

    public void PlayLevel()
    {
        m_hasLevelStarted = true;
        UIManager.instance.UpdateUIPanel(PlayerManager.instance.activeCharacter);
    }

    // Restart te current level
    public void RestartLevel()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void GetCurrentActors()
    {
        actorList.Clear();
        var actors = Object.FindObjectsOfType<Actor>();
        actorList.AddRange(actors);
    }
}