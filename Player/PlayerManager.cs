using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerMover))]
public class PlayerManager : MonoBehaviour 
{
    public static PlayerManager instance;
    public PlayerInput playerInput;
    public PlayerMover playerMover;
    public BoxCollider2D finder;
    public MovePosition swapBtn;

    public static float spacing = -3.5f;
    public static readonly Vector2[] positions =
    {
        new Vector2(1 * spacing, 0f),
        new Vector2(2 * spacing, 0f),
        new Vector2(3 * spacing, 0f),
        new Vector2(4 * spacing, 0f),
    };


    public bool isMovingForward = false;
    public bool isMovingBackWard = false;
    public bool isReached = false;
    public float swapSpeed = 0.6f;


    public GameObject[] characterPrefabs = new GameObject[4];
    public List<BaseCharacter> characterList = new List<BaseCharacter>();
    public BaseCharacter activeCharacter;
    public GameObject clickedObject;

    // Skill targets storage
    public List<BaseEnemy> unfriendlyTargets = new List<BaseEnemy>();
    public List<BaseCharacter> friendlyTargets = new List<BaseCharacter>();

    private void Awake()
    {
        MakeSingleton();
        playerMover = GetComponent<PlayerMover>();
        playerInput = GetComponent<PlayerInput>();
        finder = GetComponent<BoxCollider2D>();
        swapBtn = Object.FindObjectOfType<MovePosition>().GetComponent<MovePosition>();

    }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        // If character List is empty, then Instantiate default character setting
        if(characterList.Count == 0)
        {
            for(int i = 0; i < characterPrefabs.Length; i++)
            {
                if(characterPrefabs[i] != null)
                {
                    var survivor = Instantiate(characterPrefabs[i], Vector3.zero, Quaternion.identity);
                    survivor.name = "Survivor " + (i+1);
                    survivor.transform.SetParent(this.transform);
                    characterList.Add(survivor.GetComponent<BaseCharacter>());
                }
            }
            Debug.Log("Character List has been made by default setting");
        }

        SetPositions(characterList);
        // Setting for UI
        UIManager.instance.SkillPanelInitilize();

