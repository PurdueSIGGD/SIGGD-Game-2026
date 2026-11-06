using UnityEngine;

public interface IEquipment
{
    // this is only for misc weapons and it is what should happen when the item is used
    // ie. blowing up a bomb, activating the magnet or reeling in the grappling hook
    public void Use();
}
