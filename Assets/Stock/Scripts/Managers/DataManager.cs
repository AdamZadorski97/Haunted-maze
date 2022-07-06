using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private int ammunitionMagazine = 10;
    private int ammunition;
    private int collectedPoints;
    private int currentPointsMultipler;
    private int allLevelPointsAmount;
    private List<GameObject> pickablePointsGameObjects = new List<GameObject>();
    [SerializeField] private List<Color> multiplerColor = new List<Color>();
    private void Awake()
    {
        ammunition = 7;
        collectedPoints = 0;
        currentPointsMultipler = 1;
        LevelManager.Instance.uIManager.UpdateUI();
        allLevelPointsAmount = GameObject.FindGameObjectsWithTag("Point").Length;
        pickablePointsGameObjects.AddRange(GameObject.FindGameObjectsWithTag("Point"));

    }



    public int GetLevelPointsAmount()
    {
        return allLevelPointsAmount;
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
        return currentPointsMultipler;
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
        if (collectedPoints == allLevelPointsAmount)
        {
            
            Invoke("ResetPoints", 0.1f);
        }
       
        LevelManager.Instance.uIManager.UpdateCurrentPoints();
    }
    private void ResetPoints()
    {
        currentPointsMultipler++;
        foreach (GameObject item in pickablePointsGameObjects)
        {
            item.SetActive(true);
            item.GetComponent<PickablePoint>().InteractivePointMesh.material.SetColor("_Color", multiplerColor[currentPointsMultipler]);
        }
        collectedPoints = 1;
       
    }
}
