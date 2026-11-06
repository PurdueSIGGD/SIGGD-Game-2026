using Sirenix.OdinInspector;
using UnityEngine;

namespace PlayerSystem
{
    [RequireComponent(typeof(PlayerController))]
    public class Player : MonoBehaviour
    {
        [ShowInInspector, ReadOnly] public int PlayerIndex { get; private set; }

        // Player Components
        public PlayerController Controller { get; private set; }
        public PlayerHealth Health { get; private set; }


        void Awake()
        {
            PlayerIndex = PlayerRegistry.RegisterPlayer(this);
            if (PlayerIndex == -1)
            {
                Debug.LogError("Max player count exceeded, cannot register player");
                Destroy(gameObject);
            }

            Controller = GetComponent<PlayerController>();
            Health = GetComponent<PlayerHealth>();
        }

        void OnDestroy()
        {
            PlayerRegistry.UnregisterPlayer(this);
        }
    }
}