using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : PhysicalEffect 
{
    public override void SetEffect(int power, int duration, Actor target)
    {
        base.SetEffect(power, duration, target);
        physicalEffectType = PhysicalEffectType.Move;
        m_isSkipTurn = false;

        SoundManager.Instance.PlaySE(10);

        if (owner is BaseCharacter)
        {
            if(Commander.instance.turnStateMachine.currentTurn == TurnStateMachine.Turn.PLAYER && owner.activeCommand.skillName != "Mad Cannon")
            {
                // Move forward
                int proceed = owner.Position - 1;
                if (owner.Position == 1)
                {
                    proceed = 4;
                }
                PlayerManager.instance.Swap(PlayerManager.instance.characterList, owner.Position, proceed);
                return;
            }

            // Move backward
            int swapPos = owner.Position + 1;
            if(PlayerManager.instance.characterList.Count < swapPos)
            {
                swapPos = 1;
            }
            PlayerManager.instance.Swap(PlayerManager.instance.characterList, owner.Position, swapPos);
        }
        else
        {
            int swapPos = owner.Position + 1;
            if (EnemyManager.instance.characterList.Count < swapPos)
            {
                swapPos = 1;
            }
            EnemyManager.instance.Swap(EnemyManager.instance.characterList, owner.Position, swapPos);
        }
    }
}
