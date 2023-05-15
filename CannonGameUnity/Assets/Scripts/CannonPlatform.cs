using System.Collections;
using UnityEngine;

public class CannonPlatform : MonoBehaviour
{
    public GameObject[] cannons;
    public CanvasManager canvasManager;
    public GameObject activeCannon;
    public GameObject cannonBall;
    public GameObject cannonBallFX;
    public GameObject wheel1;
    public GameObject wheel2;
    public GameObject wheel3;
    private float wheel1RotationSpeed = 10f;
    private float wheel2RotationSpeed = 20f;
    private float wheel3RotationSpeed = 40f;
    public int turningGear = 1;
    AudioSource wheel1Audio;
    public AudioSource cannonAudioSource;

    void Start()
    {
        activeCannon = cannons[0];
        wheel1Audio = wheel1.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            TurnLeft();
            if (!wheel1Audio.isPlaying)
                wheel1Audio.Play();
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            TurnRight();
            if (!wheel1Audio.isPlaying)
                wheel1Audio.Play();
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            wheel1Audio.Pause();
        }
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            TiltUp();
        }
        else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            TiltDown();
        }
        else if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
        {
            cannonAudioSource.Pause();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FireCannonBall();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ToggleCannons();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            turningGear = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            turningGear = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            turningGear = 3;
        }
    }

    private void TurnLeft()
    {
        wheel1.transform.Rotate(new Vector3(0f, -wheel1RotationSpeed * turningGear * Time.deltaTime), Space.World);
        if (turningGear > 1)
            wheel2.transform.Rotate(new Vector3(0f, wheel2RotationSpeed * turningGear * Time.deltaTime), Space.World);
        if (turningGear > 2)
            wheel3.transform.Rotate(new Vector3(0f, -wheel3RotationSpeed * turningGear * Time.deltaTime), Space.World);
    }

    private void TurnRight()
    {
        wheel1.transform.Rotate(new Vector3(0f, wheel1RotationSpeed * turningGear * Time.deltaTime), Space.World);
        if (turningGear > 1)
            wheel2.transform.Rotate(new Vector3(0f, -wheel2RotationSpeed * turningGear * Time.deltaTime), Space.World);
        if (turningGear > 2)
            wheel3.transform.Rotate(new Vector3(0f, wheel3RotationSpeed * turningGear * Time.deltaTime), Space.World);
    }

    private void TiltUp()
    {
        activeCannon.GetComponent<Cannon>().TiltUp(cannonAudioSource);
    }

    private void TiltDown()
    {
        activeCannon.GetComponent<Cannon>().TiltDown(cannonAudioSource);
    }

    private void FireCannonBall()
    {
        canvasManager.UpdaeShots();
        Transform cannonBarrel = activeCannon.GetComponent<Cannon>().cannonBarrel;
        Vector3 firingAngle = cannonBarrel.forward;
        Instantiate(cannonBallFX, cannonBarrel.position + firingAngle, cannonBarrel.rotation);
        GameObject newCannonBall = Instantiate(cannonBall, cannonBarrel.position + firingAngle * 3f, Quaternion.Euler(firingAngle));
        newCannonBall.GetComponent<CannonBall>().canvasManager = canvasManager;
        if(activeCannon == cannons[0])
            newCannonBall.GetComponent<Rigidbody>().velocity = firingAngle * 45f;
        else
            newCannonBall.GetComponent<Rigidbody>().velocity = firingAngle * 65f;
    }

    private void ToggleCannons()
    {
        foreach(GameObject cannon in cannons)
        {
            if (cannon.activeSelf == true){
                cannon.SetActive(false);
            }
            else
            {
                cannon.SetActive(true);
                activeCannon = cannon;
            }
        }
    }

    public void ResetCannons()
    {
        foreach (GameObject cannon in cannons)
            cannon.GetComponent<Cannon>().ResetTilt();
        turningGear = 1;
        if (activeCannon == cannons[1])
            ToggleCannons();
    }
}
