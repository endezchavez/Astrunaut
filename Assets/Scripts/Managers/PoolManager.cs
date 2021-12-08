using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PoolManager : MonoBehaviour
{
    private static PoolManager _instance;

    public static PoolManager Instance { get { return _instance; } }

    Dictionary<string, List<Transform>> poolLookup = new Dictionary<string, List<Transform>>();

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

    }

    private void Start()
    {
        InitObstaclePools();
    }

    void InitObstaclePools()
    {

        Transform obstacleParent = transform.Find("Obtsacles");
        if (obstacleParent == null)
        {
            obstacleParent = new GameObject("Obstacles").transform;
            obstacleParent.parent = this.transform;
        }

        foreach(LevelSettings settings in GameManager.Instance.levelSettings)
        {

            foreach (ObstacleData obstacleData in settings.obstacles)
            {

                if (poolLookup.ContainsKey(obstacleData.prefab.name))
                {
                    continue;
                }
                List<Transform> pool = new List<Transform>();
                Transform parent = obstacleParent.Find(obstacleData.prefab.name + "s");
                if (parent == null)
                {
                    parent = new GameObject(obstacleData.prefab.name + "s").transform;
                    parent.parent = obstacleParent;
                }

                for (int i = 0; i < obstacleData.numObjectsToPool; i++)
                {
                    Transform obstacle = Instantiate(obstacleData.prefab).transform;
                    obstacle.parent = parent;
                    obstacle.gameObject.SetActive(false);
                    obstacle.gameObject.name = obstacle.gameObject.name.Replace("(Clone)", "");
                    pool.Add(obstacle);
                }
                
                poolLookup.Add(obstacleData.prefab.name, pool);
            }
        }

    }

    public Transform GetPooledObject(string name)
    {
        foreach (Transform transform in poolLookup[name])
        {
            if (!transform.gameObject.activeSelf)
            {
                transform.gameObject.SetActive(true);
                poolLookup[name].Remove(transform);
                return transform;
            }
        }
        return null;
    }

    public void ReturnToPool(Transform transform)
    {
        transform.gameObject.SetActive(false);

        poolLookup[transform.name].Add(transform);
    }


}
