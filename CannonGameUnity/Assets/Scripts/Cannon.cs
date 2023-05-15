using UnityEngine;

public class Cannon : MonoBehaviour
{
    public Transform cannonBarrel;

    private float minAngle = 35f;
    private float maxAngle = 350f;

    public bool longRangeCannon;

    private void Start()
    {
        if (longRangeCannon)
        {
            maxAngle = 355f;
            minAngle = 340f;
        }
    }

    public void ResetTilt()
    {
        if (longRangeCannon)
            cannonBarrel.localEulerAngles = new Vector3(-10f, 0f, 0f); 
        else
            cannonBarrel.localEulerAngles = new Vector3(0f, 0f, 0f);
    }

    public void TiltUp(AudioSource cannonAudioSource)
    {
        if (longRangeCannon && cannonBarrel.localEulerAngles.x > minAngle)
        {
            cannonBarrel.Rotate(new Vector3(-10f * Time.deltaTime, 0f, 0f), Space.Self);
            if (!cannonAudioSource.isPlaying)
                cannonAudioSource.Play();
        }
        else if (!longRangeCannon && (cannonBarrel.localEulerAngles.x > maxAngle || cannonBarrel.localEulerAngles.x < 100f))
        {
            cannonBarrel.Rotate(new Vector3(-10f * Time.deltaTime, 0f, 0f), Space.Self);
            if (!cannonAudioSource.isPlaying)
                cannonAudioSource.Play();
        }
        else
        {
            cannonAudioSource.Pause();
        }
    }

    public void TiltDown(AudioSource cannonAudioSource)
    {
        if (longRangeCannon && cannonBarrel.localEulerAngles.x < maxAngle)
        {
            cannonBarrel.Rotate(new Vector3(10f * Time.deltaTime, 0f, 0f), Space.Self);
            if (!cannonAudioSource.isPlaying)
                cannonAudioSource.Play();
        }
        else if (!longRangeCannon && (cannonBarrel.localEulerAngles.x < minAngle || cannonBarrel.localEulerAngles.x > 100f))
        {
            cannonBarrel.Rotate(new Vector3(10f * Time.deltaTime, 0f, 0f), Space.Self);
            if (!cannonAudioSource.isPlaying)
                cannonAudioSource.Play();
        }
        else
        {
            cannonAudioSource.Pause();
        }
    }
}
