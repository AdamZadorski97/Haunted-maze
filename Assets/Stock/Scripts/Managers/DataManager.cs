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
    public CoinsProportiesData coinsProportiesData;


    private List<GameObject> pickablePointsGameObjects = new List<GameObject>();
 
    public float currentPointsMultiplied;

    [SerializeField] public SaveLoadDataManager saveLoadDataManager;
    private void Awake()
    {
        saveLoadDataManager.LoadData();
        ammunitionLeft = saveLoadDataManager.GetWeaponClipValue(0);
        SetAmmunition();
        collectedPoints = 0;
        currentMultipler = 0;
        LevelManager.Instance.uIManager.UpdateUI();
        allLevelPointsAmount = GameObject.FindGameObjectsWithTag("Point").Length;
        pickablePointsGameObjects.AddRange(GameObject.FindGameObjectsWithTag("Point"));
    }
    #region points
    public int GetLevelPointsAmount()
    {
        return allLevelPointsAmount;
    }

    public float GetCurrentPointsMultiplied()
    {
        return currentPointsMultiplied;
    }
    public int GetCurrentCollectedPoints()
    {
        return collectedPoints;
    }

    public void SetPoint()
    {
        collectedPoints += 1;
        currentPointsMultiplied += 1 * GetCoinMultipler();
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
            item.GetComponent<PickablePoint>().SetMultipler(
                GetCoinMultipler().ToString(), 
                GetCoinMultiplersFrameColor(),
                GetCoinMultiplersPlateColor(),
                GetCoinMultiplersTextColor());
        }
        LevelManager.Instance.uIManager.UpdateUI();
        collectedPoints = 1;
    }
    #endregion


    #region ammunition
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
    #endregion


    #region CoinProporties
    public float GetCoinMultipler()
    {
        return coinsProportiesData.coinMultiplers[currentMultipler];
    }

    public string GetCoinMultiplerString()
    {
        return coinsProportiesData.coinMultiplersString[currentMultipler];
    }

    public Color GetCoinMultiplersPlateColor()
    {
        return coinsProportiesData.coinMultiplersPlateColor[currentMultipler];
    }

    public Color GetCoinMultiplersFrameColor()
    {
        return coinsProportiesData.coinMultiplersFrameColor[currentMultipler];
    }

    public Color GetCoinMultiplersTextColor()
    {
        return coinsProportiesData.coinMultiplersTextColor[currentMultipler];
    }
    #endregion



}
