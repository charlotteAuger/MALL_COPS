using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookingAtTackleState : State
{
    public Vector3 targetPosition;
    private float duration;

    public override void OnStateEnter(AIController aiController)
    {
        aiController.anim.SetTrigger("surprised");
        
        duration = Random.Range(aiController.stats.tackleLookingTime_min, aiController.stats.tackleLookingTime_max);
    }

    public override void OnStateExit(AIController aiController)
    {
        
        base.OnStateExit(aiController);
    }

    public override State StateEffect(AIController aiController, float dt)
    {
        t += dt;
        aiController.LookTowards(targetPosition);

        if (t >= duration)
        {
            return new GoingToIPState();
        }

        return null;
    }

    public override State OnSeeTackle(AIController aiController, Vector3 tacklePosition)
    {
        return base.OnSeeTackle(aiController, tacklePosition);
    }

    public override State OnTackled(AIController aiController)
    {
        return base.OnTackled(aiController);
    }
}
