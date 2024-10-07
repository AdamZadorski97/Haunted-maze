using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace GoogleSheetsForUnity
{
    public class DriveConnection : MonoBehaviour
    {
        public ConnectionData connectionData;
        public bool use;

        public void ExecuteRequest(UnityWebRequest www, Dictionary<string, string> postData)
        {
         if (use)
            StartCoroutine(CoExecuteRequest(www, postData));
        }

        private IEnumerator CoExecuteRequest(UnityWebRequest www, Dictionary<string, string> postData)
        {
            www.SendWebRequest();

            float elapsedTime = 0.0f;

            while (!www.isDone)
            {
                elapsedTime += Time.deltaTime;
                if (elapsedTime >= connectionData.timeOutLimit)
                {
                    Drive.HandleError("Operation timed out, connection aborted. Check your internet connection and try again.", elapsedTime);
                    yield break;
                }

                yield return null;
            }

            if (www.isNetworkError)
            {
                Drive.HandleError("Connection error after " + elapsedTime.ToString() + " seconds: " + www.error, elapsedTime);
                yield break;
            }

            Drive.ProcessResponse(www.downloadHandler.text, elapsedTime);
        }

    }
}
	