using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCannonTester : MonoBehaviour
{
    float firingPower = 65f;
    public GameObject cannonBall;
    public Transform wheel1;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("FireCannon", 3f, 3f);
    }

    void FireCannon()
    {
        float d = (transform.position - wheel1.position).magnitude;
        float theta = Mathf.Atan((Mathf.Pow(firingPower, 2f) - Mathf.Sqrt(Mathf.Pow(firingPower, 4f) - 9.81f * (9.81f * Mathf.Pow(d, 2f) + 2f * (wheel1.position.y - 2f) * Mathf.Pow(firingPower, 2f)))) / (9.81f * d));
        float thetaDegrees = theta * (180f / Mathf.PI);
        GameObject newCannonBallGameObject = Instantiate(cannonBall, transform.position, Quaternion.identity);
        newCannonBallGameObject.transform.LookAt(new Vector3(0f, 0f, 0f));
        Vector3 eulers = newCannonBallGameObject.transform.rotation.eulerAngles;
        newCannonBallGameObject.transform.rotation = Quaternion.Euler(new Vector3(-thetaDegrees, eulers.y, eulers.z));
        newCannonBallGameObject.GetComponent<Rigidbody>().velocity = newCannonBallGameObject.transform.forward * firingPower;
    }
}
