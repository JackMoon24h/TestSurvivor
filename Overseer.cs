using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.UI;

public class Overseer : MonoBehaviour 
{

    // Level Management related parameters
    bool m_hasLevelStarted = false;
    public bool HasLevelStarted { get { return m_hasLevelStarted; } set { m_hasLevelStarted = value; } }

    bool m_isGamePlaying = false;
    public bool IsGamePlaying { get { return m_isGamePlaying; } set { m_isGamePlaying = value; } }

    bool m_isGameOver = false;
    public bool IsGameOver { get { return m_isGameOver; } set { m_isGameOver = value; } }

    bool m_isBattle = false;
    public bool IsBattle { get { return m_isBattle; } set { m_isBattle = value; } }

    bool m_hasLevelFinished = false;
    public bool HasLevelFinished { get { return m_hasLevelFinished; } set { m_hasLevelFinished = value; } }

    bool m_isTurnComplete = false;
    public bool IsTurnComplete { get { return m_isTurnComplete; } set { m_isTurnComplete = value; } }

    bool m_isActing = false;
    public bool IsActing { get { return m_isActing; } set { m_isActing = value; } }

    public enum TurnState
    {
        Player,
        Enemy
    }
    public TurnState turnState = TurnState.Player;

    public enum TurnStep
    {
        WaitForCommand,
        ChooseCommand,
        ConfirmCommand,
        Act,
        FinishTurn
    }
    public TurnStep turnStep = TurnStep.WaitForCommand;

    public int turnCount = 0;
    public TurnState previousTurnState;


    // Game loop related Events
    public UnityEvent startLevelEvent;
    public UnityEvent playLevelEvent;
    public UnityEvent endLevelEvent;

    // Battle loop related Events
    public UnityEvent battleEvent;
    public UnityEvent battleOverEvent;
    public UnityEvent loseLevelEvent;

    // Other Events
    public UnityEvent narrationEvent;

    // Refrences
    Narration narration;
    Deck playerDeck;



	// Use this for initialization
	void Awake () 
    {
        narration = Object.FindObjectOfType<Narration>().GetComponent<Narration>();
        playerDeck = Object.FindObjectOfType<Deck>().GetComponent<Deck>();
	}

    void Start()
    {
        StartCoroutine("RunGameLoop");
    }

    // Update is called once per frame
    void Update () 
    {
        
	}

    IEnumerator RunGameLoop()
    {
        yield return StartCoroutine("StartLevelRoutine");
        yield return StartCoroutine("PlayLevelRoutine");
        yield return StartCoroutine("EndLevelRoutine");
    }

    // Start the game
    IEnumerator StartLevelRoutine()
    {
        Debug.Log("START LEVEL");

        // Input disabled
        playerDeck.inputManager.InputEnabled = false;

        while (!m_hasLevelStarted)
        {
            yield return null;
        }

        if (startLevelEvent != null)
        {
            startLevelEvent.Invoke();
        }
    }

    // This will be called when user clicks the start button
    public void PlayLevel()
    {
        m_hasLevelStarted = true;
    }

    // Play the game
    IEnumerator PlayLevelRoutine()
    {
        Debug.Log("PLAY LEVEL");
        narration.Narrate("Play Level");

        m_isGamePlaying = true;
        yield return new WaitForSeconds(1f);

        playerDeck.inputManager.InputEnabled = true;

        if (playLevelEvent != null)
        {
            // Fade off start screens
            playLevelEvent.Invoke();
        }

        while (!m_isGameOver)
        {
            yield return null;

            //m_isGameOver = IsWinner();

        }

        Debug.Log("WIN! ===================");
    }


    // End the game
    IEnumerator EndLevelRoutine()
    {
        Debug.Log("END LEVEL");

        playerDeck.inputManager.InputEnabled = false;

        narration.Narrate("Survived 1 more night...");

        // Input disabled

        if (endLevelEvent != null)
        {
            endLevelEvent.Invoke();
        }

        // When user click the return button, then it will trigger m_hasLevelFinished to true
        while (!m_hasLevelFinished)
        {
            yield return null;
        }

        RestartLevel();
    }

    void RestartLevel()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void InitBattle()
    {
        Debug.Log("BATTLE LEVEL");
        narration.Narrate("Battle Started");
        m_isBattle = true;
        turnCount = 0;

        StartCoroutine("BattleLevelRoutine");
    }

    IEnumerator BattleLevelRoutine()
    {
        Debug.Log("Battle Routine Started!");

        if (battleEvent != null)
        {
            // Battle Animations and start turn
            battleEvent.Invoke();
        }

        // Show Battle Animation for seconds
        yield return new WaitForSeconds(2f);

        StartTurn();

        while (m_isBattle)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.3f);

        if (battleOverEvent != null)
        {
            battleOverEvent.Invoke();
        }

        Debug.Log("BATTLE IS OVER");
    }

    public void StartTurn()
    {
        // For debugging purpose, set random.range(0, 1). It should be set to random.range(-1, 1)
        var min = 0;
        var max = 1;
        var temp = Random.Range(min, max);

        m_isTurnComplete = false;

        if (temp >= 0)
        {
            this.turnState = Overseer.TurnState.Player;
            turnCount++;
            StartCoroutine("PlayerTurnRoutine");
        }
        else
        {
            this.turnState = Overseer.TurnState.Enemy;
            turnCount++;
            StartCoroutine("EnemyTurnRoutine");
        }
    }


    IEnumerator PlayerTurnRoutine()
    {
        Debug.Log("Player Turn Started");

        yield return new WaitForSeconds(0.5f);

        while (turnStep == TurnStep.WaitForCommand)
        {
            yield return new WaitForSeconds(1.5f);

            turnStep = TurnStep.ChooseCommand;
        }

        while (turnStep == TurnStep.ChooseCommand)
        {
            // If Skill Button is clicked, it will call SkillTarget.DrawTargets() and move to the next TurnStep

            yield return null;
        }

        while (turnStep == TurnStep.ConfirmCommand)
        {

            yield return null;
        }

        while (turnStep == TurnStep.Act)
        {
            // If comandBtn is clicked, it will call ComfirmCommand() in order to change the TurnStep to Act.

            narration.Narrate("Striked...!");



            while (m_isActing)
            {
                yield return null;
            }

            // Set Anim Time and then change turn step
            yield return new WaitForSeconds(0.5f);

            turnStep = TurnStep.FinishTurn;
        }

        while (turnStep == TurnStep.FinishTurn)
        {
            // Calculation, unit saying, status change, etc

            // If second Anim is over, change the turnstep
            yield return new WaitForSeconds(1f);

            turnStep = TurnStep.WaitForCommand;
        }

        yield return new WaitForSeconds(1f);

        m_isTurnComplete = true;

        // Check if Enemies are all dead after turn is complete
        UpdateTurn();
    }

    public void UpdateTurn()
    {
        Debug.Log("Turn Updated");
        StartTurn();
    }


    // For debugging purposes
    public void EndLevel()
    {
        m_isGameOver = true;
    }

    public void Reload()
    {
        m_hasLevelFinished = true;
    }
}
