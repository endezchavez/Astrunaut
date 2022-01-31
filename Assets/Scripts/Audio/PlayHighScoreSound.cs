using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayHighScoreSound : MonoBehaviour
{
    [SerializeField]
    private float delay;
    [SerializeField]
    private string audioClipName;

    private void OnEnable()
    {
        EventManager.Instance.onHighScoreUpdated += PlayAudioWithDelay;
    }

    void OnDisable()
    {
        EventManager.Instance.onHighScoreUpdated -= PlayAudioWithDelay;
    }

    IEnumerator PlayAudio()
    {
        yield return new WaitForSeconds(delay);
        AudioManager.Instance.Play(audioClipName);
    }

    void PlayAudioWithDelay()
    {
        StartCoroutine(PlayAudio());
    }
}
