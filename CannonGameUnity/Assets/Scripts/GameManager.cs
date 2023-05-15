using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject spawnShipFX;
    public GameObject[] ships;
    public List<GameObject> existingShips = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("StartCreatingShip", 4f, 10f);
    }

    void StartCreatingShip()
    {
        StartCoroutine(CreateShip());
    }

    IEnumerator CreateShip()
    {
        yield return new WaitForSeconds(0f);
        if (existingShips.Count != 10)
        {
            float xPos = Random.Range(20f, 140f);
            float zPos = Random.Range(20f, 140f);
            if (Random.Range(0, 2) == 0)
                xPos *= -1;
            if (Random.Range(0, 2) == 0)
                zPos *= -1;
            Vector3 newShipPosition = new Vector3(xPos, 0f, zPos);
            Instantiate(spawnShipFX, newShipPosition, Quaternion.identity);
            yield return new WaitForSeconds(3f);
            GameObject newShip = Instantiate(ships[Random.Range(0, ships.Length)], newShipPosition, Quaternion.Euler(0f, Random.Range(0f, 360f), 0f));
            existingShips.Add(newShip);
            newShip.GetComponent<Ship>().gameManager = this;
        }
    }

    public void DestroyAllShips()
    {
        foreach(GameObject ship in existingShips)
        {
            Destroy(ship);
        }
        existingShips = new List<GameObject>();
    }
}
