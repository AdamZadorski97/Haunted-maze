using System;
using System.Collections.Generic;
using UnityEngine;

namespace GoogleSheetsForUnity
{
    /// <summary>
    /// Example of Unity Editor use for updating a local data storage (based on a ScriptableObject).
    /// This class represents each text element on the game to be translated. Select the LocalizationData
    /// asset, go to the inspector, click on the gear icon, and select an option from the menu as needed:
    /// 
    /// * "Create Drive Localization Table"
    /// * "Upload Localization Table"
    /// * "Download Localization Table"
    /// 
    /// </summary>
    [Serializable]
    public class Localization
    {
        public string key;
        public string english;
        public string spanish;
    }

    // List of available languages.
    public enum Languages
    {
        english = 0,
        spanish = 1,
    }

    //[CreateAssetMenu(fileName = "LocalizationData", menuName = "Google Sheets For Unity/Localization Data Example Asset")]
    public class LocalizationDataSO : ScriptableObject
    {
        public string missingTranslation = "Translation not found for that key.";
        public string missingKey = "Key not found in the localization data.";
        public string localizationTableName = "Localization";
        public List<Localization> localizationData;

        // Overwrites local translation data with the table obtained from the cloud.
        [ContextMenu("Download Localization Table")]
        private void RetrieveCloudData()
        {
            // Suscribe for catching cloud responses.
            Drive.responseCallback += HandleDriveResponse;
            // Make the query.
            Drive.GetTable(localizationTableName, false);
        }

        // Creates a new localization table on the cloud.
        [ContextMenu("Create Drive Localization Table")]
        private void CreateTable()
        {
            // Suscribe to Drive event to get the Drive response.
            Drive.responseCallback += HandleDriveResponse;

            string[] tableHeaders = new string[] { "key", "english", "spanish" };
            Drive.CreateTable(tableHeaders, localizationTableName, false);
        }

        [ContextMenu("Upload Localization Table")]
        private void AddAllKeysToTable()
        {
            // Suscribe to Drive event to get the Drive response.
            Drive.responseCallback += HandleDriveResponse;
                        
            string jsonData = JsonHelper.ToJson(localizationData.ToArray());
            Drive.CreateObjects(jsonData, localizationTableName, false);
        }

        // Processes the data received from the cloud.
        private void HandleDriveResponse(Drive.DataContainer dataContainer)
        {
            if (dataContainer.objType != localizationTableName)
                return;

            // First check the type of answer.
            if (dataContainer.QueryType == Drive.QueryType.getTable)
            {
                string rawJSon = dataContainer.payload;
                Debug.Log("Data from Google Drive received.");

                // Parse from json to the desired object type.
                Localization[] localization = JsonHelper.ArrayFromJson<Localization>(rawJSon);
                localizationData = new List<Localization>(localization);
            }

            if (dataContainer.QueryType != Drive.QueryType.createTable || dataContainer.QueryType != Drive.QueryType.createObjects)
            {
                Debug.Log(dataContainer.msg);
            }

            Drive.responseCallback -= HandleDriveResponse;
        }
    }
}