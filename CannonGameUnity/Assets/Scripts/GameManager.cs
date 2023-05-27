using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Tower tower;
    public GameObject playerCannon;
    public GameObject spawnShipFX;
    public Transform wheel1;
    public List<GameObject> existingShips = new List<GameObject>();
    public string levelType;
    public int levelNumber;
    public int totalShipsMade = 0;
    public GameObject[] vikingProps;
    public GameObject[] sailorProps;
    public GameObject[] pirateProps;
    public GameObject[] navyProps;
    public GameObject[] vikingBoats;
    public GameObject[] sailorBoats;
    public GameObject[] pirateBoats;
    public GameObject[] navyBoats;
    bool playingNewLevel = false;
    public WeatherManager weatherManager;

    private Dictionary<string, int[]> propSeeds = new Dictionary<string, int[]>()
    {
        { "Vikings", new int[20] { 1, 2, 3, 4, 6, 10, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 24, 25, 26, 27 } },
        { "Sailors", new int[20] { 1, 2, 3, 4, 6, 10, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 24, 25, 26, 27 } },
        { "Pirates", new int[20] { 1, 2, 3, 4, 6, 10, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 24, 25, 26, 27 } },
        { "Navy", new int[20] { 1, 2, 3, 4, 6, 10, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 24, 25, 26, 27 } }
    };

    private Dictionary<string, GameObject[]> boatGroups = new Dictionary<string, GameObject[]>();


    // Start is called before the first frame update
    void Start()
    {
        boatGroups["Vikings"] = vikingBoats;
        boatGroups["Sailors"] = sailorBoats;
        boatGroups["Pirates"] = pirateBoats;
        boatGroups["Navy"] = navyBoats;
    }

    public void InitializeLevel()
    {
        weatherManager.setWeather(levelType, levelNumber);
        tower.SetTower(levelType);
        CreateProps();
        StartCoroutine(StartCreatingShip());
    }

    void CreateProps()
    {
        UnityEngine.Random.InitState(propSeeds[levelType][levelNumber]);
        GameObject[] props = vikingProps;
        switch (levelType)
        {
            case "Vikings":
                props = vikingProps;
                break;
            case "Sailors":
                props = sailorProps;
                break;
            case "Pirates":
                props = pirateProps;
                break;
            case "Navy":
                props = navyProps;
                break;
        }
        int numOfProps = UnityEngine.Random.Range(5, 100);
        for(int i = 0; i < numOfProps; i++)
        {
            Vector3 coords = PickCoordinateInRadius(50f, 200f);
            Vector3 pos = new Vector3(coords.x, 0f, coords.z);
            GameObject newProp = Instantiate(props[UnityEngine.Random.Range(0, props.Length)], pos, Quaternion.Euler(new Vector3(0f, UnityEngine.Random.Range(0f, 360f), 0f)));
            newProp.transform.localScale = new Vector3(UnityEngine.Random.Range(0.2f, 3f), UnityEngine.Random.Range(0.2f, 3f), UnityEngine.Random.Range(0.2f, 3f));
        }
        UnityEngine.Random.InitState((int)DateTime.Now.Ticks);
    }

    IEnumerator StartCreatingShip()
    {
        playingNewLevel = true;
        while (totalShipsMade < levelNumber + 1 || !playingNewLevel) {
            totalShipsMade += 1;
            CreateShip();
            yield return new WaitForSeconds(5f);
        }
    }

    void CreateShip()
    {
        Vector3 coords = PickCoordinateInRadius(20f, 120);
        Vector3 newShipPosition = new Vector3(coords.x, 0f, coords.z);
        Instantiate(spawnShipFX, newShipPosition, Quaternion.identity);
        GameObject chosenBoat = boatGroups[levelType][(int)Mathf.Floor(levelNumber / 5)];
        GameObject newShip = Instantiate(chosenBoat, newShipPosition, Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f));
        existingShips.Add(newShip);
        newShip.GetComponent<Ship>().gameManager = this;
        newShip.GetComponent<Ship>().playerCannon = playerCannon;
    }

    public void EndLevel()
    {
        playingNewLevel = false;
        foreach(GameObject ship in existingShips)
        {
            Destroy(ship);
        }
        existingShips = new List<GameObject>();
        foreach (ParticleSystem ps in FindObjectsOfType<ParticleSystem>())
            Destroy(ps.gameObject);
        foreach (CannonBall cannonBall in FindObjectsOfType<CannonBall>())
            Destroy(cannonBall.gameObject);
        foreach (EnemyCannonBall cannonBall in FindObjectsOfType<EnemyCannonBall>())
            Destroy(cannonBall.gameObject);
        foreach (Prop prop in FindObjectsOfType<Prop>())
            Destroy(prop.gameObject);
    }

    Vector3 PickCoordinateInRadius(float min, float max)
    {
        float xPos = UnityEngine.Random.Range(min, max);
        float zPos = UnityEngine.Random.Range(min, max);
        if (UnityEngine.Random.Range(0, 2) == 0)
            xPos *= -1;
        if (UnityEngine.Random.Range(0, 2) == 0)
            zPos *= -1;
        bool needToCheckAvailibilityOfPosition = true;
        RaycastHit hit;
        while (needToCheckAvailibilityOfPosition) { 
            if (Physics.Raycast(new Vector3(xPos, 50f, zPos), Vector3.down, out hit, 60, LayerMask.NameToLayer("Default")))
            {
                Debug.Log("looking for new ship placement location");
                xPos = UnityEngine.Random.Range(min, max);
                zPos = UnityEngine.Random.Range(min, max);
                if (UnityEngine.Random.Range(0, 2) == 0)
                    xPos *= -1;
                if (UnityEngine.Random.Range(0, 2) == 0)
                    zPos *= -1;
            }
            else
                needToCheckAvailibilityOfPosition = false;
        }
        return new Vector3(xPos, 0f, zPos);
    }
}
