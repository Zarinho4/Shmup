using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float fireRate = 0.5f;

    public int health = 3;
    public int score = 0;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI scoreText;

    public AudioSource shootingSound;

    private float nextFireTime = 0f;
    private bool isLevel2Loaded = false;
    private bool isVictoryLoaded = false;

    void Start()
    {
        UpdateUI();
        shootingSound = GetComponent<AudioSource>();

        // Check if there is a GameManager instance
        if (GameManager.instance != null)
        {
            // Load player data from GameManager
            health = GameManager.instance.playerHealth;
            score = GameManager.instance.playerScore;
            UpdateUI();
        }
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0.0f);
        transform.position += movement * moveSpeed * Time.deltaTime;

        // Clamping the player position within the camera bounds
        float clampedX = Mathf.Clamp(transform.position.x, Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x, Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x);
        float clampedY = Mathf.Clamp(transform.position.y, Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y, Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y);
        transform.position = new Vector3(clampedX, clampedY, 0.0f);

        if (Input.GetButton("Fire1") && Time.time > nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Shoot();

            if (shootingSound != null)
            {
                shootingSound.Play();
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ActivateAbility1();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ActivateAbility2();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ActivateAbility3();
        }

        CheckOutOfBounds();

        // Check for reaching score 1000 and load Level 2 with a delay
        if (score >= 1000 && !isLevel2Loaded)
        {
            // Save player data to GameManager before loading Level 2
            GameManager.instance.playerHealth = health;
            GameManager.instance.playerScore = score;

            StartCoroutine(LoadLevelWithDelay("Level 2", 3.0f));
            isLevel2Loaded = true;  // Set the flag to true after loading Level 2
        }

        // Check for reaching score 3000 and load Victory scene
        if (score >= 3000 && !isVictoryLoaded)
        {
            // Save player data to GameManager before loading the victory scene
            GameManager.instance.playerHealth = health;
            GameManager.instance.playerScore = score;

            SceneManager.LoadScene("Victory scene");
            isVictoryLoaded = true;  // Set the flag to true after loading the victory scene
        }
    }

    void Shoot()
    {
        Vector3 offset = new Vector3(0.0f, 1.0f, 0.0f);
        Vector3 spawnPosition = bulletSpawnPoint.position + offset;

        GameObject bullet = Instantiate(bulletPrefab, spawnPosition, bulletSpawnPoint.rotation);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.velocity = new Vector2(moveSpeed, 0.0f);
        Destroy(bullet, 3.0f);
    }

    void ActivateAbility1()
    {
        Debug.Log("Ability 1 activated");
    }

    void ActivateAbility2()
    {
        Debug.Log("Ability 2 activated");
    }

    void ActivateAbility3()
    {
        Debug.Log("Ability 3 activated");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyBullet"))
        {
            TakeDamage(5);
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(1);
            Destroy(collision.gameObject);
        }
    }

    private void CheckOutOfBounds()
    {
        // Implement your out-of-bounds handling logic here
        // For example, you can respawn the player or perform other actions
        // This is a placeholder, replace it with your own code
        if (transform.position.y < Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y)
        {
            Respawn();
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            GameOver();
        }
        UpdateUI();
    }

    void GameOver()
    {
        // Save player data to GameManager before loading the GameOverScene
        GameManager.instance.playerHealth = health;
        GameManager.instance.playerScore = score;

        SceneManager.LoadScene("GameOverScene");
        // Additional game over logic can be added here
    }

    public void AddScore(int points)
    {
        score += points;
        Debug.Log("Score increased by: " + points);
        UpdateUI();
    }

    void Respawn()
    {
        health = 3;
        // Additional respawn logic (e.g., invincibility period)
    }

    private void UpdateUI()
    {
        if (healthText) healthText.text = "Health: " + health;
        if (scoreText) scoreText.text = "Score: " + score;
    }

    IEnumerator LoadLevelWithDelay(string levelName, float delay)
    {
        yield return new WaitForSeconds(delay);

        // Check if the current scene is not Level 2 before loading
        if (SceneManager.GetActiveScene().name != "Level 2")
        {
            SceneManager.LoadScene(levelName);
        }
    }
}
