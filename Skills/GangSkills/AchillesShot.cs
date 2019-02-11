﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchillesShot : BaseSkill 
{
    public override void Excute(Actor attacker, GameObject target)
    {
        base.Excute(attacker, target);

        // 1. Main Effect with character's basic action
        targetActor.TakeDamage(this.dmgMode + PlayerManager.instance.activeCharacter.Damage);

        // 2. SubEffect
        targetActor.TakeEffect(PhysicalEffectType.Bleed, 2, 3);
    }
}
