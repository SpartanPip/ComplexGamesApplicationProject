using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    public GameObject normalWeather;
    public GameObject vikingWeather;
    public GameObject sailorWeather;
    public GameObject pirateWeather;
    public GameObject navyWeather;

    public void setWeather(string levelType, int levelNumber)
    {
        normalWeather.SetActive(false);
        vikingWeather.SetActive(false);
        sailorWeather.SetActive(false);
        pirateWeather.SetActive(false);
        navyWeather.SetActive(false);
        bool isNormal = levelNumber % 2 == 0 ? true : false;
        switch (levelType){
            case "Vikings":
                normalWeather.SetActive(isNormal);
                vikingWeather.SetActive(!isNormal);
                break;
            case "Sailors":
                normalWeather.SetActive(isNormal);
                sailorWeather.SetActive(!isNormal);
                break;
            case "Pirates":
                normalWeather.SetActive(isNormal);
                pirateWeather.SetActive(!isNormal);
                break;
            case "Navy":
                normalWeather.SetActive(isNormal);
                navyWeather.SetActive(!isNormal);
                break;
        }
    }
}
