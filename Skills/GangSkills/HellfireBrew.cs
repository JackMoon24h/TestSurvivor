using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellfireBrew : BaseSkill 
{
    public override void Excute(GameObject target)
    {
        base.Excute(target);

        targetActor.TakeDamage(this.dmgMode + PlayerManager.instance.activeCharacter.Damage);
    }
}
