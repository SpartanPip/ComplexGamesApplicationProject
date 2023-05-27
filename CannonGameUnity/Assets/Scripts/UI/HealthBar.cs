using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public bool isPlayersHealthBar = false;
    public Slider bar;
    Vector3 camPos;

    private void Start()
    {
        camPos = FindObjectOfType<Camera>().gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isPlayersHealthBar)
            transform.LookAt(camPos);
    }

    public void UpdateBar(float percentage)
    {
        bar.value = percentage;
    }
}

