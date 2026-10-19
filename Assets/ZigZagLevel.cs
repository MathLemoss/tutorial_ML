using UnityEngine;

public class ZigZagLevel : MonoBehaviour
{
    public Transform player;
    public Camera followCamera;
    public float startX = 0f;
    public float finishX = 84f;

    public float Progress => Mathf.InverseLerp(startX, finishX, player.position.x);

    void Awake()
    {
        GameController.Init(GameObject.FindGameObjectsWithTag("Coletavel").Length);
    }

    void Update()
    {
        if (GameController.State == GameController.MatchState.Playing && player.position.x >= finishX)
            GameController.Win();
    }

    void LateUpdate()
    {
        // Keep Bruno to the left of the screen, with more room to see incoming obstacles.
        float lookAhead = followCamera.orthographicSize * followCamera.aspect * 0.4f;
        followCamera.transform.position = new Vector3(player.position.x + lookAhead, 0f, -10f);
    }
}
