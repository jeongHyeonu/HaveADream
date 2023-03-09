using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    // private Rigidbody rb;
    //스피드 조절
    [SerializeField] float speed;
    bool isTouching = false;


    void Start()
    {
        //rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        if (Input.GetMouseButton(0))
            isTouching = true;
        else
            isTouching = false;
    }

    void FixedUpdate()
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
    /*void FixedUpdate()
    {

        if (Input.GetMouseButton(0))

        {
            GetComponent<Rigidbody2D>() = new Vector3(0, speed, 0);
        }

    }*/

}
