using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }

    [SerializeField]
    private PlayerController player;

    [SerializeField]
    LevelPartGenerator moonSurfaceGenerator;

    [SerializeField]
    LevelPartGenerator backgroundGenerator;

    public List<LevelSettings> levelSettings;

    [SerializeField]
    private int startingLevel = 1;

    [SerializeField]
    private bool resetHints = false;

    [SerializeField]
    private bool resetStats = false;

    public bool showHints = true;

    [HideInInspector]
    public bool isGamePlaying = false;
    [HideInInspector]
    public bool isPlayerAlive = true;
    [HideInInspector]
    public int currentLevel = 1;
    [HideInInspector]
    public int score;

    [HideInInspector]
    public bool hasEncounteredJump;
    [HideInInspector]
    public bool hasEncounteredSlide;
    [HideInInspector]
    public bool hasEncounteredShoot;
    [HideInInspector]
    public bool hasEncounteredDoubleJump;

    private float distanceTravelled;
    [HideInInspector]
    public float distanceSlid;
    [HideInInspector]
    public int meteorsDestroyed;

    private float timeLevelStarted;
    private float timePreviousLevelStarted;

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

    private void OnEnable()
    {
        EventManager.Instance.onPlayerDeath += CheckForNewHighScore;
        EventManager.Instance.onLevelIncremented += CalculateDistanceTravelled;
        EventManager.Instance.onPlayerDeath += UpdateDistanceTravelled;
        EventManager.Instance.onPlayerDeath += UpdateDistanceSlid;
        EventManager.Instance.onPlayerDeath += UpdateMeteorsDestroyed;
    }

    private void OnDisable()
    {
        EventManager.Instance.onPlayerDeath -= CheckForNewHighScore;
        EventManager.Instance.onLevelIncremented -= CalculateDistanceTravelled;
        EventManager.Instance.onPlayerDeath -= UpdateDistanceTravelled;
        EventManager.Instance.onPlayerDeath -= UpdateDistanceSlid;
        EventManager.Instance.onPlayerDeath -= UpdateMeteorsDestroyed;

    }

    private void Start()
    {
        currentLevel = startingLevel;

        if (resetHints)
        {
            PlayerPrefs.SetInt("HasCompletedTutorial", 0);
        }

        if (resetStats)
        {
            PlayerPrefs.DeleteAll();
        }

        distanceTravelled = 0;
        distanceSlid = 0;
        meteorsDestroyed = 0;

        moonSurfaceGenerator.UpdateScrollSpeed(levelSettings[currentLevel - 1].scrollSpeed);
        backgroundGenerator.UpdateScrollSpeed(levelSettings[currentLevel - 1].backgroundScrollSpeed);
        //Debug.Log(levelSettings[currentLevel - 1].backgroundScrollSpeed);

    }


    public void AddToScore(int points)
    {
        score+= points;
        if(score > levelSettings[currentLevel - 1].levelScore && currentLevel - 1 < levelSettings.Count - 1)
        {
            IncrementLevel();
        }
    }

    void IncrementLevel()
    {
        currentLevel++;
        if(PlayerPrefs.GetInt("HighestLevel") < currentLevel)
        {
            PlayerPrefs.SetInt("HighestLevel", currentLevel);
            EventManager.Instance.HighestLevelUpdated();
        }
        EventManager.Instance.LevelIncremented();
        EventManager.Instance.ThemeTempoChanged();
        moonSurfaceGenerator.UpdateScrollSpeed(levelSettings[currentLevel - 1].scrollSpeed);
        backgroundGenerator.UpdateScrollSpeed(levelSettings[currentLevel - 1].backgroundScrollSpeed);

    }

    public LevelSettings GetCurrentLevelSettings()
    {
        return levelSettings[currentLevel - 1];
    }

    void ResetStats()
    {
        distanceTravelled = 0;
        distanceSlid = 0;
        meteorsDestroyed = 0;
    }

    void ResetTips()
    {
        hasEncounteredJump = false;
        hasEncounteredSlide = false;
        hasEncounteredShoot = false;
        hasEncounteredDoubleJump = false;
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Retry()
    {
        player.Respawn();

        currentLevel = startingLevel;
        score = 0;
        EventManager.Instance.PlayerRespawned();
        moonSurfaceGenerator.UpdateScrollSpeed(levelSettings[currentLevel - 1].scrollSpeed);
        backgroundGenerator.UpdateScrollSpeed(levelSettings[currentLevel - 1].backgroundScrollSpeed);
        StartCoroutine(StartGameWithDelay(0.5f));
    }

    IEnumerator StartGameWithDelay(float t)
    {
        yield return new WaitForSeconds(t);
        isPlayerAlive = true;
        isGamePlaying = false;


        ResetStats();
        ResetTips();

        player.ResetInitialJump();
        isPlayerAlive = true;
        EventManager.Instance.PlayButtonPressed();
        AudioManager.Instance.ResetThemeTempo();
        AudioManager.Instance.PlayTheme();

    }


    public void StartGame()
    {
        EventManager.Instance.PlayButtonPressed();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    void CheckForNewHighScore()
    {
        if (PlayerPrefs.GetInt("HighScore") <= score)
        {
            PlayerPrefs.SetInt("HighScore", score);
            EventManager.Instance.HighScoreUpdated();
            //PlayGames.Instance.AddScoreToLeaderboard(score);
            CloudOnceServices.Instance.SubmitScoreToLeaderboard(score);
        }
    }

    void UpdateDistanceTravelled()
    {
        float distanceTravelledForLevel = (Time.time - timeLevelStarted) * levelSettings[currentLevel - 1].scrollSpeed;
        distanceTravelled += distanceTravelledForLevel;
        float totaldistanceTravelled = (float)System.Math.Round((double)(PlayerPrefs.GetFloat("DistanceTravelled") + distanceTravelled), 2);
        PlayerPrefs.SetFloat("DistanceTravelled", totaldistanceTravelled);
        EventManager.Instance.DistanceTravelledUpdated();
    }


    void CalculateDistanceTravelled()
    {
        timePreviousLevelStarted = timeLevelStarted;
        timeLevelStarted = Time.time;
        if(currentLevel != 1)
        {
            distanceTravelled = (Time.time - timePreviousLevelStarted) * levelSettings[currentLevel - 2].scrollSpeed;
        }
    }

    public void AddToSlideDistance(float time)
    {
        distanceSlid += time * levelSettings[currentLevel - 1].scrollSpeed;
    }

    void UpdateDistanceSlid()
    {
        float totalDistanceSlid = (float)System.Math.Round((double)(PlayerPrefs.GetFloat("DistanceSlid") + distanceSlid), 2);
        PlayerPrefs.SetFloat("DistanceSlid", totalDistanceSlid);
        EventManager.Instance.DistanceSlidUpdated();
    }

    public void AddToMeteorsDestroyed()
    {
        meteorsDestroyed++;
    }

    void UpdateMeteorsDestroyed()
    {
        PlayerPrefs.SetInt("MeteorsDestroyed", PlayerPrefs.GetInt("MeteorsDestroyed") + meteorsDestroyed);
        EventManager.Instance.MeteorsDestroyedUpdated();
    }

}
