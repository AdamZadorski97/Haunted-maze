using System;
using System.Collections.Generic;
using UnityEngine;
using GoogleSheetsForUnity;

[Serializable]
public class WeaponData
{
    public double damageValue;
    public double damageUpgradeCost;
    public double clipValue;
    public double clipUpgradeCost;
    public double reloadTimeValue;
    public double reloadTimeUpgradeCost;
    public double shootSpeedTimeValue;
    public double shootSpeedTimeUpgradeCost;
    public double knockbackValue;
    public double knockbackUpgradeCost;
    public double knockbackChanceValue;
    public double knockbackChanceUpgradeCost;
}



[CreateAssetMenu(fileName = "WeaponDataSO", menuName = "ScriptableObjects/WeaponDataSO", order = 1)]
public class WeaponDataSO : ScriptableObject
{
    public string missingTranslation = "Translation not found for that key.";
    public string missingKey = "Key not found in the localization data.";
    public string weaponDataTableName = "WeaponData";
    public List<WeaponData> weaponData;

    // Overwrites local translation data with the table obtained from the cloud.
    [ContextMenu("Download Localization Table")]
    public void RetrieveCloudData()
    {
        // Suscribe for catching cloud responses.
        Drive.responseCallback += HandleDriveResponse;
        // Make the query.
        Drive.GetTable(weaponDataTableName, true);
    }

    // Creates a new localization table on the cloud.
    [ContextMenu("Create Drive Localization Table")]
    private void CreateTable()
    {
        // Suscribe to Drive event to get the Drive response.
        Drive.responseCallback += HandleDriveResponse;

        string[] tableHeaders = new string[] { "damageValue", "damageUpgradeCost", "clipValue", "clipUpgradeCost", "reloadTimeValue", "reloadTimeUpgradeCost", "knockbackChanceValue", "knockbackChanceUpgradeCost" };
        Drive.CreateTable(tableHeaders, weaponDataTableName, false);
    }

    [ContextMenu("Upload Localization Table")]
    private void AddAllKeysToTable()
    {
        // Suscribe to Drive event to get the Drive response.
        Drive.responseCallback += HandleDriveResponse;

        string jsonData = JsonHelper.ToJson(weaponData.ToArray());
        Drive.CreateObjects(jsonData, weaponDataTableName, false);
    }

    // Processes the data received from the cloud.
    private void HandleDriveResponse(Drive.DataContainer dataContainer)
    {
        if (dataContainer.objType != weaponDataTableName)
            return;

        // First check the type of answer.
        if (dataContainer.QueryType == Drive.QueryType.getTable)
        {
            string rawJSon = dataContainer.payload;
          //  Debug.Log("Data from Google Drive received.");

            // Parse from json to the desired object type.
            WeaponData[] weapons = JsonHelper.ArrayFromJson<WeaponData>(rawJSon);
            weaponData = new List<WeaponData>(weapons);
        }

        if (dataContainer.QueryType != Drive.QueryType.createTable || dataContainer.QueryType != Drive.QueryType.createObjects)
        {
          //  Debug.Log(dataContainer.msg);
        }
        Drive.responseCallback -= HandleDriveResponse;
    }
}

