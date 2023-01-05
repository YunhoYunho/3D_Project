using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public string moveAxisName = "Vertical";
    public string rotateAxisName = "Horizontal";
    public string jumpButtonName = "Jump";
    public string crouchButtonName = "Crouch";
    public string fireButtonName = "Fire1";
    public string reloadButtonName = "Reload";
    public string mouseXAxisName = "Mouse X";
    public string mouseYAxisName = "Mouse Y";
    public string interActionButtonName = "InterAction";

    public float move { get; private set; }
    public float rotate { get; private set; }
    public bool jump { get; private set; }
    public bool fire { get; private set; }
    public bool crouch { get; private set; }
    public bool reload { get; private set; }
    public float mouseX { get; private set; }
    public float mouseY { get; private set; }
    public bool interAction { get; private set; }
    public bool weaponSlot1 { get; private set; }
    public bool weaponSlot2 { get; private set; }
    public bool weaponSlot3 { get; private set; }
    public bool weaponSlot4 { get; private set; }

    private void Update()
    {
        //if (GameManager.Instance != null
        //    && GameManager.Instance.isGameOver)
        //{
        //    move = 0;
        //    jump = false;
        //    fire = false;
        //    reload = false;
        //    mouseX = 0;
        //    mouseY = 0;
        //    return;
        //}

        move = Input.GetAxis(moveAxisName);
        rotate = Input.GetAxis(rotateAxisName);
        jump = Input.GetButton(jumpButtonName);
        crouch = Input.GetButton(crouchButtonName);
        fire = Input.GetButton(fireButtonName);
        reload = Input.GetButtonDown(reloadButtonName);
        mouseX = Input.GetAxis(mouseXAxisName);
        mouseY = Input.GetAxis(mouseYAxisName);
        interAction = Input.GetButtonDown(interActionButtonName);
        weaponSlot1 = Input.GetKeyDown(KeyCode.Alpha1);
        weaponSlot2 = Input.GetKeyDown(KeyCode.Alpha2);
        weaponSlot3 = Input.GetKeyDown(KeyCode.Alpha3);
        weaponSlot4 = Input.GetKeyDown(KeyCode.Alpha4);
    }
}