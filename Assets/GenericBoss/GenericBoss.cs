using UnityEngine;
using UnityEngine.Events;

public class GenericBoss : MonoBehaviour
{
    [SerializeField] private double health;
    public UnityEvent<double> CauseBossDamage = new UnityEvent<double>();

    void DamageBoss(double damage)
    {
        health -= damage;
    }

    public double GetBossHealth()
    {
        return health;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CauseBossDamage.AddListener(DamageBoss);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
