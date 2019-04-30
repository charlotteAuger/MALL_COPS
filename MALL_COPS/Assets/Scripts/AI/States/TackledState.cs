using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TackledState : State
{
    public bool wasRobbing;
    public float duration;

    public override void OnStateEnter(AIController aiController)
    {
        //animation
        //stop velocity
        base.OnStateEnter(aiController);
    }

    public override void OnStateExit(AIController aiController)
    {
        base.OnStateExit(aiController);
    }

    public override State StateEffect(AIController aiController, float dt)
    {
        t += dt;
        if (t >= duration)
        {
            if (wasRobbing)
            {
                return new CaughtState();
            }
            else
            {
                return new AngryState();
            }
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