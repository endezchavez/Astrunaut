using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioWithDelay : MonoBehaviour
{
    [SerializeField]
    private float delay;
    [SerializeField]
    private string audioClipName;

    private void OnEnable()
    {
        StartCoroutine(PlayAudio());
    }

    IEnumerator PlayAudio()
    {
        yield return new WaitForSeconds(delay);
        AudioManager.Instance.Play(audioClipName);
    }
}
