using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTrigger : Trigger {
    public int scene = 0;

    protected override void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(scene);
        }
    }
}