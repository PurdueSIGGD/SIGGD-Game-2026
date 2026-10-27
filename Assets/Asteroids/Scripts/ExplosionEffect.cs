using UnityEngine;

/// <summary>
/// A sprite that grows slightly and fades out, then returns itself to the pool. Spawned when an asteroid dies so the
/// fade reveals the fragments underneath.
/// </summary>
[DisallowMultipleComponent]
[RequireComponent(typeof(SpriteRenderer))]
public class ExplosionEffect : MonoBehaviour
{
    private const float MinDuration = 0.01f;

    [Tooltip("Seconds the whole effect lasts.")]
    [SerializeField] private float duration = 0.6f;

    [Tooltip("Scale at the end of the effect relative to the starting scale.")]
    [SerializeField] private float endScaleMultiplier = 1.3f;

    [Tooltip("Alpha over time (X is progress 0 to 1). Holds opaque first so fragments stay hidden.")]
    [SerializeField] private AnimationCurve alphaCurve = new AnimationCurve(
        new Keyframe(0.0f, 1.0f), new Keyframe(0.3f, 1.0f), new Keyframe(1.0f, 0.0f));

    private SpriteRenderer spriteRenderer;
    private float startScale = 1.0f;
    private Vector2 drift;
    private float elapsed;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        elapsed += Time.deltaTime;
        float progress = Mathf.Clamp01(elapsed / duration);

        transform.position += (Vector3)(drift * Time.deltaTime);
        ApplyFrame(progress);

        if (progress >= 1.0f)
        {
            PrefabPool.Release(gameObject);
        }
    }

    private void OnValidate()
    {
        duration = Mathf.Max(MinDuration, duration);
    }

    /// <summary>Starts the effect. Call right after getting it from PrefabPool.</summary>
    /// <param name="baseSize">Starting scale of the sprite.</param>
    /// <param name="driftVelocity">Velocity to move with, so the blast travels with the debris.</param>
    public void Play(float baseSize, Vector2 driftVelocity)
    {
        elapsed = 0.0f;
        startScale = baseSize;
        drift = driftVelocity;
        ApplyFrame(0.0f);
    }

    private void ApplyFrame(float progress)
    {
        float scale = Mathf.Lerp(startScale, startScale * endScaleMultiplier, progress);
        transform.localScale = new Vector3(scale, scale, 1.0f);

        Color color = spriteRenderer.color;
        color.a = alphaCurve.Evaluate(progress);
        spriteRenderer.color = color;
    }
}
