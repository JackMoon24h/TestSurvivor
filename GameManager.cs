using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : MonoBehaviour 
{

    public SquadManager squadManager;
    public EnemySquadManager enemySquadManager;
    public ObjectTrigger goal;
    Camera mainCamera;

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


    public List<MainPanel> mainPanels = new List<MainPanel>();

    Vector3 enemyPositionOffset = new Vector3(0f, 0.25f, 0f);

    public UnityEvent startLevelEvent;
    public UnityEvent playLevelEvent;
    public UnityEvent battleEvent;
    public UnityEvent battleOverEvent;
    public UnityEvent endLevelEvent;


    public enum TurnState
    {
        Player,
        Enemy
    }
    public TurnState turnState = TurnState.Player;

    private void Awake()
    {
        squadManager = Object.FindObjectOfType<SquadManager>().GetComponent<SquadManager>();
        goal = GameObject.FindWithTag("Goal").GetComponent<ObjectTrigger>();
        mainPanels = (Object.FindObjectsOfType<MainPanel>() as MainPanel[]).ToList();
        mainCamera = Camera.main;

        for (int i = 0; i < 3; i++)
        {
            mainPanels[i].gameObject.SetActive(false);
        }

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
        Debug.Log("Battle Routine Started!");

        squadManager.squadInput.InputEnabled = false;

        if(battleEvent != null)
        {
            // Battle Animations and start turn
            battleEvent.Invoke();
        }

        // Show Battle Animation for seconds
        yield return new WaitForSeconds(2f);

        squadManager.squadInput.InputEnabled = true;

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
            return goal.isActive;
        }
        Debug.LogWarning("Warning : Goal has not been set! ====================");
        return false;
    }

    // This will be called by triggers on the field
    public void InitBattle(GameObject enemySquadPrefab)
    {
        Debug.Log("BATTLE LEVEL");
        m_isBattle = true;
        squadManager.squadMover.Stop();

        var enemySquad = Instantiate(enemySquadPrefab, squadManager.gameObject.transform.position + enemyPositionOffset, Quaternion.identity);
        enemySquadManager = enemySquad.GetComponent<EnemySquadManager>();

        StartCoroutine("BattleLevelRoutine");
    }

    // This will be called by SquadInput
    public void GetClickedObject(Collider2D col)
    {
        Debug.Log("Clicked " + col.gameObject.name);

        switch (col.gameObject.tag)
        {
            case "Survivor":

                // if it is not in the battle
                SetActivePanel(col.gameObject);

                // Change active unit and Update status window

                // if it is in the middle of battle, check if the clicked unit is active unit



                break;

            case "Enemy":
                
                break;

            case "Skill":
                // If skill has button component, maybe this is not necessary
                break;

            case "Object":
                
                // Get active unit

                // Start coroutine to pick up the object

                break;
        }
    }

    void SetActivePanel(GameObject target)
    {
        foreach(var t in mainPanels)
        {
            if(t.character.gameObject == target)
            {
                t.gameObject.SetActive(true);
            }
            else
            {
                t.gameObject.SetActive(false);
            }
        }
    }

    // For Test Purpose
    public void TestButton()
    {
        StartCoroutine(TestRoutine());
    }

    IEnumerator TestRoutine()
    {
        yield return new WaitForSeconds(2f);
        m_isBattle = false;
    }

}
