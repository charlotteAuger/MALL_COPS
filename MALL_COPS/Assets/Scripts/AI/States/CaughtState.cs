using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaughtState : State
{
    public override void OnStateEnter(AIController aiController)
    {
        base.OnStateEnter(aiController);
    }

    public override void OnStateExit(AIController aiController)
    {
        base.OnStateExit(aiController);
    }

    public override State StateEffect(AIController aiController, float dt)
    {
        return base.StateEffect(aiController, dt);
    }

    public override State OnSeeTackle(AIController aiController, Vector3 tacklePosition)
    {
        return null;
    }

    public override State OnTackled(AIController aiController)
    {
        return null;
    }
}
