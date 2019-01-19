using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour 
{
    // References
    public static UIManager instance;
    public List<SkillDisplay> skillDisplays = new List<SkillDisplay>();
    public Image profileImage;
    public Text profileName;

    // Set from inspector
    public Image[] playerListIMG = new Image[4];
    public Color pExistColor;
    public Color pNotExistColor;

    public Image[] enemyListIMG = new Image[4];
    public Color eExistColor;
    public Color eNotExistColor;

    public GameObject UIShield;

    public GameObject actionResultPrefab;

    // Use this for initialization
    private void Awake()
    {
        profileImage = this.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        profileName = this.transform.GetChild(0).GetChild(1).GetComponent<Text>();
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

        foreach(SkillDisplay sd in skillDisplays)
        {
            sd.UpdateSkillInfo(updateTarget);
        }

        SetPlayerListUI();
        SetEnemyListtUI();
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
        }
    }

    public void EndUIShield()
    {
        if(UIShield != null)
        {
            UIShield.SetActive(false);
        }
    }

    public void CreateActionResultUI()
    {
        if (actionResultPrefab != null)
        {
            var actionResult = Instantiate(actionResultPrefab, Vector3.zero, Quaternion.identity);
        }
    }
}
