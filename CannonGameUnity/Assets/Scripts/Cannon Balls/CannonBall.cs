using UnityEngine;

public class CannonBall : MonoBehaviour
{
    public CanvasManager canvasManager;
    public CannonPlatform cannonPlatform;
    public GameObject mainCamera;
    public GameObject landingInWaterFX;
    public GameObject hitShipFX;
    public Transform wheel1Transform;
    public int damage;

    void OnCollisionEnter(Collision collision)
    {
        if (mainCamera.transform.parent == transform) {
            mainCamera.transform.parent = wheel1Transform;
            mainCamera.transform.localPosition = cannonPlatform.originalCamPos;
            mainCamera.transform.localRotation = cannonPlatform.originalCamRot;
            cannonPlatform.firingSpecialCannonBall = false;
        }
        if (collision.gameObject.tag == "Ship")
        {
            if (transform.position.magnitude > PlayerPrefs.GetInt("maxShotDistance") || !PlayerPrefs.HasKey("maxShotDistance"))
            {
                PlayerPrefs.SetInt("maxShotDistance", (int)Mathf.Round(transform.position.magnitude));
                canvasManager.UpdateDistance();
            }
            canvasManager.UpdateScore();
            Instantiate(hitShipFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        else if(collision.gameObject.tag == "Water")
        {
            Instantiate(landingInWaterFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        else {
            Instantiate(hitShipFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
