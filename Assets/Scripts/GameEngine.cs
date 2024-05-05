using UnityEngine;
using UnityEngine.UI;

public class GameEngine : MonoBehaviour
{
    public int maxPlanes = 10;
    private int numPlanes = 0;
    private int planeDestroyed = 0;
    private int numberEggs = 0;

    public bool isSequential = true;
    public bool mouseMode = true;
    public Text text;

    public GameObject[] waypoints;
    public int maxWaypoints = 6;
    private Vector2[] origPosition;
    public Vector2[] curPosition;
    public bool wayHidden = false;
    // Start is called before the first frame update
    void Start()
    {
        //Initialize waypoints
        origPosition = new Vector2[maxWaypoints];
        curPosition = new Vector2[maxWaypoints];
        for (int i = 0; i < waypoints.Length; i++)
        {
            origPosition[i] = waypoints[i].transform.position;
            curPosition[i] = origPosition[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Quit Game
        if (Input.GetKey(KeyCode.Q))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
        //

        //Text Editor
        text.text = "Planes Destroyed: " + planeDestroyed + " || Hero Mode: " + (mouseMode ? "Mouse" : "Keyboard") 
            + " || Number of Eggs: " + numberEggs + " || Number of Planes: " + numPlanes
            + " || " + (isSequential ? "Sequential" : "Random") + " Mode || Waypoints: " + (wayHidden ? "Hidden" : "Shown"); 

        //Plane Spawner
        if (numPlanes < maxPlanes)
        {
            float camHeight = Camera.main.orthographicSize;
            float camWidth = Camera.main.aspect * camHeight;

            GameObject newPlane = Instantiate(Resources.Load("Prefabs/Plane") as GameObject);   
            Vector2 spawnPosition = new Vector2(Random.Range(-camWidth, camWidth), Random.Range(-camHeight, camHeight));

            newPlane.transform.position = spawnPosition;
            numPlanes++;
        }
        //

        //PlaneMovementToggle
        if (Input.GetKeyDown(KeyCode.J))
        {
            isSequential = !isSequential;
        }

        //Waypoint Hider
        if (Input.GetKeyDown(KeyCode.H))
        {
            wayHidden = !wayHidden;
        }
    }
    public void PlaneDestroyed()
    {
        numPlanes--;
        planeDestroyed++;
    }

    public void NewEgg()
    {
        numberEggs++;
    }

    public void BadEgg()
    {
        numberEggs--;
    }

    public void WaypointDestroyed(int waypointIndex)
    {
        // Respawn the waypoint at a random position within ±15 units of the original position
        Vector2 originalPosition = origPosition[waypointIndex];
        float offsetX = Random.Range(-15, 15);
        float offsetY = Random.Range(-15, 15);
        Vector2 respawnPosition = originalPosition + new Vector2(offsetX, offsetY);

        GameObject newWaypoint = Instantiate(waypoints[waypointIndex]);
        newWaypoint.transform.position = respawnPosition;
        curPosition[waypointIndex] = respawnPosition;
    }
}
