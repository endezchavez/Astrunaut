using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    bool isFirstLaunch = true;
    static bool isCreated = false;

    private void Awake()
    {
        if (!isCreated)
        {
            DontDestroyOnLoad(this.gameObject);
            isCreated = true;
            PlayerPrefs.SetInt("PlayMusic", 1);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

}
