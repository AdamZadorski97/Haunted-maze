using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private int maxAmmunitionInMagazine = 10;
    private float ammunitionInMagazine;
    private float ammunitionLeft;
    private int currentKilledUnits;
    private int collectedPoints;
    private int allCollectedPoints;
    [SerializeField] private int currentMultipler; 
    private float currentKillsMultipler = 0.1f;
    private int allLevelPointsAmount;
    private int currentWeaponID;
   [SerializeField] private float currentPointsMultiplied;
    public SaveData saveData;
    public CoinsProportiesData coinsProportiesData;
    public AudioSource audioSource;
    public AudioClip resetCoinsSound;

    private List<GameObject> pickablePointsGameObjects = new List<GameObject>();
 


    [SerializeField] public SaveLoadDataManager saveLoadDataManager;
    private void Awake()
    {
        saveLoadDataManager.LoadData();
        ammunitionLeft = saveLoadDataManager.GetWeaponClipValue(0);
        SetAmmunition();
        collectedPoints = 0;
        currentMultipler = 1;
        LevelManager.Instance.uIManager.UpdateUI();
        AllLevelPointsAmount = GameObject.FindGameObjectsWithTag("Point").Length;
        pickablePointsGameObjects.AddRange(GameObject.FindGameObjectsWithTag("Point"));
    }

    public int CurrentKilledUnits
    {
        get { return currentKilledUnits; }
        set { currentKilledUnits = value; }
    }

    #region points

    public int AllLevelPointsAmount
    {
        get { return allLevelPointsAmount; }
        set { allLevelPointsAmount = value; }
    }
    public int CollectedPoints
    {
        get { return collectedPoints; }
        set { collectedPoints = value; }
    }

    public int AllCollectedPoints
    {
        get { return allCollectedPoints; }
        set { allCollectedPoints = value; }
    }


    public float CurrentPointsMultiplied
    {
        get { return currentPointsMultiplied; }
        set { currentPointsMultiplied = value; }
    }

    public int CurrentMultipler
    {
        get { return currentMultipler; }
        set { currentMultipler = value; }
    }

    public void SetPoint()
    {
        AllCollectedPoints += 1;
        CollectedPoints += 1;

        CurrentPointsMultiplied += 1 * CurrentMultipler ;
   

        if (collectedPoints == AllLevelPointsAmount)
        {
            Invoke("ResetPoints", 0.1f);
        }
        LevelManager.Instance.uIManager.UpdateCurrentPoints();
    }

    private void ResetPoints()
    {
        audioSource.PlayOneShot(resetCoinsSound);
        CurrentMultipler++;
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

    public float AmmunitionLeft
    {
        get { return ammunitionLeft; }
        set { ammunitionLeft = value; }
    }

    public float AmmunitionInMagazine
    {
        get { return ammunitionInMagazine; }
        set { ammunitionInMagazine = value; }
    }

    public int MaxAmmunitionInMagazine
    {
        get { return maxAmmunitionInMagazine; }
        set { maxAmmunitionInMagazine = value; }
    }

    public bool CheckCanShoot()
    {
        if (AmmunitionInMagazine > 0)
        {
            AmmunitionInMagazine--;
            LevelManager.Instance.uIManager.UpdateAmmunition();
            return true;
        }
        return false;
    }

    public void SetAmmunition()
    {
        if (AmmunitionLeft > 10)
        {
            AmmunitionInMagazine = MaxAmmunitionInMagazine;
            AmmunitionLeft -= 10;
        }

        else
        {
            AmmunitionInMagazine = AmmunitionLeft;
            AmmunitionLeft = 0;
        }
        LevelManager.Instance.uIManager.UpdateAmmunition();
    }

    public float GetReloadTime()
    {
        return saveLoadDataManager.GetWeaponRealoadTime(currentWeaponID);
    }

    public float GetKillMultipler()
    {
        if (CurrentKilledUnits > 0)
            return 1+ (CurrentKilledUnits * currentKillsMultipler);
        else
            return 1;
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
