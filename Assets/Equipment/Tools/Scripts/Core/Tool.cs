using UnityEngine;

public abstract class Tool : MonoBehaviour
{
    [SerializeField] protected ToolData data;

    [SerializeField] float nextUseTime = 0; // this is a fucking quantum variable like fuck abstract classes dude

    public void Use()
    {
        Debug.Log("in use");
        Debug.Log("Time: " + Time.time + " next use time: " + nextUseTime);
        if (Time.time < nextUseTime) return;
        nextUseTime = Time.time + data.cooldown;
        Debug.Log("on use called");
        OnUse();
    }

    public virtual void UseAlt() {}

    protected abstract void OnUse();

    // base equip method thatll get a definition in ProjectileTool and MeleeTool
    public virtual void Equip() {}
}
