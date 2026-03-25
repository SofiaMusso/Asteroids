using System.Collections;
using UnityEngine;

public class AsteroidsManager : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] float speed;
    [SerializeField] float maxAngularSpeed;

    public enum Type { Big, Medium, Small };
    public Type type;

    public GameObject explosion;

    private int bigPoints = 100;
    private int mediumPoints = 50;
    private int smallPoints = 25;

    void Start()
    {
       rb = GetComponent<Rigidbody2D>();
       rb.AddForce(Random.insideUnitCircle.normalized * speed);
       rb.angularVelocity = Random.Range(-maxAngularSpeed, maxAngularSpeed);
    }

    private void Update()
    {
        if (GameManager.playerScore >= 1000 && GameManager.playerScore < 2000)
        {
            speed = speed + 1;            
        }
        else if (GameManager.playerScore >= 2000 && GameManager.playerScore < 3000)
        {
            speed = speed + 5;
        }
        else if (GameManager.playerScore >= 3000 && GameManager.playerScore < 4000)
        {
            speed = speed + 10;
        }
        else if (GameManager.playerScore >= 4000 && GameManager.playerScore < 5000)
        {
            speed = speed + 20;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("tag is: " + collision.tag);

        if (collision.CompareTag("Bullet"))
        {
            Debug.Log("Asteroide colpito");

            if (type == Type.Big)
            {
                GameManager.Instance.SpawnAsteroids(transform.position, Type.Medium);
                GameManager.Instance.SpawnAsteroids((transform.position + new Vector3(1, 0.4f, transform.position.z)), Type.Medium);
                GameManager.playerScore = GameManager.playerScore + bigPoints;
                GameManager.Instance.numCurrentBigAsteroids--;
            }
            else if (type == Type.Medium)
            {
                GameManager.Instance.SpawnAsteroids(transform.position, Type.Small);
                GameManager.Instance.SpawnAsteroids((transform.position + new Vector3(1, 0.3f, transform.position.z)), Type.Small);
                GameManager.playerScore = GameManager.playerScore + mediumPoints;
            }
            else if (type == Type.Small)
            {
                GameManager.playerScore = GameManager.playerScore + smallPoints;
            }

            Destroy(collision.gameObject);
            Destroy(gameObject);

            GameManager.Instance.explosionSfx.Play();

            GameManager.Instance.numCurrentAsteroids--;

            Debug.Log(GameManager.playerScore);
            Debug.Log("Number of Asteroids: " + GameManager.Instance.numCurrentBigAsteroids);

            Instantiate(explosion, transform.position, Quaternion.identity);
        }
    }
}
