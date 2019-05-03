using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngryState : State
{
    private float angerLevel;

    public override void OnStateEnter(AIController aiController)
    {
        angerLevel = aiController.stats.angerTime;
        aiController.angryRend.enabled = true;
        aiController.angryRend.materials[0].SetFloat("AngerValue", 0f);
        base.OnStateEnter(aiController);
    }

    public override void OnStateExit(AIController aiController)
    {
        aiController.angryRend.enabled = false;
    }

    public override State StateEffect(AIController aiController, float dt)
    {
        aiController.LookTowards(AIManager.instance.GetClosestPlayerPosition(aiController.transform.position));
        if (aiController.watched)
        {
            angerLevel -= dt * aiController.watchable.peopleWatching.Count;
            aiController.angryRend.materials[0].SetFloat("_AngerValue", 1-angerLevel / aiController.stats.angerTime);
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