        // Set Active Character
        SetActiveCharacterAtPos(1);
    }

    private void Update()
    {
        if(this.swapBtn.isBtnPressed)
        {
            // Don't move if move button is being pressed
            return;
        }
        playerInput.GetKeyInput();

        if(playerInput.H > 0)
        {
            if (this.transform.position.x > Commander.instance.rightBorder)
            {
                return;
            }
            playerMover.MoveForward();
        }
        else if (playerInput.H < 0)
        {
            if (this.transform.position.x < Commander.instance.leftBorder)
            {
                return;
            }
            playerMover.MoveBackWard();
        }
        else
        {
            playerMover.Stop();
        }
    }

    void MakeSingleton()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }


    // Initialize ()
    public void SetPositions(List<BaseCharacter> cList)
    {
        if (Commander.instance.IsBattle)
        {
            Commander.instance.IsActing = true;
        }

        for (int i = 0; i < cList.Count; i++)
        {
            if(cList[i].transform.localPosition.x != PlayerManager.positions[i].x)
            {
                // Move GameObjects
                iTween.MoveTo(cList[i].gameObject, iTween.Hash(
                    "x", PlayerManager.positions[i].x,
                    "time", swapSpeed,
                    "isLocal", true,
                    "easetype", iTween.EaseType.easeOutExpo
                ));

                // Set Position
                cList[i].Position = i + 1;

                // Change its transform in the hierarchy
                cList[i].transform.SetSiblingIndex(i);
            }
        }
    }


    public void SetActiveCharacterAtPos(int pos)
    {
        var target = GetCharacterAtPos(pos);

        SetActiveCharacter(target);
    }

    public void SetActiveCharacter(BaseCharacter target)
    {
        if (!Commander.instance.IsBattle && activeCharacter == target)
        {
            return;
        }

        if(activeCharacter)
        {
            ClearActiveCharacter();
        }

        if(EnemyManager.instance.activeCharacter)
        {
            EnemyManager.instance.ClearActiveCharacter();
        }
        activeCharacter = target;

        activeCharacter.isActive = true;
        activeCharacter.cursor.SetActive(true);

        // Update UI Panel
        UIManager.instance.UpdateUIPanel(activeCharacter);
    }

    public BaseCharacter GetCharacterAtPos(int number)
    {
        // It can return null if the character at pos is dead
        return characterList[number - 1];
    }

    public void ClearActiveCharacter()
    {

        activeCharacter.isActive = false;
        activeCharacter.cursor.SetActive(false);
        activeCharacter.targetCursor.SetActive(false);

        // After completing necessary things, then clear activeCharacter to null
        activeCharacter = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch(collision.gameObject.tag)
        {
            case "Trigger":
                var battle = collision.gameObject.GetComponent<Trigger>();
                Commander.instance.InitBattle(battle);
                break;

            case "Goal":
                isReached = true;
                Commander.instance.IsGameOver = true;
                Debug.Log("Cleared!========================");
                break;
        }
    }

    public void DrawTargets(BaseSkill activeSkill)
    {

        if(this.swapBtn.isBtnPressed)
        {
            ClearSwapTargets();
        }

        switch (activeSkill.skillRange)
        {
            case SkillRange.Unfriendly:

                ClearUnfriendlyTargets();

                for (int i = 0; i < EnemyManager.instance.characterList.Count; i++)
                {
                    if(activeSkill.targetPositions[i])
                    {
                        EnemyManager.instance.characterList[i].targetCursor.SetActive(true);
                        EnemyManager.instance.characterList[i].isTargeted = true;
                        unfriendlyTargets.Add(EnemyManager.instance.characterList[i]);
                    }
                }
                Commander.instance.turnStateMachine.currentTurnState = TurnStateMachine.TurnState.ConfirmTarget;
                break;


            case SkillRange.Friendly:
                ClearFriendlyTargets();

                for (int i = 0; i < PlayerManager.instance.characterList.Count; i++)
                {
                    if (activeSkill.targetPositions[i])
                    {
                        PlayerManager.instance.characterList[i].targetCursor.SetActive(true);
                        PlayerManager.instance.characterList[i].isTargeted = true;
                        friendlyTargets.Add(PlayerManager.instance.characterList[i]);
                    }
                }
                Commander.instance.turnStateMachine.currentTurnState = TurnStateMachine.TurnState.ConfirmTarget;

                break;

            case SkillRange.Self:
                ClearUnfriendlyTargets();

                activeCharacter.targetCursor.SetActive(true);
                activeCharacter.isTargeted = true;
                friendlyTargets.Add(activeCharacter);
                Commander.instance.turnStateMachine.currentTurnState = TurnStateMachine.TurnState.ConfirmTarget;
                break;
            default:
                break;
        }
    }

    void ClearUnfriendlyTargets()
    {
        foreach (var t in unfriendlyTargets)
        {
            t.targetCursor.SetActive(false);
            t.isTargeted = false;
        }

        unfriendlyTargets.Clear();
    }

    void ClearFriendlyTargets()
    {
        foreach (var t in friendlyTargets)
        {
            t.targetCursor.SetActive(false);
            t.isTargeted = false;
        }

        friendlyTargets.Clear();
    }

    void ClearSwapTargets()
    {
        foreach(var t in characterList)
        {
            t.isSwapTarget = false;
            t.targetCursor.SetActive(false);
        }
        swapBtn.isBtnPressed = false;
    }

    public void ConfirmAllyTarget(BaseCharacter selectedTarget)
    {
        Commander.instance.turnStateMachine.HasConfirmedCommand = true;
        UIManager.instance.BeginUIShield();

        switch (activeCharacter.activeCommand.skillType)
        {
            case SkillType.SingleTarget:
                activeCharacter.CastToAlly(activeCharacter.activeCommand, selectedTarget);

                ClearFriendlyTargets();
                break;
            case SkillType.MultipleTarget:
                activeCharacter.CastToAllies(activeCharacter.activeCommand, friendlyTargets);

                ClearFriendlyTargets();
                break;
        }
    }

    public void ConfirmEnemyTarget(BaseEnemy selectedTarget)
    {
        Commander.instance.turnStateMachine.HasConfirmedCommand = true;
        UIManager.instance.BeginUIShield();

        switch (activeCharacter.activeCommand.skillType)
        {
            case SkillType.SingleTarget:
                activeCharacter.CastToEnemy(activeCharacter.activeCommand, selectedTarget);

                ClearUnfriendlyTargets();
                break;

            case SkillType.MultipleTarget:
                activeCharacter.CastToEnemies(activeCharacter.activeCommand, unfriendlyTargets);

                ClearUnfriendlyTargets();
                break;
            default:
                break;
        }
    }

    // Swap Positions

    public void Swap(List<BaseCharacter> sList, int activePos, int targetPos)
    {
        StartCoroutine(SwapRoutine(sList, activePos, targetPos));
    }

    IEnumerator SwapRoutine(List<BaseCharacter> sList, int activePos, int targetPos)
    {
        if(Commander.instance.IsBattle)
        {
            Commander.instance.turnStateMachine.HasConfirmedCommand = true;
        }

        UIManager.instance.BeginUIShield();
        // 1. Update character list
        var t = sList[activePos - 1];
        sList[activePos - 1] = sList[targetPos - 1];
        sList[targetPos - 1] = t;

        SetPositions(sList);

        ClearSwapTargets();

        yield return new WaitForSeconds(swapSpeed);

        if (Commander.instance.IsBattle)
        {
            Commander.instance.IsActing = false;
        }
        else
        {
            UIManager.instance.EndUIShield();
        }
    }

    // This function will be called by button clicking
    public void DrawSwapPositions()
    {
        ClearUnfriendlyTargets();
        ClearFriendlyTargets();

        var frontPos = activeCharacter.Position - 1;
        var backPos = activeCharacter.Position + 1;

        if(frontPos > 0)
        {
            var target = GetCharacterAtPos(frontPos);
            target.targetCursor.SetActive(true);
            target.isSwapTarget = true;
        }

        if(backPos < 5)
        {
            var target = GetCharacterAtPos(backPos);
            target.targetCursor.SetActive(true);
            target.isSwapTarget = true;
        }

        if(Commander.instance.IsBattle)
        {
            Commander.instance.turnStateMachine.currentTurnState = TurnStateMachine.TurnState.ConfirmTarget;
        }

        PlayerManager.instance.activeCharacter.activeCommand = null;
    }

    public void CancelSwap()
    {
        ClearSwapTargets();
    }
}
