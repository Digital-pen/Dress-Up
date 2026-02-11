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

    #region Loading
    [CloudCodeFunction("GetOrCreateProfileData")]
    public async Task<ProfileData> GetOrCreateProfileData(IExecutionContext context, IGameApiClient gameApiClient)
    {
        try
        {
            var result = await gameApiClient.CloudSaveData.GetItemsAsync(
                context,
                context.AccessToken!,
                context.ProjectId!,
                context.PlayerId!,
                new List<string> { "profileData" });

            var existing = result.Data.Results.FirstOrDefault()?.Value?.ToString();

            // ─── Player exists, return their data ─────────────────
            if (!string.IsNullOrEmpty(existing))
            {
                _logger.LogInformation("Existing player found: {PlayerId}", context.PlayerId);
                return System.Text.Json.JsonSerializer.Deserialize<ProfileData>(existing)!;
            }
        }
        catch (ApiException ex) when (ex.HResult == 404)
        {
            // No data found, fall through to create default
            _logger.LogInformation("No data found for player, creating default profile...");
        }
        catch (ApiException ex)
        {
            _logger.LogError("Error checking player data: {Error}", ex.Message);
            throw new Exception($"Unable to check player data: {ex.Message}");
        }

        // ─── New player, create default profile ───────────────────
        return await CreateDefaultProfile(context, gameApiClient);
    }

    private async Task<ProfileData> CreateDefaultProfile(IExecutionContext context, IGameApiClient gameApiClient)
    {
        ProfileData defaultProfile = new ProfileData
        {
            TotalScore = 0,
            MonthlyScore = 0,
            TotalBadgesEarned = 0,
            SavedStylesThisMonth = 0,
        };

        await SavePlayerData(context, gameApiClient, defaultProfile);

        _logger.LogInformation("Default profile created for new player: {PlayerId}", context.PlayerId);
        return defaultProfile;
    }
    #endregion

    #region saving
    [CloudCodeFunction("SaveProfileData")]
    public async Task SavePlayerData(IExecutionContext context, IGameApiClient gameApiClient, ProfileData data)
    {
        try
        {
            await gameApiClient.CloudSaveData.SetItemAsync(
                context,
                context.AccessToken!,
                context.ProjectId!,
                context.PlayerId!,
                new SetItemBody("profileData", data));

            _logger.LogInformation("Successfully saved data for profileData");
        }
        catch (ApiException ex)
        {
            _logger.LogError("Failed to save data for profileData. Error: {Error}", ex.Message);
            throw new Exception($"Unable to save player data: {ex.Message}");
        }
    }
    #endregion

    #region Deleting
    [CloudCodeFunction("DeletePlayerProfile")]
    public async Task<bool> DeletePlayerProfile(IExecutionContext context, IGameApiClient gameApiClient)
    {
        try
        {
            await gameApiClient.CloudSaveData.DeleteItemAsync(
                context,
                context.ServiceToken!,
                "profileData",
                context.ProjectId!,
                context.PlayerId!
                );

            _logger.LogInformation("Profile deleted for player: {PlayerId}", context.PlayerId);
            return true;
        }
        catch (ApiException ex)
        {
            _logger.LogError("Failed to delete profile for player {PlayerId}. Error: {Error}",
                context.PlayerId, ex.Message);
            throw new Exception($"Unable to delete player profile: {ex.Message}");
        }
    }
    #endregion
}

// ─── Models ───────────────────────────────────────────────────

public class ProfileData
{
    public int TotalScore { get; set; }
    public int MonthlyScore { get; set; }
    public int TotalBadgesEarned { get; set; }
    public int SavedStylesThisMonth { get; set; }
}


