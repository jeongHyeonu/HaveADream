using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MovingObstacle : MonoBehaviour
{
    [SerializeField] private float objectOriginPos_y = 6.25f;
    [SerializeField] private float objectMovingY = 2f;
    [SerializeField] private float objectMovingDuration = 0.5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("MovingObstacleSpawnPoint"))
        {
            if (this.gameObject.transform.position.y < 0)
            {
                this.transform.DOLocalMoveY(-objectMovingY, objectMovingDuration);
            }
            else
            {
                this.transform.DOLocalMoveY(objectMovingY, objectMovingDuration);
            }
        }
    }

    private void OnDisable()
    {
        Vector2 originPos = this.transform.position;
        if(originPos.y<0) originPos.y = objectOriginPos_y;
        else originPos.y = objectOriginPos_y;
        //this.transform.position = originPos;
    }
}
