public class DogController : MonoBehaviour {
    public int dogSpeed = 5;

    public void Awake()
    {
        dogSpeed = 10;
    }

    public void Update(){
        Debug.Log("This is super cool!");

        bool is_ok = true;
    }
}
