using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour
{
    [SerializeField] string _androidGameId;
    [SerializeField] string _iOsGameId;
    [SerializeField] bool _testMode = true;
    [SerializeField] bool _enablePerPlacementMode = true;
    private string _gameId;

    [SerializeField] string _androidAdUnitId = "Interstitial_Android";
    [SerializeField] string _iOsAdUnitId = "Interstitial_iOS";
    string _adUnitId;

    [SerializeField]
    private int numDeathsPerAd = 5;

    [SerializeField]
    private GameObject[] adButtons;


    void Awake()
    {
        if (!PlayerPrefs.HasKey("Ads"))
        {
            InitializeAds();
        }

        // Get the Ad Unit ID for the current platform:
        _adUnitId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? _iOsAdUnitId
            : _androidAdUnitId;
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("Ads"))
        {
            RemoveAdButtons();
        }
    }

    private void OnEnable()
    {
        EventManager.Instance.onPlayerDeath += LoadAndShowAdd;
    }

    private void OnDisable()
    {
        EventManager.Instance.onPlayerDeath -= LoadAndShowAdd;
    }

    public void InitializeAds()
    {
        _gameId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? _iOsGameId
            : _androidGameId;
        Advertisement.Initialize(_gameId, _testMode, _enablePerPlacementMode);
    }

    // Load content to the Ad Unit:
    public void LoadAd()
    {
        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        Debug.Log("Loading Ad: " + _adUnitId);
        Advertisement.Load(_adUnitId);
    }

    // Show the loaded content in the Ad Unit: 
    public void ShowAd()
    {
        // Note that if the ad content wasn't previously loaded, this method will fail
        Debug.Log("Showing Ad: " + _adUnitId);
        Advertisement.Show(_adUnitId);
    }

    void LoadAndShowAdd()
    {
        if (!PlayerPrefs.HasKey("Ads"))
        {
            int deathsSinceLastAd = PlayerPrefs.GetInt("DeathsSinceLastAd");
            PlayerPrefs.SetInt("DeathsSinceLastAd", deathsSinceLastAd + 1);
            if (PlayerPrefs.GetInt("DeathsSinceLastAd") >= numDeathsPerAd)
            {
                PlayerPrefs.SetInt("DeathsSinceLastAd", 0);
                LoadAd();
                ShowAd();
            }
        }
    }

    public void RemoveAds()
    {
        if (!PlayerPrefs.HasKey("Ads"))
        {
            PlayerPrefs.SetInt("Ads", 0);
            RemoveAdButtons();
        }
    }

    void RemoveAdButtons()
    {
        foreach(GameObject o in adButtons)
        {
            o.SetActive(false);
        }
    }

}
