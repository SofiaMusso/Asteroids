using System.Collections;
using UnityEngine;

public class UfoManager : MonoBehaviour
{
    private GameObject player;

    private int speed = 2;
    private float timeBetweenBullets = 1;

    public GameObject bulletPrefab;

    private Vector3 playerDirection;
    private Quaternion transformRotation;

    public Vector3 targetPosition;

    public GameObject explosion;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        StartCoroutine(Shoot());

    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            Destroy(gameObject);
            GameManager.Instance.explosionSfx.Play();
        }
    }

    IEnumerator Shoot()
    {
        while (true)
        {
            ShootBullet();
            yield return new WaitForSeconds(timeBetweenBullets);
        }
    }

    void ShootBullet()
    {
        BulletUfoManager.direction = (player.transform.position - transform.position).normalized;

        float angle = Mathf.Atan2(BulletUfoManager.direction.y, BulletUfoManager.direction.x) * Mathf.Rad2Deg;

        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        Instantiate(bulletPrefab, transform.position, rotation);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("tag is: " + collision.tag);

        if (collision.CompareTag("Bullet"))
        {
            Debug.Log("Ufo colpito");

            Destroy(collision.gameObject);
            Destroy(gameObject);

            GameManager.playerScore = GameManager.playerScore + 250;

            GameManager.Instance.explosionSfx.Play();
            Instantiate(explosion, transform.position, Quaternion.identity);
        }
    }
}
