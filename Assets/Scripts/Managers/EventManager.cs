using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private static EventManager _instance;

    public static EventManager Instance { get { return _instance; } }

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

    public event Action onLevelIncremented;
    public void LevelIncremented()
    {
        if(onLevelIncremented != null)
        {
            onLevelIncremented();
        }
    }

    public event Action onPlayerDeath;
    public void PlayerDied()
    {
        if (onPlayerDeath != null)
        {
            onPlayerDeath();
        }
    }

    public event Action onPlayButtonPressed;
    public void PlayButtonPressed()
    {
        if (onPlayButtonPressed != null)
        {
            onPlayButtonPressed();
        }
    }

    public event Action onGameStarted;
    public void GameStarted()
    {
        if (onGameStarted != null)
        {
            onGameStarted();
        }
    }
    public event Action onHighScoreUpdated;
    public void HighScoreUpdated()
    {
        if (onHighScoreUpdated != null)
        {
            onHighScoreUpdated();
        }
    }

    public event Action onHighestLevelUpdated;
    public void HighestLevelUpdated()
    {
        if (onHighestLevelUpdated != null)
        {
            onHighestLevelUpdated();
        }
    }

    public event Action onDistanceTravelledUpdated;
    public void DistanceTravelledUpdated()
    {
        if (onDistanceTravelledUpdated != null)
        {
            onDistanceTravelledUpdated();
        }
    }

    public event Action onDistanceSlidUpdated;
    public void DistanceSlidUpdated()
    {
        if (onDistanceSlidUpdated != null)
        {
            onDistanceSlidUpdated();
        }
    }

    public event Action onMeteorsDestroyedUpdated;
    public void MeteorsDestroyedUpdated()
    {
        if (onMeteorsDestroyedUpdated != null)
        {
            onMeteorsDestroyedUpdated();
        }
    }

    public event Action onFirstJumpEncountered;
    public void FirstJumpEncountered()
    {
        if (onFirstJumpEncountered != null)
        {
            onFirstJumpEncountered();
        }
    }

    public event Action onFirstSlideEncountered;
    public void FirstSlideEncountered()
    {
        if (onFirstSlideEncountered != null)
        {
            onFirstSlideEncountered();
        }
    }

    public event Action onFirstShootEncountered;
    public void FirstShootEncountered()
    {
        if (onFirstShootEncountered != null)
        {
            onFirstShootEncountered();
        }
    }

    public event Action onFirstDoubleJumpEncountered;
    public void FirstDoubleJumpEncountered()
    {
        if (onFirstDoubleJumpEncountered != null)
        {
            onFirstDoubleJumpEncountered();
        }
    }

    public event Action onThemeTemposChange;
    public void ThemeTempoChanged()
    {
        if (onThemeTemposChange != null)
        {
            onThemeTemposChange();
        }
    }

    public event Action onPlayerRespawn;
    public void PlayerRespawned()
    {
        if (onPlayerRespawn != null)
        {
            onPlayerRespawn();
        }
    }



}
