using System;
using UnityEngine;

namespace GoogleSheetsForUnity
{
    /* 
        This example will create a number of buttons on the scene, with self describing actions.
        It introduces to basic operations to handle spreadsheets with the API to make CRUD operations on:
        tables (worksheets) with fields (column headers), as well as objects (rows) on those tables.
    */
    public class SpreadsheetsExample : MonoBehaviour
    {
        // Simple struct for the example.
        [System.Serializable]
        public struct PlayerInfo
        {
            public string name;
            public int level;
            public float health;
            public string role;
        }

        // Create an example object.
        private PlayerInfo _playerData = new PlayerInfo { name = "Mithrandir", level = 99, health = 98.6f, role = "Wizzard" };
        
        // For the table to be created and queried.
        private string _tableName = "PlayerInfo";

        

        private void OnEnable()
        {
            // Suscribe for catching cloud responses.
            Drive.responseCallback += HandleDriveResponse;
        }

        private void OnDisable()
        {
            // Remove listeners.
            Drive.responseCallback -= HandleDriveResponse;
        }

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 10, 800, 1000));
            GUILayout.BeginHorizontal();
            GUILayout.Space(10f);
            GUILayout.BeginVertical();

            GUILayout.Label("This example uses the Google Sheets for Unity API to work with data on spreadsheets, in a database-like approach.\n" +
                "You can create tables (worksheets) and objects (rows), update them, and retrieve them (one by one, or all at once).",
                GUILayout.MaxWidth(800f));

            GUILayout.Space(10f);
            GUILayout.BeginVertical("Example 'Player' Object Data:", GUI.skin.box, GUILayout.MaxWidth(230));
            GUILayout.Space(20f);

            GUILayout.BeginHorizontal();
            GUILayout.Space(10f);
            GUILayout.Label("Player Name:", GUILayout.MaxWidth(100f));
            _playerData.name = GUILayout.TextField(_playerData.name, GUILayout.MaxWidth(100f));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Space(10f);
            GUILayout.Label("Player Level:", GUILayout.MaxWidth(100f));
            _playerData.level = int.Parse(GUILayout.TextField(_playerData.level.ToString(), GUILayout.MaxWidth(100f)));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Space(10f);
            GUILayout.Label("Player Health:", GUILayout.MaxWidth(100f));
            _playerData.health = float.Parse(GUILayout.TextField(_playerData.health.ToString(), GUILayout.MaxWidth(100f)));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Space(10f);
            GUILayout.Label("Player Role:", GUILayout.MaxWidth(100f));
            _playerData.role = GUILayout.TextField(_playerData.role, GUILayout.MaxWidth(100f));
            GUILayout.EndHorizontal();
            GUILayout.Space(5f);
            GUILayout.EndVertical();

            GUILayout.Space(10f);

            if (GUILayout.Button("Create Table", GUILayout.MinHeight(20f), GUILayout.MaxWidth(220f)))
                CreatePlayerTable();

            if (GUILayout.Button("Create Player", GUILayout.MinHeight(20f), GUILayout.MaxWidth(220f)))
                SavePlayer();

            if (GUILayout.Button("Update Player By Name", GUILayout.MinHeight(20f), GUILayout.MaxWidth(220f)))
                UpdatePlayer(false);

            if (GUILayout.Button("Update or Create Player By Name", GUILayout.MinHeight(20f), GUILayout.MaxWidth(220f)))
                UpdatePlayer(true);

            if (GUILayout.Button("Retrieve Player By Name", GUILayout.MinHeight(20f), GUILayout.MaxWidth(220f)))
                RetrievePlayer();

            if (GUILayout.Button("Retrieve All Players", GUILayout.MinHeight(20f), GUILayout.MaxWidth(220f)))
                GetAllPlayers();

            if (GUILayout.Button("Retrieve All Tables", GUILayout.MinHeight(20f), GUILayout.MaxWidth(220f)))
                GetAllTables();

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        private void CreatePlayerTable()
        {
            Debug.Log("<color=yellow>Creating a table in the cloud for players data.</color>");

            // Creating a string array for field names (table headers) .
            string[] fieldNames = new string[4];
            fieldNames[0] = "name";
            fieldNames[1] = "level";
            fieldNames[2] = "health";
            fieldNames[3] = "role";

            // Request for the table to be created on the cloud.
            Drive.CreateTable(fieldNames, _tableName, true);
        }

        private void SavePlayer()
        {
            // Get the json string of the object.
            string jsonPlayer = JsonUtility.ToJson(_playerData);

            Debug.Log("<color=yellow>Sending following player to the cloud: \n</color>" + jsonPlayer);

            // Save the object on the cloud, in a table called like the object type.
            Drive.CreateObject(jsonPlayer, _tableName, true);
        }

        private void UpdatePlayer(bool create)
        {
            Debug.Log("<color=yellow>Updating cloud data: player called Mithrandir will be level 100.</color>");

            // Get the json string of the object.
            string jsonPlayer = JsonUtility.ToJson(_playerData);

            // Look in the 'PlayerInfo' table, for an object of name as specified, and overwrite with the current obj data.
            Drive.UpdateObjects(_tableName, "name", _playerData.name, jsonPlayer, create, true);
        }

        private void RetrievePlayer()
        {
            Debug.Log("<color=yellow>Retrieving player of name Mithrandir from the Cloud.</color>");

            // Get any objects from table 'PlayerInfo' with value 'Mithrandir' in the field called 'name'.
            Drive.GetObjectsByField(_tableName, "name", _playerData.name, true);
        }

        private void GetAllPlayers()
        {
            Debug.Log("<color=yellow>Retrieving all players from the Cloud.</color>");

            // Get all objects from table 'PlayerInfo'.
            Drive.GetTable(_tableName, true);
        }

        private void GetAllTables()
        {
            Debug.Log("<color=yellow>Retrieving all data tables from the Cloud.</color>");

            // Get all objects from table 'PlayerInfo'.
            Drive.GetAllTables(true);
        }

        // Processes the data received from the cloud.
        public void HandleDriveResponse(Drive.DataContainer dataContainer)
        {
            Debug.Log(dataContainer.msg);

            // First check the type of answer.
            if (dataContainer.QueryType == Drive.QueryType.getObjectsByField)
            {
                string rawJSon = dataContainer.payload;
                Debug.Log(rawJSon);

                // Check if the type is correct.
                if (string.Compare(dataContainer.objType, _tableName) == 0)
                {
                    // Parse from json to the desired object type.
                    PlayerInfo[] players = JsonHelper.ArrayFromJson<PlayerInfo>(rawJSon);

                    for (int i = 0; i < players.Length; i++)
                    {
                        _playerData = players[i];
                        Debug.Log("<color=yellow>Object retrieved from the cloud and parsed: \n</color>" +
                            "Name: " + _playerData.name + "\n" +
                            "Level: " + _playerData.level + "\n" +
                            "Health: " + _playerData.health + "\n" +
                            "Role: " + _playerData.role + "\n");
                    }
                }
            }

            // First check the type of answer.
            if (dataContainer.QueryType == Drive.QueryType.getTable)
            {
                string rawJSon = dataContainer.payload;
                Debug.Log(rawJSon);

                // Check if the type is correct.
                if (string.Compare(dataContainer.objType, _tableName) == 0)
                {
                    // Parse from json to the desired object type.
                    PlayerInfo[] players = JsonHelper.ArrayFromJson<PlayerInfo>(rawJSon);

                    string logMsg = "<color=yellow>" + players.Length.ToString() + " objects retrieved from the cloud and parsed:</color>";
                    for (int i = 0; i < players.Length; i++)
                    {
                        logMsg += "\n" +
                            "<color=blue>Name: " + players[i].name + "</color>\n" +
                            "Level: " + players[i].level + "\n" +
                            "Health: " + players[i].health + "\n" +
                            "Role: " + players[i].role + "\n";
                    }
                    Debug.Log(logMsg);
                }
            }

            // First check the type of answer.
            if (dataContainer.QueryType == Drive.QueryType.getAllTables)
            {
                string rawJSon = dataContainer.payload;

                // The response for this query is a json list of objects that hold tow fields:
                // * objType: the table name (we use for identifying the type).
                // * payload: the contents of the table in json format.
                Drive.DataContainer[] tables = JsonHelper.ArrayFromJson<Drive.DataContainer>(rawJSon);

                // Once we get the list of tables, we could use the objTypes to know the type and convert json to specific objects.
                // On this example, we will just dump all content to the console, sorted by table name.
                string logMsg = "<color=yellow>All data tables retrieved from the cloud.\n</color>";
                for (int i = 0; i < tables.Length; i++)
                {
                    logMsg += "\n<color=blue>Table Name: " + tables[i].objType + "</color>\n" + tables[i].payload + "\n";
                }
                Debug.Log(logMsg);
            }
        }

    }
}



