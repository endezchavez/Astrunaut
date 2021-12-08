using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenerator : MonoBehaviour
{
    [SerializeField]
    private Transform spawnPos;

    Queue<Transform> obstaclesQueue;

    Dictionary<string, List<Transform>> poolLookup = new Dictionary<string, List<Transform>>();

    LevelSettings currentLevelSettings;



    private void Awake()
    {
        obstaclesQueue = new Queue<Transform>();
    }

    private void OnEnable()
    {
        EventManager.Instance.onPlayerRespawn += ReturnAllObjectsToPool;
    }

    private void OnDisable()
    {
        EventManager.Instance.onPlayerRespawn -= ReturnAllObjectsToPool;
    }

    void Start()
    {
        EventManager.Instance.onLevelIncremented += UpdateLevelSettings;
        EventManager.Instance.onGameStarted += StartspawningObstacles;
        currentLevelSettings = GameManager.Instance.GetCurrentLevelSettings();
    }

    private void Update()
    {
        if (GameManager.Instance.isGamePlaying)
        {
            if (obstaclesQueue.Count > 0 && GameManager.Instance.isPlayerAlive)
            {
                if (Helper.IsOffScreen(obstaclesQueue.Peek()))
                {
                    PoolManager.Instance.ReturnToPool(obstaclesQueue.Peek());
                    obstaclesQueue.Dequeue();
                }

                foreach (Transform obstacle in obstaclesQueue)
                {
                    obstacle.position += -Vector3.right * Time.deltaTime * currentLevelSettings.scrollSpeed;
                }
            }
        }
        
    }

    void StartspawningObstacles()
    {
        StartCoroutine(SpawnObstacles());
    }


    IEnumerator SpawnObstacles()
    {
        int cyclesSinceLastSpawn = 0;
        while (GameManager.Instance.isPlayerAlive && GameManager.Instance.isGamePlaying)
        {
            yield return new WaitForSeconds(currentLevelSettings.obstacleSpawnTimer);
            ObstacleData obstacleData = currentLevelSettings.obstacles[Random.Range(0, currentLevelSettings.obstacles.Count)];
            int randChanceToSpawn = Random.Range(1, 100);
            cyclesSinceLastSpawn++;

            if (randChanceToSpawn <= obstacleData.chanceToSpawn || cyclesSinceLastSpawn >= currentLevelSettings.minCyclesBeforeSpawn)
            {
                Transform obstacle = PoolManager.Instance.GetPooledObject(obstacleData.prefab.name);
                obstacle.position = spawnPos.position;
                obstaclesQueue.Enqueue(obstacle);
                cyclesSinceLastSpawn = 0;
                if(PlayerPrefs.GetInt("HasCompletedTutorial") == 0 && GameManager.Instance.showHints)
                {
                    ShowHints(obstacleData);
                }
            }
        }
    }

    void ReturnAllObjectsToPool()
    {
        while(obstaclesQueue.Count > 0)
        {
            PoolManager.Instance.ReturnToPool(obstaclesQueue.Peek());
            obstaclesQueue.Dequeue();
        }
    }

    void ShowHints(ObstacleData data)
    {
        if (!GameManager.Instance.hasEncounteredJump && data.prefab.name == "Hole")
        {
            EventManager.Instance.FirstJumpEncountered();
            GameManager.Instance.hasEncounteredJump = true;
        }
        else if (!GameManager.Instance.hasEncounteredSlide && data.prefab.name == "Crate Middle")
        {
            EventManager.Instance.FirstSlideEncountered();
            GameManager.Instance.hasEncounteredSlide = true;
        }
        else if (!GameManager.Instance.hasEncounteredShoot && data.prefab.name == "Meteor Middle")
        {
            EventManager.Instance.FirstShootEncountered();
            GameManager.Instance.hasEncounteredShoot = true;
        }
        else if (!GameManager.Instance.hasEncounteredDoubleJump && (data.prefab.name == "2 Crates Bottom" || data.prefab.name == "Double Hole"))
        {
            EventManager.Instance.FirstDoubleJumpEncountered();
            GameManager.Instance.hasEncounteredDoubleJump = true;
        }

        if (GameManager.Instance.hasEncounteredJump && GameManager.Instance.hasEncounteredSlide && GameManager.Instance.hasEncounteredShoot && GameManager.Instance.hasEncounteredDoubleJump)
        {
            PlayerPrefs.SetInt("HasCompletedTutorial", 1);
        }
    }
    
    void UpdateLevelSettings()
    {
        currentLevelSettings = GameManager.Instance.GetCurrentLevelSettings();
    }

}
