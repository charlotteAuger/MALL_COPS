using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    [SerializeField] private float inputThreshold;

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

    private void FixedUpdate()
    {
        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");
        if (Mathf.Abs(xInput) > inputThreshold || Mathf.Abs(yInput) > inputThreshold)
        {
            Vector2 inputDirection = new Vector2(xInput, yInput);
            FirstPlayerMoveInput(inputDirection);
        }

        float xLookInput = Input.GetAxis("LookHorizontal");
        float yLookInput = Input.GetAxis("LookVertical");
        if (Mathf.Abs(xLookInput) > inputThreshold || Mathf.Abs(yLookInput) > inputThreshold)
        {
            Vector2 inputDirection = new Vector2(xLookInput, yLookInput);
            FirstPlayerLookInput(inputDirection);
        }
    }

    private void Test(Vector2 inputValues)
    {
        print(inputValues);
    }
}
