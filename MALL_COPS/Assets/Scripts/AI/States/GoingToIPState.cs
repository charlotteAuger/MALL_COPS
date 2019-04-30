using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoingToIPState : State
{
    public Vector3[] path;
    private float speed;

    public override void OnStateEnter(AIController aiController)
    {
        speed = Random.Range(aiController.stats.walkSpeed_min, aiController.stats.walkSpeed_min);

        //Aquire path
        base.OnStateEnter(aiController);
    }

    public override void OnStateExit(AIController aiController)
    {
        
        base.OnStateExit(aiController);
    }

    public override State StateEffect(AIController aiController, float dt)
    {
        //Follow path
        //if arrived : switch to looking
        return base.StateEffect(aiController, dt);
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
