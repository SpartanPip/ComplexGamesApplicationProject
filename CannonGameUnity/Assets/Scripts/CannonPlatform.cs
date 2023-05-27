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
    public GameObject mainCamera;
    private float wheel1RotationSpeed = 10f;
    private float wheel2RotationSpeed = 20f;
    private float wheel3RotationSpeed = 40f;
    public int turningGear = 1;
    AudioSource wheel1Audio;
    public AudioSource cannonAudioSource;
    public bool firingSpecialCannonBall = false;
    public Vector3 originalCamPos;
    public Quaternion originalCamRot;
    public int health;
    private int totalHealth;
    public HealthBar healthBar;
    Recoil recoil;

    void Start()
    {
        recoil = mainCamera.GetComponent<Recoil>();
        totalHealth = health;
        originalCamPos = mainCamera.transform.localPosition;
        originalCamRot = mainCamera.transform.localRotation;
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
        if (Input.GetKeyDown(KeyCode.Space) && !recoil.isRecoiling && !firingSpecialCannonBall)
        {
            FireCannonBall(false);
        }
        if (Input.GetKeyDown(KeyCode.R) && !recoil.isRecoiling && !firingSpecialCannonBall)
        {
            FireCannonBall(true);
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
        wheel1.transform.Rotate(new Vector3(0f, -wheel1RotationSpeed * turningGear * 3f * Time.deltaTime), Space.World);
        if (turningGear > 1)
            wheel2.transform.Rotate(new Vector3(0f, wheel2RotationSpeed * turningGear * Time.deltaTime), Space.World);
        if (turningGear > 2)
            wheel3.transform.Rotate(new Vector3(0f, -wheel3RotationSpeed * turningGear * Time.deltaTime), Space.World);
    }

    private void TurnRight()
    {
        wheel1.transform.Rotate(new Vector3(0f, wheel1RotationSpeed * turningGear * 3f * Time.deltaTime), Space.World);
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

    private void FireCannonBall(bool special)
    {
        if (special)
            firingSpecialCannonBall = true;
        else
            recoil.StartRecoil();
        canvasManager.UpdateShots();
        Transform cannonBarrel = activeCannon.GetComponent<Cannon>().cannonBarrel;
        Vector3 firingAngle = cannonBarrel.forward;
        Instantiate(cannonBallFX, cannonBarrel.position + firingAngle, cannonBarrel.rotation);
        GameObject newCannonBallGameObject = Instantiate(cannonBall, cannonBarrel.position + firingAngle * 3f, Quaternion.Euler(firingAngle));
        newCannonBallGameObject.transform.forward = firingAngle;
        CannonBall newCannonBall = newCannonBallGameObject.GetComponent<CannonBall>();
        newCannonBall.canvasManager = canvasManager;
        newCannonBall.cannonPlatform = this;
        newCannonBall.mainCamera = mainCamera;
        newCannonBall.wheel1Transform = wheel1.transform;
        if (special)
        {
            mainCamera.transform.parent = newCannonBallGameObject.transform;
            mainCamera.transform.localPosition = new Vector3(0f, 1.2f, -2f);
            newCannonBall.damage = 5;
        }
        if (activeCannon == cannons[0])
            newCannonBallGameObject.GetComponent<Rigidbody>().velocity = firingAngle * 45f;
        else
            newCannonBallGameObject.GetComponent<Rigidbody>().velocity = firingAngle * 65f;
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

    public void Reset()
    {
        health = totalHealth;
        healthBar.UpdateBar(1);
        foreach (GameObject cannon in cannons)
            cannon.GetComponent<Cannon>().ResetTilt();
        turningGear = 1;
        if (activeCannon == cannons[1])
            ToggleCannons();
    }

    public void GetHit(int damage)
    {
        health = (int)Mathf.Clamp(health - damage, 0f, totalHealth);
        float newHealth = Mathf.Clamp((float)health / (float)totalHealth, 0f, 1f);
        healthBar.UpdateBar(newHealth);
        if (health <= 0)
            canvasManager.LoseGame();
    }
}
