using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour 
{
    // References
    public static UIManager instance;
    Canvas canvas;
    public Vector3 correction = new Vector3(0, 85f, 0);
    public List<SkillDisplay> skillDisplays = new List<SkillDisplay>();
    public Image profileImage;
    public Text profileName;
    public Text jobName;

    // Set from inspector
    public GameObject mainPanel;
    public GameObject commandPanel;
    public GameObject infoPanel;
    public GameObject skillInfoPanel;

    public Text skillDes;

    public Text curHpLabel;
    public Text maxHpLabel;
    public Text curMpLabel;
    public Text maxMpLabel;
    public Text dmgLabel;
    public Text protLabel;
    public Text accLabel;
    public Text dodLabel;
    public Text critLabel;
    public Text spdLabel;
    public Text bleedLabel;
    public Text infectLabel;
    public Text stunLabel;
    public Text moveLabel;
    public Text virtueLabel;
    public Text afflictLabel;

    public GameObject UIShield;
    public bool OnUIShield;

    public int availableSkNum;
    public GameObject afflictedMark;
    public GameObject virtuedMark;

    public GameObject damagePrefab;
    public GameObject criticalPrefab;
    public GameObject bleedPrefab; // Bleed, Bleed Resist
    public GameObject infectPrefab; // Infect, Infect Resist
    public GameObject healPrefab;
    public GameObject stunPrefab; // Stun, Stun Resist
    public GameObject movePrefab;
    public GameObject resistPrefab; // Resist, Buff
    public GameObject dodgePrefab; // Dodge, Miss
    public GameObject mentalPrefab;
    public GameObject mentalHealPrefab;
    public GameObject deathPrefab;

    public GameObject quirkPrefab;

    // Status Windows set in the inspector
    public GameObject rewardWindow;


    // Use this for initialization
    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        skillDes = skillInfoPanel.GetComponentInChildren<Text>();
        if(profileImage == null)
        {
            profileImage = mainPanel.transform.GetChild(0).GetComponent<Image>();
        }

        if(profileName == null)
        {
            profileName = mainPanel.transform.GetChild(1).GetComponent<Text>();
        }
        if(afflictedMark == null)
        {
            afflictedMark = profileImage.transform.GetChild(0).gameObject;
        }
        if (virtuedMark == null)
        {
            virtuedMark = profileImage.transform.GetChild(1).gameObject;
        }
    }
    void Start () 
    {
        MakeSingleton();
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

    public void SkillPanelInitilize()
    {
        // Doing this for loading scene
        if(skillDisplays.Count > 0)
        {
            skillDisplays.Clear();
        }

        skillDisplays.AddRange(GetComponentsInChildren<SkillDisplay>());
        for (int i = 0; i < skillDisplays.Count; i++)
        {
            skillDisplays[i].SetSkillInfo(i + 1);
        }
    }

    public void UpdateUIPanel(BaseCharacter updateTarget)
    {
        this.profileImage.sprite = updateTarget.profileImage;
        this.profileName.text = updateTarget.Name;
        this.jobName.text = updateTarget.JobName;
        this.curHpLabel.text = updateTarget.Health.ToString();
        this.maxHpLabel.text = updateTarget.MaxHealth.ToString();
        this.curMpLabel.text = updateTarget.Mental.ToString();
        this.maxMpLabel.text = updateTarget.MaxMental.ToString();
        this.dmgLabel.text = updateTarget.Damage.ToString();
        this.protLabel.text = updateTarget.Protection.ToString();
        this.accLabel.text = Mathf.RoundToInt(updateTarget.Accuracy * 100).ToString() + "%";
        this.dodLabel.text = Mathf.RoundToInt(updateTarget.Dodge * 100).ToString() + "%";
        this.critLabel.text = Mathf.RoundToInt(updateTarget.Critical * 100).ToString() + "%";
        this.spdLabel.text = updateTarget.Speed.ToString();
        this.bleedLabel.text = Mathf.RoundToInt(updateTarget.BleedRes * 100).ToString() + "%";
        this.infectLabel.text = Mathf.RoundToInt(updateTarget.InfectRes * 100).ToString() + "%";
        this.stunLabel.text = Mathf.RoundToInt(updateTarget.StunRes * 100).ToString() + "%";
        this.moveLabel.text = Mathf.RoundToInt(updateTarget.MoveRes * 100).ToString() + "%";
        this.virtueLabel.text = Mathf.RoundToInt(updateTarget.Virtue * 100).ToString() + "%";

        if (updateTarget.IsAfflicted)
        {
            virtuedMark.SetActive(false);
            afflictedMark.SetActive(true);
            this.afflictLabel.text = updateTarget.affliction.Name;
        }
        else if (updateTarget.IsVirtuous)
        {
            afflictedMark.SetActive(false);
            virtuedMark.SetActive(true);
            this.afflictLabel.text = updateTarget.virtuousEffect.Name;
        }
        else
        {
            virtuedMark.SetActive(false);
            afflictedMark.SetActive(false);
            this.afflictLabel.text = "";
        }

        availableSkNum = 0;

        foreach (SkillDisplay sd in skillDisplays)
        {
            sd.UpdateSkillInfo(updateTarget);

            if(sd.IsAvailable)
            {
                availableSkNum++;
            }
        }
    }

    public void BeginUIShield()
    {
        if(UIShield != null)
        {
            UIShield.SetActive(true);
            OnUIShield = true;
        }
    }

    public void EndUIShield()
    {
        if(UIShield != null)
        {
            UIShield.SetActive(false);
            OnUIShield = false;
        }
    }

    public void OpenSkillInfo()
    {
        if(!skillInfoPanel.activeInHierarchy)
        {
            skillInfoPanel.SetActive(true);
        }
        skillDes.text = PlayerManager.instance.activeCharacter.activeCommand.skillDescription;
    }

    public void CloseSkillInfo()
    {
        skillInfoPanel.SetActive(false);
    }

    public void CreateEffect(string effect, Actor target, int amount)
    {
        GameObject label;

        switch(effect)
        {
            case "Damage":
                label = Instantiate(damagePrefab);
                label.GetComponentInChildren<Text>().text = amount.ToString();
                break;

            case "Critical":
                label = Instantiate(criticalPrefab);
                label.GetComponentInChildren<Text>().text = "Crit!\n" + amount.ToString();
                break;

            case "Heal":
                label = Instantiate(healPrefab);
                label.GetComponentInChildren<Text>().text = amount.ToString();
                break;

            case "Cure":
                label = Instantiate(healPrefab);
                label.GetComponentInChildren<Text>().text = "Cure!";
                break;

            case "Bleed":
                label = Instantiate(bleedPrefab);
                label.GetComponentInChildren<Text>().text = "BLEED";
                break;

            case "BleedResist":
                label = Instantiate(bleedPrefab);
                label.GetComponentInChildren<Text>().text = "RESIST";
                break;

            case "Infect":
                label = Instantiate(infectPrefab);
                label.GetComponentInChildren<Text>().text = "INFECT";
                break;

            case "InfectResist":
                label = Instantiate(infectPrefab);
                label.GetComponentInChildren<Text>().text = "RESIST";
                break;

            case "Stun":
                label = Instantiate(stunPrefab);
                label.GetComponentInChildren<Text>().text = "STUN!";
                break;

            case "StunResist":
                label = Instantiate(stunPrefab);
                label.GetComponentInChildren<Text>().text = "RESIST";
                break;

            case "Move":
                label = Instantiate(movePrefab);
                label.GetComponentInChildren<Text>().text = "MOVE!";
                break;

            case "MoveResist":
                label = Instantiate(movePrefab);
                label.GetComponentInChildren<Text>().text = "RESIST";
                break;

            case "Resist":
                label = Instantiate(resistPrefab);
                label.GetComponentInChildren<Text>().text = "RESIST";
                break;

            case "Buff":
                label = Instantiate(resistPrefab);
                label.GetComponentInChildren<Text>().text = "BUFF";
                break;

            case "Dodge":
                label = Instantiate(dodgePrefab);
                label.GetComponentInChildren<Text>().text = "MISS!";
                break;

            case "Death":
                label = Instantiate(deathPrefab);
                label.GetComponentInChildren<Text>().text = "DEATH";
                break;

            // When mental is broken
            case "MentalDamage":
                label = Instantiate(mentalPrefab);
                label.GetComponentInChildren<Text>().text = "MENTAL\n" + amount.ToString();
                break;

            case "MentalHeal":
                label = Instantiate(mentalHealPrefab);
                label.GetComponentInChildren<Text>().text = "Encourage\n" + amount.ToString();
                break;

            case "Refusal":
                label = Instantiate(mentalPrefab);
                label.GetComponentInChildren<Text>().text = "Refuse";
                break;

            case "Quirk":
                label = Instantiate(quirkPrefab);
                label.GetComponentInChildren<Text>().text = "Gained\n" + "Personality";
                break;

            default:
                label = Instantiate(damagePrefab);
                label.GetComponentInChildren<Text>().text = amount.ToString();
                break;
        }

        label.transform.SetParent(canvas.gameObject.transform);
        label.transform.localScale = new Vector3(1f, 1f, 1f);
        var rand = Random.Range(0.95f, 1.25f);
        Vector2 screenPos = Camera.main.WorldToScreenPoint(target.transform.position) + correction * rand;
        label.transform.position = screenPos;
    }
}