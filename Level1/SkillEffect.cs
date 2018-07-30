using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffect : MonoBehaviour 
{
    GameManager gameManager;
    SquadManager squadManager;
    Skill skillManager;

    public enum MainEffect
    {
        DAMAGE,
        MENTALDAMAGE,
        HEAL,
        CURE,
        BLEED,
        INFECT,
        STUN,
        MOVE
    }
    public MainEffect mainEffect;

    public float damageRate;
    public float mentalDamageRate;
    public float healRate;
    public float cureRate;
    public float bleedRate;
    public float infectRate;
    public float stunRate;
    public float moveRate;

	// Use this for initialization
	void Start () 
	{
        gameManager = Object.FindObjectOfType<GameManager>().GetComponent<GameManager>();
        squadManager = Object.FindObjectOfType<SquadManager>().GetComponent<SquadManager>();
        skillManager = GetComponent<Skill>();
	}
}
