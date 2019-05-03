using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookingAtIPState : State
{
    public PointOfInterest ip;
    private float duration;

    public override void OnStateEnter(AIController aiController)
    {
        duration = Random.Range(aiController.stats.ipLookingTime_min, aiController.stats.ipLookingTime_max);
        base.OnStateEnter(aiController);
    }

    public override void OnStateExit(AIController aiController)
    {
        base.OnStateExit(aiController);
    }

    public override State StateEffect(AIController aiController, float dt)
    {
        t += dt;
        aiController.LookTowards(ip.transform.position);

        if (t >= duration)
        {
            int r = Random.Range(0, 3);

            if (r == 0)
            {
                return new GoingToIPState();
            }
            else
            {
                InShopState s = new InShopState();
                s.ip = ip;
                return s;
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
