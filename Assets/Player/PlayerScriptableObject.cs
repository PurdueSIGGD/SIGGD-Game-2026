using UnityEngine;

//An attempt at creating scriptable objects for player movement/physcs

[CreateAssetMenu(fileName ="PlayerStats", menuName = "Player/Stats")]
public class PlayerScriptableObject : ScriptableObject
{
    [SerializeField] private float moveSpeed = 20f;
    [SerializeField] private float mass = 2f;
    [SerializeField] private float acceleration = 20f;
    [SerializeField] private float terminalVelocity = 10f;
    [SerializeField] private bool brake;
    public float MoveSpeed => moveSpeed;
    public float Mass => mass;
    public float Acceleration => acceleration;
    public float TerminalVelocity => terminalVelocity;
    public bool Brake => brake;
    

    
}
