using UnityEditor;
using UnityEngine;

namespace GoogleSheetsForUnity
{

    /// <summary>
    /// We use ScriptableObjects for connection data, so the app can have multiple connection presets, that can be interchangeable.
    /// </summary>
    [CreateAssetMenu(fileName = "ConnectionData", menuName = "Google Sheets For Unity/Connection Data Asset", order = 0)]
    public class ConnectionData : ScriptableObject
    {
        [Tooltip("URL of the webapp deployed on Google Drive.")]
        public string webServiceUrl = "";
        [Tooltip("The Id of the spreadsheet to be used for the objects & tables operations. If more than one spreadsheet are required, different connections can be made, or the spreadsheet Ids manually stated on the requests forms.")]
        public string spreadsheetId = "";
        [Tooltip("The password to use on the individual queries. Will not be used if the connection is set to useSessionContext.")]
        public string servicePassword = "";
        [Tooltip("The time in seconds before declaring the connection as timed out.")]
        public float timeOutLimit = 15f;
        [Tooltip("WWW Request type: true will use POST and false will use GET.")]
        public bool usePOST = true;
    }
}
