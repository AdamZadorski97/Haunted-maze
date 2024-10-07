using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine.Networking;
using System;

namespace GoogleSheetsForUnity
{
    public static class Drive
    {
        /// <summary>
        /// List containing all query types.
        /// Needed if want to make custom query methods, or adding new queries on the webservice.
        /// </summary>
        public enum QueryType
        {
            // Create
            createObject,
            createObjects,
            createTable,

            // Retrieve
            getObjectsByField,
            getCellValue,
            getTable,
            getAllTables,

            // Update
            updateObjects,
            setCellValue,

            // Delete
            deleteObjects,
        }

        // This works as an auxiliary container for deserializing json data coming from Drive responses.
        // Once is on the structure, is expected that final objects will be instatiated and feed with the data for app use.
        [Serializable]
        public struct DataContainer
        {
            public string query;
            public string result;
            public string msg;
            public string payload;

            public string objType;
            public string column;
            public string row;
            public string searchField;
            public string searchValue;
            public string value;

            public QueryType QueryType { get { return (QueryType)Enum.Parse(typeof(QueryType), query); } }
        }

        private static ConnectionData _connectionData;
        private static string _currentStatus = "";

        public static bool debugMode = true;

        public static DriveConnection driveConnectorRuntime;
#if UNITY_EDITOR
        public static DriveConnectionEditor driveConnectionEditor;
#endif

        /// <summary>
        ///  Subscribe to this event to receive the response data from Google Drive.
        /// </summary>
        public static UnityAction<DataContainer> responseCallback;
        /// <summary>
        /// Subscribe to this event if you want to receive error callbacks.
        /// </summary>
        public static UnityAction<string> errorResponseCallback;


        #region Queries API

        /// <summary>
        /// Will create a new object (new row in a worksheet) with the specified json data. 
        /// Expects a json string with the object, as well as the type/table name. 
        /// If not all fields of the type are passed, those missing will be filled as "null".
        /// Fields passed but not in the table, will be ignored.
        /// </summary>
        /// <param name="jsonObject">Json string with the object to be persisted.</param>
        /// <param name="objTypeName">Name of the table that will hold the object.</param>
        /// <param name="runtime">Bool value indicating if the request was sent from Unity Editor or running game.</param>
        public static void CreateObject(string jsonObject, string objTypeName, bool runtime = true)
        {
            Dictionary<string, string> form = new Dictionary<string, string>();
            form.Add("action", QueryType.createObject.ToString());
            form.Add("type", objTypeName);
            form.Add("jsonData", jsonObject);

            CreateRequest(form, runtime);
        }

        /// <summary>
        /// Will create a new object (new row in a worksheet) with the specified field values. 
        /// Expects a dictionary of field names and values to be stored, as well as the type/table name. 
        /// If not all type fields available in the table are passed, those missing will be filled as "null".
        /// Fields passed but not in the table, will be ignored.
        /// </summary>
        /// <param name="fields">Dictionary of field names and values to be stored.</param>
        /// <param name="objTypeName">Name of the table that will hold the object.</param>
        /// <param name="runtime">Bool value indicating if the request was sent from Unity Editor or running game.</param>
        public static void CreateObject(Dictionary<string, string> fields, string objTypeName, bool runtime = true)
        {
            Dictionary<string, string> form = fields;
            form.Add("action", QueryType.createObject.ToString());
            form.Add("type", objTypeName);
            form.Add("isJson", "False");
            CreateRequest(form, runtime);
        }

