using UnityEngine;

public abstract class ToolProjectile : MonoBehaviour
{
    public ToolProjectileData Data { get; private set; }
    public event System.Action<ToolProjectile> Finished;

    public virtual void Init(ToolProjectileData data, Vector2 direction)
    {
        Data = data;
    }

    protected void Finish()
    {
        Finished?.Invoke(this);
        Destroy(gameObject);
    }
}
