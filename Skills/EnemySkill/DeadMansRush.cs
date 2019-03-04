using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadMansRush : BaseSkill 
{
    public int mentalDmgMode = 4;
    public bool hasMentalDMG;

    public override void Excute(Actor attacker, GameObject target)
    {
        base.Excute(attacker, target);

        // 1. Main Effect with character's basic action
        targetActor.TakeDamage(this.dmgMode + EnemyManager.instance.activeCharacter.Damage);

        // 2. Sub Effects
        targetActor.TakeEffect(attacker, true, mentalDmgMode, true, PhysicalEffectType.Bleed, 2, 2);
    }
}
