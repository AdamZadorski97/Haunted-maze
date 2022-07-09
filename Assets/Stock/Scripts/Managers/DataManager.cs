using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private int maxAmmunitionInMagazine = 10;
    private float ammunitionInMagazine;
    private float ammunitionLeft;
    private float ammunitionOnStart;
    private int collectedPoints;
    private int currentMultipler;
    private int allLevelPointsAmount;
    public SaveData saveData;

    private List<GameObject> pickablePointsGameObjects = new List<GameObject>();
    [SerializeField] private List<Color> multiplerColor = new List<Color>();
    [SerializeField] private List<int> multiplerValue = new List<int>();
    public int currentPointsMultiplied;

   [SerializeField] public SaveLoadDataManager saveLoadDataManager;
    private void Awake()
    {
        saveLoadDataManager.LoadData();
        ammunitionLeft = saveLoadDataManager.GetWeaponClipValue(0);
        collectedPoints = 0;
        currentMultipler = 1;
        LevelManager.Instance.uIManager.UpdateUI();
        allLevelPointsAmount = GameObject.FindGameObjectsWithTag("Point").Length;
        pickablePointsGameObjects.AddRange(GameObject.FindGameObjectsWithTag("Point"));
    }

    public int GetLevelPointsAmount()
    {
        return allLevelPointsAmount;
    }
    public int GetCurrentPointsMultiplied()
    {
        return currentPointsMultiplied;
    }
    public float GetLeftAmmunition()
    {
        return ammunitionLeft;
    }
    public float GetMaxAmmunitionInMagazine()
    {
        return maxAmmunitionInMagazine;
    }
    public float GetAmmunitionInMagazine()
    {
        return ammunitionInMagazine;
    }
    public int GetCurrentCollectedPoints()
    {
        return collectedPoints;
    }
    public int GetCurrentPointsPointsMultipler()
    {
        return currentMultipler;
    }

    public bool CheckCanShoot()
    {
        if (GetAmmunitionInMagazine() > 0)
        {
            ammunitionInMagazine--;
            LevelManager.Instance.uIManager.UpdateAmmunition();
            return true;
        }
        return false;
    }
    public void SetAmmunition()
    {
        if (GetLeftAmmunition() > 10)
        {
            ammunitionInMagazine = maxAmmunitionInMagazine;
            ammunitionLeft -= 10;
        }

        else
        {
            ammunitionInMagazine = GetLeftAmmunition();
            ammunitionLeft = 0;
        }
        LevelManager.Instance.uIManager.UpdateAmmunition();
    }
    public void SetPoint()
    {
        collectedPoints += 1;
        currentPointsMultiplied += 1 * multiplerValue[currentMultipler];
        if (collectedPoints == allLevelPointsAmount)
        {
            
            Invoke("ResetPoints", 0.1f);
        }
       
        LevelManager.Instance.uIManager.UpdateCurrentPoints();
    }
    private void ResetPoints()
    {
        currentMultipler++;
        foreach (GameObject item in pickablePointsGameObjects)
        {
            item.SetActive(true);
            item.GetComponent<PickablePoint>().InteractivePointMesh.material.SetColor("_Color", multiplerColor[currentMultipler]);
        }
        collectedPoints = 1;
    }
}
