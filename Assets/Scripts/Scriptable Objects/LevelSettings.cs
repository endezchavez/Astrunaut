using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class LevelSettings : ScriptableObject
{
    public int level;
    public int levelScore;
    public float scrollSpeed;
    public float backgroundScrollSpeed;
    public float obstacleSpawnTimer;
    public List<ObstacleData> obstacles;
    public int minCyclesBeforeSpawn = 3;

}
