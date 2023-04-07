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

    // 카메라
    [SerializeField] public GameObject camera;

    // 줌인 대상 오브젝트
    private Transform targetObj;
    // 경유 지점 위치
    public Transform subTarget;
    // 부드럽게 이동될 감도
    public float smoothTime = .3f;

    private Vector2 velocity = Vector2.zero;
    
    // 카메라 타겟 상태 줌인 상태
    [SerializeField] public static bool isActive = false;

    // 줌인 정도가 -가 클수록 줌아웃
    public float zoomIn = -5;

    // 경유지점 도착 후 진행 카운트
    private int passCnt = 0;

    private void Update()
    {
        if(isActive)
        {
            Vector2 targetPos;

            // 경유지점이 있다면 목표지점을 경유우선으로 지정한다
            if (subTarget!=null && passCnt==0)
            {
                targetPos = subTarget.transform.position;
                smoothTime = 0.1f;
            }
            // 경유지점 없다면 목표지점을 종착지로
            else
            {
                targetPos = this.transform.position;
                //targetPos = targetObj.TransformPoint(new Vector2(0, 0));
            }

            // 위에서 설정된 위치로 부드럽게 이동
            //camera.transform.position = Vector2.SmoothDamp(camera.transform.position, targetPos, ref velocity, smoothTime);
            camera.transform.LookAt(targetObj);

            // 목표지점 이내에 도착
            if (Vector2.Distance(targetPos, camera.transform.position) < 0.1f)
            {
                // 경유지점 있을 경우
                if (subTarget != null)
                {

                }
                else
                {
                    //경유지점 없이 최종 목적지 도착했을때 이벤트
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
