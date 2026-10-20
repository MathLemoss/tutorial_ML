using System.Collections.Generic;
using UnityEngine;

public class CorridorExtension : MonoBehaviour
{
    public Transform player;
    public Camera followCamera;
    public GameObject upperWallTemplate;
    public GameObject lowerWallTemplate;
    public GameObject notePrefab;
    public float existingEndX = 92.5f;

    [Header("Largura do corredor")]
    [Min(1f)] public float initialHalfWidth = 13f;
    [Min(1f)] public float halfWidth = 1.65f;
    [Min(1f)] public float narrowingDuration = 192.52827f;

    public float noteSpacing = 40.841415f;
    public float nextNoteX = 368.57272f;

    private float builtThrough;
    private float horizontalSpeed;
    private float playerStartX;
    private readonly Queue<GameObject> generated = new Queue<GameObject>();

    void Start()
    {
        horizontalSpeed = Mathf.Max(0.1f, player.GetComponent<PlayerMovement>().speed) / Mathf.Sqrt(2f);
        ConfigureInitialCourse();
        builtThrough = existingEndX;
        Extend();
    }

    void Update()
    {
        if (!GameController.gameOver)
            Extend();
    }

    private static float CenterY(float x)
    {
        return 2.5f - Mathf.Abs(Mathf.Repeat(x + 2.5f, 10f) - 5f);
    }

    private float HalfWidthAt(float x)
    {
        // Com velocidade constante, a posicao indica o tempo de chegada a cada curva.
        float arrivalTime = Mathf.Max(0f, x - playerStartX) / horizontalSpeed;
        float remaining = 1f - Mathf.Clamp01(arrivalTime / Mathf.Max(1f, narrowingDuration));
        // As paredes entram no campo de visao cedo e fecham suavemente ate o fim.
        return Mathf.Lerp(halfWidth, initialHalfWidth, remaining * remaining);
    }

    private void ConfigureInitialCourse()
    {
        playerStartX = player.position.x;
        SequentialMusicPlayer music = GetComponent<SequentialMusicPlayer>();
        if (music != null && music.HasTrack)
            noteSpacing = music.Duration / music.SegmentCount * 0.9f * horizontalSpeed;

        var notes = new List<Transform>();
        foreach (Transform child in transform)
        {
            if (child.name.StartsWith("Parede superior") || child.name.StartsWith("Parede inferior"))
            {
                float centerX = child.localPosition.x;
                ConfigureWall(child, centerX - 2.5f, centerX + 2.5f,
                    child.name.StartsWith("Parede superior") ? 1f : -1f);
            }
            else if (child.CompareTag("Coletavel"))
                notes.Add(child);
        }

        // A velocidade pode mudar no Inspector sem adiantar as notas em relacao ao audio.
        notes.Sort((a, b) => a.localPosition.x.CompareTo(b.localPosition.x));
        for (int i = 0; i < notes.Count; i++)
        {
            float x = playerStartX + 1f + i * noteSpacing;
            notes[i].localPosition = new Vector3(x, CenterY(x), 0f);
            notes[i].GetComponent<CollectableNote>().Initialize(player);
        }
        nextNoteX = playerStartX + 1f + notes.Count * noteSpacing;
    }

    private void Extend()
    {
        float ahead = player.position.x + Mathf.Max(30f, followCamera.orthographicSize * followCamera.aspect * 2f);
        while (builtThrough < ahead)
        {
            float next = builtThrough + 5f;
            CreateWall(upperWallTemplate, builtThrough, next, 1f);
            CreateWall(lowerWallTemplate, builtThrough, next, -1f);
            builtThrough = next;
        }

        while (nextNoteX < ahead)
        {
            GameObject note = Instantiate(notePrefab, transform);
            note.name = "Nota musical";
            note.transform.localPosition = new Vector3(nextNoteX, CenterY(nextNoteX), 0f);
            note.transform.localScale = new Vector3(0.6f, 0.6f, 1f);
            note.GetComponent<SpriteRenderer>().color = new Color(1f, 0.8f, 0.15f);
            note.GetComponent<CollectableNote>().Initialize(player);
            generated.Enqueue(note);
            GameController.RegisterCollectable();
            nextNoteX += Mathf.Max(1f, noteSpacing);
        }

        while (generated.Count > 0)
        {
            GameObject first = generated.Peek();
            if (first != null && first.transform.position.x > player.position.x - 40f)
                break;
            generated.Dequeue();
            if (first != null)
                Destroy(first);
        }
    }

    private void CreateWall(GameObject template, float fromX, float toX, float side)
    {
        GameObject wall = Instantiate(template, transform);
        wall.name = template.name.Contains("superior") ? "Parede superior" : "Parede inferior";
        ConfigureWall(wall.transform, fromX, toX, side);
        generated.Enqueue(wall);
    }

    private void ConfigureWall(Transform wall, float fromX, float toX, float side)
    {
        Vector2 from = new Vector2(fromX, CenterY(fromX) + side * HalfWidthAt(fromX));
        Vector2 to = new Vector2(toX, CenterY(toX) + side * HalfWidthAt(toX));
        Vector2 delta = to - from;
        wall.localPosition = (from + to) / 2f;
        wall.localRotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg);
        wall.localScale = new Vector3(delta.magnitude + 0.45f, 0.45f, 1f);
    }
}
