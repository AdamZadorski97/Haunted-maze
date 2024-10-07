using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace GoogleSheetsForUnity
{
    [DisallowMultipleComponent]
    public class ConsoleExample : MonoBehaviour
    {
        public struct LogMsg
        {
            public string logString;
            public string stackTrace;
            public string logType;
            public string logNumber;
            public string sessionId;
        }

        public const string TABLE_NAME = "ConsoleLog";
        public enum TABLE_HEADERS { logNumber, logType, logString, stackTrace, sessionId }
        
        private List<LogMsg> _logMsgQueue;
        private bool _processing;
        private float _interval;
        private int _logCounter;

        private static string sessionUuid;


        private void OnEnable()
        {
            // We dont want the asset to write logs on Unity Console, that would create a loop.
            Drive.debugMode = false;

            Application.logMessageReceivedThreaded += AddToQueue;            
        }

        private void OnDisable()
        {
            Drive.debugMode = true;

            Application.logMessageReceivedThreaded -= AddToQueue;
        }

        private void Start()
        {
            _logMsgQueue = new List<LogMsg>();
            _processing = false;
            _interval = 0.05f;
            _logCounter = 1;

            if (string.IsNullOrEmpty(sessionUuid))
                sessionUuid = System.Guid.NewGuid().ToString().Substring(0, 8); // Let's use a cutdown version of the uuid
        }

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 10, 600, 1000));
            GUILayout.BeginHorizontal();
            GUILayout.Space(10f);
            GUILayout.BeginVertical();

            GUILayout.Label("This example will send all Unity Console Output to a worksheet on the spreadsheet defined at the DriveConnection prefab." +
                "The Logs are not expected to arrive in order, but they are sent with a number so the worksheet can be sorted by that index to read them properly." +
                "However, have in mind that the number resets each time the scene is played." +
                "\n\nNote that Google Sheets For Unity operations are not reported on Unity console on this example, to prevent a loop.", GUILayout.MaxWidth(600f));
            
            if (GUILayout.Button("1) Create Log Table", GUILayout.MinHeight(20f), GUILayout.MaxWidth(200f)))
            {
                CreateConsoleLogTable();
            }

            if (GUILayout.Button("2) Send Test Logs", GUILayout.MinHeight(20f), GUILayout.MaxWidth(200f)))
            {
                TestConsoleMessages();
            }

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        public void CreateConsoleLogTable()
        {
            var logFields = Enum.GetNames(typeof(TABLE_HEADERS));
            Drive.CreateTable(logFields, TABLE_NAME);
        }

        public void TestConsoleMessages()
        {
            Debug.LogFormat("------------ Sending Tests Logs. Session uid: {0}. Date: {1}. ------------", sessionUuid, DateTime.Now);
            Debug.Log("Log Test");
            Debug.LogAssertion("Assertion Test");
            Debug.LogError("Error Test");
            Debug.LogWarning("Warning Test");
            print("Print Test");
            try
            {
                int byZero = 0;
                int a = 4 / byZero;
                Debug.Log(a);
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
            }
            Debug.Log("------------ Tests Logs Finished ------------");
        }

        // Avoid flooding the web service. Sets a small pause before 
        // firing next call (See 'internal' field member).
        private void AddToQueue(string logString, string stackTrace, LogType logType)
        {
            LogMsg msg = new LogMsg
            {
                logString = logString,
                stackTrace = stackTrace,
                logType = logType.ToString(),
                logNumber = _logCounter.ToString(),
                sessionId = sessionUuid
            };

            _logCounter++;
            _logMsgQueue.Add(msg);

            if (!_processing)
                StartCoroutine(ProcessQueue());
        }

        private IEnumerator ProcessQueue()
        {
            _processing = true;

            while (_logMsgQueue.Count > 0)
            {
                string jsonLog = JsonUtility.ToJson(_logMsgQueue[0]);
                Drive.CreateObject(jsonLog, TABLE_NAME);
                
                _logMsgQueue.RemoveAt(0);
                yield return new WaitForSeconds(_interval);
            }

            _processing = false;
        }
    }
}
