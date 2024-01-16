using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public float shootInterval = 1.0f;
    public float projectileSpeed = 5.0f;
    public float moveSpeed = 2.0f;
    public float moveRange = 1.0f;
    public float enemyLifeTime = 10.0f;
    public float respawnTime = 3.0f;
    public AudioClip hitSound; // Sound to play when hit by a player bullet

    private float nextShootTime = 0.0f;
    private float startY;
    private float spawnTime;
    private AudioSource audioSource;

    void Start()
    {
        startY = transform.position.y;
        spawnTime = Time.time;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        Move();

        // Check if the enemy's x position is less than or equal to 13
        if (transform.position.x <= 10.0f)
        {
            // Start shooting bullets
            Shoot();
        }

        if (Time.time - spawnTime >= enemyLifeTime)
        {
            Respawn();
        }
    }

    void Move()
    {
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);

        float newY = startY + Mathf.Sin(Time.time) * moveRange;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    void Shoot()
    {
        if (Time.time > nextShootTime)
        {
            GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
            Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
            projectileRb.velocity = new Vector2(-projectileSpeed, 0.0f);
            Destroy(projectile, 3.0f);

            // Adjust the rotation of the enemy to face left
            if (projectileSpawnPoint.localScale.x > 0)
            {
                Vector3 newScale = transform.localScale;
                newScale.x *= -1;
                transform.localScale = newScale;
            }

            nextShootTime = Time.time + shootInterval;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            Destroy(collision.gameObject);

            // Play hit sound
            if (audioSource && hitSound != null)
            {
                audioSource.PlayOneShot(hitSound);
                Debug.Log("Hit sound played");
            }

            Destroy(gameObject);
        }
    }

    void Respawn()
    {
        spawnTime = Time.time;
        transform.position = new Vector3(transform.position.x, startY, transform.position.z);
    }
}
