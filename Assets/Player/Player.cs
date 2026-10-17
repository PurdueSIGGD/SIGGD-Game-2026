using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[DefaultExecutionOrder(-1)]
public class Player : MonoBehaviour
{
    [ShowInInspector, ReadOnly] public int PlayerIndex { get; private set; }

    // Player Components
    public PlayerController Controller { get; private set; }


    void Awake()
    {
        PlayerIndex = PlayerRegistery.RegisterPlayer(this);
        if (PlayerIndex == -1)
        {
            Debug.LogError("Max player count exceeded, cannot register player");
            Destroy(gameObject);
        }

        Controller = GetComponent<PlayerController>();
    }
}