using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookingAtIPState : State
{
    public PointOfInterest ip;

    public override void OnStateEnter(AIController aiController)
    {
        //get timing
        base.OnStateEnter(aiController);
    }

    public override void OnStateExit(AIController aiController)
    {
        base.OnStateExit(aiController);
    }

    public override State StateEffect(AIController aiController, float dt)
    {
        //wait
        //then choose if going in or not
        return base.StateEffect(aiController, dt);
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
