using System.Collections;
using UnityEngine;

public enum BossState { MoveToAppearPoint = 0, }
public class Boss : MonoBehaviour
{
    [SerializeField]
    private float bossAppearPoint = 0f;
    private BossState bossState = BossState.MoveToAppearPoint;
    private Movement2D movement2D;

    private void Awake()
    {
        movement2D = GetComponent<Movement2D>();
    }
    public void ChangeState(BossState newState)
    {
        StopCoroutine(bossState.ToString());
        bossState = newState;
        StartCoroutine(bossState.ToString());
    }
    public IEnumerator MoveToAppearPoint()
    {
        //�ڷ�ƾ ���� �� 1ȸ ȣ��
        movement2D.MoveTo(Vector3.left);
        while (true)
        {
            if (transform.position.x <= bossAppearPoint)
            {
                movement2D.MoveTo(Vector3.zero);
            }
            yield return null;
        }
    }
}
