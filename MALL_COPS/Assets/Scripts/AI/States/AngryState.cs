using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngryState : State
{
    private float angerLevel;

    public override void OnStateEnter(AIController aiController)
    {
        //animation
        angerLevel = aiController.stats.angerTime;
        base.OnStateEnter(aiController);
    }

    public override void OnStateExit(AIController aiController)
    { 
        base.OnStateExit(aiController);
    }

    public override State StateEffect(AIController aiController, float dt)
    {
        aiController.LookTowards(AIManager.instance.GetClosestPlayerPosition(aiController.transform.position));
        if (aiController.watched)
        {
            angerLevel -= dt * aiController.watchable.peopleWatching.Count;
        }
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