using UnityEngine;

public class DebrisScrolling : MonoBehaviour
{
    // array of four debris objects to support endless scrolling
    private GameObject[] DebrisField = new GameObject[4];

    [SerializeField] float XVelocity, YVelocity;

    private void Awake()
    {
        // get the debris object from the child to use for the copies
        DebrisField[0] = transform.GetChild(0).gameObject;

        // copy to make four copies in the array
        for (int i = 1; i < 4; i++)
        {
            DebrisField[i] = Instantiate(DebrisField[0], transform);
        }

        // set their velocities for the scrolling
        for (int i = 0; i < 4; i++)
            DebrisField[i].GetComponent<Rigidbody2D>().linearVelocity = new Vector2(XVelocity, YVelocity);

        Camera cam = Camera.main;

        float midX = cam.ViewportToWorldPoint(new Vector3(0.5f, 0, 0)).x;
        float midY = cam.ViewportToWorldPoint(new Vector3(0, 0.5f, 0)).y;

        // set the positions of the debris objects to be equidistant around the center
        DebrisField[0].transform.position = new Vector2(midX - DebrisField[0].GetComponent<Renderer>().bounds.extents.x, midY + DebrisField[0].GetComponent<Renderer>().bounds.extents.y);
        DebrisField[1].transform.position = new Vector2(midX + DebrisField[1].GetComponent<Renderer>().bounds.extents.x, midY + DebrisField[1].GetComponent<Renderer>().bounds.extents.y);
        DebrisField[2].transform.position = new Vector2(midX - DebrisField[2].GetComponent<Renderer>().bounds.extents.x, midY - DebrisField[2].GetComponent<Renderer>().bounds.extents.y);
        DebrisField[3].transform.position = new Vector2(midX + DebrisField[3].GetComponent<Renderer>().bounds.extents.x, midY - DebrisField[3].GetComponent<Renderer>().bounds.extents.y);
    }

    void FixedUpdate()
    {
        Camera cam = Camera.main;

        float right = cam.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
        float top = cam.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;

        // move the right two debris objects to the left side when they move offscreen to the right
        if (DebrisField[1].transform.position.x - DebrisField[1].GetComponent<Renderer>().bounds.extents.x > right)
        {
            DebrisField[1].transform.position = new Vector2(DebrisField[0].transform.position.x - DebrisField[1].GetComponent<Renderer>().bounds.size.x, DebrisField[1].transform.position.y);
            DebrisField[3].transform.position = new Vector2(DebrisField[0].transform.position.x - DebrisField[3].GetComponent<Renderer>().bounds.size.x, DebrisField[3].transform.position.y);

            GameObject temp1 = DebrisField[1];
            GameObject temp2 = DebrisField[3];
            DebrisField[1] = DebrisField[0];
            DebrisField[3] = DebrisField[2];
            DebrisField[0] = temp1;
            DebrisField[2] = temp2;
        }

        // move the top two debris objects to the bottom side when they move offscreen to the top
        if (DebrisField[0].transform.position.y - DebrisField[0].GetComponent<Renderer>().bounds.extents.y > top)
        {
            DebrisField[0].transform.position = new Vector2(DebrisField[0].transform.position.x, DebrisField[2].transform.position.y - DebrisField[0].GetComponent<Renderer>().bounds.size.y);
            DebrisField[1].transform.position = new Vector2(DebrisField[1].transform.position.x, DebrisField[2].transform.position.y - DebrisField[1].GetComponent<Renderer>().bounds.size.y);

            GameObject temp1 = DebrisField[0];
            GameObject temp2 = DebrisField[1];
            DebrisField[0] = DebrisField[2];
            DebrisField[1] = DebrisField[3];
            DebrisField[2] = temp1;
            DebrisField[3] = temp2;
        }
    }
}
