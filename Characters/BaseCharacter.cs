using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Rarity
{
    Common,
    Rare,
    SuperRare,
    Legend
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

    public GameObject mpGauge;
   
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
    public Virtue virtuousEffect;
    bool m_isVirtuous = false;
    public bool IsVirtuous { get { return m_isVirtuous; } set { m_isVirtuous = value; } }

    bool m_doingMentalAction = false;
    public bool DoingMentalAction { get { return m_doingMentalAction; } set { m_doingMentalAction = value; } }

    public int onDodMentalDamage = 4;
    public int onCritMentalDamage = 5;
    public int onCritMentalHeal = 3;

    private bool m_hasDoneActOut = false;
    public bool HasDoneActOut { get { return m_hasDoneActOut; } set { m_hasDoneActOut = value; } }

    public List<Quirk> positiveQuirks = new List<Quirk>(2);
    public List<Quirk> negativeQuirks = new List<Quirk>(2);

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

    protected override IEnumerator MentalEffectRoutine(Actor attacker, bool hasMentalEffect, int mentalDMG)
    {
        yield return base.MentalEffectRoutine(attacker, hasMentalEffect, mentalDMG);
        m_doingMentalAction = true;

        if (this.onDodge)
        {
            this.m_doingMentalAction = false;
            yield break;
        }

        // If this character took critical attack from enemy
        if (hasMentalEffect)
        {
            yield return StartCoroutine(TakeMentalDamage(mentalDMG));
        }
        else if (onCrit)
        {
            yield return StartCoroutine(TakeMentalDamage(onCritMentalDamage));
        }

        if(m_hasDoneActOut)
        {
            if(m_isAfflicted)
            {
                speaker.FixedSpeak("No need to help me...I am Done....It is Over..");
                while(Commander.instance.IsSpeaking)
                {
                    yield return null;
                }
            }
            else if (m_isVirtuous)
            {
                speaker.FixedSpeak("Don't give up! Keep supporting me! I can destroy them all.");
                while (Commander.instance.IsSpeaking)
                {
                    yield return null;
                }
            }

            m_hasDoneActOut = false;
        }

        Debug.Log("MentalEffectRoutine Over");
        yield return new WaitForSeconds(0.5f);

        m_doingMentalAction = false;
    }

    public virtual void TakeMentalHeal(int heal)
    {
        StartCoroutine(TakeMentalHealRoutine(heal));
    }

    public virtual IEnumerator TakeMentalHealRoutine(int heal)
    {
        m_doingMentalAction = true;
        while (Commander.instance.IsActing)
        {
            yield return null;
        }

        this.m_mental = Mathf.Clamp(m_mental + heal, -100, m_maxMental);
        UIManager.instance.CreateEffect("MentalHeal", this, heal);

        yield return new WaitForSeconds(0.1f);

        UpdateMPBar();

        yield return new WaitForSeconds(0.4f);
        m_doingMentalAction = false;
    }

    public virtual IEnumerator TakeMentalDamage(int mentalDmg)
    {
        m_doingMentalAction = true;
        Debug.Log("TakeMentalDamage Routine");
        int temp = 0;
        if (this.m_isAfflicted)
        {
            temp = Mathf.RoundToInt(mentalDmg * 1.5f);
        }
        else if (this.m_isVirtuous)
        {
            temp = 0;
        }
        else
        {
            temp = mentalDmg;
        }

        var actualDMG = Mathf.Clamp(temp, 0, 100);
        this.m_mental -= actualDMG;

        var randQueue = Random.Range(0.2f, 0.4f);

        yield return new WaitForSeconds(randQueue);

        UIManager.instance.CreateEffect("MentalDamage", this, actualDMG);

        yield return new WaitForSeconds(0.1f);

        // Mental Check
        if (this.m_mental <= 0)
        {
            UpdateMPBar();

            var rand = Random.Range(0.2f, 0.4f);

            yield return new WaitForSeconds(rand);

            while(PlayerManager.instance.isSuffering)
            {
                yield return null;
            }

            yield return StartCoroutine(Suffer());
        }
        else
        {
            UpdateMPBar();
        }

        m_doingMentalAction = false;
    }

    // When mental reaches 0, the character suffers...
    IEnumerator Suffer()
    {
        PlayerManager.instance.isSuffering = true;
        Debug.Log("Suffer Routine");
        if (this.m_isAfflicted)
        {
            var roll = Random.Range(0f, 1f);
            if(roll <= 0.25f)
            {
                this.speaker.Speak();
                while (Commander.instance.IsSpeaking)
                {
                    yield return null;
                }
            }
            // Do Afflicted Behavior
        }
        else if (this.m_isVirtuous)
        {
            var roll = Random.Range(0f, 1f);
            if (roll <= 0.25f)
            {
                this.speaker.Speak();
                while (Commander.instance.IsSpeaking)
                {
                    yield return null;
                }
            }

            // Do Virtuous Behavior
        }
        else
        {
            float rand = Random.Range(0, 1f);

            if (rand > m_virtue)
            {
                // Affliction
                yield return StartCoroutine(AfflictRoutine());

                this.speaker.Speak();
                while (Commander.instance.IsSpeaking)
                {
                    yield return null;
                }

                GainQuirk(false);
            }
            else
            {
                // Virtue
                yield return StartCoroutine(VirtueRoutine());

                this.speaker.Speak();
                while (Commander.instance.IsSpeaking)
                {
                    yield return null;
                }

                GainQuirk(true);
            }
        }

        yield return new WaitForSeconds(0.5f);

        PlayerManager.instance.isSuffering = false;
    }

    IEnumerator AfflictRoutine()
    {
        Debug.Log("Affliction Routine");
        // Tested
        Commander.instance.narrator.Narrate(this.m_name +"'s Mental is Tested...");
        Camera.main.GetComponent<CameraController>().Shake(0.2f, 3f, 0.2f);
        while (Commander.instance.narrator.IsNarrating)
        {
            yield return null;
        }
        // Camera Red Blur & Act
        this.characterAction.MentalAct("Affliction");

        // Create BG effect
        var bg = Instantiate(Commander.instance.sufferBackGrounds[0]);
        bg.transform.SetParent(this.characterAction.body.transform);
        bg.transform.localPosition = Vector3.zero;

        // Set Effect
        var rand = Random.Range(0, Commander.instance.afflictionPrefabs.Count);
        var afflictionObj = Instantiate(Commander.instance.afflictionPrefabs[rand], new Vector3(-1000f, -1000f, 0f), Quaternion.identity);
        var temp = (AfflictionType)rand;

        afflictionObj.GetComponent<Affliction>().SetEffects(this);

        Commander.instance.narrator.ShowAfflictionResult(temp.ToString()); ;

        while(!this.characterAction.isAfflictionActionOver)
        {
            yield return null;
        }
        Destroy(bg);
        afflictionObj.transform.SetParent(this.transform);
        afflictionObj.transform.localPosition = mentalPosition;
        // ActOut
    }

    IEnumerator VirtueRoutine()
    {
        Debug.Log("Virtue Routine");
        // Tested
        Commander.instance.narrator.Narrate(this.m_name + "'s Mental is Tested...");
        Camera.main.GetComponent<CameraController>().Shake(0.2f, 3f, 0.2f);
        while (Commander.instance.narrator.IsNarrating)
        {
            yield return null;
        }

        this.characterAction.MentalAct("Virtue");

        // Create BG effect
        var bg = Instantiate(Commander.instance.sufferBackGrounds[1]);
        bg.transform.SetParent(this.characterAction.body.transform);
        bg.transform.localPosition = Vector3.zero;

        // Set Effect
        var rand = Random.Range(0, Commander.instance.virtuePrefabs.Count);
        var virtueObj = Instantiate(Commander.instance.virtuePrefabs[rand], new Vector3(-1000f, -1000f, 0f), Quaternion.identity);
        var temp = (VirtueType)rand;

        virtueObj.GetComponent<Virtue>().SetEffects(this);

        Commander.instance.narrator.ShowAfflictionResult(temp.ToString());

        while (!this.characterAction.isAfflictionActionOver)
        {
            yield return null;
        }
        Destroy(bg);
        virtueObj.transform.SetParent(this.transform);
        virtueObj.transform.localPosition = mentalPosition;
        // ActOut
    }

    public override void TakeHeal(int heal)
    {
        if(m_isAfflicted)
        {
            UIManager.instance.CreateEffect("Refusal", this, heal);
            m_hasDoneActOut = true;
            return;
        }

        if(m_isVirtuous)
        {
            heal = Mathf.RoundToInt(heal * 1.25f);
            m_hasDoneActOut = true;
        }

        base.TakeHeal(heal);
    }

    // TakeMentalHeal : Auto mental healing when take-menta-effect coroutine
    // TakeMentalCure : Direct skill effect
    public override void TakeMentalCure(int amount)
    {
        base.TakeMentalCure(amount);

        if (m_isAfflicted)
        {
            UIManager.instance.CreateEffect("Refusal", this, amount);
            m_hasDoneActOut = true;
            return;
        }

        if (m_isVirtuous)
        {
            amount = Mathf.RoundToInt(amount * 1.25f);
            m_hasDoneActOut = true;
            return;
        }

        Debug.Log(m_mental + " " + amount + " " + m_mental + amount);

        this.m_mental = Mathf.Clamp(m_mental + amount, 0, m_maxMental);
        UIManager.instance.CreateEffect("MentalHeal", this, amount);
        UpdateMPBar();
    }

    void UpdateMPBar()
    {
        var ratio = Mathf.Max((float)this.m_mental / (float)this.m_maxMental, 0);

        iTween.ScaleTo(this.mpGauge, iTween.Hash(
            "x", ratio,
            "isLocal", true,
            "easetype", iTween.EaseType.easeInQuart,
            "time", 1.2f,
            "delay", 0.6f
        ));
    }

    void GainQuirk(bool positive)
    {
        if(positive && this.positiveQuirks.Count < 2)
        {
            int rand = Random.Range(0, Commander.instance.positiveQuirkPrefabs.Count);
            var temp = Instantiate(Commander.instance.positiveQuirkPrefabs[rand]);
            temp.transform.SetParent(this.gameObject.transform);
            this.positiveQuirks.Add(temp.GetComponent<Quirk>());
        }
        else if (!positive && this.negativeQuirks.Count < 2)
        {
            int rand = Random.Range(0, Commander.instance.positiveQuirkPrefabs.Count);
            var temp = Instantiate(Commander.instance.negativeQuirkPrefabs[rand]);
            temp.transform.SetParent(this.gameObject.transform);
            this.negativeQuirks.Add(temp.GetComponent<Quirk>());
        }

        UIManager.instance.CreateEffect("Quirk", this, 0);
    }
}
