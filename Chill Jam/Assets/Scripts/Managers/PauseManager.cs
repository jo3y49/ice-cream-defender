using UnityEngine;
using System;

public class PauseManager : MonoBehaviour {
    private PauseManager instance;
    public static event Action<bool> PauseEvent;

    private void Awake() {
        instance = this;
    }

    public static void TogglePause() {
        bool resume = Time.timeScale == 0;

        if (resume)
        {
            Time.timeScale = 1;
            PauseEvent.Invoke(false);
        }
        else
        {
            Time.timeScale = 0;
            PauseEvent.Invoke(true);
        }
    }
}