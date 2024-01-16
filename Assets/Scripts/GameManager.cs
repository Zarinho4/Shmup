using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int playerHealth;
    public int playerScore;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
