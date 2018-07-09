using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour 
{
    public GameManager gameManager;
    public Character character;

    public enum MainTarget
    {
        ENEMY,
        TEAM,
        SELF
    }

    public enum MainEffect
    {
        DAMAGE,
        MENTALDAMAGE,
        HEAL,
        CURE,
        BLEED,
        INFECT,
        STUN,
        MOVE,
        STATECHANGE
    }

    public bool[] targetRange = new bool[4];
    public bool[] availablePos = new bool[4];

    public Sprite image;

    // Skill Paramters
    protected MainTarget mainTarget;
    protected MainEffect mainEffect;
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
    protected string stateChange;

    // Use this for initialization
    void Start () 
    {
        gameManager = Object.FindObjectOfType<GameManager>().GetComponent<GameManager>();
        character = this.GetComponent<Character>();
	}
	
    // When this button is clicked...
    public void OnClickEnter()
    {
        
    }
}
