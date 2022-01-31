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

    }

    //Add points to score and increment level if current level max score is surpassed
    public void AddToScore(int points)
    {
        score+= points;
        if(score > levelSettings[currentLevel - 1].levelScore && currentLevel - 1 < levelSettings.Count - 1)
        {
            IncrementLevel();
        }
    }

    //Increase the current level by 1 and update level settings
    void IncrementLevel()
    {
        currentLevel++;
        //If the player has reached a new highest level update the highest level stat in player prefs
        if(PlayerPrefs.GetInt("HighestLevel") < currentLevel)
        {
            PlayerPrefs.SetInt("HighestLevel", currentLevel);
            EventManager.Instance.HighestLevelUpdated();
        }
        //Update the the tempo of the music and update the scroll speed to match the new level settings
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

    //Respawns the player and resets various stats when the game is restarted
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

    //After a delay start the game again
    IEnumerator StartGameWithDelay(float t)
    {
        yield return new WaitForSeconds(t);
        isPlayerAlive = true;
        isGamePlaying = false;

        ResetStats();
        ResetTips();

        player.ResetInitialJump();
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

    //Check for a new high score and if a new high score is reached update the high score in player prefs and submit the score to the leaderboards
    void CheckForNewHighScore()
    {
        if (PlayerPrefs.GetInt("HighScore") <= score)
        {
            PlayerPrefs.SetInt("HighScore", score);
            EventManager.Instance.HighScoreUpdated();
            CloudOnceServices.Instance.SubmitScoreToLeaderboard(score);
        }
    }

    //Update the distance travelled in the player prefs
    void UpdateDistanceTravelled()
    {
        float distanceTravelledForLevel = (Time.time - timeLevelStarted) * levelSettings[currentLevel - 1].scrollSpeed;
        distanceTravelled += distanceTravelledForLevel;
        float totaldistanceTravelled = (float)System.Math.Round((double)(PlayerPrefs.GetFloat("DistanceTravelled") + distanceTravelled), 2);
        PlayerPrefs.SetFloat("DistanceTravelled", totaldistanceTravelled);
        EventManager.Instance.DistanceTravelledUpdated();
    }

    //calculate the distance travelled, as the player is stationary in the scene we need to calculate distance time since the level started and the current scroll speed
    void CalculateDistanceTravelled()
    {
        timePreviousLevelStarted = timeLevelStarted;
        timeLevelStarted = Time.time;
        if(currentLevel != 1)
        {
            distanceTravelled = (Time.time - timePreviousLevelStarted) * levelSettings[currentLevel - 2].scrollSpeed;
        }
    }

    //Add to the total distance slid, this is equal to the slide time * the current scroll speed
    public void AddToSlideDistance(float time)
    {
        distanceSlid += time * levelSettings[currentLevel - 1].scrollSpeed;
    }

    //Update the distance slid in the player prefs
    void UpdateDistanceSlid()
    {
        float totalDistanceSlid = (float)System.Math.Round((double)(PlayerPrefs.GetFloat("DistanceSlid") + distanceSlid), 2);
        PlayerPrefs.SetFloat("DistanceSlid", totalDistanceSlid);
        EventManager.Instance.DistanceSlidUpdated();
    }

    //Add to number of meteors destroyed
    public void AddToMeteorsDestroyed()
    {
        meteorsDestroyed++;
    }

    //Update the number of meteors destroyed in the player prefs
    void UpdateMeteorsDestroyed()
    {
        PlayerPrefs.SetInt("MeteorsDestroyed", PlayerPrefs.GetInt("MeteorsDestroyed") + meteorsDestroyed);
        EventManager.Instance.MeteorsDestroyedUpdated();
    }

}
