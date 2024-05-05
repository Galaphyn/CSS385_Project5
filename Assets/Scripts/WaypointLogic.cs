using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WaypointLogic : MonoBehaviour
{
    public int index;
    private int health = 100;
    private GameEngine engine;
    private SpriteRenderer sprite;
    private float originalValue;
    private int hit = 0;
    private camScript camScript;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        engine = Object.FindAnyObjectByType<GameEngine>();
        camScript = Object.FindAnyObjectByType<camScript>();
        originalValue = sprite.color.a;
    }

    // Update is called once per frame
    void Update()
    {
        //If h is pressed, set alpha to 0, else return to original value
        if (engine.wayHidden)
        {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0);
        }
        else
        {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, originalValue);
        }
    }
    public void hitWaypoint()
    {
        health -= 25;
        
        //Multiply and store alpha value by .75 to reduce by 25%
        originalValue *= 0.75f;
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, originalValue);

        if (health <= 0)
        {
            Debug.Log("Destroyed Waypoint!");
            Destroy(gameObject);
            engine.WaypointDestroyed(index);
        }
        else
        {
            hit++;
            StartCoroutine(Shake(hit, hit));
            camScript.activateCam(index, hit);
        }
    }

    //Shaking code from 100 list with minor edits
    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector2 originalPos = transform.localPosition; //Changed from vector3 to 2 just because its 2d and the 3rd dimension probably doesnt matter

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector2(x, y) + originalPos; //+originalPos as, otherwise, newVector2 sets it in the center of the scene

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;
    }


}
