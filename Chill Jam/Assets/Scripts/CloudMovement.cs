using UnityEngine;

public class CloudMovement : MonoBehaviour {
    [SerializeField] private RectTransform cloudContainer;
    [SerializeField] private RectTransform[] clouds;

    public float speed = 20f;
    private float cloudReappearLocation;

    private void Awake() {
        cloudReappearLocation = cloudContainer.rect.width/2;
    }

    private void Update() 
    {
        foreach (RectTransform r in clouds)
        {
            r.anchoredPosition += speed * Time.deltaTime * Vector2.left;

            if (r.anchoredPosition.x + r.rect.width/2 < -cloudReappearLocation)
                r.anchoredPosition = new Vector2(cloudReappearLocation + r.rect.width/2, r.anchoredPosition.y);
        }
    }
}