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
    public MainPanel mainPanel;
    public Button skillBtn;
    public Sprite image;

    public SkillTarget skillTarget;
    public SkillEffect skillEffect;

    // Skill Paramters
    protected string skillName;
    protected string description;
    protected int level;
    protected int damage;
    protected int mentalDamage;
    protected int heal;
    protected int cure;
    protected int bleedProb;
    protected int infectProb;
    protected int stunProb;
    protected int moveProb;

    // Use this for initialization
    void Start () 
    {
        gameManager = Object.FindObjectOfType<GameManager>().GetComponent<GameManager>();
        squadManager = Object.FindObjectOfType<SquadManager>().GetComponent<SquadManager>();
        character = this.GetComponent<Character>();
	}
	
    public void OnClickEnter()
    {
        
    }
}
