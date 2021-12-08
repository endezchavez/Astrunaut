using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPartGenerator : MonoBehaviour
{
    [SerializeField]
    private bool listenForEvents;
    [SerializeField]
    bool autoScroll = false;
    [SerializeField]
    private float scrollSpeed;
    [SerializeField]
    private Transform prefab;
    [SerializeField]
    private Vector3 startPos;

    [SerializeField]
    private int numInitialLevelParts = 3;
    [SerializeField]
    private Vector3 levelPartOffset;

    Queue<Transform> levelPartsQueue;

    Transform lastSpawnedLevelPart;

    private void Awake()
    {
        levelPartsQueue = new Queue<Transform>(numInitialLevelParts);
    }

    private void Start()
    {
  
        Vector3 nextPos = startPos;
        for (int i = 0; i < numInitialLevelParts; i++)
        {
            Transform levelPart = SpawnLevelPart(nextPos, prefab);
            nextPos = levelPart.position + levelPartOffset;
            levelPartsQueue.Enqueue(levelPart);
            if (i == numInitialLevelParts - 1)
            {
                lastSpawnedLevelPart = levelPart;
            }
        }
    }


    private void Update()
    {
        if (GameManager.Instance.isPlayerAlive && GameManager.Instance.isGamePlaying || autoScroll)
        {
            if (Helper.IsOffScreen(levelPartsQueue.Peek().Find("End Pos")))
            {
                Transform o = levelPartsQueue.Dequeue();
                o.position = lastSpawnedLevelPart.position + levelPartOffset;
                levelPartsQueue.Enqueue(o);
                lastSpawnedLevelPart = o;
            }

            foreach (Transform levelPart in levelPartsQueue)
            {
                levelPart.Translate(-Vector2.right * Time.deltaTime * scrollSpeed);
            }
        }
    }

    Transform SpawnLevelPart(Vector3 pos, Transform prefab)
    {
        Transform levelPartTransform = Instantiate(prefab);
        levelPartTransform.position = pos;
        return levelPartTransform.transform;
    }

    public void UpdateScrollSpeed(float speed)
    {
        scrollSpeed = speed;
    }

}
