using UnityEngine;

public class EnemyInfo : MonoBehaviour {
    public int turnCount;

    [SerializeField] private Path path;

    public float distance = 100;

    private void Start() {
        transform.position = path.GetLocation(distance);
    }

    public void NextTurn()
    {
        float travelDistance = 100/turnCount;

        distance -= travelDistance;

        transform.position = path.GetLocation(distance);
    }
}