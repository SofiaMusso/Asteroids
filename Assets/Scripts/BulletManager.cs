using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public float speed;

    void Start()
    {
        GetComponent<Rigidbody2D>().AddForce(transform.up * speed);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 screenposition = Camera.main.WorldToScreenPoint(transform.position);

        if ( screenposition.x < 0 || screenposition.x > Screen.width || screenposition.y < 0 || screenposition.y > Screen.height)
        {
            Destroy(gameObject);
        }
    }
}
