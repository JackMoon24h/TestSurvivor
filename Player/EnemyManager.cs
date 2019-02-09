using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    public static float spacing = 3.5f;
    public static readonly Vector2[] positions =
    {
        new Vector2(1 * spacing, 0f),
        new Vector2(2 * spacing, 0f),
        new Vector2(3 * spacing, 0f),
        new Vector2(4 * spacing, 0f),
    };

    public List<BaseEnemy> characterList = new List<BaseEnemy>();
    public BaseEnemy activeCharacter;
    public GameObject clickedObject;
    public float swapSpeed = 0.6f;

    public List<BaseCharacter> unfriendlyTargets = new List<BaseCharacter>();
    public List<BaseEnemy> friendlyTargets = new List<BaseEnemy>();

    // Exclusive Parameters


    private void Awake()
    {
        MakeSingleton();
    }

    // Use this for initialization
    void Start () 
    {
		
	}
	
	// Update is called once per frame
	void Update () 
    {
		
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

    public void Deploy(Trigger t)
    {
        characterList.Clear();
        this.transform.position = PlayerManager.instance.transform.position + new Vector3(0, 0.25f, 0f);

        for (int i = 0; i < t.enemyPrefabs.Length; i++)
        {
            if (t.enemyPrefabs[i] != null)
            {
                var enemy = Instantiate(t.enemyPrefabs[i]);
                enemy.name = "Walker" + (i + 1);
                enemy.transform.SetParent(this.transform);

                var enemyCompo = enemy.GetComponent<BaseEnemy>();
                characterList.Add(enemyCompo);

                // Initialize Enemies
                enemyCompo.Initiate();
                enemyCompo.characterAction.Initiate();

                enemyCompo.cursor.SetActive(false);
            }
        }
        InitialSetPosition(characterList);
    }

    void InitialSetPosition(List<BaseEnemy> eList)
    {
        for (int i = 0; i < eList.Count; i++)
        {
            if (eList[i] != null)
            {
                eList[i].Position = i + 1;
                eList[i].transform.localPosition = EnemyManager.positions[i];
            }
        }
    }

    public void SetPositions(List<BaseEnemy> cList)
    {
        if (Commander.instance.IsBattle)
        {
            Commander.instance.IsActing = true;
        }

        for (int i = 0; i < cList.Count; i++)
        {
            if (cList[i].transform.localPosition.x != EnemyManager.positions[i].x)
            {
                // Move GameObjects
                iTween.MoveTo(cList[i].gameObject, iTween.Hash(
                    "x", EnemyManager.positions[i].x,
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

    public void SetActiveCharacter(BaseEnemy target)
    {
        if (activeCharacter)
        {
            ClearActiveCharacter();
        }

        if (PlayerManager.instance.activeCharacter)
        {
            PlayerManager.instance.ClearActiveCharacter();
        }
        activeCharacter = target;

        activeCharacter.isActive = true;
        activeCharacter.cursor.SetActive(true);
    }

    public BaseEnemy GetCharacterAtPos(int number)
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

    public void DrawTargets(BaseSkill activeSkill)
    {
        switch (activeSkill.skillRange)
        {
            case SkillRange.Unfriendly:
                ClearUnfriendlyTargets();
                int rand = Random.Range(0, activeSkill.targetPositions.Length);

                for (int i = 0; i < PlayerManager.instance.characterList.Count; i++)
                {
                    if (activeSkill.targetPositions[i])
                    {
                        PlayerManager.instance.characterList[i].targetCursor.SetActive(true);
                        PlayerManager.instance.characterList[i].isTargeted = true;
                        unfriendlyTargets.Add(PlayerManager.instance.characterList[i]);
                    }
                }
                Commander.instance.turnStateMachine.currentTurnState = TurnStateMachine.TurnState.ConfirmTarget;
                break;


            case SkillRange.Friendly:
                ClearFriendlyTargets();

                for (int i = 0; i < EnemyManager.instance.characterList.Count; i++)
                {
                    if (activeSkill.targetPositions[i])
                    {
                        EnemyManager.instance.characterList[i].targetCursor.SetActive(true);
                        EnemyManager.instance.characterList[i].isTargeted = true;
                        friendlyTargets.Add(EnemyManager.instance.characterList[i]);
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
        foreach (var t in characterList)
        {
            t.isSwapTarget = false;
            t.targetCursor.SetActive(false);
        }
    }

    public void ConfirmAllyTarget(BaseEnemy selectedTarget)
    {
        Commander.instance.turnStateMachine.HasConfirmedCommand = true;

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

    public void ConfirmEnemyTarget(BaseCharacter selectedTarget)
    {
        Commander.instance.turnStateMachine.HasConfirmedCommand = true;

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

    public void Swap(List<BaseEnemy> sList, int activePos, int targetPos)
    {
        StartCoroutine(SwapRoutine(sList, activePos, targetPos));
    }

    IEnumerator SwapRoutine(List<BaseEnemy> sList, int activePos, int targetPos)
    {
        if (Commander.instance.IsBattle)
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
        if (EnemyManager.instance.characterList.Count == 1)
        {
            Commander.instance.turnStateMachine.IsSkipTurn = true;
            return;
        }
        ClearUnfriendlyTargets();
        ClearFriendlyTargets();

        var frontPos = activeCharacter.Position - 1;
        var backPos = activeCharacter.Position + 1;

        if (frontPos > 0)
        {
            var target = GetCharacterAtPos(frontPos);
            target.targetCursor.SetActive(true);
            target.isSwapTarget = true;
        }

        if (backPos < 5)
        {
            var target = GetCharacterAtPos(backPos);
            target.targetCursor.SetActive(true);
            target.isSwapTarget = true;
        }

        if (Commander.instance.IsBattle)
        {
            Commander.instance.turnStateMachine.currentTurnState = TurnStateMachine.TurnState.ConfirmTarget;
        }

        EnemyManager.instance.activeCharacter.activeCommand = null;
    }


    public void PlayTurn()
    {
        StartCoroutine(PlayTurnRoutine());
    }

    IEnumerator PlayTurnRoutine()
    {
        yield return new WaitForSeconds(0.5f);

        EnemyManager.instance.activeCharacter.ChooseCommand();
        if(Commander.instance.turnStateMachine.IsSkipTurn)
        {
            yield break;
        }

        Commander.instance.narrator.Narrate(EnemyManager.instance.activeCharacter.activeCommand.skillName);

        while (Commander.instance.narrator.IsNarrating)
        {
            yield return null;
        }

        // Set Target
        DrawTargets(EnemyManager.instance.activeCharacter.activeCommand);
        yield return new WaitForSeconds(0.7f);

        switch(EnemyManager.instance.activeCharacter.activeCommand.skillRange)
        {
            case SkillRange.Unfriendly:
                int randU = Random.Range(0, unfriendlyTargets.Count);
                ConfirmEnemyTarget(unfriendlyTargets[randU]);
                break;
            case SkillRange.Friendly:
                int randF = Random.Range(0, friendlyTargets.Count);
                ConfirmAllyTarget(friendlyTargets[randF]);
                break;
            case SkillRange.Self:
                ConfirmAllyTarget(activeCharacter);
                break;
        }

        yield return new WaitForSeconds(0.2f);
    }


}
