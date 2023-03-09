using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //스피드 조절
    [SerializeField] float speed;
    bool isTouching = false;

    void Update()
    {
        if (Input.GetMouseButton(0)) isTouching = true;
        else isTouching = false;
    }
    private void FixedUpdate()
    {
        if (isTouching == false)
        {
            this.GetComponent<Rigidbody2D>().AddForce(Vector3.down * 20f);
        }
        else
        {
            this.GetComponent<Rigidbody2D>().AddForce(Vector3.up * 20f);
        }
    }
}
