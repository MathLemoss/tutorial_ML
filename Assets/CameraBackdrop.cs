using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CameraBackdrop : MonoBehaviour
{
    public Camera targetCamera;
    private SpriteRenderer backdrop;

    void Awake()
    {
        backdrop = GetComponent<SpriteRenderer>();
        FitCamera();
    }

    void LateUpdate()
    {
        FitCamera();
    }

    private void FitCamera()
    {
        if (targetCamera == null || backdrop.sprite == null)
            return;
        Vector2 size = backdrop.sprite.bounds.size;
        float height = targetCamera.orthographicSize * 2f;
        float scale = Mathf.Max(height / size.y, height * targetCamera.aspect / size.x);
        transform.localScale = new Vector3(scale, scale, 1f);
    }
}
