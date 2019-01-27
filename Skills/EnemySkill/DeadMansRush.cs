using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadMansRush : BaseSkill 
{
    public int mentalDmgMode;

    public override void Excute(GameObject target)
    {
        base.Excute(target);

        targetActor.ReceiveDamage(this.dmgMode + EnemyManager.instance.activeCharacter.Damage);
    }
}
