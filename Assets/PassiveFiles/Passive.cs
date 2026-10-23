using System.Collections.Generic;
using UnityEngine;

public enum StatModifyMode
{
    Flat,
    Percent
}

[CreateAssetMenu(fileName = "Passive", menuName = "Passives/Passive")]
public class Passive : ScriptableObject, IEquipment
{
    [SerializeField] private string passiveName;
    [TextArea]
    [SerializeField] private string description;

    //this will contain a abstract code that will show what to add and remove
    [Tooltip("This gives a percentage buff or flat buff provided by passive")]
    public float buff;
    [Tooltip("The number of additional asteroids spawned per wave.")]
    public float debuff;
    public StatModifyMode mode;

    public void Use()
    {

    }
}