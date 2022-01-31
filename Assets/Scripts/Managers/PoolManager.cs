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

    //Initialize an object pool by instantiating a defined number of each type of obstacle
    void InitObstaclePools()
    {
        //Initialize parent object
        Transform obstacleParent = transform.Find("Obtsacles");
        if (obstacleParent == null)
        {
            obstacleParent = new GameObject("Obstacles").transform;
            obstacleParent.parent = this.transform;
        }

        //Loop through all the level settings defined in the game manager
        foreach(LevelSettings settings in GameManager.Instance.levelSettings)
        {
            //Loop through each type of obstacle available for a given level
            foreach (ObstacleData obstacleData in settings.obstacles)
            {
                //If the object already exists in the pool we don't need to add it
                if (poolLookup.ContainsKey(obstacleData.prefab.name))
                {
                    continue;
                }
                List<Transform> pool = new List<Transform>();
                //Init parent for object for each type of obstcale
                Transform parent = obstacleParent.Find(obstacleData.prefab.name + "s");
                if (parent == null)
                {
                    parent = new GameObject(obstacleData.prefab.name + "s").transform;
                    parent.parent = obstacleParent;
                }

                //If the object does not yet exist in the object pool, instantiate a defined number of objects and add them to the pool
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

    //Given a name for an object, retrieve it from the object pool if it exists
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
        Debug.LogWarning("object: " + name + " could not be found in the pool!");
        return null;
    }

    //Return an object to the pool and set it to inactive
    public void ReturnToPool(Transform transform)
    {
        transform.gameObject.SetActive(false);

        poolLookup[transform.name].Add(transform);
    }


}
