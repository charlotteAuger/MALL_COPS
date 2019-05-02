using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeingState : State
{
    public PointOfInterest ip;
    private GameObject stolenItem;
    private Vector3[] path;
    private int currentWP = 0;

    public override void OnStateEnter(AIController aiController)
    {
        //ALARM
        speed = aiController.stats.fleeingSpeed;
        aiController.SetupStolenItem(ip);

        aiController.SetPresence(true);

        Vector3 target = AIManager.instance.GetSafestExit(aiController.transform.position);
        path = aiController.GetPath(target);
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
        aiController.DropStolenItem(stolenItem);
        TackledState s = new TackledState();
        s.wasRobbing = true;
        return s;
    }
}