        /// <summary>
        /// Will create a number of objects (rows in a worksheet) from the specified json data. 
        /// Expects a json string with the array of objects, as well as the type/table name. 
        /// If not all type fields available in the table are passed, those missing will be filled as "null".
        /// Fields passed but not in the table, will be ignored.
        /// </summary>
        /// <param name="jsonObjects">Json string with the object to be persisted.</param>
        /// <param name="objTypeName">Name of the table that will hold the object.</param>
        /// <param name="runtime">Bool value indicating if the request was sent from Unity Editor or running game.</param>
        public static void CreateObjects(string jsonObjects, string objTypeName, bool runtime = true)
        {
            Dictionary<string, string> form = new Dictionary<string, string>();
            form.Add("action", QueryType.createObjects.ToString());
            form.Add("type", objTypeName);
            form.Add("jsonData", jsonObjects);

            CreateRequest(form, runtime);
        }

        /// <summary>
        /// Will create a new table (new worksheet on the spreadsheet) with the specified name and headers. 
        /// Expects an array of field names/headers, as well as the type/table name to be used. 
        /// </summary>
        /// <param name="fields">String array with the names of the table headers.</param>
        /// <param name="tableTypeName">The name of the table to be created.</param>
        /// <param name="runtime">Bool value indicating if the request was sent from Unity Editor or running game.</param>
        public static void CreateTable(string[] headers, string tableTypeName, bool runtime = true)
        {
            Dictionary<string, string> form = new Dictionary<string, string>();
            form.Add("action", QueryType.createTable.ToString());
            form.Add("type", tableTypeName);
            form.Add("num", headers.Length.ToString());

            for (int i = 0; i < headers.Length; i++)
            {
                form.Add("field" + i.ToString(), headers[i]);
            }

            CreateRequest(form, runtime);
        }

        /// <summary>
        /// Retrieves from the spreadsheet an array of objects found by searching with the specified criteria. 
        /// Expects the table name, the name of the field to search by, and the value to search. 
        /// </summary>
        /// <param name="objTypeName">Name of the table to search.</param>
        /// <param name="searchFieldName">Name of the field to search by.</param>
        /// <param name="searchValue">Value to search for.</param>
        /// <param name="runtime">Bool value indicating if the request was sent from Unity Editor or running game.</param>
        public static void GetObjectsByField(string objTypeName, string searchFieldName, string searchValue, bool runtime = true)
        {
            Dictionary<string, string> form = new Dictionary<string, string>();
            form.Add("action", QueryType.getObjectsByField.ToString());
            form.Add("type", objTypeName);
            form.Add(searchFieldName, searchValue);
            form.Add("search", searchFieldName);

            CreateRequest(form, runtime);
        }

        /// <summary>
        /// Retrieves from the spreadsheet an object by looking into a specified cell. 
        /// Expects the table name, the column and row. 
        /// </summary>
        /// <param name="objTypeName">Name of the table to search.</param>
        /// <param name="column">Cell column.</param>
        /// <param name="row">Cell row.</param>
        /// <param name="runtime">Bool value indicating if the request was sent from Unity Editor or running game.</param>
        public static void GetCellValue(string objTypeName, string column, string row, bool runtime = true)
        {
            Dictionary<string, string> form = new Dictionary<string, string>();
            form.Add("action", QueryType.getCellValue.ToString());
            form.Add("type", objTypeName);
            form.Add("column", column);
            form.Add("row", row);

            CreateRequest(form, runtime);
        }

        /// <summary>
        /// Retrieves from the spreadsheet an array of all the objects found in the specified table. 
        /// Expects the table name. 
        /// </summary>
        /// <param name="tableTypeName">The name of the table to be retrieved.</param>
        /// <param name="runtime">Bool value indicating if the request was sent from Unity Editor or running game.</param>
        public static void GetTable(string tableTypeName, bool runtime = true)
        {
            Dictionary<string, string> form = new Dictionary<string, string>();
            form.Add("action", QueryType.getTable.ToString());
            form.Add("type", tableTypeName);

            CreateRequest(form, runtime);
        }

        /// <summary>
        /// Retrieves from the spreadsheet the data from all tables, in the form of one or more array of objects. 
        /// </summary>
        /// <param name="runtime">Bool value indicating if the request was sent from Unity Editor or running game.</param>
        public static void GetAllTables(bool runtime = true)
        {
            Dictionary<string, string> form = new Dictionary<string, string>();
            form.Add("action", QueryType.getAllTables.ToString());

            CreateRequest(form, runtime);
        }

