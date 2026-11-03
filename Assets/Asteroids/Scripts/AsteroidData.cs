using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AsteroidData", menuName = "Scriptable Objects/AsteroidData")]
public class AsteroidData : ScriptableObject
{
    private const float MinPositiveValue = 0.01f;

    // Stats

    [field: SerializeField, Tooltip("Average Rigidbody2D mass. Each asteroid is randomized around this value.")]
    public float BaseMass { get; private set; } = 1.0f;

    [field: SerializeField, Tooltip("Average uniform scale in world units. Randomized per asteroid.")]
    public float BaseSize { get; private set; } = 1.0f;

    [field: SerializeField, Tooltip("Sprite for fragments of this type. Spawned asteroids keep their prefab's sprite.")]
    public Sprite AsteroidSprite { get; private set; }

    [field: SerializeField, Tooltip("Health at spawn. The asteroid is destroyed when it reaches zero.")]
    public float BaseMaxHealth { get; private set; } = 100.0f;

    [field: SerializeField, Tooltip("Random +/- fraction applied to mass (0.2 means 20 percent).")]
    public float MassVariationPercentage { get; private set; } = 0.2f;

    [field: SerializeField, Tooltip("Random +/- fraction applied to size (0.2 means 20 percent).")]
    public float SizeVariationPercentage { get; private set; } = 0.2f;

    [field: SerializeField, Tooltip("Largest random starting spin in degrees per second (spawned asteroids only).")]
    public float MaxInitialSpin { get; private set; } = 30.0f;

    // Destruction

    [field: SerializeField, Tooltip("Number of fragments this asteroid breaks into. 0 means it just explodes.")]
    public int DebrisCount { get; private set; } = 3;

    [field: SerializeField, Tooltip("Prefab used for each fragment. The asteroid prefab itself works.")]
    public AsteroidController FragmentPrefab { get; private set; }

    [field: SerializeField, Tooltip("Data applied to fragments. Give it a DebrisCount of 0 to stop the splitting.")]
    public AsteroidData FragmentData { get; private set; }

    [field: SerializeField, Tooltip("Explosion effect spawned when this asteroid dies. Optional.")]
    public ExplosionEffect ExplosionPrefab { get; private set; }

    [field: SerializeField, Tooltip("Starting size of the explosion sprite relative to the dying asteroid's size.")]
    public float ExplosionScale { get; private set; } = 1.0f;

    [field: SerializeField, Tooltip("Impulse each fragment receives from the blast (mass times velocity).")]
    public float ExplosionImpulse { get; private set; } = 1.0f;

    [field: SerializeField, Tooltip("Random +/- fraction applied to each fragment's impulse (0.25 means 25 percent).")]
    public float ImpulseVariationPercentage { get; private set; } = 0.25f;

    [field: SerializeField, Tooltip("How far from the center fragments appear, as a fraction of the parent's size.")]
    public float FragmentSpawnRadius { get; private set; } = 0.3f;

    private void OnValidate()
    {
        BaseMass = Mathf.Max(MinPositiveValue, BaseMass);
        BaseSize = Mathf.Max(MinPositiveValue, BaseSize);
        BaseMaxHealth = Mathf.Max(MinPositiveValue, BaseMaxHealth);
        MaxInitialSpin = Mathf.Max(0.0f, MaxInitialSpin);
        DebrisCount = Mathf.Max(0, DebrisCount);
        FragmentSpawnRadius = Mathf.Max(0.0f, FragmentSpawnRadius);

        ValidateFragmentSetup();
    }

    private void ValidateFragmentSetup()
    {
        if (DebrisCount > 0 && FragmentPrefab == null)
        {
            Debug.LogWarning(
                $"{name}: DebrisCount is {DebrisCount} but FragmentPrefab is empty, so nothing will spawn.",
                this);
        }

        if (DebrisCount > 0 && FragmentData == null)
        {
            Debug.LogWarning(
                $"{name}: DebrisCount is {DebrisCount} but FragmentData is empty, so nothing will spawn.",
                this);
        }

        HashSet<AsteroidData> visited = new HashSet<AsteroidData> { this };
        AsteroidData next = FragmentData;
        while (next != null)
        {
            if (!visited.Add(next))
            {
                Debug.LogError(
                    $"{name}: the FragmentData chain loops back to {next.name}. Asteroids would split forever.",
                    this);
                break;
            }

            next = next.FragmentData;
        }
    }
}
