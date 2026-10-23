using System;
using UnityEngine;

[CreateAssetMenu(fileName = "MisTEST", menuName = "Scriptable Objects/MisTEST")]
public class MisTEST : IEquipment
{
    public void Use()
    {
        Debug.Log("Using seconday/Miscellaneous");
    }
}
