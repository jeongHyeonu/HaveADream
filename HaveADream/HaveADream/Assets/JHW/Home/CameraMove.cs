using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCompleteEventArgs
{
    public GameObject targetObj;
    public Vector2 pos;
}

public class CameraMove : MonoBehaviour
{
    public static event System.EventHandler<MoveCompleteEventArgs> EventHandler_CameraMoveTarget;

    // ī�޶�
    [SerializeField] public GameObject camera;

    // ���� ��� ������Ʈ
    private Transform targetObj;
    // ���� ���� ��ġ
    public Transform subTarget;
    // �ε巴�� �̵��� ����
    public float smoothTime = .3f;

    private Vector2 velocity = Vector2.zero;
    
    // ī�޶� Ÿ�� ���� ���� ����
    [SerializeField] public static bool isActive = false;

    // ���� ������ -�� Ŭ���� �ܾƿ�
    public float zoomIn = -5;

    // �������� ���� �� ���� ī��Ʈ
    private int passCnt = 0;

    private void Update()
    {
        if(isActive)
        {
            Vector2 targetPos;

            // ���������� �ִٸ� ��ǥ������ �����켱���� �����Ѵ�
            if (subTarget!=null && passCnt==0)
            {
                targetPos = subTarget.transform.position;
                smoothTime = 0.1f;
            }
            // �������� ���ٸ� ��ǥ������ ��������
            else
            {
                targetPos = this.transform.position;
                //targetPos = targetObj.TransformPoint(new Vector2(0, 0));
            }

            // ������ ������ ��ġ�� �ε巴�� �̵�
            //camera.transform.position = Vector2.SmoothDamp(camera.transform.position, targetPos, ref velocity, smoothTime);
            camera.transform.LookAt(targetObj);

            // ��ǥ���� �̳��� ����
            if (Vector2.Distance(targetPos, camera.transform.position) < 0.1f)
            {
                // �������� ���� ���
                if (subTarget != null)
                {

                }
                else
                {
                    //�������� ���� ���� ������ ���������� �̺�Ʈ
                    MoveCompleteEventArgs args = new MoveCompleteEventArgs();
                    args.targetObj = targetObj.gameObject;
                    args.pos = camera.transform.position;
                    EventHandler_CameraMoveTarget(this, args);
                }
            }
                
        }
    }

    public void SetTarget(GameObject target)
    {
        if (target == null) return;
        isActive = true;
        camera.transform.position = Vector2.SmoothDamp(camera.transform.position, subTarget.position, ref velocity, smoothTime) ;
        camera.transform.LookAt(targetObj);
        targetObj = target.transform;
    }

    private void Clear()
    {
        smoothTime = 0.3f;
        isActive = false;
        targetObj = null;
        passCnt = 0;
    }
}
