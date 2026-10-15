using UnityEngine;

public class SwordVisualEffects : MonoBehaviour
{
    private static Color[] swipeColors = new Color[3]
    {
        new Color(1f, 0.8f, 0.3f, 0.6f),  // Golden yellow
        new Color(0.8f, 1f, 0.3f, 0.6f),  // Lime green
        new Color(1f, 0.6f, 0.2f, 0.6f)   // Orange
    };

    /// <summary>
    /// Gets a random swipe color with variation for visual interest
    /// </summary>
    public static Color GetRandomSwipeColor()
    {
        return swipeColors[Random.Range(0, swipeColors.Length)];
    }

    /// <summary>
    /// Creates a hit particle effect at a specific position
    /// </summary>
    public static void CreateHitParticles(Vector3 position, int particleCount = 8)
    {
        for (int i = 0; i < particleCount; i++)
        {
            GameObject particle = new GameObject("HitParticle");
            particle.transform.position = position;

            SpriteRenderer renderer = particle.AddComponent<SpriteRenderer>();
            renderer.color = new Color(1f, 1f, 0.5f, 0.8f);

            Rigidbody2D rb = particle.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0.3f;
            rb.linearVelocity = Random.insideUnitCircle * 5f;

            // Destroy after a short time
            Destroy(particle, 0.5f);
        }
    }

    /// <summary>
    /// Creates a screen shake effect for impact feedback
    /// </summary>
    public static void ScreenShake(Camera camera, float intensity = 0.1f, float duration = 0.1f)
    {
        if (camera == null) return;

        // Simple screen shake implementation
        // This can be expanded with a dedicated camera shake system
    }

    /// <summary>
    /// Creates a visual feedback arc at specified angle and range
    /// </summary>
    public static GameObject CreateArcVisual(Vector3 origin, float angle, float range)
    {
        GameObject arcObject = new GameObject("ArcVisual");
        arcObject.transform.position = origin;

        LineRenderer lineRenderer = arcObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 20;
        lineRenderer.startColor = new Color(1f, 0.8f, 0.3f, 0.5f);
        lineRenderer.endColor = new Color(1f, 0.8f, 0.3f, 0f);
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.05f;

        // Draw arc
        float startAngle = angle - 45f;
        for (int i = 0; i < 20; i++)
        {
            float currentAngle = startAngle + (90f * i / 19f);
            float rad = currentAngle * Mathf.Deg2Rad;
            float x = Mathf.Cos(rad) * range;
            float y = Mathf.Sin(rad) * range;
            lineRenderer.SetPosition(i, new Vector3(x, y, 0));
        }

        // Destroy after short duration
        Destroy(arcObject, 0.3f);

        return arcObject;
    }
}
