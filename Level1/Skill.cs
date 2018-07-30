using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill : MonoBehaviour 
{
    GameManager gameManager;
    SquadManager squadManager;
    EnemySquadManager enemySquadManager;

    public Character character;
    public SkillTarget skillTarget;
    public SkillEffect skillEffect;

    // Skill Paramters
    public string skillName;
    public string description;

    public int level = 1;
    public int skillNum;

    // Use this for initialization
    void Start () 
    {
        gameManager = Object.FindObjectOfType<GameManager>().GetComponent<GameManager>();
        squadManager = Object.FindObjectOfType<SquadManager>().GetComponent<SquadManager>();
        character = this.transform.parent.GetComponent<Character>();
        skillTarget = GetComponent<SkillTarget>();
        skillEffect = GetComponent<SkillEffect>();
	}
	
    public void OnClickEnter()
    {
        Debug.Log(this.name + " is Clicked");
        skillTarget.DrawTarget();
    }
}
