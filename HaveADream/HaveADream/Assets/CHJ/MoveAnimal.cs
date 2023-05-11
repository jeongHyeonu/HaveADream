using System.Collections;
using UnityEngine;
public enum AnimalState { MoveToAppearPoint = 0, }
public class MoveAnimal : MonoBehaviour
{
    private Movement2D movement2D;
    private float animalAppearPoint = 0f;
    private AnimalState animalState = AnimalState.MoveToAppearPoint;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void ChangeState(AnimalState newState)
    {
        StopCoroutine(animalState.ToString());
        animalState = newState;
        StartCoroutine(animalState.ToString());
    }

    private IEnumerator MoveToAppearPoint()
    {
        //코루틴 실행 시 1회 호출
        movement2D.MoveTo(Vector3.left);
        while (true)
        {
            if (transform.position.x <= animalAppearPoint)
            {
                movement2D.MoveTo(Vector3.zero);
            }
            yield return null;
        }
    }
}
