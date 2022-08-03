using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class RankingManager : MonoBehaviour
{
    private string myName;
    public int myScore = 3;

    private string rankingTableLink = "http://skydomesoftware.usermd.net/HauntedMaze/HauntedMazeGetLeaderboard.php";
    private string rankingInsertRecordLink = "http://skydomesoftware.usermd.net/HauntedMaze/InsertScore.php";
    private string rankingUpdateRecordLink = "http://skydomesoftware.usermd.net/HauntedMaze/UpdateScore.php";
    public RootLeaderboardData rootObjectQuestsData;
    public List<LeaderboardData> sorted = new List<LeaderboardData>();

    public List<LeaderBoardRow> leaderBoardRows = new List<LeaderBoardRow>();

    private void Awake()
    {
        myName = PlayerPrefs.GetString("NickName");
    }
    public void CheckRanking()
    {
        StartCoroutine(DownloadRanking(output => { }));
    }


    public IEnumerator DownloadRanking(System.Action<string> onMessageReceived)
    {
        WWWForm form = new WWWForm();
        form.AddField("TableName", "HauntedMazeMuseum");
        UnityWebRequest www = UnityWebRequest.Post(rankingTableLink, form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            if (onMessageReceived != null)
            {
                onMessageReceived(www.downloadHandler.text);
                string message = www.downloadHandler.text;
                Debug.Log(message);
                rootObjectQuestsData = JsonUtility.FromJson<RootLeaderboardData>("{\"leaderboardData\":" + message + "}");
                Sort();
            }
        }
    }
    public void Sort()
    {
        sorted = rootObjectQuestsData.leaderboardData.ToList();
        sorted = sorted.OrderByDescending(go => go.Score).ToList();

        if (!CheckMyRecordExist())
        {
            StartCoroutine(CreateNewRecord(output => { }));
            return;
        }

        if (!CheckMyRecordIsBetter())
        {
            SetupRows();
            return;
        }
        StartCoroutine(UpdateRecord(output => { }));
    }

    public IEnumerator CreateNewRecord(System.Action<string> onMessageReceived)
    {
        WWWForm form = new WWWForm();
        form.AddField("TableName", "HauntedMazeMuseum");
        form.AddField("PlayerName", myName);
        form.AddField("Score", myScore);
        UnityWebRequest www = UnityWebRequest.Post(rankingInsertRecordLink, form);
        yield return www.SendWebRequest();
      
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            if (onMessageReceived != null)
            {
                onMessageReceived(www.downloadHandler.text);
                
            }
            Debug.Log("Create New Record");
            LeaderboardData leaderboardData = new LeaderboardData();
            leaderboardData.PlayerName = myName;
            leaderboardData.Score = myScore;
            sorted = rootObjectQuestsData.leaderboardData.ToList();
            sorted.Add(leaderboardData);
            sorted = sorted.OrderByDescending(go => go.Score).ToList();
            SetupRows();
        }
    }

    public IEnumerator UpdateRecord(System.Action<string> onMessageReceived)
    {
        Debug.Log("Update Record");
        WWWForm form = new WWWForm();
        form.AddField("TableName", "HauntedMazeMuseum");
        form.AddField("PlayerName", myName);
        form.AddField("Score", myScore);
        UnityWebRequest www = UnityWebRequest.Post(rankingUpdateRecordLink, form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            if (onMessageReceived != null)
            {
                onMessageReceived(www.downloadHandler.text);
                foreach (LeaderboardData item in sorted)
                {
                    if (item.PlayerName == myName)
                    {
                        item.Score = myScore;
                    }
                }
                SetupRows();
            }
        }
    }

    public bool CheckMyRecordExist()
    {
        foreach (LeaderboardData item in sorted)
        {
            if (item.PlayerName == myName)
            {
                Debug.Log("Player Exist");
                return true;
            }
        }
        return false;
    }
    public bool CheckMyRecordIsBetter()
    {
        foreach (LeaderboardData item in sorted)
        {
            if (item.PlayerName == myName)
            {
                if (item.Score < myScore)
                {
                    Debug.Log("Database record is smaller");
                    return true;
                }
            }
        }
        return false;
    }

    public void SetupRows()
    {
        for (int i = 0; i < leaderBoardRows.Count; i++)
        {
            leaderBoardRows[i].Setup(sorted[i].PlayerName, sorted[i].Score.ToString());
        }
    }

}
