using UnityEngine;

public class CannonBall : MonoBehaviour
{
    public CanvasManager canvasManager;
    public GameObject landingInWaterFX;
    public GameObject hitShipFX;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ship")
        {
            if (transform.position.magnitude > PlayerPrefs.GetInt("maxShotDistance") || !PlayerPrefs.HasKey("maxShotDistance"))
            {
                PlayerPrefs.SetInt("maxShotDistance", (int)Mathf.Round(transform.position.magnitude));
                canvasManager.UpdateDistance();
            }
            canvasManager.UpdaeScore(collision.gameObject.GetComponent<Ship>().score);
            Instantiate(hitShipFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        else
        {
            Instantiate(landingInWaterFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
