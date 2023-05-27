using System.Collections;
using UnityEngine;

public class Recoil : MonoBehaviour
{
    public float recoilDuration = 0.2f;   // Duration of the recoil effect
    public float recoilDistance = 0.2f;   // Distance the camera moves back during recoil
    public float returnSpeed = 5f;        // Speed at which the camera returns to its original position

    public Vector3 originalPosition;
    public bool isRecoiling = false;

    public CannonPlatform cannonPlatform;

    private void Start()
    {
        originalPosition = transform.localPosition;
    }

    private void Update()
    {
        if (isRecoiling)
        {
            // Move the camera back during recoil
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition - transform.up * recoilDistance, Time.deltaTime / recoilDuration);
        }
        else if (!cannonPlatform.firingSpecialCannonBall)
        {
            // Return the camera to its original position
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, Time.deltaTime * returnSpeed);
        }
    }

    public void StartRecoil()
    {
        StartCoroutine("StartRecoilTimer");
    }

    private IEnumerator StartRecoilTimer()
    {
        isRecoiling = true;

        yield return new WaitForSeconds(recoilDuration);

        isRecoiling = false;
    }
}
