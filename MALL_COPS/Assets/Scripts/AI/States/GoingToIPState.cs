using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoingToIPState : State
{
    public Vector3[] path;
    private PointOfInterest ip;
    private int currentWP = 0;

    public override void OnStateEnter(AIController aiController)
    {
        speed = Random.Range(aiController.stats.walkSpeed_min, aiController.stats.walkSpeed_min);
        int i = 0;
        while (path == null && i < 10)
        {
            ip = AIManager.instance.GetAnIP(aiController.transform.position);
            path = aiController.GetPath(ip.transform.position);
            i++;
        }
    }

    public override void OnStateExit(AIController aiController)
    {
        
        base.OnStateExit(aiController);
    }

    public override State StateEffect(AIController aiController, float dt)
    {

        float remainingDistance = Vector3.Distance(path[currentWP], aiController.transform.position);

        if (currentWP == path.Length - 1)
        {
            if (remainingDistance <= aiController.stats.shopLookingDistance)
            {
                aiController.StopMovement();
                LookingAtIPState s = new LookingAtIPState();
                s.ip = ip;
                return s;
            }
        }
        else
        {
            if (remainingDistance <= aiController.stats.stopDistance)
            {
                currentWP++;
            }
        }

        aiController.MoveTowards(path[currentWP], speed);
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
