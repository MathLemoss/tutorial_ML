using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CollectableNote : MonoBehaviour
{
    private Collider2D noteCollider;
    private Collider2D playerCollider;
    private bool resolved;

    void Awake()
    {
        noteCollider = GetComponent<Collider2D>();
    }

    public void Initialize(Transform player)
    {
        playerCollider = player.GetComponent<Collider2D>();
    }

    public bool TryCollect()
    {
        if (resolved || GameController.State != GameController.MatchState.Playing)
            return false;
        resolved = true;
        return true;
    }

    void Update()
    {
        if (resolved || playerCollider == null || GameController.State != GameController.MatchState.Playing)
            return;

        // So conta como perdida depois que Bruno ultrapassa a nota inteira.
        // O teste usa os limites dos colliders para nao punir uma coleta na borda.
        if (playerCollider.bounds.min.x > noteCollider.bounds.max.x + 0.05f)
        {
            resolved = true;
            GameController.MissNote();
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
