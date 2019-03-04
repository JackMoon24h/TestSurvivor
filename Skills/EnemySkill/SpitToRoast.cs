using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitToRoast : BaseSkill 
{

    public int mentalDmgMode = 6;
    public bool hasMentalDMG;

    public override void Excute(Actor attacker, GameObject target)
    {
        base.Excute(attacker, target);

        // 1. Main Effect with character's basic action
        targetActor.TakeDamage(this.dmgMode + EnemyManager.instance.activeCharacter.Damage);

        // 2. Sub Effects
        targetActor.TakeEffect(attacker, false, mentalDmgMode, true, PhysicalEffectType.Stun, 1, 1);
    }
}
