using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MadCannon : BaseSkill
{

    public override void Excute(Actor attacker, GameObject target)
    {
        base.Excute(attacker, target);

        // 1. Main Effect with character's basic action
        targetActor.TakeDamage(this.dmgMode + PlayerManager.instance.activeCharacter.Damage);

        // 2. SubEffect
        attacker.TakeEffect(attacker, false, 0, true, PhysicalEffectType.Move, 0, 1);
        targetActor.TakeEffect(attacker, false, 0, true, PhysicalEffectType.Move, 0, 1);
    }
}
