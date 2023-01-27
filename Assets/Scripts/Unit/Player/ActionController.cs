using UnityEngine;

public class ActionController : MonoBehaviour
{
    private RaycastHit hitInfo;

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
            UIManager.Instance.doorOpenText.gameObject.SetActive(true);
        }

        if (hitInfo.transform.tag == "Goal")
        {
            if (GameManager.Instance.isKeyEnough == true)
            {
                UIManager.Instance.winnerText.gameObject.SetActive(true);
            }

            else if (GameManager.Instance.isKeyEnough == false)
            {
                UIManager.Instance.notEnoughKeyText.gameObject.SetActive(true);
            }
        }
    }

    private void DoorTextDisAppear()
    {
        UIManager.Instance.doorOpenText.gameObject.SetActive(false);
        UIManager.Instance.notEnoughKeyText.gameObject.SetActive(false);
    }
}
