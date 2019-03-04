using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JuliasSong : BaseSkill
{

    public override void Excute(Actor attacker, GameObject target)
    {
        base.Excute(attacker, target);

        // 1. Main Effect with character's basic action
        int rand = Random.Range(3, 9);

        targetActor.TakeMentalCure(rand);

        // 2. SubEffect
        targetActor.TakeEffect(attacker, false, 0, false, PhysicalEffectType.Move, 2, 3);
    }
}
