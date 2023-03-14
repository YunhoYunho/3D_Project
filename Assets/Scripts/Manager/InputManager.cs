using UnityEngine;

public class InputManager : SingleTon<InputManager>
{
    public string moveHorizontalAxisName = "Horizontal";
    public string moveVerticalAxisName = "Vertical";
    public string jumpButtonName = "Jump";
    public string fireButtonName = "Fire1";
    public string reloadButtonName = "Reload";
    public string interActionButtonName = "InterAction";
    public string pauseButtonName = "Pause";

    public Vector2 move { get; private set; }
    public bool jump { get; private set; }
    public bool fire { get; private set; }
    public bool reload { get; private set; }
    public bool interAction { get; private set; }
    public bool pause { get; private set; }

    private void Update()
    {
        if (GameManager.Instance != null
            && GameManager.Instance.IsGameover)
        {
            move = Vector2.zero;
            jump = false;
            fire = false;
            reload = false;
            interAction = false;
            pause = false;
            return;
        }

        move = new Vector2(Input.GetAxis(moveHorizontalAxisName),
            Input.GetAxis(moveVerticalAxisName));
        if (move.sqrMagnitude > 1)
            move = move.normalized;

        jump = Input.GetButton(jumpButtonName);
        fire = Input.GetButton(fireButtonName);
        reload = Input.GetButtonDown(reloadButtonName);
        interAction = Input.GetButtonDown(interActionButtonName);
        pause = Input.GetButtonDown(pauseButtonName);
    }
}