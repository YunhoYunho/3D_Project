using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public string moveAxisName = "Vertical";
    public string rotateAxisName = "Horizontal";
    public string jumpButtonName = "Jump";
    public string fireButtonName = "Fire1";
    public string reloadButtonName = "Reload";
    public string mouseXAxisName = "Mouse X";
    public string mouseYAxisName = "Mouse Y";

    public float move { get; private set; }
    public float rotate { get; private set; }
    public bool jump { get; private set; }
    public bool fire { get; private set; }
    public bool reload { get; private set; }
    public float mouseX { get; private set; }
    public float mouseY { get; private set; }

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
        fire = Input.GetButton(fireButtonName);
        reload = Input.GetButtonDown(reloadButtonName);
        mouseX = Input.GetAxis(mouseXAxisName);
        mouseY = Input.GetAxis(mouseYAxisName);
    }
}