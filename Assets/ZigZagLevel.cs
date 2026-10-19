using UnityEngine;

public class ZigZagLevel : MonoBehaviour
{
    public Transform player;
    public Camera followCamera;
    public float startX = 0f;
    public float finishX = 84f;
    public SequentialMusicPlayer music;

    public float Progress => music != null && music.HasTrack
        ? music.Progress : Mathf.InverseLerp(startX, finishX, player.position.x);

    void Awake()
    {
        GameController.Init(GameObject.FindGameObjectsWithTag("Coletavel").Length);
    }

    void Update()
    {
        bool finished = music != null && music.HasTrack ? music.IsComplete : player.position.x >= finishX;
        if (GameController.State == GameController.MatchState.Playing && finished)
            GameController.Win();
    }

    void LateUpdate()
    {
        float lookAhead = followCamera.orthographicSize * followCamera.aspect * 0.4f;
        // Na area inicial ampla, acompanhe Bruno se ele se afastar do centro.
        float verticalMargin = followCamera.orthographicSize * 0.4f;
        float cameraY = player.position.y - Mathf.Clamp(player.position.y, -verticalMargin, verticalMargin);
        followCamera.transform.position = new Vector3(player.position.x + lookAhead, cameraY, -10f);
    }
}
