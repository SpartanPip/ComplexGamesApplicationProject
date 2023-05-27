using UnityEngine;

public class EnemyCannonBall : MonoBehaviour
{
    public GameObject landingInWaterFX;
    public GameObject hitShipFX;
    public int damage;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        transform.LookAt(transform.position + rb.velocity);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<CannonPlatform>().GetHit(damage);
            Instantiate(hitShipFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "Water")
        {
            Instantiate(landingInWaterFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        else
        {
            Instantiate(hitShipFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
