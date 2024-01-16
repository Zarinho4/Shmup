using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float speed = 20f;
    public float offScreenTime = 3f;
    public int scoreValue = 100;
    public AudioClip destroySound; // Sound to play on enemy destruction
    public AudioClip collideSound; // Sound to play on collision
    private float offScreenTimer;
    private Rigidbody2D rb2d;
    private AudioSource audioSource;

    void Start()
    {
        offScreenTimer = offScreenTime;
        rb2d = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            EnemyController enemyController = other.gameObject.GetComponent<EnemyController>();
            PlayerController playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

            if (playerController != null)
            {
                playerController.AddScore(scoreValue);
                Debug.Log("Score increased by: " + scoreValue);

                // Play destroy sound
                if (audioSource && destroySound != null)
                {
                    audioSource.PlayOneShot(destroySound);
                }
            }

            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }

    void Update()
    {
        var currentPos = rb2d.position;
        currentPos += (Vector2.right * speed * Time.deltaTime);
        rb2d.position = currentPos;

        if (!RendererIsVisible())
        {
            offScreenTimer -= Time.deltaTime;
            if (offScreenTimer <= 0)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            offScreenTimer = offScreenTime;
        }
    }

    bool RendererIsVisible()
    {
        var renderer = GetComponent<Renderer>();
        if (renderer)
        {
            return renderer.isVisible;
        }
        return false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Bullet collided with something!");

        // Play collision sound
        if (audioSource && collideSound != null)
        {
            audioSource.PlayOneShot(collideSound);
        }

        // Add collision logic here (e.g., impact on obstacles)
        Destroy(gameObject);
    }
}
