using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLock : BaseSkill 
{
    public override void Excute(Actor attacker, GameObject target)
    {
        base.Excute(attacker, target);

        // 1. Main Effect with character's basic action
        targetActor.TakeDamage(this.dmgMode + PlayerManager.instance.activeCharacter.Damage);

        // 2. SubEffect
        targetActor.TakeEffect(attacker, false, 0, false, PhysicalEffectType.Bleed, 2, 3);
    }


}
