using UnityEngine;

public class Movement2D : MonoBehaviour
{


    [SerializeField]
    private float moveSpeed = 0.0f; //이동 속도
    [SerializeField]
    private Vector3 moveDirection = Vector3.zero; //이동 방향

    private void Update()
    {
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

    }
    public void MoveTo(Vector3 direction)
    {
        moveDirection = direction;
    }

}


