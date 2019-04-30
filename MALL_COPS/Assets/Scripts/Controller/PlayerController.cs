using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStates { NORMAL, CHARGING, TACKLING }

public class PlayerController : MonoBehaviour
{
    public int index;
    public PlayerStates state;

    Vector3 movementDirection;
    Vector3 lookDirection;

    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationFactor;

    [Header("Tackle")]
    [SerializeField] private float chargeSpeed;
    [SerializeField] private float chargeInputLerp;
    [SerializeField] private float tackleTime;
    [SerializeField] private float tackleSpeed;
    [SerializeField] private float tackleRecovery;

    [Header("References")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject tackleHitbox;
    private Coroutine tackleCor;

    private void Start()
    {
        if (index == 1)
        {
            InputManager.Instance.MoveInput_1 += MovementUpdate;
            InputManager.Instance.LookInput_1 += RotationUpdate;
            InputManager.Instance.TacklePressed_1 += OnTacklePressed;
            InputManager.Instance.TackleReleased_1 += OnTackleReleased;
        }
        else if (index == 2)
        {
            InputManager.Instance.MoveInput_2 += MovementUpdate;
            InputManager.Instance.LookInput_2 += RotationUpdate;
            InputManager.Instance.TacklePressed_2 += OnTacklePressed;
            InputManager.Instance.TackleReleased_2 += OnTackleReleased;
        }
    }

    private void Update()
    {
        switch(state)
        {        
            case PlayerStates.CHARGING:
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(rb.velocity.normalized, Vector3.up), rotationFactor);
                break;
        }
    }

    private void MovementUpdate(Vector2 _inputDirection)
    {
        movementDirection = new Vector3(_inputDirection.x, 0, _inputDirection.y);
        switch(state)
        {
            case PlayerStates.NORMAL:
                rb.velocity = movementDirection * moveSpeed;
                break;

            case PlayerStates.CHARGING:
                rb.velocity = Vector3.Lerp(rb.velocity, (movementDirection != Vector3.zero ? movementDirection * chargeSpeed : rb.velocity), chargeInputLerp);
                break;

            case PlayerStates.TACKLING:
                break;
        }
    }

    private void RotationUpdate(Vector2 inputDirection)
    {
        lookDirection = new Vector3(inputDirection.x, 0, inputDirection.y);

        switch (state)
        {
            case PlayerStates.NORMAL:
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection, Vector3.up), rotationFactor);
                break;
        }
    }

    private void OnTacklePressed()
    {
        state = PlayerStates.CHARGING;
        rb.velocity = (movementDirection != Vector3.zero ? movementDirection : transform.forward) * chargeSpeed;
        tackleHitbox.SetActive(true);
    }

    private void OnTackleReleased()
    {
        if (state == PlayerStates.CHARGING)
        {
            state = PlayerStates.TACKLING;
            tackleCor = StartCoroutine(Tackle());
        }
    }

    IEnumerator Tackle()
    {
        rb.velocity = transform.forward * tackleSpeed;
        yield return new WaitForSeconds(tackleTime);
        rb.velocity = Vector3.zero;
        tackleHitbox.SetActive(false);
        yield return new WaitForSeconds(tackleRecovery);
        state = PlayerStates.NORMAL;
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (state == PlayerStates.CHARGING || state == PlayerStates.TACKLING)
    //    {
    //        if (tackleCor != null)
    //            StopCoroutine(tackleCor);



    //        if (tag == "Civilian")
    //        {
    //            Debug.Log("It's a civilian!");
    //        }
    //    }

    //}

    private void OnDestroy()
    {
        if (index == 1)
        {
            InputManager.Instance.MoveInput_1 -= MovementUpdate;
            InputManager.Instance.LookInput_1 -= RotationUpdate;
            InputManager.Instance.TacklePressed_1 -= OnTacklePressed;
            InputManager.Instance.TackleReleased_1 -= OnTackleReleased;
        }
        else if (index == 2)
        {
            InputManager.Instance.MoveInput_2 -= MovementUpdate;
            InputManager.Instance.LookInput_2 -= RotationUpdate;
            InputManager.Instance.TackleReleased_2 -= OnTackleReleased;
        }
    }
}