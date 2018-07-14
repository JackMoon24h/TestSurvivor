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

	// Use this for initialization
	void Start () 
	{
        gameManager = Object.FindObjectOfType<GameManager>().GetComponent<GameManager>();
        squadManager = Object.FindObjectOfType<SquadManager>().GetComponent<SquadManager>();
        skillManager = GetComponent<Skill>();
	}
}
