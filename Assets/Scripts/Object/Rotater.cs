using UnityEngine;

public class Rotater : MonoBehaviour
{
    [SerializeField]
    private float rotateSpeed = 120f;

    private void Update()
    {
        transform.Rotate(0f, rotateSpeed * Time.deltaTime, 0f);
    }
}
