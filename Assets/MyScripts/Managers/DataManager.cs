using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [SerializeField] private string levelName;
    private double maxAmmunitionInMagazine = 12;
    private double ammunitionInMagazine;
    private double ammunitionLeft;
    private double currentKilledUnits;
    private double collectedPoints;
    private double allCollectedPoints;
    [SerializeField] private int currentMultipler;
    private double currentKillsMultipler = 0.1f;
    private double allLevelPointsAmount;
    private int currentWeaponID;
    [SerializeField] private double currentPointsMultiplied;
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

        Debug.Log(saveLoadDataManager.GetLevelPrestigeLevel(levelName));
        currentMultipler = saveLoadDataManager.GetLevelPrestigeLevel(levelName);



        LevelManager.Instance.uIManager.UpdateUI();
        AllLevelPointsAmount = GameObject.FindGameObjectsWithTag("Point").Length;
        pickablePointsGameObjects.AddRange(GameObject.FindGameObjectsWithTag("Point"));

        foreach (GameObject item in pickablePointsGameObjects)
        {
            item.SetActive(true);
            item.GetComponent<PickablePoint>().SetMultipler(
                GetCoinTexture(),
                GetCoinMultiplersFrameColor(),
                GetCoinMultiplersPlateColor());
        }
    }

    public double CurrentKilledUnits
    {
        get { return currentKilledUnits; }
        set { currentKilledUnits = value; }
    }

    #region points

    public double AllLevelPointsAmount
    {
        get { return allLevelPointsAmount; }
        set { allLevelPointsAmount = value; }
    }
    public double CollectedPoints
    {
        get { return collectedPoints; }
        set { collectedPoints = value; }
    }

    public double AllCollectedPoints
    {
        get { return allCollectedPoints; }
        set { allCollectedPoints = value; }
    }


    public double CurrentPointsMultiplied
    {
        get { return currentPointsMultiplied; }
        set { currentPointsMultiplied = value; }
    }

    public int CurrentMultipler
    {
        get { return currentMultipler; }
        set { currentMultipler = value; }
    }

    public string LevelName
    {
        get { return levelName; }
    }

    public void SetPoint()
    {
        AllCollectedPoints += 1;
        CollectedPoints += 1;

        CurrentPointsMultiplied += 1 * coinsProportiesData.coinMultiplers[CurrentMultipler];

        if (AllLevelPointsAmount > 0)
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
                GetCoinTexture(),
                GetCoinMultiplersFrameColor(),
                GetCoinMultiplersPlateColor());
        }
        LevelManager.Instance.uIManager.UpdateUI();
        collectedPoints = 1;
    }








    #endregion


    #region ammunition

    public double AmmunitionLeft
    {
        get { return ammunitionLeft; }
        set { ammunitionLeft = value; }
    }

    public double AmmunitionInMagazine
    {
        get { return ammunitionInMagazine; }
        set { ammunitionInMagazine = value; }
    }

    public double MaxAmmunitionInMagazine
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
        LevelManager.Instance.uIManager.UpdateAmmunition();
        return false;
    }


    public void AddAmmunition()
    {
        AmmunitionLeft += 10;
        LevelManager.Instance.uIManager.UpdateAmmunition();
    }

    public void SetAmmunition()
    {

        double emptyStore = maxAmmunitionInMagazine - AmmunitionInMagazine;
        if (AmmunitionLeft > emptyStore)
        {
            AmmunitionInMagazine += emptyStore;
            AmmunitionLeft -= emptyStore;
        }
        else
        {
            AmmunitionInMagazine += AmmunitionLeft;
            AmmunitionLeft = 0;
        }

        LevelManager.Instance.uIManager.UpdateAmmunition();
    }

    public double GetReloadTime()
    {
        return saveLoadDataManager.GetWeaponRealoadTime(currentWeaponID);
    }

    public double GetWeaponDamage()
    {
        return saveLoadDataManager.GetWeaponDamageValue(currentWeaponID);
    }

    public double GetKillMultipler()
    {
        if (CurrentKilledUnits > 0)
            return 1 + (CurrentKilledUnits * currentKillsMultipler);
        else
            return 1;
    }

    #endregion


    #region CoinProporties
    public double GetCoinMultipler()
    {
        return coinsProportiesData.coinMultiplers[currentMultipler];
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

    public Texture GetCoinTexture()
    {
        return coinsProportiesData.coinTexture[currentMultipler];
    }
    #endregion



}
