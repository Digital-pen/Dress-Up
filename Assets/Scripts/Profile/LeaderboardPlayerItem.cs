using TMPro;
using Unity.Services.Leaderboards.Models;
using UnityEngine;

public class LeaderboardPlayerItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI playerScoreText;
    [SerializeField] private TextMeshProUGUI playerRankText;

    public void Initialize(LeaderboardEntry score)
    {
        playerNameText.text = score.PlayerName;
        playerScoreText.text = Mathf.RoundToInt((float)score.Score).ToString();
        playerRankText.text = score.Rank + 1.ToString();
    }

}
