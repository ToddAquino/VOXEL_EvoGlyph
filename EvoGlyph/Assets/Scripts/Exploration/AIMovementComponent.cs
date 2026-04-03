using UnityEngine;

public class AIMovementComponent : MonoBehaviour
{
    [SerializeField] Transform[] waypoints;
    float waitTime;
    [SerializeField] float speed = 5;
    public int currentWaypointIndex = 0;
    public bool canMove = false;

    [SerializeField] SpriteRenderer spriteRenderer;
    public void Initialize()
    {
        waitTime = Random.Range(0.5f, 2f);
        canMove = true;
    }

    private void Update()
    {
        if (!canMove || waypoints.Length == 0) return;

        Transform waypoint = waypoints[currentWaypointIndex];

        if (Vector2.Distance(this.transform.position, waypoint.position) < 0.5f)
        {
            if (waitTime <= 0)
            {
                currentWaypointIndex++;

                if (currentWaypointIndex >= waypoints.Length)
                    currentWaypointIndex = 0;

                waitTime = Random.Range(0.5f, 2f);
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }

        this.transform.position = Vector2.MoveTowards(transform.position, waypoint.position, speed * Time.deltaTime);
        
        //Sprite Visuals
        Vector2 direction = waypoints[currentWaypointIndex].position - transform.position;
        if (direction.x > 0)
        {
            spriteRenderer.flipX = true;  // moving right
        }
        else if (direction.x < 0)
        {
            spriteRenderer.flipX = false; // moving left
        }

      
    }
}
