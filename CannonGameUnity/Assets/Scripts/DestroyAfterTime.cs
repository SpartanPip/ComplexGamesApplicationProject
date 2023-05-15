using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float waitTime;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroySelf", waitTime);
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
