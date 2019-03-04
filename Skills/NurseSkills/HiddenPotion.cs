using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenPotion : BaseSkill
{

    public override void Excute(Actor attacker, GameObject target)
    {
        base.Excute(attacker, target);

        // 1. Main Effect with character's basic action

        int rand = Random.Range(4, 9);

        targetActor.TakeHeal(rand);

        // 2. SubEffect
        float chance = Random.Range(0, 1f);
        if(chance <= 0.3)
        {
            targetActor.TakeEffect(attacker, true, 4, false, PhysicalEffectType.Move, 2, 3);
        }
    }
}
