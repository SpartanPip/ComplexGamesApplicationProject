using System.Collections;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject playerCannon;
    public GameObject cannonBall;
    public GameObject cannonBallFX;
    public HealthBar healthBar;
    public float firingPower;
    public float speed;
    public int health;
    private int totalHealth;
    public int rateOfFire;
    public int score;
    Vector3 targetPosition;
    Vector3 mapCenter = new Vector3(0f, 0f, 0f); 
    Vector3 offset;
    Vector3 perpendicular;
    Rigidbody rb;
    private bool goingDown = false;
    private float cannonBallOffset = 2f;
    public AvoidObstacles avoidObstacles;
    public float turnSpeed;
    bool canReturnToNormalOrbit = true;
    bool startedOrbitTimer = false;
    //Quaternion targetRotation;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        totalHealth = health;
        InvokeRepeating("FireCannon", rateOfFire, rateOfFire);
        InvokeRepeating("UpdateRotationAndVelocity", 0.1f, 0.1f);
    }

    IEnumerator StartOrbitTimer()
    {
        yield return new WaitForSeconds(10f);
        canReturnToNormalOrbit = true;
        startedOrbitTimer = false;
    }

    void UpdateRotationAndVelocity()
    {
        if (goingDown)
            return;
        Vector3 avoidenceDir = avoidObstacles.CheckForObstacles();
        if (avoidenceDir != Vector3.zero)
        {
            //targetRotation = Quaternion.LookRotation(avoidenceDir);
            //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.02f);
            Vector3 eulers = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(new Vector3(eulers.x, eulers.y + 2f, eulers.z));
            rb.velocity = transform.forward;
        }
        else if (!canReturnToNormalOrbit)
        {
            if (!startedOrbitTimer)
            {
                startedOrbitTimer = true;
                StartCoroutine("StartOrbitTimer");
            }
            //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.02f);
            Vector3 eulers = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(new Vector3(eulers.x, eulers.y + 2f, eulers.z));
            rb.velocity = transform.forward;
        }
        else
        {
            targetPosition = transform.position;
            offset = targetPosition - mapCenter;
            perpendicular = Vector3.Cross(offset, Vector3.up).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(perpendicular);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.004f);
            rb.velocity = transform.forward * speed;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Cannon Ball")
        {
            health -= collision.gameObject.GetComponent<CannonBall>().damage;
            float newHealth = Mathf.Clamp((float)health / (float)totalHealth, 0f, 1f);
            healthBar.UpdateBar(newHealth);
            if (health <= 0)
            {
                goingDown = true;
                StartCoroutine(BeginDeath());
            }
        }
    }

    IEnumerator BeginDeath()
    {
        if (gameManager.totalShipsMade == gameManager.levelNumber + 1 && gameManager.existingShips.Count == 1)
            FindAnyObjectByType<CanvasManager>().WinGame();
        gameObject.layer = LayerMask.NameToLayer("Dying Ship");
        GetComponent<Rigidbody>().velocity += new Vector3(0f, -1f);
        yield return new WaitForSeconds(5f);
        gameManager.existingShips.Remove(gameObject);
        Destroy(gameObject);
    }

    void FireCannon()
    {
        // Calculate the angle (theta) at which the cannon ball need to be shot to hit the player given the cannon ball's speed and gravity
        float d = (new Vector3(transform.position.x, transform.position.y + cannonBallOffset, transform.position.z) - gameManager.playerCannon.transform.position).magnitude;
        float theta = Mathf.Atan((Mathf.Pow(firingPower, 2f) - Mathf.Sqrt(Mathf.Pow(firingPower, 4f) - 9.81f * (9.81f * Mathf.Pow(d, 2f) + 2f * (gameManager.playerCannon.transform.position.y - cannonBallOffset) * Mathf.Pow(firingPower, 2f)))) / (9.81f * d));
        float thetaDegrees = theta * (180f / Mathf.PI);
        Vector3 inaccuracy = new Vector3(Random.Range(1f, -1f), Random.Range(1f, -1f), Random.Range(1f, -1f));
        if (cannonBallFX != null)
        {
            GameObject FX = Instantiate(cannonBallFX, transform.position + new Vector3(0f, 2f, 0f), Quaternion.identity);
            FX.transform.LookAt(new Vector3(0f, 0f, 0f));
            Vector3 eulersFX = FX.transform.rotation.eulerAngles;
            FX.transform.rotation = Quaternion.Euler(new Vector3(-thetaDegrees + inaccuracy.x, eulersFX.y + inaccuracy.y, eulersFX.z + inaccuracy.z));
        }
        GameObject newCannonBallGameObject = Instantiate(cannonBall, transform.position + new Vector3(0f, 2f, 0f), Quaternion.identity);
        newCannonBallGameObject.transform.LookAt(new Vector3(0f, 0f, 0f));
        Vector3 eulers = newCannonBallGameObject.transform.rotation.eulerAngles;
        newCannonBallGameObject.transform.rotation = Quaternion.Euler(new Vector3(-thetaDegrees + inaccuracy.x, eulers.y + inaccuracy.y, eulers.z + inaccuracy.z));
        newCannonBallGameObject.GetComponent<Rigidbody>().velocity = newCannonBallGameObject.transform.forward * firingPower;
    }
}
