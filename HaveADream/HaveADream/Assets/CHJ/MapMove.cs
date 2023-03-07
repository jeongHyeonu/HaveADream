using UnityEngine;

public class MapMove : MonoBehaviour
{
    //플레이어 상하 이동, 맵이 왼쪽으로 움직임
    [SerializeField] float mapSpeed;
    private void Update()
    {
        transform.Translate(-mapSpeed * Time.deltaTime, 0, 0);
    }
}
