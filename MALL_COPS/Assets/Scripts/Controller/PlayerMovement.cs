using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody rB;
    [SerializeField] private float speed;
    [SerializeField] private float rotationFactor;

    private void Awake()
    {
        InputManager.FirstPlayerMoveInput += MovementUpdate;
        InputManager.FirstPlayerLookInput += RotationUpdate;
    }

    private void MovementUpdate(Vector2 inputDirection)
    {
        Vector3 movementDirection = new Vector3(inputDirection.x, 0, inputDirection.y);
        rB.velocity = movementDirection * speed;
    }

    private void RotationUpdate(Vector2 inputDirection)
    {
        Vector3 movementDirection = new Vector3(inputDirection.x, 0, inputDirection.y);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movementDirection, Vector3.up), rotationFactor);
    }
}
