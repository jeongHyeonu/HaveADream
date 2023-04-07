using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector2 direction;


    private void Update()
    {
        Rigidbody2D rigid = gameObject.GetComponent<Rigidbody2D>();
        rigid.AddForce(Vector2.right * 10f, ForceMode2D.Force);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Boss")
        {
            //Destroy(gameObject);
            Invoke("DestroyBullet", 0f);
        }
    }
    public void Shoot()
    {
        /*direction = dir;
        dir = Vector2.right;*/
        Invoke("DestroyBullet", 3f);
    }
    private void DestroyBullet()
    {
        BulletPool.ReturnObject(this);
    }
}
