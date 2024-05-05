using System.Collections;
using UnityEngine;

public class PlaneLogic : MonoBehaviour
{
    private float speed = 30f;
    private int health = 60;
    private int curWaypoint;
    public GameObject[] waypoints;
    private GameEngine engine;
    private SpriteRenderer sprite;
    private Vector2 target;
    private bool chase = false;
    private int hit = 0;

    private GameObject player;
    public Sprite egg;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        engine = Object.FindAnyObjectByType<GameEngine>();
        curWaypoint = Random.Range(0, waypoints.Length);
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, player.transform.position) >= 40f)
        {
            chase = false;
        }

        if(!chase && hit == 0)
        {
            //Get and travel to current position of waypoint from game engine
            target = engine.curPosition[curWaypoint];
            transform.position = Vector2.MoveTowards(transform.position, target, Time.deltaTime * speed);

            //If within .01 unit of waypoint, get next waypoint
            if (Vector2.Distance(transform.position, target) <= .01f)
            {
                if (engine.isSequential)
                {
                    curWaypoint++;
                    if (curWaypoint >= waypoints.Length)
                    {
                        curWaypoint = 0;
                    }
                }
                else
                {
                    curWaypoint = Random.Range(0, waypoints.Length);
                }
            }
        }
        else if (chase && hit == 0)
        {
            target = player.transform.position;
            transform.position = Vector2.MoveTowards(transform.position, target, Time.deltaTime * speed);
        }
        else if (hit == 1)
        {
            transform.Rotate(0, 0, 1);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && hit == 0)
        {
            StartCoroutine(RotatePlane());
        }
    }

    IEnumerator RotatePlane()
    {
        // Rotate counter-clockwise 180 degrees
        for (int i = 0; i < 180; i++)
        {
            transform.Rotate(0, 0, -1);
            yield return null;
        }

        // Rotate clockwise 180 degrees
        for (int i = 0; i < 180; i++)
        {
            transform.Rotate(0, 0, 1);
            yield return null;
        }

        chase = true;
    }
    

    public void hitPlane()
    {
        health -= 20;
        hit++;

        Color color = sprite.color;
        color.a *= 0.8f;
        sprite.color = color;

        if (hit == 2)
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = egg;
        }

        if (health <= 0)
        {
            Debug.Log("Destroyed Plane!");
            engine.PlaneDestroyed();
            Destroy(gameObject);
        }
    }
    
}
