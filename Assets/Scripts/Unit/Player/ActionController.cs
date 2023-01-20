using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActionController : MonoBehaviour
{
    private RaycastHit hitInfo;

    [SerializeField]
    private TextMeshProUGUI doorOpenText;

    [SerializeField]
    private TextMeshProUGUI doorGoalText;

    [SerializeField]
    private float range;

    [SerializeField]
    private LayerMask layerMask;

    private void Update()
    {
        Check();
    }

    private void Check()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, range, layerMask))
        {
            DoorTextAppear();
        }
        else
        {
            DoorTextDisAppear();
        }
    }

    private void DoorTextAppear()
    {
        if (hitInfo.transform.tag == "Door")
        {
            doorOpenText.gameObject.SetActive(true);
        }

        if (hitInfo.transform.tag == "Goal")
        {
            doorGoalText.gameObject.SetActive(true);
        }
    }

    private void DoorTextDisAppear()
    {
        doorOpenText.gameObject.SetActive(false);
        doorGoalText.gameObject.SetActive(false);
    }
}
