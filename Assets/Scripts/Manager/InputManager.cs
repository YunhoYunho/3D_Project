using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : SingleTon<InputManager>
{
    public string moveAxisName = "Vertical";
    public string rotateAxisName = "Horizontal";
    public string jumpButtonName = "Jump";
    public string crouchButtonName = "Crouch";
    public string fireButtonName = "Fire1";
    public string meleeButtonName = "Melee";
    public string reloadButtonName = "Reload";
    public string mouseXAxisName = "Mouse X";
    public string mouseYAxisName = "Mouse Y";
    public string interActionButtonName = "InterAction";
    public string pauseButtonName = "Pause";

    public float move { get; private set; }
    public float rotate { get; private set; }
    public bool jump { get; private set; }
    public bool fire { get; private set; }
    public bool melee { get; private set; }
    public bool crouch { get; private set; }
    public bool reload { get; private set; }
    public float mouseX { get; private set; }
    public float mouseY { get; private set; }
    public bool interAction { get; private set; }
    public bool pause { get; private set; }

    private void Update()
    {
        if (GameManager.Instance != null
            && GameManager.Instance.IsGameover)
        {
            move = 0;
            rotate = 0;
            jump = false;
            crouch = false;
            fire = false;
            melee = false;
            reload = false;
            mouseX = 0;
            mouseY = 0;
            interAction = false;
            pause = false;
            return;
        }

        move = Input.GetAxis(moveAxisName);
        rotate = Input.GetAxis(rotateAxisName);
        jump = Input.GetButton(jumpButtonName);
        crouch = Input.GetButton(crouchButtonName);
        fire = Input.GetButton(fireButtonName);
        melee = Input.GetButtonDown(meleeButtonName);
        reload = Input.GetButtonDown(reloadButtonName);
        mouseX = Input.GetAxis(mouseXAxisName);
        mouseY = Input.GetAxis(mouseYAxisName);
        interAction = Input.GetButtonDown(interActionButtonName);
        pause = Input.GetButtonDown(pauseButtonName);
    }
}