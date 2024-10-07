using System;
using System.Collections.Generic;
using UnityEngine;
using GoogleSheetsForUnity;

[Serializable]
public class LevelDatas
{
    public double minDistanceFromPlayer;
    public double maxDistanceFromPlayer;
    public double spawnRate;
    public double timeUntilSpawnRateIncrease;
    public double bossSpawnTime;
    public double maxEnemies;
}

[CreateAssetMenu(fileName = "LevelDataSO", menuName = "ScriptableObjects/LevelDataSO", order = 1)]
public class LevelDataSO : ScriptableObject
{
    public string missingTranslation = "Translation not found for that key.";
    public string missingKey = "Key not found in the localization data.";
    public string LevelDataTableName = "LevelData";
    public List<LevelDatas> levelData;


    // Overwrites local translation data with the table obtained from the cloud.
    [ContextMenu("Download Localization Table")]
    public void RetrieveCloudData()
    {
        // Suscribe for catching cloud responses.
        Drive.responseCallback += HandleDriveResponse;
        // Make the query.
        Drive.GetTable(LevelDataTableName, true);
    }

    // Creates a new localization table on the cloud.
    [ContextMenu("Create Drive Localization Table")]
    private void CreateTable()
    {
        // Suscribe to Drive event to get the Drive response.
        Drive.responseCallback += HandleDriveResponse;

        string[] tableHeaders = new string[] { "minDistanceFromPlayer", "maxDistanceFromPlayer", "spawnRate", "timeUntilSpawnRateIncrease", "bossSpawnTime", "maxEnemies" };
        Drive.CreateTable(tableHeaders, LevelDataTableName, false);
    }

    [ContextMenu("Upload Localization Table")]
    private void AddAllKeysToTable()
    {
        // Suscribe to Drive event to get the Drive response.
        Drive.responseCallback += HandleDriveResponse;

        string jsonData = JsonHelper.ToJson(levelData.ToArray());
        Drive.CreateObjects(jsonData, LevelDataTableName, false);
    }

    // Processes the data received from the cloud.
    private void HandleDriveResponse(Drive.DataContainer dataContainer)
    {
        if (dataContainer.objType != LevelDataTableName)
            return;

        // First check the type of answer.
        if (dataContainer.QueryType == Drive.QueryType.getTable)
        {
            string rawJSon = dataContainer.payload;
         //   Debug.Log("Data from Google Drive received.");

            // Parse from json to the desired object type.
            LevelDatas[] levelDatas = JsonHelper.ArrayFromJson<LevelDatas>(rawJSon);
            levelData = new List<LevelDatas>(levelDatas);
        }

        if (dataContainer.QueryType != Drive.QueryType.createTable || dataContainer.QueryType != Drive.QueryType.createObjects)
        {
           // Debug.Log(dataContainer.msg);
        }
        Drive.responseCallback -= HandleDriveResponse;
    }
}
