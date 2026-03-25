using UnityEngine;

public class ScreenWrapper : MonoBehaviour
{
    private float leftConstrain;
    private float rightConstrain;
    private float bottomConstrain;
    private float topConstrain;
    void Start()
    {
        float distanceZ = Mathf.Abs(Camera.main.transform.position.z - transform.position.z);
        leftConstrain = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, distanceZ)).x;
        rightConstrain = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - 1, 0, distanceZ)).x;
        topConstrain = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height - 1, distanceZ)).y;
        bottomConstrain = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, distanceZ)).y;

    }

    void Update()
    {

        if ( transform.position.x < leftConstrain)
        {
            transform.position = new Vector3(rightConstrain, transform.position.y, transform.position.z);
        }

        else if (transform.position.x > rightConstrain)
        {
            transform.position = new Vector3(leftConstrain, transform.position.y, transform.position.z);
        }

        if (transform.position.y < bottomConstrain)
        {
            transform.position = new Vector3(transform.position.x, topConstrain, transform.position.z);
        }

        else if (transform.position.y > topConstrain)
        {
            transform.position = new Vector3(transform.position.x, bottomConstrain, transform.position.z);
        }
    }
}
