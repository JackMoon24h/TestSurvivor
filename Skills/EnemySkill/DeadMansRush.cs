using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadMansRush : BaseSkill 
{
    public int mentalDmgMode;

    public override void Excute(GameObject target)
    {
        base.Excute(target);

        // 1. Main Effect with character's basic action
        targetActor.TakeDamage(this.dmgMode + EnemyManager.instance.activeCharacter.Damage);

        // 2. SubEffect
        targetActor.TakePhysicalEffect(PhysicalEffectType.Infect, 3, 2);
    }
}
