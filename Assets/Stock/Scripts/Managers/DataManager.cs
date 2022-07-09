using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private int ammunitionMagazine = 10;
    private int ammunition;
    private int collectedPoints;
    private int currentMultipler;
    private int allLevelPointsAmount;
    public SaveData saveData;

    private List<GameObject> pickablePointsGameObjects = new List<GameObject>();
    [SerializeField] private List<Color> multiplerColor = new List<Color>();
    [SerializeField] private List<int> multiplerValue = new List<int>();
    private int currentPointsMultiplied;
   
    private void Awake()
    {
        ammunition = 7;
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
    public int GetCurrentAmmuniton()
    {
        return ammunition;
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
        if (GetCurrentAmmuniton() > 0)
        {
            ammunition--;
            LevelManager.Instance.uIManager.UpdateAmmunition();
            return true;
        }
        return false;
    }
    public void SetAmmunition()
    {
        ammunition = ammunitionMagazine;
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
