using System.Collections;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public GameManager gameManager;
    public float speed;
    public int score;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
    }

    private void Update()
    {
        if (transform.position.magnitude > 300f)
            StartCoroutine(BeginDeath());
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Cannon Ball")
        {
            StartCoroutine(BeginDeath());
        }
    }

    IEnumerator BeginDeath()
    {
        Destroy(GetComponent<Collider>());
        GetComponent<Rigidbody>().velocity = new Vector3(0f, -1f);
        yield return new WaitForSeconds(5f);
        gameManager.existingShips.Remove(gameObject);
        Destroy(gameObject);
    }
}
