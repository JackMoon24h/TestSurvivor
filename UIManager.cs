using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour 
{
    // References
    public static UIManager instance;
    Canvas canvas;
    public Vector3 correction = new Vector3(0, 200f, 0);
    public List<SkillDisplay> skillDisplays = new List<SkillDisplay>();
    public Image profileImage;
    public Text profileName;

    // Set from inspector
    public GameObject mainPanel;
    public GameObject commandPanel;

    public Image[] playerListIMG = new Image[4];
    public Color pExistColor;
    public Color pNotExistColor;

    public Image[] enemyListIMG = new Image[4];
    public Color eExistColor;
    public Color eNotExistColor;

    public GameObject UIShield;
    public bool OnUIShield;

    public int availableSkNum;

    public GameObject damagePrefab;
    public GameObject criticalPrefab;
    public GameObject bleedPrefab; // Bleed, Bleed Resist
    public GameObject infectPrefab; // Infect, Infect Resist
    public GameObject healPrefab;
    public GameObject stunPrefab; // Stun, Stun Resist
    public GameObject resistPrefab; // Resist, Buff
    public GameObject dodgePrefab; // Dodge, Miss
    public GameObject mentalPrefab;
    public GameObject mentalHealPrefab;
    public GameObject deathPrefab;

    // Use this for initialization
    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        profileImage = mainPanel.transform.GetChild(0).GetComponent<Image>();
        profileName = mainPanel.transform.GetChild(1).GetComponent<Text>();
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
        this.profileName.text = updateTarget.gameObject.name;

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

    public void SetPlayerListUI()
    {
        for (int i = 0; i < PlayerManager.instance.characterList.Count; i++)
        {
            if (PlayerManager.instance.characterList[i])
            {
                playerListIMG[i].color = pExistColor;
            }
            else
            {
                playerListIMG[i].color = pNotExistColor;
            }
        }
    }

    public void SetEnemyListtUI()
    {
        if(!Commander.instance.IsBattle)
        {
            return;
        }
        for (int i = 0; i < EnemyManager.instance.characterList.Count; i++)
        {
            if (EnemyManager.instance.characterList[i])
            {
                enemyListIMG[i].color = eExistColor;
            }
            else
            {
                enemyListIMG[i].color = eNotExistColor;
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
                label.GetComponentInChildren<Text>().text = "Critical!\n" + amount.ToString();
                break;

            case "Heal":
                label = Instantiate(healPrefab);
                label.GetComponentInChildren<Text>().text = amount.ToString();
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
                label.GetComponentInChildren<Text>().text = "DODGE!";
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
                label.GetComponentInChildren<Text>().text = amount.ToString();
                break;

            case "Refusal":
                label = Instantiate(mentalPrefab);
                label.GetComponentInChildren<Text>().text = "Refusal..";
                break;

            default:
                label = Instantiate(damagePrefab);
                label.GetComponentInChildren<Text>().text = amount.ToString();
                break;
        }

        label.transform.SetParent(canvas.gameObject.transform);
        var rand = Random.Range(0.95f, 1.15f);
        Vector2 screenPos = Camera.main.WorldToScreenPoint(target.transform.position) + correction * rand;
        label.transform.position = screenPos;
    }
}