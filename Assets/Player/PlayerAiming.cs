using Player;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAiming : MonoBehaviour
{
    private Camera mainCam;
    public bool CanLook = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCam = Camera.main;
        InputManager.Instance.OnPlayerLook.AddListener(RotatePlayerArm);
    }

    // Update is called once per frame
    void Update()
    {
        //mousePos = ;
        //mousePos.z = 0f;
        //Vector3 mousePos = Mouse.current.position.ReadValue();
        //mousePos.z = Camera.main.nearClipPlane;
        //worldPos = Camera.main.ScreenToWorldPoint(mousePos);


        //Debug.Log(Mouse.current.position.ReadValue());

        //Vector3 direction = worldPos - transform.position;
        ////print(direction);
        //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //Debug.Log(angle);

        //transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void RotatePlayerArm(Vector2 target)
    {
        Debug.Log(target);

        Vector2 playerPos = PlayerRegistery.Players[0].transform.position;

        Vector2 direction = target - playerPos;
        //print(direction);
        //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        ////Debug.Log(angle);

        //transform.rotation = Quaternion.Euler(0, 0, angle);
        transform.up = -direction;
    }
}
