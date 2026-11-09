using UnityEngine;

public class PlanetRotation : MonoBehaviour
{
    private Rigidbody2D rb2d;
    public float SpinSpeed = 5f;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.angularVelocity = SpinSpeed;
    }
}
