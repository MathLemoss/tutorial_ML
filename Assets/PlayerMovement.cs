using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;
    AudioSource audio;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audio = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        Vector2 movement = Vector2.zero;

        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
            movement.x = -1;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            movement.x = 1;
        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
            movement.y = -1;
        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
            movement.y = 1;

        rb.MovePosition(rb.position + movement.normalized * speed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Coletavel")
        {
            audio.Play();
            GameController.Collect();
            Destroy(other.gameObject);
        }
    }
}