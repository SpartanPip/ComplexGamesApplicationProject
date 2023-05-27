using UnityEngine;

public class Tower : MonoBehaviour
{
    public GameObject vikingTower;
    public GameObject sailorTower;
    public GameObject pirateTower;
    public GameObject navyTower;

    private void Start()
    {
        sailorTower.SetActive(false);
        pirateTower.SetActive(false);
        navyTower.SetActive(false);
    }

    void HideTowers()
    {
        vikingTower.SetActive(false);
        sailorTower.SetActive(false);
        pirateTower.SetActive(false);
        navyTower.SetActive(false);
    }

    public void SetTower(string levelType)
    {
        HideTowers();
        switch (levelType)
        {
            case "Vikings":
                vikingTower.SetActive(true);
                break;
            case "Sailors":
                sailorTower.SetActive(true);
                break;
            case "Pirates":
                pirateTower.SetActive(true);
                break;
            case "Navy":
                navyTower.SetActive(true);
                break;
        }
    }
}
