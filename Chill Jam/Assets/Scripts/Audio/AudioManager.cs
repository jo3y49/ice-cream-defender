using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour {
    public static AudioManager instance;
    private AudioSource audioSource;
    [SerializeField] private AudioClip waveStart, win, lose, enemyHit;
    [SerializeField] private AudioClip[] uiSounds;
    private void Awake() {
        if (instance == null)
        {
            instance = this;
            audioSource = GetComponent<AudioSource>();
            DontDestroyOnLoad(gameObject);
        }
        else 
        {
            Destroy(gameObject);
        }
    }

    public void StartWave()
    {
        PlayClip(waveStart);
    }

    public void Win()
    {
        PlayClip(win);
    }

    public void Lose()
    {
        PlayClip(lose);
    }

    public void EnemyHit()
    {
        PlayClip(enemyHit);
    }

    public void PlayUIClip(int i)
    {
        if (uiSounds.Length > i) PlayClip(uiSounds[i]);

        else PlayClip(uiSounds[0]);
    }

    private void PlayClip(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}