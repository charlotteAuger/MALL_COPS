using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    [SerializeField] private float inputThreshold;
    x360_Gamepad gamepad_1;
    x360_Gamepad gamepad_2;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);
    }

    public delegate void InputEvent(Vector2 inputValues);
    public static event InputEvent FirstPlayerMoveInput;
    public static event InputEvent FirstPlayerLookInput;
    public static event InputEvent SecondPlayerMoveInput;
    public static event InputEvent SecondPlayerLookInput;

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
            //if (Mathf.Abs(xInput_1) > inputThreshold || Mathf.Abs(yInput_1) > inputThreshold)
            //{
                inputDirection = new Vector2(xInput_1, yInput_1);
                FirstPlayerMoveInput(inputDirection);
            //}

            float xLookInput_1 = gamepad_1.GetStick_R().X;
            float yLookInput_1 = gamepad_1.GetStick_R().Y;
            if (Mathf.Abs(xLookInput_1) > inputThreshold || Mathf.Abs(yLookInput_1) > inputThreshold)
            {
                inputDirection = new Vector2(xLookInput_1, yLookInput_1);
                FirstPlayerLookInput(inputDirection);
            }
        }

        if (gamepad_2 != null)
        {
            float xInput_2 = gamepad_2.GetStick_L().X;
            float yInput_2 = gamepad_2.GetStick_L().Y;
            //if (Mathf.Abs(xInput_2) > inputThreshold || Mathf.Abs(yInput_2) > inputThreshold)
            //{
                inputDirection = new Vector2(xInput_2, yInput_2);
                SecondPlayerMoveInput(inputDirection);
            //}

            float xLookInput_2 = gamepad_2.GetStick_R().X;
            float yLookInput_2= gamepad_2.GetStick_R().Y;
            if (Mathf.Abs(xLookInput_2) > inputThreshold || Mathf.Abs(yLookInput_2) > inputThreshold)
            {
                inputDirection = new Vector2(xLookInput_2, yLookInput_2);
                SecondPlayerLookInput(inputDirection);
            }
        }

    }

    private void Test(Vector2 inputValues)
    {
        print(inputValues);
    }
}
