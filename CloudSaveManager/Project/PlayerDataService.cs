using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Unity.Services.CloudCode.Apis;
using Unity.Services.CloudCode.Core;
using Unity.Services.CloudCode.Shared;
using Unity.Services.CloudSave.Model;
using System.Linq;

namespace CloudSaveManager;

public class PlayerDataService
{
    private readonly ILogger<PlayerDataService> _logger;

    public PlayerDataService(ILogger<PlayerDataService> logger)
    {
        _logger = logger;
    }

    private async Task SavePlayerData(IExecutionContext context, IGameApiClient gameApiClient, string key, string value)
    {
        try
        {
            await gameApiClient.CloudSaveData.SetItemAsync(
                context,
                context.AccessToken!,
                context.ProjectId!,
                context.PlayerId!,
                new SetItemBody(key, value));

            _logger.LogInformation("Successfully saved data for key: {Key}", key);
        }
        catch (ApiException ex)
        {
            _logger.LogError("Failed to save data for key {Key}. Error: {Error}", key, ex.Message);
            throw new Exception($"Unable to save player data: {ex.Message}");
        }
    }

    private async Task<string> GetPlayerData(IExecutionContext context, IGameApiClient gameApiClient, string key)
    {
        try
        {
            var result = await gameApiClient.CloudSaveData.GetItemsAsync(
                context,
                context.AccessToken!,
                context.ProjectId!,
                context.PlayerId!,
                new List<string> { key });

            var data = result.Data.Results.FirstOrDefault()?.Value?.ToString() ?? string.Empty;
            _logger.LogInformation("Successfully retrieved data for key: {Key}", key);
            return data;
        }
        catch (ApiException ex)
        {
            _logger.LogError("Failed to retrieve data for key {Key}. Error: {Error}", key, ex.Message);
            throw new Exception($"Unable to retrieve player data: {ex.Message}");
        }
    }
}


