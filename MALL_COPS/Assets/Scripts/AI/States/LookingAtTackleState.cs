using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookingAtTackleState : State
{
    public Vector3 targetPosition;
    private float lookingDuration;

    public override void OnStateEnter(AIController aiController)
    {
        //Play animation maybe ?
        //Pick random duration
        base.OnStateEnter(aiController);
    }

    public override void OnStateExit(AIController aiController)
    {
        
        base.OnStateExit(aiController);
    }

    public override State StateEffect(AIController aiController, float dt)
    {
        t += dt;

        if (t >= lookingDuration)
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
