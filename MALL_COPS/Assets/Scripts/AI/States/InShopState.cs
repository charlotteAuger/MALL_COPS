using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InShopState : State
{
    public PointOfInterest ip;
    private float duration;
    private bool isIn;

    public override void OnStateEnter(AIController aiController)
    {
        aiController.SetCollider(false);
        duration = Random.Range(aiController.stats.inShopTime_min, aiController.stats.inShopTime_max);
        base.OnStateEnter(aiController);
    }

    public override void OnStateExit(AIController aiController)
    {
        base.OnStateExit(aiController);
    }

    public override State StateEffect(AIController aiController, float dt)
    {
        if (!isIn)
        {
            float remainingDistance = new Vector3(ip.transform.position.x- aiController.transform.position.x, 0, ip.transform.position.z - aiController.transform.position.z).magnitude;
            if (remainingDistance <= aiController.stats.stopDistance)
            {
                aiController.SetPresence(false);
                isIn = true;
                return null;
            }
            else
            {
                aiController.MoveTowards(ip.transform.position, speed);
                return null;
            }
        }
        else
        {
            t += dt;
            if (t >= duration)
            {
                //If place is available
                if (aiController.willRob)
                {
                    FleeingState s = new FleeingState();
                    s.ip = ip;
                    return s;
                }
                else
                {
                    aiController.SetPresence(true);
                    return new GoingToIPState();
                }
            }
            else
            {
                return null;
            }
        }
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