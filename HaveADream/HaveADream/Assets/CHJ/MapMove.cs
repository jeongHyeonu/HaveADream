using UnityEngine;

public class MapMove : Singleton<MapMove>
{
    //플레이어 상하 이동, 맵이 왼쪽으로 움직임
    public float mapSpeed;

    private void OnEnable()
    {
        this.transform.position = Vector3.zero; // 시작시 위치 초기화
        mapSpeed = 5.0f;
    }

    private void Update()
    {
        transform.Translate(-mapSpeed * Time.deltaTime, 0, 0);
    }
}
