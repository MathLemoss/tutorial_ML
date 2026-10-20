using System.Collections.Generic;
using UnityEngine;

public class MarroneFollower : MonoBehaviour
{
    public Transform player;
    [Min(1f)] public float safeDistance = 3.5f;
    [Min(0.8f)] public float warningDistance = 1.1f;
    [Min(1f)] public float approachSpeed = 5f;
    [Min(1f)] public float captureSpeed = 16f;

    private readonly List<Vector3> trail = new List<Vector3>();
    private float currentDistance;

    void Start()
    {
        currentDistance = safeDistance;
        transform.position = player.position - Vector3.right * safeDistance;
        trail.Add(transform.position);
        trail.Add(player.position);
    }

    void LateUpdate()
    {
        if (GameController.State == GameController.MatchState.Lost)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position,
                captureSpeed * Time.unscaledDeltaTime);
            return;
        }
        if (GameController.State != GameController.MatchState.Playing)
            return;

        Vector3 position = player.position;
        if (position.x > trail[trail.Count - 1].x)
            trail.Add(position);

        float targetDistance = GameController.ConsecutiveMisses == 0 ? safeDistance : warningDistance;
        currentDistance = Mathf.MoveTowards(currentDistance, targetDistance, approachSpeed * Time.deltaTime);
        transform.position = SampleTrail(position.x - currentDistance);

        // Guarde caminho suficiente para ele recuar quando Bruno pegar uma nota.
        int obsolete = 0;
        float oldestX = position.x - safeDistance - 2f;
        while (obsolete + 1 < trail.Count && trail[obsolete + 1].x < oldestX)
            obsolete++;
        if (obsolete > 0)
            trail.RemoveRange(0, obsolete);
    }

    private Vector3 SampleTrail(float x)
    {
        for (int i = 1; i < trail.Count; i++)
        {
            if (trail[i].x >= x)
            {
                float t = Mathf.InverseLerp(trail[i - 1].x, trail[i].x, x);
                return Vector3.Lerp(trail[i - 1], trail[i], t);
            }
        }
        return trail[trail.Count - 1];
    }
}
