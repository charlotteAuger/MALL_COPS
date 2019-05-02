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
    [SerializeField] private LayerMask floorScanMask;
    [SerializeField] private float walkableMaxAngle;
    private Vector3 moveForward;

    [Header("Tackle")]
    [SerializeField] private float chargeSpeed;
    [SerializeField] private float chargeInputLerp;
    [SerializeField] private float chargeVibrationStep;
    [SerializeField] private float tackleTime;
    [SerializeField] private float tackleSpeed;
    [SerializeField] private float tackleRecovery;
    private float chargeVibrationTime;

    [Header("References")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject tackleHitbox;
    private Coroutine tackleCor;
    [SerializeField] private GameObject fov;

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
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3 (rb.velocity.x, 0, rb.velocity.z).normalized, Vector3.up), rotationFactor);
                break;
        }
    }

    private void MovementUpdate(Vector2 _inputDirection)
    {
        movementDirection = new Vector3(_inputDirection.x, 0, _inputDirection.y);
        Vector3 velocity;

        moveForward = GetForwardFromSlopeNormal(false);
        Debug.DrawLine(transform.position, transform.position + moveForward * 2, Color.magenta);

        switch(state)
        {
            case PlayerStates.NORMAL:
                velocity = moveForward * moveSpeed;
                //velocity.y = rb.velocity.y;
                rb.velocity = velocity;
                break;

            case PlayerStates.CHARGING:
                velocity = (moveForward != Vector3.zero ? moveForward * chargeSpeed : rb.velocity);
                //velocity.y = rb.velocity.y;
                rb.velocity = Vector3.Lerp(rb.velocity, velocity, chargeInputLerp);

                //Footstep vibrations
                if (chargeVibrationTime >= chargeVibrationStep)
                {
                    GameManager.Instance.vibro.VibrateFor(.1f, index-1, .5f, .3f);
                    chargeVibrationTime = 0;
                }
                chargeVibrationTime += Time.fixedDeltaTime;

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
        rb.velocity = (moveForward != Vector3.zero ? moveForward : GetForwardFromSlopeNormal(true)) * chargeSpeed;
        tackleHitbox.SetActive(true);
        GameManager.Instance.vibro.VibrateFor(.1f, index - 1, .3f, .1f);
        chargeVibrationTime = 0;
    }

    private void OnTackleReleased()
    {
        if (state == PlayerStates.CHARGING)
        {
            state = PlayerStates.TACKLING;
            tackleCor = StartCoroutine(Tackle());
        }
    }

    private Vector3 GetForwardFromSlopeNormal(bool _usingRotation)
    {
        Debug.DrawLine(transform.position, transform.position + -transform.up * .5f, Color.red);
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, .5f, floorScanMask))
        {
            if (Mathf.Abs(Vector3.Angle(transform.up, hit.normal)) < walkableMaxAngle)
            {
                Vector3 right = _usingRotation ? transform.right : Vector3.Cross(transform.up, movementDirection);
                return Vector3.Cross(right, hit.normal);
            }
        }

        return (_usingRotation ? transform.forward : movementDirection);
    }

    IEnumerator Tackle()
    {
        Vector3 velocity = GetForwardFromSlopeNormal(true) * tackleSpeed;
        //velocity.y = rb.velocity.y;
        rb.velocity = velocity;
        yield return new WaitForSeconds(tackleTime);
        rb.velocity = /*new Vector3(0, rb.velocity.y, 0);*/ Vector3.zero;
        tackleHitbox.SetActive(false);
        GameManager.Instance.shaker.SetTrauma(.5f, .2f, 7f, 3f);
        yield return new WaitForSeconds(tackleRecovery);
        state = PlayerStates.NORMAL;
    }

    IEnumerator StopTackle()
    {
        state = PlayerStates.TACKLING;
        GameManager.Instance.shaker.SetTrauma(.5f, .2f, 10f, 3f);
        GameManager.Instance.vibro.VibrateFor(.1f, index-1, .4f, 1f);
        //GameManager.Instance.fovBooster.SetFOV(55, 0.9f);
        rb.velocity = /*new Vector3(0, rb.velocity.y, 0);*/ Vector3.zero;
        tackleHitbox.SetActive(false);
        yield return new WaitForSeconds(tackleRecovery);
        state = PlayerStates.NORMAL;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (state == PlayerStates.CHARGING || state == PlayerStates.TACKLING)
        {
            if (tackleCor != null)
                StopCoroutine(tackleCor);

            StartCoroutine(StopTackle());

            if (tag == "Civilian")
            {
                Debug.Log("It's a civilian!");
            }
        }
    }

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