using UnityEngine;

[CreateAssetMenu(fileName = "AsteroidData", menuName = "Scriptable Objects/AsteroidData")]
public class AsteroidData : ScriptableObject
{
    //Physics and Visuals
    public float baseMass = 1.0f;
    public float baseSize = 1.0f;
    public Sprite asteroidSprite;

    //Gameplay (Maybe add a score value for destroying asteroids)
    public float baseMaxHealth = 100.0f;

    //Destruction
    public int debrisCount = 3;

    //Randomization
    public float massVariationPercentage = 0.2f;
    public float sizeVariationPercentage = 0.2f;
}
