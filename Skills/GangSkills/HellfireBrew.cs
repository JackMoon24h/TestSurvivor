﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellfireBrew : BaseSkill {

    public override void Excute(GameObject target)
    {
        base.Excute(target);
        Debug.Log(skillName + " excuted");

        var enemy = target.GetComponent<BaseEnemy>();

        enemy.ReceiveDamage(this.damage + PlayerManager.instance.activeCharacter.Damage);
    }
}
