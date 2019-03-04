using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusWindow : BaseWindow 
{
    public Image profile;
    public Text rarity;
    public Text level;
    public Text job;
    public Text affliction;
    public Text afflictionDes;

    public GameObject[] preferredPositions = new GameObject[4];
    public GameObject[] preferredTargets = new GameObject[4];

    public Text[] positiveQuirkTitles = new Text[2];
    public Text[] positiveQuirkDes = new Text[2];
    public Text[] negativeQuirkTitles = new Text[2];
    public Text[] negativeQuirkDes = new Text[2];

    public override void OpenWindow()
    {
        if(CanOpen())
        {
            UpdateCard(PlayerManager.instance.activeCharacter);
            SoundManager.Instance.PlaySE(0);
            base.OpenWindow();
        }
    }

    public void UpdateCard(BaseCharacter target)
    {
        profile.sprite = target.profileImage;
        rarity.text = target.rarity.ToString();
        level.text = target.Level.ToString();
        job.text = target.job.ToString();

        for(int i = 0; i < preferredPositions.Length; i++)
        {
            preferredPositions[i].transform.localScale = target.PreffredPosition[i];
        }

        for (int i = 0; i < preferredTargets.Length; i++)
        {
            preferredTargets[i].transform.localScale = target.PreffredTarget[i];
        }

        if (target.IsAfflicted)
        {
            affliction.text = target.affliction.Name;
            afflictionDes.text = target.affliction.Description;
        }
        else if (target.IsVirtuous)
        {
            affliction.text = target.virtuousEffect.Name;
            afflictionDes.text = target.virtuousEffect.Description;
        }
        else
        {
            affliction.text = "Affliction";
            afflictionDes.text = "None";
        }

        switch(target.positiveQuirks.Count)
        {
            case 0:
                for(int i = 0; i < positiveQuirkTitles.Length; i++)
                {
                    this.positiveQuirkTitles[i].text = "";
                    this.positiveQuirkDes[i].text = "";
                }
                break;
            case 1:
                this.positiveQuirkTitles[0].text = target.positiveQuirks[0].Name;
                this.positiveQuirkDes[0].text = target.positiveQuirks[0].Description;
                this.positiveQuirkTitles[1].text = "";
                this.positiveQuirkDes[1].text = "";
                break;
            case 2:
                for (int i = 0; i < positiveQuirkTitles.Length; i++)
                {
                    this.positiveQuirkTitles[i].text = target.positiveQuirks[i].Name;
                    this.positiveQuirkDes[i].text = target.positiveQuirks[i].Description;
                }
                break;
            default:
                break;
        }

        switch (target.negativeQuirks.Count)
        {
            case 0:
                for (int i = 0; i < negativeQuirkTitles.Length; i++)
                {
                    this.negativeQuirkTitles[i].text = "";
                    this.negativeQuirkDes[i].text = "";
                }
                break;
            case 1:
                this.negativeQuirkTitles[0].text = target.negativeQuirks[0].Name;
                this.negativeQuirkDes[0].text = target.negativeQuirks[0].Description;
                this.negativeQuirkTitles[1].text = "";
                this.negativeQuirkDes[1].text = "";
                break;
            case 2:
                for (int i = 0; i < positiveQuirkTitles.Length; i++)
                {
                    this.negativeQuirkTitles[i].text = target.negativeQuirks[i].Name;
                    this.negativeQuirkDes[i].text = target.negativeQuirks[i].Description;
                }
                break;
            default:
                break;
        }

    }
}
