using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Min(0.1f)]
    public float speed = 5f;
    private Rigidbody2D rb;
    private AudioSource collectAudio;
    private int verticalDirection = 1;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        collectAudio = GetComponent<AudioSource>();
        rb.gravityScale = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    void Update()
    {
        if (GameController.gameOver)
            return;

        // Read button edges per rendered frame so quick presses are not lost.
        bool pressed = (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
            || (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame);
        if (!pressed)
            return;

        if (GameController.State == GameController.MatchState.Ready)
            GameController.Begin();
        else
            verticalDirection *= -1;
    }

    void FixedUpdate()
    {
        if (GameController.State != GameController.MatchState.Playing)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        GameController.Tick(Time.fixedDeltaTime);
        Vector2 direction = new Vector2(1f, verticalDirection).normalized;
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (GameController.State != GameController.MatchState.Playing)
            return;

        GameController.Lose();
        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;
        if (collectAudio != null)
            collectAudio.Stop();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (GameController.State == GameController.MatchState.Playing && other.CompareTag("Coletavel"))
        {
            other.gameObject.SetActive(false);
            if (collectAudio != null && collectAudio.clip != null)
                collectAudio.Play();
            GameController.Collect();
            Destroy(other.gameObject);
        }
    }
}
