using UnityEngine;

public class MapMove : MonoBehaviour
{
    //�÷��̾� ���� �̵�, ���� �������� ������
    [SerializeField] float mapSpeed;
    private void Update()
    {
        transform.Translate(-mapSpeed * Time.deltaTime, 0, 0);
    }
}
