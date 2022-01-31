using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;

    public static AudioManager Instance { get { return _instance; } }

    public bool playThemeOnAwake;

    public Sound[] sounds;

    public Sound[] themeSongTempos;

    int currentTempoIndex = 0;

    Sound currentThemeSound;
    float currentThemeVolume;

    bool isThemePlaying = true;

    [SerializeField]
    private Image audioImg;
    [SerializeField]
    private Sprite audioEnabledSpr;
    [SerializeField]
    private Sprite audioDisabledSpr;

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

        //Loop over each theme tempo and initialize their values
        foreach(Sound sound in themeSongTempos)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;

            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = true;
        }

        currentThemeSound = themeSongTempos[0];

        //Disable or enable the theme music based on player prefs
        if(PlayerPrefs.GetInt("PlayMusic") == 1)
        {
            currentThemeSound.source.volume = currentThemeSound.volume;
            audioImg.sprite = audioEnabledSpr;
        }
        else
        {
            currentThemeSound.source.volume = 0;
            audioImg.sprite = audioDisabledSpr;
        }

        Play(currentThemeSound.source);

        //Loop over each sound effect and initialize their values
        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;

            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
            if (sound.playOnAwake)
            {
                Play(sound.source);
            }
        }

        if (!playThemeOnAwake)
        {
            ToggleMusic();
        }

        if(PlayerPrefs.GetInt("PlayMusic") == 0)
        {
            SetVolumeForAllThemeTempos(0f);
        }
    }

    private void OnEnable()
    {
        EventManager.Instance.onThemeTemposChange += IncreaseThemeTempo;
        EventManager.Instance.onPlayerRespawn += ResetAllThemeTempos;
    }

    private void OnDisable()
    {
        EventManager.Instance.onThemeTemposChange -= IncreaseThemeTempo;
        EventManager.Instance.onPlayerRespawn -= ResetAllThemeTempos;

    }

    //Play a sound effect given a name
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
    }

    //Play a sound effect given a source
    void Play(AudioSource source)
    {
        source.Play();
    }

    //Play a sound effect with a slightly random pitch given a name
    public void PlayWithRandomPitch(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        float p = UnityEngine.Random.Range(-0.2f, 0.2f);

        s.source.pitch = s.pitch + p;
        Play(s.source);
        
    }

    //Increase the theme tempo
    void IncreaseThemeTempo()
    {
        currentThemeSound.source.loop = false;
        StartCoroutine(WaitBeforeTempoChange());
    }

    //Waits until the current theme tempo audio clip has finished and then increments the theme tempo
    IEnumerator WaitBeforeTempoChange()
    {
        yield return new WaitWhile(() => currentThemeSound.source.isPlaying);

        if (GameManager.Instance.isPlayerAlive)
        {
            if (currentTempoIndex < themeSongTempos.Length)
            {
                currentTempoIndex++;
                currentThemeSound.source.loop = true;
                currentThemeSound = themeSongTempos[currentTempoIndex];

            }

            Play(currentThemeSound.source);
        }
    }

    //Toggles the music on and off
    public void ToggleMusic()
    {
        if (PlayerPrefs.GetInt("PlayMusic") == 1)
        {
            PlayerPrefs.SetInt("PlayMusic", 0);
            audioImg.sprite = audioDisabledSpr;
            SetVolumeForAllThemeTempos(0);
        }
        else
        {
            PlayerPrefs.SetInt("PlayMusic", 1);
            audioImg.sprite = audioEnabledSpr;
            SetVolumeForAllThemeTempos(0.5f);
        }
    }

    //Loops through all theme tempos and sets their volume to a given value
    void SetVolumeForAllThemeTempos(float volume)
    {
        foreach(Sound s in themeSongTempos)
        {
            s.source.volume = volume;
            s.source.loop = true;
        }
    }

    //Stops the current theme tempo from playing
    public void StopTheme()
    {
        currentThemeSound.source.Stop();
    }

    //Resets the theme temp back to the slowest one
    public void ResetThemeTempo()
    {
        currentTempoIndex = 0;
        currentThemeSound = themeSongTempos[currentTempoIndex];
    }

    //Plays the current theme tempo
    public void PlayTheme()
    {
        Play(currentThemeSound.source);
    }

    //Loops through all theme tempos and sets them to loop
    void ResetAllThemeTempos()
    {
        foreach (Sound s in themeSongTempos)
        {
            s.source.loop = true;
        }
    }

}
