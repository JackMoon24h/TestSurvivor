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

    public Text positiveQuirk1;
    public Text positiveQuirk1Des;
    public Text positiveQuirk2;
    public Text positiveQuirk2Des;

    public Text negativeQuirk1;
    public Text negativeQuirk1Des;
    public Text negativeQuirk2;
    public Text negativeQuirk2Des;

    public override void OpenWindow()
    {
        if(CanOpen())
        {
            UpdateCard(PlayerManager.instance.activeCharacter);
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
            affliction.text = target.virtuousEffect.name;
            afflictionDes.text = target.virtuousEffect.Description;
        }
        else
        {
            affliction.text = "Affliction";
            afflictionDes.text = "None";
        }

    }
}
