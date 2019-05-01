using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State// : MonoBehaviour
{
    public float t;
    public float speed = 2f;

    public virtual void OnStateEnter(AIController aiController)
    {

    }

    public virtual State StateEffect(AIController aiController, float dt)
    {
        return null;
    }

    public virtual void OnStateExit(AIController aiController)
    {

    }

    public virtual State OnSeeTackle(AIController aiController, Vector3 tacklePosition)
    {
        LookingAtTackleState s = new LookingAtTackleState();
        s.targetPosition = tacklePosition;
        return s;
    }

    public virtual State OnTackled(AIController aiController) //inform tackling player
    {
        return new TackledState();
    }
}
