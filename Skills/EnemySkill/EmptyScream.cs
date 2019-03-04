using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyScream : BaseSkill
{

    public int mentalDmgMode = 12;
    public bool hasMentalDMG;

    public override void Excute(Actor attacker, GameObject target)
    {
        base.Excute(attacker, target);

        // 1. Main Effect with character's basic action
        targetActor.TakeDamage(this.dmgMode + EnemyManager.instance.activeCharacter.Damage);

        // 2. Sub Effects

        int rand = Random.Range(mentalDmgMode - 2, mentalDmgMode + 3);
        targetActor.TakeEffect(attacker, true, rand, false, PhysicalEffectType.Infect, 3, 2);
    }
}
