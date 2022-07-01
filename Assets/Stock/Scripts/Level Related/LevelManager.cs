using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
public class LevelManager : MonoSingleton<LevelManager>
{
    public static LevelManager _Instance { get; private set; }

    public List<FloorController> floorControllers = new List<FloorController>();
    public List<WallController> wallControllers = new List<WallController>();
    public List<PickablePoint> pickablePoints = new List<PickablePoint>();
    public EnemySpawnerController enemySpawner;
  
    public int currentPlayerFloor;
    public int currentLevelMoneyCollected;
    public int moneyToUnlockKey;
    public GameObject NextLevelKey;

    public TMP_Text fpsCounterText;
    public float fps;
    public void Start()
    {
        Application.targetFrameRate = 300;
        InvokeRepeating("GetFPS", 1, 1);
    }

    private void Awake()
    {
        floorControllers = GameObject.FindObjectsOfType<FloorController>().ToList();
        wallControllers = GameObject.FindObjectsOfType<WallController>().ToList();
        pickablePoints = GameObject.FindObjectsOfType<PickablePoint>().ToList();
    }

    public void GetFPS()
    {
        fps = (int)(1f / Time.unscaledDeltaTime);
        fpsCounterText.text = fps + "fps";
    }



    public void OnMoneyCollect(int value)
    {
        currentLevelMoneyCollected += value;

        if (moneyToUnlockKey >= currentLevelMoneyCollected)
        {
            UnlockNextLevelKey();
        }
    }

    public void UnlockNextLevelKey()
    {
        NextLevelKey.gameObject.SetActive(true);
    }






    public void ShowFloor(int number)
    {
        currentPlayerFloor = number;
        foreach (var item in wallControllers)
        {
            if (Mathf.Round(item.transform.position.y) == number * 3)
            {
                foreach (GameObject mapLine in item.mapLines)
                {
                    mapLine.SetActive(true);
                }
            }
        }
        foreach (var item in floorControllers)
        {
            if (Mathf.Round(item.transform.position.y) == number * 3)
            {
                if (item.interactivePoint != null)
                    item.interactivePoint.SetActive(true);
            }
        }
        foreach (var item in enemySpawner.spawnedEnemies)
        {
            if (Mathf.Round(item.transform.position.y) == (number * 3))
            {
                item.gameObject.SetActive(true);
            }
        }


    }
    public void HideFloor(int number)
    {
   
        foreach (var item in wallControllers)
        {
            if (Mathf.Round(item.transform.position.y) == number * 3)
            {
                foreach (GameObject mapLine in item.mapLines)
                {
                    mapLine.SetActive(false);
                }
            }
        }
        foreach (var item in floorControllers)
        {
            if (Mathf.Round(item.transform.position.y) == number * 3)
            {
                if (item.interactivePoint != null)
                    item.interactivePoint.SetActive(false);
            }
        }

        foreach (var item in enemySpawner.spawnedEnemies)
        {
            if (Mathf.Round(item.transform.position.y) == (number * 3))
            {
                item.gameObject.SetActive(false);
            }
        }
    }
}
