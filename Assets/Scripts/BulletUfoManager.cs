using UnityEngine;

public class BulletUfoManager : MonoBehaviour
{
    public static Vector3 direction;

    public float speed;

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;

        Vector2 screenposition = Camera.main.WorldToScreenPoint(transform.position);

        if (screenposition.x < 0 || screenposition.x > Screen.width || screenposition.y < 0 || screenposition.y > Screen.height)
        {
            Destroy(gameObject);
        }
    }
}

