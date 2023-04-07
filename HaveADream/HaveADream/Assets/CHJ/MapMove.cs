using UnityEngine;

public class MapMove : Singleton<MapMove>
{
    //�÷��̾� ���� �̵�, ���� �������� ������
    public float mapSpeed;

    private void OnEnable()
    {
        this.transform.position = Vector3.zero; // ���۽� ��ġ �ʱ�ȭ
        mapSpeed = 5.0f;
    }

    private void Update()
    {
        transform.Translate(-mapSpeed * Time.deltaTime, 0, 0);
    }
}
