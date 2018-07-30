using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandPanel : MonoBehaviour 
{
	[HideInInspector]public Text[] unitInfo = new Text[18];
    public Image thumb;
    public Image abilityImage1;
    public Image abilityImage2;
    public Image abilityImage3;
    public Image abilityImage4;
    public Image abilityImage5;

    Overseer overseer;
    Deck playerDeck;
    public Unit unit;

    private void Awake()
    {
        overseer = Object.FindObjectOfType<Overseer>().GetComponent<Overseer>();
        playerDeck = Object.FindObjectOfType<Deck>().GetComponent<Deck>();

        for (int i = 0; i < unitInfo.Length; i++)
        {
            unitInfo[i] = this.gameObject.transform.GetChild(i).GetComponent<Text>();
        }
    }

    // Use this for initialization
    void Start () 
	{
        AssignUnit(playerDeck.GetUnitByPosition(1));
	}
	
    public void AssignUnit(Unit targetUnit)
    {
        unit = targetUnit;
        thumb.sprite = targetUnit.thumbSprite;
    }

    public void UpdatePanel()
    {
        unitInfo[0].text = unit.Health.ToString() + " / " + unit.MaxHealth.ToString();
        unitInfo[1].text = unit.Mental.ToString() + " / " + unit.MaxMental.ToString();
        unitInfo[2].text = unit.Damage.ToString();
        unitInfo[3].text = unit.Protection.ToString();
        unitInfo[4].text = unit.Endurance.ToString();
        unitInfo[5].text = unit.Speed.ToString();
        unitInfo[6].text = unit.Accuracy.ToString();
        unitInfo[7].text = unit.Dodge.ToString();
        unitInfo[8].text = unit.Critical.ToString();
        unitInfo[9].text = unit.Virtue.ToString();
        unitInfo[10].text = unit.StressRes.ToString();
        unitInfo[11].text = unit.BleedRes.ToString();
        unitInfo[12].text = unit.InfectRes.ToString();
        unitInfo[13].text = unit.StunRes.ToString();
        unitInfo[14].text = unit.MoveRes.ToString();
        unitInfo[15].text = unit.DeathBlow.ToString();
        unitInfo[16].text = unit.Name;
        unitInfo[17].text = unit.job.ToString();
    }
}
