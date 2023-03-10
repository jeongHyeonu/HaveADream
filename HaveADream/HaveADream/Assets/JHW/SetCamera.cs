using UnityEngine;

public class SetCamera : MonoBehaviour
{
    [SerializeField] Camera MainCamera;

    // Start is called before the first frame update
    void Start()
    {
        MainCamera.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
