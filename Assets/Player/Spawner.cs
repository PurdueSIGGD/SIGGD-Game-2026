using UnityEngine;

public class Spawner : MonoBehaviour
{
    // FOR TESTING PURPOSES ONLY
    [SerializeField] private GameObject asteroid;

    [SerializeField] private float spawnRate = 0.25f;
    private float t = 0;


    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        if (t >= spawnRate)
        {
            t = 0;
            Instantiate(asteroid, transform.position, Quaternion.identity);
        }
    }
}
