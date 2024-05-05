using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class camScript : MonoBehaviour
{

    private GameEngine engine;
    public Text statusLabel;
    private Camera waypointCam;

    // Start is called before the first frame update
    void Start()
    {
        engine = FindObjectOfType<GameEngine>();
        waypointCam = GetComponent<Camera>(); 
        waypointCam.enabled = false;
        statusLabel.text = "Waypoint Cam: Off";
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void activateCam(int index, float duration)
    {
        Debug.Log("Activating camera...");
        waypointCam.enabled = true;
        //waypointCam.transform.position = engine.curPosition[index];
        Vector2 waypointPosition = engine.curPosition[index];
        waypointCam.transform.position = new Vector3(waypointPosition.x, waypointPosition.y, waypointCam.transform.position.z);
        statusLabel.text = "Waypoint Cam: On";
        StartCoroutine(ResetCam(duration));
    }

    IEnumerator ResetCam(float duration)
    {
        yield return new WaitForSeconds(duration);
        waypointCam.enabled = false;
        statusLabel.text = "Waypoint Cam: Off";
    }
}
