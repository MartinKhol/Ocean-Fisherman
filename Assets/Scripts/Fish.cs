using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public float movementForce = 5f;
    public float chasedMultiplier = 2f;
    public float movementRange = 3f;
    public float maxSpeed = 5f;
    public bool dangerous = false;
    public bool aggresive = false;
    public int damage = 1;
	public float minDepth = 0;
	public float maxDepth = 0;
	
	[Space]
	public LayerMask collisionMask;

    private float chaseDistance = 2f;
   // private float chaseTime = 1f;
    private bool isChased = false;
    private new Rigidbody2D rigidbody2D;
    [SerializeField]
    private Vector3 targetPosition;
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private float stoppingDistance = 0.5f;
    private Animator animator;

    public static int fishCount;

    private void Awake()
    {
        fishCount++;
    }

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        targetPosition = GetRandomPosition();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, targetPosition) < stoppingDistance)
        {
            if (isChased) StopChase();
            targetPosition = GetRandomPosition();
        }

        Vector3 direction = targetPosition - transform.position;

        direction.Normalize();

        if (direction.x < 0)
            FlipLeft();
        else
            FlipRight();

        rigidbody2D.AddForce(direction * movementForce * (isChased ? chasedMultiplier : 1));
    }

    Vector3 GetRandomPosition()
    {
        Vector3 position = Vector3.zero;

        position.y = transform.position.y + Random.Range(-movementRange, movementRange);
        position.x = transform.position.x + Random.Range(-movementRange, movementRange);
		
		if (maxDepth != 0)
		{
			if (position.y < maxDepth)
			{
				position.y = maxDepth + Random.Range(-1, 2);
			}
			
		}
		if (minDepth != 0)
		{
				if (position.y > minDepth)
				position.y = minDepth  + Random.Range(-1, 2);;
		}
		
		RaycastHit2D hit = Physics2D.Raycast (transform.position, (position - transform.position), movementRange + 0.1f, collisionMask);
		if (hit.collider != null)
		{
			Vector3 pos3D = transform.position;
			Vector2 pos2D = pos3D;
			position = hit.point + (pos2D - hit.point) * 0.2f;
		}

        //Debug.Log("unclamped target position " + position);

        position.y = Mathf.Clamp(position.y, Level.Bottom, Level.Surface);
        position.x = Mathf.Clamp(position.x, Level.LeftBorder, Level.RightBorder);

      //  Vector3 direction = transform.position
       // Physics2D.Raycast(transform.position, )

        //Debug.Log("target position " + position);

        return position;
    }

    void FlipLeft()
    {
        Vector3 scale = transform.localScale;
        scale.x = -1f;
        transform.localScale = scale;

    }
    void FlipRight()
    {
        Vector3 scale = transform.localScale;
        scale.x = 1f;
        transform.localScale = scale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (aggresive)
            {
                var direction = collision.transform.position - transform.position;

                targetPosition = targetPosition + direction * chaseDistance;

                targetPosition.y = Mathf.Clamp(targetPosition.y, Level.Bottom, Level.Surface);
                targetPosition.x = Mathf.Clamp(targetPosition.x, Level.LeftBorder, Level.RightBorder);

                isChased = true;

                if (animator != null) animator.SetBool("Attack", true);
            }
            else if (!dangerous)
                RunAway(collision);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!dangerous) return;

        if (collision.collider.CompareTag("Player"))
        {
            GameObject gameObject = collision.gameObject;

            gameObject.GetComponent<PlayerState>().TakeDamage(damage);

            var playerRb = gameObject.GetComponent<Rigidbody2D>();

            playerRb.AddForce(-playerRb.velocity.normalized * 5f, ForceMode2D.Impulse);

        }
        else if (aggresive && collision.collider.CompareTag("Fish"))
        {
            collision.collider.GetComponent<CollectibleFish>().PickUp();
        }
    }

    private void RunAway(Collider2D collision)
    {
        Vector3 direction = transform.position - collision.transform.position;

        direction.Normalize();

        targetPosition = targetPosition + direction * chaseDistance;

        targetPosition.y = Mathf.Clamp(targetPosition.y, Level.Bottom, Level.Surface);
        targetPosition.x = Mathf.Clamp(targetPosition.x, Level.LeftBorder, Level.RightBorder);

        isChased = true;
    }

    private void StopChase()
    {
        isChased = false;
        if (aggresive && animator != null) animator.SetBool("Attack", false);
    }

    private void OnDestroy()
    {
        fishCount--;
    }
}
