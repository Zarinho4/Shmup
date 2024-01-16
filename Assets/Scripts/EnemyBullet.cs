using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 20f;
    public float offScreenTime = 3f;
    public int damage = 5; // Adjust damage as needed

    private float offScreenTimer;

    void Start()
    {
        offScreenTimer = offScreenTime;
    }

    void Update()
    {
        var currentPos = GetComponent<Rigidbody2D>().position;
        currentPos += (Vector2.left * speed * Time.deltaTime);
        GetComponent<Rigidbody2D>().position = currentPos;

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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            DealDamageToPlayer();
            Destroy(gameObject);
        }
    }

    void DealDamageToPlayer()
    {
        // Assuming the player has a PlayerController script
        PlayerController playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        if (playerController != null)
        {
            playerController.TakeDamage(damage);
        }
    }
}
