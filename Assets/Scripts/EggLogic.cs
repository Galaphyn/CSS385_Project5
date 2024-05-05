using UnityEngine;

public class EggLogic : MonoBehaviour
{
    public float speed = 40f;
    private GameEngine engine;

    // Start is called before the first frame update
    void Start()
    {
        engine = Object.FindAnyObjectByType<GameEngine>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.up * (speed * Time.smoothDeltaTime);
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Plane")
        {
            PlaneLogic plane = collision.GetComponent<PlaneLogic>();
            plane.hitPlane();
            Destroy(gameObject);
        }

        //Only hit waypoint if tag is right and it is not hidden.
        if (collision.gameObject.tag == "Waypoint" && !engine.wayHidden)
        {
            WaypointLogic waypoint = collision.GetComponent<WaypointLogic>();
            waypoint.hitWaypoint();
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
        engine.BadEgg();
    }
}
