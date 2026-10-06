using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputManager.Instance.OnPlayerMove.AddListener(MovePlayerCharacter);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MovePlayerCharacter(Vector2 moveVector)
    {
        // create a gameobject in scene, call it player, attach this script
        // add a rigidbody2d to the player, find an api that applies force to that rigidbody
        // serialize a rigidbody2d field, drag it in from the editor. 
    }
}
