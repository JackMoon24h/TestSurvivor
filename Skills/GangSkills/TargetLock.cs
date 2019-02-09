using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLock : BaseSkill 
{
    public override void Excute(GameObject target)
    {
        base.Excute(target);

        // 1. Main Effect with character's basic action
        targetActor.TakeDamage(this.dmgMode + PlayerManager.instance.activeCharacter.Damage);

        // 2. SubEffect
        targetActor.TakePhysicalEffect(PhysicalEffectType.Bleed, 2, 3);
    }


}
