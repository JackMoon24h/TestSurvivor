using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaSpirit : BaseSkill
{

    public override void Excute(Actor attacker, GameObject target)
    {
        base.Excute(attacker, target);

        // 1. Main Effect with character's basic action
        int rand = Random.Range(16, 23);

        targetActor.TakeMentalCure(rand);

        // 2. SubEffect
        targetActor.TakeEffect(attacker, false, 0, false, PhysicalEffectType.Move, 2, 3);
    }
}
