using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class MainPanel : MonoBehaviour 
{
    public Character character;
    GameManager gameManager;

    [HideInInspector]public Text[] unitInfo = new Text[18];
    [HideInInspector]public Image thumb;
    Image skill1;
    Image skill2;
    Image skill3;
    Image skill4;
    Image skill5;

    /*
     * unitInfo[18]
     * 
    0.  health;
    1.  mental;
    2.  damage;
    3.  protection;
    4.  endurance;
    5.  speed;
    6.  accuracy;
    7.  dodge;
    8. critical;
    9. virtue;
    10. stressRes;
    11. bleedRes;
    12. infectRes;
    13. stunRes;
    14. moveRes;
    15. deathBlow;
    16. name
    17. job
    ------------------------------ OUT OF unitInfo --------------------------------
    18. thumb
    19. Skill1
    20. Skill2
    21. Skill3
    22. Skill4
    23. Skill5
    */

    private void Awake()
    {
        gameManager = Object.FindObjectOfType<GameManager>().GetComponent<GameManager>();
        thumb = this.transform.GetChild(18).GetComponent<Image>();
        skill1 = this.transform.GetChild(19).GetComponent<Image>();
        skill2 = this.transform.GetChild(20).GetComponent<Image>();
        skill3 = this.transform.GetChild(21).GetComponent<Image>();
        skill4 = this.transform.GetChild(22).GetComponent<Image>();
        skill5 = this.transform.GetChild(23).GetComponent<Image>();

        for (int i = 0; i < unitInfo.Length; i++)
        {
            unitInfo[i] = this.gameObject.transform.GetChild(i).GetComponent<Text>(); 
        }

    }


    public void AssignCharacter(GameObject child)
    {
        character = child.GetComponent<Character>();
        CreateImage(character);
    }

    // Create character's thumb, skill images which will be displayed in the panel
    public void CreateImage(Character target)
    {
        thumb.sprite = target.thumbImage;
        skill1.sprite = target.skillSprite1;
        skill2.sprite = target.skillSprite2;
        skill3.sprite = target.skillSprite3;
        skill4.sprite = target.skillSprite4;
        skill5.sprite = target.skillSprite5;
    }

    public void UpdatePanel()
    {
        unitInfo[0].text = character.Health.ToString() + " / " + character.MaxHealth.ToString();
        unitInfo[1].text = character.Mental.ToString() + " / " + character.MaxMental.ToString();
        unitInfo[2].text = character.Damage.ToString();
        unitInfo[3].text = character.Protection.ToString();
        unitInfo[4].text = character.Endurance.ToString();
        unitInfo[5].text = character.Speed.ToString();
        unitInfo[6].text = character.Accuracy.ToString();
        unitInfo[7].text = character.Dodge.ToString();
        unitInfo[8].text = character.Critical.ToString();
        unitInfo[9].text = character.Virtue.ToString();
        unitInfo[10].text = character.StressRes.ToString();
        unitInfo[11].text = character.BleedRes.ToString();
        unitInfo[12].text = character.InfectRes.ToString();
        unitInfo[13].text = character.StunRes.ToString();
        unitInfo[14].text = character.MoveRes.ToString();
        unitInfo[15].text = character.DeathBlow.ToString();
        unitInfo[16].text = character.Name;
        unitInfo[17].text = character.job.ToString();
    }

}
