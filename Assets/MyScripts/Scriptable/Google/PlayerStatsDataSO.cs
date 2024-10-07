using System;
using System.Collections.Generic;
using UnityEngine;
using GoogleSheetsForUnity;

[Serializable]
public class PlayerDatas
{
    public double hpValue;
    public double hpUpgradeCost;
    public double sprintTimeValue;
    public double sprintTimeUpgradeCost;
    public double sprintReloadSpeedValue;
    public double sprintReloadSpeedUpgradeCost;
}



[CreateAssetMenu(fileName = "PlayerDataSO", menuName = "ScriptableObjects/PlayerDataSO", order = 1)]
public class PlayerStatsDataSO : ScriptableObject
{
    public string missingTranslation = "Translation not found for that key.";
    public string missingKey = "Key not found in the localization data.";
    public string PlayerDataTableName = "PLayerData";
    public List<PlayerDatas> playerData;

    // Overwrites local translation data with the table obtained from the cloud.
    [ContextMenu("Download Localization Table")]
    public void RetrieveCloudData()
    {
        // Suscribe for catching cloud responses.
        Drive.responseCallback += HandleDriveResponse;
        // Make the query.
        Drive.GetTable(PlayerDataTableName, true);
    }

    // Creates a new localization table on the cloud.
    [ContextMenu("Create Drive Localization Table")]
    private void CreateTable()
    {
        // Suscribe to Drive event to get the Drive response.
        Drive.responseCallback += HandleDriveResponse;

        string[] tableHeaders = new string[] { "hpValue", "hpUpgradeCost", "sprintTimeValue", "sprintTimeUpgradeCost", "sprintReloadSpeedValue", "sprintReloadSpeedUpgradeCost" };
        Drive.CreateTable(tableHeaders, PlayerDataTableName, false);
    }

    [ContextMenu("Upload Localization Table")]
    private void AddAllKeysToTable()
    {
        // Suscribe to Drive event to get the Drive response.
        Drive.responseCallback += HandleDriveResponse;

        string jsonData = JsonHelper.ToJson(playerData.ToArray());
        Drive.CreateObjects(jsonData, PlayerDataTableName, false);
    }

    // Processes the data received from the cloud.
    private void HandleDriveResponse(Drive.DataContainer dataContainer)
    {
        if (dataContainer.objType != PlayerDataTableName)
            return;

        // First check the type of answer.
        if (dataContainer.QueryType == Drive.QueryType.getTable)
        {
            string rawJSon = dataContainer.payload;
          //  Debug.Log("Data from Google Drive received.");

            // Parse from json to the desired object type.
            PlayerDatas[] playerDatas = JsonHelper.ArrayFromJson<PlayerDatas>(rawJSon);
            playerData = new List<PlayerDatas>(playerDatas);
        }

        if (dataContainer.QueryType != Drive.QueryType.createTable || dataContainer.QueryType != Drive.QueryType.createObjects)
        {
         //   Debug.Log(dataContainer.msg);
        }
        Drive.responseCallback -= HandleDriveResponse;
    }
}