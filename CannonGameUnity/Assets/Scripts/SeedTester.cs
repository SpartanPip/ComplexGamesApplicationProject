using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedTester : MonoBehaviour
{
    public GameObject[] props;
    public int seed = 0;

    public void CreateProps()
    {
        Random.InitState(seed);
        int numOfProps = Random.Range(5, 100);
        for (int i = 0; i < numOfProps; i++)
        {
            Vector3 coords = PickCoordinateInRadius(50f, 200f);
            Vector3 pos = new Vector3(coords.x, 0f, coords.z);
            GameObject newProp = Instantiate(props[Random.Range(0, props.Length)], pos, Quaternion.Euler(new Vector3(0f, Random.Range(0f, 360f), 0f)));
            newProp.transform.localScale = new Vector3(Random.Range(0.2f, 3f), Random.Range(0.2f, 3f), Random.Range(0.2f, 3f));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            seed += 1;
            Debug.Log(seed);
            ResetProps();
        }
    }

    void ResetProps()
    {
        foreach (Prop prop in FindObjectsOfType<Prop>())
            Destroy(prop.gameObject);
        CreateProps();
    }

    Vector3 PickCoordinateInRadius(float min, float max)
    {
        float xPos = Random.Range(min, max);
        float zPos = Random.Range(min, max);
        if (Random.Range(0, 2) == 0)
            xPos *= -1;
        if (Random.Range(0, 2) == 0)
            zPos *= -1;
        return new Vector3(xPos, 0f, zPos);
    }
}
