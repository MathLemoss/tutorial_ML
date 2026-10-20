using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[DefaultExecutionOrder(100)]
public class CharacterTrail : MonoBehaviour
{
    public Material trailMaterial;
    [Min(0.01f)] public float duration = 0.18f;
    [Min(0.01f)] public float width = 0.26f;
    public Color trailColor = Color.white;

    private TrailRenderer movementTrail;

    void Start()
    {
        // Start depois do posicionamento inicial de Marrone evita um risco no spawn.
        SpriteRenderer character = GetComponent<SpriteRenderer>();
        movementTrail = gameObject.AddComponent<TrailRenderer>();
        movementTrail.emitting = false;
        movementTrail.sharedMaterial = trailMaterial;
        movementTrail.time = duration;
        movementTrail.minVertexDistance = 0.04f;
        movementTrail.startWidth = width;
        movementTrail.endWidth = 0f;
        movementTrail.numCornerVertices = 2;
        movementTrail.numCapVertices = 2;
        movementTrail.alignment = LineAlignment.View;
        movementTrail.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        movementTrail.receiveShadows = false;
        movementTrail.sortingLayerID = character.sortingLayerID;
        movementTrail.sortingOrder = character.sortingOrder - 1;
        Color color = trailColor;
        movementTrail.startColor = new Color(color.r, color.g, color.b, 0.65f);
        movementTrail.endColor = new Color(color.r, color.g, color.b, 0f);
        movementTrail.Clear();
    }

    void LateUpdate()
    {
        // A captura tambem deixa um rastro; quando param, ele desaparece sozinho.
        movementTrail.emitting = GameController.State == GameController.MatchState.Playing
            || GameController.State == GameController.MatchState.Lost;
    }

    void OnDisable()
    {
        if (movementTrail == null)
            return;
        movementTrail.emitting = false;
        movementTrail.Clear();
    }
}
