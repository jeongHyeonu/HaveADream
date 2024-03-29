using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EndlessPlatformSpawn : MonoBehaviour
{
    //just list of available platform types
    public Transform[] platforms;
    public Transform playerCam;
    [SerializeField] public GameObject BackgroundPool;

    private Vector2 lastPos = Vector2.zero;
    private Vector2 delta = Vector2.zero;

    private LinkedList<SpriteRenderer> platformList;

    private void OnEnable()
    {
        

        platformList = new LinkedList<SpriteRenderer>();
        lastPos = playerCam.position;
        delta = Vector2.zero;

        for (int i = 0; i < platforms.Length; ++i)
        {
            platforms[i].gameObject.SetActive(false);
        }

        var randomStart = getRandomSprite();
        var newObj = Instantiate(randomStart, randomStart.transform.position, randomStart.transform.rotation);
        newObj.transform.SetParent(BackgroundPool.transform);
        newObj.SetActive(true);
        platformList.AddLast(newObj.GetComponent<SpriteRenderer>());

        spawnInView();
    }

    private void OnDisable()
    {
        platformList.Clear();
        BackgroundPool.transform.parent.transform.position = new Vector2(0, 0);
    }

    private void updateDelta()
    {
        delta = (Vector2)playerCam.position - lastPos;

        lastPos = playerCam.position;
    }
    GameObject getRandomSprite()
    {
        return platforms[Random.Range(0, platforms.Length)].gameObject;
    }


    void Update()
    {
        updateDelta();

        spawnInView();

        clearInvisible();
    }

    private void clearInvisible()
    {
        var node = platformList?.First;
        while (node != null)
        {
            var next = node.Next;
            if (CameraUtils.outOfView(node.Value, playerCam, 1) && platformList.Count > 1)
            {
                Destroy(node.Value.gameObject);
                platformList.Remove(node);
            }

            node = next;
        }
    }

    private void spawnInView()
    {
        if (platformList.Any())
        {
            var sprite = platformList.Last;
            if (CameraUtils.rightEdgeIn(sprite.Value, playerCam, 1))
            {
                var newObject = spawnSpriteObj(sprite.Value, getRandomSprite());
                platformList.AddLast(newObject.GetComponent<SpriteRenderer>());
            }

            sprite = platformList.First;
            if (CameraUtils.leftEdgeIn(sprite.Value, playerCam, 1))
            {
                var newObject = spawnSpriteObj(sprite.Value, getRandomSprite(), -1);
                platformList.AddFirst(newObject.GetComponent<SpriteRenderer>());
            }
        }
    }

    private GameObject spawnSpriteObj(SpriteRenderer sample, GameObject sprite, float shift = 1.0f)
    {
        return spawnSprite(sample, sprite.GetComponent<SpriteRenderer>(), shift);
    }

    private GameObject spawnSprite(SpriteRenderer sample, SpriteRenderer sprite, float shift = 1.0f)
    {
        var sampleTransform = sample.transform;

        var newObject = Instantiate(sprite.gameObject, sampleTransform.position, sprite.transform.rotation);
        newObject.transform.SetParent(BackgroundPool.transform);

        var position = newObject.transform.position;

        newObject.transform.position = new Vector3(position.x, sprite.transform.position.y, position.z);
        newObject.SetActive(true);
        
        //newObject.transform.position = sample.transform.position;
        newObject.transform.Translate(shift * (CameraUtils.getWidth(sprite) + CameraUtils.getWidth(sample)), 0, 0,
            Space.Self);
        return newObject;
    }
}



