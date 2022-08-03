using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class LeaderBoardRow : MonoBehaviour
{
    [SerializeField] private TMP_Text playerName;
    [SerializeField] private TMP_Text playerScore;

    public void Setup(string name, string score)
    {
        playerName.text = name;
        playerScore.text = score;
    }
}
