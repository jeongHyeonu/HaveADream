using System.Collections.Generic;
using UnityEngine;


public class BulletPool : MonoBehaviour
{
    [SerializeField] public static BulletPool Instance;
    [SerializeField] GameObject bulletPrefab;

    private Queue<Bullet> bulletPoolingQueue = new Queue<Bullet>();

    private void Awake()
    {
        Instance = this;
        Initialize(10);
    }
    private Bullet CreateNewBullet()
    {
        var newObj = Instantiate(bulletPrefab, transform).GetComponent<Bullet>();
        newObj.gameObject.SetActive(false);
        return newObj;
    }
    private void Initialize(int count)
    {
        for (int i = 0; i < count; i++)
        {
            bulletPoolingQueue.Enqueue(CreateNewBullet());
        }
    }

    public static Bullet GetObject()
    {
        if (Instance.bulletPoolingQueue.Count > 0)
        {
            var obj = Instance.bulletPoolingQueue.Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            var newobj = Instance.CreateNewBullet();
            newobj.transform.SetParent(null);
            newobj.gameObject.SetActive(true);
            return newobj;
        }
    }
    public static void ReturnObject(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
        bullet.transform.SetParent(Instance.transform);
    }

}
