using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaughtState : State
{
    private float duration;
    public override void OnStateEnter(AIController aiController)
    {
        duration = 1f;
    }

    public override void OnStateExit(AIController aiController)
    {
      
    }

    public override State StateEffect(AIController aiController, float dt)
    {
        t += dt;
        if (t >= duration)
        {
            aiController.SetPresence(false);
        }
        return null;
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
