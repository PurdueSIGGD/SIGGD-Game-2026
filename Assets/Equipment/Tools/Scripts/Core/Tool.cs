using UnityEngine;

public abstract class Tool : MonoBehaviour
{
    [SerializeField] protected ToolData data;

    [SerializeField] float nextUseTime = 0;

    public void Use()
    {
        if (Time.time < nextUseTime) return;
        nextUseTime = Time.time + data.cooldown;
        OnUse();
    }

    public virtual void UseAlt() {}

    protected abstract void OnUse();

    // base equip method thatll get a definition in ProjectileTool and MeleeTool
    public virtual void Equip() {}
}
