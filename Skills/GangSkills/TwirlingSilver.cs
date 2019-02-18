using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwirlingSilver : BaseSkill 
{
    public override void Excute(Actor attacker, GameObject target)
    {
        base.Excute(attacker, target);

        // 1. Main Effect with character's basic action
        targetActor.TakeDamage(this.dmgMode + PlayerManager.instance.activeCharacter.Damage);

        targetActor.TakeEffect(attacker, false, 0, true, PhysicalEffectType.Bleed, 3, 2);
    }


}
