using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlayerNumber
{
    Player1,
    Player2,
    Player3,
    Player4,
}

public enum InputMethod
{
    Keyboard,
    XBox,
    PS4,
    NitendoSwitch
}

public class Player : Character
{
    //[SerializeField]
    //PlayerNumber playerNumber = PlayerNumber.Player1;

    //[SerializeField]
    //InputMethod inputMethod = InputMethod.Keyboard;
    [SerializeField]
    SelectingBehaviour enemySelecting = null;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
        var input = GetMoveInput();
        if (Moving) Moving.MoveInDirection(input);
        if (Looking) Looking.LookInDirection(input);
        if (Picking)
        {
            if (IsButtonDown("Fire1")) Picking.PickUpOrRelease();
            if (enemySelecting) enemySelecting.SetEnabled(Picking.IsHolding);
        }

    }

    Vector2 GetMoveInput()
    {
        return GetCameraRelativeMovement();
    }

    Vector2 GetCameraRelativeMovement()
    {
        Transform cameraTransform = Camera.main.transform;

        Vector2 forward = cameraTransform.TransformDirection(Vector3.forward).ToSceenPosition();
        forward = forward.normalized;
        Vector2 right = new Vector2(forward.y, -forward.x);

        float v = GetAxis("Vertical");
        float h = GetAxis("Horizontal");
        return h * right + v * forward;
    }

    float GetAxis(string axisName)
    {
        return Input.GetAxis(axisName);
    }

    bool IsButton(string buttonName)
    {
        return Input.GetButton(buttonName);
    }

    bool IsButtonDown(string buttonName)
    {
        return Input.GetButtonDown(buttonName);
    }

    bool IsButtonUp(string buttonName)
    {
        return Input.GetButtonUp(buttonName);
    }
}
