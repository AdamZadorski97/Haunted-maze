#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.Networking;

namespace GoogleSheetsForUnity
{
    public class DriveConnectionEditor : Editor
    {
        public ConnectionData connectionData;

        private UnityWebRequest _www;
        private double _elapsedTime = 0.0f;
        private double _startTime = 0.0f;

        public void ExecuteRequest(UnityWebRequest www, Dictionary<string, string> form)
        {
            EditorApplication.update += EditorUpdate;
            _startTime = EditorApplication.timeSinceStartup;
            _www = www;
            _www.SendWebRequest();
        }

        private void EditorUpdate()
        {
            while (!_www.isDone)
            {
                _elapsedTime = EditorApplication.timeSinceStartup - _startTime;
                if (_elapsedTime >= connectionData.timeOutLimit)
                {
                    Drive.ProcessResponse("TIME_OUT", (float)_elapsedTime);
                    EditorApplication.update -= EditorUpdate;
                }
                return;
            }

            if (_www.isNetworkError)
            {
                Drive.ProcessResponse("Connection error after " + _elapsedTime.ToString() + " seconds: " + _www.error, (float)_elapsedTime);
                return;
            }

            Drive.ProcessResponse(_www.downloadHandler.text, (float)_elapsedTime);

            EditorApplication.update -= EditorUpdate;
        }
    }
}
#endif