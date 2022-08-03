using System;

[Serializable]
public class LeaderboardData
{
    public int id;
    public string PlayerName;
    public int Score;
}

[Serializable]
public class RootLeaderboardData
{
    public LeaderboardData[] leaderboardData;
}