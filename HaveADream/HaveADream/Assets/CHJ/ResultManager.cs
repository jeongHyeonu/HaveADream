using UnityEngine;

public class ResultManager : MonoBehaviour
{
    private SceneManager sm = null;
    void Start()
    {
        // ΩÃ±€≈Ê
        sm = SceneManager.Instance;
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.CompareTo("Player") == 0)
        {
            sm.Scene_Change_Result();
        }
    }
}
