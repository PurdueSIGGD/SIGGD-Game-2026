using UnityEngine;

public class turn : MonoBehaviour
{
    private Rigidbody2D rb2d;
    public float SpinSpeed = 5f;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.angularVelocity = SpinSpeed;
    }
}
