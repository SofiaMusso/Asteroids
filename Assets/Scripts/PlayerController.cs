using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;

    private float forwardInput;
    public float forwardSpeed;
    private float turnInput;
    public float turnSpeed;

    public GameObject bulletPrefab;
    public AudioSource bulletSfx;

    public bool justHit;

    private Coroutine noHitCoroutine;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        forwardInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.S))
        {
            TeleportRandom();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(bulletPrefab, transform.position, transform.rotation);
            bulletSfx.Play();
        }
    }

    private void FixedUpdate()
    {
        if (forwardInput > 0)
        {
            rb.AddForce(transform.up * forwardSpeed * forwardInput);
        }

        if (turnInput != 0)
        {
            float rotatioAmount = turnInput * turnSpeed * Time.fixedDeltaTime;
            rb.MoveRotation(rb.rotation + rotatioAmount);
        }

    }
   
    private void TeleportRandom()
    {
        float distanceZ = Mathf.Abs(Camera.main.transform.position.z - transform.position.z);

        Vector2 screenBL = Camera.main.ScreenToWorldPoint(new Vector3 (0,0, distanceZ));
        Vector2 screenTR = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, distanceZ));

        float randomX = Random.Range(screenBL.x, screenTR.x);
        float randomY = Random.Range(screenBL.y, screenTR.y);

        transform.position = new Vector3(randomX, randomY, transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Asteroid"))
        {
            if (!justHit)
            {
                Debug.Log("Just Hit");
                GameManager.playerLives = GameManager.playerLives - 1;
                Debug.Log(GameManager.playerLives);
                transform.position = Vector3.zero;
                justHit = true;
                Debug.Log("Can't be Hit");
                noHitCoroutine = StartCoroutine(NoHitTimer());
            }
                
        }
        if (other.gameObject.CompareTag("BulletUfo"))
        {
            if (!justHit)
            {
                Debug.Log("Just Hit");
                GameManager.playerLives = GameManager.playerLives - 1;
                Debug.Log(GameManager.playerLives);
                transform.position = Vector3.zero;
                justHit = true;
                Debug.Log("Can't be Hit");
                noHitCoroutine = StartCoroutine(NoHitTimer());
            }

        }
        CheckEndGame();
    }

    IEnumerator NoHitTimer()
    {

            for (float i = 2f; i >= 0; i -= 0.25f)
            {
                gameObject.GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 25);
                //Debug.Log("- Alpha");
                yield return new WaitForSeconds(0.1f);
                gameObject.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, 25);
                //Debug.Log("+ Alpha");
                yield return new WaitForSeconds(0.1f);
            }

            justHit = false;
            yield return null;
    }
    private void CheckEndGame()
    {
        if (GameManager.playerLives <= 0)
        {
            Debug.Log("game has ENDED");
            Debug.Log(GameManager.playerScore);
        }
    }

    public void StopNoHitTimer()
    {
        if (noHitCoroutine != null)
        {
            StopCoroutine(noHitCoroutine);
            Debug.Log("Stopped No Hit Timer");
            justHit = false;
        }
    }
}
