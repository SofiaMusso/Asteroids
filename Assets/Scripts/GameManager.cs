using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] bigAsteroids;
    public GameObject[] mediumAsteroids;
    public GameObject[] smallAsteroids;
    public GameObject player;

    public static int playerScore;
    public static int playerLives = 3;

    public Dictionary<AsteroidsManager.Type, GameObject[]> asteroids;

    public static GameManager Instance { get; private set; }
    private PlayerController playerController;

    public int numInitialAsteroids = 3;
    public float spawnRadius;

    public int numCurrentBigAsteroids;
    public int numCurrentAsteroids;

    public AudioSource explosionSfx;

    public GameObject ufo;
    private GameObject currentUfo;

    public float spawnDelayMin = 5f;
    public float spawnDelayMax = 10f;

    public float screenWidth = 8f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        //DontDestroyOnLoad(gameObject);

        asteroids = new Dictionary<AsteroidsManager.Type, GameObject[]>
        {
            { AsteroidsManager.Type.Big, bigAsteroids },
            { AsteroidsManager.Type.Medium, mediumAsteroids },
            { AsteroidsManager.Type.Small, smallAsteroids }
        };

        numCurrentBigAsteroids = 0;
        numCurrentAsteroids = 0;
        SpawnInitialAsteroids();
        Debug.Log("Number of Asteroids: " + numCurrentBigAsteroids);
    }

    void Start()
    {
        StartCoroutine(SpawnUfo());
    }

    IEnumerator SpawnUfo()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(spawnDelayMin, spawnDelayMax));

            int direction = Random.Range(-1, 1);
            float spawnHeight = Random.Range(-3f, 3f);

            Vector3 spawnPos;
            Vector3 targetPos;

            if (direction >= 0)
            {
                // sinistra -> destra
                spawnPos = new Vector3(-screenWidth, spawnHeight, 0);
                targetPos = new Vector3(screenWidth, spawnHeight, 0);
            }
            else
            {
                // destra -> sinistra
                spawnPos = new Vector3(screenWidth, spawnHeight, 0);
                targetPos = new Vector3(-screenWidth, spawnHeight, 0);
            }

            currentUfo = Instantiate(ufo, spawnPos, Quaternion.identity);

            currentUfo.GetComponent<UfoManager>().targetPosition = targetPos;
        }
    }

    private void Update()
    {
        RespwanAsteroids();
    }

    private void SpawnInitialAsteroids()
    {
        for (int i = 0; i < numInitialAsteroids; i++) 
        {
            float randomAngle = Random.Range(0f, 360f);
  
            Vector3 spawnPos = new Vector3(Mathf.Cos(randomAngle) * spawnRadius, Mathf.Sin(randomAngle) * spawnRadius, 0);
            SpawnAsteroids(spawnPos, AsteroidsManager.Type.Big);
        }
    }

    private void RespwanAsteroids()
    {
        if (numCurrentBigAsteroids < numInitialAsteroids)
        {
            float randomAngle = Random.Range(0f, 360f);

            Vector3 spawnPos = new Vector3(Mathf.Cos(randomAngle) * (spawnRadius + 1), Mathf.Sin(randomAngle) * (spawnRadius + 1 ), 0);
            SpawnAsteroids(spawnPos, AsteroidsManager.Type.Big);

        }
    }

    public void SpawnAsteroids(Vector2 position, AsteroidsManager.Type type)
    {
        GameObject asteroidPrefab = asteroids[type][Random.Range(0, asteroids[type].Length)];
        Instantiate(asteroidPrefab, position, Quaternion.identity);

        if (type == AsteroidsManager.Type.Big)
        {
            numCurrentBigAsteroids++;
        }

        numCurrentAsteroids++;

        Debug.Log("Number of Asteroids: " + numCurrentBigAsteroids);
    }

    public void Restart()
    {
        playerLives = 3;
        playerScore = 0;

        DestroyAllAsteroids();
        DestroyAllBullets();
        Destroy(currentUfo);

        playerController = player.GetComponent<PlayerController>();
        playerController.StopNoHitTimer();

        //Resetta l'alpha del player
        SpriteRenderer sr = player.GetComponent<SpriteRenderer>();
        Color c = sr.color;
        c.a = 1f;
        sr.color = c;

        numCurrentBigAsteroids = 0;
        numCurrentAsteroids = 0;

        SpawnInitialAsteroids();

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        rb.position = Vector3.zero;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.rotation = 0f;

        Time.timeScale = 1;
    }

    public void DestroyAllAsteroids()
    {
        AsteroidsManager[] asteroids = FindObjectsByType<AsteroidsManager>(FindObjectsSortMode.None); 

        foreach (AsteroidsManager asteroid in asteroids)
        {
            Destroy(asteroid.gameObject);
        }
    }

    public void DestroyAllBullets()
    {
        BulletManager[] bullets = FindObjectsByType<BulletManager>(FindObjectsSortMode.None);

        foreach (BulletManager bullet in bullets)
        {
            Destroy(bullet.gameObject);
        }

        BulletUfoManager[] ufoBullets = FindObjectsByType<BulletUfoManager>(FindObjectsSortMode.None);

        foreach (BulletUfoManager ufoBullet in ufoBullets)
        {
            Destroy(ufoBullet.gameObject);
        }
    }
}
