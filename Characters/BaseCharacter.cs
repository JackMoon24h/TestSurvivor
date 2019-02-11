using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Rarity
{
    Common,
    Rare,
    Unique,
    Legendary
}

[RequireComponent(typeof(Speaker))]
public class BaseCharacter : Actor 
{

    // Images
    public Sprite profileImage;
    public Speaker speaker;
    public CameraEffect cameraEffect;

    // Exclusive properties
    protected string jobName;
    public string JobName { get { return jobName; } set { jobName = value; } }

    protected string jobDescription;
    public string JobDescription { get { return jobDescription; } set { jobDescription = value; } }

    protected int m_maxMental;
    public int MaxMental { get { return m_maxMental; } set { m_maxMental = value; } }

    protected int m_mental;
    public int Mental { get { return m_mental; } set { m_mental = value; } }

    protected int m_endurance;
    public int Endurance { get { return m_endurance; } set { m_endurance = value; } }

   
    // float params

    protected float m_virtue;
    public float Virtue { get { return m_virtue; } set { m_virtue = value; } }

    protected float m_deathBlow;
    public float DeathBlow { get { return m_deathBlow; } set { m_deathBlow = value; } }

    // Enum
    public Rarity rarity;

    // Afflicion
    public Affliction affliction;
    bool m_isAfflicted = false;
    public bool IsAfflicted { get { return m_isAfflicted; } set { m_isAfflicted = value; } }

    // Virtue

    bool m_isVirtuous = false;
    public bool IsVirtuous { get { return m_isVirtuous; } set { m_isVirtuous = value; } }

    protected override void Awake()
    {
        base.Awake();
        cameraEffect = Camera.main.GetComponent<CameraEffect>();
        speaker = GetComponent<Speaker>();
    }

    // Use this for initialization
    protected virtual void Start () 
    {

	}
	
	// Update is called once per frame
	protected virtual void Update () 
    {
        Move();
	}

    protected void Move()
    {
        if (PlayerManager.instance.isMovingForward)
        {
            characterAction.MoveForwardAction();
        }
        else if (PlayerManager.instance.isMovingBackWard)
        {
            characterAction.MoveBackWardAction();
        }
        else
        {
            characterAction.StopAction();
        }
    }

    public override void UpdateHPBar()
    {
        base.UpdateHPBar();
    }

    public override void TakeDamage(int dmg)
    {
        base.TakeDamage(dmg);
    }

    public override void TakeMentalDamage(int mentalDmg)
    {
        var actualDMG = (int)Mathf.Round(mentalDmg / this.m_endurance);
        this.m_mental -= actualDMG;

        UIManager.instance.CreateEffect("MentalDamage", this, actualDMG);

        MentalCheck();
    }

    void MentalCheck()
    {
        if (this.m_mental <= 0)
        {
            this.m_mental = 0;
            UpdateMPBar();
            Suffer();
        }
        else
        {
            UpdateMPBar();
        }
    }

    void UpdateMPBar()
    {

    }

    public override void TakeMentalEffect()
    {
        base.TakeMentalEffect();


    }

    // When mental reaches 0, the character suffers...
    public void Suffer()
    {
        float rand = Random.Range(0, 1f);

        if(rand > m_virtue)
        {
            // Affliction
            StartCoroutine(AfflictRoutine());
        }
        else
        {
            // Virtue
            StartCoroutine(VirtueRoutine());
        }
    }

    IEnumerator AfflictRoutine()
    {
        // Tested
        Commander.instance.narrator.Narrate(this.m_name +"'s Mental is Tested...");
        while(Commander.instance.narrator.IsNarrating)
        {
            yield return null;
        }
        // Camera Red Blur & Act
        cameraEffect.EnableRedCameraBlur(true);
        // Set Effect
        var rand = Random.Range(0, Commander.instance.afflictionPrefabs.Count);
        var afflictionObj = Instantiate(Commander.instance.afflictionPrefabs[rand]);
        afflictionObj.transform.SetParent(this.transform);

        afflictionObj.GetComponent<Affliction>().SetEffects();

        yield return new WaitForSeconds(1f);
        cameraEffect.EnableRedCameraBlur(false);
        // ActOut

    }

    IEnumerator VirtueRoutine()
    {
        // Tested
        Commander.instance.narrator.Narrate(this.m_name + "'s Mental is Tested...");
        while (Commander.instance.narrator.IsNarrating)
        {
            yield return null;
        }
        // Camera Red Blur & Act

        // Set Effect

        // ActOut

    }
}
