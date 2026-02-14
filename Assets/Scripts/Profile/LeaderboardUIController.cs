using System.Threading.Tasks;
using Newtonsoft.Json;
using TMPro;
using Unity.Services.Leaderboards;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardUIController : MonoBehaviour
{
    public const string leaderboardID = "Monthly_Score";

    [Header("References")]
    [SerializeField] private GameObject playerLeaderboardEntryPrefab;
    [SerializeField] private RectTransform containerRectTransform;
    [SerializeField] private int playersPerPage = 10;
    [SerializeField] private TextMeshProUGUI pageText;
    [SerializeField] private Button nextPageButton;
    [SerializeField] private Button previousPageButton;

    [SerializeField] private GameObject leaderboardPanel = null;


    private int currentPage = 1;
    private int totalPages = 1;

    public void Initialize()
    {
        pageText.text = "-";
        ClearPlayersList();
        currentPage = 1;
        totalPages = 1;
        LoadPlayers(currentPage);
    }

    public async Task AddScoreAsync(string leaderboardId, int score)
    {
        var playerEntry = await LeaderboardsService.Instance
            .AddPlayerScoreAsync(leaderboardId, score);
        Debug.Log(JsonConvert.SerializeObject(playerEntry));
    }

    private async void LoadPlayers(int page)
    {
        nextPageButton.interactable = false;
        previousPageButton.interactable = false;

        try
        {
            GetScoresOptions options = new GetScoresOptions
            {
                Offset = (page - 1) * playersPerPage,
                Limit = playersPerPage
            };

            var scores = await LeaderboardsService.Instance.GetScoresAsync(leaderboardID, options);
            ClearPlayersList();
            foreach (var score in scores.Results)
            {
                LeaderboardPlayerItem entry = Instantiate(playerLeaderboardEntryPrefab, containerRectTransform).GetComponent<LeaderboardPlayerItem>();
                entry.Initialize(score);
            }
            totalPages = Mathf.CeilToInt((float)scores.Total / scores.Limit);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error loading players: " + ex.Message);
        }

        pageText.text = currentPage + " / " + totalPages;
        nextPageButton.interactable = currentPage < totalPages && totalPages > 1;
        previousPageButton.interactable = currentPage > 1 && totalPages > 1;
    }

    public void NextPage()
    {
        if (currentPage + 1 > totalPages)
        {
            LoadPlayers(currentPage);
        }
        else
        {
            LoadPlayers(currentPage + 1);
        }
    }

    public void PreviousPage()
    {
        if (currentPage - 1 <= 0)
        {
            LoadPlayers(currentPage);
        }
        else
        {
            LoadPlayers(currentPage - 1);
        }
    }

    private void ClearPlayersList()
    {
        LeaderboardPlayerItem[] items = containerRectTransform.GetComponentsInChildren<LeaderboardPlayerItem>();

        foreach (LeaderboardPlayerItem item in items)
        {
            Destroy(item.gameObject);
        }
    }

    public void ToggleLeaderboardPanel()
    {
        leaderboardPanel.SetActive(!leaderboardPanel.activeSelf);
    }
}
