using UnityEngine;

[CreateAssetMenu(fileName = "PasTEST", menuName = "Scriptable Objects/PasTEST")]
public class PasTEST : IEquipment
{
    public void Use()
    {
        Debug.Log("Using seconday/Miscellaneous");
    }
}


