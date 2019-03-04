using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShroudStep : BaseSkill
{

    public override void Excute(Actor attacker, GameObject target)
    {
        base.Excute(attacker, target);

        // 1. Main Effect with character's basic action
        targetActor.TakeDamage(this.dmgMode + PlayerManager.instance.activeCharacter.Damage);


        // 2. SubEffect
        targetActor.TakeEffect(attacker, false, 0, false, PhysicalEffectType.Stun, 1, 1);
        attacker.TakeEffect(attacker, false, 0, true, PhysicalEffectType.Move, 0, 1);
    }
}
