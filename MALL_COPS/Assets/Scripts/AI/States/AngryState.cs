using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngryState : State
{
    private float angerLevel = 1;

    public override void OnStateEnter(AIController aiController)
    {
        //animation
        base.OnStateEnter(aiController);
    }

    public override void OnStateExit(AIController aiController)
    {
        //stop animation ?
        base.OnStateExit(aiController);
    }

    public override State StateEffect(AIController aiController, float dt)
    {
        aiController.LookTowards(AIManager.instance.GetClosestPlayerPosition(aiController.transform.position));
        if (angerLevel <= 0)
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