        /// <summary>
        /// Updates one or more objects found by searching with the specified criteria. 
        /// Expects the table name, the name of the field to search by, and the value to search, 
        /// as well as the object in json format to be updated in the matching objects.
        /// The json must contain only those fields that one to be updated.
        /// </summary>
        /// <param name="objTypeName">Name of the table to search.</param>
        /// <param name="searchFieldName">Name of the field to search by.</param>
        /// <param name="searchValue">Value to search for.</param>
        /// <param name="jsonObject">Json string with the object to be updated.</param>
        /// <param name="create">Indicates whether a new object should be created with the json data provided, in the case that no objects are found matching the search criteria.</param>
        /// <param name="runtime">Bool value indicating if the request was sent from Unity Editor or running game.</param>
        public static void UpdateObjects(string objTypeName, string searchFieldName, string searchValue, string jsonObject, bool create, bool runtime = true)
        {
            Dictionary<string, string> form = new Dictionary<string, string>();
            form.Add("action", QueryType.updateObjects.ToString());
            form.Add("type", objTypeName);
            form.Add("searchField", searchFieldName);
            form.Add("searchValue", searchValue);
            form.Add("jsonData", jsonObject);
            form.Add("create", create.ToString());

            CreateRequest(form, runtime);
        }
        
        /// <summary>
        /// Updates a cell value specified by column and row coordinates.
        /// Expects the table name, the column and row coordinates, and the new value to be set.
        /// * Setting values by table cell coordinates is unsafe and error prone, to be used only on very specific scenarios. *
        /// </summary>
        /// <param name="objTypeName">Name of the table to search.</param>
        /// <param name="column">Cell column.</param>
        /// <param name="row">Cell row.</param>
        /// <param name="value">New value to be set.</param>
        /// <param name="runtime">Bool value indicating if the request was sent from Unity Editor or running game.</param>
        public static void SetCellValue(string objTypeName, string column, string row, string value, bool runtime = true)
        {
            Dictionary<string, string> form = new Dictionary<string, string>();
            form.Add("action", QueryType.setCellValue.ToString());
            form.Add("type", objTypeName);
            form.Add("column", column);
            form.Add("row", row);
            form.Add("value", value);

            CreateRequest(form, runtime);
        }

        /// <summary>
        /// Deletes one or more objects (rows) from a table (worksheet) found by searching with the specified criteria. 
        /// Expects the table name, the name of the field to search by, and the value to search. 
        /// </summary>
        /// <param name="objTypeName">Name of the table to search.</param>
        /// <param name="searchFieldName">Name of the field to search by.</param>
        /// <param name="searchValue">Value to search for.</param>
        /// <param name="runtime">Bool value indicating if the request was sent from Unity Editor or running game.</param>
        public static void DeleteObjects(string objTypeName, string searchFieldName, string searchValue, bool runtime = true)
        {
            Dictionary<string, string> form = new Dictionary<string, string>();
            form.Add("action", QueryType.deleteObjects.ToString());
            form.Add("type", objTypeName);
            form.Add(searchFieldName, searchValue);
            form.Add("search", searchFieldName);

            CreateRequest(form, runtime);
        }

        #endregion

        #region Connection Handling

