using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    [SerializeField] private float inputThreshold;
    x360_Gamepad gamepad_1;
    x360_Gamepad gamepad_2;
    
    public delegate void AxisEvent(Vector2 inputValues);
    public delegate void TriggerEvent();

    public event AxisEvent MoveInput_1;
    public event AxisEvent MoveInput_2;
    public event AxisEvent LookInput_1;
    public event AxisEvent LookInput_2;

    public event TriggerEvent TacklePressed_1;
    public event TriggerEvent TacklePressed_2;
    public event TriggerEvent TackleReleased_1;
    public event TriggerEvent TackleReleased_2;
    bool holdingRTrigger_1;
    bool holdingRTrigger_2;


    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this.gameObject);
    }

    private void Start()
    {
        gamepad_1 = GamepadManager.Instance.GetGamepad(1);
        gamepad_2 = GamepadManager.Instance.GetGamepad(2);
    }

    private void FixedUpdate()
    {
        Vector2 inputDirection;
        if (gamepad_1 != null)
        {
            float xInput_1 = gamepad_1.GetStick_L().X;
            float yInput_1 = gamepad_1.GetStick_L().Y;
            inputDirection = new Vector2(xInput_1, yInput_1);
            MoveInput_1?.Invoke(inputDirection);

            float xLookInput_1 = gamepad_1.GetStick_R().X;
            float yLookInput_1 = gamepad_1.GetStick_R().Y;
            if (Mathf.Abs(xLookInput_1) > inputThreshold || Mathf.Abs(yLookInput_1) > inputThreshold)
            {
                inputDirection = new Vector2(xLookInput_1, yLookInput_1);
                LookInput_1?.Invoke(inputDirection);
            }

            if (gamepad_1.GetTrigger_R() > 0.1f && !holdingRTrigger_1)
            {
                TacklePressed_1?.Invoke();
                holdingRTrigger_1 = true;
            }
            else if (gamepad_1.GetTrigger_R() <= 0.1f && holdingRTrigger_1)
            {
                TackleReleased_1?.Invoke();
                holdingRTrigger_1 = false;
            }
        }

        if (gamepad_2 != null)
        {
            float xInput_2 = gamepad_2.GetStick_L().X;
            float yInput_2 = gamepad_2.GetStick_L().Y;
            inputDirection = new Vector2(xInput_2, yInput_2);
            MoveInput_2?.Invoke(inputDirection);

            float xLookInput_2 = gamepad_2.GetStick_R().X;
            float yLookInput_2= gamepad_2.GetStick_R().Y;
            if (Mathf.Abs(xLookInput_2) > inputThreshold || Mathf.Abs(yLookInput_2) > inputThreshold)
            {
                inputDirection = new Vector2(xLookInput_2, yLookInput_2);
                LookInput_2?.Invoke(inputDirection);
            }

            if (gamepad_1.GetTrigger_R() > 0.1f && !holdingRTrigger_2)
            {
                TacklePressed_2?.Invoke();
                holdingRTrigger_2 = true;
            }
            else if (holdingRTrigger_2)
            {
                TackleReleased_2?.Invoke();
                holdingRTrigger_2 = false;
            }
        }
    }

    private void Test(Vector2 inputValues)
    {
        print(inputValues);
    }
}
