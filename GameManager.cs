using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour 
{

    public SquadManager squadManager;
    public ObjectTrigger goal;

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

    public float delay = 2.5f;

    public UnityEvent startLevelEvent;
    public UnityEvent playLevelEvent;
    public UnityEvent battleEvent;
    public UnityEvent battleOverEvent;
    public UnityEvent endLevelEvent;

    private void Awake()
    {
        squadManager = Object.FindObjectOfType<SquadManager>().GetComponent<SquadManager>();
        goal = GameObject.FindWithTag("Goal").GetComponent<ObjectTrigger>();
    }

    // Use this for initialization
    void Start () 
    {
        if(squadManager != null)
        {
            StartCoroutine("RunGameLoop");
        }
        else
        {
            Debug.LogWarning("No Squad found!");
        }
	}

    IEnumerator RunGameLoop()
    {
        yield return StartCoroutine("StartLevelRoutine");
        yield return StartCoroutine("PlayLevelRoutine");
        yield return StartCoroutine("EndLevelRoutine");
    }

    // By Clicking the start button, StartLevelRoutine will be over
    IEnumerator StartLevelRoutine()
    {
        Debug.Log("START LEVEL");
        squadManager.squadInput.InputEnabled = false;

        while(!m_hasLevelStarted)
        {
            yield return null;
        }

        if(startLevelEvent != null)
        {
            startLevelEvent.Invoke();
        }
    }

    IEnumerator PlayLevelRoutine()
    {
        Debug.Log("PLAY LEVEL");
        m_isGamePlaying = true;
        yield return new WaitForSeconds(delay);
        squadManager.squadInput.InputEnabled = true;

        if(playLevelEvent != null)
        {
            // Fade off start screens
            playLevelEvent.Invoke(); 
        }

        while(!m_isGameOver)
        {
            yield return null;

            m_isGameOver = IsWinner();

        }

        Debug.Log("WIN! ===================");
    }

    IEnumerator BattleLevelRoutine()
    {
        Debug.Log("Battle Routine Worked!");

        if(battleEvent != null)
        {
            // Battle Animations and start turn
            battleEvent.Invoke();
        }

        yield return new WaitForSeconds(delay);

        // Until something sets m_isBattle true, wait.
        while(m_isBattle)
        {
            yield return null;

            // Something should set m_isBattle to true here
        }

        yield return new WaitForSeconds(delay);

        if(battleOverEvent != null)
        {
            battleOverEvent.Invoke();
        }

        squadManager.squadInput.InputEnabled = true;
        Debug.Log("BATTLE IS OVER");
    }


    IEnumerator EndLevelRoutine()
    {
        Debug.Log("END LEVEL");

        squadManager.squadInput.InputEnabled = false;

        if(endLevelEvent != null)
        {
            endLevelEvent.Invoke();
        }

        while(!m_hasLevelFinished)
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

    // attached to start button in order to start the level
    public void PlayLevel()
    {
        m_hasLevelStarted = true;
    }

    // Check if player has reached the goal
    bool IsWinner()
    {
        if(goal != null)
        {
            return goal.isClicked;
        }
        Debug.LogWarning("Warning : Goal has not been set! ====================");
        return false;
    }

    // This will be called by triggers on the field
    public void InitBattle()
    {
        Debug.Log("BATTLE LEVEL");
        m_isBattle = true;
        squadManager.squadMover.Stop();
        squadManager.squadInput.InputEnabled = false;

        StartCoroutine("BattleLevelRoutine");
    }

    // For Test Purpose
    public void TestButton()
    {
        m_isBattle = false;
    }

}