        private static void SetConnectionData(bool runtime)
        {
            if (_connectionData == null)
            {
                if (runtime)
                {
                    if (driveConnectorRuntime == null)
                        driveConnectorRuntime = GameObject.FindObjectOfType<DriveConnection>();

                    if (driveConnectorRuntime == null)
                        UpdateStatus("Cannot find CloudConnector script on scene, cannot execute the request.");
                    
                    _connectionData = driveConnectorRuntime.connectionData;
                }
#if UNITY_EDITOR
                else
                {
                    if (driveConnectionEditor == null)
                        driveConnectionEditor = UnityEditor.Editor.CreateInstance<DriveConnectionEditor>();

                    if (driveConnectionEditor == null)
                        UpdateStatus("Cannot find CloudConnector script on project, cannot execute the request.");

                    _connectionData = driveConnectionEditor.connectionData;
                }
#endif
            }

            if (_connectionData.webServiceUrl == "")
            {
                UpdateStatus("Connection data does not specified web service address.");
                _connectionData = null;
            }

            if (_connectionData == null)
                UpdateStatus("Error, connection data not found.");
        }

        private static Dictionary<string, string> CompleteForm(Dictionary<string, string> form)
        {
            form.Add("ssid", _connectionData.spreadsheetId);
            form.Add("pass", _connectionData.servicePassword);

            return form;
        }

        private static void CreateRequest(Dictionary<string, string> dataForm, bool runtime) 
        {
            SetConnectionData(runtime);
            if (_connectionData == null)
                return;

            var form = CompleteForm(dataForm);
            if (form == null)
                return;

            UnityWebRequest www;

            if (_connectionData.usePOST)
            {
                UpdateStatus("Establishing Connection at URL ", _connectionData.webServiceUrl);
                www = UnityWebRequest.Post(_connectionData.webServiceUrl, form);
            }
            else
            {
                string urlParams = "?";
                foreach (KeyValuePair<string, string> item in form)
                {
                    urlParams += item.Key + "=" + item.Value + "&";
                }
                UpdateStatus("Establishing Connection at URL ", _connectionData.webServiceUrl, urlParams);
                www = UnityWebRequest.Get(_connectionData.webServiceUrl + urlParams);
            }

            if (runtime)
            {
                driveConnectorRuntime.ExecuteRequest(www, form);
            }
#if UNITY_EDITOR
            else
            {
                if (driveConnectionEditor == null)
                    driveConnectionEditor = UnityEditor.Editor.CreateInstance<DriveConnectionEditor>();

                driveConnectionEditor.ExecuteRequest(www, form);
            }
#endif
        }

        #endregion

        #region Response Handling
        
        // This method is called from the connection handlers (DriveConnection or DriveConnectionEditor).
        public static void ProcessResponse(string response, float time)
        {
            DataContainer dataContainer;
            try
            {
                dataContainer = JsonUtility.FromJson<DataContainer>(response);
            }
            catch (Exception)
            {
                HandleError("Undefined server response: \n" + response, time);
                return;
            }

            if (dataContainer.result == "ERROR")
            {
                HandleError(dataContainer.msg, time);
                return;
            }

            if (string.IsNullOrEmpty(dataContainer.result) || dataContainer.result != "ERROR" && dataContainer.result != "OK")
            {
                HandleError("Undefined server response: \n" + response, time);
                return;
            }

            if (responseCallback != null)
                responseCallback(dataContainer);
        }

        public static void HandleError(string response, float time)
        {
            UpdateStatus(response);

            if (errorResponseCallback != null)
                errorResponseCallback(response);
        }

        #endregion

        public static void UpdateStatus(params string[] statusDetails)
        {            
            _currentStatus = string.Concat(statusDetails);
            if (debugMode)
            {
                //Debug.Log(_currentStatus);
            }
             
        }
    }

    // Helper class: because UnityEngine.JsonUtility does not support deserializing an array...
    // http://forum.unity3d.com/threads/how-to-load-an-array-with-jsonutility.375735/
    public class JsonHelper
    {
        public static T[] ArrayFromJson<T>(string json)
        {
            string newJson = "{ \"array\": " + json + "}";
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
            return wrapper.array;
        }

        public static string ToJson<T>(T[] array, bool prettyPrint = false)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.array = array;
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }

        [Serializable]
        private class Wrapper<T>
        {
            public T[] array;
        }
    }
}
