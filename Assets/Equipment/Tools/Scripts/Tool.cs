using UnityEngine;

public abstract class Tool : MonoBehaviour
{
    [SerializeField] protected ToolData data;

    float nextUseTime;

    public void Use()
    {
        if (Time.time < nextUseTime) return;
        nextUseTime = Time.time + data.cooldown;
    }

    public virtual void UseAlt() {}

    protected abstract void OnUse();
}
